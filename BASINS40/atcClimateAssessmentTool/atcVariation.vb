Imports atcData
Imports atcEvents
Imports atcUtility
Imports MapWinUtility

Public Class atcVariation
    Private pNaN As Double = atcUtility.GetNaN

    'Parameters for Hammon
    Private pDegF As Boolean
    Private pLatDeg As Double

    'WestBranch of Patux
    'Private pCTS() As Double = {0, 0.0045, 0.01, 0.01, 0.01, 0.0085, 0.0085, 0.0085, 0.0085, 0.0085, 0.0095, 0.0095, 0.0095}
    'Monocacy - CBP
    Private pCTS() As Double = {0, 0.0057, 0.0057, 0.0057, 0.0057, 0.0057, 0.0057, _
                                   0.0057, 0.0057, 0.0057, 0.0057, 0.0057, 0.0057}

    Private pName As String
    Private pDataSets As atcDataGroup
    Public PETdata As atcDataGroup
    Private pComputationSource As atcDataSource
    Private pOperation As String
    'Public AddRemovePer As String
    Private pSelected As Boolean

    'TODO: make rest of public variables into properties
    Public Seasons As atcSeasons.atcSeasonBase
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

    Public FlashVolumeFraction As Double

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

    Public Overridable Property DataSets() As atcDataGroup
        Get
            Return pDataSets
        End Get
        Set(ByVal newValue As atcDataGroup)
            pDataSets = newValue
        End Set
    End Property

    Public Overridable Property ComputationSource() As atcDataSource
        Get
            Return pComputationSource
        End Get
        Set(ByVal newValue As atcDataSource)
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
            Try
                Return (Max - Min) / Increment + 1
            Catch ex As Exception
                Return 1
            End Try
        End Get
    End Property

    Public Overridable Function StartIteration() As atcDataGroup
        Me.CurrentValue = Me.Min
        pIncrementsSinceStart = 0
        Return VaryData()
    End Function

    Public Overridable Function NextIteration() As atcDataGroup
        pIncrementsSinceStart += 1
        If pIncrementsSinceStart < Iterations Then
            Me.CurrentValue = Me.Min + Me.Increment * pIncrementsSinceStart
            Return VaryData()
        Else
            Return Nothing
        End If
    End Function

    Protected Overridable Function VaryData() As atcDataGroup
        'Dim lMetCmp As New atcMetCmp.atcMetCmpPlugin
        Dim lArgsMath As New atcDataAttributes
        Dim lModifiedTS As atcTimeseries = Nothing
        Dim lModifiedGroup As New atcDataGroup
        Dim lDataSetIndex As Integer = 0
        Dim lValueIndex As Integer

        Dim lEvents As atcDataGroup = Nothing
        Dim lEvent As atcTimeseries

        Dim lModifyThis As atcTimeseries

        For Each lOriginalData As atcDataSet In DataSets
            Dim lSplitData As atcDataGroup = Nothing
            If UseEvents Then
                lEvents = EventSplit(lOriginalData, Nothing, EventThreshold, EventDaysGapAllowed, EventHigh)

                'Remove events outside selected seasons
                If Not Seasons Is Nothing Then
                    For lEventIndex As Integer = lEvents.Count - 1 To 0 Step -1
                        lEvent = lEvents.ItemByIndex(lEventIndex)

                        'Find peak value of event
                        Dim lPeakIndex As Integer = 1
                        For lValueIndex = 2 To lEvent.numValues
                            If lEvent.Value(lValueIndex) > lEvent.Value(lPeakIndex) Then
                                lPeakIndex = lValueIndex
                            End If
                        Next

                        'If peak is not in season, remove this event
                        If Not Seasons.SeasonSelected(Seasons.SeasonIndex(lEvent.Dates.Value(lPeakIndex))) Then
                            lEvents.RemoveAt(lEventIndex)
                        End If
                    Next
                End If

                'Remove events outside target volume threshold
                Try
                    If Not Double.IsNaN(EventVolumeThreshold) Then
                        For lEventIndex As Integer = lEvents.Count - 1 To 0 Step -1
                            Dim lEventVolume As Double = lEvents.ItemByIndex(lEventIndex).Attributes.GetValue("Sum")
                            If EventVolumeHigh Then
                                If lEventVolume < EventVolumeThreshold Then
                                    lEvents.RemoveAt(lEventIndex)
                                End If
                            Else
                                If lEventVolume > EventVolumeThreshold Then
                                    lEvents.RemoveAt(lEventIndex)
                                End If
                            End If

                        Next
                    End If
                Catch e As Exception
                    Logger.Dbg("VaryDataException-EventVolumeThreshold " & e.Message)
                End Try

                'Remove events outside target duration threshold
                If Not Double.IsNaN(EventDurationDays) Then
                    For lEventIndex As Integer = lEvents.Count - 1 To 0 Step -1
                        Dim lEventDuration As Double = atcSynopticAnalysis.atcSynopticAnalysisPlugin.DataSetDuration(lEvents.ItemByIndex(lEventIndex))
                        If EventDurationHigh Then
                            If lEventDuration < EventDurationDays Then
                                lEvents.RemoveAt(lEventIndex)
                            End If
                        Else
                            If lEventDuration > EventDurationDays Then
                                lEvents.RemoveAt(lEventIndex)
                            End If
                        End If

                    Next
                End If

                If Operation <> "Flash" Then
                    lModifyThis = MergeTimeseries(lEvents)
                    lSplitData = New atcDataGroup(lModifyThis)
                    lSplitData.Add(lOriginalData)
                End If

            Else
                lModifyThis = lOriginalData
                If Seasons Is Nothing Then
                    lSplitData = New atcDataGroup(lModifyThis)
                Else
                    lSplitData = Seasons.SplitBySelected(lModifyThis, Nothing)
                End If
            End If

            Dim lModifiedSplit As New atcDataGroup

            Select Case Operation
                Case "AddEvents"
                    'if there are seasons, modify data only from selected seasons
                    Dim lSplitOriginalData As atcDataGroup
                    If Seasons Is Nothing Then
                        lSplitOriginalData = New atcDataGroup(lOriginalData)
                    Else
                        lSplitOriginalData = Seasons.SplitBySelected(lOriginalData, Nothing)
                    End If

                    Dim lSplitTS As atcTimeseries = lSplitOriginalData.ItemByIndex(0)

                    lModifiedSplit.Add(AddRemoveEventsVolumeFraction(lSplitTS, CurrentValue, lEvents, 0))
                    If lSplitOriginalData.Count > 0 Then
                        lModifiedSplit.Add(lSplitOriginalData.ItemByIndex(1))
                    End If

                    'TODO Case "AddVolume"
                Case "Flash"
                    'if there are seasons, modify data only from selected seasons
                    Dim lSplitOriginalData As atcDataGroup
                    If Seasons Is Nothing Then
                        lSplitOriginalData = New atcDataGroup(lOriginalData)
                    Else
                        lSplitOriginalData = Seasons.SplitBySelected(lOriginalData, Nothing)
                    End If
                    Dim lTotalVolume As Double = lSplitOriginalData.ItemByIndex(0).Attributes.GetValue("Sum")
                    Dim lEventFlashFactor As Double
                    Dim lCurrentVolume As Double = 0
                    Dim lTargetChange As Double = CurrentValue * lTotalVolume
                    Dim lNewEventTotalVolume As Double = 0.0

                    Try
                        If Not Double.IsNaN(FlashVolumeFraction) Then
                            Dim lTargetVolumeToFlash As Double = lTotalVolume * FlashVolumeFraction
                            Logger.Dbg("TargetChange " & DecimalAlign(lTargetChange) & " TargetVolumeToFlash " & DecimalAlign(lTargetVolumeToFlash))
                            'sort events by volume
                            Logger.Dbg("Flash " & DecimalAlign(FlashVolumeFraction) & " CurrentValue " & DecimalAlign(CurrentValue))
                            Dim lNewEvents As New System.Collections.SortedList(lEvents.Count)
                            For Each lEvent In lEvents
                                Dim lEventVolume As Double = lEvent.Attributes.GetValue("Sum")
