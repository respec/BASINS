Imports atcUtility
Imports atcData
Imports atcUCI
Imports MapWinUtility

Public Module ConstituentBudget
    Private pRunningTotals As New atcCollection
    Private pOutputTotals As New atcCollection
    Private pPERLND As New atcCollection
    Private pIMPLND As New atcCollection
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
        Dim lReport4 As New atcReport.ReportText

        For Each lSource As HspfOperation In aUci.OpnSeqBlock.Opns
            If lSource.Name = "PERLND" AndAlso Not pPERLND.Keys.Contains(lSource.Description) Then
                pPERLND.Add(lSource.Description)

            ElseIf lSource.Name = "IMPLND" AndAlso Not pIMPLND.Keys.Contains(lSource.Description) Then
                pIMPLND.Add(lSource.Description)
            End If

        Next

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
                lReport3.AppendLine("Budget report not yet defined for balance type '" & aBalanceType & "'")
                lReport4.AppendLine("Budget report not yet defined for balance type '" & aBalanceType & "'")
                Return New Tuple(Of atcReport.IReport, atcReport.IReport, atcReport.IReport)(lReport, lReport2, lReport3)

        End Select

        Dim lUpstreamInflows As New atcCollection
        Dim lCumulativePointNonpointColl As New atcCollection


        lReport.AppendLine(aScenario & " " & "Average Annual " & aBalanceType & " Budget by Reach " & lUnits & ".")
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunPeriod)


        lReport2.AppendLine(aScenario & " " & "Average Annual Nonpoint Loading of " & aBalanceType & " by Each Land Use to Each Reach " & lUnits & ".")
        lReport2.AppendLine("   Run Made " & aRunMade)
        lReport2.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport2.AppendLine("   " & aUci.GlobalBlock.RunPeriod)


        lReport3.AppendLine(aScenario & " " & " Average Annual Load Allocation of " & aBalanceType & " to Each Source by Reach " & lUnits & ".")
        lReport3.AppendLine("The Losses at Each Reach have been applied to the source, and the Gains are accumulated.")
        lReport3.AppendLine("   Run Made " & aRunMade)
        lReport3.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport3.AppendLine("   " & aUci.GlobalBlock.RunPeriod)

        lReport4.AppendLine("")
        lReport4.AppendLine(aScenario & " " & " Percent of loadings of " & aBalanceType & " from each individual source to the Reaches (%).")

        Dim YearsOfSimulation As Double = YearCount(aUci.GlobalBlock.SDateJ, aUci.GlobalBlock.EdateJ)
        Dim lOutputTable As New atcTableDelimited

        Select Case aBalanceType
            Case "Water"
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & vbTab & _
                                    "Rate (ft/ac)" & vbTab & "Total Load (ac-ft)")
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 10
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
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Diversion"
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

                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData)
                        Dim lEvapLoss As Double = ValueForReach(lID, lEvapLossData)
                        Dim lAdditionalSource As Double = 0.0
                        lAdditionalSource = lTotalInflow - lNonpointVol - lUpstreamIn - lPointVol - lInflowFromPrecip + lEvapLoss
                        Dim lCumulativePointNonpoint As Double = lNonpointVol + lPointVol + lAdditionalSource
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
                        Dim Diversion As Double = 0.0
                        If lID.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                            lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                        Else
                            Dim lExitNumber As Integer = 0
                            FindDownStreamExitNumber(aUci, lID, lExitNumber)
                            If lExitNumber = 0 Then
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            Else
                                Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lID.Id).FindData("Constituent", "OVOL-" & lExitNumber)(0)
                                Diversion = lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual")
                                lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            End If

                        End If
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
                        lField += 1 : .Value(lField) = DoubleToString(Diversion, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lAdditionalSource, , lNumberFormat)

                    Next
                    lReport.Append(.ToString)
                End With

            Case "Sediment"
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & _
                                    vbTab & "Rate (tons/ac)" & vbTab & "Total Load (tons)")
                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 12
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
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Diversion"
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

                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                        Dim lDepScour As Double = ValueForReach(lID, lDepScourData) 'TotalForReach(lID, lAreas, lDepScourData)
                        Dim lNonpointTons As Double
                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, 0.0, lPointTons, 0.0, lDepScour, lTotalInflow, lUpstreamIn)
                            lReport2.Append(.Item1)
                            lNonpointTons = .Item2

                        End With
                        Dim lAdditionalSourceTons As Double = lTotalInflow - lNonpointTons - lUpstreamIn - lPointTons
                        Dim lCumulativePointNonpoint As Double = lNonpointTons + lAdditionalSourceTons + lPointTons
                        If lCumulativePointNonpointColl.Keys.Contains(lID.Id) Then
                            lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lID.Id)
                        End If

                        Dim lReachTrappingEfficiency As Double
                        Try
                            lReachTrappingEfficiency = lDepScour / lTotalInflow
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try

                        Dim lDownstreamReachID As Integer = lID.DownOper("RCHRES")
                        Dim Diversion As Double = 0.0
                        If lID.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                            lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                        Else
                            Dim lExitNumber As Integer = 0
                            FindDownStreamExitNumber(aUci, lID, lExitNumber)
                            If lExitNumber = 0 Then
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            Else
                                Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lID.Id).FindData("Constituent", "OSED-TOT-EXIT" & lExitNumber)(0)
                                Diversion = lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual")
                                lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            End If

                        End If
                        Dim lCululativeTrappingEfficiency As Double = 0
                        Try
                            lCululativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try
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
                        lField += 1 : .Value(lField) = DoubleToString(Diversion, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lAdditionalSourceTons, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)
                End With

            Case "TotalN"
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & vbTab & _
                                    "Rate (lbs/ac)" & vbTab & "Total Load (lbs)")

                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 13
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
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Diversion"
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

                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData)
                        Dim lDepScour As Double = lOutflow - lTotalInflow

                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, CVON, lPointlbs, lTotalAtmDep, lDepScour, lTotalInflow, lUpstreamIn)
                            lReport2.Append(.Item1)
                            lNonpointlbs = .Item2
                        End With

                        lAdditionalSourcelbs = lTotalInflow - lNonpointlbs - lTotalAtmDep - lUpstreamIn - lPointlbs

                        Dim lCumulativePointNonpoint As Double = lNonpointlbs + lPointlbs + lAdditionalSourcelbs
                        If lCumulativePointNonpointColl.Keys.Contains(lID.Id) Then
                            lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lID.Id)
                        End If

                        Dim lReachTrappingEfficiency As Double
                        Try
                            lReachTrappingEfficiency = lDepScour / lTotalInflow
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try

                        Dim lDownstreamReachID As Integer = lID.DownOper("RCHRES")
                        Dim Diversion As Double = 0.0
                        If lID.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                            lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                        Else
                            Dim lExitNumber As Integer = 0
                            FindDownStreamExitNumber(aUci, lID, lExitNumber)
                            If lExitNumber = 0 Then
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            Else
                                Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lID.Id).FindData("Constituent", "N-TOT-OUT-EXIT" & lExitNumber)(0)
                                Diversion = lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual")
                                lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            End If

                        End If
                        Dim lCululativeTrappingEfficiency As Double = 0
                        Try
                            lCululativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try
                        pRunningTotals.Add("Reach" & lID.Id & " Diversion", Diversion)

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
                        lField += 1 : .Value(lField) = DoubleToString(Diversion, 15, lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lAdditionalSourcelbs, 15, lNumberFormat)
                    Next
                    lReport.Append(.ToString)

                End With

            Case "TotalP"
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & vbTab & _
                                    "Rate (lbs/ac)" & vbTab & "Total Load (lbs)")

                With lOutputTable
                    .Delimiter = vbTab
                    .NumFields = 13
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
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Diversion"
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

                        Dim Diversion As Double = 0.0
                        Dim lDownstreamReachID As Integer = lID.DownOper("RCHRES")
                        If lID.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                            lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                        Else
                            Dim lExitNumber As Integer = 0
                            FindDownStreamExitNumber(aUci, lID, lExitNumber)
                            If lExitNumber = 0 Then
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            Else
                                Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lID.Id).FindData("Constituent", "P-TOT-OUT-EXIT" & lExitNumber)(0)
                                Diversion = lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual")
                                lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            End If
                        End If

                        pRunningTotals.Add("Reach" & lID.Id & " Diversion", Diversion)
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
                        lField += 1 : .Value(lField) = DoubleToString(Diversion, , lNumberFormat)
                        lField += 1 : .Value(lField) = DoubleToString(lAdditionalSourcelbs, , lNumberFormat)
                    Next
                    lReport.Append(.ToString)
                End With
        End Select

        Dim lLandUses As New List(Of String)
        Dim lReaches As New List(Of String)
        Dim lLandusesHeader As String = ""
        For Each Key As String In pRunningTotals.Keys
            Dim lSpacePlace As Integer = Key.IndexOf(" ")
            Dim lLanduseDescription As String = Key.Substring(lSpacePlace + 1)
            If Not lLandUses.Contains(lLanduseDescription) Then
                lLandUses.Add(lLanduseDescription)
                lLandusesHeader &= vbTab & lLanduseDescription
            End If
            Dim lReach As String = Key.Substring(0, lSpacePlace)
            If Not lReaches.Contains(lReach) Then
                lReaches.Add(lReach)
            End If
        Next
        lLandusesHeader = lLandusesHeader.Replace("DirectAtmosphericDeposition", "Direct Atmospheric Deposition on the Reach")
        lLandusesHeader = lLandusesHeader.Replace("Loss", "Cumulative Instream Losses")
        lLandusesHeader = lLandusesHeader.Replace("Gain", "Cumulative Instream Gains")
        lLandusesHeader = lLandusesHeader.Replace("AdditionalSources", "Mass Balance Errors/Additional Sources*")


        lReport3.AppendLine("Reach" & lLandusesHeader & vbTab & "Total")
        lReport4.AppendLine("Reach" & lLandusesHeader)

        For Each lReach As String In lReaches
            lReport3.Append(aUci.OpnBlks("RCHRES").OperFromID(lReach.Substring(5)).Caption.Substring(12))
            lReport4.Append(aUci.OpnBlks("RCHRES").OperFromID(lReach.Substring(5)).Caption.Substring(12))

            Dim lTotal As Double = 0.0
            For Each lSourceDescription As String In lLandUses

                Dim Key As String = lReach & " " & lSourceDescription
                Dim lValue As Double = pRunningTotals.ItemByKey(Key)

                lReport3.Append(vbTab & FormatNumber(lValue, 2, TriState.True, TriState.False, TriState.False))
                lTotal += lValue

            Next lSourceDescription

            lReport3.AppendLine(vbTab & FormatNumber(lTotal, 2, TriState.True, TriState.False, TriState.False))

            For Each lSourceDescription As String In lLandUses

                Dim Key As String = lReach & " " & lSourceDescription
                Dim lValue As Double = pRunningTotals.ItemByKey(Key)

                lReport4.Append(vbTab & FormatNumber(lValue * 100 / lTotal, 2, TriState.True, TriState.False, TriState.False))

            Next lSourceDescription
            lReport4.AppendLine()

        Next
        lReport3.AppendLine("*The additional sources may include sources other than non-point sources, point sources, atmospheric deposition, and upstream contribution.")
        lReport4.AppendLine("*The additional sources may include sources other than non-point sources, point sources, atmospheric deposition, and upstream contribution.")
        pRunningTotals.Clear()

        lReport3.Append(lReport4.ToString)

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
    'FELU stands for For Each Land Use
    Private Sub felu(ByVal aUCI As HspfUci, _
                        ByVal aReach As HspfOperation, _
                        ByVal aBalanceType As String, _
                        ByVal aVolName As String, _
                        ByVal aLandUses As atcCollection, _
                        ByVal aNonpointData As atcTimeseriesGroup, _
                        ByVal aConversionFactor As Double, _
                        ByRef aLoadingByLanduse As String, _
                        ByRef aReachTotal As Double, _
                        ByVal aReporting As Boolean, _
                        ByRef aContribPercent As atcCollection)
        Dim lTotalIndex As Integer = 0
        Dim lTotal As Double = 0

        For Each landUse As String In aLandUses
            Dim lFound As Boolean = False
            For Each lConnection As HspfConnection In aReach.Sources
                If lConnection.Source.VolName = aVolName AndAlso lConnection.Source.Opn.Description = landUse Then
                    lFound = True
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
                    felu2(aUCI, aReach, aBalanceType, aVolName, aLandUses, aLoadingByLanduse, aReachTotal, aReporting, aContribPercent, lTotal, lConnectionArea, landUse)
                    Exit For
                End If

            Next

            If Not lFound Then
                felu2(aUCI, aReach, aBalanceType, aVolName, aLandUses, aLoadingByLanduse, aReachTotal, aReporting, aContribPercent, 0, 0, landUse)
            End If

        Next

    End Sub
    Private Sub felu2(ByVal aUCI As HspfUci, _
                        ByVal aReach As HspfOperation, _
                        ByVal aBalanceType As String, _
                        ByVal aVolName As String, _
                        ByVal aLandUses As atcCollection, _
                        ByRef aLoadingByLanduse As String, _
                        ByRef aReachTotal As Double, _
                        ByVal aReporting As Boolean, _
                        ByRef aContribPercent As atcCollection, _
                        ByRef aTotal As Double, _
                        ByVal aConnectionArea As Double, _
                        ByVal aLandUse As String)
        Dim lVolPrefix As String = aVolName.Substring(0, 1) & ":"
        If aReporting Then

            Dim lPercentContrib As Double = 0.0
            Dim lNumberofTributaries As Integer = 0
            For Each lTributary As HspfConnection In aReach.Sources

                If lTributary.Source.VolName = "RCHRES" Then
                    Dim lTributaryID As Integer = lTributary.Source.VolId
                    lNumberofTributaries += 1
                    Dim UpstreamLoadByCategory As Double = pRunningTotals.ItemByKey("Reach" & lTributaryID & " " & lVolPrefix & aLandUse)
                    aTotal += UpstreamLoadByCategory
                    lPercentContrib = aTotal * 100 / aReachTotal

                End If
            Next

            If lNumberofTributaries = 0 Then
                lPercentContrib = aTotal * 100 / aReachTotal
            End If

            aContribPercent.Increment("Reach" & aReach.Id & " " & lVolPrefix & aLandUse, lPercentContrib)

            If aConnectionArea > 0 Then
                aLoadingByLanduse &= aReach.Caption.ToString.Substring(10) & vbTab _
                & lVolPrefix & _
                & aLandUse & vbTab _
                & aConnectionArea & vbTab _
                & DoubleToString(aTotal / aConnectionArea, 15, "#,##0.###") & vbTab & _
                                    DoubleToString(aTotal, 15, "#,##0.###") & vbTab & vbCrLf
            Else
                aLoadingByLanduse &= aReach.Caption.ToString.Substring(10) & vbTab _
                & lVolPrefix & _
                & aLandUse & vbTab _
                & aConnectionArea & vbTab _
                & DoubleToString(0.0, 15, "#,##0.###") & vbTab & _
                                    DoubleToString(0.0, 15, "#,##0.###") & vbTab & vbCrLf
            End If
            
        Else
            pRunningTotals.Add("Reach" & aReach.Id & " " & lVolPrefix & aLandUse, aTotal)
            aReachTotal += aTotal
            For Each lTributary As HspfConnection In aReach.Sources

                If lTributary.Source.VolName = "RCHRES" Then
                    Dim lTributaryID As String = lTributary.Source.VolId
                    Dim UpstreamLoadByCategory As Double = pRunningTotals.ItemByKey("Reach" & lTributaryID & " " & lVolPrefix & aLandUse)
                    pRunningTotals.Increment("Reach" & aReach.Id & " " & lVolPrefix & aLandUse, UpstreamLoadByCategory)
                End If

            Next

        End If

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
        felu(aUCI, aReach, aBalanceType, "PERLND", pPERLND, aNonpointData, aConversionFactor, LoadingByLanduse, lReachTotal, False, lContribPercent)
        felu(aUCI, aReach, aBalanceType, "IMPLND", pIMPLND, aNonpointData, aConversionFactor, LoadingByLanduse, lReachTotal, False, lContribPercent)

        Dim lAdditionalSource As Double = lTotalInflow - lReachTotal - lAtmDep - lPoint - lUpstreamIn


        felu(aUCI, aReach, aBalanceType, "PERLND", pPERLND, aNonpointData, aConversionFactor, LoadingByLanduse, lTotalInflow, True, lContribPercent)
        felu(aUCI, aReach, aBalanceType, "IMPLND", pIMPLND, aNonpointData, aConversionFactor, LoadingByLanduse, lTotalInflow, True, lContribPercent)

        For Each lTributary As HspfConnection In aReach.Sources
            If lTributary.Source.VolName = "RCHRES" Then
                Dim lTributaryId As String = lTributary.Source.VolId
                lPoint += pRunningTotals.ItemByKey("Reach" & lTributaryId & " PointSources")
                lAdditionalSource += pRunningTotals.ItemByKey("Reach" & lTributaryId & " AdditionalSources")
                lAtmDep += pRunningTotals.ItemByKey("Reach" & lTributaryId & " DirectAtmosphericDeposition")
                pRunningTotals.Increment("Reach" & aReach.Id & " Gain", pRunningTotals.ItemByKey("Reach" & lTributaryId & " Gain"))
                pRunningTotals.Increment("Reach" & aReach.Id & " Loss", pRunningTotals.ItemByKey("Reach" & lTributaryId & " Loss"))
            End If
        Next

        pRunningTotals.Add("Reach" & aReach.Id & " PointSources", lPoint)
        pRunningTotals.Add("Reach" & aReach.Id & " DirectAtmosphericDeposition", lAtmDep)
        pRunningTotals.Add("Reach" & aReach.Id & " AdditionalSources", lAdditionalSource)

        If GainLoss < 0 Then
            pRunningTotals.Increment("Reach" & aReach.Id & " Loss", GainLoss)
            pRunningTotals.Increment("Reach" & aReach.Id & " Gain", 0.0)
        Else
            pRunningTotals.Increment("Reach" & aReach.Id & " Loss", 0.0)
            pRunningTotals.Increment("Reach" & aReach.Id & " Gain", GainLoss)
        End If

        lContribPercent.Add("Reach" & aReach.Id & " PointSources", lPoint * 100 / lTotalInflow)
        lContribPercent.Add("Reach" & aReach.Id & " DirectAtmosphericDeposition", lAtmDep * 100 / lTotalInflow)
        lContribPercent.Add("Reach" & aReach.Id & " AdditionalSources", lAdditionalSource * 100 / lTotalInflow)

        If GainLoss < 0 Then
            For Each lKey As String In lContribPercent.Keys
                'If aReach.Id = "110" Then Stop
                For Each lTotalsKey As String In pRunningTotals.Keys
                    Dim ReachIDLength As Integer = aReach.Id.ToString.Length
                    If lTotalsKey.Contains(aReach.Id) AndAlso lTotalsKey.Contains(SafeSubstring(lKey, 6 + ReachIDLength)) Then

                        pRunningTotals.ItemByKey(lTotalsKey) = pRunningTotals.ItemByKey(lTotalsKey) + (GainLoss * lContribPercent.ItemByKey(lKey) / 100)
                        Exit For
                    End If

                Next lTotalsKey
            Next lKey

        End If

        Return New Tuple(Of String, Double)(LoadingByLanduse, lReachTotal)
    End Function

    Private Function FindDownStreamExitNumber(ByVal aUCI As HspfUci, _
                                              ByVal aReachID As HspfOperation, _
                                              ByRef aExitNumber As Integer) As Integer
        'Function to find the EXIT number through which the flow is sent to the downstream waterbody.
        Dim lDownstreamReachID As Integer = aReachID.DownOper("RCHRES")
        For Each lReachConnection As HspfConnection In aReachID.Targets
            If lReachConnection.Target.VolId = lDownstreamReachID Then
                Dim lMasslinkID As Integer = lReachConnection.MassLink
                For Each lMasslink As HspfMassLink In aUCI.MassLinks
                    If lMasslink.MassLinkId = lMasslinkID Then
                        If lMasslink.Source.Member.ToString = "ROFLOW" Then
                            aExitNumber = 0
                        Else
                            aExitNumber = lMasslink.Source.MemSub1
                            Exit For
                        End If
                    End If
                Next
            End If
        Next
        Return aExitNumber
    End Function
End Module
