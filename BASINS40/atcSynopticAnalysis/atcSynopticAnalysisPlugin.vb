Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class atcSynopticAnalysisPlugin
    Inherits atcData.atcDataDisplay

    Public Shared TimeUnitNames() As String = {"Seconds", "Minutes", "Hours", "Days", "Weeks", "Months", "Years"}
    Public Shared TimeUnitFactor() As String = {JulianSecond, JulianMinute, JulianHour, 1, 7, 31, 366}

    'Private pMeasurementsGroupEdges() As Double = {100, 50, 20, 10, 5, 2, 1}
    Public Shared pMeasurementsGroupEdges() As Double = {100, 75, 50, 40, 30, 20, 15, _
                                                    10, 7.5, 5, 4, 3, 2, 1.5, _
                                                    1}
    'Private pVolumeGroupEdges() As Double = {10, 5, 2, 1, 0.5, 0.2, 0.1, 0.05, 0.02, 0.01, 0}
    Public Shared pVolumeGroupEdges() As Double = {10, 7.5, 5, 4, 3, 2, 1.5, _
                                              1, 0.75, 0.5, 0.4, 0.3, 0.2, 0.15, _
                                              0.1, 0.075, 0.05, 0.04, 0.03, 0.02, 0.015, _
                                              0.01, 0}
    'Private pMaximumGroupEdges() As Double = {10, 5, 2, 1, 0.5, 0.2, 0.1, 0.05, 0.02, 0.01, 0}
    Public Shared pMaximumGroupEdges() As Double = {10, 7.5, 5, 4, 3, 2, 1.5, _
                                              1, 0.75, 0.5, 0.4, 0.3, 0.2, 0.15, _
                                              0.1, 0.075, 0.05, 0.04, 0.03, 0.02, 0.015, _
                                              0.01, 0}

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Synoptic"
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcDataGroup) _
                                   As Object 'System.Windows.Forms.Form
        Dim lForm As New frmSynoptic
        lForm.Initialize(aTimeseriesGroup)
        Return lForm
    End Function

    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOptions() As String)

        Dim lForm As New frmSynoptic
        lForm.Initialize(aTimeseriesGroup)
        lForm.Hide()

        Dim lReport As New IO.StreamWriter(aFileName)

        If aOptions Is Nothing OrElse _
           aOptions.Length = 0 OrElse _
           aOptions(0) Is Nothing OrElse _
           aOptions(0).Length < 1 Then

            ReDim aOptions(0)
            aOptions(0) = lForm.cboGroupBy.Text
        End If

        For Each lOption As String In aOptions
            Dim lGroupByIndex As Integer = lForm.cboGroupBy.Items.IndexOf(lOption)
            If lGroupByIndex >= 0 Then
                lForm.cboGroupBy.SelectedIndex = lGroupByIndex
                lReport.WriteLine(lForm.ToString)
            End If
        Next

        lReport.Close()
        Logger.Dbg("SynopticAnalysis Complete, Results in file '" & aFileName & "'")
    End Sub

    Public Shared Function ComputeEvents(ByVal aDataGroup As atcTimeseriesGroup, ByVal aThreshold As Double, ByVal aDaysGapAllowed As Double, ByVal aHighEvents As Boolean) As atcTimeseriesGroup
        ComputeEvents = New atcTimeseriesGroup
        If Not aDataGroup Is Nothing AndAlso aDataGroup.Count > 0 Then
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            For Each lDataSet As atcTimeseries In aDataGroup
                Dim lEvents As atcTimeseriesGroup = atcEvents.EventSplit(lDataSet, Nothing, aThreshold, aDaysGapAllowed, aHighEvents)
                ComputeEvents.AddRange(lEvents)
            Next
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End If
    End Function

    Public Shared Function ComputeGroups(ByVal aGroupBy As String, _
                                         ByVal aEvents As atcTimeseriesGroup) As atcCollection
        Dim lGroups As New atcCollection
        Dim lGroup As atcTimeseriesGroup
        Dim lValue As Double
        Dim lValueIndex As Integer
        Dim lGroupIndex As Integer = 0

        Select Case aGroupBy
            Case "Each Event"
                For Each lEvent As atcTimeseries In aEvents
                    lGroupIndex += 1
                    lGroups.Add(lGroupIndex, New atcTimeseriesGroup(lEvent))
                Next
            Case "Number of Measurements"
                Dim lIndex As Integer
                For Each lValue In pMeasurementsGroupEdges
                    lGroups.Add(DoubleToString(lValue, , , , , 3), New atcTimeseriesGroup)
                Next
                For Each lEvent As atcTimeseries In aEvents
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
                    lGroups.Add(MonthName(lGroupIndex, True), New atcTimeseriesGroup)
                Next
                For Each lEvent As atcTimeseries In aEvents
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
                For Each lEvent As atcTimeseries In aEvents
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

                    If lYear < lFirstYear Then
                        'Year before first year found, probably using bad timeseries or more than one timeseries, add earlier years than have been added
                        For lAddYear As Integer = lFirstYear - 1 To lYear Step -1
                            lGroups.Insert(0, lAddYear, New atcTimeseriesGroup)
                        Next
                        lFirstYear = lYear
                    Else
                        For lAddYear As Integer = lFirstYear + lGroups.Count To lYear
                            lGroups.Add(lAddYear, New atcTimeseriesGroup)
                        Next
                    End If
                    lGroups.ItemByIndex(lYear - lFirstYear).Add(lEvent)
                Next
            Case "Total Volume", "Cummulative Volume"
                Dim lIndex As Integer
                For Each lValue In pVolumeGroupEdges
                    lGroups.Add(DoubleToString(lValue, , , , , 3), New atcTimeseriesGroup)
                Next
                For Each lEvent As atcTimeseries In aEvents
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
                    lGroups.Add(DoubleToString(lValue, , , , , 3), New atcTimeseriesGroup)
                Next
                For Each lEvent As atcTimeseries In aEvents
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
                    lGroups.Add(DoubleToString(lValue, , , , , 3), New atcTimeseriesGroup)
                Next
                For Each lEvent As atcTimeseries In aEvents
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
                lGroups.Add("All", aEvents)
        End Select

        'Omit empty groups from display
        For lGroupIndex = lGroups.Count - 1 To 0 Step -1
            lGroup = lGroups.ItemByIndex(lGroupIndex)
            If lGroup.Count = 0 Then lGroups.RemoveAt(lGroupIndex)
        Next

        Return lGroups
    End Function

    Public Shared Function PopulateGrid(ByVal aGroups As atcCollection, _
                                        ByVal aTimeUnits As String, _
                                        ByVal aColumnTitles() As String, _
                                        ByVal aColumnAttributes() As String) As atcControls.atcGridSource
        Dim lGroup As atcTimeseriesGroup
        Dim lDataset As atcTimeseries

        Dim lColumn As Integer
        Dim lDate As Date
        Dim lValue As Double
        Dim lCumulativeValue As Double
        Dim lGroupIndex As Integer = 0
        Dim lValueIndex As Integer
        Dim lDurationFactor As Double

        Try
            lDurationFactor = TimeUnitFactor(Array.IndexOf(TimeUnitNames, aTimeUnits))
        Catch
            Try
                lDurationFactor = TimeUnitFactor(Array.IndexOf(TimeUnitNames, aTimeUnits & "s"))
            Catch
                Throw New ApplicationException("Unknown time units '" & aTimeUnits & "'")
            End Try
        End Try

        Dim lSource As New atcControls.atcGridSource
        With lSource
            .Columns = aColumnTitles.Length
            .FixedRows = 3
            .Rows = aGroups.Count + lSource.FixedRows
            For lColumn = 0 To aColumnTitles.Length - 1
                .CellValue(0, lColumn) = aColumnTitles(lColumn)
                .CellValue(1, lColumn) = aColumnAttributes(lColumn)
                .CellValue(2, lColumn) = ColumnUnits(aColumnTitles(lColumn), aTimeUnits)
                .CellColor(0, lColumn) = Drawing.Color.LightGray
                .CellColor(1, lColumn) = Drawing.Color.LightGray
                .CellColor(2, lColumn) = Drawing.Color.LightGray
            Next
        End With

        lGroupIndex = 0
        For Each lGroup In aGroups
            Dim lMeasurements As Integer = 0
            Dim lWholeGroupDataset As atcTimeseries = MergeTimeseries(lGroup)
            Dim lDurationDataset As New atcTimeseries(Nothing)
            Dim lVolumeDataset As New atcTimeseries(Nothing)
            Dim lTimeSinceLastDataset As New atcTimeseries(Nothing)

            For Each lDataset In lGroup
                lMeasurements += lDataset.numValues
            Next

            lDurationDataset.numValues = lGroup.Count
            lVolumeDataset.numValues = lGroup.Count
            lTimeSinceLastDataset.numValues = lGroup.Count
            lValueIndex = 1
            For Each lDataset In lGroup
                lDurationDataset.Value(lValueIndex) = DataSetDuration(lDataset)
                lVolumeDataset.Value(lValueIndex) = lDataset.Attributes.GetValue("Sum")
                lTimeSinceLastDataset.Value(lValueIndex) = lDataset.Attributes.GetValue("EventTimeSincePrevious")
                lValueIndex += 1
            Next

            For lColumn = 0 To aColumnTitles.Length - 1
                lValue = 0
                lCumulativeValue = 0
                lDataset = Nothing
                Select Case aColumnTitles(lColumn)
                    Case "Group"
                        lSource.CellValue(lGroupIndex + lSource.FixedRows, lColumn) = aGroups.Keys(lGroupIndex)
                    Case "Location"
                        lSource.CellValue(lGroupIndex + lSource.FixedRows, lColumn) = lGroup.ItemByIndex(0).Attributes.GetValue("Location")
                    Case "Start Date"
                        lDataset = lGroup.ItemByIndex(0)
                        lDate = Date.FromOADate(lDataset.Dates.Value(1))
                        lSource.CellValue(lGroupIndex + lSource.FixedRows, lColumn) = lDate.ToString("yyyy-MM-dd")

                    Case "Start Time"
                        lDataset = lGroup.ItemByIndex(0)
                        lDate = Date.FromOADate(lDataset.Dates.Value(1))
                        lSource.CellValue(lGroupIndex + lSource.FixedRows, lColumn) = lDate.ToString("HH:mm")
                    Case "Events"
                        lValue = lGroup.Count
                    Case "Measurements"
                        lValue = lMeasurements
                    Case "Volume"
                        lDataset = lVolumeDataset
                    Case "Duration"
                        lDataset = lDurationDataset
                    Case "Intensity"
                        lDataset = lWholeGroupDataset
                    Case "Time Since Last"
                        lDataset = lTimeSinceLastDataset
                End Select

                If aColumnAttributes(lColumn) = "Cumulative" Then
                    If Not lDataset Is Nothing Then
                        lValue = lDataset.Attributes.GetValue("Sum")
                    End If
                    If lGroupIndex > 0 Then
                        Try
                            lCumulativeValue = CDbl(lSource.CellValue(lGroupIndex - 1 + lSource.FixedRows, lColumn))
                        Catch
                            lCumulativeValue = 0
                        End Try
                    End If
                ElseIf aColumnAttributes(lColumn).Length > 0 AndAlso Not lDataset Is Nothing Then
                    Dim lStr As String = lDataset.Attributes.GetValue(aColumnAttributes(lColumn))
                    If IsNumeric(lStr) Then
                        lValue = lStr
                    End If
                End If

                Select Case aColumnTitles(lColumn)
                    Case "Duration", "Time Since Last"
                        lValue /= lDurationFactor
                End Select

                lValue += lCumulativeValue

                If lValue <> 0 Then
                    Select Case aColumnTitles(lColumn)
                        Case "Group", "Start Date", "Start Time"
                            'custom code above to set cell value
                        Case "Events", "Measurements" 'Integer values
                            lSource.CellValue(lGroupIndex + lSource.FixedRows, lColumn) = CInt(lValue)
                        Case Else
                            lSource.CellValue(lGroupIndex + lSource.FixedRows, lColumn) = DoubleToString(lValue)
                    End Select
                End If
            Next
            lGroupIndex += 1
        Next

        Return lSource
    End Function

    Friend Shared Function ColumnUnits(ByVal aColumnName As String, ByVal aTimeUnits As String) As String
        Select Case aColumnName
            Case "Events" : Return ""
            Case "Group" : Return ""
            Case "Start Date" : Return ""
            Case "Start Time" : Return ""
            Case "Measurements" : Return ""
            Case "Volume" : Return "in" 'TODO: use data units
            Case "Duration" : Return aTimeUnits
            Case "Intensity" : Return "in/hr" 'TODO: use aTimeUnits?
            Case "Time Since Last" : Return aTimeUnits
            Case Else : Return ""
        End Select
    End Function

    Public Shared Function DataSetDuration(ByVal aTimeseries As atcTimeseries) As Double
        If Not Double.IsNaN(aTimeseries.Dates.Value(0)) Then
            Return aTimeseries.Dates.Value(aTimeseries.numValues) - aTimeseries.Dates.Value(0)
        Else
            Return aTimeseries.Dates.Value(aTimeseries.numValues) + aTimeseries.Dates.Value(2) - 2 * aTimeseries.Dates.Value(1)
        End If
    End Function
End Class
