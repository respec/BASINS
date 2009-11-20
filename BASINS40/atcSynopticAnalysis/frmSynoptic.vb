Imports atcControls
Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmSynoptic
    'The group of atcTimeseries displayed
    Private WithEvents pDataGroup As atcTimeseriesGroup

    Private WithEvents pEvents As atcTimeseriesGroup

    Private pInitialized As Boolean = False
    Private pLastThreshold As Double = GetNaN()
    Private pLastDaysGapAllowed As Double = GetNaN()
    Private pLastHighEvents As Boolean = True

    Private pSwapperSource As atcControls.atcGridSourceRowColumnSwapper

    Private pGroupByNames() As String = {"Each Event", "Number of Measurements", "Maximum Intensity", "Mean Intensity", "Total Volume", "Month", "Year", "One Group"} ', "Season", "Year", "Length", }

    Private pColumnTitles() As String
    Private pColumnAttributes() As String

    Private pColumnTitlesAll() As String   'Column titles to use for all but "Each Event" grouping
    Private pColumnTitlesEvent() As String 'Column titles in use for "Each Event" grouping

    'Column Attributes (Min, Max, Mean, SD) to use for all but "Each Event" grouping
    Private pColumnAttributesAll() As String
    'Column Attributes (Min, Max, Mean, SD) to use for "Each Event" grouping
    Private pColumnAttributesEvent() As String

    'Private pColumnTitlesAllAvailable() As String = {"Group", "Events", "Measurements", "Volume", "Duration", "Intensity", "Time Since Last"}
    Private pColumnTitlesAllDefault() As String = {"Group", "Events", "Measurements", "Volume", "Volume", "Volume", "Volume", "Duration", "Duration", "Duration", "Duration", "Intensity", "Intensity", "Intensity", "Time Since Last"}
    Private pColumnAttributesAllDefault() As String = {"", "", "", "Max", "Mean", "Sum", "Cumulative", "Max", "Mean", "Sum", "Standard Deviation", "Max", "Mean", "Standard Deviation", "Mean"}

    'Private pColumnTitlesEventAvailable() As String = {"Group", "Start Date", "Start Time", "Measurements", "Volume", "Duration", "Intensity", "Time Since Last"}
    Private pColumnTitlesEventDefault() As String = {"Group", "Location", "Start Date", "Start Time", "Measurements", "Volume", "Duration", "Intensity", "Intensity", "Time Since Last"}
    Private pColumnAttributesEventDefault() As String = {"", "", "", "", "", "Sum", "Sum", "Max", "Mean", "Mean"}

    Public Sub Initialize(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing)
        If aTimeseriesGroup Is Nothing Then
            pDataGroup = New atcTimeseriesGroup
        Else
            pDataGroup = aTimeseriesGroup
        End If

        Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
        For Each lDisp As atcDataDisplay In DisplayPlugins
            Dim lMenuText As String = lDisp.Name
            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
            mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
        Next

        If pDataGroup.Count = 0 Then 'ask user to specify some timeseries
            atcDataManager.UserSelectData("Select Data for Synoptic Analysis", pDataGroup)
        End If

        If pDataGroup.Count > 0 Then
            If pDataGroup.Count = 1 Then
                Me.Text &= " of " & pDataGroup.ItemByIndex(0).ToString
            Else
                Me.Text &= " of " & pDataGroup.Count & " datasets"
            End If
            Me.Show()
            cboGapUnits.Items.AddRange(atcSynopticAnalysisPlugin.TimeUnitNames)
            cboGroupBy.Items.AddRange(pGroupByNames)

            Try
                LoadColumnTitles()

                cboGapUnits.SelectedIndex = GetSetting("Synoptic", "Defaults", "GapUnits", 3)
                cboGroupBy.SelectedIndex = GetSetting("Synoptic", "Defaults", "GroupBy", 0)
                txtThreshold.Text = GetSetting("Synoptic", "Defaults", "Threshold", txtThreshold.Text)
                If GetSetting("Synoptic", "Defaults", "High", 0) Then
                    cboAboveBelow.SelectedIndex = 0
                Else
                    cboAboveBelow.SelectedIndex = 1
                End If
                txtGap.Text = GetSetting("Synoptic", "Defaults", "GapNumber", txtGap.Text)
                If GetSetting("Synoptic", "Defaults", "ReverseGroupOrder", "False") = "True" Then
                    mnuReverseGroupOrder.Checked = True
                End If
            Catch 'Restoring setting is not critical, ignore errors
            End Try
            SetColumnTitlesFromGroupBy()
            pInitialized = True
            PopulateGrid()
        Else 'user declined to specify timeseries
            Me.Close()
        End If
    End Sub

    Private Sub LoadColumnTitles()
        pColumnTitlesAll = LoadStringArray("ColumnTitles", pColumnTitlesAllDefault)
        pColumnAttributesAll = LoadStringArray("ColumnAttributes", pColumnAttributesAllDefault)

        pColumnTitlesEvent = LoadStringArray("ColumnTitlesEvent", pColumnTitlesEventDefault)
        pColumnAttributesEvent = LoadStringArray("ColumnAttributesEvent", pColumnAttributesEventDefault)

        'Sanity check: attributes and titles have to be the same length, set to defaults if not
        If pColumnAttributesAll.Length <> pColumnTitlesAll.Length Then
            pColumnTitlesAll = pColumnTitlesAllDefault.Clone
            pColumnAttributesAll = pColumnAttributesAllDefault.Clone
        End If
        If pColumnAttributesEvent.Length <> pColumnTitlesEvent.Length OrElse Array.IndexOf(pColumnTitlesEvent, "Location") = -1 Then
            pColumnTitlesEvent = pColumnTitlesEventDefault.Clone
            pColumnAttributesEvent = pColumnAttributesEventDefault.Clone
        End If
    End Sub

    Private Function LoadStringArray(ByVal aKey As String, ByVal aDefault() As String) As String()
        Dim lTitleString As String = GetSetting("Synoptic", "Defaults", aKey)
        If lTitleString.Length > 0 Then
            Return lTitleString.Split(",")
        Else
            Return aDefault.Clone
        End If
    End Function

    Private Sub SaveSettings()
        If pInitialized Then
            Try
                SaveSetting("Synoptic", "Defaults", "GapUnits", cboGapUnits.SelectedIndex)
                SaveSetting("Synoptic", "Defaults", "GroupBy", cboGroupBy.SelectedIndex)
                SaveSetting("Synoptic", "Defaults", "Threshold", txtThreshold.Text)
                SaveSetting("Synoptic", "Defaults", "High", (cboAboveBelow.SelectedIndex = 0))
                SaveSetting("Synoptic", "Defaults", "GapNumber", txtGap.Text)
                SaveSetting("Synoptic", "Defaults", "ColumnTitles", String.Join(",", pColumnTitlesAll))
                SaveSetting("Synoptic", "Defaults", "ColumnAttributes", String.Join(",", pColumnAttributesAll))
                SaveSetting("Synoptic", "Defaults", "ColumnTitlesEvent", String.Join(",", pColumnTitlesEvent))
                SaveSetting("Synoptic", "Defaults", "ColumnAttributesEvent", String.Join(",", pColumnAttributesEvent))
                If mnuReverseGroupOrder.Checked Then
                    SaveSetting("Synoptic", "Defaults", "ReverseGroupOrder", "True")
                Else
                    Try
                        DeleteSetting("Synoptic", "Defaults", "ReverseGroupOrder")
                    Catch 'ignore error if setting is already not there
                    End Try
                End If
            Catch 'Saving setting is not critical, ignore errors
            End Try
        End If
    End Sub

    Private Sub PopulateGrid()
        If Not pInitialized Then Exit Sub 'too early to do this!
        If pEvents Is Nothing Then
            ComputeEventsFromFormParameters(False)
        End If

        Dim lGroups As atcCollection = atcSynopticAnalysisPlugin.ComputeGroups(cboGroupBy.Text, pEvents)

        If mnuReverseGroupOrder.Checked Then
            lGroups.Reverse()
        End If

        Dim lSource As atcGridSource = atcSynopticAnalysisPlugin.PopulateGrid(lGroups, cboGapUnits.Text, pColumnTitles, pColumnAttributes)

        pSwapperSource = New atcControls.atcGridSourceRowColumnSwapper(lSource)
        pSwapperSource.SwapRowsColumns = mnuAttributeColumns.Checked
        agdMain.Initialize(pSwapperSource)
        agdMain.SizeAllColumnsToContents()
        agdMain.Refresh()
    End Sub

    Private Function GetIndex(ByVal aName As String) As Integer
        Return CInt(Mid(aName, InStr(aName, "#") + 1))
    End Function

    Private Sub mnuAnalysis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAnalysis.Click
        atcDataManager.ShowDisplay(sender.Text, pDataGroup)
    End Sub

    Private Sub pDataGroup_Added(ByVal aAdded As atcCollection) Handles pDataGroup.Added
        If pInitialized Then PopulateGrid()
    End Sub

    Private Sub pDataGroup_Removed(ByVal aRemoved As atcCollection) Handles pDataGroup.Removed
        If pInitialized Then PopulateGrid()
    End Sub

    Protected Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
        pDataGroup = Nothing
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

    'Private Sub mnuFileSelectAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectAttributes.Click
    '    Dim lst As New atcControls.atcSelectList
    '    Dim lAvailable As New ArrayList
    '    For Each lAttrDef As atcAttributeDefinition In atcDataAttributes.AllDefinitions
    '        Select Case lAttrDef.TypeString.ToLower
    '            Case "double", "integer", "boolean", "string"
    '                lAvailable.Add(lAttrDef.Name)
    '        End Select
    '    Next
    '    lAvailable.Sort()
    '    If lst.AskUser(lAvailable, atcDataManager.DisplayAttributes) Then
    '        PopulateGrid()
    '    End If
    'End Sub

    Private Sub mnuFileSelectData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSelectData.Click
        atcDataManager.UserSelectData(, pDataGroup, , False)
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
        Dim lDaysGapAllowed As Double = Double.Parse(txtGap.Text) * atcSynopticAnalysisPlugin.TimeUnitFactor(cboGapUnits.SelectedIndex)
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

    Public Property GroupBy() As String
        Get
            Return cboGroupBy.Text
        End Get
        Set(ByVal newValue As String)
            Dim lNewIndex As Integer = -1
            If IsNumeric(newValue) Then
                lNewIndex = CInt(newValue)
                If lNewIndex < 0 OrElse lNewIndex >= cboGroupBy.Items.Count Then
                    lNewIndex = -1
                End If
            Else
                Dim lCurrentIndex As Integer = 0
                For Each lItem As String In cboGroupBy.Items
                    If lItem = newValue Then lNewIndex = lCurrentIndex
                    lCurrentIndex += 1
                Next
            End If

            If lNewIndex >= 0 Then
                cboGroupBy.SelectedIndex = lNewIndex
            Else
                Logger.Msg("Cannot group by " & newValue, "Synoptic Grouping")
            End If
        End Set
    End Property

    Private Sub cboGroupBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboGroupBy.SelectedIndexChanged
        SetColumnTitlesFromGroupBy()
        SaveSettings()
        PopulateGrid()
    End Sub

    Private Sub SetColumnTitlesFromGroupBy()
        If cboGroupBy.Text = "Each Event" Then
            pColumnTitles = pColumnTitlesEvent.Clone
            pColumnAttributes = pColumnAttributesEvent.Clone
        Else
            pColumnTitles = pColumnTitlesAll.Clone
            pColumnAttributes = pColumnAttributesAll.Clone
        End If
    End Sub

    Private Sub mnuChooseColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuChooseColumns.Click
        Dim lFrmChoose As New frmChooseColumns

        If cboGroupBy.Text = "Each Event" Then
            lFrmChoose.AskUser(pColumnTitlesEvent, pColumnAttributesEvent)
        Else
            lFrmChoose.AskUser(pColumnTitlesAll, pColumnAttributesAll)
        End If
        SaveSettings()
        SetColumnTitlesFromGroupBy()
        PopulateGrid()
    End Sub

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
                Dim lUnits As String = atcSynopticAnalysisPlugin.ColumnUnits(pColumnTitles(lColumn), cboGapUnits.Text)
                If lUnits.Length > 0 Then
                    lPane.YAxis.Title.Text &= " (" & lUnits & ")"
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

    Private Function GroupTotal(ByVal aDataGroup As atcTimeseriesGroup) As Double
        Dim lTotal As Double = 0
        For Each lTimeseries As atcTimeseries In aDataGroup
            lTotal += lTimeseries.Attributes.GetValue("Sum")
        Next
        Return lTotal
    End Function

    Private Sub mnuReverseGroupOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReverseGroupOrder.Click
        mnuReverseGroupOrder.Checked = Not mnuReverseGroupOrder.Checked
        PopulateGrid()
    End Sub
End Class