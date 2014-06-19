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

        Dim lNumberFormat As String = "#,##0.###"
        Dim lUnits As String = ""
        Dim lNonpointData As New atcTimeseriesGroup
        Dim lAtmDepData As New atcTimeseriesGroup
        Dim lTotalInflowData As New atcTimeseriesGroup
        Dim lTotalPrecipData As New atcTimeseriesGroup
        Dim lOutflowData As New atcTimeseriesGroup
        Dim lDepScourData As New atcTimeseriesGroup
        Dim lEvapLossData As New atcTimeseriesGroup
        Dim lRchresOperations As HspfOperations = aUci.OpnBlks("RCHRES").Ids

        Select Case aBalanceType
            Case "Water"
                lUnits = "(ac-ft)"
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "PERO"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SURO"))

                lTotalPrecipData.Add(aScenarioResults.DataSets.FindData("Constituent", "PRSUPY"))
                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "IVOL"))

                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ROVOL"))
                lEvapLossData.Add(aScenarioResults.DataSets.FindData("Constituent", "VOLEV"))

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

                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-P IN SOLUTION - INTERFLOW - OUTFLOW"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-P IN SOLUTION - GROUNDWATER - OUTFLOW"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SDP4A")) ' Outflow of sediment-associated PO4
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SDORP")) ' Organic P Outflow (will be multiplied by 0.6)
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "ORGN - TOTAL OUTFLOW")) ' Organic N Outflow (will be multiplied by 0.05534793)
                'lNonpointData.AddRange((aScenarioResults.DataSets.FindData("Constituent", "POPHOS")))
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

        lReport2.AppendLine("")
        lReport2.AppendLine(aScenario & " " & aBalanceType & " Average Annual Totals for Each Land Segment to Individual Reaches " & lUnits)
        lReport2.AppendLine("   Run Made " & aRunMade)
        lReport2.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport2.AppendLine("   " & aUci.GlobalBlock.RunPeriod)

        Dim YearsOfSimulation As Double = YearCount(aUci.GlobalBlock.SDateJ, aUci.GlobalBlock.EdateJ)
        Dim lOutputTable As New atcTableDelimited
        Dim lOutputTable2 As New atcTableDelimited
        Select Case aBalanceType
            Case "Water"

                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 11
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Local Drainage"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Inflow from Precipitation"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point & Other Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Loss from Evaporation"
                    'lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    'lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    'lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"

                    For Each lID As HspfOperation In lRchresOperations
                        .CurrentRecord += 1
                        Dim lAreas As New atcCollection
                        lReport2.Append(ConstituentLoadingByLanduse(lID, aBalanceType, lAreas, lNonpointData))
                        LocationAreaCalc(aUci, "R:" & lID.Id, aOperationTypes, lAreas, False)

                        Dim lNonpointTons As Double = TotalForReach(aUci, lID, aBalanceType, lAreas, lNonpointData) * 0.999906
                        lNonpointTons = lNonpointTons / 12 'Converting ac-in to ac-ft
                        'The factor of 0.999906 is to reduce the overestimation of loading from land surfaces and get a more reasonable value of Point sources.
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
                        Dim lInflowFromPrecip As Double = ValueForReach(lID, lTotalPrecipData)
                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        'lNonpointTons = lTotalInflow - lUpstreamIn
                        'Anurag changes are over
                        Dim lPointTons As Double = lTotalInflow - lNonpointTons - lUpstreamIn
                        'Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData) 'TotalForReach(lID, lAreas, lTotalInflowData)
                        If lPointTons < 0.0001 * lNonpointTons Then
                            lPointTons = 0.0
                        End If
                        'A negligible point source value is generated because of rounding errors when no point sources are present. 
                        'This is to make sure that output looks cleaner
                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                        Dim lEvapLoss As Double = ValueForReach(lID, lEvapLossData) 'TotalForReach(lID, lAreas, lDepScourData)
                        Dim lCumulativePointNonpoint As Double = lNonpointTons + lPointTons
                        If lCumulativePointNonpointColl.Keys.Contains(lID.Id) Then
                            lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lID.Id)
                        End If

                        Dim lReachTrappingEfficiency As Double
                        Try
                            lReachTrappingEfficiency = lEvapLoss / lTotalInflow
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
                        lField += 1 : .Value(lField) = DoubleToString(lInflowFromPrecip, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lPointTons, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lEvapLoss, , lNumberFormat)
                        'lField += 1 : .Value(lField) = DoubleToString(lCumulativePointNonpoint, , lNumberFormat)
                        'lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, , lNumberFormat, , , 6)
                        'lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)
                End With

            Case "Sediment"

                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 11
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point & Other Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Dep(+)/Scour(-)"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point & Other Sources (wdm)"
                    For Each lID As HspfOperation In lRchresOperations
                        Dim lPointAlternate As Double = 0.0
                        .CurrentRecord += 1
                        Dim lAreas As New atcCollection
                        lReport2.Append(ConstituentLoadingByLanduse(lID, aBalanceType, lAreas, lNonpointData))
                        LocationAreaCalc(aUci, "R:" & lID.Id, aOperationTypes, lAreas, False)

                        For Each lSource As HspfPointSource In lID.PointSources
                            If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "ISED" Then
                                Dim VolName As String = lSource.Source.VolName
                                Dim lDSN As Integer = lSource.Source.VolId
                                Dim lMfact As Double = lSource.MFact
                                For i As Integer = 0 To aUci.FilesBlock.Count

                                    If aUci.FilesBlock.Value(i).Typ = VolName Then
                                        Dim lFileName As String = CurDir() & "\" & Trim(aUci.FilesBlock.Value(i).Name)
                                        Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(CurDir() & "\" & lFileName)
                                        If lDataSource Is Nothing Then
                                            If atcDataManager.OpenDataSource(lFileName) Then
                                                lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                            End If
                                        End If
                                        Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                        ltimeseries = SubsetByDate(ltimeseries, aUci.GlobalBlock.SDateJ, aUci.GlobalBlock.EdateJ, Nothing)
                                        lPointAlternate += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation

                                    End If
                                Next


                            End If


                        Next


                        Dim lNonpointTons As Double = TotalForReach(aUci, lID, aBalanceType, lAreas, lNonpointData) * 0.999906
                        'The factor of 0.999906 is to reduce the overestimation of loading from land surfaces and get a more reasonable value of Point sources.
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
                        If lPointTons < 0.0001 * lNonpointTons Then
                            lPointTons = 0.0
                        End If
                        'A negligible point source value is generated because of rounding errors when no point sources are present. 
                        'This is to make sure that output looks cleaner
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
                        lField += 1 : .Value(lField) = DoubleToString(lPointAlternate, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)
                End With
            Case "TotalN"
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 12
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point & Other Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Atm. Depo. on Water"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Gain(+)/Loss(-)"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point and Other Sources (wdm)"

                    For Each lID As HspfOperation In lRchresOperations
                        Dim lPointAlternate As Double = 0.0
                        .CurrentRecord += 1
                        Dim lAreas As New atcCollection
                        LocationAreaCalc(aUci, "R:" & lID.Id, aOperationTypes, lAreas, False)

                        For Each lSource As HspfPointSource In lID.PointSources
                            If lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1) _
                                                                        Or (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 2) _
                                                                        Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 3) _
                                                                        Or (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then
                                Dim VolName As String = lSource.Source.VolName
                                Dim lDSN As Integer = lSource.Source.VolId
                                Dim lMfact As Double = lSource.MFact
                                If lSource.Target.Member = "OXIF" Then lMfact *= 0.05294
                                For i As Integer = 0 To aUci.FilesBlock.Count

                                    If aUci.FilesBlock.Value(i).Typ = VolName Then
                                        Dim lFileName As String = CurDir() & "\" & Trim(aUci.FilesBlock.Value(i).Name)
                                        Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                        If lDataSource Is Nothing Then
                                            If atcDataManager.OpenDataSource(lFileName) Then
                                                lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                            End If
                                        End If
                                        Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                        ltimeseries = SubsetByDate(ltimeseries, aUci.GlobalBlock.SDateJ, aUci.GlobalBlock.EdateJ, Nothing)
                                        lPointAlternate += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation

                                    End If
                                Next


                            End If


                        Next
                        Dim lNonpointlbs As Double = TotalForReach(aUci, lID, aBalanceType, lAreas, lNonpointData) * 0.999906
                        'The factor of 0.999906 is to reduce the overestimation of loading from land surfaces and get a more reasonable value of Point sources.
                        lReport2.Append(ConstituentLoadingByLanduse(lID, aBalanceType, lAreas, lNonpointData))
                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If

                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        Dim lPointlbs As Double = 0

                        Dim lTotalAtmDep As Double = 0
                        If lAtmDepData.Count > 0 Then
                            lTotalAtmDep = ValueForReach(lID, lAtmDepData.FindData("Constituent", "NO3-ATMDEPTOT"))
                            lTotalAtmDep += ValueForReach(lID, lAtmDepData.FindData("Constituent", "TAM-ATMDEPTOT"))
                        End If

                        lPointlbs = lTotalInflow - lNonpointlbs - lTotalAtmDep - lUpstreamIn
                        If lPointlbs < (0.0002 * lNonpointlbs) Then
                            lPointlbs = 0.0
                        End If
                        'A negligible point source value is generated because of rounding errors when no point sources are present. 
                        'This is to make sure that output looks cleaner

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
                        lField += 1 : .Value(lField) = DoubleToString(lNonpointlbs, 15, lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lPointlbs, 15, lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalAtmDep, 15, lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, 15, lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, 15, lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lOutflow, 15, lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lDepScour, 15, lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lCumulativePointNonpoint, 15, lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, , lNumberFormat, , , 6)
                        lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lPointAlternate, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)

                End With
                lReport.Append(lReport2.ToString)
            Case "TotalP"
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 11
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point & Other Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Gain(+)/Loss(-)"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point and Other Sources (wdm)"
                    For Each lID As HspfOperation In lRchresOperations
                        Dim lPointAlternate As Double = 0.0
                        .CurrentRecord += 1
                        Dim lAreas As New atcCollection
                        lReport2.Append(ConstituentLoadingByLanduse(lID, aBalanceType, lAreas, lNonpointData))
                        LocationAreaCalc(aUci, "R:" & lID.Id, aOperationTypes, lAreas, False)

                        For Each lSource As HspfPointSource In lID.PointSources
                            If lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 4) _
                                                                       Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 4) _
                                                                       Or (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then
                                Dim VolName As String = lSource.Source.VolName
                                Dim lDSN As Integer = lSource.Source.VolId
                                Dim lMfact As Double = lSource.MFact
                                If lSource.Target.Member = "OXIF" Then lMfact *= 0.007326
                                For i As Integer = 0 To aUci.FilesBlock.Count

                                    If aUci.FilesBlock.Value(i).Typ = VolName Then
                                        Dim lFileName As String = Trim(aUci.FilesBlock.Value(i).Name)
                                        Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(CurDir() & "\" & lFileName)
                                        If lDataSource Is Nothing Then
                                            If atcDataManager.OpenDataSource(lFileName) Then
                                                lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                            End If
                                        End If
                                        Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                        ltimeseries = SubsetByDate(ltimeseries, aUci.GlobalBlock.SDateJ, aUci.GlobalBlock.EdateJ, Nothing)
                                        lPointAlternate += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation

                                    End If
                                Next


                            End If


                        Next
                        Dim lNonpointlbs As Double = TotalForReach(aUci, lID, aBalanceType, lAreas, lNonpointData) * 0.999906
                        'The factor of 0.999906 is to reduce the overestimation of loading from land surfaces and get a more reasonable value of Point sources.
                        lReport2.Append(ConstituentLoadingByLanduse(lID, aBalanceType, lAreas, lNonpointData))
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
                        If lPointlbs < 0.00025 * lNonpointlbs Then
                            lPointlbs = 0.0
                        End If
                        'A negligible point source value is generated because of rounding errors when no point sources are present. 
                        'This is to make sure that output looks cleaner
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
                        lField += 1 : .Value(lField) = DoubleToString(lPointAlternate, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)

                End With
                lReport.Append(lReport2.ToString)
        End Select
        Return lReport
    End Function

    Private Function TotalForReach(ByVal aUCI As HspfUci, _
                                    ByVal aReach As HspfOperation, _
                                    ByVal aBalanceType As String, _
                                    ByVal aAreas As atcCollection, _
                                   ByVal aNonpointData As atcTimeseriesGroup) As Double
        Dim lTotal As Double = 0
        'Dim lNewTotal As Double = 0
        For lAreaIndex As Integer = 0 To aAreas.Count - 1
            Dim lLocation As String = aAreas.Keys(lAreaIndex)
            Dim lArea As Double = aAreas.ItemByIndex(lAreaIndex)
            Dim lMassLinkMultFactor As Double = 0
            Dim lMassLinkID As Integer
            'Logger.Dbg("Area is " & lArea)
            'Dim lSubTotal As Double = 0
            For Each lTs As atcTimeseries In aNonpointData.FindData("Location", lLocation)
                If aBalanceType = "Sediment" Then

                    For Each lMLBlockConnection As HspfConnection In aUCI.Connections
                        If lMLBlockConnection.Source.VolId = Right(lLocation, lLocation.Length - 2) _
                        AndAlso Left(lMLBlockConnection.Source.VolName, 1) = Left(lLocation, 1) _
                        AndAlso lMLBlockConnection.Target.VolName = "RCHRES" _
                        AndAlso lMLBlockConnection.Target.Opn.Id = aReach.Id Then
                            lMassLinkID = lMLBlockConnection.MassLink
                            For Each lMasslink As HspfMassLink In aUCI.MassLinks

                                If lMasslink.MassLinkId = lMassLinkID AndAlso lMasslink.Target.Member.Contains("ISED") _
                                AndAlso lMasslink.Target.MemSub1 <= 3 Then
                                    lMassLinkMultFactor += lMasslink.MFact
                                End If
                            Next
                            Exit For
                        End If
                    Next lMLBlockConnection
                Else
                    lMassLinkMultFactor = 1
                End If

                If Not (Left(lLocation, 2) = "P:" And (Left(lTs.Attributes.GetValue("Constituent").ToString, 6) = "SOQUAL" Or _
                                                       Left(lTs.Attributes.GetValue("Constituent").ToString, 6) = "SURO")) Then

                    'This condition makes sure that surface washoff from PERLND are not added as they are already added in POQUAL.
                    'Anurag checked this on 3/5/2014

                    Select Case lTs.Attributes.GetValue("Constituent").ToString & "_" & aBalanceType
                        Case "POQUAL-BOD_TotalN"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.069176
                            'Multiplying 0.048 + 0.05294*.4 to BOD to get Organic N
                        Case "SOQUAL-BOD_TotalN"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.069176
                        Case "POQUAL-BOD_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.0052304
                            'Multiplying 0.0023 + 0.4*0.007326 to BOD to get Organic P
                            'The factor is 0.0052304 for most watersheds, but it was changed to
                            '0.0075304 becase PQUAL 4 to PKIF 4 factor was changed from 0.0023 to 0.0046
                        Case "SOQUAL-BOD_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.0052304
                        Case "ORGN - TOTAL OUTFLOW_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.05534793
                        Case "SDORP_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.6
                            'PERLND     PHOS   SEDP   1     1.20        RCHRES         INFLOW PKIF   4  
                            'The factor was changed frm 0.6 to 1.20in IRW
                        Case Else
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * lMassLinkMultFactor

                    End Select
                End If

                lMassLinkMultFactor = 0
            Next

        Next

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

                If Not (Left(lLocation, 2) = "P:" And (Left(lTs.Attributes.GetValue("Constituent").ToString, 6) = "SOQUAL" Or _
                                                       Left(lTs.Attributes.GetValue("Constituent").ToString, 6) = "SURO")) Then

                    Select Case lTs.Attributes.GetValue("Constituent").ToString & "_" & aBalanceType
                        Case "POQUAL-BOD_TotalN"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.069176
                            'Multiplying 0.048 + 0.05294*.4 to BOD to get Organic N
                        Case "SOQUAL-BOD_TotalN"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.069176
                        Case "POQUAL-BOD_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.0052304
                            'Multiplying 0.0023 + 0.4*0.007326 to BOD to get Organic P
                        Case "SOQUAL-BOD_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.0052304
                            'Multiplying 0.0023 + 0.4*0.007326 to BOD to get Organic P for IRW
                            'The factor is 0.0052304 for most watersheds, but it was changed to
                            '0.0075304 becase PQUAL 4 to PKIF 4 factor was changed from 0.0023 to 0.0046
                        Case "ORGN - TOTAL OUTFLOW_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.05534793
                        Case "SDORP_TotalP"
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual") * 0.6
                            'PERLND     PHOS   SEDP   1     1.20        RCHRES         INFLOW PKIF   4  
                            'The factor was changed frm 0.6 to 1.20in IRW
                        Case Else
                            lTotal += lArea * lTs.Attributes.GetValue("SumAnnual")

                    End Select
                End If

            Next
            lTotal = lTotal * 0.999906
            Select Case lLocation.Substring(0, 1)
                Case "P"
                    LoadingByLanduse &= vbCrLf & aReach.Caption.ToString.Substring(10) & vbTab & lLocation & vbTab _
                        & aReach.Uci.OpnBlks("PERLND").OperFromID(lLocation.Substring(2)).Description & vbTab & lTotal
                Case "I"
                    LoadingByLanduse &= vbCrLf & aReach.Caption.ToString.Substring(10) & vbTab & lLocation & vbTab _
                        & aReach.Uci.OpnBlks("IMPLND").OperFromID(lLocation.Substring(2)).Description & vbTab & lTotal

            End Select

            'lNewTotal += lArea * lSubTotal
        Next

        Return LoadingByLanduse

    End Function
End Module
