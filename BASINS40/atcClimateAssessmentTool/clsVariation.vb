Imports atcData
Imports atcEvents
Imports MapWinUtility

Public Class Variation

    'Parameters for Hammon - TODO: don't hard code these
    Private pDegF As Boolean = True
    Private pLatDeg As Double = 39
    'WestBranch of Patux
    'Private pCTS() As Double = {0, 0.0045, 0.01, 0.01, 0.01, 0.0085, 0.0085, 0.0085, 0.0085, 0.0085, 0.0095, 0.0095, 0.0095}
    'Monocacy - CBP
    Private pCTS() As Double = {0, 0.0057, 0.0057, 0.0057, 0.0057, 0.0057, 0.0057, _
                                   0.0057, 0.0057, 0.0057, 0.0057, 0.0057, 0.0057}

    Private pName As String = "<untitled>"
    Private pDataSets As atcDataGroup
    Private pComputationSource As atcDataSource
    Private pOperation As String = ""
    Private pSelected As Boolean = False

    'TODO: make rest of public variables into properties
    Public Seasons As atcSeasons.atcSeasonBase
    Public Min As Double = Double.NaN
    Public Max As Double = Double.NaN
    Public Increment As Double = Double.NaN
    Private pIncrementsSinceStart As Integer = 0
    Public CurrentValue As Double = Double.NaN

    Public UseEvents As Boolean = False
    Public EventThreshold As Double = Double.NaN
    Public EventDaysGapAllowed As Double = 0
    Public EventGapDisplayUnits As String = ""
    Public EventHigh As Boolean = True
    Public EventsPer As String = ""

    Public IsInput As Boolean = False

    Public ColorAboveMax As System.Drawing.Color = System.Drawing.Color.OrangeRed
    Public ColorBelowMin As System.Drawing.Color = System.Drawing.Color.DeepSkyBlue
    Public ColorDefault As System.Drawing.Color = System.Drawing.Color.White

    Public PETdata As atcDataGroup = New atcDataGroup

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
        Dim lMetCmp As New atcMetCmp.atcMetCmpPlugin
        Dim lArgsMath As New atcDataAttributes
        Dim lModifiedTS As atcTimeseries
        Dim lModifiedGroup As New atcDataGroup
        Dim lDataSetIndex As Integer = 0

        Dim lEvents As atcDataGroup = Nothing
        Dim lEvent As atcTimeseries

        Dim lModifyThis As atcTimeseries

        For Each lOriginalData As atcDataSet In DataSets
            If UseEvents Then
                lEvents = EventSplit(lOriginalData, Nothing, EventThreshold, EventDaysGapAllowed, EventHigh)

                'Remove events outside selected seasons
                If Not Seasons Is Nothing Then
                    Dim lValueIndex As Integer
                    For lEventIndex As Integer = lEvents.Count - 1 To 0 Step -1
                        lEvent = lEvents.ItemByIndex(lEventIndex)

                        Dim lPeakIndex As Integer = 1
                        For lValueIndex = 2 To lEvent.numValues
                            If lEvent.Value(lValueIndex) > lEvent.Value(lPeakIndex) Then
                                lPeakIndex = lValueIndex
                            End If
                        Next

                        If Not Seasons.SeasonSelected(Seasons.SeasonIndex(lEvent.Dates.Value(lPeakIndex))) Then
                            lEvents.RemoveAt(lEventIndex)
                        End If
                    Next
                End If
                lModifyThis = MergeTimeseries(lEvents)
            Else
                lModifyThis = lOriginalData
            End If

            If Seasons Is Nothing OrElse UseEvents Then
                Select Case Operation
                    Case "AddEvents"
                        lModifiedTS = AddRemoveEventsTotalVolume(lOriginalData, Math.Abs(CurrentValue), lEvents, 0)
                    Case "AddVolume"
                        'TODO
                    Case Else '"Add", "Multiply"
                        ComputationSource.DataSets.Clear()
                        lArgsMath.Clear()
                        lArgsMath.SetValue("timeseries", lModifyThis)
                        lArgsMath.SetValue("Number", CurrentValue)
                        ComputationSource.Open(Operation, lArgsMath)
                        lModifiedTS = ComputationSource.DataSets(0)
                End Select
            Else
                Dim lSplitData As atcDataGroup = Seasons.Split(lModifyThis, Nothing)
                Dim lModifiedSplit As New atcDataGroup
                For Each lSplitTS As atcTimeseries In lSplitData
                    If Seasons.SeasonSelected(lSplitTS.Attributes.GetValue("SeasonIndex")) Then
                        'modify data from this season
                        ComputationSource.DataSets.Clear()
                        lArgsMath.Clear()
                        lArgsMath.SetValue("timeseries", lSplitTS)
                        lArgsMath.SetValue("Number", CurrentValue)
                        ComputationSource.Open(Operation, lArgsMath)
                        lModifiedSplit.Add(ComputationSource.DataSets(0))
                    Else 'Add unmodified data from this season that was not selected
                        lModifiedSplit.Add(lSplitTS)
                    End If
                Next
                lModifiedTS = MergeTimeseries(lModifiedSplit)
            End If

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

        lNewTimeseries.Attributes.AddHistory(lOperation & lFoundIndexes.Count & " events to change total volume " & DoubleString(aTargetVolumeChange) _
            & " (actual change = " & DoubleString(lNewTimeseries.Attributes.GetValue("Sum") - aTimeseries.Attributes.GetValue("Sum")) & ")")

        Return lNewTimeseries

    End Function

    Public Overridable Function Clone() As Variation
        Dim newVariation As New Variation
        Me.CopyTo(newVariation)
        Return newVariation
    End Function

    Public Overridable Sub CopyTo(ByVal aTargetVariation As Variation)
        With aTargetVariation
            .Name = Name
            .UseEvents = UseEvents
            If UseEvents Then
                .EventDaysGapAllowed = EventDaysGapAllowed
                .EventGapDisplayUnits = EventGapDisplayUnits
                .EventHigh = EventHigh
                .EventsPer = EventsPer
                .EventThreshold = EventThreshold
            End If

            If Not DataSets Is Nothing Then .DataSets = DataSets.Clone()
            If Not PETdata Is Nothing Then .PETdata = PETdata.Clone()
            .ComputationSource = ComputationSource
            .Operation = Operation.Clone()
            .Seasons = Seasons 'TODO: clone Seasons of not Nothing
            .Selected = Selected
            .Min = Min
            .Max = Max
            .Increment = Increment
            .EventsPer = EventsPer
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
                              & " GapDays='" & EventDaysGapAllowed & "' " _
                              & " GapDisplayUnits='" & EventGapDisplayUnits & "' " _
                              & " EventsPer='" & EventsPer & "'/>" & vbCrLf
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
                    EventDaysGapAllowed = lXML.GetAttrValue("GapDays")
                    EventGapDisplayUnits = lXML.GetAttrValue("GapDisplayUnits")
                    EventsPer = lXML.GetAttrValue("EventsPer")
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
                            For Each lDataSource As atcDataSource In g_DataManager.DataSources
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
                            lDataGroup = g_DataManager.DataSets.FindData("ID", lID, 2)
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
                                Case "computationsource"
                                    ComputationSource = g_DataManager.DataSourceByName(.Content)
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
        Dim retStr As String = Name & " " & Operation

        If Not Double.IsNaN(Min) Then retStr &= " from " & DoubleString(Min)
        If Not Double.IsNaN(Max) Then retStr &= " to " & DoubleString(Max)
        If Not Double.IsNaN(Increment) Then retStr &= " step " & DoubleString(Increment)
        If Not Seasons Is Nothing Then
            retStr &= " " & atcSeasons.atcSeasonPlugin.SeasonClassNameToLabel(Seasons.GetType.Name) _
                   & ": " & Seasons.SeasonsSelectedString
        End If
        Return retStr
    End Function

    Private Function DoubleString(ByVal aNumber As Double) As String
        DoubleString = Format(aNumber, "0.000").TrimEnd("0"c, "."c)
        If DoubleString.Length = 0 Then DoubleString = "0"
    End Function
End Class
