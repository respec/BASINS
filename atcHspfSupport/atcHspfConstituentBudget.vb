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
                           ByVal aRunMade As String) As Text.StringBuilder

        Dim lNumberFormat As String = "#,##0.0"
        Dim lUnits As String = ""
        Dim lNonpointData As New atcTimeseriesGroup

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
            Case Else
                Return New Text.StringBuilder("Budget report not yet defined for balance type '" & aBalanceType & "'")
        End Select

        Dim lUpstreamInflows As New atcCollection
        Dim lCumulativePointNonpointColl As New atcCollection

        Dim lSB As New Text.StringBuilder
        lSB.AppendLine(aScenario & " " & aBalanceType & " Average Annual Totals " & lUnits)
        lSB.AppendLine("   Run Made " & aRunMade)
        lSB.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lSB.AppendLine("   " & aUci.GlobalBlock.RunPeriod)

        Dim lOutputTable As New atcTableDelimited
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
            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Deposit-Scour"
            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"

            For Each lID As HspfOperation In lRchresOperations
                .CurrentRecord += 1
                Dim lAreas As New atcCollection
                LocationAreaCalc(aUci, "R:" & lID.Id, aOperationTypes, lAreas, False)

                Dim lNonpointTons As Double = TotalForReach(lID, lAreas, lNonpointData)

                'TODO: actually find point tons
                Dim lPointTons As Double = 0

                Dim lUpstreamIn As Double = 0
                If lUpstreamInflows.Keys.Contains(lID.Id) Then
                    lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                End If

                'TODO: these two formulations are slightly different - WHY?
                Dim lTotalInflow As Double = lNonpointTons + lPointTons + lUpstreamIn
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

            lSB.Append(.ToString)
            Return lSB
        End With
    End Function

    Private Function TotalForReach(ByVal aReach As HspfOperation, _
                                   ByVal aAreas As atcCollection, _
                                   ByVal aNonpointData As atcTimeseriesGroup) As Double
        Dim lTotal As Double = 0

        For lAreaIndex As Integer = 0 To aAreas.Count - 1
            Dim lLocation As String = aAreas.Keys(lAreaIndex)
            Dim lArea As Double = aAreas.ItemByIndex(lAreaIndex)
            For Each lTs As atcTimeseries In aNonpointData.FindData("Location", lLocation)
                lTotal += lArea * lTs.Attributes.GetValue("SumAnnual")
            Next
        Next
        Return lTotal
    End Function

    Private Function ValueForReach(ByVal aReach As HspfOperation, _
                                   ByVal aReachData As atcTimeseriesGroup) As Double
        Dim lReachData As atcTimeseries = aReachData.FindData("Location", "R:" & aReach.Id).Item(0)
        Dim lOutflow As Double = lReachData.Attributes.GetDefinedValue("SumAnnual").Value
        Return lOutflow
    End Function

End Module