tryAgain:
                                Try
                                    lNewEvents.Add(lEventVolume, lEvent)
                                    lNewEventTotalVolume += lEventVolume
                                Catch
                                    lEventVolume += 0.00000001
                                    GoTo tryAgain
                                End Try
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
                                    If lCurrentVolume > lTargetVolumeToFlash Then
                                        Logger.Dbg("Flash " & lNewEvents.Count - lEventIndex & " of " & lNewEvents.Count)
                                        Exit For
                                    End If
                                Next lEventIndex
                            Else
                                For lEventIndex As Integer = 0 To lNewEvents.Count - 1
                                    lEvent = lNewEvents.GetByIndex(lEventIndex)
                                    lCurrentVolume += lNewEvents.GetKey(lEventIndex)
                                    ' Logger.Dbg("CurrentVolumeRemoved " & lCurrentVolumeChange)
                                    lEvents.Add(lEvent)
                                    If lCurrentVolume > lTargetVolumeToFlash Then
                                        Logger.Dbg("NegativeFlash " & lEventIndex & " of " & lNewEvents.Count)
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    Catch e As Exception
                        Logger.Dbg("VaryDataException-EventFlashFactor " & e.Message)
                    End Try

                    Logger.Dbg(" CurrentVolume " & DecimalAlign(lCurrentVolume) & _
                               " TargetChange " & DecimalAlign(lTargetChange))
                    lEventFlashFactor = lTargetChange / lCurrentVolume
                    Logger.Dbg("EventFlashFactor " & DecimalAlign(lEventFlashFactor))
                    lModifyThis = MergeTimeseries(lEvents)
                    lSplitData = New atcDataGroup(lModifyThis)
                    lSplitData.Add(lOriginalData)

                    Dim lSplitTS As atcTimeseries = lSplitData.ItemByIndex(0)
                    ComputationSource.DataSets.Clear()
                    lArgsMath.Clear()
                    lArgsMath.SetValue("timeseries", lSplitTS)
                    lArgsMath.SetValue("Number", 1 + lEventFlashFactor)
                    ComputationSource.Open("Multiply", lArgsMath)
                    lModifiedTS = ComputationSource.DataSets(0)
                    lModifiedSplit.Add(ComputationSource.DataSets(0))
                    If lSplitData.Count > 0 Then
                        lModifiedSplit.Add(lSplitData.ItemByIndex(1))
                    End If

                Case Else '"Add", "Multiply"
                    Dim lSplitTS As atcTimeseries = lSplitData.ItemByIndex(0)
                    ComputationSource.DataSets.Clear()
                    lArgsMath.Clear()
                    lArgsMath.SetValue("timeseries", lSplitTS)
                    lArgsMath.SetValue("Number", CurrentValue)
                    ComputationSource.Open(Operation, lArgsMath)
                    lModifiedTS = ComputationSource.DataSets(0)
                    lModifiedSplit.Add(ComputationSource.DataSets(0))
                    If lSplitData.Count > 1 Then
                        lModifiedSplit.Add(lSplitData.ItemByIndex(1))
                    End If
            End Select

            Select Case lModifiedSplit.Count
                Case 0
                    Throw New ApplicationException("VaryData: No data computed")
                Case 1
                    lModifiedTS = lModifiedSplit.Item(0)
                Case Is > 1
                    lModifiedTS = MergeTimeseries(lModifiedSplit)
            End Select

            lModifiedGroup.Add(lModifiedTS)

            If PETdata.Count > lDataSetIndex Then
                Dim lOldPET As atcDataSet = PETdata(lDataSetIndex)
                pLatDeg = lOldPET.Attributes.GetValue("LatDeg", pLatDeg)
                Dim lNewPET As atcDataSet = atcMetCmp.CmpHamX(lModifiedTS, Nothing, pDegF, pLatDeg, pCTS)
                If lOldPET.Attributes.GetValue("tu") < 4 Then
                    lNewPET = atcMetCmp.DisSolPet(lNewPET, Nothing, 2, pLatDeg)
                End If
                With lNewPET.Attributes
                    .SetValue("Location", lOldPET.Attributes.GetValue("Location"))
                    .SetValue("Constituent", lOldPET.Attributes.GetValue("Constituent"))
                    .SetValue("History 1", lOldPET.Attributes.GetValue("History 1").ToString)
                    .SetValue("Id", lOldPET.Attributes.GetValue("Id"))
                End With
                lModifiedGroup.Add(lNewPET)
            End If
            lDataSetIndex += 1
        Next
        Return lModifiedGroup
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
                                                   ByVal aEventsToSearch As atcDataGroup, _
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

    Private Function AddRemoveEventsTotalVolume(ByVal aTimeseries As atcTimeseries, ByVal aTargetVolumeChange As Double, ByVal aEventsToSearch As atcDataGroup, ByVal aSeed As Integer) As atcTimeseries
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

        'Parameters for Hammon - TODO: don't hard code these
        pDegF = True
        pLatDeg = 39

        pName = "<untitled>"
        pDataSets = New atcDataGroup
        pComputationSource = Nothing
        pOperation = ""
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
        FlashVolumeFraction = pNaN
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

        PETdata = New atcDataGroup
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
                .FlashVolumeFraction = FlashVolumeFraction
            End If

            If Not DataSets Is Nothing Then .DataSets = DataSets.Clone()
            If Not PETdata Is Nothing Then .PETdata = PETdata.Clone()
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
                              & " FlashVolumeFraction='" & FlashVolumeFraction & "' " _
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
            Dim lXML As New Chilkat.Xml
            If lXML.LoadXml(newValue) Then
                If lXML.Tag.ToLower.Equals("events") Then
                    UseEvents = True
                    EventThreshold = lXML.GetAttrValue("Threshold")
                    EventHigh = lXML.GetAttrValue("High")
                    FlashVolumeFraction = lXML.GetAttrValue("FlashVolumeFraction")
                    EventDaysGapAllowed = lXML.GetAttrValue("GapDays")
                    'EventGapDisplayUnits = lXML.GetAttrValue("GapDisplayUnits")

                    EventVolumeHigh = lXML.GetAttrValue("VolumeHigh")
                    EventVolumeThreshold = lXML.GetAttrValue("VolumeThreshold")

                    EventDurationHigh = lXML.GetAttrValue("DurationHigh")
                    EventDurationDays = lXML.GetAttrValue("DurationDays")
                    'EventDurationDisplayUnits = lXML.GetAttrValue("DurationDisplayUnits")
                End If
            End If
        End Set
    End Property

    Protected Overridable Property SeasonsXML() As String
        Get
            If Seasons Is Nothing Then
                Return ""
            Else
                Return "  <Seasons Type='" & Seasons.GetType.Name & "'>" & vbCrLf _
                     & "  " & Seasons.SeasonsSelectedXML & "  </Seasons>" & vbCrLf
            End If
        End Get
        Set(ByVal newValue As String)
            Dim lXML As New Chilkat.Xml
            If lXML.LoadXml(newValue) Then
                If lXML.Tag.ToLower.Equals("seasons") Then
                    Dim lSeasonTypeName As String = lXML.GetAttrValue("Type")
                    For Each lSeasonType As Type In atcSeasons.atcSeasonPlugin.AllSeasonTypes
                        If lSeasonType.Name.Equals(lSeasonTypeName) Then
                            Seasons = lSeasonType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
                            If lXML.FirstChild2 Then
                                Seasons.SeasonsSelectedXML = lXML.GetXml
                            End If
                        End If
                    Next
                End If
            End If
        End Set
    End Property

    Private Function GetDataGroupXML(ByVal aDataGroup As atcDataGroup, ByVal aTag As String) As String
        If aDataGroup Is Nothing Then
            Return ""
        Else
            Dim lXML As String = "  <" & aTag & " count='" & aDataGroup.Count & "'>" & vbCrLf
            For lIndex As Integer = 0 To aDataGroup.Count - 1
                Dim lDataSet As atcDataSet = aDataGroup.Item(lIndex)
                Dim lDataKey As String = aDataGroup.Keys(lIndex)
                If Not lDataSet Is Nothing Then
                    lXML &= "    <DataSet"
                    lXML &= " ID='" & lDataSet.Attributes.GetValue("ID") & "'"
                    If lDataSet.Attributes.ContainsAttribute("History 1") Then
                        lXML &= " History='" & lDataSet.Attributes.GetValue("History 1") & "'"
                    End If
                    If lDataSet.Attributes.ContainsAttribute("Location") Then
                        lXML &= " Location='" & lDataSet.Attributes.GetValue("Location") & "'"
                    End If
                    If lDataSet.Attributes.ContainsAttribute("Constituent") Then
                        lXML &= " Constituent='" & lDataSet.Attributes.GetValue("Constituent") & "'"
                    End If
                    If Not lDataKey Is Nothing Then
                        lXML &= " Key='" & lDataKey & "'"
                    End If
                    lXML &= " />" & vbCrLf
                End If
            Next
            Return lXML & "  </" & aTag & ">" & vbCrLf
        End If
    End Function

    Private Sub SetDataGroupXML(ByRef aDataGroup As atcDataGroup, ByVal aTag As String, ByVal aXML As String)
        Dim lXML As New Chilkat.Xml
        If lXML.LoadXml(aXML) Then
            aDataGroup = New atcDataGroup
            If lXML.FirstChild2() Then
                Do
                    Dim lKey As String = lXML.GetAttrValue("Key")
                    Dim lID As String = lXML.GetAttrValue("ID")
                    Dim lHistory As String = lXML.GetAttrValue("History")
                    If lID.Length > 0 Then
                        Dim lDataGroup As atcDataGroup = Nothing
                        If lHistory.Length > 10 Then
                            Dim lSourceSpecification As String = lHistory.Substring(10).ToLower
                            For Each lDataSource As atcDataSource In atcDataManager.DataSources
                                If lDataSource.Specification.ToLower.Equals(lSourceSpecification) Then
                                    lDataGroup = lDataSource.DataSets.FindData("ID", lID, 2)
                                    If lDataGroup.Count > 0 Then
                                        Logger.Dbg("Found data set #" & lID & " in " & lSourceSpecification)
                                        Exit For
                                    Else
                                        lDataGroup = Nothing
                                    End If
                                End If
                            Next
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
                            Logger.Dbg("No data set ID found in XML, skipping: ", lXML.GetXml)
                        End If
                        aDataGroup.Add(lKey, Nothing)
                    End If
                Loop While lXML.NextSibling2
            End If
        End If
    End Sub

    Public Overridable Property XML() As String
        Get
            Dim lXML As String = "<Variation>" & vbCrLf _
                 & "  <Name>" & Name & "</Name>" & vbCrLf
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
            lXML &= "  <Operation>" & Operation & "</Operation>" & vbCrLf
            'lXML &= "  <AddRemovePer>" & AddRemovePer & "</AddRemovePer>" & vbCrLf
            If Not ComputationSource Is Nothing Then
                lXML &= "  <ComputationSource>" & ComputationSource.Name & "</ComputationSource>" & vbCrLf
            End If
            lXML &= "  <Selected>" & Selected & "</Selected>" & vbCrLf _
                 & GetDataGroupXML(DataSets, "DataSets") _
                 & GetDataGroupXML(PETdata, "PETdata") _
                 & EventsXML _
                 & SeasonsXML _
                 & "</Variation>" & vbCrLf
            Return lXML
        End Get
        Set(ByVal Value As String)
            Dim lXML As New Chilkat.Xml
            If lXML.LoadXml(Value) Then
                If lXML.FirstChild2() Then
                    Do
                        With lXML
                            Select Case .Tag.ToLower
                                Case "name" : Name = .Content
                                Case "min" : Min = CDbl(.Content)
                                Case "max" : Max = CDbl(.Content)
                                Case "increment" : Increment = CDbl(.Content)
                                Case "isinput" : IsInput = CBool(.Content)
                                Case "operation" : Operation = .Content
                                    'Case "addremoveper" : AddRemovePer = .Content
                                Case "computationsource"
                                    ComputationSource = atcDataManager.DataSourceByName(.Content)
                                Case "datasets" : SetDataGroupXML(DataSets, "DataSets", .GetXml)
                                Case "petdata" : SetDataGroupXML(PETdata, "PETdata", .GetXml)
                                Case "selected"
                                    Selected = .Content.ToLower.Equals("true")
                                Case "seasons" : SeasonsXML = .GetXml
                                Case "events" : EventsXML = .GetXml
                            End Select
                        End With
                    Loop While lXML.NextSibling2
                End If
            End If
        End Set
    End Property

    Public Overrides Function ToString() As String
        Dim retStr As String = Name & " " & Operation & " "

        If Max <= Min Then
            retStr &= DoubleString(Min)
        Else
            If Not Double.IsNaN(Min) Then retStr &= "from " & DoubleString(Min)
            If Not Double.IsNaN(Max) Then retStr &= " to " & DoubleString(Max)
            If Not Double.IsNaN(Increment) Then retStr &= " step " & DoubleString(Increment)
        End If
        If Not Seasons Is Nothing Then
            retStr &= " " & atcSeasons.atcSeasonPlugin.SeasonClassNameToLabel(Seasons.GetType.Name) _
                   & ": " & Seasons.SeasonsSelectedString
        End If
        Return retStr
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
        If Not pDataSets Is Nothing Then pDataSets.Clear()
        If Not PETdata Is Nothing Then PETdata.Clear()
        pComputationSource = Nothing
        Seasons = Nothing
        MyBase.Finalize()
    End Sub
End Class
