Imports atcData
Imports atcEvents
Imports atcUtility
Imports MapWinUtility

Public Class atcVariation
    Private pNaN As Double = atcUtility.GetNaN

    'Parameters for Hamon
    Private pDegF As Boolean
    Private pLatDeg As Double

    'WestBranch of Patux
    'Private pCTS() As Double = {0, 0.0045, 0.01, 0.01, 0.01, 0.0085, 0.0085, 0.0085, 0.0085, 0.0085, 0.0095, 0.0095, 0.0095}
    'Monocacy - CBP
    Private pCTS() As Double = {0, 0.0057, 0.0057, 0.0057, 0.0057, 0.0057, 0.0057, _
                                   0.0057, 0.0057, 0.0057, 0.0057, 0.0057, 0.0057}

    Private pName As String
    Private pDataSets As atcTimeseriesGroup

    Public PETtemperature As atcTimeseriesGroup
    Public PETprecipitation As atcTimeseriesGroup
    Public PETelevation As Integer = Integer.MinValue
    Public PETstationID As String = Nothing
    Private Shared PETswatStations As atcMetCmp.SwatWeatherStations

    Private pComputationSource As atcTimeseriesSource
    Private pOperation As String
    'Public AddRemovePer As String
    Private pSelected As Boolean

    'TODO: make rest of public variables into properties
    Public Seasons As atcSeasonBase
    Public Min As Double
    Public Max As Double
    Public Increment As Double
    Private pIncrementsSinceStart As Integer
    Public CurrentValue As Double

    Public UseEvents As Boolean
    Public EventThreshold As Double
    Public EventDaysGapAllowed As Double
    'Public EventGapDisplayUnits As String
    Public EventHigh As Boolean

    Public EventVolumeHigh As Boolean
    Public EventVolumeThreshold As Double

    Public EventDurationHigh As Boolean
    Public EventDurationDays As Double
    'Public EventDurationDisplayUnits As String

    Public IntensifyVolumeFraction As Double

    Public IsInput As Boolean

    Public ColorAboveMax As System.Drawing.Color
    Public ColorBelowMin As System.Drawing.Color
    Public ColorDefault As System.Drawing.Color

    Public Overridable Property Name() As String
        Get
            Return pName
        End Get
        Set(ByVal newValue As String)
            pName = newValue
        End Set
    End Property

    Public Overridable Property DataSets() As atcTimeseriesGroup
        Get
            Return pDataSets
        End Get
        Set(ByVal newValue As atcTimeseriesGroup)
            pDataSets = newValue
        End Set
    End Property

    Public Overridable Property ComputationSource() As atcTimeseriesSource
        Get
            Return pComputationSource
        End Get
        Set(ByVal newValue As atcTimeseriesSource)
            pComputationSource = newValue
        End Set
    End Property

    Public Overridable Property Operation() As String
        Get
            Return pOperation
        End Get
        Set(ByVal newValue As String)
            pOperation = newValue
        End Set
    End Property

    Public Overridable Property Selected() As Boolean
        Get
            Return pSelected
        End Get
        Set(ByVal newValue As Boolean)
            pSelected = newValue
        End Set
    End Property

    Public Overridable ReadOnly Property Iterations() As Integer
        Get
            If Increment = 0 Then
                Return 1
            Else
                Try
                    Return (Max - Min) / Increment + 1
                Catch ex As Exception
                    Return 1
                End Try
            End If
        End Get
    End Property

    Public Overridable Function StartIteration() As atcTimeseriesGroup
        Me.CurrentValue = Me.Min
        pIncrementsSinceStart = 0
        Return VaryData()
    End Function

    Public Overridable Function NextIteration() As atcTimeseriesGroup
        pIncrementsSinceStart += 1
        If pIncrementsSinceStart < Iterations Then
            Me.CurrentValue = Me.Min + Me.Increment * pIncrementsSinceStart
            Return VaryData()
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Divide the data in lOriginalData into a group of two atcTimeseries.
    ''' First in group contains all values in the selected events and/or seasons, 
    ''' Second includes all other values.
    ''' </summary>
    ''' <param name="aOriginalData">timeseries to split into selected and not selected</param>
    ''' <param name="aEvents">Return argument: populated with one timeseries per selected event
    '''                       found in aOriginalData if events are in use,
    '''                       not set if events are not in use</param>
    ''' <returns>Group of two timeseries</returns>
    Public Function SplitData(ByVal aOriginalData As atcTimeseries, _
                              ByRef aEvents As atcTimeseriesGroup) As atcTimeseriesGroup
        Dim lSplitData As atcTimeseriesGroup = Nothing
        If UseEvents Then
            Dim lEvent As atcTimeseries
            aEvents = EventSplit(aOriginalData, Nothing, EventThreshold, EventDaysGapAllowed, EventHigh)

            'Remove events outside selected seasons
            If Seasons IsNot Nothing Then
                For lEventIndex As Integer = aEvents.Count - 1 To 0 Step -1
                    lEvent = aEvents.ItemByIndex(lEventIndex)

                    'Find peak value of event
                    Dim lPeakIndex As Integer = 1
                    For lValueIndex As Integer = 2 To lEvent.numValues
                        If lEvent.Value(lValueIndex) > lEvent.Value(lPeakIndex) Then
                            lPeakIndex = lValueIndex
                        End If
                    Next

                    'If peak is not in season, remove this event
                    If Not Seasons.SeasonSelected(Seasons.SeasonIndex(lEvent.Dates.Value(lPeakIndex))) Then
                        aEvents.RemoveAt(lEventIndex)
                    End If
                Next
            End If

            'Remove events outside target volume threshold
            Try
                If Not Double.IsNaN(EventVolumeThreshold) Then
                    For lEventIndex As Integer = aEvents.Count - 1 To 0 Step -1
                        Dim lEventVolume As Double = aEvents.ItemByIndex(lEventIndex).Attributes.GetValue("Sum")
                        If EventVolumeHigh Then
                            If lEventVolume < EventVolumeThreshold Then
                                aEvents.RemoveAt(lEventIndex)
                            End If
                        Else
                            If lEventVolume > EventVolumeThreshold Then
                                aEvents.RemoveAt(lEventIndex)
                            End If
                        End If
                    Next
                End If
            Catch e As Exception
                Logger.Dbg("VaryDataException-EventVolumeThreshold " & e.Message)
            End Try

            'Remove events outside target duration threshold
            If Not Double.IsNaN(EventDurationDays) Then
                For lEventIndex As Integer = aEvents.Count - 1 To 0 Step -1
                    Dim lEventDuration As Double = atcSynopticAnalysis.atcSynopticAnalysisPlugin.DataSetDuration(aEvents.ItemByIndex(lEventIndex))
                    If EventDurationHigh Then
                        If lEventDuration < EventDurationDays Then
                            aEvents.RemoveAt(lEventIndex)
                        End If
                    Else
                        If lEventDuration > EventDurationDays Then
                            aEvents.RemoveAt(lEventIndex)
                        End If
                    End If
                Next
            End If

            'If Operation <> "Intensify" Then
            If aEvents.Count > 0 Then
                lSplitData = New atcTimeseriesGroup(MergeTimeseries(aEvents))
                lSplitData.Add(aOriginalData)
            End If
            'End If

        Else
            If Seasons Is Nothing Then
                lSplitData = New atcTimeseriesGroup(aOriginalData)
            Else
                lSplitData = Seasons.SplitBySelected(aOriginalData, Nothing)
            End If
        End If
        Return lSplitData
    End Function

    Private Function VaryDataIntensify(ByRef lSplitData As atcTimeseriesGroup, ByRef lEvents As atcTimeseriesGroup) As atcTimeseries
        Dim lEvent As atcTimeseries
        Dim lArgsMath As New atcDataAttributes
        Dim lTotalVolume As Double = lSplitData(0).Attributes.GetValue("Sum")
        Dim lEventIntensifyFactor As Double
        Dim lCurrentVolume As Double = 0
        Dim lTargetChange As Double = CurrentValue / 100 * lTotalVolume
        Dim lNewEventTotalVolume As Double = 0.0

        Try
            If Not Double.IsNaN(IntensifyVolumeFraction) Then
                Dim lTargetVolumeToIntensify As Double = lTotalVolume * IntensifyVolumeFraction
                Logger.Dbg("TargetChange " & DecimalAlign(lTargetChange) & " TargetVolumeToIntensify " & DecimalAlign(lTargetVolumeToIntensify))
                'sort events by volume
                Logger.Dbg("Intensify " & DecimalAlign(IntensifyVolumeFraction) & " CurrentValue " & DecimalAlign(CurrentValue))
                Dim lNewEvents As New System.Collections.SortedList(lEvents.Count)
                For Each lEvent In lEvents
                    Dim lEventVolume As Double = lEvent.Attributes.GetValue("Sum")
                    While lNewEvents.IndexOfKey(lEventVolume) >= 0
                        lEventVolume += 0.00000001
                    End While
                    lNewEvents.Add(lEventVolume, lEvent)
                    lNewEventTotalVolume += lEventVolume
                Next
                lEvents.Clear()
                Logger.Dbg(" TotalVolume " & DecimalAlign(lTotalVolume) & _
                           " EventTotalVolume " & DecimalAlign(lNewEventTotalVolume) & _
                           " PercentOfVolume " & DecimalAlign(100 * (lNewEventTotalVolume / lTotalVolume)))

                If CurrentValue > 0.0 Then
                    For lEventIndex As Integer = lNewEvents.Count - 1 To 0 Step -1
                        lEvent = lNewEvents.GetByIndex(lEventIndex)
                        Dim lAddFromThisEvent As Double = lNewEvents.GetKey(lEventIndex)
                        lCurrentVolume += lAddFromThisEvent
                        'Logger.Dbg("CurrentVolumeAdded " & lCurrentVolumeChange & " FromThisEvent " & lAddFromThisEvent)

                        If lEventIndex = lNewEvents.Count - 1 Then 'details of biggest event
                            Dim lEventStr As String = "  Event " & lEventIndex
                            lEventStr &= "    Sum " & DecimalAlign(lEvent.Attributes.GetValue("Sum"))
                            lEventStr &= " NumVals " & lEvent.numValues
                            lEventStr &= " Starts " & DumpDate(lEvent.Dates.Value(1))
                            Logger.Dbg(lEventStr)
                        End If

                        lEvents.Add(lEvent)
                        If lCurrentVolume > lTargetVolumeToIntensify Then
                            Logger.Dbg("Intensify " & lNewEvents.Count - lEventIndex & " of " & lNewEvents.Count)
                            Exit For
                        End If
                    Next lEventIndex
                Else
                    For lEventIndex As Integer = 0 To lNewEvents.Count - 1
                        lEvent = lNewEvents.GetByIndex(lEventIndex)
                        lCurrentVolume += lNewEvents.GetKey(lEventIndex)
                        ' Logger.Dbg("CurrentVolumeRemoved " & lCurrentVolumeChange)
                        lEvents.Add(lEvent)
                        If lCurrentVolume > lTargetVolumeToIntensify Then
                            Logger.Dbg("NegativeIntensify " & lEventIndex & " of " & lNewEvents.Count)
                            Exit For
                        End If
                    Next
                End If
            End If
        Catch e As Exception
            Logger.Dbg("VaryDataException-EventIntensifyFactor " & e.Message)
        End Try

        Logger.Dbg(" CurrentVolume " & DecimalAlign(lCurrentVolume) & _
                   " TargetChange " & DecimalAlign(lTargetChange))
        lEventIntensifyFactor = lTargetChange / lCurrentVolume
        Logger.Dbg("EventIntensifyFactor " & DecimalAlign(lEventIntensifyFactor))
        Dim lSplitTS As atcTimeseries = MergeTimeseries(lEvents)

        ComputationSource.DataSets.Clear()
        lArgsMath.Clear()
        lArgsMath.SetValue("timeseries", lSplitTS)
        lArgsMath.SetValue("Number", 1 + lEventIntensifyFactor)
        ComputationSource.Open("Multiply", lArgsMath)
        If lSplitData.Count > 1 Then
            Return MergeTimeseries(New atcTimeseriesGroup(ComputationSource.DataSets(0), lSplitData.ItemByIndex(1)))
        Else
            Return ComputationSource.DataSets(0)
        End If
    End Function

    Private Function VaryDataAddMultiply(ByVal lOriginalData As atcTimeseries, ByRef lSplitData As atcTimeseriesGroup) As atcTimeseries
        Dim lArgsMath As New atcDataAttributes
        If lSplitData IsNot Nothing AndAlso lSplitData.Count > 0 Then
            Dim lSplitTS As atcTimeseries = lSplitData.ItemByIndex(0)
            ComputationSource.DataSets.Clear()
            lArgsMath.Clear()
            lArgsMath.SetValue("timeseries", lSplitTS)
            lArgsMath.SetValue("Number", CurrentValue)
            ComputationSource.Open(Operation, lArgsMath)
            Dim lModifiedTS As atcTimeseries = ComputationSource.DataSets(0)
            If lModifiedTS.Dates = lOriginalData.Dates Then
                lModifiedTS.Dates = lModifiedTS.Dates.Clone
            End If

            If lSplitData.Count > 1 Then
                Return MergeTimeseries(New atcTimeseriesGroup(ComputationSource.DataSets(0), lSplitData.ItemByIndex(1)))
            Else
                Return ComputationSource.DataSets(0)
            End If
        Else
            Return lOriginalData.Clone
        End If
    End Function

    Protected Overridable Function VaryData() As atcTimeseriesGroup
        Dim lModifiedGroup As New atcTimeseriesGroup
        Dim lDataSetIndex As Integer = 0

        For Each lOriginalData As atcTimeseries In DataSets
            Dim lEvents As atcTimeseriesGroup = Nothing
            Dim lSplitData As atcTimeseriesGroup = SplitData(lOriginalData, lEvents)
            Dim lModifiedTS As atcTimeseries = Nothing

            Select Case Operation
                Case "AddEvents" : lModifiedTS = AddRemoveEventsVolumeFraction(lOriginalData, CurrentValue, lEvents, 0)
                Case "Intensify" : lModifiedTS = VaryDataIntensify(lSplitData, lEvents)
                Case "Hamon"

                    If PETtemperature.Count > lDataSetIndex Then
                        pLatDeg = lOriginalData.Attributes.GetValue("LatDeg", pLatDeg)
                        lModifiedTS = atcMetCmp.PanEvaporationTimeseriesComputedByHamonX(PETtemperature(lDataSetIndex), Nothing, pDegF, pLatDeg, pCTS)
                        If lOriginalData.Attributes.GetValue("tu") < 4 Then
                            lModifiedTS = atcMetCmp.DisSolPet(lModifiedTS, Nothing, 2, pLatDeg)
                        End If
                    End If

                Case "Penman-Monteith"
                    If PETtemperature.Count > lDataSetIndex AndAlso PETprecipitation.Count > lDataSetIndex Then
                        Dim PETstation As atcMetCmp.SwatWeatherStation = Nothing

                        If PETswatStations Is Nothing Then
                            PETswatStations = New atcMetCmp.SwatWeatherStations
                        End If

                        If PETswatStations IsNot Nothing Then
                            If Not String.IsNullOrEmpty(PETstationID) Then 'Find by station's Name, NameKey or ID
                                Dim lStationID As String = PETstationID.ToLower
                                For Each lSearchStation As atcMetCmp.SwatWeatherStation In PETswatStations
                                    If lSearchStation.Id.ToLower = lStationID OrElse _
                                       lSearchStation.Name.ToLower = lStationID OrElse _
                                       lSearchStation.NameKey.ToLower = lStationID Then
                                        PETstation = lSearchStation
                                        Exit For
                                    End If
                                Next
                            End If

                            If PETstation Is Nothing Then 'Find by lat/lon of original TS
                                Dim lLatitude As Double = lOriginalData.Attributes.GetValue("Latitude", -999)
                                Dim lLongitude As Double = lOriginalData.Attributes.GetValue("Longitude", -999)
                                If lLatitude > -90 AndAlso lLongitude > -360 Then
                                    PETstation = PETswatStations.Closest(lLatitude, lLongitude, 1)(0)
                                End If
                            End If
                        End If

                        If PETstation Is Nothing Then
                            Throw New ApplicationException("VaryData: PET station not found")
                        ElseIf Double.IsNaN(PETelevation) Then
                            Throw New ApplicationException("VaryData: Elevation not found for PET")
                        ElseIf PETprecipitation(lDataSetIndex) Is Nothing Then
                            Throw New ApplicationException("VaryData: Precipitation not found for PET")
                        ElseIf PETtemperature(lDataSetIndex) Is Nothing Then
                            Throw New ApplicationException("VaryData: Temperature not found for PET")
                        Else
                            lModifiedTS = atcMetCmp.PanEvaporationTimeseriesComputedByPenmanMonteith(PETelevation, PETprecipitation(lDataSetIndex), PETtemperature(lDataSetIndex), Nothing, PETstation)
                        End If
                    End If

                Case Else
                        lModifiedTS = VaryDataAddMultiply(lOriginalData, lSplitData)
            End Select

            If lModifiedTS Is Nothing Then
                Throw New ApplicationException("VaryData: No data computed")
            End If

            Dim lMostOriginal As atcTimeseries = MostOriginal(lOriginalData)
            With lModifiedTS.Attributes
                .SetValue("CAToriginal", lMostOriginal)
                .SetValue("ID", lOriginalData.Attributes.GetValue("ID"))
                .SetValue("Location", lOriginalData.Attributes.GetValue("Location"))
                .SetValue("Constituent", lOriginalData.Attributes.GetValue("Constituent"))
                .SetValue("History 1", lOriginalData.Attributes.GetValue("History 1").ToString)
                If lOriginalData.Attributes.ContainsAttribute("TSTYPE") Then
                    .SetValue("TSTYPE", lOriginalData.Attributes.GetValue("TSTYPE"))
                End If
            End With

            lModifiedGroup.Add(lMostOriginal, lModifiedTS)

            lDataSetIndex += 1
        Next
        Return lModifiedGroup
    End Function

    Private Function MostOriginal(ByVal aTimeseries As atcTimeseries) As atcTimeseries
        Dim lMostOriginal As atcTimeseries = aTimeseries
        While lMostOriginal.Attributes.ContainsAttribute("CAToriginal")
            lMostOriginal = lMostOriginal.Attributes.GetValue("CAToriginal")
        End While
        Return lMostOriginal
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="aTimeseries">Original data values</param>
    ''' <param name="aVolumeChangeFraction">Amount to change total volume, (-0.5=remove 50%, 0=no change, 0.5=add 50%)</param>
    ''' <param name="aEventsToSearch">Events available for adding</param>
    ''' <param name="aSeed">Random number seed</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddRemoveEventsVolumeFraction(ByVal aTimeseries As atcTimeseries, _
                                                   ByVal aVolumeChangeFraction As Double, _
                                                   ByVal aEventsToSearch As atcTimeseriesGroup, _
                                                   ByVal aSeed As Integer) As atcTimeseries
        Dim lNewTimeseries As atcTimeseries = aTimeseries.Clone
        Dim lMaxEventIndex As Integer = aEventsToSearch.Count  'exclusive upper value, random less than
        Dim lFoundIndexes As New ArrayList
        Dim lRandom As New Random(aSeed)
        Dim lAdd As Boolean = True
        Dim lValueIndex As Integer
        Dim lValueLastIndex As Integer
        Dim lOriginalVolume As Double = aTimeseries.Attributes.GetValue("Sum")
        Dim lCurrentVolume As Double = lOriginalVolume
        Dim lTargetVolume As Double = lOriginalVolume * (1 + aVolumeChangeFraction)
        Dim lChangeIndex As Integer

        If aVolumeChangeFraction < 0 Then lAdd = False

        Logger.Dbg("OriginalVolume " & DecimalAlign(lOriginalVolume) & _
                   " Target Volume " & DecimalAlign(lTargetVolume) & _
                   " ChangeFraction " & DecimalAlign(aVolumeChangeFraction) & _
                   " Event Count " & aEventsToSearch.Count)

        'While adding and not yet added enough, or removing and not yet removed enough
        While (lAdd AndAlso (lCurrentVolume < lTargetVolume)) OrElse _
         ((Not lAdd) AndAlso (lCurrentVolume > lTargetVolume))
            Dim lCheckIndex As Integer = lRandom.Next(0, lMaxEventIndex)
            If Not lFoundIndexes.Contains(lCheckIndex) Then
                Dim lEvent As atcTimeseries = aEventsToSearch.ItemByIndex(lCheckIndex)
                lFoundIndexes.Add(lCheckIndex)
                If lFoundIndexes.Count = aEventsToSearch.Count Then 'all events have been used, start over
                    If lAdd Then
                        Logger.Dbg("  ---- All events have been used, start adding over")
                        lFoundIndexes.Clear()
                    Else
                        Logger.Dbg("  ***** All events have been used, nothing more to remove!")
                        Exit While
                    End If
                End If

                Dim lEventStr As String = "  Event " & lCheckIndex
                lEventStr &= "    Sum " & DecimalAlign(lEvent.Attributes.GetValue("Sum"))
                lEventStr &= " NumVals " & lEvent.numValues
                lEventStr &= " Starts " & DumpDate(lEvent.Dates.Value(1))
                Logger.Dbg(lEventStr)

                'Find starting index of event and add it or remove it
                If lAdd Then
                    lValueIndex = lRandom.Next(1, lNewTimeseries.numValues - lEvent.numValues)
                    Logger.Dbg("    Add at " & DumpDate(lNewTimeseries.Dates.Value(lValueIndex)))
                    lValueLastIndex = lValueIndex + lEvent.numValues - 1
                    lChangeIndex = lValueIndex
                    While lChangeIndex <= lValueLastIndex AndAlso _
                          Not Double.IsNaN(lNewTimeseries.Values(lChangeIndex)) AndAlso _
                          lCurrentVolume < lTargetVolume
                        'lCurrentVolume -= lNewTimeseries.Values(lIndex)
                        'add to old value
                        Dim lAddValue As Double = lEvent.Values(lChangeIndex - lValueIndex + 1)
                        lNewTimeseries.Values(lChangeIndex) += lAddValue
                        lCurrentVolume += lAddValue
                        lChangeIndex += 1
                    End While
                Else 'remove
                    lValueIndex = FindDateAtOrAfter(lNewTimeseries.Dates.Values, lEvent.Dates.Value(1))
                    Logger.Dbg("    Remove at " & DumpDate(lNewTimeseries.Dates.Value(lValueIndex).ToString))
                    lValueLastIndex = lValueIndex + lEvent.numValues - 1
                    lChangeIndex = lValueIndex
                    While lChangeIndex <= lValueLastIndex AndAlso _
                          Not Double.IsNaN(lNewTimeseries.Values(lChangeIndex)) AndAlso _
                          lCurrentVolume > lTargetVolume
                        lCurrentVolume -= lNewTimeseries.Values(lChangeIndex)
                        lNewTimeseries.Values(lChangeIndex) = 0.0
                        lChangeIndex += 1
                    End While
                End If
                Logger.Dbg("    CurrentVolume " & DecimalAlign(lCurrentVolume))
            End If
        End While

        lChangeIndex -= 1
        Dim lFinalVolumeAdjustment As Double = lCurrentVolume - lTargetVolume
        If lChangeIndex >= 0 AndAlso (lNewTimeseries.Values(lChangeIndex) - lFinalVolumeAdjustment) > 0 Then
            Logger.Dbg("  Final Volume Adjustment " & DecimalAlign(lFinalVolumeAdjustment) & _
                       " on " & DecimalAlign(lNewTimeseries.Values(lChangeIndex)) & _
                       " at " & DumpDate(lNewTimeseries.Dates.Value(lChangeIndex)))
            lNewTimeseries.Values(lChangeIndex) -= lFinalVolumeAdjustment
        Else
            Dim lDbgStr As String = "  ***** Fail Final Volume Adjustment " & DecimalAlign(lFinalVolumeAdjustment) & " " & lChangeIndex
            If lChangeIndex > 0 Then lDbgStr &= " " & DecimalAlign(lNewTimeseries.Values(lChangeIndex))
            Logger.Dbg(lDbgStr)
        End If

        lNewTimeseries.Attributes.DiscardCalculated()

        Dim lOperation As String
        If lAdd Then lOperation = "Added " Else lOperation = "Removed "

        Dim lSum As Double = lNewTimeseries.Attributes.GetValue("Sum")
        Dim lStr As String = lOperation & lFoundIndexes.Count & " events to change total volume " & DoubleString(aVolumeChangeFraction)
        lStr &= " (actual change = " & DoubleString((lSum - lOriginalVolume) / lOriginalVolume) & ")"
        lStr &= " Total Volume " & DecimalAlign(lSum)
        Logger.Dbg(lStr)

        Return lNewTimeseries
    End Function

    Private Function DoubleArraySum(ByVal aValues() As Double, ByVal aStart As Integer, ByVal aCount As Integer) As Double
        DoubleArraySum = 0
        Dim lBeyondLastIndex As Integer = aStart + aCount
        While aStart < lBeyondLastIndex
            DoubleArraySum += aValues(aStart)
            aStart += 1
        End While
    End Function

    Private Function AddRemoveEventsTotalVolume(ByVal aTimeseries As atcTimeseries, ByVal aTargetVolumeChange As Double, ByVal aEventsToSearch As atcTimeseriesGroup, ByVal aSeed As Integer) As atcTimeseries
        '    Private Function FindEventsTotalVolume(ByVal aTargetTotalVolume As Double, ByVal aEventsToSearch As atcDataGroup, ByVal aSeed As Integer) As atcDataGroup
        'Dim lEventsFound As New atcDataGroup
        Dim lNewTimeseries As atcTimeseries = aTimeseries.Clone
        Dim lMaxEventIndex As Integer = aEventsToSearch.Count - 1
        Dim lFoundIndexes As New ArrayList
        Dim lVolumeFound As Double = 0
        Dim lRandom As New Random(aSeed)
        Dim lAdd As Boolean = True
        Dim lValueIndex As Integer
        Dim lLastValueIndex As Integer

        If aTargetVolumeChange < 0 Then
            lVolumeFound = aTargetVolumeChange
            aTargetVolumeChange = 0
            lAdd = False
        End If

        While lVolumeFound < aTargetVolumeChange
            Dim lCheckIndex As Integer = lRandom.Next(0, lMaxEventIndex)
            If Not lFoundIndexes.Contains(lCheckIndex) Then
                Dim lEvent As atcTimeseries = aEventsToSearch.ItemByIndex(lCheckIndex)
                lFoundIndexes.Add(lCheckIndex)
                'lEventsFound.Add(lEvent)
                lVolumeFound += lEvent.Attributes.GetValue("Sum")

                'Find starting index of event
                If lAdd Then
                    lValueIndex = lRandom.Next(1, aTimeseries.numValues - lEvent.numValues)
                Else
                    lValueIndex = FindDateAtOrAfter(lNewTimeseries.Dates.Values, lEvent.Dates.Value(1))
                End If
                lLastValueIndex = lValueIndex + lEvent.numValues - 1

                'Reduce event volume by volume being replaced
                For lScanReplaced As Integer = lValueIndex To lLastValueIndex
                    lVolumeFound -= lNewTimeseries.Values(lScanReplaced)
                Next

                If lAdd Then
                    Array.Copy(lEvent.Values, 1, lNewTimeseries.Values, lValueIndex, lEvent.numValues)
                Else
                    Array.Clear(lNewTimeseries.Values, lValueIndex, lEvent.numValues)
                End If

            End If
        End While

        Dim lOperation As String
        If lAdd Then lOperation = "Added " Else lOperation = "Removed "

        Dim lStr As String = lOperation & lFoundIndexes.Count & " events to change total volume " & DoubleString(aTargetVolumeChange)
        lStr &= " (actual change = " & DoubleString(lNewTimeseries.Attributes.GetValue("Sum") - aTimeseries.Attributes.GetValue("Sum")) & ")"
        Logger.Dbg(lStr)

        Return lNewTimeseries

    End Function

    Public Overridable Function Clone() As atcVariation
        Dim newVariation As New atcVariation
        Me.CopyTo(newVariation)
        Return newVariation
    End Function

    Sub Clear()

        'Parameters for Hamon - TODO: don't hard code these
        pDegF = True
        pLatDeg = 39

        pName = "<untitled>"
        pDataSets = New atcTimeseriesGroup
        pComputationSource = Nothing
        pOperation = "Add"
        pSelected = False

        Seasons = Nothing
        Min = pNaN
        Max = pNaN
        Increment = pNaN
        pIncrementsSinceStart = 0
        CurrentValue = pNaN

        UseEvents = False
        EventThreshold = pNaN
        EventDaysGapAllowed = 0
        'EventGapDisplayUnits = ""
        EventHigh = True
        IntensifyVolumeFraction = pNaN
        'AddRemovePer = "Entire Span"

        EventVolumeHigh = True
        EventVolumeThreshold = pNaN

        EventDurationHigh = True
        EventDurationDays = pNaN
        'EventDurationDisplayUnits = ""

        IsInput = False

        ColorAboveMax = System.Drawing.Color.OrangeRed
        ColorBelowMin = System.Drawing.Color.DeepSkyBlue
        ColorDefault = System.Drawing.Color.White

        PETtemperature = New atcTimeseriesGroup
    End Sub

    Public Overridable Sub CopyTo(ByVal aTargetVariation As atcVariation)
        With aTargetVariation
            .Name = Name
            .UseEvents = UseEvents
            If UseEvents Then
                .EventDaysGapAllowed = EventDaysGapAllowed
                '.EventGapDisplayUnits = EventGapDisplayUnits
                .EventHigh = EventHigh
                .EventThreshold = EventThreshold
                .EventVolumeHigh = EventVolumeHigh
                .EventVolumeThreshold = EventVolumeThreshold
                .EventDurationHigh = EventDurationHigh
                .EventDurationDays = EventDurationDays
                .IntensifyVolumeFraction = IntensifyVolumeFraction
            End If

            If DataSets IsNot Nothing Then .DataSets = DataSets.Clone()
            If PETtemperature IsNot Nothing Then .PETtemperature = PETtemperature.Clone()
            .ComputationSource = ComputationSource
            .Operation = Operation.Clone()
            If Seasons Is Nothing Then
                .Seasons = Nothing
            Else
                .Seasons = Seasons.Clone
            End If
            .Selected = Selected
            .Min = Min
            .Max = Max
            .Increment = Increment
            '.AddRemovePer = AddRemovePer
            .IsInput = IsInput
            .CurrentValue = CurrentValue
            .ColorAboveMax = ColorAboveMax
            .ColorBelowMin = ColorBelowMin
            .ColorDefault = ColorDefault
        End With
    End Sub

    Protected Overridable Property EventsXML() As String
        Get
            If UseEvents Then
                Return "  <Events Threshold='" & EventThreshold & "' " _
                              & " High='" & EventHigh & "' " _
                              & " IntensifyVolumeFraction='" & IntensifyVolumeFraction & "' " _
                              & " GapDays='" & EventDaysGapAllowed & "' " _
                              & " VolumeHigh='" & EventVolumeHigh & "' " _
                              & " VolumeThreshold='" & EventVolumeThreshold & "' " _
                              & " DurationHigh='" & EventDurationHigh & "' " _
                              & " DurationDays='" & EventDurationDays & "' " _
                              & "/>" & vbCrLf
                '& " GapDisplayUnits='" & EventGapDisplayUnits & "' " _
                '& " DurationDisplayUnits='" & EventDurationDisplayUnits & "' " _
            Else
                Return ""
            End If
        End Get
        Set(ByVal newValue As String)
            Dim lXMLDoc As New Xml.XmlDocument
            Try
                lXMLDoc.LoadXml(newValue)
                Dim lXML As Xml.XmlNode = lXMLDoc.FirstChild
                If lXML.Name.ToLower.Equals("events") Then
                    UseEvents = True
                    EventThreshold = GetAtt(lXML, "Threshold", EventThreshold)
                    EventHigh = GetAtt(lXML, "High", EventHigh)
                    IntensifyVolumeFraction = GetAtt(lXML, "FlashVolumeFraction", IntensifyVolumeFraction) 'supports old name
                    IntensifyVolumeFraction = GetAtt(lXML, "IntensifyVolumeFraction", IntensifyVolumeFraction)
                    If IntensifyVolumeFraction > 1 Then IntensifyVolumeFraction -= 1 'Backward compatible with files saved when this was centered at 100%
                    EventDaysGapAllowed = GetAtt(lXML, "GapDays", EventDaysGapAllowed)
                    'EventGapDisplayUnits = lXML.GetAttrValue("GapDisplayUnits")

                    EventVolumeHigh = GetAtt(lXML, "VolumeHigh", EventVolumeHigh)
                    EventVolumeThreshold = GetAtt(lXML, "VolumeThreshold", EventVolumeThreshold)

                    EventDurationHigh = GetAtt(lXML, "DurationHigh", EventDurationHigh)
                    EventDurationDays = GetAtt(lXML, "DurationDays", EventDurationDays)
                    'EventDurationDisplayUnits = lXML.GetAttrValue("DurationDisplayUnits")
                End If
            Catch e As Exception
                Logger.Msg("Could not read Events XML:" & vbCrLf & newValue & vbCrLf & e.Message, "CAT Events XML Problem")
            End Try
        End Set
    End Property

    Private Function GetAtt(ByVal aNode As Xml.XmlNode, ByVal aAttributeName As String, Optional ByVal aDefault As Object = "") As Object
        Dim lAttribute As Xml.XmlAttribute = aNode.Attributes.GetNamedItem(aAttributeName)
        If lAttribute IsNot Nothing Then
            Return lAttribute.InnerText
        Else
            Return aDefault
        End If
    End Function

    Protected Overridable Property SeasonsXML() As String
        Get
            If Seasons Is Nothing Then
                Return ""
            Else
                Return "  <Seasons Type='" & ToXML(Seasons.GetType.Name) & "'>" & vbCrLf _
                     & "  " & Seasons.SeasonsSelectedXML & "  </Seasons>" & vbCrLf
            End If
        End Get
        Set(ByVal newValue As String)
            Dim lXMLdoc As New Xml.XmlDocument
            Try
                lXMLdoc.LoadXml(newValue)
                Dim lXML As Xml.XmlNode = lXMLdoc.FirstChild
                If lXML.Name.ToLower.Equals("seasons") Then
                    Dim lSeasonTypeName As String = GetAtt(lXML, "Type")
                    For Each lSeasonType As Type In atcSeasons.atcSeasonPlugin.AllSeasonTypes
                        If lSeasonType.Name.Equals(lSeasonTypeName) Then
                            Seasons = lSeasonType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
                            If lXML.InnerXml.Contains("<") Then
                                Seasons.SeasonsSelectedXML = lXML.InnerXml
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                Logger.Msg("Unable to parse:" & vbCrLf & newValue, "CAT Seasons XML Problem")
            End Try
        End Set
    End Property

    Private Function GetDataGroupXML(ByVal aDataGroup As atcTimeseriesGroup, ByVal aTag As String) As String
        If aDataGroup Is Nothing OrElse aDataGroup.Count = 0 Then
            Return ""
        Else
            Dim lXML As String = "  <" & aTag & " count='" & aDataGroup.Count & "'>" & vbCrLf
            For lIndex As Integer = 0 To aDataGroup.Count - 1
                Dim lDataSet As atcDataSet = aDataGroup.Item(lIndex)
                Dim lDataKey As String = aDataGroup.Keys(lIndex)
                If Not lDataSet Is Nothing Then
                    lXML &= "    <DataSet"
                    lXML &= " ID='" & ToXML(lDataSet.Attributes.GetValue("ID")) & "'"
                    If lDataSet.Attributes.ContainsAttribute("History 1") Then
                        lXML &= " History='" & ToXML(lDataSet.Attributes.GetValue("History 1")) & "'"
                    End If
                    If lDataSet.Attributes.ContainsAttribute("Location") Then
                        lXML &= " Location='" & ToXML(lDataSet.Attributes.GetValue("Location")) & "'"
                    End If
                    If lDataSet.Attributes.ContainsAttribute("Constituent") Then
                        lXML &= " Constituent='" & ToXML(lDataSet.Attributes.GetValue("Constituent")) & "'"
                    End If
                    If lDataKey IsNot Nothing Then
                        lXML &= " Key='" & ToXML(lDataKey) & "'"
                    End If
                    lXML &= " />" & vbCrLf
                End If
            Next
            Return lXML & "  </" & aTag & ">" & vbCrLf
        End If
    End Function

    Private Sub SetDataGroupXML(ByRef aDataGroup As atcTimeseriesGroup, ByVal aTag As String, ByVal aXML As String)
        Dim lXMLdoc As New Xml.XmlDocument
        Try
            lXMLdoc.LoadXml(aXML)
            aDataGroup = New atcTimeseriesGroup
            For Each lXML As Xml.XmlNode In lXMLdoc.FirstChild.ChildNodes
                Dim lKey As String = GetAtt(lXML, "Key")
                Dim lID As String = GetAtt(lXML, "ID")
                Dim lHistory As String = GetAtt(lXML, "History")
                If lID.Length > 0 Then
                    Dim lDataGroup As atcTimeseriesGroup = Nothing
                    If lHistory.Length > 10 Then
                        Dim lSourceSpecification As String = lHistory.Substring(10).ToLower
                        Dim lDataSource As atcTimeseriesSource = atcDataManager.DataSourceBySpecification(lSourceSpecification)
                        If lDataSource IsNot Nothing Then
                            lDataGroup = lDataSource.DataSets.FindData("ID", lID, 2)
                            If lDataGroup.Count > 0 Then
                                Logger.Dbg("Found data set #" & lID & " in " & lSourceSpecification)
                            Else
                                lDataGroup = Nothing
                            End If
                        End If
                    End If
                    If lDataGroup Is Nothing Then
                        lDataGroup = atcDataManager.DataSets.FindData("ID", lID, 2)
                    End If
                    If lDataGroup.Count > 0 Then
                        Logger.Dbg("Found data set #" & lID & " without a specification")
                        If lDataGroup.Count > 1 Then Logger.Dbg("Warning: more than one data set matched ID " & lID)
                        aDataGroup.Add(lKey, lDataGroup.ItemByIndex(0))
                    Else
                        Logger.Msg("No data found with ID " & lID, "Variation from XML")
                    End If
                Else
                    If lKey Is Nothing OrElse lKey.Length = 0 Then
                        Logger.Dbg("No data set ID found in XML, skipping: ", lXML.OuterXml)
                    End If
                    aDataGroup.Add(lKey, Nothing)
                End If
            Next
        Catch e As Exception
            Logger.Msg("Unable to parse:" & vbCrLf & aXML & vbCrLf & e.Message, "CAT Data Group XML Problem")
        End Try
    End Sub

    Public Overridable Property XML() As String
        Get
            Dim lXML As String = "<Variation>" & vbCrLf _
                 & "  <Name>" & ToXML(Name) & "</Name>" & vbCrLf
            If PETelevation > Integer.MinValue Then
                lXML &= "  <Elevation>" & PETelevation & "</Elevation>" & vbCrLf
            End If
            If PETstationID > Integer.MinValue Then
                lXML &= "  <StationID>" & PETstationID & "</StationID>" & vbCrLf
            End If
            If Not Double.IsNaN(Min) Then
                lXML &= "  <Min>" & Min & "</Min>" & vbCrLf
            End If
            If Not Double.IsNaN(Max) Then
                lXML &= "  <Max>" & Max & "</Max>" & vbCrLf
            End If
            If Not Double.IsNaN(Increment) Then
                lXML &= "  <Increment>" & Increment & "</Increment>" & vbCrLf
            End If
            If IsInput Then
                lXML &= "  <IsInput>" & IsInput & "</IsInput>" & vbCrLf
            End If
            lXML &= "  <Operation>" & ToXML(Operation) & "</Operation>" & vbCrLf
            'lXML &= "  <AddRemovePer>" & AddRemovePer & "</AddRemovePer>" & vbCrLf
            If Not ComputationSource Is Nothing Then
                lXML &= "  <ComputationSource>" & ToXML(ComputationSource.Name) & "</ComputationSource>" & vbCrLf
            End If
            lXML &= "  <Selected>" & Selected & "</Selected>" & vbCrLf _
                 & GetDataGroupXML(DataSets, "DataSets") _
                 & GetDataGroupXML(PETtemperature, "PETtemperature") _
                 & GetDataGroupXML(PETprecipitation, "PETprecipitation") _
                 & EventsXML _
                 & SeasonsXML _
                 & "</Variation>" & vbCrLf
            Return lXML
        End Get
        Set(ByVal newValue As String)
            Dim lXMLdoc As New Xml.XmlDocument
            Try
                lXMLdoc.LoadXml(newValue)
                For Each lXML As Xml.XmlNode In lXMLdoc.FirstChild.ChildNodes
                    With lXML
                        Select Case .Name.ToLower
                            Case "name" : Name = .InnerText
                            Case "elevation" : PETelevation = CInt(.InnerText)
                            Case "stationid" : PETstationID = CInt(.InnerText)
                            Case "min" : Min = CDbl(.InnerText)
                            Case "max" : Max = CDbl(.InnerText)
                            Case "increment" : Increment = CDbl(.InnerText)
                            Case "isinput" : IsInput = CBool(.InnerText)
                            Case "operation"
                                Operation = .InnerText
                                If Operation = "Flash" Then Operation = "Intensify"
                                'Case "addremoveper" : AddRemovePer = .Content
                            Case "computationsource"
                                ComputationSource = atcDataManager.DataSourceByName(.InnerText)
                                If ComputationSource Is Nothing Then
                                    Select Case .InnerText
                                        Case "Timeseries::Math"
                                            ComputationSource = New atcTimeseriesMath.atcTimeseriesMath
                                        Case Else
                                            Logger.Msg("UnknownComputationSource " & .InnerText)
                                    End Select
                                End If
                            Case "datasets" : SetDataGroupXML(DataSets, "DataSets", .OuterXml)
                            Case "pettemperature" : SetDataGroupXML(PETtemperature, "PETtemperature", .OuterXml)
                            Case "petprecipitation" : SetDataGroupXML(PETprecipitation, "PETprecipitation", .OuterXml)
                            Case "selected"
                                Selected = .InnerText.ToLower.Equals("true")
                            Case "seasons" : SeasonsXML = .OuterXml
                            Case "events" : EventsXML = .OuterXml
                        End Select
                    End With
                Next
            Catch e As Exception
                Logger.Msg("Unable to parse:" & vbCrLf & newValue, "CAT Variation XML Problem")
            End Try
        End Set
    End Property

    Public Overrides Function ToString() As String
        Dim lString As String = Name & " " & Operation & " "

        If Max <= Min Then
            lString &= DoubleString(Min)
        Else
            If Not Double.IsNaN(Min) Then lString &= "from " & DoubleString(Min)
            If Not Double.IsNaN(Max) Then lString &= " to " & DoubleString(Max)
            If Not Double.IsNaN(Increment) Then lString &= " step " & DoubleString(Increment)
        End If
        If Seasons IsNot Nothing Then
            lString &= " " & atcSeasons.atcSeasonPlugin.SeasonClassNameToLabel(Seasons.GetType.Name) _
                   & ": " & Seasons.SeasonsSelectedString
        End If
        Return lString
    End Function

    Private Function DoubleString(ByVal aNumber As Double) As String
        Dim lStr As String = Format(aNumber, "0.000")
        Dim lDecimalPos As Integer = lStr.IndexOf("."c)
        If lDecimalPos >= 0 Then
            'Trim trailing zeroes after decimal point
            lStr = lStr.TrimEnd("0"c)
            'Trim trailing decimal point
            If lStr.Length = lDecimalPos + 1 Then lStr = lStr.Substring(0, lDecimalPos)
        End If
        If lStr.Length = 0 Then lStr = "0"
        Return lStr
    End Function

    Public Sub New()
        Clear()
    End Sub

    Protected Overrides Sub Finalize()
        If pDataSets IsNot Nothing Then
            pDataSets.Dispose()
            pDataSets = Nothing
        End If
        If PETtemperature IsNot Nothing Then
            PETtemperature.Dispose()
            PETtemperature = Nothing
        End If
        If PETprecipitation IsNot Nothing Then
            PETprecipitation.Dispose()
            PETprecipitation = Nothing
        End If
        pComputationSource = Nothing
        Seasons = Nothing
        MyBase.Finalize()
    End Sub
End Class
