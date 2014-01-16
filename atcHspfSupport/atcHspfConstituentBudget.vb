Imports atcUtility
Imports atcData
Imports atcUCI
Imports MapWinUtility

Public Module ConstituentBudget
    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aBalanceType As String, _
                           ByVal aOperationTypes As atcCollection, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcTimeseriesSource, _
                           ByVal aRunMade As String) As atcReport.IReport

        Dim lNumberFormat As String = "#,##0.00"
        Dim lUnits As String = ""
        Dim lNonpointData As New atcTimeseriesGroup
        Dim lAtmDepData As New atcTimeseriesGroup
        Dim lTotalInflowData As New atcTimeseriesGroup
        Dim lOutflowData As New atcTimeseriesGroup
        Dim lDepScourData As New atcTimeseriesGroup
        Dim lRchresOperations As HspfOperations = aUci.OpnBlks("RCHRES").Ids

        Select Case aBalanceType
            Case "Sediment"
                lUnits = "(tons)"
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOSED"))
                lNonpointData.AddRange((aScenarioResults.DataSets.FindData("Constituent", "SOSLD")))

                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ISED-TOT"))
                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ROSED-TOT"))
                lDepScourData.Add(aScenarioResults.DataSets.FindData("Constituent", "DEPSCOUR-TOT"))
            Case "TotalN"
                lUnits = "(lbs)"
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "NITROGEN - TOTAL OUTFLOW"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "POQUAL-NO3"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-NO3"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "POQUAL-NH3+NH4"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-NH3+NH4"))

                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "POQUAL-BOD"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-BOD"))

                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "N-TOT-IN"))
                lAtmDepData.Add(aScenarioResults.DataSets.FindData("Constituent", "NO3-ATMDEPTOT"))
                lAtmDepData.Add(aScenarioResults.DataSets.FindData("Constituent", "TAM-ATMDEPTOT"))

                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "N-TOT-OUT"))

            Case "TotalP"
                lUnits = "(lbs)"
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "POPHOS"))
                lNonpointData.AddRange((aScenarioResults.DataSets.FindData("Constituent", "POQUAL-Ortho P")))
                lNonpointData.AddRange((aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-Ortho P")))
                lNonpointData.AddRange((aScenarioResults.DataSets.FindData("Constituent", "POQUAL-BOD")))
                lNonpointData.AddRange((aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-BOD")))

                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "P-TOT-IN"))
                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "P-TOT-OUT"))
            Case Else

                Return New atcReport.ReportText("Budget report not yet defined for balance type '" & aBalanceType & "'")
        End Select

        Dim lUpstreamInflows As New atcCollection
        Dim lCumulativePointNonpointColl As New atcCollection

        Dim lReport As New atcReport.ReportText
        Dim lReport2 As New atcReport.ReportText
        lReport.AppendLine(aScenario & " " & aBalanceType & " Average Annual Totals " & lUnits)
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunPeriod)

        lReport2.AppendLine(aScenario & " " & aBalanceType & " Average Annual Totals for Each Land Segment to Individual Reaches " & lUnits)
        lReport2.AppendLine("   Run Made " & aRunMade)
        lReport2.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport2.AppendLine("   " & aUci.GlobalBlock.RunPeriod)


        Dim lOutputTable As New atcTableDelimited
        Dim lOutputTable2 As New atcTableDelimited
        Select Case aBalanceType
            Case "Sediment"

                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 10
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Source"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Dep(+)/Scour(-)"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"

                    For Each lID As HspfOperation In lRchresOperations
                        .CurrentRecord += 1
                        Dim lAreas As New atcCollection
                        LocationAreaCalc(aUci, "R:" & lID.Id, aOperationTypes, lAreas, False)

                        Dim lNonpointTons As Double = TotalForReach(lID, aBalanceType, lAreas, lNonpointData)
                        'This calculation assumes that multiplication factor in
                        'MASS-LINK Blocks sum to 1. Should be able to get sum of Mult Factors for SSED1, 2 and 3 from the uci.

                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If

                        'TODO: these two formulations are slightly different - WHY?

                        'Anurag commentedout following line on July 12, 2012, to account for any mult factor in MASS-LINK block.
                        'Dim lTotalInflow As Double = lNonpointTons + lPointTons + lUpstreamIn
                        'Anurag added following lines
                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        'lNonpointTons = lTotalInflow - lUpstreamIn
                        'Anurag changes are over
                        Dim lPointTons As Double = lTotalInflow - lNonpointTons - lUpstreamIn
                        'Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData) 'TotalForReach(lID, lAreas, lTotalInflowData)

                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                        Dim lDepScour As Double = ValueForReach(lID, lDepScourData) 'TotalForReach(lID, lAreas, lDepScourData)
                        Dim lCumulativePointNonpoint As Double = lNonpointTons + lPointTons
                        If lCumulativePointNonpointColl.Keys.Contains(lID.Id) Then
                            lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lID.Id)
                        End If

                        Dim lReachTrappingEfficiency As Double
                        Try
                            lReachTrappingEfficiency = lDepScour / lTotalInflow
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try

                        Dim lCululativeTrappingEfficiency As Double = 0
                        Try
                            lCululativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try

                        Dim lDownstreamReachID As Integer = lID.DownOper("RCHRES")
                        lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                        lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                        lField = 0
                        lField += 1 : .Value(lField) = lID.Name & " " & lID.Id & " - " & lID.Description
                        lField += 1 : .Value(lField) = DoubleToString(lNonpointTons, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lPointTons, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lDepScour, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lCumulativePointNonpoint, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, , lNumberFormat, , , 6)
                        lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)
                End With
            Case "TotalN"
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 11
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Source"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Atm. Depo. on Water"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Gain(+)/Loss(-)"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"


                    For Each lID As HspfOperation In lRchresOperations
                        .CurrentRecord += 1
                        Dim lAreas As New atcCollection
                        LocationAreaCalc(aUci, "R:" & lID.Id, aOperationTypes, lAreas, False)

                        Dim lNonpointlbs As Double = TotalForReach(lID, aBalanceType, lAreas, lNonpointData)
                        lReport2.Append(ConstituentLoadingByLanduse(lID, aBalanceType, lAreas, lNonpointData))
                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If

                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        Dim lPointlbs As Double = 0

                        Dim lTotalAtmDep As Double = 0
                        lTotalAtmDep = ValueForReach(lID, lAtmDepData.FindData("Constituent", "NO3-ATMDEPTOT"))
                        lTotalAtmDep += ValueForReach(lID, lAtmDepData.FindData("Constituent", "TAM-ATMDEPTOT"))
                        lPointlbs = lTotalInflow - lNonpointlbs - lTotalAtmDep - lUpstreamIn
                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                        Dim lDepScour As Double = lOutflow - lTotalInflow
                        Dim lCumulativePointNonpoint As Double = lNonpointlbs + lPointlbs
                        If lCumulativePointNonpointColl.Keys.Contains(lID.Id) Then
                            lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lID.Id)
                        End If

                        Dim lReachTrappingEfficiency As Double
                        Try
                            lReachTrappingEfficiency = lDepScour / lTotalInflow
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try

                        Dim lCululativeTrappingEfficiency As Double = 0
                        Try
                            lCululativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try

                        Dim lDownstreamReachID As Integer = lID.DownOper("RCHRES")
                        lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                        lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                        lField = 0
                        lField += 1 : .Value(lField) = lID.Name & " " & lID.Id & " - " & lID.Description
                        lField += 1 : .Value(lField) = DoubleToString(lNonpointlbs, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lPointlbs, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalAtmDep, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lDepScour, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lCumulativePointNonpoint, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, , lNumberFormat, , , 6)
                        lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)

                End With
                lReport.Append(lReport2.ToString)
            Case "TotalP"
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 10
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Source"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Gain(+)/Loss(-)"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"

                    For Each lID As HspfOperation In lRchresOperations
                        .CurrentRecord += 1
                        Dim lAreas As New atcCollection
                        LocationAreaCalc(aUci, "R:" & lID.Id, aOperationTypes, lAreas, False)

                        Dim lNonpointlbs As Double = TotalForReach(lID, aBalanceType, lAreas, lNonpointData)

                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If

                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        Dim lPointlbs As Double = 0


                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                        Dim lDepScour As Double = lOutflow - lTotalInflow
                        Dim lCumulativePointNonpoint As Double = lNonpointlbs + lPointlbs
                        If lCumulativePointNonpointColl.Keys.Contains(lID.Id) Then
                            lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lID.Id)
                        End If
                        lPointlbs = lTotalInflow - lNonpointlbs - lUpstreamIn
                        Dim lReachTrappingEfficiency As Double
                        Try
                            lReachTrappingEfficiency = lDepScour / lTotalInflow
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try

                        Dim lCululativeTrappingEfficiency As Double = 0
                        Try
                            lCululativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try

                        Dim lDownstreamReachID As Integer = lID.DownOper("RCHRES")
                        lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                        lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                        lField = 0
                        lField += 1 : .Value(lField) = lID.Name & " " & lID.Id & " - " & lID.Description
                        lField += 1 : .Value(lField) = DoubleToString(lNonpointlbs, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lPointlbs, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lDepScour, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lCumulativePointNonpoint, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, , lNumberFormat, , , 6)
                        lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)

                End With

        End Select
        Return lReport
    End Function

    Private Function TotalForReach(ByVal aReach As HspfOperation, _
                                    ByVal aBalanceType As String, _
                                    ByVal aAreas As atcCollection, _
                                   ByVal aNonpointData As atcTimeseriesGroup) As Double
        Dim lTotal As Double = 0
        'Dim lNewTotal As Double = 0
        For lAreaIndex As Integer = 0 To aAreas.Count - 1
            Dim lLocation As String = aAreas.Keys(lAreaIndex)
            Dim lArea As Double = aAreas.ItemByIndex(lAreaIndex)
            'Logger.Dbg("Area is " & lArea)
            'Dim lSubTotal As Double = 0
            For Each lTs As atcTimeseries In aNonpointData.FindData("Location", lLocation)
                'lSubTotal += lTs.Attributes.GetValue("SumAnnual")
                If Not (Left(lLocation, 2) = "P:" And Left(lTs.Attributes.GetValue("Constituent").ToString, 6) = "SOQUAL") Then

                    Select Case lTs.Attributes.GetValue("Constituent").ToString & "_" & aBalanceType
                        Case "POQUAL-BOD_TotalN"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.069176
                            'Multiplying 0.048 + 0.05294*.4 to BOD to get Organic N
                        Case "SOQUAL-BOD_TotalN"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.069176
                        Case "POQUAL-BOD_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.005234
                            'Multiplying 0.0023 + 0.007326 to BOD to get Organic N
                        Case "SOQUAL-BOD_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.005234
                        Case Else
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual")

                    End Select
                End If


            Next
            'lNewTotal += lArea * lSubTotal
        Next
        'Logger.Dbg("difference " & lTotal - lNewTotal) '"SumAnnual is " & lTs.Attributes.GetValue("SumAnnual"))
        Return lTotal
    End Function

    Private Function ValueForReach(ByVal aReach As HspfOperation, _
                                   ByVal aReachData As atcTimeseriesGroup) As Double
        Dim lReachData As atcTimeseries = aReachData.FindData("Location", "R:" & aReach.Id).Item(0)
        Dim lOutflow As Double = lReachData.Attributes.GetValue("SumAnnual")
        Return lOutflow
    End Function
    Private Function ConstituentLoadingByLanduse(ByVal aReach As HspfOperation, _
                                    ByVal aBalanceType As String, _
                                    ByVal aAreas As atcCollection, _
                                   ByVal aNonpointData As atcTimeseriesGroup) As String
        Dim LoadingByLanduse As String = ""
        Dim lTotal As Double = 0
        For lAreaIndex As Integer = 0 To aAreas.Count - 1
            Dim lLocation As String = aAreas.Keys(lAreaIndex)
            Dim lArea As Double = aAreas.ItemByIndex(lAreaIndex)
            'Logger.Dbg("Area is " & lArea)
            'Dim lSubTotal As Double = 0
            lTotal = 0
            For Each lTs As atcTimeseries In aNonpointData.FindData("Location", lLocation)
                'lSubTotal += lTs.Attributes.GetValue("SumAnnual")

                If Not (Left(lLocation, 2) = "P:" And Left(lTs.Attributes.GetValue("Constituent").ToString, 6) = "SOQUAL") Then

                    Select Case lTs.Attributes.GetValue("Constituent").ToString & "_" & aBalanceType
                        Case "POQUAL-BOD_TotalN"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.069176
                            'Multiplying 0.048 + 0.05294*.4 to BOD to get Organic N
                        Case "SOQUAL-BOD_TotalN"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.069176
                        Case "POQUAL-BOD_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.005234
                            'Multiplying 0.0023 + 0.007326 to BOD to get Organic N
                        Case "SOQUAL-BOD_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.005234
                        Case Else
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual")

                    End Select
                End If

            Next
            LoadingByLanduse = LoadingByLanduse & vbCrLf & aReach.Id.ToString & vbTab & lLocation & vbTab & lTotal
            'lNewTotal += lArea * lSubTotal
        Next




        Return LoadingByLanduse

    End Function
End Module
