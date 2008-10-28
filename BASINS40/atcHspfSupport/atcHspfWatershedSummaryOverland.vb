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

        Dim lCumulativePointNonpointColl As New atcCollection

        Dim lSB As New Text.StringBuilder
        lSB.AppendLine("Overland Summary Report for " & aBalanceType & " in " & aScenario & " " & lUnits)
        lSB.AppendLine("   Run Made " & aRunMade)
        lSB.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lSB.AppendLine("   " & aUci.GlobalBlock.RunPeriod)

        Dim lOutputTable As New atcTableDelimited
        With lOutputTable
            .Delimiter = vbTab
            .NumFields = 2 + lNumUniquePerlnd + lNumUniqueImplnd
            .NumRecords = 8 + lPerlndOperations.Count / lNumUniquePerlnd
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
                lField += 1
                .FieldLength(lField) = 10
                .FieldType(lField) = "N"
                .Value(lField) = lUnits
                .FieldName(lField) = lPerlndOperations(lOperationIndex).Description
                lMinTonsPerAcre(lField) = GetMaxValue()
            Next
            For lOperationIndex = 0 To lNumUniqueImplnd - 1
                lField += 1
                .FieldLength(lField) = 10
                .FieldType(lField) = "N"
                .Value(lField) = lUnits
                .FieldName(lField) = lImplndOperations(lOperationIndex).Description
                lMinTonsPerAcre(lField) = GetMaxValue()
            Next

            lField += 1
            .FieldLength(lField) = 10
            .FieldType(lField) = "N"
            .Value(lField) = lUnits
            .FieldName(lField) = "Weighted Average"
            lMinTonsPerAcre(lField) = GetMaxValue()

            .CurrentRecord += 1
            .Value(2) = "PERLND"
            .Value(lNumUniquePerlnd + 2) = "IMPLND"

            Dim lFirstDataRecord As Integer = .CurrentRecord + 1
            Dim lLastDataRecord As Integer = lFirstDataRecord + lPerlndOperations.Count / lNumUniquePerlnd - 1

            For lRecord As Integer = lFirstDataRecord To lLastDataRecord
                .CurrentRecord = lRecord
                lField = 1
                Dim lSegment As Integer = lPerlndFirstId + (lRecord - lFirstDataRecord) * lRepeatPerlnd
                .Value(lField) = lSegment
                Dim lID As Integer = lSegment
                Dim lRowTotalArea As Double = 0
                Dim lRowTotalTons As Double = 0
                SetCellsTonsPerAcre(aUci, lPerlndOperations, _
                                    lID, _
                                    lID + lNumUniquePerlnd - 1, _
                                    lNonpointData, lOutputTable, lField, lRowTotalArea, lRowTotalTons, lTotalAreaPerColumn, lTotalTonsPerColumn)

                lID = lImplndFirstId + (lRecord - lFirstDataRecord) * lRepeatImplnd
                SetCellsTonsPerAcre(aUci, lImplndOperations, _
                                    lID, lID + lNumUniqueImplnd - 1, _
                                    lNonpointData, lOutputTable, lField, lRowTotalArea, lRowTotalTons, lTotalAreaPerColumn, lTotalTonsPerColumn)

                lTotalAreaPerColumn(.NumFields) += lRowTotalArea
                lTotalTonsPerColumn(.NumFields) += lRowTotalTons

                .Value(.NumFields) = DoubleToString(lRowTotalTons / lRowTotalArea)

                For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                    Dim lValue As Double
                    If Double.TryParse(.Value(lField), lValue) Then
                        If lValue > lMaxTonsPerAcre(lField) Then
                            lMaxTonsPerAcre(lField) = lValue
                            lMaxSegment(lField) = lSegment
                        End If
                        If lValue < lMinTonsPerAcre(lField) Then
                            lMinTonsPerAcre(lField) = lValue
                            lMinSegment(lField) = lSegment
                        End If
                    End If
                Next
            Next
            .CurrentRecord += 2
            .Value(1) = "Weighted Average"
            For lField = 2 To lTotalAreaPerColumn.GetUpperBound(0)
                If lTotalAreaPerColumn(lField) > 0 Then
                    .Value(lField) = DoubleToString(lTotalTonsPerColumn(lField) / lTotalAreaPerColumn(lField))
                End If
            Next

            .CurrentRecord += 1
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

            .CurrentRecord += 1
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
                                    ByRef aTotalTons As Double, _
                                    ByRef aTotalAreaPerColumn() As Double, _
                                    ByRef aTotalTonsPerColumn() As Double)
        While aID <= aLastID
            aField += 1
            Dim lKey As String = "K" & aID
            If aOperations.Contains(lKey) Then
                Dim lOperation As HspfOperation = aOperations.Item(lKey)
                Dim lArea As Double = OperationArea(aUCI, lOperation)
                Dim lTotal As Double = 0

                For Each lTs As atcTimeseries In aData.FindData("Location", lOperation.Name.Substring(0, 1) & ":" & aID)
                    lTotal += lArea * lTs.Attributes.GetValue("SumAnnual")
                Next
                aTotalArea += lArea
                aTotalTons += lTotal

                aTotalAreaPerColumn(aField) += lArea
                aTotalTonsPerColumn(aField) += lTotal

                'aTable.Value(aField) = lOperation.Name & lOperation.Id & " area=" & DoubleToString(lArea) & " total=" & DoubleToString(lTotal) & " Per=" & DoubleToString(lTotal / lArea)
                aTable.Value(aField) = DoubleToString(lTotal / lArea)
            Else
                aTable.Value(aField) = "Missing"
            End If
            aID += 1
        End While
    End Sub
End Module
