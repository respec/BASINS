Imports atcUtility
Imports atcData
Imports atcUCI
Imports MapWinUtility

Public Module ConstituentBudget
    Private pRunningTotals As atcCollection
    Private pOutputConnectionArea As atcCollection
    Private pOutputLoad As atcCollection
    Private pPERLND As atcCollection
    Private pIMPLND As atcCollection
    Private pGENERLoadingExists As Boolean = False

    Public Sub Report(ByVal aUci As atcUCI.HspfUci,
                      ByVal aBalanceType As String,
                      ByVal aOperationTypes As atcCollection,
                      ByVal aScenario As String,
                      ByVal aScenarioResults As atcDataSource,
                      ByVal aOutputLocations As atcCollection,
                      ByVal aRunMade As String,
                      ByVal aSDateJ As Double,
                      ByVal aEDateJ As Double,
                      ByVal aConstProperties As List(Of ConstituentProperties),
                      ByVal aSeasonsLabel As String,
                      ByRef aReport1ReachBudget As atcReport.ReportText,
                      ByRef aReport2NPSLoads As atcReport.ReportText,
                      ByRef aReport3LoadAllocationAll As atcReport.ReportText,
                      ByRef aReport5LoadAllocationLocations As atcReport.ReportText,
                      ByRef aReport7LoadingRate As atcReport.ReportText,
                      ByRef aDataForAllBarGraphs As atcCollection)

        'Implemented for Water, Sediment, TN and TP
        '  (Water does only report 1 and 2)

        'In HSPEXP+ these reports are named as follows:
        '    aReport1ReachBudget => "_Per_RCH_Ann_Avg_Budget.txt"
        '    aReport2NPSLoads => "_Per_RCH_Per_LU_Ann_Avg_NPS_Lds.txt"
        '    aReport3LoadAllocationAll => "_LoadAllocation.txt"
        '    aReport5LoadAllocationLocations => "_LoadAllocation_Locations.txt"
        '    aReport7LoadingRate => "_LoadingRates.txt"

        Dim lReport4 As New atcReport.ReportText   'used to be separate, now part of Report3
        Dim lReport6 As New atcReport.ReportText   'used to be separate, now part of Report5

        Dim lNumberFormat As String = "#,##0.###"
        Dim lNumberOfSignificantDigits As Integer = 11
        Dim lUnits As String = ""
        Dim lNonpointData As New atcTimeseriesGroup
        Dim lAtmDepData As New atcTimeseriesGroup
        Dim lTotalInflowData As New atcTimeseriesGroup
        Dim lTotalPrecipData As New atcTimeseriesGroup
        Dim lOutflowData As New atcTimeseriesGroup
        Dim lDepScourData As New atcTimeseriesGroup
        Dim lEvapLossData As New atcTimeseriesGroup
        Dim lRchresOperations As HspfOperations = aUci.OpnBlks("RCHRES").Ids

        pRunningTotals = New atcCollection
        pOutputConnectionArea = New atcCollection
        pOutputLoad = New atcCollection
        pPERLND = New atcCollection
        pIMPLND = New atcCollection
        pGENERLoadingExists = False

        For Each lSource As HspfOperation In aUci.OpnSeqBlock.Opns
            'Collecting the names of the land uses
            If lSource.Name = "PERLND" AndAlso Not pPERLND.Keys.Contains(lSource.Description) Then
                pPERLND.Add(lSource.Description)
            ElseIf lSource.Name = "IMPLND" AndAlso Not pIMPLND.Keys.Contains(lSource.Description) Then
                pIMPLND.Add(lSource.Description)
            ElseIf lSource.Name = "GENER" Then
                pGENERLoadingExists = True
            End If
        Next

        Select Case aBalanceType
            Case "Water"
                lUnits = "ac-ft"

                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SURO"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "IFWO"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "AGWO"))

                lTotalPrecipData.Add(aScenarioResults.DataSets.FindData("Constituent", "PRSUPY"))
                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "IVOL"))

                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ROVOL"))
                lEvapLossData.Add(aScenarioResults.DataSets.FindData("Constituent", "VOLEV"))

            Case "Sediment"
                lUnits = "tons"
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "WSSD"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SCRSD"))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "SOSLD")))

                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ISED-TOT"))
                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "ROSED-TOT"))
                lDepScourData.Add(aScenarioResults.DataSets.FindData("Constituent", "DEPSCOUR-TOT"))
            Case "TN", "TAM", "TKN", "NO2NO3"
                lUnits = "lbs"

                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "NITROGEN - TOTAL OUTFLOW"))
                If aConstProperties IsNot Nothing Then
                    Dim lConstituentList As New List(Of String)
                    For Each lConsituent As ConstituentProperties In aConstProperties
                        Dim lConstituentName As String = lConsituent.ConstituentNameInUCI
                        If Not lConstituentList.Contains(lConstituentName) Then
                            lConstituentList.Add(lConstituentName)
                            lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "WASHQS-" & lConstituentName))
                            lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SCRQS-" & lConstituentName))
                            lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOQO-" & lConstituentName))
                            lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "AOQUAL-" & lConstituentName))
                            lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "IOQUAL-" & lConstituentName))
                        End If
                    Next
                End If

                If aBalanceType = "TN" Then
                    lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "N-TOT-IN"))
                    lAtmDepData.Add(aScenarioResults.DataSets.FindData("Constituent", "NO3-ATMDEPTOT"))
                    lAtmDepData.Add(aScenarioResults.DataSets.FindData("Constituent", "TAM-ATMDEPTOT"))
                    lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "N-TOT-OUT"))
                ElseIf aBalanceType = "TAM" Then
                    lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "TAM-INTOT"))
                    lAtmDepData.Add(aScenarioResults.DataSets.FindData("Constituent", "TAM-ATMDEPTOT"))
                    lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "TAM-OUTTOT"))
                ElseIf aBalanceType = "TKN" Then
                    lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "TAM-INTOT"))
                    lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "N-TOTORG-IN"))
                    lAtmDepData.Add(aScenarioResults.DataSets.FindData("Constituent", "TAM-ATMDEPTOT"))
                    lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "TAM-OUTTOT"))
                    lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "N-TOTORG-OUT"))
                ElseIf aBalanceType = "NO2NO3" Then
                    lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "NO2-INTOT"))
                    lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "NO3-INTOT"))
                    lAtmDepData.Add(aScenarioResults.DataSets.FindData("Constituent", "NO3-ATMDEPTOT"))
                    lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "NO2-OUTTOT"))
                    lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "NO3-OUTTOT"))
                End If

            Case "TP", "ORTHO P"
                lUnits = "lbs"

                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-P IN SOLUTION - INTERFLOW - OUTFLOW"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-P IN SOLUTION - GROUNDWATER - OUTFLOW"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SDP4A")) ' Outflow of sediment-associated PO4
                If aBalanceType = "TP" Then
                    lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SDORP")) ' Organic P Outflow (will be multiplied by 0.6)
                    lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "ORGN - TOTAL OUTFLOW")) ' Organic N Outflow (will be multiplied by 0.05534793)
                End If
                'lNonpointData.AddRange((aScenarioResults.DataSets.FindData("Constituent", "POPHOS")))
                If aConstProperties IsNot Nothing Then
                    Dim ConstituentList As New List(Of String)
                    For Each consituent As ConstituentProperties In aConstProperties
                        Dim ConstituentName As String = consituent.ConstituentNameInUCI
                        If Not ConstituentList.Contains(ConstituentName) Then
                            ConstituentList.Add(ConstituentName)
                            lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "WASHQS-" & ConstituentName))
                            lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SCRQS-" & ConstituentName))
                            lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SOQO-" & ConstituentName))
                            lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "AOQUAL-" & ConstituentName))
                            lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "IOQUAL-" & ConstituentName))
                        End If
                    Next
                End If
                lAtmDepData.Add((aScenarioResults.DataSets.FindData("Constituent", "PO4-ATMDEPTOT")))

                If aBalanceType = "TP" Then
                    lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "P-TOT-IN"))
                    lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "P-TOT-OUT"))
                Else
                    lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-INTOT"))
                    lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-OUTTOT"))
                End If
        End Select

        Dim lUpstreamInflows As New atcCollection
        Dim lCumulativePointNonpointColl As New atcCollection
        Dim lTimeSpan As String = TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: ")
        If Len(aSeasonsLabel) > 0 Then
            lTimeSpan = lTimeSpan.Substring(0, lTimeSpan.Length - 2)   'remove crlf if adding seasons
        End If

        aReport1ReachBudget.AppendLine(aScenario & " " & "Average Annual " & aBalanceType & " Budget by Reach " & lUnits & ".")
        aReport1ReachBudget.AppendLine("   Run Made " & aRunMade)
        aReport1ReachBudget.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        aReport1ReachBudget.AppendLine("   " & lTimeSpan)
        If Len(aSeasonsLabel) > 0 Then
            aReport1ReachBudget.AppendLine("                    " & aSeasonsLabel & vbCrLf)
        End If

        aReport2NPSLoads.AppendLine(aScenario & " " & "Average Annual Nonpoint Loading of " & aBalanceType & " by Each Land Use to Each Reach " & lUnits & ".")
        aReport2NPSLoads.AppendLine("   Run Made " & aRunMade)
        aReport2NPSLoads.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        aReport2NPSLoads.AppendLine("   " & lTimeSpan)
        If Len(aSeasonsLabel) > 0 Then
            aReport2NPSLoads.AppendLine("                    " & aSeasonsLabel & vbCrLf)
        End If

        aReport3LoadAllocationAll.AppendLine(aScenario & " " & " Average Annual Load Allocation of " & aBalanceType & " to Each Source by Reach " & lUnits & ".")
        aReport3LoadAllocationAll.AppendLine("The Losses at Each Reach have been applied to the source, and the Gains are accumulated.")
        aReport3LoadAllocationAll.AppendLine("   Run Made " & aRunMade)
        aReport3LoadAllocationAll.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        aReport3LoadAllocationAll.AppendLine("   " & lTimeSpan)
        If Len(aSeasonsLabel) > 0 Then
            aReport3LoadAllocationAll.AppendLine("                    " & aSeasonsLabel & vbCrLf)
        End If

        lReport4.AppendLine("")
        lReport4.AppendLine("Percent of loadings of " & aBalanceType & " from each individual source to the Reaches (%).")

        aReport5LoadAllocationLocations.AppendLine(aScenario & " " & " Average Annual Load Allocation of " & aBalanceType & " to Each Source by Reaches of Interest " & lUnits & ".")
        aReport5LoadAllocationLocations.AppendLine("The Losses at Each Reach have been applied to the source, and the Gains are accumulated.")
        aReport5LoadAllocationLocations.AppendLine("   Run Made " & aRunMade)
        aReport5LoadAllocationLocations.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        aReport5LoadAllocationLocations.AppendLine("   " & lTimeSpan)
        If Len(aSeasonsLabel) > 0 Then
            aReport5LoadAllocationLocations.AppendLine("                    " & aSeasonsLabel & vbCrLf)
        End If

        lReport6.AppendLine("")
        lReport6.AppendLine("Percent of loadings of " & aBalanceType & " from each individual source to the Reaches of Interest(%).")

        aReport7LoadingRate.AppendLine(aScenario & " " & " Average Annual " & aBalanceType & " Loading rates " & lUnits & "/ac/yr.")
        aReport7LoadingRate.AppendLine("   Run Made " & aRunMade)
        aReport7LoadingRate.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        aReport7LoadingRate.AppendLine("   " & lTimeSpan)
        If Len(aSeasonsLabel) > 0 Then
            aReport7LoadingRate.AppendLine("                    " & aSeasonsLabel & vbCrLf)
        End If
        aReport7LoadingRate.AppendLine("")

        Dim YearsOfSimulation As Double = YearCount(aSDateJ, aEDateJ)
        Dim lOutputTable As New atcTableDelimited

        Select Case aBalanceType
            Case "Water", "WAT"
                aReport2NPSLoads.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & vbTab &
                                    "Rate (ft/ac)" & vbTab & "Total Load (ac-ft)")
                With lOutputTable
                    .Delimiter = vbTab
                    If pGENERLoadingExists Then
                        .NumFields = 11
                    Else
                        .NumFields = 10
                    End If

                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Local Drainage"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Sources"
                    If pGENERLoadingExists Then
                        lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "GENER Sources"
                    End If
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Diversion"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Mass Balance Differences/Additional Sources"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Inflow from Precipitation"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Loss from Evaporation"

                    For Each lROper As HspfOperation In lRchresOperations

                        .CurrentRecord += 1
                        Dim lLocationName As String = lROper.Name.Substring(0, 1) & ":" & lROper.Id
                        Logger.Status("Generating Constituent Budget Report for " & aBalanceType & " from " & lLocationName)

                        'find point sources
                        Dim lTotalPointVol As Double = 0.0
                        For Each lSource As HspfPointSource In lROper.PointSources
                            If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IVOL" Then
                                Dim lPointVol As Double = 0.0
                                Dim lTimeSeriesTransformaton As atcTran = atcTran.TranAverSame
                                If lSource.Tran.ToString.Trim = "DIV" Then
                                    lTimeSeriesTransformaton = atcTran.TranSumDiv
                                End If
                                Dim lVolName As String = lSource.Source.VolName
                                Dim lDSN As Integer = lSource.Source.VolId
                                Dim lMfact As Double = lSource.MFact
                                For i As Integer = 0 To aUci.FilesBlock.Count
                                    If aUci.FilesBlock.Value(i).Typ = lVolName Then
                                        Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
                                        Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                        If lDataSource Is Nothing Then
                                            If atcDataManager.OpenDataSource(lFileName) Then
                                                lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                            End If
                                        End If
                                        Dim lTimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                        lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                                        lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUHour, 1, lTimeSeriesTransformaton) 'assumes run is at 1 hour timestep
                                        lPointVol += lTimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation
                                    End If
                                Next
                                lTotalPointVol += lPointVol
                            End If
                        Next

                        'find GENER loads
                        Dim lGENERLoad As Double = 0.0
                        For Each lSource As HspfConnection In lROper.Sources
                            Dim lGENERTSSum As Double = 0.0
                            Dim lMfact As Double = 0.0
                            If lSource.Source.VolName = "GENER" Then
                                Dim lGENEROperationisOutputtoWDM As Boolean = False
                                GetGENERSum(aUci, lSource, aSDateJ, aEDateJ,
                                    lGENERTSSum, lGENEROperationisOutputtoWDM)
                                If lSource.MassLink > 0 Then
                                    lGENERTSSum *= lSource.MFact 'Multiplying with MFact in Schematic Block
                                    For Each lMassLink As HspfMassLink In aUci.MassLinks
                                        Dim lGENERMassLinkMFact As Double = 0
                                        If lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "IVOL" Then
                                            lGENERMassLinkMFact = lMassLink.MFact
                                            lMfact += lGENERMassLinkMFact

                                        End If
                                    Next lMassLink
                                    lGENERTSSum *= lMfact
                                    lGENERLoad += lGENERTSSum
                                ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IVOL" Then
                                    Dim lGENERTSSourceMFact As Double = lSource.MFact
                                    lGENERTSSum *= lGENERTSSourceMFact
                                    lGENERLoad += lGENERTSSum
                                End If
                                If Not lGENEROperationisOutputtoWDM Then
                                    Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & lROper.Id & " has loadings input for the constituent " & aBalanceType & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                                End If
                            End If
                        Next lSource

                        'find upstream inflows, diversions, outflows
                        Dim lUpstreamIn As Double = 0.0
                        Dim lDiversion As Double = 0.0
                        If lUpstreamInflows.Keys.Contains(lROper.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lROper.Id)
                        End If
                        Dim lTotalInflow As Double = ValueForReach(lROper, lTotalInflowData, aSDateJ, aEDateJ)
                        Dim lOutflow As Double = ValueForReach(lROper, lOutflowData, aSDateJ, aEDateJ)
                        Dim lDownstreamReachID As Integer = lROper.DownOper("RCHRES")
                        If lROper.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                            lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                        Else
                            Dim lExitNumber As Integer = 0
                            FindDownStreamExitNumber(aUci, lROper, lExitNumber)
                            If lExitNumber = 0 Then
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            Else
                                Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lROper.Id).FindData("Constituent", "OVOL-" & lExitNumber)(0)
                                lDiversion = -(lOutflow - lExitOutFlow.Attributes.GetDefinedValue("SumAnnual").Value)
                                lOutflow = lExitOutFlow.Attributes.GetDefinedValue("SumAnnual").Value
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            End If
                        End If

                        'find landuse loads
                        Dim lNonpointVol As Double = 0.0
                        Dim lLoadingByLanduse As String = ""
                        ConstituentLoadingByLanduse(aUci, lROper, aBalanceType, lNonpointData, 0.0, lTotalPointVol, 0.0, 0.0,
                                                    lTotalInflow, lUpstreamIn, lDiversion, aSDateJ, aEDateJ, aConstProperties,
                                                    lLoadingByLanduse, lNonpointVol, lGENERLoad)
                        aReport2NPSLoads.Append(lLoadingByLanduse)

                        'find input from precip, evap
                        Dim lInflowFromPrecip As Double = ValueForReach(lROper, lTotalPrecipData, aSDateJ, aEDateJ)
                        Dim lEvapLoss As Double = ValueForReach(lROper, lEvapLossData, aSDateJ, aEDateJ)

                        'find additional inputs not included in above
                        Dim lAdditionalSource As Double = 0.0
                        lAdditionalSource = lTotalInflow - lNonpointVol - lUpstreamIn - lTotalPointVol - lGENERLoad

                        'accumulate 
                        Dim lCumulativePointNonpoint As Double = lNonpointVol + lTotalPointVol + lAdditionalSource
                        If lCumulativePointNonpointColl.Keys.Contains(lROper.Id) Then
                            lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lROper.Id)
                        End If
                        Dim lReachTrappingEfficiency As Double
                        Try
                            lReachTrappingEfficiency = lEvapLoss / lTotalInflow
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try
                        Dim lCumulativeTrappingEfficiency As Double = 0
                        Try
                            lCumulativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                        Catch
                            lReachTrappingEfficiency = 0
                        End Try
                        lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                        lField = 0
                        lField += 1 : .Value(lField) = lROper.Name & " " & lROper.Id & " - " & lROper.Description
                        lField += 1 : .Value(lField) = DoubleToString(lNonpointVol, , lNumberFormat,,, lNumberOfSignificantDigits)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalPointVol, , lNumberFormat,,, lNumberOfSignificantDigits)

                        If pGENERLoadingExists Then
                            lField += 1 : .Value(lField) = DoubleToString(lGENERLoad, , lNumberFormat,,, lNumberOfSignificantDigits)
                        End If
                        lField += 1 : .Value(lField) = DoubleToString(lDiversion, , lNumberFormat,,, lNumberOfSignificantDigits)
                        lField += 1 : .Value(lField) = DoubleToString(lAdditionalSource, , lNumberFormat,,, lNumberOfSignificantDigits)
                        lField += 1 : .Value(lField) = DoubleToString(lInflowFromPrecip, , lNumberFormat,,, lNumberOfSignificantDigits)
                        lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat,,, lNumberOfSignificantDigits)
                        lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat,,, lNumberOfSignificantDigits)
                        lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat,,, lNumberOfSignificantDigits)
                        lField += 1 : .Value(lField) = DoubleToString(lEvapLoss, , lNumberFormat,,, lNumberOfSignificantDigits)

                    Next
                    aReport1ReachBudget.Append(.ToString)
                End With

            Case "Sediment", "SED"
                aReport2NPSLoads.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" &
                                    vbTab & "Rate (tons/ac)" & vbTab & "Total Load (tons)")
                With lOutputTable
                    .Delimiter = vbTab
                    If pGENERLoadingExists Then
                        .NumFields = 13
                    Else
                        .NumFields = 12
                    End If

                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Sources"
                    If pGENERLoadingExists Then
                        lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "GENER Sources"
                    End If
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Diversion"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Mass Balance Differences/Additional Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Bed Deposition(+)/Scour(-)"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = "(%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = "(%)" : .FieldName(lField) = "Reach Trapping"

                    For Each lROper As HspfOperation In lRchresOperations
                        If Not lROper.TableExists("ACTIVITY") Then
                            MsgBox("The ACTIVITY Table for the RCHRES " & lROper.Id & " is not available. HSPEXP+ will quit.", vbOK, "ACTIVITY TABLE missing")
                            Return
                        End If
                        If lROper.Tables("ACTIVITY").Parms("SEDFG").Value = 1 Then

                            .CurrentRecord += 1
                            Dim lLocationName As String = lROper.Name.Substring(0, 1) & ":" & lROper.Id
                            Logger.Status("Generating Constituent Budget Report for " & aBalanceType & " from " & lLocationName)

                            'find point load
                            Dim lTotalPointTons As Double = 0.0
                            For Each lSource As HspfPointSource In lROper.PointSources
                                If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "ISED" Then
                                    Dim lPointTons As Double = 0.0
                                    Dim lTimeSeriesTransformaton As atcTran = atcTran.TranAverSame
                                    If lSource.Tran.ToString.Trim = "DIV" Then
                                        lTimeSeriesTransformaton = atcTran.TranSumDiv
                                    End If
                                    Dim lVolName As String = lSource.Source.VolName
                                    Dim lDSN As Integer = lSource.Source.VolId
                                    Dim lMfact As Double = lSource.MFact
                                    For i As Integer = 0 To aUci.FilesBlock.Count
                                        If aUci.FilesBlock.Value(i).Typ = lVolName Then
                                            Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
                                            Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                            If lDataSource Is Nothing Then
                                                If atcDataManager.OpenDataSource(lFileName) Then
                                                    lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                                End If
                                            End If
                                            Dim lTimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                            lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                                            lTimeseries = Aggregate(lTimeseries, atcTimeUnit.TUHour, 1, lTimeSeriesTransformaton) 'assumes run is at 1 hour timestep
                                            lPointTons += lTimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation
                                        End If
                                    Next
                                    lTotalPointTons += lPointTons
                                End If
                            Next

                            'find GENER loads
                            Dim lGENERLoad As Double = 0.0
                            For Each lSource As HspfConnection In lROper.Sources
                                Dim lGENERTSSum As Double = 0.0
                                Dim lMfact As Double = 0.0
                                If lSource.Source.VolName = "GENER" Then
                                    Dim lGENEROperationisOutputtoWDM As Boolean = False
                                    GetGENERSum(aUci, lSource, aSDateJ, aEDateJ,
                                        lGENERTSSum, lGENEROperationisOutputtoWDM)
                                    If lSource.MassLink > 0 Then
                                        lGENERTSSum *= lSource.MFact 'Multiplying with MFact in Schematic Block
                                        For Each lMassLink As HspfMassLink In aUci.MassLinks
                                            Dim GENERMassLinkMFact As Double = 0
                                            If lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "ISED" Then
                                                GENERMassLinkMFact = lMassLink.MFact
                                                lMfact += GENERMassLinkMFact
                                            End If
                                        Next lMassLink
                                        lGENERTSSum *= lMfact
                                        lGENERLoad += lGENERTSSum
                                    ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "ISED" Then
                                        Dim lGENERTSSourceMFact As Double = lSource.MFact
                                        lGENERTSSum *= lGENERTSSourceMFact
                                        lGENERLoad += lGENERTSSum
                                    End If
                                    If Not lGENEROperationisOutputtoWDM Then
                                        Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & lROper.Id & " has loadings input for the constituent " & aBalanceType & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                                    End If
                                End If
                            Next lSource

                            'find upstream inflows, diversions, outflows
                            Dim lUpstreamIn As Double = 0.0
                            Dim lDiversion As Double = 0.0
                            If lUpstreamInflows.Keys.Contains(lROper.Id) Then
                                lUpstreamIn = lUpstreamInflows.ItemByKey(lROper.Id)
                            End If
                            Dim lTotalInflow As Double = ValueForReach(lROper, lTotalInflowData, aSDateJ, aEDateJ)
                            Dim lOutflow As Double = ValueForReach(lROper, lOutflowData, aSDateJ, aEDateJ)
                            Dim lDepScour As Double = ValueForReach(lROper, lDepScourData, aSDateJ, aEDateJ)
                            Dim lDownstreamReachID As Integer = lROper.DownOper("RCHRES")
                            Try
                                If lROper.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                                    Logger.Dbg(lROper.Id)
                                    lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                Else
                                    Dim lExitNumber As Integer = 0
                                    FindDownStreamExitNumber(aUci, lROper, lExitNumber)
                                    If lExitNumber = 0 Then
                                        lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                    Else
                                        Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lROper.Id).FindData("Constituent", "OSED-TOT-EXIT" & lExitNumber)(0)
                                        lDiversion = -(lOutflow - lExitOutFlow.Attributes.GetDefinedValue("SumAnnual").Value)
                                        lOutflow = lExitOutFlow.Attributes.GetDefinedValue("SumAnnual").Value
                                        lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                    End If
                                End If
                            Catch ex As Exception
                                Logger.Msg("Trouble reading the parameters of RCHRES " & lROper.Id & ". Constituent Reports will not be generated.", MsgBoxStyle.Critical, "RCHRES Parameter Issue.")
                                Return
                            End Try

                            'find landuse loads
                            'When calculating losses and gains to the water columns from the bed depth, deposition is the loss from the stream and 
                            'scour is the gain from the stream. Using this terminology for Load Allocation report makes it consistent
                            Dim lNonpointTons As Double = 0.0
                            Dim lLoadingByLanduse As String = ""
                            ConstituentLoadingByLanduse(aUci, lROper, aBalanceType, lNonpointData, 0.0, lTotalPointTons, 0.0, -lDepScour,
                                                    lTotalInflow, lUpstreamIn, lDiversion, aSDateJ, aEDateJ, aConstProperties,
                                                    lLoadingByLanduse, lNonpointTons, lGENERLoad)
                            aReport2NPSLoads.Append(lLoadingByLanduse)

                            'find additional inputs not included in above
                            Dim lAdditionalSourceTons As Double = lTotalInflow - lNonpointTons - lUpstreamIn - lTotalPointTons - lGENERLoad

                            'accumulate
                            Dim lCumulativePointNonpoint As Double = lNonpointTons + lAdditionalSourceTons + lTotalPointTons
                            If lCumulativePointNonpointColl.Keys.Contains(lROper.Id) Then
                                lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lROper.Id)
                            End If
                            Dim lReachTrappingEfficiency As Double
                            Try
                                lReachTrappingEfficiency = lDepScour / lTotalInflow
                            Catch
                                lReachTrappingEfficiency = 0
                            End Try
                            Dim lCumulativeTrappingEfficiency As Double = 0
                            Try
                                lCumulativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                            Catch
                                lReachTrappingEfficiency = 0
                            End Try
                            lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                            lField = 0
                            lField += 1 : .Value(lField) = lROper.Name & " " & lROper.Id & " - " & lROper.Description
                            lField += 1 : .Value(lField) = DoubleToString(lNonpointTons, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lTotalPointTons, , lNumberFormat,,, lNumberOfSignificantDigits)
                            If pGENERLoadingExists Then
                                lField += 1 : .Value(lField) = DoubleToString(lGENERLoad, , lNumberFormat,,, lNumberOfSignificantDigits)
                            End If
                            lField += 1 : .Value(lField) = DoubleToString(lDiversion, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lAdditionalSourceTons, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lDepScour, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lCumulativePointNonpoint, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lCumulativeTrappingEfficiency * 100, , lNumberFormat, , , 6)
                            lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, , lNumberFormat,,, lNumberOfSignificantDigits)
                        End If
                    Next lROper
                    aReport1ReachBudget.Append(.ToString)
                End With

            Case "TN", "TAM", "TKN", "NO2NO3"
                aReport2NPSLoads.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & vbTab &
                                    "Rate (lbs/ac)" & vbTab & "Total Load (lbs)")

                With lOutputTable
                    .Delimiter = vbTab
                    If pGENERLoadingExists Then
                        .NumFields = 14
                    Else
                        .NumFields = 13
                    End If
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Atm. Depo. on Water"
                    If pGENERLoadingExists Then
                        lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "GENER Sources"
                    End If
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Diversion"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Mass Balance Differences/Additional Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Gain(+)/Loss(-)"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"

                    For Each lROper As HspfOperation In lRchresOperations
                        If lROper.Tables("ACTIVITY").Parms("NUTFG").Value = 1 Then
                            If Not lROper.TableExists("ACTIVITY") Then
                                MsgBox("The ACTIVITY Table for the RCHRES " & lROper.Id & " is not available. HSPEXP+ will quit.", vbOK, "ACTIVITY TABLE missing")
                                Return
                            End If

                            Dim lLocationName As String = lROper.Name.Substring(0, 1) & ":" & lROper.Id
                            Logger.Status("Generating Constituent Budget Report for " & aBalanceType & " from " & lLocationName)
                            .CurrentRecord += 1
                            Dim lCVON As Double = ConversionFactorfromOxygen(aUci, aBalanceType, lROper)

                            'find point load
                            Dim lTotalPointlbs As Double = 0.0
                            For Each lSource As HspfPointSource In lROper.PointSources

                                If lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1) OrElse
                                                                            (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 2) OrElse
                                                                            (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 3) OrElse
                                                                            (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub2 = 1) OrElse
                                                                            (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 3) OrElse
                                                                            (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 1) OrElse
                                                                            (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 2) OrElse
                                                                            (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then
                                    Dim lPointlbs As Double = 0.0

                                    If aBalanceType = "NO2NO3" AndAlso (lSource.Target.Member = "PKIF" OrElse
                                        lSource.Target.Member = "OXIF" OrElse lSource.Target.Member = "NUIF2" OrElse
                                        lSource.Target.MemSub1 = 2) Then
                                        Continue For
                                    End If
                                    If aBalanceType = "TAM" AndAlso (lSource.Target.Member = "PKIF" OrElse
                                        lSource.Target.Member = "OXIF" OrElse lSource.Target.MemSub1 = 1 OrElse
                                        lSource.Target.MemSub1 = 3) Then
                                        Continue For
                                    End If
                                    If aBalanceType = "TKN" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1) OrElse
                                        (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 3)) Then
                                        Continue For
                                    End If



                                    Dim TimeSeriesTransformaton As atcTran = atcTran.TranAverSame
                                    If lSource.Tran.ToString.Trim = "DIV" Then
                                        TimeSeriesTransformaton = atcTran.TranSumDiv
                                    End If
                                    Dim VolName As String = lSource.Source.VolName
                                    Dim lDSN As Integer = lSource.Source.VolId
                                    Dim lMfact As Double = lSource.MFact
                                    If lSource.Target.Member = "OXIF" Then lMfact *= lCVON
                                    If lSource.Target.Member = "PKIF" And (lSource.Target.MemSub1 = 1 Or lSource.Target.MemSub1 = 2) Then lMfact *= ConversionFactorfromBiomass(aUci, aBalanceType, lROper)
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
                                            ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                            ltimeseries = Aggregate(ltimeseries, atcTimeUnit.TUHour, 1, TimeSeriesTransformaton) 'assumes run is at 1 hour timestep
                                            lPointlbs += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation
                                        End If
                                    Next
                                    lTotalPointlbs += lPointlbs
                                End If
                            Next

                            'find GENER loads
                            Dim lGENERLoad As Double = 0.0
                            For Each lSource As HspfConnection In lROper.Sources
                                Dim lGENERTSSum As Double = 0.0
                                Dim lMfact As Double = 0.0
                                If lSource.Source.VolName = "GENER" Then
                                    Dim lGENEROperationisOutputtoWDM As Boolean = False
                                    GetGENERSum(aUci, lSource, aSDateJ, aEDateJ,
                                        lGENERTSSum, lGENEROperationisOutputtoWDM)
                                    If lSource.MassLink > 0 Then
                                        lGENERTSSum *= lSource.MFact 'Multiplying with MFact in Schematic Block
                                        For Each lMassLink As HspfMassLink In aUci.MassLinks
                                            Dim lGENERMassLinkMFact As Double = 0
                                            If lMassLink.MassLinkId = lSource.MassLink AndAlso
                                                                        ((lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 1) OrElse
                                                                        (lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 2) OrElse
                                                                        (lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 3) OrElse
                                                                        (lMassLink.Target.Member = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 1) OrElse
                                                                        (lMassLink.Target.Member = "PKIF" AndAlso lMassLink.Target.MemSub1 = 3) OrElse
                                                                        (lMassLink.Target.Member = "PKIF" AndAlso lMassLink.Target.MemSub1 = 1) OrElse
                                                                        (lMassLink.Target.Member = "PKIF" AndAlso lMassLink.Target.MemSub1 = 2) OrElse
                                                                        (lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2)) Then

                                                If aBalanceType = "NO2NO3" AndAlso (lMassLink.Target.Member = "PKIF" OrElse
                                                            lMassLink.Target.Member = "OXIF" OrElse lMassLink.Target.Member = "NUIF2" OrElse
                                                            lMassLink.Target.MemSub1 = 2) Then
                                                    Continue For
                                                End If
                                                If aBalanceType = "TAM" AndAlso (lMassLink.Target.Member = "PKIF" OrElse
                                                            lMassLink.Target.Member = "OXIF" OrElse lMassLink.Target.MemSub1 = 1 OrElse
                                                            lMassLink.Target.MemSub1 = 3) Then
                                                    Continue For
                                                End If
                                                If aBalanceType = "TKN" AndAlso ((lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 1) OrElse
                                                            (lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 3)) Then
                                                    Continue For
                                                End If



                                                lGENERMassLinkMFact = lMassLink.MFact
                                                If lMassLink.Target.Member = "OXIF" Then lGENERMassLinkMFact = lMassLink.MFact * lCVON
                                                If lMassLink.Target.Member = "PKIF" And (lMassLink.Target.MemSub1 = 1 Or lMassLink.Target.MemSub1 = 2) Then lGENERMassLinkMFact = lMassLink.MFact * ConversionFactorfromBiomass(aUci, aBalanceType, lROper)
                                                lMfact += lGENERMassLinkMFact

                                            End If
                                        Next lMassLink
                                        lGENERTSSum *= lMfact
                                        lGENERLoad += lGENERTSSum
                                    ElseIf lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1) OrElse
                                                                        (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 2) OrElse
                                                                        (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 3) OrElse
                                                                        (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub2 = 1) OrElse
                                                                        (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 3) OrElse
                                                                        (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 1) OrElse
                                                                        (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 2) OrElse
                                                                        (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then
                                        If aBalanceType = "NO2NO3" AndAlso (lSource.Target.Member = "PKIF" OrElse
                                                            lSource.Target.Member = "OXIF" OrElse lSource.Target.Member = "NUIF2" OrElse
                                                            lSource.Target.MemSub1 = 2) Then
                                            Continue For
                                        End If
                                        If aBalanceType = "TAM" AndAlso (lSource.Target.Member = "PKIF" OrElse
                                                            lSource.Target.Member = "OXIF" OrElse lSource.Target.MemSub1 = 1 OrElse
                                                            lSource.Target.MemSub1 = 3) Then
                                            Continue For
                                        End If
                                        If aBalanceType = "TKN" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1) OrElse
                                                            (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 3)) Then
                                            Continue For
                                        End If

                                        Dim lGENERTSSourceMFact As Double = lSource.MFact
                                        If lSource.Target.Member = "OXIF" Then lGENERTSSourceMFact *= lCVON
                                        If lSource.Target.Member = "PKIF" And (lSource.Target.MemSub1 = 1 Or lSource.Target.MemSub1 = 2) Then lGENERTSSourceMFact *= ConversionFactorfromBiomass(aUci, aBalanceType, lROper)
                                        lGENERTSSum *= lGENERTSSourceMFact
                                        lGENERLoad += lGENERTSSum
                                    End If
                                    If Not lGENEROperationisOutputtoWDM Then
                                        Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & lROper.Id & " has loadings input for the constituent " & aBalanceType & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                                    End If
                                End If
                            Next lSource

                            'find upstream inflows, diversions, outflows
                            Dim lUpstreamIn As Double = 0.0
                            If lUpstreamInflows.Keys.Contains(lROper.Id) Then
                                lUpstreamIn = lUpstreamInflows.ItemByKey(lROper.Id)
                            End If
                            Dim lTotalInflow As Double = ValueForReach(lROper, lTotalInflowData, aSDateJ, aEDateJ)
                            Dim lAdditionalSourcelbs As Double = 0
                            Dim lTotalAtmDep As Double = 0
                            If lAtmDepData.Count > 0 Then
                                lTotalAtmDep = ValueForReach(lROper, lAtmDepData, aSDateJ, aEDateJ)
                            End If
                            Dim lOutflow As Double = ValueForReach(lROper, lOutflowData, aSDateJ, aEDateJ)
                            Dim lDepScour As Double = lOutflow - lTotalInflow
                            Dim lDownstreamReachID As Integer = lROper.DownOper("RCHRES")
                            Dim lDiversion As Double = 0.0
                            Try
                                If lROper.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                                    lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                Else
                                    Dim lNumberOfExits As Integer = 0
                                    FindDownStreamExitNumber(aUci, lROper, lNumberOfExits)
                                    If lNumberOfExits = 0 Then
                                        lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                    Else
                                        Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lROper.Id).FindData("Constituent", "N-TOT-OUT-EXIT" & lNumberOfExits)(0)
                                        lDiversion = -(lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual")) 'Diversion is a type of loss so it is calculated as -ve
                                        lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                                        lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                    End If
                                End If
                            Catch ex As Exception
                                Logger.Msg("Trouble reading the parameters of RCHRES " & lROper.Id & ". Constituent Reports will not be generated.", MsgBoxStyle.Critical, "RCHRES Parameter Issue.")
                                Return
                            End Try

                            'find Nonpoint loads
                            Dim lNonpointlbs As Double = 0.0
                            Dim lLoadingByLanduse As String = ""
                            ConstituentLoadingByLanduse(aUci, lROper, aBalanceType, lNonpointData, lCVON, lTotalPointlbs, lTotalAtmDep, lDepScour,
                                                    lTotalInflow, lUpstreamIn, lDiversion, aSDateJ, aEDateJ, aConstProperties,
                                                    lLoadingByLanduse, lNonpointlbs, lGENERLoad)
                            aReport2NPSLoads.Append(lLoadingByLanduse)

                            'find additional inputs not included in above
                            lAdditionalSourcelbs = lTotalInflow - lNonpointlbs - lTotalAtmDep - lUpstreamIn - lTotalPointlbs - lGENERLoad

                            'accumulate
                            Dim lCumulativePointNonpoint As Double = lNonpointlbs + lTotalPointlbs + lAdditionalSourcelbs
                            If lCumulativePointNonpointColl.Keys.Contains(lROper.Id) Then
                                lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lROper.Id)
                            End If
                            Dim lReachTrappingEfficiency As Double
                            Try
                                lReachTrappingEfficiency = lDepScour / lTotalInflow
                            Catch
                                lReachTrappingEfficiency = 0
                            End Try
                            Dim lCumulativeTrappingEfficiency As Double = 0
                            Try
                                lCumulativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                            Catch
                                lReachTrappingEfficiency = 0
                            End Try
                            lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                            lField = 0
                            lField += 1 : .Value(lField) = lROper.Name & " " & lROper.Id & " - " & lROper.Description
                            lField += 1 : .Value(lField) = DoubleToString(lNonpointlbs, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lTotalPointlbs, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lTotalAtmDep, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            If pGENERLoadingExists Then
                                lField += 1 : .Value(lField) = DoubleToString(lGENERLoad, , lNumberFormat,,, lNumberOfSignificantDigits)
                            End If
                            lField += 1 : .Value(lField) = DoubleToString(lDiversion, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lAdditionalSourcelbs, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lOutflow, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lDepScour, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lCumulativePointNonpoint, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lCumulativeTrappingEfficiency * 100, 15, lNumberFormat, , , 6)
                            lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                        End If
                    Next lROper
                    aReport1ReachBudget.Append(.ToString)
                End With

            Case "TP", "ORTHO P"
                aReport2NPSLoads.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & vbTab &
                                    "Rate (lbs/ac)" & vbTab & "Total Load (lbs)")

                With lOutputTable
                    .Delimiter = vbTab
                    If pGENERLoadingExists Then
                        .NumFields = 14
                    Else
                        .NumFields = 13
                    End If
                    .NumRecords = lRchresOperations.Count + 1
                    .CurrentRecord = 1
                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Atm. Depo. on Water"
                    If pGENERLoadingExists Then
                        lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "GENER Sources"
                    End If
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Diversion"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Mass Balance Differences/Additional Sources"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Gain(+)/Loss(-)"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                    lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"

                    For Each lROper As HspfOperation In lRchresOperations
                        If Not lROper.TableExists("ACTIVITY") Then
                            MsgBox("The ACTIVITY Table for the RCHRES " & lROper.Id & " is not available. HSPEXP+ will quit.", vbOK, "ACTIVITY TABLE missing")
                            Return
                        End If
                        If lROper.Tables("ACTIVITY").Parms("NUTFG").Value = 1 Then
                            'If lID.Id = 815 Then Stop

                            Dim lLocationName As String = lROper.Name.Substring(0, 1) & ":" & lROper.Id
                            Logger.Status("Generating Constituent Budget Report for " & aBalanceType & " from " & lLocationName)
                            .CurrentRecord += 1
                            Dim lCVOP As Double = ConversionFactorfromOxygen(aUci, aBalanceType, lROper)

                            'find point loads
                            Dim lTotalPointlbs As Double = 0.0
                            For Each lSource As HspfPointSource In lROper.PointSources
                                If lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 4) OrElse
                                                                            (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub2 = 2) OrElse
                                                                            (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 4) OrElse
                                                                            (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 1) OrElse
                                                                            (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 2) OrElse
                                                                            (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then

                                    If aBalanceType = "ORTHO P" AndAlso (lSource.Target.Member = "PKIF" OrElse
                                                            lSource.Target.Member = "OXIF") Then
                                        Continue For
                                    End If

                                    Dim lPointlbs As Double = 0.0
                                    Dim TimeSeriesTransformaton As atcTran = atcTran.TranAverSame
                                    If lSource.Tran.ToString.Trim = "DIV" Then
                                        TimeSeriesTransformaton = atcTran.TranSumDiv
                                    End If
                                    Dim lVolName As String = lSource.Source.VolName
                                    Dim lDSN As Integer = lSource.Source.VolId
                                    Dim lMfact As Double = lSource.MFact
                                    If lSource.Target.Member = "OXIF" Then lMfact *= lCVOP
                                    If lSource.Target.Member = "PKIF" And (lSource.Target.MemSub1 = 1 Or lSource.Target.MemSub1 = 2) Then lMfact *= ConversionFactorfromBiomass(aUci, aBalanceType, lROper)
                                    For i As Integer = 0 To aUci.FilesBlock.Count

                                        If aUci.FilesBlock.Value(i).Typ = lVolName Then
                                            Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
                                            Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                            If lDataSource Is Nothing Then
                                                If atcDataManager.OpenDataSource(lFileName) Then
                                                    lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                                End If
                                            End If
                                            Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                            ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                            ltimeseries = Aggregate(ltimeseries, atcTimeUnit.TUHour, 1, TimeSeriesTransformaton) 'assumes run is at 1 hour timestep
                                            lPointlbs += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation
                                        End If
                                    Next
                                    lTotalPointlbs += lPointlbs
                                End If
                            Next

                            'find GENER loads
                            Dim lGENERLoad As Double = 0.0
                            For Each lSource As HspfConnection In lROper.Sources
                                Dim lGENERTSSum As Double = 0.0
                                Dim lMfact As Double = 0.0
                                If lSource.Source.VolName = "GENER" Then
                                    Dim lGENEROperationisOutputtoWDM As Boolean = False
                                    GetGENERSum(aUci, lSource, aSDateJ, aEDateJ,
                                        lGENERTSSum, lGENEROperationisOutputtoWDM)
                                    If lSource.MassLink > 0 Then
                                        lGENERTSSum *= lSource.MFact 'Multiplying with MFact in Schematic Block
                                        For Each lMassLink As HspfMassLink In aUci.MassLinks
                                            Dim lGENERMassLinkMFact As Double = 0
                                            If lMassLink.MassLinkId = lSource.MassLink AndAlso
                                                                        ((lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4) OrElse
                                                                        (lMassLink.Target.Member = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 2) OrElse
                                                                        (lMassLink.Target.Member = "PKIF" AndAlso lMassLink.Target.MemSub1 = 4) OrElse
                                                                        (lMassLink.Target.Member = "PKIF" AndAlso lMassLink.Target.MemSub1 = 1) OrElse
                                                                        (lMassLink.Target.Member = "PKIF" AndAlso lMassLink.Target.MemSub1 = 2) OrElse
                                                                        (lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2)) Then
                                                If aBalanceType = "ORTHO P" AndAlso (lMassLink.Target.Member = "PKIF" OrElse
                                                            lMassLink.Target.Member = "OXIF") Then
                                                    Continue For
                                                End If
                                                lGENERMassLinkMFact = lMassLink.MFact
                                                If lMassLink.Target.Member = "OXIF" Then lGENERMassLinkMFact = lMassLink.MFact * lCVOP
                                                If lMassLink.Target.Member = "PKIF" And (lMassLink.Target.MemSub1 = 1 Or lMassLink.Target.MemSub1 = 2) Then lGENERMassLinkMFact = lMassLink.MFact * ConversionFactorfromBiomass(aUci, aBalanceType, lROper)
                                                lMfact += lGENERMassLinkMFact
                                            End If
                                        Next lMassLink
                                        lGENERTSSum *= lMfact
                                        lGENERLoad += lGENERTSSum
                                    ElseIf lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 4) OrElse
                                                                        (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub2 = 2) OrElse
                                                                        (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 4) OrElse
                                                                        (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 1) OrElse
                                                                        (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 2) OrElse
                                                                        (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then
                                        If aBalanceType = "ORTHO P" AndAlso (lSource.Target.Member = "PKIF" OrElse
                                                            lSource.Target.Member = "OXIF") Then
                                            Continue For
                                        End If
                                        Dim lGENERTSSourceMFact As Double = lSource.MFact
                                        If lSource.Target.Member = "OXIF" Then lGENERTSSourceMFact *= lCVOP
                                        If lSource.Target.Member = "PKIF" And (lSource.Target.MemSub1 = 1 Or lSource.Target.MemSub1 = 2) Then lGENERTSSourceMFact *= ConversionFactorfromBiomass(aUci, aBalanceType, lROper)
                                        lGENERTSSum *= lGENERTSSourceMFact
                                        lGENERLoad += lGENERTSSum
                                    End If
                                    If Not lGENEROperationisOutputtoWDM Then
                                        Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & lROper.Id & " has loadings input for the constituent " & aBalanceType & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                                    End If
                                End If
                            Next lSource

                            'find upstream inflows, diversions, outflows
                            Dim lUpstreamIn As Double = 0.0
                            If lUpstreamInflows.Keys.Contains(lROper.Id) Then
                                lUpstreamIn = lUpstreamInflows.ItemByKey(lROper.Id)
                            End If
                            Dim lTotalInflow As Double = ValueForReach(lROper, lTotalInflowData, aSDateJ, aEDateJ)
                            Dim lOutflow As Double = ValueForReach(lROper, lOutflowData, aSDateJ, aEDateJ) 'TotalForReach(lID, lAreas, lOutflowData)
                            Dim lTotalAtmDep As Double = 0
                            If lAtmDepData.Count > 0 Then
                                lTotalAtmDep = ValueForReach(lROper, lAtmDepData, aSDateJ, aEDateJ)
                            End If
                            Dim lDiversion As Double = 0.0
                            Dim lDownstreamReachID As Integer = lROper.DownOper("RCHRES")
                            Try
                                If lROper.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                                    lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                Else
                                    Dim lExitNumber As Integer = 0
                                    FindDownStreamExitNumber(aUci, lROper, lExitNumber)
                                    If lExitNumber = 0 Then
                                        lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                    Else
                                        Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lROper.Id).FindData("Constituent", "P-TOT-OUT-EXIT" & lExitNumber)(0)
                                        lDiversion = -(lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual"))
                                        lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                                        lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                    End If
                                End If
                            Catch ex As Exception
                                Logger.Msg("Trouble reading the parameters of RCHRES " & lROper.Id & ". Constituent Reports will not be generated.", MsgBoxStyle.Critical, "RCHRES Parameter Issue.")
                                Return
                            End Try
                            Dim lDepScour As Double = lOutflow - lTotalInflow - lDiversion

                            'find nonpoint loads
                            Dim lNonpointlbs As Double = 0.0
                            Dim lLoadingByLanduse As String = ""
                            ConstituentLoadingByLanduse(aUci, lROper, aBalanceType, lNonpointData, lCVOP, lTotalPointlbs, lTotalAtmDep, lDepScour,
                                                    lTotalInflow, lUpstreamIn, lDiversion, aSDateJ, aEDateJ, aConstProperties,
                                                    lLoadingByLanduse, lNonpointlbs, lGENERLoad)
                            aReport2NPSLoads.Append(lLoadingByLanduse)

                            'find additional inputs not included in above
                            Dim lAdditionalSourcelbs As Double = 0
                            lAdditionalSourcelbs = lTotalInflow - lNonpointlbs - lUpstreamIn - lTotalAtmDep - lTotalPointlbs - lGENERLoad

                            'accumulate
                            Dim lCumulativePointNonpoint As Double = lNonpointlbs + lAdditionalSourcelbs
                            If lCumulativePointNonpointColl.Keys.Contains(lROper.Id) Then
                                lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lROper.Id)
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
                            lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                            lField = 0
                            lField += 1 : .Value(lField) = lROper.Name & " " & lROper.Id & " - " & lROper.Description
                            lField += 1 : .Value(lField) = DoubleToString(lNonpointlbs, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lTotalPointlbs, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lTotalAtmDep, , lNumberFormat,,, lNumberOfSignificantDigits)
                            If pGENERLoadingExists Then
                                lField += 1 : .Value(lField) = DoubleToString(lGENERLoad, , lNumberFormat,,, lNumberOfSignificantDigits)
                            End If
                            lField += 1 : .Value(lField) = DoubleToString(lDiversion, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lAdditionalSourcelbs, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lDepScour, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lCumulativePointNonpoint, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, , lNumberFormat, , , 6)
                            lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, , lNumberFormat,,, lNumberOfSignificantDigits)
                        End If
                    Next
                    aReport1ReachBudget.Append(.ToString)
                End With
        End Select

        If aBalanceType = "TN" Or aBalanceType = "TP" Or aBalanceType = "ORTHO P" Or
           aBalanceType = "TAM" Or aBalanceType = "TKN" Or aBalanceType = "NO2NO3" Or
           aBalanceType = "Sediment" Then
            'do load allocation reports
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
            lLandusesHeader = lLandusesHeader.Replace("GENERSources", "GENER Sources")
            lLandusesHeader = lLandusesHeader.Replace("AdditionalSources", "Mass Balance Differences/Additional Sources*")

            Dim lPointSourcesPlacesLocation As Integer = lLandusesHeader.IndexOf("PointSources")
            Dim lLandUsesList() As String = lLandusesHeader.Substring(1, lPointSourcesPlacesLocation - 2).Split(vbTab)

            Dim lLoadingRateSummary As String = "Loading Rate Summary for " & aBalanceType & " by Each Land Land Use: " & lUnits & "/acre/yr" & vbCrLf

            lLoadingRateSummary &= "Landuse" & vbTab & "Mean" & vbTab & "Min" & vbTab & "Max" & vbCrLf
            For Each lLandUse As String In lLandUsesList
                Dim lLoadingRateLine1 As String = lLandUse & vbTab
                Dim lLoadingRateLine2 As String = lLandUse & vbTab
                lLoadingRateSummary &= lLandUse & vbTab

                Dim lMax As Double = 0.0
                Dim lMin As Double = 1.0E+30
                Dim lSum As Double = 0.0
                Dim lCount As Int16 = 0

                Dim LoadingRateCollectionForEachLanduse As New List(Of Double)

                For Each lKey As String In pOutputConnectionArea.Keys
                    Dim lColonLocation As Integer = lKey.IndexOf(":")
                    Dim lLandUseFromKey As String = SafeSubstring(lKey, 0, lColonLocation - 1)
                    Dim lOperationType As String = SafeSubstring(lKey, lColonLocation - 1, 1)
                    Dim lOperationNumber As Integer = SafeSubstring(lKey, lColonLocation + 1, 3)
                    Dim lLoadingRate As Double = 0.0
                    If lOperationType & ":" & lLandUseFromKey = lLandUse Then
                        lCount += 1
                        lLoadingRateLine1 &= lOperationType & ":" & lOperationNumber & vbTab
                        lLoadingRate = pOutputLoad.ItemByKey(lKey) / pOutputConnectionArea.ItemByKey(lKey)
                        lLoadingRateLine2 &= DoubleToString(lLoadingRate, , lNumberFormat,,, lNumberOfSignificantDigits) & vbTab
                        lSum += lLoadingRate
                        If lLoadingRate > lMax Then
                            lMax = lLoadingRate
                        End If
                        If lLoadingRate < lMin Then
                            lMin = lLoadingRate
                        End If
                        LoadingRateCollectionForEachLanduse.Add(lLoadingRate)
                    End If
                Next lKey

                lLoadingRateSummary &= DoubleToString(lSum / lCount, , lNumberFormat,,, lNumberOfSignificantDigits) & vbTab &
                    DoubleToString(lMin, , lNumberFormat,,, lNumberOfSignificantDigits) &
                    vbTab & DoubleToString(lMax, , lNumberFormat,,, lNumberOfSignificantDigits) & vbCrLf

                aReport7LoadingRate.AppendLine(lLoadingRateLine1)
                aReport7LoadingRate.AppendLine(lLoadingRateLine2)
            Next lLandUse

            aReport7LoadingRate.AppendLine()
            aReport7LoadingRate.AppendLine()
            aReport7LoadingRate.AppendLine(lLoadingRateSummary)

            aReport3LoadAllocationAll.AppendLine("Reach" & lLandusesHeader & vbTab & "Total**")
            lReport4.AppendLine("Reach" & lLandusesHeader & vbTab & "Total**")
            If aOutputLocations.Count > 0 Then
                aReport5LoadAllocationLocations.AppendLine("Reach" & lLandusesHeader & vbTab & "Total**")
                lReport6.AppendLine("Reach" & lLandusesHeader & vbTab & "Total**")
            Else
                aReport5LoadAllocationLocations.AppendLine("Load Allocation Report was not requested for specific reaches.  The Load Allocation Report")
                aReport5LoadAllocationLocations.AppendLine("for all the reaches is available as " & aBalanceType & "_" & aScenario & "_" & "LoadAllocation.txt")
            End If

            For Each lReach As String In lReaches
                Dim lDataForOneBarGraph As New BarGraphItem
                lDataForOneBarGraph.Scenario = aScenario
                lDataForOneBarGraph.TimeSpan = TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: ")
                lDataForOneBarGraph.Units = "(" & lUnits & "/yr)"
                lDataForOneBarGraph.Constituent = aBalanceType

                aReport3LoadAllocationAll.Append(aUci.OpnBlks("RCHRES").OperFromID(lReach.Substring(5)).Caption.Substring(12))
                lReport4.Append(aUci.OpnBlks("RCHRES").OperFromID(lReach.Substring(5)).Caption.Substring(12))

                If aOutputLocations.Count > 0 Then
                    For Each lOutputLocation As String In aOutputLocations
                        If lReach.Substring(5) = lOutputLocation.Substring(2) Then
                            aReport5LoadAllocationLocations.Append(aUci.OpnBlks("RCHRES").OperFromID(lReach.Substring(5)).Caption.Substring(12))
                            lReport6.Append(aUci.OpnBlks("RCHRES").OperFromID(lReach.Substring(5)).Caption.Substring(12))
                        End If
                    Next
                End If

                Dim lTotal As Double = 0.0
                For Each lSourceDescription As String In lLandUses
                    Dim lKey As String = lReach & " " & lSourceDescription
                    Dim lValue As Double = pRunningTotals.ItemByKey(lKey)

                    aReport3LoadAllocationAll.Append(vbTab & FormatNumber(lValue, 2, TriState.True, TriState.False, TriState.False))
                    If aOutputLocations.Count > 0 Then
                        For Each lOutputLocation As String In aOutputLocations
                            If lReach.Substring(5) = lOutputLocation.Substring(2) Then
                                lSourceDescription = lSourceDescription.Replace("Loss", "Cumulative Instream Losses")
                                lSourceDescription = lSourceDescription.Replace("Gain", "Cumulative Instream Gains")
                                lSourceDescription = lSourceDescription.Replace("AdditionalSources", "Additional Sources")
                                lSourceDescription = lSourceDescription.Replace("DirectAtmosphericDeposition", "Direct Atm. Desposition")
                                lSourceDescription = lSourceDescription.Replace("GENERSources", "GENER Sources")
                                lSourceDescription = lSourceDescription.Replace("PointSources", "Point Sources")

                                aReport5LoadAllocationLocations.Append(vbTab & FormatNumber(lValue, 2, TriState.True, TriState.False, TriState.False))
                                lDataForOneBarGraph.LabelValueCollection.Add(lSourceDescription, lValue)
                                lDataForOneBarGraph.Location = lReach
                            End If
                        Next
                    End If
                    If Not lSourceDescription.Contains("Loss") Then
                        lTotal += lValue
                    End If
                Next lSourceDescription

                If lDataForOneBarGraph.LabelValueCollection.Count > 0 Then
                    aDataForAllBarGraphs.Add(lReach, lDataForOneBarGraph)
                End If

                aReport3LoadAllocationAll.AppendLine(vbTab & FormatNumber(lTotal, 2, TriState.True, TriState.False, TriState.False))
                If aOutputLocations.Count > 0 Then
                    For Each lOutputLocation As String In aOutputLocations
                        If lReach.Substring(5) = lOutputLocation.Substring(2) Then
                            aReport5LoadAllocationLocations.AppendLine(vbTab & FormatNumber(lTotal, 2, TriState.True, TriState.False, TriState.False))
                        End If
                    Next
                End If

                Dim lPercentTotal As Double = 0.0
                For Each lSourceDescription As String In lLandUses
                    Dim lKey As String = lReach & " " & lSourceDescription
                    Dim lValue As Double = pRunningTotals.ItemByKey(lKey)

                    lReport4.Append(vbTab & FormatNumber(lValue * 100 / lTotal, 2, TriState.True, TriState.False, TriState.False))
                    If aOutputLocations.Count > 0 Then
                        For Each lOutputLocation As String In aOutputLocations
                            If lReach.Substring(5) = lOutputLocation.Substring(2) Then
                                lReport6.Append(vbTab & FormatNumber(lValue * 100 / lTotal, 2, TriState.True, TriState.False, TriState.False))
                            End If
                        Next
                    End If
                    If Not lSourceDescription.Contains("Loss") Then
                        lPercentTotal += lValue * 100 / lTotal
                    End If
                Next lSourceDescription

                lReport4.Append(vbTab & FormatNumber(lPercentTotal, 1, TriState.True, TriState.False, TriState.False))
                lReport4.AppendLine()
                If aOutputLocations.Count > 0 Then
                    For Each lOutputLocation As String In aOutputLocations
                        If lReach.Substring(5) = lOutputLocation.Substring(2) Then
                            lReport6.Append(vbTab & FormatNumber(lPercentTotal, 1, TriState.True, TriState.False, TriState.False))
                            lReport6.AppendLine()
                        End If
                    Next
                End If
            Next
            aReport3LoadAllocationAll.AppendLine("*The additional sources may include sources other than nonpoint sources, point sources, atmospheric deposition, and upstream contribution.")
            aReport3LoadAllocationAll.AppendLine("**The totals do not include losses as they have already been applied to the respective sources")
            lReport4.AppendLine("*The additional sources may include sources other than nonpoint sources, point sources, atmospheric deposition, and upstream contribution.")
            lReport4.AppendLine("**The totals do not include losses as they have already been applied to the respective sources")

            aReport5LoadAllocationLocations.AppendLine("*The additional sources may include sources other than nonpoint sources, point sources, atmospheric deposition, and upstream contribution.")
            aReport5LoadAllocationLocations.AppendLine("**The totals do not include losses as they have already been applied to the respective sources")
            lReport6.AppendLine("*The additional sources may include sources other than nonpoint sources, point sources, atmospheric deposition, and upstream contribution.")
            lReport6.AppendLine("**The totals do not include losses as they have already been applied to the respective sources")

            aReport3LoadAllocationAll.Append(lReport4.ToString)
            aReport5LoadAllocationLocations.Append(lReport6.ToString)
            'The Total column in percent line needs to be recalculated.
        Else
            aReport3LoadAllocationAll.AppendLine("Load Allocation Report are not produced for " & aBalanceType)
            aReport5LoadAllocationLocations.AppendLine("Load Allocation Report are not produced for " & aBalanceType)
        End If
        pRunningTotals.Clear()

    End Sub

    Private Function ValueForReach(ByVal aReach As HspfOperation,
                                   ByVal aReachData As atcTimeseriesGroup,
                                   ByVal aSJDate As Double,
                                   ByVal aEJDate As Double) As Double
        Dim lOutFlow As Double
        For Each aTimeseries As atcTimeseries In aReachData.FindData("Location", "R:" & aReach.Id)
            lOutFlow += SubsetByDate(aTimeseries, aSJDate, aEJDate, Nothing).Attributes.GetValue("SumAnnual")
        Next
        Return lOutFlow
    End Function

    'FELU stands for For Each Land Use
    Private Sub felu(ByVal aUCI As HspfUci,
                        ByVal aReach As HspfOperation,
                        ByVal aBalanceType As String,
                        ByVal aVolName As String,
                        ByVal aLandUses As atcCollection,
                        ByVal aNonpointData As atcTimeseriesGroup,
                        ByVal aConversionFactor As Double,
                        ByRef aLoadingByLanduse As String,
                        ByRef aReachTotal As Double,
                        ByVal aReporting As Boolean,
                        ByRef aContribPercent As atcCollection,
                        ByVal aSDateJ As Double,
                        ByVal aEDateJ As Double,
                        Optional ByVal aConstProperties As List(Of ConstituentProperties) = Nothing)

        Dim lTotalIndex As Integer = 0
        Dim lTotal As Double = 0

        For Each lLandUse As String In aLandUses
            Dim lFound As Boolean = False
            For Each lConnection As HspfConnection In aReach.Sources
                If lConnection.Source.VolName = aVolName Then
                    If lConnection.Source.Opn Is Nothing Then
                        If Logger.Msg("The operation " & lConnection.Source.VolName & " " & lConnection.Source.VolId & " is not 
                                        initialized in the OPN SEQUENCE Block", MsgBoxStyle.OkCancel, "HSPEXP+") = MsgBoxResult.Cancel Then
                            Throw New ApplicationException("Uninitialized")
                        Else
                            Continue For
                        End If
                    End If
                    If lConnection.Source.Opn.Description = lLandUse Then
                        'Need to add test to make sure that the operation was listed in the OPN SEQUENCE block.
                        lFound = True
                        Dim lConnectionArea As Double = lConnection.MFact
                        If lConnectionArea = 0 Then lConnectionArea = 1.0E-30
                        '1.0E-30 was added to make sure that rate is calculated for operations with zero areas
                        Dim lMassLinkID As Integer = lConnection.MassLink
                        Dim lTestLocation As String = lConnection.Source.VolName.Substring(0, 1) & ":" & lConnection.Source.VolId
                        'lTestLocation is the operation Type and operation number

                        Dim lConstituentTotal As Double = 0
                        lTotal = 0
                        For Each lTs As atcTimeseries In aNonpointData.FindData("Location", lTestLocation)
                            lTs = SubsetByDate(lTs, aSDateJ, aEDateJ, Nothing)
                            Dim lMassLinkFactor As Double = 0.0
                            If lTs.Attributes.GetDefinedValue("SumAnnual").Value > 0 Then
                                Dim lConstNameMassLink As String = lTs.Attributes.GetValue("Constituent")
                                If Not (aConstProperties.Count = 0 OrElse (lConnection.Source.Opn.Name = "PERLND" AndAlso
                                        lConnection.Source.Opn.Tables("ACTIVITY").Parms("PQALFG").Value = "0")) Then
                                    lConstNameMassLink = Split(lTs.Attributes.GetValue("Constituent"), "-", 2)(1)
                                    Dim ConstNameEXP As String = ""
                                    For Each constt As ConstituentProperties In aConstProperties
                                        If constt.ConstituentNameInUCI.ToUpper = lConstNameMassLink Then
                                            ConstNameEXP = constt.ConstNameForEXPPlus
                                            If ConstNameEXP = "TAM" Then ConstNameEXP = "NH3+NH4"
                                            lConstNameMassLink = Split(lTs.Attributes.GetValue("Constituent"), "-", 2)(0) & "-" & ConstNameEXP
                                            Exit For
                                        End If
                                    Next
                                End If

                                'If ConstNameMassLink = "SOSLD" Then Stop
                                lMassLinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lConstNameMassLink, aBalanceType,
                                                                               aConversionFactor, aMultipleIndex:=0)
                            End If

                            lConstituentTotal = lTs.Attributes.GetDefinedValue("SumAnnual").Value * lMassLinkFactor * lConnectionArea
                            'lConstituentTotal = lConstituentRate * lConnectionArea

                            lTotal += lConstituentTotal
                            'The area and multiplication factor is multiplied to get the total load of the particular constituent to the reach "aReach" from the specific
                            'land use and then lTotal accumulates that load
                        Next

                        felu2(aUCI, aReach, aBalanceType, aVolName, aLandUses, aLoadingByLanduse, aReachTotal, aReporting, aContribPercent, lTotal, lConnectionArea, lLandUse, lTestLocation)
                    End If
                End If
            Next

            If Not lFound Then
                felu2(aUCI, aReach, aBalanceType, aVolName, aLandUses, aLoadingByLanduse, aReachTotal, aReporting, aContribPercent, 0, 0, lLandUse, "")
            End If
        Next
    End Sub

    Private Sub felu2(ByVal aUCI As HspfUci,
                        ByVal aReach As HspfOperation,
                        ByVal aBalanceType As String,
                        ByVal aVolName As String,
                        ByVal aLandUses As atcCollection,
                        ByRef aLoadingByLanduse As String,
                        ByRef aReachTotal As Double,
                        ByVal aReporting As Boolean,
                        ByRef aContribPercent As atcCollection,
                        ByRef aTotal As Double,
                        ByVal aConnectionArea As Double,
                        ByVal aLandUse As String,
                        ByVal aTestLocation As String)
        Dim lVolPrefix As String = aVolName.Substring(0, 1) & ":"
        Dim lTotal2 As Double = aTotal 'aTotal2 is used for reporting the loading from the local land area to the reach.
        Dim lKey As String = "Reach" & aReach.Id & " " & lVolPrefix & aLandUse
        If aReporting Then
            'If aReach.Id = 113 Then Stop
            Dim lPercentContrib As Double = 0.0
            Dim lNumberofTributaries As Integer = 0
            Dim lCheckedTributary As Boolean = False
            Dim lUpstreamLoadByCategory As Double = 0.0
            If aContribPercent.Keys.Contains(lKey) Then
                lCheckedTributary = True
            Else
                For Each lTributary As HspfConnection In aReach.Sources

                    If lTributary.Source.VolName = "RCHRES" Then
                        Dim lTributaryID As Integer = lTributary.Source.VolId
                        lNumberofTributaries += 1
                        lUpstreamLoadByCategory += pRunningTotals.ItemByKey("Reach" & lTributaryID & " " & lVolPrefix & aLandUse)
                    End If
                Next

            End If
            aTotal += lUpstreamLoadByCategory
            lPercentContrib = aTotal * 100 / aReachTotal

            aContribPercent.Increment(lKey, lPercentContrib)

            'If aConnectionArea > 0 Then
            Dim lrate As Double = lTotal2 / aConnectionArea
            If aConnectionArea = 1.0E-30 Then
                aConnectionArea = 0
                lTotal2 = 0
            End If
            If aTestLocation.Contains(":") Then
                pOutputConnectionArea.Increment(aLandUse & aTestLocation, aConnectionArea)
                pOutputLoad.Increment(aLandUse & aTestLocation, lTotal2)

                aLoadingByLanduse &= aReach.Caption.ToString.Substring(10) & vbTab _
                    & aTestLocation & " " _
                    & aLandUse & vbTab _
                    & aConnectionArea & vbTab _
                    & DoubleToString(lrate, 15, "#,##0.###") & vbTab &
                                      DoubleToString(lTotal2, 15, "#,##0.###") & vbTab & vbCrLf
            End If
        Else
            'If aReach.Id = 260 Then Stop
            Dim lCheckedTheTributary As Boolean = False

            If pRunningTotals.Keys.Contains(lKey) Then lCheckedTheTributary = True
            pRunningTotals.Increment(lKey, aTotal)
            'A key of the land use type and downstream reach is made in pRunningTotals to save the total land from the specific land use.
            aReachTotal += aTotal

            If Not lCheckedTheTributary Then
                For Each lTributary As HspfConnection In aReach.Sources
                    If lTributary.Source.VolName = "RCHRES" Then
                        Dim lTributaryID As String = lTributary.Source.VolId
                        Dim UpstreamLoadByCategory As Double = pRunningTotals.ItemByKey("Reach" & lTributaryID & " " & lVolPrefix & aLandUse)
                        pRunningTotals.Increment(lKey, UpstreamLoadByCategory)
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub ConstituentLoadingByLanduse(ByVal aUCI As HspfUci,
                                                 ByVal aReach As HspfOperation,
                                                 ByVal aBalanceType As String,
                                                 ByVal aNonpointData As atcTimeseriesGroup,
                                                 ByVal aConversionFactor As Double,
                                                 ByVal aPoint As Double,
                                                 ByVal aAtmDep As Double,
                                                 ByVal GainLoss As Double,
                                                 ByVal aTotalInflow As Double,
                                                 ByVal aUpstreamIn As Double,
                                                 ByVal aDiversion As Double,
                                                 ByVal aSDateJ As Double,
                                                 ByVal aEDateJ As Double,
                                                 ByVal aConstProperties As List(Of ConstituentProperties),
                                                 ByRef aLoadingByLanduse As String,
                                                 ByRef aReachTotal As Double,
                                                 ByRef aGENERLoad As Double)
        Dim lLoadingByLanduse As String = ""
        Dim lReachTotal As Double = 0.0
        Dim lUpStreamDiversion As Double = 0.0
        Dim lContribPercent As New atcCollection

        'If aReach.Id = "148" Then Stop
        felu(aUCI, aReach, aBalanceType, "PERLND", pPERLND, aNonpointData, aConversionFactor, lLoadingByLanduse, lReachTotal, False, lContribPercent, aSDateJ, aEDateJ, aConstProperties)
        felu(aUCI, aReach, aBalanceType, "IMPLND", pIMPLND, aNonpointData, aConversionFactor, lLoadingByLanduse, lReachTotal, False, lContribPercent, aSDateJ, aEDateJ, aConstProperties)

        For Each lTributary As HspfConnection In aReach.Sources
            If lTributary.Source.VolName = "RCHRES" Then
                Dim lTributaryId As String = lTributary.Source.VolId
                lUpStreamDiversion += pRunningTotals.ItemByKey("Reach" & lTributaryId & " Diversion")
            End If
        Next
        aTotalInflow += -lUpStreamDiversion
        'If aReach.Id = 102 Then Stop
        felu(aUCI, aReach, aBalanceType, "PERLND", pPERLND, aNonpointData, aConversionFactor, lLoadingByLanduse, aTotalInflow, True, lContribPercent, aSDateJ, aEDateJ, aConstProperties)
        felu(aUCI, aReach, aBalanceType, "IMPLND", pIMPLND, aNonpointData, aConversionFactor, lLoadingByLanduse, aTotalInflow, True, lContribPercent, aSDateJ, aEDateJ, aConstProperties)

        Dim lAdditionalSource As Double = aTotalInflow - lReachTotal - aAtmDep - aPoint - aUpstreamIn - aGENERLoad + lUpStreamDiversion

        For Each lTributary As HspfConnection In aReach.Sources
            If lTributary.Source.VolName = "RCHRES" Then
                Dim lTributaryId As String = lTributary.Source.VolId
                aPoint += pRunningTotals.ItemByKey("Reach" & lTributaryId & " PointSources")
                lAdditionalSource += pRunningTotals.ItemByKey("Reach" & lTributaryId & " AdditionalSources")
                aAtmDep += pRunningTotals.ItemByKey("Reach" & lTributaryId & " DirectAtmosphericDeposition")
                aDiversion += pRunningTotals.ItemByKey("Reach" & lTributaryId & " Diversion")
                If pGENERLoadingExists Then
                    'aGENERLoad += pRunningTotals.ItemByKey("Reach" & lTributaryId & " GENERSources")
                End If
                pRunningTotals.Increment("Reach" & aReach.Id & " Gain", pRunningTotals.ItemByKey("Reach" & lTributaryId & " Gain"))
                pRunningTotals.Increment("Reach" & aReach.Id & " Loss", pRunningTotals.ItemByKey("Reach" & lTributaryId & " Loss"))
                'lContribPercent.Increment("Reach" & aReach.Id & " Gain", pRunningTotals.ItemByKey("Reach" & lTributaryId & " Gain") * 100 / aTotalInflow)
            End If
        Next

        pRunningTotals.Add("Reach" & aReach.Id & " PointSources", aPoint)
        pRunningTotals.Add("Reach" & aReach.Id & " DirectAtmosphericDeposition", aAtmDep)
        If pGENERLoadingExists Then
            pRunningTotals.Add("Reach" & aReach.Id & " GENERSources", aGENERLoad)
        End If
        pRunningTotals.Add("Reach" & aReach.Id & " AdditionalSources", lAdditionalSource)
        pRunningTotals.Add("Reach" & aReach.Id & " Diversion", aDiversion)

        If GainLoss < 0 Then
            pRunningTotals.Increment("Reach" & aReach.Id & " Loss", GainLoss)
            pRunningTotals.Increment("Reach" & aReach.Id & " Gain", 0.0)
        Else
            pRunningTotals.Increment("Reach" & aReach.Id & " Loss", 0.0)
            pRunningTotals.Increment("Reach" & aReach.Id & " Gain", GainLoss)
        End If
        'If aReach.Id = 113 Then Stop
        lContribPercent.Add("Reach" & aReach.Id & " Gain", pRunningTotals.ItemByKey("Reach" & aReach.Id & " Gain") * 100 / aTotalInflow)
        lContribPercent.Add("Reach" & aReach.Id & " PointSources", aPoint * 100 / aTotalInflow)
        lContribPercent.Add("Reach" & aReach.Id & " DirectAtmosphericDeposition", aAtmDep * 100 / aTotalInflow)
        lContribPercent.Add("Reach" & aReach.Id & " GENERSources", aGENERLoad * 100 / aTotalInflow)
        lContribPercent.Add("Reach" & aReach.Id & " AdditionalSources", lAdditionalSource * 100 / aTotalInflow)

        If GainLoss < 0 Then
            For Each lKey As String In lContribPercent.Keys
                'If lKey.Contains("Gain") Then Stop
                For Each lTotalsKey As String In pRunningTotals.Keys
                    Dim lReachIDLength As Integer = aReach.Id.ToString.Length
                    If lTotalsKey.Contains("Reach" & aReach.Id & " ") AndAlso lTotalsKey.Contains(SafeSubstring(lKey, 6 + lReachIDLength)) Then
                        pRunningTotals.ItemByKey(lTotalsKey) = pRunningTotals.ItemByKey(lTotalsKey) + lContribPercent.ItemByKey(lKey) * GainLoss / 100
                        Exit For
                    End If

                Next lTotalsKey
            Next lKey
        End If

        aLoadingByLanduse = lLoadingByLanduse
        aReachTotal = lReachTotal
    End Sub

    Private Function MultiFactorForPointSource(ByVal aTStep As Integer, ByVal aTimeUnit As String, ByVal aTransformation As String,
                                               ByVal aDelta As atcTimeUnit) As Double
        Dim lMultiFactor As Double = 1.0
        If Trim(aTransformation) = "DIV" Then
            lMultiFactor = 1.0
        Else
            Select Case aTransformation
                Case "SAME"
                    If aDelta / 60 = 1 AndAlso aTimeUnit = "TUDay" AndAlso aTStep = 1 Then
                        lMultiFactor = 24.0
                    End If
                    If aDelta = 60 AndAlso aTimeUnit = "TUMonth" AndAlso aTStep = 1 Then
                        lMultiFactor = 24.0 * 365.25 / 12
                    End If
            End Select
        End If

        Return lMultiFactor
    End Function

    Private Sub GetGENERSum(ByVal aUCI As HspfUci, ByVal aSource As HspfConnection, ByVal aSDateJ As Double, ByVal aEDateJ As Double,
                            ByRef aGenerSum As Double, ByRef aGENEROperationisOutputtoWDM As Boolean)
        aGenerSum = 0
        Dim lGENERID As Integer = aSource.Source.VolId
        aGENEROperationisOutputtoWDM = False
        Dim lGENEROperation As HspfOperation = aSource.Source.Opn
        If Not lGENEROperation Is Nothing Then
            For Each lEXTTarget As HspfConnection In lGENEROperation.Targets
                If lEXTTarget.Target.VolName.Contains("WDM") Then
                    aGENEROperationisOutputtoWDM = True
                    Dim lWDMFile As String = lEXTTarget.Target.VolName.ToString
                    Dim lDSN As Integer = lEXTTarget.Target.VolId
                    For i As Integer = 0 To aUCI.FilesBlock.Count
                        If aUCI.FilesBlock.Value(i).Typ = lWDMFile Then
                            Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                            Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                            If lDataSource Is Nothing Then
                                If atcDataManager.OpenDataSource(lFileName) Then
                                    lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                End If
                            End If
                            Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                            ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                            aGenerSum = ltimeseries.Attributes.GetDefinedValue("Sum").Value / YearCount(aSDateJ, aEDateJ)
                        End If
                    Next
                End If
            Next lEXTTarget
        End If
    End Sub
End Module
