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
    Private pMessageShown As Boolean = False
    Public Function Report(ByVal aUci As atcUCI.HspfUci,
                           ByVal aBalanceType As String,
                           ByVal aOperationTypes As atcCollection,
                           ByVal aScenario As String,
                           ByVal aScenarioResults As atcDataSource,
                           ByVal aOutputLocations As atcCollection,
                           ByVal aRunMade As String,
                           ByVal aSDateJ As Double,
                           ByVal aEDateJ As Double) As _
                           Tuple(Of atcReport.IReport, atcReport.IReport, atcReport.IReport, atcReport.IReport, atcReport.IReport, BoxWhiskerItem, atcCollection)

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
        Dim lReport As New atcReport.ReportText
        Dim lReport2 As New atcReport.ReportText
        Dim lReport3 As New atcReport.ReportText
        Dim lReport4 As New atcReport.ReportText
        Dim lReport5 As New atcReport.ReportText
        Dim lReport6 As New atcReport.ReportText
        Dim lReportLoadingRate As New atcReport.ReportText
        Dim lBoxWhiskerItems As New List(Of BoxWhiskerItem)
        Dim lDataForBoxWhiskerPlot As New BoxWhiskerItem

        Dim lDataForAllBarGraphs As New atcCollection

        pRunningTotals = New atcCollection
        pOutputConnectionArea = New atcCollection
        pOutputLoad = New atcCollection
        pPERLND = New atcCollection
        pIMPLND = New atcCollection
        pGENERLoadingExists = False
        pMessageShown = False

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
            Case "TotalN"
                lUnits = "lbs"

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
                lUnits = "lbs"

                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-P IN SOLUTION - INTERFLOW - OUTFLOW"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "PO4-P IN SOLUTION - GROUNDWATER - OUTFLOW"))
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SDP4A")) ' Outflow of sediment-associated PO4
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "SDORP")) ' Organic P Outflow (will be multiplied by 0.6)
                lNonpointData.Add(aScenarioResults.DataSets.FindData("Constituent", "ORGN - TOTAL OUTFLOW")) ' Organic N Outflow (will be multiplied by 0.05534793)
                'lNonpointData.AddRange((aScenarioResults.DataSets.FindData("Constituent", "POPHOS")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-ORTHO P")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "IOQUAL-ORTHO P")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "AOQUAL-ORTHO P")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-BOD")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "IOQUAL-BOD")))
                lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "AOQUAL-BOD")))
                lAtmDepData.Add((aScenarioResults.DataSets.FindData("Constituent", "PO4-ATMDEPTOT")))

                lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "P-TOT-IN"))
                lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "P-TOT-OUT"))

                'Case "BOD-PQUAL"
                '    lUnits = "(lbs)"
                '    lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "SOQUAL-BOD")))
                '    lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "IOQUAL-BOD")))
                '    lNonpointData.Add((aScenarioResults.DataSets.FindData("Constituent", "AOQUAL-BOD")))

                '    lTotalInflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "BODIN"))
                '    lOutflowData.Add(aScenarioResults.DataSets.FindData("Constituent", "BODOUTTOT"))

            Case Else
                lReport.AppendLine("Budget report not yet defined for balance type '" & aBalanceType & "'")
                lReport2.AppendLine("Budget report not yet defined for balance type '" & aBalanceType & "'")
                lReport3.AppendLine("Budget report not yet defined for balance type '" & aBalanceType & "'")
                lReport4.AppendLine("Budget report not yet defined for balance type '" & aBalanceType & "'")
                lReport5.AppendLine("Budget report not yet defined for balance type '" & aBalanceType & "'")
                lReport6.AppendLine("Budget report not yet defined for balance type '" & aBalanceType & "'")
                Return New Tuple(Of atcReport.IReport, atcReport.IReport, atcReport.IReport, atcReport.IReport, atcReport.IReport, BoxWhiskerItem, atcCollection)(lReport, lReport2, lReport3, lReport5, lReportLoadingRate, Nothing, Nothing)

        End Select

        Dim lUpstreamInflows As New atcCollection
        Dim lCumulativePointNonpointColl As New atcCollection


        lReport.AppendLine(aScenario & " " & "Average Annual " & aBalanceType & " Budget by Reach " & lUnits & ".")
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))


        lReport2.AppendLine(aScenario & " " & "Average Annual Nonpoint Loading of " & aBalanceType & " by Each Land Use to Each Reach " & lUnits & ".")
        lReport2.AppendLine("   Run Made " & aRunMade)
        lReport2.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport2.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))


        lReport3.AppendLine(aScenario & " " & " Average Annual Load Allocation of " & aBalanceType & " to Each Source by Reach " & lUnits & ".")
        lReport3.AppendLine("The Losses at Each Reach have been applied to the source, and the Gains are accumulated.")
        lReport3.AppendLine("   Run Made " & aRunMade)
        lReport3.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport3.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))

        lReport4.AppendLine("")
        lReport4.AppendLine("Percent of loadings of " & aBalanceType & " from each individual source to the Reaches (%).")

        lReport5.AppendLine(aScenario & " " & " Average Annual Load Allocation of " & aBalanceType & " to Each Source by Reaches of Interest " & lUnits & ".")
        lReport5.AppendLine("The Losses at Each Reach have been applied to the source, and the Gains are accumulated.")
        lReport5.AppendLine("   Run Made " & aRunMade)
        lReport5.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport5.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))

        lReport6.AppendLine("")
        lReport6.AppendLine("Percent of loadings of " & aBalanceType & " from each individual source to the Reaches of Interest(%).")

        lReportLoadingRate.AppendLine(aScenario & " " & " Average Annual " & aBalanceType & "Loading rates " & lUnits & "ac/yr.")

        lReportLoadingRate.AppendLine("   Run Made " & aRunMade)
        lReportLoadingRate.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReportLoadingRate.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
        lReportLoadingRate.AppendLine("")

        Dim YearsOfSimulation As Double = YearCount(aSDateJ, aEDateJ)
        Dim lOutputTable As New atcTableDelimited

        Select Case aBalanceType
            Case "Water"
                Dim lGENERInNetworkBlockMessageShown As Boolean = False
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & vbTab &
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


                    For Each lID As HspfOperation In lRchresOperations
                        .CurrentRecord += 1

                        Dim lPointVol As Double = 0.0
                        For Each lSource As HspfPointSource In lID.PointSources
                            If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IVOL" Then
                                Dim TimeSeriesTransformaton As String = lSource.Tran.ToString
                                Dim VolName As String = lSource.Source.VolName
                                Dim lDSN As Integer = lSource.Source.VolId
                                Dim lMfact As Double = lSource.MFact
                                For i As Integer = 0 To aUci.FilesBlock.Count
                                    Dim lPointSourceLoad As Double = 0.0
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
                                        lPointSourceLoad = ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation
                                        lPointSourceLoad *= MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                  TimeSeriesTransformaton, aUci.OpnSeqBlock.Delt)
                                        lPointVol += lPointSourceLoad

                                    End If
                                Next

                            End If
                        Next lSource
                        Dim lGENERLoad As Double = 0.0
                        For Each lSource As HspfConnection In lID.Sources
                            If lSource.Source.VolName = "GENER" AndAlso lSource.Target.Member = "IVOL" Then

                                Dim lGENERID As Integer = lSource.Source.VolId
                                Dim lMfact As Double = lSource.MFact
                                Dim lGENEROperationisOutputtoWDM As Boolean = False
                                Dim lGENEROperation As HspfOperation = lSource.Source.Opn
                                For Each EXTTarget As HspfConnection In lGENEROperation.Targets

                                    If EXTTarget.Target.VolName.Contains("WDM") Then
                                        lGENEROperationisOutputtoWDM = True
                                        Dim lWDMFile As String = EXTTarget.Target.VolName.ToString
                                        Dim lDSN As Integer = EXTTarget.Target.VolId

                                        For i As Integer = 0 To aUci.FilesBlock.Count
                                            Dim lGENERSourceLoad As Double = 0.0
                                            If aUci.FilesBlock.Value(i).Typ = lWDMFile Then
                                                Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
                                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                                If lDataSource Is Nothing Then
                                                    If atcDataManager.OpenDataSource(lFileName) Then
                                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                                    End If
                                                End If
                                                Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                                ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                                lGENERSourceLoad = ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation
                                                lGENERLoad += lGENERSourceLoad

                                            End If
                                        Next

                                    End If

                                Next EXTTarget
                                If Not lGENEROperationisOutputtoWDM AndAlso Not lGENERInNetworkBlockMessageShown Then
                                    Logger.Msg("GENER Loadings Issue: Some RCHRES operation have loadings input from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting. 
This message box will not be shown again for." & aBalanceType)
                                    lGENERInNetworkBlockMessageShown = True

                                End If

                            End If
                        Next lSource

                        Dim lUpstreamIn As Double = 0
                        If lUpstreamInflows.Keys.Contains(lID.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                        End If
                        Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                        Dim lNonpointVol As Double = 0.0
                        Dim lOutflow As Double = ValueForReach(lID, lOutflowData)
                        Dim lDownstreamReachID As Integer = lID.DownOper("RCHRES")
                        Dim lDiversion As Double = 0.0
                        If lID.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                            lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                        Else
                            Dim lExitNumber As Integer = 0
                            FindDownStreamExitNumber(aUci, lID, lExitNumber)
                            If lExitNumber = 0 Then
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            Else
                                Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lID.Id).FindData("Constituent", "OVOL-" & lExitNumber)(0)
                                lDiversion = -(lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual"))
                                lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            End If

                        End If

                        With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, 0.0, lPointVol, lGENERLoad, 0.0, 0.0,
                                                         lTotalInflow, lUpstreamIn, lDiversion, aSDateJ, aEDateJ)
                            lReport2.Append(.Item1)
                            lNonpointVol = .Item2
                            lGENERLoad = .Item3
                        End With

                        Dim lInflowFromPrecip As Double = ValueForReach(lID, lTotalPrecipData)


                        Dim lEvapLoss As Double = ValueForReach(lID, lEvapLossData)
                        Dim lAdditionalSource As Double = 0.0
                        lAdditionalSource = lTotalInflow - lNonpointVol - lUpstreamIn - lPointVol - lGENERLoad
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



                        lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                        lField = 0
                        lField += 1 : .Value(lField) = lID.Name & " " & lID.Id & " - " & lID.Description
                        lField += 1 : .Value(lField) = DoubleToString(lNonpointVol, , lNumberFormat,,, lNumberOfSignificantDigits)
                        lField += 1 : .Value(lField) = DoubleToString(lPointVol, , lNumberFormat,,, lNumberOfSignificantDigits)

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
                    lReport.Append(.ToString)
                End With

            Case "Sediment"
                Dim lGENERInNetworkBlockMessageShown As Boolean = False
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" &
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

                    For Each lID As HspfOperation In lRchresOperations
                        If lID.Tables("ACTIVITY").Parms("SEDFG").Value = 1 Then

                            Dim lPointTons As Double = 0.0
                            .CurrentRecord += 1
                            'If lID.Id = 610 Then Stop
                            For Each lSource As HspfPointSource In lID.PointSources
                                If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "ISED" Then
                                    Dim TimeSeriesTransformaton As String = lSource.Tran.ToString
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
                                            ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                            lPointTons += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation
                                            lPointTons *= MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                    TimeSeriesTransformaton, aUci.OpnSeqBlock.Delt)
                                        End If
                                    Next

                                End If
                            Next lSource

                            Dim lGENERLoad As Double = 0.0
                            For Each lSource As HspfConnection In lID.Sources
                                If lSource.Source.VolName = "GENER" AndAlso lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "ISED" Then

                                    Dim lGENERID As Integer = lSource.Source.VolId
                                    Dim lMfact As Double = lSource.MFact
                                    Dim lGENEROperationisOutputtoWDM As Boolean = False
                                    Dim lGENEROperation As HspfOperation = lSource.Source.Opn
                                    For Each EXTTarget As HspfConnection In lGENEROperation.Targets

                                        If EXTTarget.Target.VolName.Contains("WDM") Then
                                            lGENEROperationisOutputtoWDM = True
                                            Dim lWDMFile As String = EXTTarget.Target.VolName.ToString
                                            Dim lDSN As Integer = EXTTarget.Target.VolId

                                            For i As Integer = 0 To aUci.FilesBlock.Count

                                                If aUci.FilesBlock.Value(i).Typ = lWDMFile Then
                                                    Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
                                                    Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                                    If lDataSource Is Nothing Then
                                                        If atcDataManager.OpenDataSource(lFileName) Then
                                                            lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                                        End If
                                                    End If
                                                    Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                                    ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                                    lGENERLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation

                                                End If
                                            Next

                                        End If

                                    Next EXTTarget
                                    If Not lGENEROperationisOutputtoWDM AndAlso Not lGENERInNetworkBlockMessageShown Then
                                        Logger.Msg("GENER Loadings Issue: Some RCHRES operation have loadings input from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting. 
This message box will not be shown again for." & aBalanceType)
                                        lGENERInNetworkBlockMessageShown = True

                                    End If

                                End If
                            Next lSource

                            Dim lUpstreamIn As Double = 0
                            If lUpstreamInflows.Keys.Contains(lID.Id) Then
                                lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                            End If

                            Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)

                            Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                            Dim lDepScour As Double = ValueForReach(lID, lDepScourData) 'TotalForReach(lID, lAreas, lDepScourData)
                            Dim lNonpointTons As Double = 0.0

                            Dim lDownstreamReachID As Integer = lID.DownOper("RCHRES")
                            Dim lDiversion As Double = 0.0
                            If lID.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            Else
                                Dim lExitNumber As Integer = 0
                                FindDownStreamExitNumber(aUci, lID, lExitNumber)
                                If lExitNumber = 0 Then
                                    lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                Else
                                    Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lID.Id).FindData("Constituent", "OSED-TOT-EXIT" & lExitNumber)(0)
                                    lDiversion = -(lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual"))
                                    lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                                    lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                End If

                            End If


                            With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, 0.0, lPointTons, lGENERLoad, 0.0, -lDepScour, lTotalInflow, lUpstreamIn,
                                                         lDiversion, aSDateJ, aEDateJ)
                                lReport2.Append(.Item1)
                                lNonpointTons = .Item2
                                lGENERLoad = + .Item3
                                'When calculating losses and gains to the water columns from the be depth, deposition is the loss from the stream and 
                                'scour is the gain from the stream. Using this terminology for Load Allocation report makes it consistent for the Load Allocation Report.

                            End With
                            Dim lAdditionalSourceTons As Double = lTotalInflow - lNonpointTons - lUpstreamIn - lPointTons - lGENERLoad
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


                            Dim lCululativeTrappingEfficiency As Double = 0
                            Try
                                lCululativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                            Catch
                                lReachTrappingEfficiency = 0
                            End Try

                            lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                            lField = 0
                            lField += 1 : .Value(lField) = lID.Name & " " & lID.Id & " - " & lID.Description
                            lField += 1 : .Value(lField) = DoubleToString(lNonpointTons, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lPointTons, , lNumberFormat,,, lNumberOfSignificantDigits)
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
                            lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, , lNumberFormat, , , 6)
                            lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, , lNumberFormat,,, lNumberOfSignificantDigits)
                        End If
                    Next lID
                    lReport.Append(.ToString)
                End With

            Case "TotalN"
                Dim lGENERInNetworkBlockMessageShown As Boolean = False
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & vbTab &
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


                    For Each lID As HspfOperation In lRchresOperations
                        If lID.Tables("ACTIVITY").Parms("NUTFG").Value = 1 Then

                            Dim lPointlbs As Double = 0.0
                            .CurrentRecord += 1

                            Dim CVON As Double = ConversionFactorfromOxygen(aUci, aBalanceType, lID)
                            For Each lSource As HspfPointSource In lID.PointSources
                                If lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1) _
                                                                        Or (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 2) _
                                                                        Or (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 3) _
                                                                        Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 1 AndAlso lSource.Target.MemSub2 = 1) _
                                                                        Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 2 AndAlso lSource.Target.MemSub2 = 1) _
                                                                        Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 3 AndAlso lSource.Target.MemSub2 = 1) _
                                                                        Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 3) _
                                                                        Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 1) _
                                                                        Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 2) _
                                                                        Or (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then
                                    Dim TimeSeriesTransformaton As String = lSource.Tran.ToString
                                    Dim VolName As String = lSource.Source.VolName
                                    Dim lDSN As Integer = lSource.Source.VolId
                                    Dim lMfact As Double = lSource.MFact
                                    If lSource.Target.Member = "OXIF" Then lMfact *= CVON
                                    If lSource.Target.Member = "PKIF" And (lSource.Target.MemSub1 = 1 Or lSource.Target.MemSub1 = 2) Then lMfact *= ConversionFactorfromBiomass(aUci, aBalanceType, lID)
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
                                            lPointlbs += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation
                                            lPointlbs *= MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                                                                TimeSeriesTransformaton, aUci.OpnSeqBlock.Delt)
                                        End If
                                    Next
                                End If
                            Next

                            Dim lGENERLoad As Double = 0.0
                            For Each lSource As HspfConnection In lID.Sources
                                If lSource.Source.VolName = "GENER" AndAlso lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1) _
                                                                        Or (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 2) _
                                                                        Or (lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 3) _
                                                                        Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 1 AndAlso lSource.Target.MemSub2 = 1) _
                                                                        Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 2 AndAlso lSource.Target.MemSub2 = 1) _
                                                                        Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 3 AndAlso lSource.Target.MemSub2 = 1) _
                                                                        Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 3) _
                                                                        Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 1) _
                                                                        Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 2) _
                                                                        Or (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then

                                    Dim lGENERID As Integer = lSource.Source.VolId
                                    Dim lMfact As Double = lSource.MFact
                                    If lSource.Target.Member = "OXIF" Then lMfact *= CVON
                                    If lSource.Target.Member = "PKIF" And (lSource.Target.MemSub1 = 1 Or lSource.Target.MemSub1 = 2) Then lMfact *= ConversionFactorfromBiomass(aUci, aBalanceType, lID)
                                    Dim lGENEROperation As HspfOperation = lSource.Source.Opn
                                    Dim lGENEROperationisOutputtoWDM As Boolean = False
                                    For Each EXTTarget As HspfConnection In lGENEROperation.Targets

                                        If EXTTarget.Target.VolName.Contains("WDM") Then
                                            lGENEROperationisOutputtoWDM = True
                                            Dim lWDMFile As String = EXTTarget.Target.VolName.ToString
                                            Dim lDSN As Integer = EXTTarget.Target.VolId

                                            For i As Integer = 0 To aUci.FilesBlock.Count

                                                If aUci.FilesBlock.Value(i).Typ = lWDMFile Then
                                                    Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
                                                    Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                                    If lDataSource Is Nothing Then
                                                        If atcDataManager.OpenDataSource(lFileName) Then
                                                            lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                                        End If
                                                    End If
                                                    Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                                    ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                                    lGENERLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation

                                                End If
                                            Next

                                        End If

                                    Next EXTTarget
                                    If Not lGENEROperationisOutputtoWDM AndAlso Not lGENERInNetworkBlockMessageShown Then
                                        Logger.Msg("GENER Loadings Issue: Some RCHRES operation have loadings input from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting. 
This message box will not be shown again for." & aBalanceType)
                                        lGENERInNetworkBlockMessageShown = True


                                    End If



                                End If
                            Next lSource

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
                            Dim lDownstreamReachID As Integer = lID.DownOper("RCHRES")
                            Dim lDiversion As Double = 0.0
                            If lID.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                            Else
                                Dim lNumberOfExits As Integer = 0
                                FindDownStreamExitNumber(aUci, lID, lNumberOfExits)
                                If lNumberOfExits = 0 Then
                                    lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                Else
                                    Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lID.Id).FindData("Constituent", "N-TOT-OUT-EXIT" & lNumberOfExits)(0)
                                    lDiversion = -(lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual")) 'Diversion is a type of loss so it is calculated as -ve
                                    lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                                    lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                End If

                            End If



                            With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, CVON, lPointlbs, lGENERLoad, lTotalAtmDep, lDepScour, lTotalInflow, lUpstreamIn,
                                                         lDiversion, aSDateJ, aEDateJ)
                                lReport2.Append(.Item1)
                                lNonpointlbs = .Item2
                                lGENERLoad = .Item3
                            End With

                            lAdditionalSourcelbs = lTotalInflow - lNonpointlbs - lTotalAtmDep - lUpstreamIn - lPointlbs - lGENERLoad
                            'If lID.Id = 637 Then Stop
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


                            Dim lCululativeTrappingEfficiency As Double = 0
                            Try
                                lCululativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                            Catch
                                lReachTrappingEfficiency = 0
                            End Try


                            lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                            lField = 0
                            lField += 1 : .Value(lField) = lID.Name & " " & lID.Id & " - " & lID.Description
                            lField += 1 : .Value(lField) = DoubleToString(lNonpointlbs, 15, lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lPointlbs, 15, lNumberFormat,,, lNumberOfSignificantDigits)
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
                            lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, 15, lNumberFormat, , , 6)
                            lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, 15, lNumberFormat,,, lNumberOfSignificantDigits)

                        End If
                    Next lID

                    lReport.Append(.ToString)

                End With

                'Case "BOD-PQUAL"
                '    lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & vbTab & _
                '                        "Rate (lbs/ac)" & vbTab & "Total Load (lbs)")

                '    With lOutputTable
                '        .Delimiter = vbTab
                '        .NumFields = 12
                '        .NumRecords = lRchresOperations.Count + 1
                '        .CurrentRecord = 1
                '        Dim lField As Integer = 0
                '        lField += 1 : .FieldLength(lField) = 30 : .FieldType(lField) = "C" : .Value(lField) = "    " : .FieldName(lField) = "Reach Segment"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Nonpoint"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Point Sources"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Upstream In"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Total Inflow"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Outflow"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Gain(+)/Loss(-)"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Cumulative Total"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Cumulative Trapping"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = " (%)" : .FieldName(lField) = "Reach Trapping"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Diversion"
                '        lField += 1 : .FieldLength(lField) = 15 : .FieldType(lField) = "N" : .Value(lField) = lUnits : .FieldName(lField) = "Additional Sources"

                '        For Each lID As HspfOperation In lRchresOperations
                '            Dim lPointlbs As Double = 0.0
                '            .CurrentRecord += 1
                '            Dim CVOP As Double = ConversionFactorfromOxygen(aUci, aBalanceType, lID)

                '            For Each lSource As HspfPointSource In lID.PointSources
                '                If lSource.Target.Group = "INFLOW" AndAlso (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2) Then
                '                    Dim VolName As String = lSource.Source.VolName
                '                    Dim lDSN As Integer = lSource.Source.VolId
                '                    Dim lMfact As Double = lSource.MFact

                '                    For i As Integer = 0 To aUci.FilesBlock.Count
                '                        If aUci.FilesBlock.Value(i).Typ = VolName Then
                '                            Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
                '                            Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                '                            If lDataSource Is Nothing Then
                '                                If atcDataManager.OpenDataSource(lFileName) Then
                '                                    lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                '                                End If
                '                            End If
                '                            Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                '                            ltimeseries = SubsetByDate(ltimeseries, aUci.GlobalBlock.SDateJ, aUci.GlobalBlock.EdateJ, Nothing)
                '                            lPointlbs += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation

                '                        End If
                '                    Next
                '                End If
                '            Next
                '            Dim lNonpointlbs As Double
                '            Dim lUpstreamIn As Double = 0

                '            If lUpstreamInflows.Keys.Contains(lID.Id) Then
                '                lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                '            End If

                '            Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                '            Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)
                '            Dim lDepScour As Double = lOutflow - lTotalInflow
                '            Dim lTotalAtmDep As Double = 0
                '            If lAtmDepData.Count > 0 Then
                '                lTotalAtmDep = ValueForReach(lID, lAtmDepData)
                '            End If
                '            Dim lAdditionalSourcelbs As Double = 0

                '            'Following few lines calculate diversion if happening in the stream.
                '            Dim Diversion As Double = 0.0
                '            Dim lDownstreamReachID As Integer = lID.DownOper("RCHRES")
                '            If lID.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                '                lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                '            Else
                '                Dim lExitNumber As Integer = 0
                '                FindDownStreamExitNumber(aUci, lID, lExitNumber)
                '                If lExitNumber = 0 Then
                '                    lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                '                Else
                '                    Dim lExitOutFlow As atcTimeseries = aScenarioResults.DataSets.FindData("Location", "R:" & lID.Id).FindData("Constituent", "P-TOT-OUT-EXIT" & lExitNumber)(0)
                '                    Diversion = -(lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual"))
                '                    lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                '                    lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                '                End If
                '            End If



                '            With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, CVOP, lPointlbs, lTotalAtmDep, lDepScour, lTotalInflow, lUpstreamIn, Diversion)
                '                lReport2.Append(.Item1)
                '                lNonpointlbs = .Item2
                '            End With
                '            lAdditionalSourcelbs = lTotalInflow - lNonpointlbs - lUpstreamIn - lTotalAtmDep - lPointlbs

                '            Dim lCumulativePointNonpoint As Double = lNonpointlbs + lAdditionalSourcelbs
                '            If lCumulativePointNonpointColl.Keys.Contains(lID.Id) Then
                '                lCumulativePointNonpoint += lCumulativePointNonpointColl.ItemByKey(lID.Id)
                '            End If

                '            Dim lReachTrappingEfficiency As Double
                '            Try
                '                lReachTrappingEfficiency = lDepScour / lTotalInflow
                '            Catch
                '                lReachTrappingEfficiency = 0
                '            End Try

                '            Dim lCululativeTrappingEfficiency As Double = 0
                '            Try
                '                lCululativeTrappingEfficiency = 1 - (lOutflow / lCumulativePointNonpoint)
                '            Catch
                '                lReachTrappingEfficiency = 0
                '            End Try

                '            lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                '            lField = 0
                '            lField += 1 : .Value(lField) = lID.Name & " " & lID.Id & " - " & lID.Description
                '            lField += 1 : .Value(lField) = DoubleToString(lNonpointlbs, , lNumberFormat,,, lNumberOfSignificantDigits)
                '            lField += 1 : .Value(lField) = DoubleToString(lPointlbs, , lNumberFormat,,, lNumberOfSignificantDigits)
                '            lField += 1 : .Value(lField) = DoubleToString(lUpstreamIn, , lNumberFormat,,, lNumberOfSignificantDigits)
                '            lField += 1 : .Value(lField) = DoubleToString(lTotalInflow, , lNumberFormat,,, lNumberOfSignificantDigits)
                '            lField += 1 : .Value(lField) = DoubleToString(lOutflow, , lNumberFormat,,, lNumberOfSignificantDigits)
                '            lField += 1 : .Value(lField) = DoubleToString(lDepScour, , lNumberFormat,,, lNumberOfSignificantDigits)
                '            lField += 1 : .Value(lField) = DoubleToString(lCumulativePointNonpoint, , lNumberFormat,,, lNumberOfSignificantDigits)
                '            lField += 1 : .Value(lField) = DoubleToString(lCululativeTrappingEfficiency * 100, , lNumberFormat, , , 6)
                '            lField += 1 : .Value(lField) = DoubleToString(lReachTrappingEfficiency * 100, , lNumberFormat,,, lNumberOfSignificantDigits)
                '            lField += 1 : .Value(lField) = DoubleToString(Diversion, , lNumberFormat,,, lNumberOfSignificantDigits)
                '            lField += 1 : .Value(lField) = DoubleToString(lAdditionalSourcelbs, , lNumberFormat,,, lNumberOfSignificantDigits)
                '        Next
                '        lReport.Append(.ToString)
                '    End With
            Case "TotalP"
                Dim lGENERInNetworkBlockMessageShown As Boolean = False
                lReport2.AppendLine("Reach" & vbTab & "Nonpoint Source" & vbTab & "Area (ac)" & vbTab &
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


                    For Each lID As HspfOperation In lRchresOperations
                        If lID.Tables("ACTIVITY").Parms("NUTFG").Value = 1 Then
                            'If lID.Id = 815 Then Stop

                            Dim lPointlbs As Double = 0.0
                            .CurrentRecord += 1
                            Dim CVOP As Double = ConversionFactorfromOxygen(aUci, aBalanceType, lID)

                            For Each lSource As HspfPointSource In lID.PointSources
                                If lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 4) _
                                                                       Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 1 AndAlso lSource.Target.MemSub2 = 2) _
                                                                       Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 2 AndAlso lSource.Target.MemSub2 = 2) _
                                                                       Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 3 AndAlso lSource.Target.MemSub2 = 2) _
                                                                       Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 4) _
                                                                       Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 1) _
                                                                       Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 2) _
                                                                       Or (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then
                                    Dim TimeSeriesTransformaton As String = lSource.Tran.ToString
                                    Dim VolName As String = lSource.Source.VolName
                                    Dim lDSN As Integer = lSource.Source.VolId
                                    Dim lMfact As Double = lSource.MFact
                                    If lSource.Target.Member = "OXIF" Then lMfact *= CVOP
                                    If lSource.Target.Member = "PKIF" And (lSource.Target.MemSub1 = 1 Or lSource.Target.MemSub1 = 2) Then lMfact *= ConversionFactorfromBiomass(aUci, aBalanceType, lID)
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
                                            lPointlbs += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation
                                            lPointlbs *= MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                                                                TimeSeriesTransformaton, aUci.OpnSeqBlock.Delt)
                                        End If
                                    Next
                                End If
                            Next

                            Dim lGENERLoad As Double = 0.0
                            For Each lSource As HspfConnection In lID.Sources
                                If lSource.Source.VolName = "GENER" AndAlso lSource.Target.Group = "INFLOW" AndAlso ((lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 4) _
                                                                       Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 1 AndAlso lSource.Target.MemSub2 = 2) _
                                                                       Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 2 AndAlso lSource.Target.MemSub2 = 2) _
                                                                       Or (lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 3 AndAlso lSource.Target.MemSub2 = 2) _
                                                                       Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 4) _
                                                                       Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 1) _
                                                                       Or (lSource.Target.Member = "PKIF" AndAlso lSource.Target.MemSub1 = 2) _
                                                                       Or (lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2)) Then

                                    Dim lGENERID As Integer = lSource.Source.VolId
                                    Dim lMfact As Double = lSource.MFact
                                    If lSource.Target.Member = "OXIF" Then lMfact *= CVOP
                                    If lSource.Target.Member = "PKIF" And (lSource.Target.MemSub1 = 1 Or lSource.Target.MemSub1 = 2) Then lMfact *= ConversionFactorfromBiomass(aUci, aBalanceType, lID)
                                    Dim lGENEROperation As HspfOperation = lSource.Source.Opn
                                    Dim lGENEROperationisOutputtoWDM As Boolean = False
                                    For Each EXTTarget As HspfConnection In lGENEROperation.Targets

                                        If EXTTarget.Target.VolName.Contains("WDM") Then
                                            lGENEROperationisOutputtoWDM = True
                                            Dim lWDMFile As String = EXTTarget.Target.VolName.ToString
                                            Dim lDSN As Integer = EXTTarget.Target.VolId

                                            For i As Integer = 0 To aUci.FilesBlock.Count

                                                If aUci.FilesBlock.Value(i).Typ = lWDMFile Then
                                                    Dim lFileName As String = AbsolutePath(aUci.FilesBlock.Value(i).Name.Trim, CurDir())
                                                    Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                                    If lDataSource Is Nothing Then
                                                        If atcDataManager.OpenDataSource(lFileName) Then
                                                            lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                                        End If
                                                    End If
                                                    Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                                    ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                                    lGENERLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearsOfSimulation

                                                End If
                                            Next

                                        End If

                                    Next EXTTarget
                                    If Not lGENEROperationisOutputtoWDM AndAlso Not lGENERInNetworkBlockMessageShown Then
                                        Logger.Msg("GENER Loadings Issue: Some RCHRES operation have loadings input from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting. 
This message box will not be shown again for." & aBalanceType)
                                        lGENERInNetworkBlockMessageShown = True


                                    End If



                                End If
                            Next lSource




                            Dim lNonpointlbs As Double = 0.0
                            Dim lUpstreamIn As Double = 0.0

                            If lUpstreamInflows.Keys.Contains(lID.Id) Then
                                lUpstreamIn = lUpstreamInflows.ItemByKey(lID.Id)
                            End If

                            Dim lTotalInflow As Double = ValueForReach(lID, lTotalInflowData)
                            Dim lOutflow As Double = ValueForReach(lID, lOutflowData) 'TotalForReach(lID, lAreas, lOutflowData)

                            Dim lTotalAtmDep As Double = 0
                            If lAtmDepData.Count > 0 Then
                                lTotalAtmDep = ValueForReach(lID, lAtmDepData)
                            End If
                            Dim lAdditionalSourcelbs As Double = 0

                            'Following few lines calculate diversion if happening in the stream.
                            Dim lDiversion As Double = 0.0
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
                                    lDiversion = -(lOutflow - lExitOutFlow.Attributes.GetValue("SumAnnual"))
                                    lOutflow = lExitOutFlow.Attributes.GetValue("SumAnnual")
                                    lUpstreamInflows.Increment(lDownstreamReachID, lOutflow)
                                End If
                            End If


                            Dim lDepScour As Double = lOutflow - lTotalInflow - lDiversion
                            With ConstituentLoadingByLanduse(aUci, lID, aBalanceType, lNonpointData, CVOP, lPointlbs, lGENERLoad, lTotalAtmDep,
                                                         lDepScour, lTotalInflow, lUpstreamIn, lDiversion, aSDateJ, aEDateJ)
                                lReport2.Append(.Item1)
                                lNonpointlbs = .Item2
                                lGENERLoad = .Item3
                            End With
                            lAdditionalSourcelbs = lTotalInflow - lNonpointlbs - lUpstreamIn - lTotalAtmDep - lPointlbs - lGENERLoad

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

                            lCumulativePointNonpointColl.Increment(lDownstreamReachID, lCumulativePointNonpoint)

                            lField = 0
                            lField += 1 : .Value(lField) = lID.Name & " " & lID.Id & " - " & lID.Description
                            lField += 1 : .Value(lField) = DoubleToString(lNonpointlbs, , lNumberFormat,,, lNumberOfSignificantDigits)
                            lField += 1 : .Value(lField) = DoubleToString(lPointlbs, , lNumberFormat,,, lNumberOfSignificantDigits)
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

                    lReport.Append(.ToString)
                End With
        End Select

        If aBalanceType = "TotalN" Or aBalanceType = "TotalP" Or aBalanceType = "Sediment" Then 'Or aBalanceType = "BOD-PQUAL" Then
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


            Dim PointSourcesPlacesLocation As Integer = lLandusesHeader.IndexOf("PointSources")
            Dim LandUsesList() As String = lLandusesHeader.Substring(1, PointSourcesPlacesLocation - 2).Split(vbTab)


            Dim LoadingRateSummary As String = "Loading Rate Summary for " & aBalanceType & " by Each Land Land Use: " & lUnits & "/acre/yr" & vbCrLf

            lDataForBoxWhiskerPlot.Constituent = aBalanceType
            lDataForBoxWhiskerPlot.Scenario = aScenario
            lDataForBoxWhiskerPlot.TimeSpan = TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: ")
            lDataForBoxWhiskerPlot.Units = "(" & lUnits & "/acre/yr)"
            lDataForBoxWhiskerPlot.Location = "All"
            LoadingRateSummary &= "Landuse" & vbTab & "Mean" & vbTab & "Min" & vbTab & "Max" & vbCrLf
            For Each LandUse As String In LandUsesList
                Dim LoadingRateLine1 As String = LandUse & vbTab
                Dim LoadingRateLine2 As String = LandUse & vbTab
                LoadingRateSummary &= LandUse & vbTab

                Dim max As Double = 0.0
                Dim min As Double = 1.0E+30
                Dim sum As Double = 0.0
                Dim count As Int16 = 0

                Dim LoadingRateCollectionForEachLanduse As New List(Of Double)

                For Each Key As String In pOutputConnectionArea.Keys
                    Dim colonLocation As Integer = Key.IndexOf(":")
                    Dim LandUseFromKey As String = SafeSubstring(Key, 0, colonLocation - 1)
                    Dim OperationType As String = SafeSubstring(Key, colonLocation - 1, 1)
                    Dim OperationNumber As Integer = SafeSubstring(Key, colonLocation + 1, 3)
                    Dim loadingRate As Double = 0.0
                    If OperationType & ":" & LandUseFromKey = LandUse Then
                        count += 1
                        LoadingRateLine1 &= OperationType & ":" & OperationNumber & vbTab
                        loadingRate = pOutputLoad.ItemByKey(Key) / pOutputConnectionArea.ItemByKey(Key)
                        LoadingRateLine2 &= DoubleToString(loadingRate, , lNumberFormat,,, lNumberOfSignificantDigits) & vbTab

                        sum += loadingRate
                        If loadingRate > max Then
                            max = loadingRate
                        End If
                        If loadingRate < min Then
                            min = loadingRate
                        End If

                        LoadingRateCollectionForEachLanduse.Add(loadingRate)

                    End If
                Next Key
                If LoadingRateCollectionForEachLanduse.Count > 0 Then
                    lDataForBoxWhiskerPlot.LabelValueCollection.Add(LandUse, LoadingRateCollectionForEachLanduse.ToArray)
                End If

                LoadingRateSummary &= DoubleToString(sum / count, , lNumberFormat,,, lNumberOfSignificantDigits) & vbTab &
                    DoubleToString(min, , lNumberFormat,,, lNumberOfSignificantDigits) &
                    vbTab & DoubleToString(max, , lNumberFormat,,, lNumberOfSignificantDigits) & vbCrLf


                lReportLoadingRate.AppendLine(LoadingRateLine1)
                lReportLoadingRate.AppendLine(LoadingRateLine2)

            Next LandUse

            lReportLoadingRate.AppendLine()
            lReportLoadingRate.AppendLine()
            lReportLoadingRate.AppendLine(LoadingRateSummary)



            lReport3.AppendLine("Reach" & lLandusesHeader & vbTab & "Total**")
            lReport4.AppendLine("Reach" & lLandusesHeader & vbTab & "Total**")
            If aOutputLocations.Count > 0 Then
                lReport5.AppendLine("Reach" & lLandusesHeader & vbTab & "Total**")
                lReport6.AppendLine("Reach" & lLandusesHeader & vbTab & "Total**")
            Else
                lReport5.AppendLine("Load Allocation Report was not requested for specific reaches.  The Load Allocation Report")
                lReport5.AppendLine("for all the reaches is available as " & aBalanceType & "_" & aScenario & "_" & "LoadAllocation.txt")
            End If


            For Each lReach As String In lReaches
                Dim lDataForOneBarGraph As New BarGraphItem
                lDataForOneBarGraph.Scenario = aScenario
                lDataForOneBarGraph.TimeSpan = TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: ")
                lDataForOneBarGraph.Units = "(" & lUnits & "/yr)"
                lDataForOneBarGraph.Constituent = aBalanceType

                lReport3.Append(aUci.OpnBlks("RCHRES").OperFromID(lReach.Substring(5)).Caption.Substring(12))
                lReport4.Append(aUci.OpnBlks("RCHRES").OperFromID(lReach.Substring(5)).Caption.Substring(12))

                If aOutputLocations.Count > 0 Then
                    For Each lOutputLocation As String In aOutputLocations
                        If lReach.Substring(5) = lOutputLocation.Substring(2) Then

                            lReport5.Append(aUci.OpnBlks("RCHRES").OperFromID(lReach.Substring(5)).Caption.Substring(12))
                            lReport6.Append(aUci.OpnBlks("RCHRES").OperFromID(lReach.Substring(5)).Caption.Substring(12))
                        End If
                    Next

                End If

                Dim lTotal As Double = 0.0
                For Each lSourceDescription As String In lLandUses

                    Dim Key As String = lReach & " " & lSourceDescription
                    Dim lValue As Double = pRunningTotals.ItemByKey(Key)

                    lReport3.Append(vbTab & FormatNumber(lValue, 2, TriState.True, TriState.False, TriState.False))
                    If aOutputLocations.Count > 0 Then
                        For Each lOutputLocation As String In aOutputLocations
                            If lReach.Substring(5) = lOutputLocation.Substring(2) Then
                                lSourceDescription = lSourceDescription.Replace("Loss", "Cumulative Instream Losses")
                                lSourceDescription = lSourceDescription.Replace("Gain", "Cumulative Instream Gains")
                                lSourceDescription = lSourceDescription.Replace("AdditionalSources", "Additional Sources")
                                lSourceDescription = lSourceDescription.Replace("DirectAtmosphericDeposition", "Direct Atm. Desposition")
                                lSourceDescription = lSourceDescription.Replace("GENERSources", "GENER Sources")
                                lSourceDescription = lSourceDescription.Replace("PointSources", "Point Sources")

                                lReport5.Append(vbTab & FormatNumber(lValue, 2, TriState.True, TriState.False, TriState.False))
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
                    lDataForAllBarGraphs.Add(lReach, lDataForOneBarGraph)
                End If

                lReport3.AppendLine(vbTab & FormatNumber(lTotal, 2, TriState.True, TriState.False, TriState.False))
                If aOutputLocations.Count > 0 Then
                    For Each lOutputLocation As String In aOutputLocations
                        If lReach.Substring(5) = lOutputLocation.Substring(2) Then
                            lReport5.AppendLine(vbTab & FormatNumber(lTotal, 2, TriState.True, TriState.False, TriState.False))
                        End If
                    Next
                End If

                Dim lPercentTotal As Double = 0.0
                For Each lSourceDescription As String In lLandUses

                    Dim Key As String = lReach & " " & lSourceDescription
                    Dim lValue As Double = pRunningTotals.ItemByKey(Key)

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
            lReport3.AppendLine("*The additional sources may include sources other than non-point sources, point sources, atmospheric deposition, and upstream contribution.")
            lReport3.AppendLine("**The totals do not include losses as they have already been applied to the respective sources")
            lReport4.AppendLine("*The additional sources may include sources other than non-point sources, point sources, atmospheric deposition, and upstream contribution.")
            lReport4.AppendLine("**The totals do not include losses as they have already been applied to the respective sources")

            lReport5.AppendLine("*The additional sources may include sources other than non-point sources, point sources, atmospheric deposition, and upstream contribution.")
            lReport5.AppendLine("**The totals do not include losses as they have already been applied to the respective sources")
            lReport6.AppendLine("*The additional sources may include sources other than non-point sources, point sources, atmospheric deposition, and upstream contribution.")
            lReport6.AppendLine("**The totals do not include losses as they have already been applied to the respective sources")

            lReport3.Append(lReport4.ToString)
            lReport5.Append(lReport6.ToString)
            'The Total column in percent line needs to be recalculated.
        Else
            lReport3.AppendLine("Load Allocation Report are not produced for " & aBalanceType)
            lReport5.AppendLine("Load Allocation Report are not produced for " & aBalanceType)
        End If
        pRunningTotals.Clear()

        Return New Tuple(Of atcReport.IReport, atcReport.IReport, atcReport.IReport, atcReport.IReport, atcReport.IReport, BoxWhiskerItem, atcCollection)(lReport, lReport2, lReport3, lReport5, lReportLoadingRate, lDataForBoxWhiskerPlot, lDataForAllBarGraphs)

    End Function

    Private Function ValueForReach(ByVal aReach As HspfOperation,
                                   ByVal aReachData As atcTimeseriesGroup) As Double
        Dim lOutFlow As Double
        For Each aTimeseries As atcTimeseries In aReachData.FindData("Location", "R:" & aReach.Id)
            lOutFlow += aTimeseries.Attributes.GetValue("SumAnnual")
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
                        ByRef aContribPercent As atcCollection)

        Dim lTotalIndex As Integer = 0
        Dim lTotal As Double = 0

        For Each landUse As String In aLandUses
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
                    If lConnection.Source.Opn.Description = landUse Then
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

                            Dim lMassLinkFactor As Double = 0.0
                            If lTs.Attributes.GetValue("SumAnnual") > 0 Then
                                lMassLinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lTs.Attributes.GetValue("Constituent"), aBalanceType,
                                                                               aConversionFactor, aMultipleIndex:=0)
                            End If

                            lConstituentTotal = lTs.Attributes.GetValue("SumAnnual") * lMassLinkFactor * lConnectionArea
                            'lConstituentTotal = lConstituentRate * lConnectionArea

                            lTotal += lConstituentTotal
                            'The area and multiplication factor is multiplied to get the total load of the particular constituent to the reach "aReach" from the specific
                            'land use and then lTotal accumulates that load
                        Next

                        felu2(aUCI, aReach, aBalanceType, aVolName, aLandUses, aLoadingByLanduse, aReachTotal, aReporting, aContribPercent, lTotal, lConnectionArea, landUse, lTestLocation)

                    End If
                End If

            Next

            If Not lFound Then
                felu2(aUCI, aReach, aBalanceType, aVolName, aLandUses, aLoadingByLanduse, aReachTotal, aReporting, aContribPercent, 0, 0, landUse, "")
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
        Dim aTotal2 As Double = aTotal 'aTotal2 is used for reporting the loading from the local land area to the reach.
        Dim lKey As String = "Reach" & aReach.Id & " " & lVolPrefix & aLandUse
        If aReporting Then
            'If aReach.Id = 113 Then Stop
            Dim lPercentContrib As Double = 0.0
            Dim lNumberofTributaries As Integer = 0
            Dim lCheckedTributary As Boolean = False
            Dim UpstreamLoadByCategory As Double = 0.0
            If aContribPercent.Keys.Contains(lKey) Then
                lCheckedTributary = True
            Else
                For Each lTributary As HspfConnection In aReach.Sources

                    If lTributary.Source.VolName = "RCHRES" Then
                        Dim lTributaryID As Integer = lTributary.Source.VolId
                        lNumberofTributaries += 1
                        UpstreamLoadByCategory += pRunningTotals.ItemByKey("Reach" & lTributaryID & " " & lVolPrefix & aLandUse)
                    End If
                Next

            End If
            aTotal += UpstreamLoadByCategory
            lPercentContrib = aTotal * 100 / aReachTotal

            aContribPercent.Increment(lKey, lPercentContrib)

            'If aConnectionArea > 0 Then
            Dim lrate As Double = aTotal2 / aConnectionArea
            If aConnectionArea = 1.0E-30 Then
                aConnectionArea = 0
                aTotal2 = 0
            End If
            If aTestLocation.Contains(":") Then
                pOutputConnectionArea.Increment(aLandUse & aTestLocation, aConnectionArea)
                pOutputLoad.Increment(aLandUse & aTestLocation, aTotal2)

                aLoadingByLanduse &= aReach.Caption.ToString.Substring(10) & vbTab _
            & aTestLocation & " " _
            & aLandUse & vbTab _
            & aConnectionArea & vbTab _
            & DoubleToString(lrate, 15, "#,##0.###") & vbTab &
                                DoubleToString(aTotal2, 15, "#,##0.###") & vbTab & vbCrLf
            End If
        Else
            'If aReach.Id = 260 Then Stop
            Dim checkedTheTributary As Boolean = False

            If pRunningTotals.Keys.Contains(lKey) Then checkedTheTributary = True
            pRunningTotals.Increment(lKey, aTotal)
            'A key of the land use type and downstream reach is made in pRunningTotals to save the total land from the specific land use.
            aReachTotal += aTotal

            If Not checkedTheTributary Then
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

    Private Function ConstituentLoadingByLanduse(ByVal aUCI As HspfUci,
                                                 ByVal aReach As HspfOperation,
                                                 ByVal aBalanceType As String,
                                                 ByVal aNonpointData As atcTimeseriesGroup,
                                                 ByVal aConversionFactor As Double,
                                                 ByVal aPoint As Double,
                                                 ByRef aGENERLoad As Double,
                                                 ByVal aAtmDep As Double,
                                                 ByVal GainLoss As Double,
                                                 ByVal aTotalInflow As Double,
                                                 ByVal aUpstreamIn As Double,
                                                 ByVal aDiversion As Double,
                                                 ByVal aSDateJ As Double,
                                                 ByVal aEDateJ As Double) As Tuple(Of String, Double, Double)
        Dim LoadingByLanduse As String = ""
        Dim lReachTotal As Double = 0.0
        Dim UpStreamDiversion As Double = 0.0
        Dim lContribPercent As New atcCollection
        'If aReach.Id = "148" Then Stop
        felu(aUCI, aReach, aBalanceType, "PERLND", pPERLND, aNonpointData, aConversionFactor, LoadingByLanduse, lReachTotal, False, lContribPercent)
        felu(aUCI, aReach, aBalanceType, "IMPLND", pIMPLND, aNonpointData, aConversionFactor, LoadingByLanduse, lReachTotal, False, lContribPercent)

        For Each lTributary As HspfConnection In aReach.Sources
            If lTributary.Source.VolName = "RCHRES" Then
                Dim lTributaryId As String = lTributary.Source.VolId
                UpStreamDiversion += pRunningTotals.ItemByKey("Reach" & lTributaryId & " Diversion")
            End If
        Next
        aTotalInflow += -UpStreamDiversion
        'If aReach.Id = 102 Then Stop
        felu(aUCI, aReach, aBalanceType, "PERLND", pPERLND, aNonpointData, aConversionFactor, LoadingByLanduse, aTotalInflow, True, lContribPercent)
        felu(aUCI, aReach, aBalanceType, "IMPLND", pIMPLND, aNonpointData, aConversionFactor, LoadingByLanduse, aTotalInflow, True, lContribPercent)
        'If aReach.Id = 102 Then Stop

        Dim GENERTSinWDM As Boolean = False
        Dim SingleGENERLoad As Double = 0.0
        Dim TotalGENERLoad As Double = 0.0
        Dim lGENERLoadingExists As Boolean = False

        For Each GENERConnection As HspfConnection In aReach.Sources
            If GENERConnection.Source.VolName = "GENER" Then
                Dim GENERID As String = GENERConnection.Source.VolId
                Dim lSchematicMFact As Double = GENERConnection.MFact
                Dim lMassLinkID As Integer = GENERConnection.MassLink
                If Not aUCI.OperationExists("GENER", GENERID) Or lMassLinkID = 0 Then Continue For
                lGENERLoadingExists = True

                For Each EXTTarget As HspfConnection In aUCI.Connections
                    If EXTTarget.Source.VolName = "GENER" AndAlso EXTTarget.Source.VolId = GENERID AndAlso EXTTarget.Target.VolName.Contains("WDM") Then
                        'The GENER timeseries is available as a WDM timeseries
                        GENERTSinWDM = True
                        Dim VolName As String = EXTTarget.Target.VolName
                        Dim lDSN As Integer = EXTTarget.Target.VolId
                        Dim lMfact As Double = EXTTarget.MFact
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = VolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                SingleGENERLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearCount(aSDateJ, aEDateJ)
                                Exit For
                            End If
                        Next

                    End If

                Next

                SingleGENERLoad *= FindMassLinkFactor(aUCI, lMassLinkID, "GENER", aBalanceType, aConversionFactor, aMultipleIndex:=0)
                SingleGENERLoad *= lSchematicMFact

            End If
            TotalGENERLoad += SingleGENERLoad
            SingleGENERLoad = 0.0

        Next GENERConnection


        aGENERLoad += TotalGENERLoad
        Dim lAdditionalSource As Double = aTotalInflow - lReachTotal - aAtmDep - aPoint - aUpstreamIn - aGENERLoad + UpStreamDiversion
        If lGENERLoadingExists AndAlso (Not GENERTSinWDM) AndAlso (Not pMessageShown) Then
            Logger.Msg("GENER Loadings Issue: Some RCHRES operation have loadings input from GENER connections. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting. 
This message box will not be shown again for." & aBalanceType)
            pMessageShown = True
        End If

        For Each lTributary As HspfConnection In aReach.Sources
            If lTributary.Source.VolName = "RCHRES" Then
                Dim lTributaryId As String = lTributary.Source.VolId
                aPoint += pRunningTotals.ItemByKey("Reach" & lTributaryId & " PointSources")
                lAdditionalSource += pRunningTotals.ItemByKey("Reach" & lTributaryId & " AdditionalSources")
                aAtmDep += pRunningTotals.ItemByKey("Reach" & lTributaryId & " DirectAtmosphericDeposition")
                aDiversion += pRunningTotals.ItemByKey("Reach" & lTributaryId & " Diversion")
                If pGENERLoadingExists Then
                    aGENERLoad += pRunningTotals.ItemByKey("Reach" & lTributaryId & " GENERSources")
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
                    Dim ReachIDLength As Integer = aReach.Id.ToString.Length
                    If lTotalsKey.Contains("Reach" & aReach.Id & " ") AndAlso lTotalsKey.Contains(SafeSubstring(lKey, 6 + ReachIDLength)) Then
                        pRunningTotals.ItemByKey(lTotalsKey) = pRunningTotals.ItemByKey(lTotalsKey) + lContribPercent.ItemByKey(lKey) * GainLoss / 100
                        Exit For
                    End If

                Next lTotalsKey
            Next lKey

        End If

        Return New Tuple(Of String, Double, Double)(LoadingByLanduse, lReachTotal, TotalGENERLoad)
    End Function

    Private Function FindDownStreamExitNumber(ByVal aUCI As HspfUci,
                                              ByVal aReachID As HspfOperation,
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
    Private Function MultiFactorForPointSource(ByVal aTStep As Integer, ByVal aTimeUnit As String, ByVal aTransformation As String,
                                               ByVal aDelta As atcTimeUnit) As Double
        Dim MultiFactor As Double = 0.0
        If Trim(aTransformation) = "DIV" Then
            MultiFactor = 1.0
        Else
            Select Case aTransformation
                Case "SAME"
                    If aDelta / 60 = 1 AndAlso aTimeUnit = "TUDay" AndAlso aTStep = 1 Then
                        MultiFactor = 24.0
                    End If


            End Select



        End If






        Return MultiFactor
    End Function

End Module
