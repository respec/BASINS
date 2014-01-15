Imports atcUtility
Imports atcData
Imports atcUCI
Imports MapWinUtility

Public Module atcHspfAGCHEM
    'Public Sub ReportsToFiles(ByVal aUci As atcUCI.HspfUci, _
    '                          ByVal aOperations As atcCollection, _
    '                          ByVal aBalanceTypes As atcCollection, _
    '                          ByVal aScenario As String, _
    '                          ByVal aScenarioResults As atcTimeseriesSource, _
    '                          ByVal aLocations As atcCollection, _
    '                          ByVal aRunMade As String, _
    '                 Optional ByVal aDateColumns As Boolean = False, _
    '                 Optional ByVal aDecimalPlaces As Integer = 3, _
    '                 Optional ByVal aSignificantDigits As Integer = 5, _
    '                 Optional ByVal aFieldWidth As Integer = 12)

    '    For Each lBalanceType As String In aBalanceTypes
    '        Dim lReport As atcReport.ReportText = Report(aUci, lBalanceType, _
    '                                                     aScenario, aScenarioResults, aLocations, _
    '                                                     aRunMade, _
    '                                                     aDateColumns, aDecimalPlaces, aSignificantDigits, aFieldWidth)
    '        Dim lOutFileName As String = aScenario & "_" & lBalanceType & "_Balance.txt"
    '        Logger.Dbg("  WriteReportTo " & lOutFileName)
    '        SaveFileString(lOutFileName, lReport.ToString)
    '    Next lBalanceType
    'End Sub

    Private Function AllQuantity() As atcCollection
        Dim lAll As New atcCollection()
        With lAll
            .Add("P:SUPY", "Rainfall (in)")
            .Add("P:Header1", "Runoff (in)")
            .Add("P:SURO", "    Surface")
            .Add("P:IFWO", "    Interflow")
            .Add("P:AGWO", "    Baseflow")
            .Add("P:PERO", "    Total")
            .Add("P:SOSED", "Sediment Loss (t/a)")
            .Add("P:Header2", "Nutrient Loss (lb/a)")
            .Add("P:Header2a", "  NO3 Loss")
            .Add("P:NO3+NO2-N - SURFACE LAYER OUTFLOW", "    Surface")
            .Add("P:NO3+NO2-N - UPPER LAYER OUTFLOW", "    Interflow")
            .Add("P:NO3+NO2-N - GROUNDWATER OUTFLOW", "    Baseflow")
            .Add("P:NO3-N - TOTAL OUTFLOW", "    Total")
            .Add("P:Header2b", "  NH3 Loss")
            .Add("P:NH4-N IN SOLUTION - SURFACE LAYER OUTFLOW", "    Surface")
            .Add("P:NH4-N IN SOLUTION - UPPER LAYER OUTFLOW", "    Interflow")
            .Add("P:NH4-N IN SOLUTION - GROUNDWATER OUTFLOW", "    Baseflow")
            .Add("P:NH4-N ADS - SEDIMENT ASSOC OUTFLOW", "    Sediment")
            .Add("P:NH4-N - TOTAL OUTFLOW", "    Total")
            .Add("P:Header2c", "  Labile ORGN Loss")
            .Add("P:LABILE ORGN - SURFACE LAYER OUTFLOW", "    Surface")
            .Add("P:LABILE ORGN - UPPER LAYER OUTFLOW", "    Interflow")
            .Add("P:LABILE ORGN - GROUNDWATER OUTFLOW", "    Baseflow")
            .Add("P:LABILE ORGN - SEDIMENT ASSOC OUTFLOW", "    Sediment")
            .Add("P:Header2d", "  Refractory ORGN Loss")
            .Add("P:REFRAC ORGN - SURFACE LAYER OUTFLOW", "    Surface")
            .Add("P:REFRAC ORGN - UPPER LAYER OUTFLOW", "    Interflow")
            .Add("P:REFRAC ORGN - GROUNDWATER OUTFLOW", "    Baseflow")
            .Add("P:REFRAC ORGN - SEDIMENT ASSOC OUTFLOW", "    Sediment")
            .Add("P:ORGN - TOTAL OUTFLOW", "  Total ORGN Loss")
            .Add("P:NITROGEN - TOTAL OUTFLOW", "  Total N Loss")
            .Add("P:Header2e", "  PO4 Loss")
            .Add("P:PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW", "    Surface")
            .Add("P:PO4-P IN SOLUTION - INTERFLOW - OUTFLOW", "    Interflow")
            .Add("P:PO4-P IN SOLUTION - GROUNDWATER - OUTFLOW", "    Baseflow")
            .Add("P:SDP4A", "    Sediment")
            .Add("P:POPO4", "    Total")
            .Add("P:SDORP", "  ORGP Sediment")
            .Add("P:POPHOS", "  Total P Loss")
        End With
        Return lAll
    End Function

    Private Function AllStorages() As atcCollection
        Dim lAll As New atcCollection()
        With lAll
            .Add("P:Header0", "STORAGES (lb/ac)")
            .Add("P:ABOVE-GROUND PLANT STORAGE", "Above Ground Plant N")
            .Add("P:LITTER STORAGE", "Litter N")
            .Add("P:Header1", "Below Ground Plant N")
            .Add("P:PLANT N - SURFACE LAYER STORAGE", "    Surface")
            .Add("P:PLANT N - UPPER PRINCIPAL STORAGE", "    Upper")
            .Add("P:PLANT N - LOWER LAYER STORAGE", "    Lower")
            .Add("P:PLANT N - ACTIVE GROUNDWATER STORAGE", "    GW")
            .Add("P:PLANT N - TOTAL STORAGE", "Total AG, BG, Litter PLTN")
            .Add("P:TOTAL N - TOTAL STORAGE", "Total Soil, Litter, & Plant N")
            .Add("P:Header2", "NH4-N Soln Storage")
            .Add("P:NH4-N SOL - SURFACE LAYER STORAGE", "    Surface")
            .Add("P:NH4-N SOL - UPPER PRINCIPAL STORAGE", "    Upper")
            .Add("P:NH4-N SOL - UPPER TRANSITORY STORAGE", "    Interflow")
            .Add("P:NH4-N SOL - LOWER LAYER STORAGE", "    Lower")
            .Add("P:NH4-N SOL - ACTIVE GROUNDWATER STORAGE", "    GW")
            .Add("P:NH4-N SOL - TOTAL STORAGE", "    Total")
            .Add("P:Header3", "NH4-N Ads Storage")
            .Add("P:NH4-N ADS - SURFACE LAYER STORAGE", "    Surface")
            .Add("P:NH4-N ADS - UPPER PRINCIPAL STORAGE", "    Upper")
            .Add("P:NH4-N ADS - LOWER LAYER STORAGE", "    Lower")
            .Add("P:NH4-N ADS - ACTIVE GROUNDWATER STORAGE", "    GW")
            .Add("P:NH4-N ADS - TOTAL STORAGE", "    Total")
            .Add("P:Header4", "NO3/2-N Storage")
            .Add("P:NO3/2-N - SURFACE LAYER STORAGE", "    Surface")
            .Add("P:NO3/2-N - UPPER PRINCIPAL STORAGE", "    Upper")
            .Add("P:NO3/2-N - UPPER TRANSITORY STORAGE", "    Interflow")
            .Add("P:NO3/2-N - LOWER LAYER STORAGE", "    Lower")
            .Add("P:NO3/2-N - ACTIVE GROUNDWATER STORAGE", "    GW")
            .Add("P:NO3/2-N - TOTAL STORAGE", "    Total")
            .Add("P:Header5", "Labile ORGN Soln")
            .Add("P:SOL LABIL ORGANIC N - SURFACE LAYER STORAGE", "    Surface")
            .Add("P:SOL LABIL ORGANIC N - UPPER PRINCIPAL STORAGE", "    Upper")
            .Add("P:SOL LABIL ORGANIC N - UPPER TRANSITORY STORAGE", "    Interflow")
            .Add("P:SOL LABIL ORGANIC N - LOWER LAYER STORAGE", "    Lower")
            .Add("P:SOL LABIL ORGANIC N - ACTIVE GROUNDWATER STORAGE", "    GW")
            .Add("P:SOL LABIL ORGANIC N - TOTAL STORAGE", "    Total")
            .Add("P:Header6", "Labile ORGN Ads")
            .Add("P:ADS LABIL ORGANIC N - SURFACE LAYER STORAGE", "    Surface")
            .Add("P:ADS LABIL ORGANIC N - UPPER PRINCIPAL STORAGE", "    Upper")
            .Add("P:ADS LABIL ORGANIC N - LOWER LAYER STORAGE", "    Lower")
            .Add("P:ADS LABIL ORGANIC N - ACTIVE GROUNDWATER STORAGE", "    GW")
            .Add("P:ADS LABIL ORGANIC N - TOTAL STORAGE", "    Total")
            .Add("P:Header7", "Refractory ORGN Soln")
            .Add("P:SOL REFR ORGANIC N - SURFACE LAYER STORAGE", "    Surface")
            .Add("P:SOL REFR ORGANIC N - UPPER PRINCIPAL STORAGE", "    Upper")
            .Add("P:SOL REFR ORGANIC N - UPPER TRANSITORY STORAGE", "    Interflow")
            .Add("P:SOL REFR ORGANIC N - LOWER LAYER STORAGE", "    Lower")
            .Add("P:SOL REFR ORGANIC N - ACTIVE GROUNDWATER STORAGE", "    GW")
            .Add("P:SOL REFR ORGANIC N - TOTAL STORAGE", "    Total")
            .Add("P:Header8", "Refractory ORGN Ads")
            .Add("P:ADS REFR ORGANIC N - SURFACE LAYER STORAGE", "    Surface")
            .Add("P:ADS REFR ORGANIC N - UPPER PRINCIPAL STORAGE", "    Upper")
            .Add("P:ADS REFR ORGANIC N - LOWER LAYER STORAGE", "    Lower")
            .Add("P:ADS REFR ORGANIC N - ACTIVE GROUNDWATER STORAGE", "    GW")
            .Add("P:ADS REFR ORGANIC N - TOTAL STORAGE", "    Total")
            .Add("P:Header9", "Plant P")
            .Add("P:PLANT P - SURFACE LAYER", "    Surface")
            .Add("P:PLANT P - UPPER PRINCIPAL", "    Upper")
            .Add("P:PLANT P - LOWER LAYER", "    Lower")
            .Add("P:PLANT P - ACTIVE GROUNDWATER", "    GW")
            .Add("P:PLANT P - TOTALS", "    Total")
            .Add("P:Header10", "PO4-P Soln Storage")
            .Add("P:PO4-P SOL - SURFACE LAYER", "    Surface")
            .Add("P:PO4-P SOL - UPPER PRINCIPAL", "    Upper")
            .Add("P:PO4-P SOL - UPPER TRANSITORY", "    Interflow")
            .Add("P:PO4-P SOL - LOWER LAYER", "    Lower")
            .Add("P:PO4-P SOL - ACTIVE GROUNDWATER", "    GW")
            .Add("P:PO4-P SOL - TOTALS", "    Total")
            .Add("P:Header11", "PO4-P Ads Storage")
            .Add("P:PO4-P ADS - SURFACE LAYER", "    Surface")
            .Add("P:PO4-P ADS - UPPER PRINCIPAL", "    Upper")
            .Add("P:PO4-P ADS - LOWER LAYER", "    Lower")
            .Add("P:PO4-P ADS - ACTIVE GROUNDWATER", "    GW")
            .Add("P:PO4-P ADS - TOTALS", "    Total")
            .Add("P:Header12", "ORGP Storage")
            .Add("P:ORGANIC P - SURFACE LAYER", "    Surface")
            .Add("P:ORGANIC P - UPPER PRINCIPAL", "    Upper")
            .Add("P:ORGANIC P - LOWER LAYER", "    Lower")
            .Add("P:ORGANIC P - ACTIVE GROUNDWATER", "    GW")
            .Add("P:ORGANIC P - TOTALS", "    Total")
        End With
        Return lAll
    End Function

    Private Function AllFluxes() As atcCollection
        Dim lAll As New atcCollection()
        With lAll
            .Add("P:Header0", "N FLUXES")
            .Add("P:Header1", "Atmospheric Deposition (lb/a)")
            .Add("P:NH4-N - SURFACE LAYER - TOTAL AD", "    NH3-N - Surface")
            .Add("P:NO3-N - SURFACE LAYER - TOTAL AD", "    NO3-N - Surface")
            .Add("P:ORGN - SURFACE LAYER - TOTAL AD", "    ORGN - Surface")
            .Add("P:NH4-N - UPPER LAYER - TOTAL AD", "    NH3-N - Upper")
            .Add("P:NO3-N - UPPER LAYER - TOTAL AD", "    NO3-N - Upper")
            .Add("P:ORGN - UPPER LAYER - TOTAL AD", "    ORGN - Upper")
            '.Add("P:add these up to get a total of each species ?", "")
            .Add("P:Header2", "Applications (lb/a)")
            ' note there is an error in the hspf code here - NH3 and NO3 are switched, so I am correcting it here until the code gets fixed in hspf
            .Add("P:AMMONIA APPLICATION", "    NO3-N")
            .Add("P:NITRATE APPLICATION", "    NH3-N")
            '.Add("P:there is an error here - in the HSPF code NH3 and NO3 applic amts are mixed up", "")
            .Add("P:ORGANIC N APPLICATION", "    ORGN")
            .Add("P:Header3", "Above Ground Plant Uptake")
            .Add("P:TAMUPA", "    NH3-N")
            .Add("P:TNIUPA", "    NO3-N")
            .Add("P:Header4", "Below Ground Plant Uptake")
            .Add("P:TAMUPB", "    NH3-N")
            .Add("P:TNIUPB", "    NO3-N")
            .Add("P:RETAGN", "Above Gr Plant N to Litter")
            .Add("P:Header5", "Litter N Return to Labile ORGN")
            .Add("P:SRTLLN", "    Surface")
            .Add("P:URTLLN", "    Upper")
            .Add("P:TRTLLN", "    Total")
            .Add("P:Header6", "Litter N Return to Refractory ORGN")
            .Add("P:SRTRLN", "    Surface")
            .Add("P:URTRLN", "    Upper")
            .Add("P:TRTRLN", "    Total")
            .Add("P:Header7", "BG Plant N Return to Labile ORGN")
            .Add("P:SRTLBN", "    Surface")
            .Add("P:URTLBN", "    Upper")
            .Add("P:LRTLBN", "    Lower")
            .Add("P:TRTLBN", "    Total")
            .Add("P:Header8", "BG Plant N Return to Refractory ORGN")
            .Add("P:SRTRBN", "    Surface")
            .Add("P:URTRBN", "    Upper")
            .Add("P:LRTRBN", "    Lower")
            .Add("P:TRTRBN", "    Total")
            .Add("P:Header9", "Other fluxes (lb/a)")
            .Add("P:TREFON", "    Labile/Refractory ORGN Conversion")
            .Add("P:TORNMN", "    Labile ORGN Mineralization")
            .Add("P:TDENI", "    Denitrification")
            .Add("P:TAMNIT", "    NH3 Nitrification")
            .Add("P:TAMIMB", "    NH3 Immobilization")
            .Add("P:TNIIMB", "    NO3 Immobilization")
            .Add("P:Header10", "P FLUXES")
            .Add("P:Header11", "Atmospheric Deposition (lb/a)")
            .Add("P:PO4-P - SURFACELAYER - TOTAL", "    PO4-P - Surface")
            .Add("P:ORG-P - SURFACELAYER -TOTAL", "    ORGP - Surface")
            .Add("P:PO4-P - UPPER LAYER - TOTAL", "    PO4-P - Upper")
            .Add("P:ORG-P - UPPER LAYER - TOTAL", "    ORGP - Upper")
            '.Add("P:add these up to get a total of each species ?", "")
            .Add("P:Header12", "Applications (lb/a)")
            .Add("P:IPO4", "    PO4-P")
            .Add("P:IORP", "    ORGP")
            .Add("P:Header13", "Plant Uptake")
            .Add("P:PLANT P - SURFACE LAYER", "    Surface")
            .Add("P:PLANT P - UPPER PRINCIPAL", "    Upper")
            .Add("P:PLANT P - LOWER LAYER", "    Lower")
            .Add("P:PLANT P - ACTIVE GROUNDWATER", "    GW")
            .Add("P:PLANT P - TOTALS", "    Total")
            .Add("P:Header14", "Other fluxes (lb/a)")
            .Add("P:TORPMN", "    ORGP Mineralization")
            .Add("P:TP4IMB", "    PO4-P Immobilization")
        End With
        Return lAll
    End Function

    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcTimeseriesSource, _
                           ByVal aLocations As atcCollection, _
                           ByVal aRunMade As String, _
                  Optional ByVal aDateRows As Boolean = False, _
                  Optional ByVal aDecimalPlaces As Integer = 3, _
                  Optional ByVal aSignificantDigits As Integer = 5, _
                  Optional ByVal aFieldWidth As Integer = 12) As atcReport.IReport

        Dim lReport As New atcReport.ReportText
        lReport.AppendLine("AGCHEM Simulation Results For " & aScenario)
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunPeriod)
        lReport.AppendLine(vbCrLf)

        Dim aOperationTypes As New atcCollection
        aOperationTypes.Add("P:", "PERLND")
        'aOperationTypes.Add("I:", "IMPLND")
        'aOperationTypes.Add("R:", "RCHRES")

        For Each lOperationKey As String In aOperationTypes.Keys
            Dim lConstLists() As atcCollection = {AllQuantity(), AllStorages(), AllFluxes()}
            For Each lConstituentsToOutput As atcCollection In lConstLists
                For Each lLocation As String In aLocations
                    If lLocation.StartsWith(lOperationKey) Then
                        'Logger.Dbg(aOperations(lOperationIndex) & " " & lLocation)
                        Dim lLocationDataGroup As atcTimeseriesGroup = aScenarioResults.DataSets.FindData("Location", lLocation)
                        'Logger.Dbg("     MatchingDatasetCount " & lTempDataGroup.Count)
                        Dim lNeedHeader As Boolean = True
                        Dim lPendingOutput As String = ""

                        Try
                            Dim lOutputTable As New atcTableDelimited
                            lOutputTable.TrimValues = False
                            With lOutputTable
                                For Each lConstituentKey As String In lConstituentsToOutput.Keys
                                    If lConstituentKey.StartsWith(lOperationKey) Then
                                        Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                                        Dim lMultipleIndex As Integer = 0
                                        If Not lConstituentKey.ToLower.Contains("header") AndAlso Not lConstituentKey.ToLower.Contains("total") Then
                                            If lConstituentKey.EndsWith("1") Or lConstituentKey.EndsWith("2") Then
                                                lMultipleIndex = lConstituentKey.Substring(lConstituentKey.Length - 1)
                                                lConstituentKey = lConstituentKey.Substring(0, lConstituentKey.Length - 1)
                                            End If
                                        End If
                                        lConstituentKey = lConstituentKey.Remove(0, 2)
                                        Dim lConstituentDataGroup As atcTimeseriesGroup = lLocationDataGroup.FindData("Constituent", lConstituentKey)
                                        If lConstituentDataGroup.Count > 0 Then
                                            Dim lTempDataSet As atcDataSet = SubsetByDate(lConstituentDataGroup.Item(0), aUci.GlobalBlock.SDateJ, aUci.GlobalBlock.EdateJ, Nothing)
                                            Dim lSeasons As atcSeasonBase
                                            If aUci.GlobalBlock.SDate(1) = 10 Then 'month Oct
                                                lSeasons = New atcSeasonsWaterYear
                                            Else
                                                lSeasons = New atcSeasonsCalendarYear
                                            End If
                                            Dim lSeasonalAttributes As New atcDataAttributes
                                            If ConstituentsThatUseLast.Contains(lConstituentKey) Then
                                                lSeasonalAttributes.SetValue("Last", 0) 'fluxes are last from daily, monthly or annual to annual
                                            Else
                                                lSeasonalAttributes.SetValue("Sum", 0) 'fluxes are summed from daily, monthly or annual to annual
                                            End If
                                            Dim lYearlyAttributes As New atcDataAttributes
                                            lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lYearlyAttributes)

                                            If lNeedHeader Then  'get operation description for header
                                                Dim lOperName As String = ""
                                                Select Case lLocation.Substring(0, 1)
                                                    Case "P" : lOperName = "PERLND"
                                                    Case "I" : lOperName = "IMPLND"
                                                    Case "R" : lOperName = "RCHRES"
                                                End Select

                                                Dim lDesc As String = ""
                                                If lOperName.Length > 0 Then
                                                    lDesc = aUci.OpnBlks(lOperName).OperFromID(lLocation.Substring(2)).Description
                                                End If

                                                .Header = lLocation & " (" & lDesc & ")" & vbCrLf
                                                .NumHeaderRows = 1
                                                .Delimiter = vbTab

                                                .NumFields = 2 + lYearlyAttributes.Count
                                                .FieldLength(1) = 16
                                                .FieldName(1) = "Date".PadRight(12)
                                                .FieldLength(2) = aFieldWidth
                                                .FieldName(2) = Center("Mean", aFieldWidth)
                                                For i As Integer = 3 To .NumFields
                                                    .FieldLength(i) = aFieldWidth
                                                    .FieldName(i) = Center(lYearlyAttributes.Item(i - 3).Arguments(1).Value.ToString, aFieldWidth)
                                                Next
                                                .CurrentRecord = 1
                                                lNeedHeader = False
                                            End If
                                            If lPendingOutput.Length > 0 Then
                                                Dim lPendingRecords() As String = lPendingOutput.Split(vbCr)
                                                For Each lPendingRecord As String In lPendingRecords
                                                    .Value(1) = lPendingRecord
                                                    .CurrentRecord += 1
                                                Next
                                                lPendingOutput = ""
                                            End If

                                            Dim lAttribute As atcDefinedValue
                                            Dim lStateVariable As Boolean
                                            Dim lMult As Double = 1.0
                                            'Select Case lConstituentKey
                                            '    Case "POQUAL-BOD", "SOQUAL-BOD", "IOQUAL-BOD", "AOQUAL-BOD"
                                            '        'might need another multiplier for bod
                                            '        If aBalanceType = "BOD" Then
                                            '            lMult = 0.4
                                            '        ElseIf aBalanceType = "OrganicN" Or aBalanceType = "TotalN" Then
                                            '            If lMultipleIndex = 1 Then
                                            '                lMult = 0.048
                                            '            ElseIf lMultipleIndex = 2 Then
                                            '                0.021176 'obtained by multiplying 0.4 to 0.05294
                                            '            End If
                                            '        ElseIf aBalanceType = "OrganicP" Or aBalanceType = "TotalP" Then
                                            '            If lMultipleIndex = 1 Then
                                            '                lMult = 0.0023
                                            '            ElseIf lMultipleIndex = 2 Then
                                            '                lMult = 0.0029304
                                            'obtained by multiplying 0.4 to 0.007326
                                            '            End If
                                            '        End If
                                            '    Case "POQUAL-F.Coliform", "SOQUAL-F.Coliform"
                                            '        lMult = 1 / 1000000000.0 '10^9
                                            'End Select

                                            If ConstituentsThatUseLast.Contains(lConstituentKey) Then
                                                lAttribute = lTempDataSet.Attributes.GetDefinedValue("Last")
                                                lStateVariable = True
                                            Else
                                                lAttribute = lTempDataSet.Attributes.GetDefinedValue("SumAnnual")
                                                lStateVariable = False
                                            End If

                                            .Value(1) = lConstituentName.PadRight(aFieldWidth)
                                            If Not lAttribute Is Nothing Then
                                                If lStateVariable Then 'no value needed for mean column
                                                    .Value(2) = "<NA>".PadLeft(10)
                                                Else
                                                    .Value(2) = DecimalAlign(lMult * lAttribute.Value, aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                                End If
                                                Dim lFieldIndex As Integer = 3
                                                For Each lAttribute In lYearlyAttributes
                                                    .Value(lFieldIndex) = DecimalAlign(lMult * lAttribute.Value, aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                                    lFieldIndex += 1
                                                Next
                                            Else
                                                .Value(2) = "Skip-NoData"
                                            End If
                                            .CurrentRecord += 1
                                        ElseIf lConstituentKey.StartsWith("Total") AndAlso _
                                               lConstituentKey.Length > 5 AndAlso _
                                               IsNumeric(lConstituentKey.Substring(5)) Then
                                            Dim lTotalCount As Integer = lConstituentKey.Substring(5)
                                            Dim lCurFieldValues(.NumFields) As Double
                                            Dim lCurrentRecordSave As Integer = .CurrentRecord
                                            For lCount As Integer = 1 To lTotalCount
                                                .CurrentRecord -= 1
                                                For lFieldPos As Integer = 2 To lCurFieldValues.GetUpperBound(0)
                                                    If IsNumeric(.Value(lFieldPos)) Then
                                                        lCurFieldValues(lFieldPos) += .Value(lFieldPos)
                                                    Else
                                                        Logger.Dbg("Why")
                                                    End If
                                                Next
                                            Next
                                            .CurrentRecord = lCurrentRecordSave
                                            .Value(1) = lConstituentName.PadRight(aFieldWidth)
                                            For lFieldPos As Integer = 2 To lCurFieldValues.GetUpperBound(0)
                                                .Value(lFieldPos) = DecimalAlign(lCurFieldValues(lFieldPos), aFieldWidth, aDecimalPlaces, aSignificantDigits)
                                            Next
                                            .CurrentRecord += 1
                                        Else
                                            If lPendingOutput.Length > 0 Then
                                                lPendingOutput &= vbCr
                                            End If
                                            If lConstituentKey.StartsWith("Header") Then
                                                lPendingOutput &= vbCr
                                            End If
                                            lPendingOutput &= lConstituentName
                                        End If
                                    End If
                                Next
                                If lOutputTable.NumFields > 0 Then
                                    If aDateRows Then
                                        lReport.Append(.ToStringPivoted)
                                    Else
                                        lReport.Append(.ToString)
                                    End If
                                End If
                            End With
                        Catch lEx As Exception
                            Logger.Dbg(lEx.Message)
                        End Try

                        If lConstituentsToOutput.Count = 0 Then
                            Logger.Dbg(" AGCHEM at " & lLocation & " has no timeseries to output in script!")
                        Else
                            If lPendingOutput.Length > 0 Then
                                If lNeedHeader Then
                                    lReport.AppendLine("No data at " & lLocation & "!")
                                Else
                                    Logger.Dbg("  No data for " & lPendingOutput)
                                End If
                            End If
                        End If
                        lLocationDataGroup = Nothing
                        lReport.AppendLine(vbCrLf)
                    Else
                        'Logger.Dbg("   SKIP " & lLocation)
                    End If
                Next lLocation
            Next lConstituentsToOutput
        Next lOperationKey

        Return lReport
    End Function

End Module
