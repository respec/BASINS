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
                           ByVal aRunMade As String) As Text.StringBuilder

        Dim lNumberFormat As String = "#,##0.0"
        Dim lUnits As String = ""
        Dim lNonpointData As New atcDataGroup

        Dim lTotalInflowData As New atcDataGroup
        Dim lOutflowData As New atcDataGroup
        Dim lDepScourData As New atcDataGroup

        Dim lPerlndOperations As HspfOperations = aUci.OpnBlks("PERLND").Ids
        Dim lImplndOperations As HspfOperations = aUci.OpnBlks("IMPLND").Ids
        Dim lPerlndFirstId As Integer = lPerlndOperations(0).Id
        Dim lImplndFirstId As Integer = lImplndOperations(0).Id
        Dim lNumUniquePerlnd As Integer = NumUniqueOperations(lPerlndOperations)
        Dim lNumUniqueImplnd As Integer = NumUniqueOperations(lImplndOperations)
        Dim lRepeatPerlnd As Integer = lPerlndOperations(lNumUniquePerlnd).Id - lPerlndFirstId
        Dim lRepeatImplnd As Integer = lImplndOperations(lNumUniqueImplnd).Id - lImplndFirstId
        Dim lOperationIndex As Integer

        Select Case aBalanceType
            Case "Sediment"
                lUnits = "(tons/acre)"
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOSED"))
                lNonpointData.AddRange((aScenarioResults.DataSets.FindData("Constituent", "SOSLD")))

                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ISED-TOT"))
                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ROSED-TOT"))
                lDepScourData.Add(aScenarioResults.DataSets.FindData("Constituent", "DEPSCOUR-TOT"))
            Case Else
                Return New Text.StringBuilder("Overland report not yet defined for balance type '" & aBalanceType & "'")
        End Select

        Dim lSB As New Text.StringBuilder

        lSB.AppendLine("Overland Summary Report for " & aBalanceType & " in " & aScenario & " " & lUnits)
        lSB.AppendLine("   Run Made " & aRunMade)
        lSB.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lSB.AppendLine("   " & aUci.GlobalBlock.RunPeriod)

        Dim lOutputTable As New atcTableDelimited
        With lOutputTable
            .Delimiter = vbTab
            .NumFields = 1 + (lNumUniquePerlnd + lNumUniqueImplnd + 1) * 3
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

                'Min
                lField += 1
                .FieldLength(lField) = 10
                .FieldType(lField) = "N"

                'Max
                lField += 1
                .FieldLength(lField) = 10
                .FieldType(lField) = "N"
            Next
            For lOperationIndex = 0 To lNumUniqueImplnd - 1
                'Mean
                lField += 1
                .FieldLength(lField) = 10
                .FieldType(lField) = "N"
                .Value(lField) = lImplndOperations(lOperationIndex).Description
                If lOperationIndex = 0 Then .FieldName(lField) = "IMPLND"

                'Min
                lField += 1
                .FieldLength(lField) = 10
                .FieldType(lField) = "N"

                'Max
                lField += 1
                .FieldLength(lField) = 10
                .FieldType(lField) = "N"
            Next

            lField += 1
            .FieldLength(lField) = 10
            .FieldType(lField) = "N"
            .Value(lField) = "Weighted Average"

            lField += 1
            .FieldLength(lField) = 10
            .FieldType(lField) = "N"

            lField += 1
            .FieldLength(lField) = 10
            .FieldType(lField) = "N"

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
                    SetCellsTonsPerAcre(aUci, lPerlndOperations, lID, lID + lNumUniquePerlnd - 1, _
                                        lNonpointData, _
                                        lOutputTable, lField, _
                                        lRowTotalArea, _
                                        lRowTotalTonsMean, lRowTotalTonsMin, lRowTotalTonsMax, _
                                        lTotalAreaPerColumn, lTotalTonsPerColumn)

                    lID = lSegmentImplnd
                    SetCellsTonsPerAcre(aUci, lImplndOperations, lID, lID + lNumUniqueImplnd - 1, _
                                        lNonpointData, _
                                        lOutputTable, lField, _
                                        lRowTotalArea, _
                                        lRowTotalTonsMean, lRowTotalTonsMin, lRowTotalTonsMax, _
                                        lTotalAreaPerColumn, lTotalTonsPerColumn)

                    If lRowTotalArea > 0 Then
                        lFound = True
                        lTotalAreaPerColumn(.NumFields - 2) += lRowTotalArea
                        lTotalAreaPerColumn(.NumFields - 1) += lRowTotalArea
                        lTotalAreaPerColumn(.NumFields) += lRowTotalArea

                        lTotalTonsPerColumn(.NumFields - 2) += lRowTotalTonsMean
                        lTotalTonsPerColumn(.NumFields - 1) += lRowTotalTonsMin
                        lTotalTonsPerColumn(.NumFields) += lRowTotalTonsMax

                        .Value(.NumFields - 2) = DoubleToString(lRowTotalTonsMean / lRowTotalArea)
                        .Value(.NumFields - 1) = DoubleToString(lRowTotalTonsMin / lRowTotalArea)
                        .Value(.NumFields) = DoubleToString(lRowTotalTonsMax / lRowTotalArea)

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
            Return lSB
        End With
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

    Private Sub SetCellsTonsPerAcre(ByVal aUCI As HspfUci, _
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
                    Dim lTsYearly As atcTimeseries = FillValues(lTs, atcTimeUnit.TUYear, , GetNaN)
                    lMinAnnual += lArea * lTsYearly.Attributes.GetValue("Min")
                    lMaxAnnual += lArea * lTsYearly.Attributes.GetValue("Max")
                Next
                aTotalArea += lArea
                aYearlyTonsMean += lSumAnnual
                aYearlyTonsMin += lMinAnnual
                aYearlyTonsMax += lMaxAnnual


                'aTable.Value(aField) = lOperation.Name & lOperation.Id & " area=" & DoubleToString(lArea) & " total=" & DoubleToString(lTotal) & " Per=" & DoubleToString(lTotal / lArea)
                aTable.Value(aField) = DoubleToString(lSumAnnual / lArea)
                aTotalAreaPerColumn(aField) += lArea
                aTotalTonsPerColumn(aField) += lSumAnnual

                aField += 1
                aTable.Value(aField) = DoubleToString(lMinAnnual / lArea)
                aTotalAreaPerColumn(aField) += lArea
                aTotalTonsPerColumn(aField) += lMinAnnual

                aField += 1
                aTable.Value(aField) = DoubleToString(lMaxAnnual / lArea)
                aTotalAreaPerColumn(aField) += lArea
                aTotalTonsPerColumn(aField) += lMaxAnnual
            Else
                aTable.Value(aField) = "--"
                aField += 1
                aTable.Value(aField) = "--"
                aField += 1
                aTable.Value(aField) = "--"
            End If
            aID += 1
        End While
    End Sub
End Module
