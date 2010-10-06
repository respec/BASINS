Imports atcUtility
Imports atcData
Imports atcUCI
Imports MapWinUtility

Public Module WatershedSummaryOverland

    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aConstituentType As String, _
                           ByVal aOperationTypes As atcCollection, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcTimeseriesSource, _
                           ByVal aRunMade As String, _
                           ByVal aPerlndSegmentStarts() As Integer, _
                           ByVal aImplndSegmentStarts() As Integer, _
                  Optional ByVal aEachYear As Boolean = True, _
                  Optional ByVal aSummary As Boolean = True, _
                  Optional ByVal aIncludeMinMax As Boolean = True, _
                  Optional ByVal aWaterYears As Boolean = False, _
                  Optional ByVal aIdsPerSeg As Integer = 50) As atcReport.IReport

        Dim lNumberFormat As String = "#,##0.000"
        Dim lUnits As String = ""
        Dim lAllNonpointData As New atcTimeseriesGroup

        Dim lPerlndOperations As HspfOperations = aUci.OpnBlks("PERLND").Ids
        Dim lPerlndLastId As Integer = 0
        For Each lPerlndOperation As HspfOperation In lPerlndOperations
            If lPerlndOperation.Id > lPerlndLastId Then
                lPerlndLastId = lPerlndOperation.Id
            End If
        Next
        Dim lImplndOperations As HspfOperations = aUci.OpnBlks("IMPLND").Ids
        Dim lImplndLastId As Integer = 0
        For Each lImplndOperation As HspfOperation In lImplndOperations
            If lImplndOperation.Id > lImplndLastId Then
                lImplndLastId = lImplndOperation.Id
            End If
        Next
        'Dim lPerlndFirstId As Integer
        'Dim lPerlndLastId As Integer
        'Dim lImplndFirstId As Integer
        'Dim lImplndLastId As Integer
        Dim lPerlndColumns As ArrayList = Nothing 'Column titles for PERLND
        Dim lImplndColumns As ArrayList = Nothing 'Column titles for IMPLND
        'Dim lNumUniquePerlnd As Integer = NumUniqueOperations(lPerlndOperations)
        'Dim lNumUniqueImplnd As Integer = NumUniqueOperations(lImplndOperations)
        'Dim lRepeatPerlnd As Integer = OperationsRepeatInterval(lPerlndOperations)
        'Dim lRepeatImplnd As Integer = OperationsRepeatInterval(lImplndOperations)
        Dim lOperationIndex As Integer
        Dim lSeasonName As String

        'MinMaxID(lPerlndOperations, lPerlndFirstId, lPerlndLastId)
        'MinMaxID(lImplndOperations, lImplndFirstId, lImplndLastId)

        Dim lSegment As Integer
        Dim lSegmentImplnd As Integer

        Dim lPerlndUniqueIds As ArrayList = Nothing
        FindSegmentStarts(lPerlndOperations, aPerlndSegmentStarts, lPerlndColumns, lPerlndUniqueIds)
        lPerlndLastId = lPerlndLastId Mod aIdsPerSeg  '+ aPerlndSegmentStarts(0)
        Dim lImplndUniqueIds As ArrayList = Nothing
        FindSegmentStarts(lImplndOperations, aImplndSegmentStarts, lImplndColumns, lImplndUniqueIds)
        lImplndLastId = lImplndLastId Mod aIdsPerSeg '+ aImplndSegmentStarts(0)

        Select Case aConstituentType
            Case "Sediment"
                lUnits = "(tons/acre)"
                lAllNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOSED"))
                lAllNonpointData.AddRange(aScenarioResults.DataSets.FindData("Constituent", "SOSLD"))
            Case Else
                Return New atcReport.ReportText("Overland report not yet defined for balance type '" & aConstituentType & "'")
        End Select

        Dim lAllYearsToDo As New Generic.Dictionary(Of String, atcTimeseriesGroup)
        If aEachYear Then
            Dim lSplitter As atcSeasonBase
            If aWaterYears Then
                lSplitter = New atcSeasonsWaterYear
            Else
                lSplitter = New atcSeasonsCalendarYear
            End If
            For Each lOriginalTimeseries As atcTimeseries In lAllNonpointData
                For Each lYearlyTs As atcTimeseries In lSplitter.Split(lOriginalTimeseries, Nothing)
                    lSeasonName = lYearlyTs.Attributes.GetValue("SeasonName", "")
                    Dim lDataGroup As atcTimeseriesGroup
                    If lAllYearsToDo.ContainsKey(lSeasonName) Then
                        lDataGroup = lAllYearsToDo.Item(lSeasonName)
                    Else
                        lDataGroup = New atcTimeseriesGroup
                        lAllYearsToDo.Add(lSeasonName, lDataGroup)
                    End If
                    lDataGroup.Add(lYearlyTs)
                Next
            Next
        End If

        If aSummary Then lAllYearsToDo.Add("Summary", lAllNonpointData)

        Dim lReport As New atcReport.ReportText
        Dim lSJDate As Double = aUci.GlobalBlock.SDateJ
        For Each lCurrentNonpointData As atcTimeseriesGroup In lAllYearsToDo.Values
            lSeasonName = lCurrentNonpointData.ItemByIndex(0).Attributes.GetValue("SeasonName", "")
            lReport.AppendLine("Overland Summary Report for " & aConstituentType & " in " & aScenario & " " & lUnits)
            lReport.AppendLine("   Run Made " & aRunMade)
            lReport.AppendLine("   Results From File " & aScenarioResults.Specification)
            lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)

            Dim lCurrentIsAll As Boolean = lCurrentNonpointData.Equals(lAllNonpointData)
            Dim lSummary As Boolean = aIncludeMinMax AndAlso lCurrentIsAll
            If lSummary OrElse lCurrentIsAll Then
                lReport.AppendLine("   " & aUci.GlobalBlock.RunPeriod)
            Else
                Dim lEJDate As Double = TimAddJ(lSJDate, 6, 1, 1)
                lReport.AppendLine(TimeSpanAsString(lSJDate, lEJDate, "   Time Span: "))
                lSJDate = lEJDate
            End If

            Dim lOutputTable As New atcTableDelimited
            With lOutputTable
                .Delimiter = vbTab
                If lSummary Then
                    .NumFields = 1 + (lPerlndColumns.Count + lImplndColumns.Count + 1) * 3
                Else
                    .NumFields = 1 + (lPerlndColumns.Count + lImplndColumns.Count + 1)
                End If
                .NumRecords = 8 + lPerlndOperations.Count / lPerlndColumns.Count
                Dim lTotalPerColumn(.NumFields) As Double
                Dim lCountPerColumn(.NumFields) As Integer
                Dim lTotalAreaPerColumn(.NumFields) As Double
                Dim lTotalTonsPerColumn(.NumFields) As Double
                Dim lMaxTonsPerAcre(.NumFields) As Double
                Dim lMinTonsPerAcre(.NumFields) As Double
                Dim lMaxSegment(.NumFields) As Integer
                Dim lMinSegment(.NumFields) As Integer

                .CurrentRecord = 1
                Dim lFieldName As String
                Dim lField As Integer = 0
                lField += 1
                .FieldLength(lField) = 30
                .FieldType(lField) = "C"
                .FieldName(lField) = "Segment"
                .Value(lField) = "Start"
                .FieldName(lField + 1) = "PERLND"
                For Each lFieldName In lPerlndColumns
                    'Mean
                    lField += 1
                    .FieldLength(lField) = 10
                    .FieldType(lField) = "N"
                    .Value(lField) = lFieldName
                    If lSummary Then
                        'Min
                        lField += 1
                        .FieldLength(lField) = 10
                        .FieldType(lField) = "N"

                        'Max
                        lField += 1
                        .FieldLength(lField) = 10
                        .FieldType(lField) = "N"
                    End If
                Next
                .FieldName(lField + 1) = "IMPLND"
                For Each lFieldName In lImplndColumns
                    'Mean
                    lField += 1
                    .FieldLength(lField) = 10
                    .FieldType(lField) = "N"
                    .Value(lField) = lFieldName
                    If lSummary Then
                        'Min
                        lField += 1
                        .FieldLength(lField) = 10
                        .FieldType(lField) = "N"

                        'Max
                        lField += 1
                        .FieldLength(lField) = 10
                        .FieldType(lField) = "N"
                    End If
                Next

                lField += 1
                .FieldLength(lField) = 10
                .FieldType(lField) = "N"
                .Value(lField) = "Weighted Average"
                If lSummary Then
                    lField += 1
                    .FieldLength(lField) = 10
                    .FieldType(lField) = "N"

                    lField += 1
                    .FieldLength(lField) = 10
                    .FieldType(lField) = "N"
                End If

                If lSummary Then
                    .CurrentRecord += 1
                    lField = 1
                    For lOperationIndex = 0 To lPerlndColumns.Count - 1
                        lField += 1 : .Value(lField) = "Mean"
                        lField += 1 : .Value(lField) = "Min"
                        lField += 1 : .Value(lField) = "Max"
                    Next
                    For lOperationIndex = 0 To lImplndColumns.Count - 1
                        lField += 1 : .Value(lField) = "Mean"
                        lField += 1 : .Value(lField) = "Min"
                        lField += 1 : .Value(lField) = "Max"
                    Next
                    lField += 1 : .Value(lField) = "Mean"
                    lField += 1 : .Value(lField) = "Min"
                    lField += 1 : .Value(lField) = "Max"
                End If

                .CurrentRecord += 1
                For lField = 2 To .NumFields
                    .Value(lField) = lUnits
                    lMinTonsPerAcre(lField) = GetMaxValue()
                Next

                Dim lFirstDataRecord As Integer = .CurrentRecord + 1
                'Dim lLastDataRecord As Integer = lFirstDataRecord + lPerlndOperations.Count / lNumUniquePerlnd - 1
                Dim lSegmentLabel As Integer

                Dim lRecord As Integer = lFirstDataRecord
                For lSegmentIndex As Integer = 0 To aPerlndSegmentStarts.GetUpperBound(0)
                    lSegment = aPerlndSegmentStarts(lSegmentIndex)
                    If aImplndSegmentStarts.GetUpperBound(0) > 0 Then
                        lSegmentImplnd = aImplndSegmentStarts(lSegmentIndex)
                    End If
                    'While lSegment < lPerlndLastId OrElse lSegmentImplnd < lImplndLastId
                    .CurrentRecord = lRecord
                    Dim lFound As Boolean = False
                    While Not lFound
                        Dim lDescriptionsThisRow As New ArrayList
                        lField = 1
                        lSegmentLabel = lSegment 'CInt(10 * Math.Floor(lSegment / 10))
                        .Value(lField) = lSegmentLabel
                        Dim lID As Integer = lSegment
                        Dim lRowTotalArea As Double = 0
                        Dim lRowTotalTonsMean As Double = 0
                        Dim lRowTotalTonsMin As Double = 0
                        Dim lRowTotalTonsMax As Double = 0
                        Dim lBaseId As Integer = lID - lID Mod aIdsPerSeg
                        SetCellsTonsPerAcre(lSummary, aUci, lPerlndOperations, lPerlndColumns, lPerlndUniqueIds, _
                                            lID, lBaseId + lPerlndLastId, aIdsPerSeg, _
                                            lCurrentNonpointData, _
                                            lOutputTable, lField, _
                                            lRowTotalArea, _
                                            lRowTotalTonsMean, lRowTotalTonsMin, lRowTotalTonsMax, _
                                            lTotalAreaPerColumn, lTotalTonsPerColumn)

                        lID = lSegmentImplnd
                        SetCellsTonsPerAcre(lSummary, aUci, lImplndOperations, lImplndColumns, lImplndUniqueIds, _
                                            lID, lBaseId + lImplndLastId, aIdsPerSeg, _
                                            lCurrentNonpointData, _
                                            lOutputTable, lField, _
                                            lRowTotalArea, _
                                            lRowTotalTonsMean, lRowTotalTonsMin, lRowTotalTonsMax, _
                                            lTotalAreaPerColumn, lTotalTonsPerColumn)

                        If lRowTotalArea > 0 Then
                            lFound = True
                            lTotalAreaPerColumn(.NumFields) += lRowTotalArea
                            If lSummary Then
                                lTotalAreaPerColumn(.NumFields - 2) += lRowTotalArea
                                lTotalAreaPerColumn(.NumFields - 1) += lRowTotalArea

                                lTotalTonsPerColumn(.NumFields - 2) += lRowTotalTonsMean
                                lTotalTonsPerColumn(.NumFields - 1) += lRowTotalTonsMin
                                lTotalTonsPerColumn(.NumFields) += lRowTotalTonsMax

                                .Value(.NumFields - 2) = Format(lRowTotalTonsMean / lRowTotalArea, lNumberFormat)
                                .Value(.NumFields - 1) = Format(lRowTotalTonsMin / lRowTotalArea, lNumberFormat)
                                .Value(.NumFields) = Format(lRowTotalTonsMax / lRowTotalArea, lNumberFormat)
                            Else
                                lTotalTonsPerColumn(.NumFields) += lRowTotalTonsMean
                                .Value(.NumFields) = Format(lRowTotalTonsMean / lRowTotalArea, lNumberFormat)
                            End If
                            For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                                Dim lValue As Double
                                If Double.TryParse(.Value(lField), lValue) Then
                                    lCountPerColumn(lField) += 1
                                    lTotalPerColumn(lField) += lValue
                                    If lValue > lMaxTonsPerAcre(lField) Then
                                        lMaxTonsPerAcre(lField) = lValue
                                        lMaxSegment(lField) = lSegmentLabel
                                    End If
                                    If lValue < lMinTonsPerAcre(lField) Then
                                        lMinTonsPerAcre(lField) = lValue
                                        lMinSegment(lField) = lSegmentLabel
                                    End If
                                End If
                            Next
                        End If
                    End While
                    lRecord += 1
                Next

                .CurrentRecord += 2
                .Value(1) = "Weighted Average"
                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    If lTotalAreaPerColumn(lField) > 0 Then
                        .Value(lField) = Format(lTotalTonsPerColumn(lField) / lTotalAreaPerColumn(lField), lNumberFormat)
                    End If
                Next
                .CurrentRecord += 1
                .Value(1) = "Arithmetic Mean"
                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    If lCountPerColumn(lField) > 0 Then
                        .Value(lField) = Format(lTotalPerColumn(lField) / lCountPerColumn(lField), lNumberFormat)
                    End If
                Next

                .CurrentRecord += 2
                .Value(1) = "Maximum"
                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    If lMaxSegment(lField) > 0 Then
                        .Value(lField) = Format(lMaxTonsPerAcre(lField), lNumberFormat)
                    End If
                Next
                .CurrentRecord += 1
                .Value(1) = "Max Segment"
                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    If lMaxSegment(lField) > 0 Then
                        .Value(lField) = lMaxSegment(lField)
                    End If
                Next

                .CurrentRecord += 2
                .Value(1) = "Minimum"
                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    If lMinSegment(lField) > 0 Then
                        .Value(lField) = Format(lMinTonsPerAcre(lField), lNumberFormat)
                    End If
                Next
                .CurrentRecord += 1
                .Value(1) = "Min Segment"
                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    If lMinSegment(lField) > 0 Then
                        .Value(lField) = lMinSegment(lField)
                    End If
                Next

                lReport.Append(.ToString)
                lReport.AppendLine()
            End With
        Next
        Return lReport
    End Function

    Private Sub MinMaxID(ByVal aOperations As HspfOperations, ByRef aMinID As Integer, ByRef aMaxID As Integer)
        aMinID = 10000
        aMaxID = 0
        For Each lOperation As HspfOperation In aOperations
            If lOperation.Id < aMinID Then aMinID = lOperation.Id
            If lOperation.Id > aMaxID Then aMaxID = lOperation.Id
        Next
        If aMinID > aMaxID Then aMinID = 0
    End Sub

    Private Function NumUniqueOperations(ByVal aOperations As HspfOperations) As Integer
        Dim lAlreadySeen As New ArrayList
        For Each lOperation As HspfOperation In aOperations
            If Not lAlreadySeen.Contains(lOperation.Description) Then
                lAlreadySeen.Add(lOperation.Description)
            End If
        Next
        Return lAlreadySeen.Count
    End Function

    Private Function OperationsRepeatInterval(ByVal aOperations As HspfOperations) As Integer
        If aOperations.Count > 1 Then
            Dim lAlreadySeen As New atcCollection
            Dim lRepeatCheck As Integer = 0
            While lRepeatCheck < aOperations.Count AndAlso Not lAlreadySeen.Keys.Contains(aOperations(lRepeatCheck).Description)
                lAlreadySeen.Add(aOperations(lRepeatCheck).Description, aOperations(lRepeatCheck).Id)
                lRepeatCheck += 1
            End While

            If lRepeatCheck < aOperations.Count Then
                Return aOperations(lRepeatCheck).Id - lAlreadySeen.ItemByKey(aOperations(lRepeatCheck).Description)
            End If
        End If
        Return 100
    End Function

    Private Sub FindSegmentStarts(ByVal aOperations As HspfOperations, ByRef aStarts As Integer(), _
                                  ByRef aUniqueOperations As ArrayList, ByRef aUniqueIds As ArrayList)
        Dim lMinID As Integer = 10000
        Dim lMaxID As Integer = 0
        Dim lMinDescription As String = ""
        Dim lOperationIds As New SortedList
        For Each lOperation As HspfOperation In aOperations
            With lOperation
                Dim lId As Integer = .Id Mod 100
                If Not lOperationIds.ContainsValue(.Description) Then
                    If lOperationIds.IndexOfKey(lId) = -1 Then
                        lOperationIds.Add(lId, .Description)
                    Else
                        'TODO: update description
                    End If
                End If
                    If .Id < lMinID Then lMinID = .Id : lMinDescription = .Description
                    If .Id > lMaxID Then lMaxID = .Id
            End With
        Next

        aUniqueIds = New ArrayList
        aUniqueIds.AddRange(lOperationIds.Keys)

        aUniqueOperations = New ArrayList
        aUniqueOperations.AddRange(lOperationIds.Values)

        If aStarts Is Nothing OrElse aStarts.Length = 0 Then
            Dim lStarts As New ArrayList
            For Each lOperation As HspfOperation In aOperations
                If Not lOperation.Description.Equals(lMinDescription) Then
                    lStarts.Add(lOperation.Id)
                End If
            Next
            ReDim aStarts(lStarts.Count)
            For lIndex As Integer = 0 To lStarts.Count - 1
                aStarts(lIndex) = lStarts(lIndex)
            Next
        End If
    End Sub

    Private Sub SetCellsTonsPerAcre(ByVal aIncludeMinMax As Boolean, _
                                    ByVal aUCI As HspfUci, _
                                    ByVal aOperations As HspfOperations, _
                                    ByVal aHeaders As ArrayList, _
                                    ByVal aUniqueIds As ArrayList, _
                                    ByVal aID As Integer, ByVal aLastID As Integer, ByVal aIdsPerSeg As Integer, _
                                    ByVal aData As atcTimeseriesGroup, _
                                    ByVal aTable As atcTable, _
                                    ByRef aField As Integer, _
                                    ByRef aTotalArea As Double, _
                                    ByRef aYearlyTonsMean As Double, _
                                    ByRef aYearlyTonsMin As Double, _
                                    ByRef aYearlyTonsMax As Double, _
                                    ByRef aTotalAreaPerColumn() As Double, _
                                    ByRef aTotalTonsPerColumn() As Double)

        While aID <= aLastID
            Dim lKey As String = "K" & aID
            If aOperations.Contains(lKey) Then
                aField += 1
                If aField >= aTotalAreaPerColumn.GetUpperBound(0) Then
                    Dim lExpandAmount As Integer = 1
                    If aIncludeMinMax Then lExpandAmount = 3
                    Logger.Dbg("Field " & aField & " ExpandArrays")
                    ReDim Preserve aTotalAreaPerColumn(aField + lExpandAmount)
                    ReDim Preserve aTotalTonsPerColumn(aField + lExpandAmount)
                    aTable.NumFields += lExpandAmount
                End If
                Dim lOperation As HspfOperation = aOperations.Item(lKey)
                Dim lArea As Double = OperationArea(aUCI, lOperation)
                Dim lSumAnnual As Double = 0
                Dim lMinAnnual As Double = 0
                Dim lMaxAnnual As Double = 0

                For Each lTs As atcTimeseries In aData.FindData("Location", lOperation.Name.Substring(0, 1) & ":" & aID)
                    lSumAnnual += lArea * lTs.Attributes.GetValue("SumAnnual")
                    If aIncludeMinMax Then
                        Dim lTsYearly As atcTimeseries = Aggregate(lTs, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                        lMinAnnual += lArea * lTsYearly.Attributes.GetValue("Min")
                        lMaxAnnual += lArea * lTsYearly.Attributes.GetValue("Max")
                    End If
                Next
                aTotalArea += lArea
                aYearlyTonsMean += lSumAnnual
                aYearlyTonsMin += lMinAnnual
                aYearlyTonsMax += lMaxAnnual

                'aTable.Value(aField) = lOperation.Name & lOperation.Id & " area=" & DoubleToString(lArea) & " total=" & DoubleToString(lTotal) & " Per=" & DoubleToString(lTotal / lArea)
                Dim lNumberFormat As String = "#,##0.000"
                aTable.Value(aField) = Format(lSumAnnual / lArea, lNumberFormat)
                aTotalAreaPerColumn(aField) += lArea
                aTotalTonsPerColumn(aField) += lSumAnnual

                If aIncludeMinMax Then
                    aField += 1
                    aTable.Value(aField) = Format(lMinAnnual / lArea, lNumberFormat)
                    aTotalAreaPerColumn(aField) += lArea
                    aTotalTonsPerColumn(aField) += lMinAnnual

                    aField += 1
                    aTable.Value(aField) = Format(lMaxAnnual / lArea, lNumberFormat)
                    aTotalAreaPerColumn(aField) += lArea
                    aTotalTonsPerColumn(aField) += lMaxAnnual
                End If
            ElseIf aUniqueIds.Contains(aID Mod aIdsPerSeg) Then
                aField += 1
                aTable.Value(aField) = "--"
                If aIncludeMinMax Then
                    aField += 1
                    aTable.Value(aField) = "--"
                    aField += 1
                    aTable.Value(aField) = "--"
                End If
            End If
            aID += 1
        End While
    End Sub
End Module
