Imports atcUtility
Imports atcData
Imports atcSeasons
Imports atcUCI
Imports MapWinUtility

Public Module WatershedSummaryOverland

    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aBalanceType As String, _
                           ByVal aOperationTypes As atcCollection, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcDataSource, _
                           ByVal aRunMade As String, _
                  Optional ByVal aEachYear As Boolean = True, _
                  Optional ByVal aSummary As Boolean = True) As Text.StringBuilder

        Dim lNumberFormat As String = "#,##0.0"
        Dim lUnits As String = ""
        Dim lAllNonpointData As New atcDataGroup

        Dim lPerlndOperations As HspfOperations = aUci.OpnBlks("PERLND").Ids
        Dim lImplndOperations As HspfOperations = aUci.OpnBlks("IMPLND").Ids
        Dim lPerlndFirstId As Integer = lPerlndOperations(0).Id
        Dim lImplndFirstId As Integer = lImplndOperations(0).Id
        Dim lNumUniquePerlnd As Integer = NumUniqueOperations(lPerlndOperations)
        Dim lNumUniqueImplnd As Integer = NumUniqueOperations(lImplndOperations)
        Dim lRepeatPerlnd As Integer = OperationsRepeatInterval(lPerlndOperations)
        Dim lRepeatImplnd As Integer = OperationsRepeatInterval(lImplndOperations)
        Dim lOperationIndex As Integer
        Dim lSeasonName As String

        Select Case aBalanceType
            Case "Sediment"
                lUnits = "(tons/acre)"
                lAllNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOSED"))
                lAllNonpointData.AddRange((aScenarioResults.DataSets.FindData("Constituent", "SOSLD")))
            Case Else
                Return New Text.StringBuilder("Overland report not yet defined for balance type '" & aBalanceType & "'")
        End Select

        Dim lAllYearsToDo As New Generic.Dictionary(Of String, atcDataGroup)
        If aEachYear Then
            Dim lSplitter As New atcSeasons.atcSeasonsCalendarYear
            For Each lOriginalTimeseries As atcTimeseries In lAllNonpointData
                For Each lYearlyTs As atcTimeseries In lSplitter.Split(lOriginalTimeseries, Nothing)
                    lSeasonName = lYearlyTs.Attributes.GetValue("SeasonName", "")
                    Dim lDataGroup As atcDataGroup
                    If lAllYearsToDo.ContainsKey(lSeasonName) Then
                        lDataGroup = lAllYearsToDo.Item(lSeasonName)
                    Else
                        lDataGroup = New atcDataGroup
                        lAllYearsToDo.Add(lSeasonName, lDataGroup)
                    End If
                    lDataGroup.Add(lYearlyTs)
                Next
            Next
        End If

        If aSummary Then lAllYearsToDo.Add("Summary", lAllNonpointData)

        Dim lSB As New Text.StringBuilder
        For Each lCurrentNonpointData As atcDataGroup In lAllYearsToDo.Values
            Dim lSummary As Boolean = lCurrentNonpointData.Equals(lAllNonpointData)
            lSeasonName = lCurrentNonpointData.ItemByIndex(0).Attributes.GetValue("SeasonName", "")
            lSB.AppendLine("Overland Summary Report for " & aBalanceType & " in " & aScenario & " " & lUnits)
            lSB.AppendLine("   Run Made " & aRunMade)
            lSB.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
            If lSummary Then
                lSB.AppendLine("   " & aUci.GlobalBlock.RunPeriod)
            Else
                lSB.AppendLine("   Time Span: 1 yr from " & lSeasonName) 'TODO: dates
            End If

            Dim lOutputTable As New atcTableDelimited
            With lOutputTable
                .Delimiter = vbTab
                If lSummary Then
                    .NumFields = 1 + (lNumUniquePerlnd + lNumUniqueImplnd + 1) * 3
                Else
                    .NumFields = 1 + (lNumUniquePerlnd + lNumUniqueImplnd + 1)
                End If
                .NumRecords = 8 + lPerlndOperations.Count / lNumUniquePerlnd
                Dim lTotalPerColumn(.NumFields) As Double
                Dim lCountPerColumn(.NumFields) As Integer
                Dim lTotalAreaPerColumn(.NumFields) As Double
                Dim lTotalTonsPerColumn(.NumFields) As Double
                Dim lMaxTonsPerAcre(.NumFields) As Double
                Dim lMinTonsPerAcre(.NumFields) As Double
                Dim lMaxSegment(.NumFields) As Integer
                Dim lMinSegment(.NumFields) As Integer

                .CurrentRecord = 1
                Dim lField As Integer = 0
                lField += 1
                .FieldLength(lField) = 30
                .FieldType(lField) = "C"
                .Value(lField) = "    "
                .FieldName(lField) = "Segment"
                For lOperationIndex = 0 To lNumUniquePerlnd - 1
                    'Mean
                    lField += 1
                    .FieldLength(lField) = 10
                    .FieldType(lField) = "N"
                    .Value(lField) = lPerlndOperations(lOperationIndex).Description
                    If lOperationIndex = 0 Then .FieldName(lField) = "PERLND"
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
                For lOperationIndex = 0 To lNumUniqueImplnd - 1
                    'Mean
                    lField += 1
                    .FieldLength(lField) = 10
                    .FieldType(lField) = "N"
                    .Value(lField) = lImplndOperations(lOperationIndex).Description
                    If lOperationIndex = 0 Then .FieldName(lField) = "IMPLND"
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
                    For lOperationIndex = 0 To lNumUniquePerlnd - 1
                        lField += 1 : .Value(lField) = "Mean"
                        lField += 1 : .Value(lField) = "Min"
                        lField += 1 : .Value(lField) = "Max"
                    Next
                    For lOperationIndex = 0 To lNumUniqueImplnd - 1
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
                Dim lLastDataRecord As Integer = lFirstDataRecord + lPerlndOperations.Count / lNumUniquePerlnd - 1
                Dim lSegment As Integer = lPerlndFirstId
                Dim lSegmentLabel As Integer
                Dim lSegmentImplnd As Integer = lImplndFirstId

                For lRecord As Integer = lFirstDataRecord To lLastDataRecord
                    .CurrentRecord = lRecord
                    Dim lFound As Boolean = False
                    While Not lFound
                        lField = 1
                        lSegmentLabel = CInt(10 * Math.Floor(lSegment / 10))
                        .Value(lField) = lSegmentLabel
                        Dim lID As Integer = lSegment
                        Dim lRowTotalArea As Double = 0
                        Dim lRowTotalTonsMean As Double = 0
                        Dim lRowTotalTonsMin As Double = 0
                        Dim lRowTotalTonsMax As Double = 0
                        SetCellsTonsPerAcre(lSummary, aUci, lPerlndOperations, _
                                            lID, lID + lNumUniquePerlnd - 1, _
                                            lCurrentNonpointData, _
                                            lOutputTable, lField, _
                                            lRowTotalArea, _
                                            lRowTotalTonsMean, lRowTotalTonsMin, lRowTotalTonsMax, _
                                            lTotalAreaPerColumn, lTotalTonsPerColumn)

                        lID = lSegmentImplnd
                        SetCellsTonsPerAcre(lSummary, aUci, lImplndOperations, _
                                            lID, lID + lNumUniqueImplnd - 1, _
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

                                .Value(.NumFields - 2) = DoubleToString(lRowTotalTonsMean / lRowTotalArea)
                                .Value(.NumFields - 1) = DoubleToString(lRowTotalTonsMin / lRowTotalArea)
                                .Value(.NumFields) = DoubleToString(lRowTotalTonsMax / lRowTotalArea)
                            Else
                                lTotalTonsPerColumn(.NumFields) += lRowTotalTonsMean
                                .Value(.NumFields) = DoubleToString(lRowTotalTonsMean / lRowTotalArea)
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
                        lSegment += lRepeatPerlnd
                        lSegmentImplnd += lRepeatImplnd
                    End While
                Next

                .CurrentRecord += 2
                .Value(1) = "Weighted Average"
                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    If lTotalAreaPerColumn(lField) > 0 Then
                        .Value(lField) = DoubleToString(lTotalTonsPerColumn(lField) / lTotalAreaPerColumn(lField))
                    End If
                Next
                .CurrentRecord += 1
                .Value(1) = "Arithmetic Mean"
                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    If lCountPerColumn(lField) > 0 Then
                        .Value(lField) = DoubleToString(lTotalPerColumn(lField) / lCountPerColumn(lField))
                    End If
                Next

                .CurrentRecord += 2
                .Value(1) = "Maximum"
                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    If lMaxSegment(lField) > 0 Then
                        .Value(lField) = DoubleToString(lMaxTonsPerAcre(lField))
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
                        .Value(lField) = DoubleToString(lMinTonsPerAcre(lField))
                    End If
                Next
                .CurrentRecord += 1
                .Value(1) = "Min Segment"
                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    If lMinSegment(lField) > 0 Then
                        .Value(lField) = lMinSegment(lField)
                    End If
                Next

                lSB.Append(.ToString)
                lSB.AppendLine()
            End With
        Next
        Return lSB
    End Function

    Private Function NumUniqueOperations(ByVal aOperations As HspfOperations) As Integer
        Dim lNumUnique As Integer = 1
        If aOperations.Count < 2 Then
            lNumUnique = aOperations.Count
        Else
            'While aOperations(lNumUnique).Description <> aOperations(0).Description
            '    lNumUnique += 1
            'End While

            Dim lUniqueThisGroup As Integer
            Dim lStartGroupIndex As Integer = 0

            While lStartGroupIndex < aOperations.Count
                lUniqueThisGroup = 1
                While (lStartGroupIndex + lUniqueThisGroup) < aOperations.Count AndAlso _
                       aOperations(lStartGroupIndex + lUniqueThisGroup).Description <> aOperations(lStartGroupIndex).Description
                    lUniqueThisGroup += 1
                End While
                If lUniqueThisGroup > lNumUnique Then lNumUnique = lUniqueThisGroup
                lStartGroupIndex += lUniqueThisGroup
            End While
        End If
        Return lNumUnique
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

    Private Sub SetCellsTonsPerAcre(ByVal aIncludeMinMax As Boolean, _
                                    ByVal aUCI As HspfUci, _
                                    ByVal aOperations As HspfOperations, _
                                    ByVal aID As Integer, ByVal aLastID As Integer, _
                                    ByVal aData As atcDataGroup, _
                                    ByVal aTable As atcTable, _
                                    ByRef aField As Integer, _
                                    ByRef aTotalArea As Double, _
                                    ByRef aYearlyTonsMean As Double, _
                                    ByRef aYearlyTonsMin As Double, _
                                    ByRef aYearlyTonsMax As Double, _
                                    ByRef aTotalAreaPerColumn() As Double, _
                                    ByRef aTotalTonsPerColumn() As Double)
        While aID <= aLastID
            aField += 1
            Dim lKey As String = "K" & aID
            If aOperations.Contains(lKey) Then
                Dim lOperation As HspfOperation = aOperations.Item(lKey)
                Dim lArea As Double = OperationArea(aUCI, lOperation)
                Dim lSumAnnual As Double = 0
                Dim lMinAnnual As Double = 0
                Dim lMaxAnnual As Double = 0

                For Each lTs As atcTimeseries In aData.FindData("Location", lOperation.Name.Substring(0, 1) & ":" & aID)
                    lSumAnnual += lArea * lTs.Attributes.GetValue("SumAnnual")
                    If aIncludeMinMax Then
                        Dim lTsYearly As atcTimeseries = FillValues(lTs, atcTimeUnit.TUYear, , GetNaN)
                        lMinAnnual += lArea * lTsYearly.Attributes.GetValue("Min")
                        lMaxAnnual += lArea * lTsYearly.Attributes.GetValue("Max")
                    End If
                Next
                aTotalArea += lArea
                aYearlyTonsMean += lSumAnnual
                aYearlyTonsMin += lMinAnnual
                aYearlyTonsMax += lMaxAnnual


                'aTable.Value(aField) = lOperation.Name & lOperation.Id & " area=" & DoubleToString(lArea) & " total=" & DoubleToString(lTotal) & " Per=" & DoubleToString(lTotal / lArea)
                aTable.Value(aField) = DoubleToString(lSumAnnual / lArea)
                aTotalAreaPerColumn(aField) += lArea
                aTotalTonsPerColumn(aField) += lSumAnnual

                If aIncludeMinMax Then
                    aField += 1
                    aTable.Value(aField) = DoubleToString(lMinAnnual / lArea)
                    aTotalAreaPerColumn(aField) += lArea
                    aTotalTonsPerColumn(aField) += lMinAnnual

                    aField += 1
                    aTable.Value(aField) = DoubleToString(lMaxAnnual / lArea)
                    aTotalAreaPerColumn(aField) += lArea
                    aTotalTonsPerColumn(aField) += lMaxAnnual
                End If
            Else
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
