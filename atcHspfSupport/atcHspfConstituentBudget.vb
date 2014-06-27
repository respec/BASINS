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
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "SOSLD")))

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
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "POQUAL-Ortho P")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-Ortho P")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "POQUAL-BOD")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-BOD")))
                lAtmDepData.Add((aScenarioResults.DataSets.FindData("Constituent", "PO4-ATMDEPTOT")))

                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "P-TOT-IN"))
                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "P-TOT-OUT"))
            Case Else

                Return New atcReport.ReportText("Budget report not yet defined for balance type '" & aBalanceType & "'")
        End Select

        Dim lUpstreamInflows As New atcCollection
        Dim lCumulativePointNonpointColl As New atcCollection

        Dim lReport As New atcReport.ReportText
        Dim lReport2 As New atcReport.ReportText
        Dim lReport3 As New atcReport.ReportText
        lReport.AppendLine(aScenario & " " & aBalanceType & " Average Annual Totals " & lUnits)
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunPeriod)

        lReport2.AppendLine("")
        lReport2.AppendLine(aScenario & " " & aBalanceType & " Average Annual Totals for Each Land Segment to Individual Reaches " & lUnits)
        lReport2.AppendLine("   Run Made " & aRunMade)
        lReport2.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport2.AppendLine("   " & aUci.GlobalBlock.RunPeriod)


        lReport3.AppendLine(aScenario & " " & aBalanceType & " Average Annual Totals of Individual Constituents from Each Land Segment to Individual Reaches " & lUnits)
        lReport3.AppendLine("   Run Made " & aRunMade)
        lReport3.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport3.AppendLine("   " & aUci.GlobalBlock.RunPeriod)


        Dim YearsOfSimulation As Double = YearCount(aUci.GlobalBlock.SDateJ, aUci.GlobalBlock.EdateJ)
        Dim lOutputTable As New atcTableDelimited
        Dim lOutputTable2 As New atcTableDelimited
        Select Case aBalanceType
            Case "Water"
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Landuse" & vbTab & "Total Load (ac-ft)" & vbTab & "Rate (ft/ac)")
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 8
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Local Drainage"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Sources"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Inflow from Precipitation"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Loss from Evaporation"

                    For Each lID As HspfOperation In lRchresOperations
                        .CurrentRecord += 1
                        
                        Dim lPointAlternate As Double = 0.0
                        For Each lSource As HspfPointSource In lID.PointSources
                            If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IVOL" Then
                                Dim VolName As String = lSource.Source.VolName
                                Dim lDSN As Integer = lSource.Source.VolId
                                Dim lMfact As Double = lSource.MFact
                                For i As Integer = 0 To aUci.FilesBlock.Count

                                    If aUci.FilesBlock.Value(i).Typ = VolName Then
                                        Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
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

                        Dim lNonpointVol As Double
                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, 0.0)
                            lReport2.Append(.Item1)
                            lNonpointVol = .Item2
                        End With

                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If

                        Dim lInflowFromPrecip As Double = ValueForReach(lID, lTotalPrecipData)
                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        Dim lPointVol As Double = 0.0
                        lPointVol = lTotalInflow - lNonpointVol - lUpstreamIn
                        If lPointVol < 0.0001 * lNonpointVol Then
                            lPointVol = 0.0
                        End If
                        'A negligible point source value is generated because of rounding errors when no point sources are present. 
                        'This is to make sure that output looks cleaner
                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                        Dim lEvapLoss As Double = ValueForReach(lID, lEvapLossData) 'TotalForReach(lID, lAreas, lDepScourData)
                        Dim lCumulativePointNonpoint As Double = lNonpointVol + lPointVol
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
                        lField += 1 : .Value(lField) = DoubleToString(lNonpointVol, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lPointAlternate, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lInflowFromPrecip, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lEvapLoss, , lNumberFormat)
                       
                    Next
                    lReport.Append(.ToString)
                End With
                lReport.Append(lReport2.ToString)
            Case "Sediment"
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Landuse" & vbTab & "Total Load (tons)" & vbTab & "Rate (tons/ac)")
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

                        For Each lSource As HspfPointSource In lID.PointSources
                            If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "ISED" Then
                                Dim VolName As String = lSource.Source.VolName
                                Dim lDSN As Integer = lSource.Source.VolId
                                Dim lMfact As Double = lSource.MFact
                                For i As Integer = 0 To aUci.FilesBlock.Count
                                    If aUci.FilesBlock.Value(i).Typ = VolName Then
                                        Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
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

                        Dim lNonpointTons As Double
                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, 0.0)
                            lReport2.Append(.Item1)
                            lNonpointTons = .Item2

                        End With

                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If

                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)

                        Dim lPointTons As Double = lTotalInflow - lNonpointTons - lUpstreamIn

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
                lReport.Append(lReport2.ToString)
            Case "TotalN"
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Landuse" & vbTab & "Total Load (lbs)" & vbTab & "Rate (lbs/ac)")
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

                        Dim lOperationIndex As Integer = aUci.OpnSeqBlock.Opns.IndexOf(lID)
                        Dim CVBO As Double = aUci.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBO").Value
                        'conversion from mg biomass to mg oxygen
                        Dim CVBPC As Double = aUci.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBPC").Value
                        'conversion from biomass expressed as P to C = ratio of moles of C to moles of P in biomass		
                        Dim CVBPN As Double = aUci.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBPN").Value
                        'conversion from biomass expressed as P to N = ratio of moles of N to moles of P in biomass		
                        Dim BPCNTC As Double = aUci.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("BPCNTC").Value
                        'conversion from biomass expressed as P to N = ratio of moles of N to moles of P in biomass		
                        Dim CVBN As Double = 14 * CVBPN * BPCNTC / 1200 / CVBPC
                        'conversion from biomass to N
                        Dim CVON As Double = CVBN / CVBO
                        'conversion from oxygen to N

                        For Each lSource As HspfPointSource In lID.PointSources
                            If lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1) _
                                                                        Or (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 2) _
                                                                        Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 3) _
                                                                        Or (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then
                                Dim VolName As String = lSource.Source.VolName
                                Dim lDSN As Integer = lSource.Source.VolId
                                Dim lMfact As Double = lSource.MFact
                                If lSource.Target.Member = "OXIF" Then lMfact *= CVON
                                For i As Integer = 0 To aUci.FilesBlock.Count

                                    If aUci.FilesBlock.Value(i).Typ = VolName Then
                                        Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
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

                        Dim lNonpointlbs As Double
                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, CVON)
                            lReport2.Append(.Item1)
                            lNonpointlbs = .Item2
                        End With

                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If

                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        Dim lPointlbs As Double = 0

                        Dim lTotalAtmDep As Double = 0
                        If lAtmDepData.Count > 0 Then
                            lTotalAtmDep = ValueForReach(lID, lAtmDepData)
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
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Landuse" & vbTab & "Total Load (lbs)" & vbTab & "Rate (lbs/ac)")
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

                        Dim lOperationIndex As Integer = aUci.OpnSeqBlock.Opns.IndexOf(lID)
                        Dim CVBO As Double = aUci.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBO").Value
                        'conversion from mg biomass to mg oxygen
                        Dim CVBPC As Double = aUci.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBPC").Value
                        'conversion from biomass expressed as P to C = ratio of moles of C to moles of P in biomass		
                        Dim CVBPN As Double = aUci.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBPN").Value
                        'conversion from biomass expressed as P to N = ratio of moles of N to moles of P in biomass		
                        Dim BPCNTC As Double = aUci.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("BPCNTC").Value
                        'conversion from biomass expressed as P to N = ratio of moles of N to moles of P in biomass		
                        Dim CVBP As Double = 31 * BPCNTC / 1200 / CVBPC
                        'conversion from biomass to P
                        Dim CVOP As Double = CVBP / CVBO
                        'conversion from oxygen to P

                        For Each lSource As HspfPointSource In lID.PointSources
                            If lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 4) _
                                                                       Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 4) _
                                                                       Or (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then
                                Dim VolName As String = lSource.Source.VolName
                                Dim lDSN As Integer = lSource.Source.VolId
                                Dim lMfact As Double = lSource.MFact
                                If lSource.Target.Member = "OXIF" Then lMfact *= CVOP
                                For i As Integer = 0 To aUci.FilesBlock.Count

                                    If aUci.FilesBlock.Value(i).Typ = VolName Then
                                        Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
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
                        Dim lNonpointlbs As Double

                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, CVOP)
                            lReport2.Append(.Item1)
                            lNonpointlbs = .Item2
                        End With

                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If

                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                        Dim lDepScour As Double = lOutflow - lTotalInflow
                        Dim lTotalAtmDep As Double = 0
                        If lAtmDepData.Count > 0 Then
                            lTotalAtmDep = ValueForReach(lID, lAtmDepData)
                        End If
                        Dim lPointlbs As Double = 0

                        lPointlbs = lTotalInflow - lNonpointlbs - lUpstreamIn - lTotalAtmDep
                        If lPointlbs < 0.0003 * lNonpointlbs Then
                            lPointlbs = 0.0
                        End If
                        Dim lCumulativePointNonpoint As Double = lNonpointlbs + lPointlbs
                        If lCumulativePointNonpointColl.Keys.Contains(lID.Id) Then
                            lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lID.Id)
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

                lMassLinkMultFactor = 0
            Next

        Next

            Return lTotal
    End Function

    Private Function ValueForReach(ByVal aReach As HspfOperation, _
                                   ByVal aReachData As atcTimeseriesGroup) As Double
        Dim lOutFlow As Double
        For Each aTimeseries As atcTimeseries In aReachData.FindData("Location", "R:" & aReach.Id)
            lOutflow += aTimeseries.Attributes.GetValue("SumAnnual")
        Next
        'Dim lReachData As atcTimeseries = aReachData.FindData("Location", "R:" & aReach.Id).Item(0)
        'Dim lOutflow As Double = lReachData.Attributes.GetValue("SumAnnual")
        Return lOutflow
    End Function
    Private Function ConstituentLoadingByLanduse(ByVal aUCI As HspfUci, _
                                                 ByVal aReach As HspfOperation, _
                                    ByVal aBalanceType As String, _
                                    ByVal aNonpointData As atcTimeseriesGroup, _
                                   ByVal aConversionFactor As Double) As Tuple(Of String, Double)
        Dim LoadingByLanduse As String = ""
        Dim lReachTotal As Double = 0.0
        Dim lTotal As Double = 0
        Dim lConnectionArea As Double
        Dim lMassLinkID As Integer
        Dim lTestLocation As String
        Dim lConstituentTotal As Double
        Dim lConstituentRate As Double
        Dim NewReport As String
        NewReport = aReach.Name & vbCrLf

        For Each lConnection As HspfConnection In aReach.Sources
            If lConnection.Source.VolName = "PERLND" Then

                lConnectionArea = lConnection.MFact
                lMassLinkID = lConnection.MassLink
                lTestLocation = lConnection.Source.VolName.Substring(0, 1) & ":" & lConnection.Source.VolId
                NewReport &= vbTab & lTestLocation
                lTotal = 0
                For Each lTs As atcTimeseries In aNonpointData.FindData("Location", lTestLocation)
                    Dim lMassLinkFactor As Double = FindMassLinkFactor(aUCI, lMassLinkID, lTs.Attributes.GetValue("Constituent"), aBalanceType, _
                                                                   True, aConversionFactor)
                    lConstituentRate = lTs.Attributes.GetValue("SumAnnual") * lMassLinkFactor
                    lConstituentTotal = lConstituentRate * lConnectionArea

                    lTotal += lConnectionArea * lTs.Attributes.GetValue("SumAnnual") * lMassLinkFactor
                Next
                LoadingByLanduse &= vbCrLf & aReach.Caption.ToString.Substring(10) & vbTab & lTestLocation & vbTab _
                                        & aReach.Uci.OpnBlks("PERLND").OperFromID(lTestLocation.Substring(2)).Description & vbTab & lTotal _
                                        & vbTab & lTotal / lConnectionArea
                lReachTotal += lTotal
            End If

        Next

        For Each lConnection As HspfConnection In aReach.Sources

            If lConnection.Source.VolName = "IMPLND" Then
                lConnectionArea = lConnection.MFact
                lMassLinkID = lConnection.MassLink
                lTestLocation = lConnection.Source.VolName.Substring(0, 1) & ":" & lConnection.Source.VolId
                lTotal = 0
                For Each lTs As atcTimeseries In aNonpointData.FindData("Location", lTestLocation)
                    Dim lMassLinkFactor As Double = FindMassLinkFactor(aUCI, lMassLinkID, lTs.Attributes.GetValue("Constituent"), aBalanceType, _
                                                                   False, aConversionFactor)
                    lConstituentRate = lTs.Attributes.GetValue("SumAnnual") * lMassLinkFactor
                    lConstituentTotal = lConstituentRate * lConnectionArea
                    lTotal += lConnectionArea * lTs.Attributes.GetValue("SumAnnual") * lMassLinkFactor
                Next
                LoadingByLanduse &= vbCrLf & aReach.Caption.ToString.Substring(10) & vbTab & lTestLocation & vbTab _
                        & aReach.Uci.OpnBlks("IMPLND").OperFromID(lTestLocation.Substring(2)).Description & vbTab & lTotal _
                        & vbTab & lTotal / lConnectionArea

                lReachTotal += lTotal
            End If

        Next

        Return New Tuple(Of String, Double)(LoadingByLanduse, lReachTotal)


    End Function

    Private Function BODMFact(ByVal aUCI As HspfUci, ByVal aMassLinkID As Integer) As Double
        For Each lMasslink As HspfMassLink In aUCI.MassLinks
            If lMasslink.MassLinkId = aMassLinkID AndAlso lMasslink.Target.Member = "OXIF" AndAlso lMasslink.Target.MemSub1 = 2 Then
                Return lMasslink.MFact

            End If
        Next lMasslink
    End Function

    Private Function FindMassLinkObject(ByVal aUCI As HspfUci, ByVal aReach As HspfOperation, ByVal aLocation As String) As HspfMassLink
        For Each lMLBlockConnection As HspfConnection In aReach.Sources
            If lMLBlockConnection.Source.VolId = Right(aLocation, aLocation.Length - 2) _
                AndAlso Left(lMLBlockConnection.Source.VolName, 1) = Left(aLocation, 1) Then
                Dim lMassLinkID As Integer = lMLBlockConnection.MassLink
                For Each lMasslink As HspfMassLink In aUCI.MassLinks
                    If lMasslink.MassLinkId = lMassLinkID Then
                        Return lMasslink
                    End If

                Next
            End If
        Next
        Return Nothing
    End Function

    Private Function FindMassLinkFactor(ByVal aUCI As HspfUci, ByVal aMassLink As Integer, ByVal aConstituent As String, _
                                        ByVal aBalanceType As String, ByVal PERLND As Boolean, ByVal aConversionFactor As Double) As Double
        Dim lMassLinkFactor As Double = 0.0
        For Each lMassLink As HspfMassLink In aUCI.MassLinks

            If lMassLink.MassLinkId = aMassLink Then

                Select Case aBalanceType
                    Case "Sediment"
                        If lMassLink.Target.Member = "ISED" Then
                            lMassLinkFactor += lMassLink.MFact
                        End If
                    Case "Water"
                        If aConstituent = "PERO" And lMassLink.Target.Member = "IVOL" And PERLND Then
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        ElseIf aConstituent = "SURO" And lMassLink.Target.Member = "IVOL" And Not PERLND Then
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        End If

                    Case "TotalN"

                        Select Case aConstituent & "_" & lMassLink.Target.Member.ToString & _
                            "_" & lMassLink.Target.MemSub1
                            Case "POQUAL-NH3+NH4_NUIF1_2", "POQUAL-NO3_NUIF1_1"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "SOQUAL-NH3+NH4_NUIF1_2", "SOQUAL-NO3_NUIF1_1"
                                lMassLinkFactor = lMassLink.MFact
                                If PERLND Then lMassLinkFactor = 0
                                Return lMassLinkFactor
                            Case "POQUAL-BOD_PKIF_3"
                                lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, lMassLink.MassLinkId) * aConversionFactor
                                Return lMassLinkFactor
                            Case "SOQUAL-BOD_PKIF_3"
                                lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, lMassLink.MassLinkId) * aConversionFactor
                                If PERLND Then lMassLinkFactor = 0
                                Return lMassLinkFactor
                        End Select
                        If aConstituent.Contains("NITROGEN - TOTAL OUTFLOW") Then
                            lMassLinkFactor = 1
                            Return lMassLinkFactor
                        End If


                    Case "TotalP"
                        Select Case aConstituent & "_" & lMassLink.Target.Member.ToString & _
                            "_" & lMassLink.Target.MemSub1
                            Case "POQUAL-ORTHO P_NUIF1_4"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "SOQUAL-ORTHO P_NUIF1_4"
                                lMassLinkFactor = lMassLink.MFact
                                If PERLND Then lMassLinkFactor = 0
                                Return lMassLinkFactor
                            Case "POQUAL-BOD_PKIF_4"
                                lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, lMassLink.MassLinkId) * aConversionFactor
                                Return lMassLinkFactor
                            Case "SOQUAL-BOD_PKIF_4"
                                lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, lMassLink.MassLinkId) * aConversionFactor
                                If PERLND Then lMassLinkFactor = 0
                                Return lMassLinkFactor
                            Case "PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW_NUIF1_4"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "PO4-P IN SOLUTION - INTERFLOW - OUTFLOW_NUIF1_4"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "PO4-P IN SOLUTION - GROUNDWATER - OUTFLOW_NUIF1_4"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "SDP4A_NUIF2_1"
                                If lMassLink.Target.MemSub2 = 2 Then
                                    lMassLinkFactor += lMassLink.MFact
                                End If
                            Case "SDP4A_NUIF2_2"
                                If lMassLink.Target.MemSub2 = 2 Then
                                    lMassLinkFactor += lMassLink.MFact
                                End If
                            Case "SDP4A_NUIF2_3"
                                If lMassLink.Target.MemSub2 = 2 Then
                                    lMassLinkFactor += lMassLink.MFact
                                End If

                            Case "SDORP_PKIF_4"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "ORGN - TOTAL OUTFLOW_OXIF_2"
                                lMassLinkFactor = lMassLink.MFact * aConversionFactor
                                Return lMassLinkFactor

                        End Select

                End Select


            End If

        Next

        Return lMassLinkFactor


    End Function
End Module
