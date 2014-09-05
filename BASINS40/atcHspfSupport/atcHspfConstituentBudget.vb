Imports atcUtility
Imports atcData
Imports atcUCI
Imports MapWinUtility

Public Module ConstituentBudget
    Private pTotals As New atcCollection
    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aBalanceType As String, _
                           ByVal aOperationTypes As atcCollection, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcTimeseriesSource, _
                           ByVal aRunMade As String) As Tuple(Of atcReport.IReport, atcReport.IReport, atcReport.IReport)

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
        Dim lReport As New atcReport.ReportText
        Dim lReport2 As New atcReport.ReportText
        Dim lReport3 As New atcReport.ReportText

        Select Case aBalanceType
            Case "Water"
                lUnits = "(ac-ft)"

                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SURO"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "IFWO"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "AGWO"))

                lTotalPrecipData.Add(aScenarioResults.DataSets.FindData("Constituent", "PRSUPY"))
                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "IVOL"))

                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ROVOL"))
                lEvapLossData.Add(aScenarioResults.DataSets.FindData("Constituent", "VOLEV"))

            Case "Sediment"
                lUnits = "(tons)"
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "WSSD"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SCRSD"))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "SOSLD")))

                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ISED-TOT"))
                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ROSED-TOT"))
                lDepScourData.Add(aScenarioResults.DataSets.FindData("Constituent", "DEPSCOUR-TOT"))
            Case "TotalN"
                lUnits = "(lbs)"

                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "NITROGEN - TOTAL OUTFLOW"))

                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-NO3"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "AOQUAL-NO3"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "IOQUAL-NO3"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-NH3+NH4"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "IOQUAL-NH3+NH4"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "AOQUAL-NH3+NH4"))

                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-BOD"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "IOQUAL-BOD"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "AOQUAL-BOD"))

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
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-Ortho P")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "IOQUAL-Ortho P")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "AOQUAL-Ortho P")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-BOD")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "IOQUAL-BOD")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "AOQUAL-BOD")))
                lAtmDepData.Add((aScenarioResults.DataSets.FindData("Constituent", "PO4-ATMDEPTOT")))

                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "P-TOT-IN"))
                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "P-TOT-OUT"))
            Case Else
                lReport.AppendLine("Budget report not yet defined for balance type '" & aBalanceType & "'")
                lReport2.AppendLine("Budget report not yet defined for balance type '" & aBalanceType & "'")
                lReport3.AppendLine(("Budget report not yet defined for balance type '" & aBalanceType & "'"))
                Return New Tuple(Of atcReport.IReport, atcReport.IReport, atcReport.IReport)(lReport, lReport2, lReport3)

        End Select

        Dim lUpstreamInflows As New atcCollection
        Dim lCumulativePointNonpointColl As New atcCollection


        lReport.AppendLine(aScenario & " " & aBalanceType & " Average Annual Totals " & lUnits)
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunPeriod)


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
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Landuse" & vbTab & "Area (ac)" & vbTab & _
                                    "Rate (ft/ac)" & vbTab & "Total Load (ac-ft)")
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 9
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
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Additional Source"

                    For Each lID As HspfOperation In lRchresOperations
                        .CurrentRecord += 1

                        Dim lPointVol As Double = 0.0
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
                                        lPointVol += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation

                                    End If
                                Next

                            End If
                        Next
                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If
                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        Dim lNonpointVol As Double

                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, 0.0, lPointVol, 0.0, 0.0, lTotalInflow, lUpstreamIn)
                            lReport2.Append(.Item1)
                            lNonpointVol = .Item2
                        End With



                        Dim lInflowFromPrecip As Double = ValueForReach(lID, lTotalPrecipData)


                        'If lAdditionalSource < 0.0001 * lTotalInflow Then
                        '    lAdditionalSource = 0.0
                        'End If
                        'A negligible point source value is generated because of rounding errors when no point sources are present. 
                        'This is to make sure that output looks cleaner
                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData)
                        Dim lEvapLoss As Double = ValueForReach(lID, lEvapLossData)
                        Dim lAdditionalSource As Double = 0.0
                        lAdditionalSource = lTotalInflow - lNonpointVol - lUpstreamIn - lPointVol - lInflowFromPrecip + lEvapLoss
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
                        lField += 1 : .Value(lField) = DoubleToString(lPointVol, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lInflowFromPrecip, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lEvapLoss, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lAdditionalSource, , lNumberFormat)

                    Next
                    lReport.Append(.ToString)
                End With

            Case "Sediment"
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Landuse" & vbTab & "Area (ac)" & _
                                    vbTab & "Rate (tons/ac)" & vbTab & "Total Load (tons)")
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 11
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Dep(+)/Scour(-)"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = "(%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = "(%)" : .FieldName(lField) = "Reach Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Additional Sources"
                    For Each lID As HspfOperation In lRchresOperations
                        Dim lPointTons As Double = 0.0
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
                                        lPointTons += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation
                                    End If
                                Next

                            End If
                        Next



                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If

                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)



                        'If lAdditionalSourceTons < 0.0001 * lNonpointTons Then
                        '    lAdditionalSourceTons = 0.0
                        'End If
                        'A negligible point source value is generated because of rounding errors when no point sources are present. 
                        'This is to make sure that output looks cleaner
                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                        Dim lDepScour As Double = ValueForReach(lID, lDepScourData) 'TotalForReach(lID, lAreas, lDepScourData)
                        Dim lNonpointTons As Double
                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, 0.0, lPointTons, 0.0, lDepScour, lTotalInflow, lUpstreamIn)
                            lReport2.Append(.Item1)
                            lNonpointTons = .Item2

                        End With
                        Dim lAdditionalSourceTons As Double = lTotalInflow - lNonpointTons - lUpstreamIn - lPointTons
                        Dim lCumulativePointNonpoint As Double = lNonpointTons + lAdditionalSourceTons
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
                        lField += 1 : .Value(lField) = DoubleToString(lAdditionalSourceTons, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)
                End With

            Case "TotalN"
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Landuse" & vbTab & "Area (ac)" & vbTab & _
                                    "Rate (lbs/ac)" & vbTab & "Total Load (lbs)" & vbTab & "Percent of Total Load (%)" & vbTab & "Effective Load (lbs)")
                lReport3.AppendLine("Reach" & vbTab & "Source" & vbTab & "Load")
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 12
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Atm. Depo. on Water"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Gain(+)/Loss(-)"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Additional Sources (wdm)"

                    For Each lID As HspfOperation In lRchresOperations
                        Dim lPointlbs As Double = 0.0
                        .CurrentRecord += 1

                        Dim CVON As Double = ConversionFactorfromOxygen(aUci, aBalanceType, lID)
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
                                        lPointlbs += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation

                                    End If
                                Next
                            End If

                        Next

                        Dim lNonpointlbs As Double = 0.0


                        Dim lUpstreamIn As Double = 0.0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If

                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        Dim lAdditionalSourcelbs As Double = 0

                        Dim lTotalAtmDep As Double = 0
                        If lAtmDepData.Count > 0 Then
                            lTotalAtmDep = ValueForReach(lID, lAtmDepData)
                        End If


                        'If lAdditionalSourcelbs < (0.0002 * lNonpointlbs) Then
                        '    lAdditionalSourcelbs = 0.0
                        'End If
                        'A negligible point source value is generated because of rounding errors when no point sources are present. 
                        'This is to make sure that output looks cleaner

                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                        Dim lDepScour As Double = lOutflow - lTotalInflow
                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, CVON, lPointlbs, lTotalAtmDep, lDepScour, lTotalInflow, lUpstreamIn)
                            lReport2.Append(.Item1)
                            lNonpointlbs = .Item2
                        End With
                        lAdditionalSourcelbs = lTotalInflow - lNonpointlbs - lTotalAtmDep - lUpstreamIn - lPointlbs
                        Dim lCumulativePointNonpoint As Double = lNonpointlbs + lAdditionalSourcelbs
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
                        lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, 15, lNumberFormat, , , 6)
                        lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, 15, lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lAdditionalSourcelbs, 15, lNumberFormat)
                    Next
                    lReport.Append(.ToString)

                End With

            Case "TotalP"
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Landuse" & vbTab & "Area (ac)" & vbTab & _
                                    "Rate (lbs/ac)" & vbTab & "Total Load (lbs)")
                lReport3.AppendLine("Reach" & vbTab & "Source" & vbTab & "Load")
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 12
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Atmospheric Deposition on Water"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Gain(+)/Loss(-)"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Additional Sources"

                    For Each lID As HspfOperation In lRchresOperations
                        Dim lPointlbs As Double = 0.0
                        .CurrentRecord += 1
                        Dim CVOP As Double = ConversionFactorfromOxygen(aUci, aBalanceType, lID)

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
                                        lPointlbs += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation

                                    End If
                                Next
                            End If
                        Next
                        Dim lNonpointlbs As Double



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
                        Dim lAdditionalSourcelbs As Double = 0
                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, CVOP, lPointlbs, lTotalAtmDep, lDepScour, lTotalInflow, lUpstreamIn)
                            lReport2.Append(.Item1)
                            lNonpointlbs = .Item2
                        End With
                        lAdditionalSourcelbs = lTotalInflow - lNonpointlbs - lUpstreamIn - lTotalAtmDep - lPointlbs

                        'If lAdditionalSourcelbs < 0.0003 * lNonpointlbs Then
                        '    lAdditionalSourcelbs = 0.0
                        'End If
                        Dim lCumulativePointNonpoint As Double = lNonpointlbs + lAdditionalSourcelbs
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
                        lField += 1 : .Value(lField) = DoubleToString(lTotalAtmDep, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lDepScour, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lCumulativePointNonpoint, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, , lNumberFormat, , , 6)
                        lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lAdditionalSourcelbs, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)

                End With

        End Select

        For Each Key As String In pTotals.Keys
            Dim lSpacePlace As Integer = Key.IndexOf(" ")
            lReport3.AppendLine(Key.Substring(0, lSpacePlace) & vbTab & Key.Substring(lSpacePlace + 1) & vbTab & FormatNumber(pTotals.ItemByKey(Key), 2, TriState.True, TriState.False, TriState.False))
        Next
        pTotals.Clear()
        Return New Tuple(Of atcReport.IReport, atcReport.IReport, atcReport.IReport)(lReport, lReport2, lReport3)

    End Function

    Private Function ValueForReach(ByVal aReach As HspfOperation, _
                                   ByVal aReachData As atcTimeseriesGroup) As Double
        Dim lOutFlow As Double
        For Each aTimeseries As atcTimeseries In aReachData.FindData("Location", "R:" & aReach.Id)
            lOutFlow += aTimeseries.Attributes.GetValue("SumAnnual")
        Next

        Return lOutFlow
    End Function

    Private Sub felu(ByVal aUCI As HspfUci, _
                        ByVal aReach As HspfOperation, _
                        ByVal aBalanceType As String, _
                        ByVal aVolName As String, _
                        ByVal aNonpointData As atcTimeseriesGroup, _
                        ByVal aConversionFactor As Double, _
                        ByRef aLoadingByLanduse As String, _
                        ByRef aReachTotal As Double, _
                        ByVal aReporting As Boolean, _
                        ByRef aContribPercent As atcCollection)
        Dim lTotalIndex As Integer = 0
        Dim lTotal As Double = 0

        For Each lConnection As HspfConnection In aReach.Sources
            If lConnection.Source.VolName = aVolName Then

                Dim lConnectionArea As Double = lConnection.MFact
                Dim lMassLinkID As Integer = lConnection.MassLink
                Dim lTestLocation As String = lConnection.Source.VolName.Substring(0, 1) & ":" & lConnection.Source.VolId

                Dim lConstituentTotal As Double = 0
                lTotal = 0
                For Each lTs As atcTimeseries In aNonpointData.FindData("Location", lTestLocation)

                    Dim lMassLinkFactor As Double = 0.0
                    If lTs.Attributes.GetValue("SumAnnual") > 0 Then
                        lMassLinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lTs.Attributes.GetValue("Constituent"), aBalanceType, _
                                                                       aConversionFactor, aMultipleIndex:=0)
                    End If
                    lConstituentTotal = lTs.Attributes.GetValue("SumAnnual") * lMassLinkFactor * lConnectionArea
                    'lConstituentTotal = lConstituentRate * lConnectionArea

                    lTotal += lConnectionArea * lTs.Attributes.GetValue("SumAnnual") * lMassLinkFactor
                Next


                If aReporting Then

                    Dim lPercentContrib As Double = lTotal * 100 / aReachTotal
                    aContribPercent.Increment("Reach" & aReach.Id & " " & lTestLocation.Substring(0, 2) & aReach.Uci.OpnBlks(aVolName).OperFromID(lTestLocation.Substring(2)).Description, lPercentContrib)
                    aLoadingByLanduse &= aReach.Caption.ToString.Substring(10) & vbTab _
                        & lTestLocation & vbTab _
                        & aReach.Uci.OpnBlks(aVolName).OperFromID(lTestLocation.Substring(2)).Description & vbTab _
                        & lConnectionArea & vbTab _
                        & DoubleToString(lTotal / lConnectionArea, 15, "#,##0.###") & vbTab & _
                                            DoubleToString(lTotal, 15, "#,##0.###") & vbTab & vbCrLf
                Else

                    pTotals.Increment("Reach" & aReach.Id & " " & lTestLocation.Substring(0, 2) & aReach.Uci.OpnBlks(aVolName).OperFromID(lTestLocation.Substring(2)).Description, lTotal)
                    For Each lTributary As HspfConnection In aReach.Sources
                        If lTributary.Source.VolName = "RCHRES" Then
                            Dim lTributaryID As String = lTributary.Source.VolId
                            Dim UpstreamLoadByCategory As Double = pTotals.ItemByKey("Reach" & lTributaryID & " " & lTestLocation.Substring(0, 2) & aReach.Uci.OpnBlks(aVolName).OperFromID(lTestLocation.Substring(2)).Description)
                            pTotals.Increment("Reach" & aReach.Id & " " & lTestLocation.Substring(0, 2) & aReach.Uci.OpnBlks(aVolName).OperFromID(lTestLocation.Substring(2)).Description, UpstreamLoadByCategory)

                        End If
                    Next
                    aReachTotal += lTotal
                End If
            End If
        Next
    End Sub

    Private Function ConstituentLoadingByLanduse(ByVal aUCI As HspfUci, _
                                                 ByVal aReach As HspfOperation, _
                                    ByVal aBalanceType As String, _
                                    ByVal aNonpointData As atcTimeseriesGroup, _
                                   ByVal aConversionFactor As Double, _
                                   ByVal lPoint As Double, _
                                   ByVal lAtmDep As Double, _
                                   ByVal GainLoss As Double, _
                                   ByVal lTotalInflow As Double, _
                                   ByVal lUpstreamIn As Double) As Tuple(Of String, Double)
        Dim LoadingByLanduse As String = ""
        Dim lReachTotal As Double = 0.0

        Dim lContribPercent As New atcCollection

        felu(aUCI, aReach, aBalanceType, "PERLND", aNonpointData, aConversionFactor, LoadingByLanduse, lReachTotal, False, lContribPercent)
        felu(aUCI, aReach, aBalanceType, "IMPLND", aNonpointData, aConversionFactor, LoadingByLanduse, lReachTotal, False, lContribPercent)

        Dim lAdditionalSource As Double = lTotalInflow - lReachTotal - lUpstreamIn - lAtmDep - lPoint
        Dim lOverAllTotal As Double = lReachTotal + lAtmDep + lPoint + lAdditionalSource
        pTotals.Add("Reach" & aReach.Id & " PointSources", lPoint)
        pTotals.Add("Reach" & aReach.Id & " DirectAtmosphericDeposition", lAtmDep)
        pTotals.Add("Reach" & aReach.Id & " AdditionalSources", lAdditionalSource)
        If GainLoss < 0 Then
            pTotals.Add("Reach" & aReach.Id & " Loss", GainLoss)
            pTotals.Add("Reach" & aReach.Id & " Gain", 0.0)
        Else
            pTotals.Add("Reach" & aReach.Id & " Loss", 0.0)
            pTotals.Add("Reach" & aReach.Id & " Gain", GainLoss)
        End If


        'lReachTotal +=

        felu(aUCI, aReach, aBalanceType, "PERLND", aNonpointData, aConversionFactor, LoadingByLanduse, lOverAllTotal, True, lContribPercent)
        felu(aUCI, aReach, aBalanceType, "IMPLND", aNonpointData, aConversionFactor, LoadingByLanduse, lOverAllTotal, True, lContribPercent)

        lContribPercent.Add("Reach" & aReach.Id & " PointSources", lPoint * 100 / lOverAllTotal)
        lContribPercent.Add("Reach" & aReach.Id & " DirectAtmosphericDeposition", lAtmDep * 100 / lOverAllTotal)
        lContribPercent.Add("Reach" & aReach.Id & " AdditionalSources", lAdditionalSource * 100 / lOverAllTotal)


        For Each lTributary As HspfConnection In aReach.Sources
            If lTributary.Source.VolName = "RCHRES" Then
                Dim lTributaryId As String = lTributary.Source.VolId
                Dim lUpStreamLoadbyCategory As Double = pTotals.ItemByKey("Reach" & lTributaryId & " PointSources")
                pTotals.Increment("Reach" & aReach.Id & " PointSources", lUpStreamLoadbyCategory)
                lUpStreamLoadbyCategory = pTotals.ItemByKey("Reach" & lTributaryId & " DirectAtmosphericDeposition")
                pTotals.Increment("Reach" & aReach.Id & " DirectAtmosphericDeposition", lUpStreamLoadbyCategory)
                lUpStreamLoadbyCategory = pTotals.ItemByKey("Reach" & lTributaryId & " AdditionalSources")
                pTotals.Increment("Reach" & aReach.Id & " AdditionalSources", lUpStreamLoadbyCategory)
                lUpStreamLoadbyCategory = pTotals.ItemByKey("Reach" & lTributaryId & " Gain")
                pTotals.Increment("Reach" & aReach.Id & " Gain", lUpStreamLoadbyCategory)
                lUpStreamLoadbyCategory = pTotals.ItemByKey("Reach" & lTributaryId & " Loss")
                pTotals.Increment("Reach" & aReach.Id & " Loss", lUpStreamLoadbyCategory)

            End If
        Next

        If GainLoss < 0 Then
            For Each lKey As String In lContribPercent.Keys

                For Each lTotalsKey As String In pTotals.Keys
                    Dim ReachIDLength As Integer = aReach.Id.ToString.Length
                    If lTotalsKey.Contains(aReach.Id) And lTotalsKey.Contains(SafeSubstring(lKey, 5 + ReachIDLength)) Then

                        pTotals.ItemByKey(lTotalsKey) = pTotals.ItemByKey(lTotalsKey) + (GainLoss * lContribPercent.ItemByKey(lKey) / 100)

                    End If
                Next
            Next

        End If




        Return New Tuple(Of String, Double)(LoadingByLanduse, lReachTotal)
    End Function

End Module
