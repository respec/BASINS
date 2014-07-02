Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcUCI


Public Module ConstituentBalance
    Public Sub ReportsToFiles(ByVal aUci As atcUCI.HspfUci, _
                              ByVal aOperations As atcCollection, _
                              ByVal aBalanceTypes As atcCollection, _
                              ByVal aScenario As String, _
                              ByVal aScenarioResults As atcTimeseriesSource, _
                              ByVal aLocations As atcCollection, _
                              ByVal aRunMade As String, _
                     Optional ByVal aDateColumns As Boolean = False, _
                     Optional ByVal aDecimalPlaces As Integer = 3, _
                     Optional ByVal aSignificantDigits As Integer = 5, _
                     Optional ByVal aFieldWidth As Integer = 12)

        For Each lBalanceType As String In aBalanceTypes
            Dim lReport As atcReport.ReportText = Report(aUci, lBalanceType, aOperations, _
                                                         aScenario, aScenarioResults, aLocations, _
                                                         aRunMade, _
                                                         aDateColumns, aDecimalPlaces, aSignificantDigits, aFieldWidth)
            Dim lOutFileName As String = aScenario & "_" & lBalanceType & "_Balance.txt"
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lReport.ToString)
        Next lBalanceType
    End Sub

    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aBalanceType As String, _
                           ByVal aOperationTypes As atcCollection, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcTimeseriesSource, _
                           ByVal aLocations As atcCollection, _
                           ByVal aRunMade As String, _
                  Optional ByVal aDateRows As Boolean = False, _
                  Optional ByVal aDecimalPlaces As Integer = 3, _
                  Optional ByVal aSignificantDigits As Integer = 5, _
                  Optional ByVal aFieldWidth As Integer = 12) As atcReport.IReport
        Dim lConstituentsToOutput As atcCollection = ConstituentsToOutput(aBalanceType)

        Dim lReport As New atcReport.ReportText
        lReport.AppendLine(aBalanceType & " Balance Report For " & aScenario)
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunPeriod)
        If aBalanceType = "Water" Then
            If aUci.GlobalBlock.EmFg = 1 Then
                lReport.AppendLine("   (Units:Inches)")
            Else
                lReport.AppendLine("   (Units:mm)")
            End If
        ElseIf aBalanceType = "Sediment" Then
            'aDecimalPlaces = 1 'TODO: adjust in calling routine?
        End If
        lReport.AppendLine(vbCrLf)
        Dim lConstituentKey As String

        For Each lOperationKey As String In aOperationTypes.Keys
            For Each lLocation As String In aLocations
                If lLocation.StartsWith(lOperationKey) Then
                    'Logger.Dbg(aOperations(lOperationIndex) & " " & lLocation)
                    Dim lLocationDataGroup As atcTimeseriesGroup = aScenarioResults.DataSets.FindData("Location", lLocation)
                    'Logger.Dbg("     MatchingDatasetCount " & lTempDataGroup.Count)
                    Dim lNeedHeader As Boolean = True
                    Dim lPendingOutput As String = ""
                    Dim lHaveData As Boolean = False

                    Try
                        Dim lOutputTable As New atcTableDelimited
                        lOutputTable.TrimValues = False
                        With lOutputTable
                            For lConstituentIndex As Integer = 0 To lConstituentsToOutput.Count - 1 'Anurag Changed the way For loop works to enable easy skipping when there are no AgCHEM constituents. Basically if the HBN file has no data for ("P:NO3+NO2-N - SURFACE LAYER OUTFLOW") or ("P:PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW") the code assumes that there are no AGCHEM constituents for that specific PERLND and looks for the PQUAL constituents
                                'For Each lConstituentKey In lConstituentsToOutput.Keys
                                lConstituentKey = lConstituentsToOutput.Keys(lConstituentIndex)
                                If lConstituentKey.StartsWith(lOperationKey) Then
                                    Dim lConstituentName As String = lConstituentsToOutput.ItemByKey(lConstituentKey)
                                    Dim lMultipleIndex As Integer = 0
                                    If Not (lConstituentKey.ToLower.Contains("header") Or lConstituentKey.ToLower.StartsWith("total")) Then
                                        If lConstituentKey.EndsWith("1") Or lConstituentKey.EndsWith("2") Then
                                            lMultipleIndex = lConstituentKey.Substring(lConstituentKey.Length - 1)
                                            lConstituentKey = lConstituentKey.Substring(0, lConstituentKey.Length - 1)
                                        End If
                                    End If
                                    lConstituentKey = lConstituentKey.Remove(0, 2)
                                    Dim lConstituentDataGroup As atcTimeseriesGroup = lLocationDataGroup.FindData("Constituent", lConstituentKey)
                                    If lConstituentDataGroup.Count > 0 Then
                                        lHaveData = True
                                      

                                        Dim lTempDataSet As atcDataSet = lConstituentDataGroup.Item(0)
                                        Dim lSeasons As atcSeasonBase
                                        If aUci.GlobalBlock.SDate(1) = 10 Then 'month Oct
                                            lSeasons = New atcSeasonsWaterYear
                                        Else
                                            lSeasons = New atcSeasonsCalendarYear
                                        End If
                                        Dim lSeasonalAttributes As New atcDataAttributes
                                        If ConstituentsThatUseLast.Contains(lConstituentKey) Then
                                            lSeasonalAttributes.SetValue("Last", 0)
                                        Else
                                            lSeasonalAttributes.SetValue("Sum", 0) 'fluxes are summed from daily, monthly or annual to annual
                                        End If
                                        Dim lYearlyAttributes As New atcDataAttributes
                                        lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lYearlyAttributes)
                                        Dim lOperName As String = ""
                                        Select Case lLocation.Substring(0, 1)
                                            Case "P" : lOperName = "PERLND"
                                            Case "I" : lOperName = "IMPLND"
                                            Case "R" : lOperName = "RCHRES"
                                        End Select
                                        If lNeedHeader Then  'get operation description for header
                                           
                                            Dim lDesc As String = ""
                                            If lOperName.Length > 0 Then
                                                lDesc = aUci.OpnBlks(lOperName).OperFromID(lLocation.Substring(2)).Description
                                            End If

                                            Select Case aBalanceType & "_" & lOperName
                                                Case "Water_PERLND", "Water_IMPLND"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (Inches)"
                                                Case "Water_RCHRES"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (ac-ft)"
                                                Case "Sediment_PERLND", "Sediement_IMPLND"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (tons/ac)"
                                                Case "Sediment_RCHRES"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (tons)"
                                                Case "TotalN_PERLND", "TotalN_IMPLND", "TotalP_PERLND", "TotalP_IMPLND", "BOD-PQUAL_PERLND", "BOD-PQUAL_IMPLND"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (lbs/ac)"
                                                Case "TotalN_RCHRES", "TotalP_RCHRES", "BOD-PQUAL_RCHRES"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (lbs)"
                                                Case "FColi_PERLND", "FColi_IMPLND"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (10^9 org/ac)"
                                                Case "FColi_RCHRES"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (10^9 org)"

                                            End Select

                                            
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
                                        Dim lOperation As HspfOperation = aUci.OpnBlks(lOperName).OperFromID(lLocation.Substring(2))
                                        Dim lMassLinkID As Integer = 0
                                        Dim lMassLinkFactor As Double = 0
                                        Dim lTotalLoad As Double = 0.0
                                        Dim lTotalArea As Double = 0.0

                                        If Not lOperName = "RCHRES" And Not lConstituentKey.ToLower.Contains("storage") Then
                                            For Each lConnection As HspfConnection In lOperation.Targets

                                                If lConnection.Target.VolName = "RCHRES" Then
                                                    Dim aConversionFactor As Double = 0.0
                                                    lMassLinkID = lConnection.MassLink
                                                    If aBalanceType = "TotalN" Then

                                                        Dim aReach As HspfOperation = aUci.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                                                        Dim lOperationIndex As Integer = aUci.OpnSeqBlock.Opns.IndexOf(aReach)
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
                                                        Dim CVBP As Double = 31 * BPCNTC / 1200 / CVBPC
                                                        'conversion from biomass to P
                                                        aConversionFactor = CVON
                                                    ElseIf aBalanceType = "TotalP" Then
                                                        lMassLinkID = lConnection.MassLink
                                                        Dim aReach As HspfOperation = aUci.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                                                        Dim lOperationIndex As Integer = aUci.OpnSeqBlock.Opns.IndexOf(aReach)
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
                                                        Dim CVBP As Double = 31 * BPCNTC / 1200 / CVBPC
                                                        'conversion from biomass to P
                                                        Dim CVOP As Double = CVBP / CVBO
                                                        'conversion from oxygen to P
                                                        aConversionFactor = CVOP
                                                    Else
                                                        aConversionFactor = 0.0
                                                    End If

                                                    lMassLinkFactor = FindMassLinkFactor(aUci, lMassLinkID, lConstituentKey.ToUpper, aBalanceType, _
                                                                                   aConversionFactor, lMultipleIndex)
                                                    Dim lArea As Double = lConnection.MFact
                                                    If lArea = 0 Then
                                                        lArea = 0.0000000001
                                                    End If
                                                    lTotalLoad += lArea * lMassLinkFactor
                                                    lTotalArea += lArea
                                                End If

                                            Next
                                            If lTotalLoad = 0 And lTotalArea > 0 Then 'Need to verify this condition
                                                lMult = 1
                                            Else
                                                lMult = lTotalLoad / lTotalArea
                                            End If
                                        End If



                                        'Select Case lConstituentKey
                                        '    Case "POQUAL-BOD", "SOQUAL-BOD", "IOQUAL-BOD", "AOQUAL-BOD", "WASHQS-BOD"
                                        '        'might need another multiplier for bod
                                        '        If aBalanceType = "BOD-PQUAL" Then
                                        '            lMult = 0.4
                                        '        ElseIf aBalanceType = "OrganicN" Or aBalanceType = "TotalN" Then
                                        '            If lMultipleIndex = 1 Then
                                        '                lMult = 0.048
                                        '            ElseIf lMultipleIndex = 2 Then
                                        '                lMult = 0.021176 'obtained by multiplying 0.4 to 0.05294
                                        '            End If
                                        '        ElseIf aBalanceType = "OrganicP" Or aBalanceType = "TotalP" Then
                                        '            If lMultipleIndex = 1 Then
                                        '                lMult = 0.0023
                                        '                'lMult = 0.0046 'for IRW
                                        '            ElseIf lMultipleIndex = 2 Then
                                        '                lMult = 0.0029304
                                        '                'obtained by multiplying 0.4 to 0.007326
                                        '            End If
                                        '        End If
                                        '    Case "ORGN - TOTAL OUTFLOW"
                                        '        lMult = 1
                                        '        If lConstituentsToOutput(lConstituentIndex).Contains("  BOD from OrganicN") Then
                                        '            lMult = 7.555
                                        '            'When BOD is calculated for the PERLND that have AGCHEM active, the Organic N is used as 
                                        '            'as a surrogate.   The labile Organic N is converted to BOD using CVON and 40% labile fraction (factor
                                        '            ' = 0.4 / CVON = 0.4 / 0.052945)
                                        '            'The BOD calculation for when then AGCHEM is active or not active needs to be more formalized
                                        '        End If
                                        '        If lConstituentsToOutput(lConstituentIndex).Contains("    Refractory N as fraction of TORN") Then
                                        '            lMult = 0.6
                                        '            'In IRW Project, the refractory Organic N was calculated as a fraction of TORN. 
                                        '        End If
                                        '        If lConstituentsToOutput(lConstituentIndex).Contains("    Labile N as fraction of TORN") Then
                                        '            lMult = 0.4
                                        '            'In IRW Project, the refractory Organic N was calculated as a fraction of TORN. 
                                        '        End If
                                        '        If lConstituentsToOutput(lConstituentIndex).Contains("    Labile Org P from POORN") Then
                                        '            lMult = 0.05534793
                                        '            'In IRW Project, this number is calculated as 7.555 * 0.007326
                                        '        End If


                                        '    Case "LABILE ORGN - SEDIMENT ASSOC OUTFLOW"
                                        '        lMult = 0

                                        '    Case "SDORP"
                                        '        lMult = 1

                                        '        If lConstituentsToOutput(lConstituentIndex).Contains("    Refractory Org P from SEDP 1") Then
                                        '            lMult = 0.6
                                        '            'lMult = 1.2 'for IRW
                                        '            'In IRW Project, Refractory organic P was calculated as a fraction of TORP
                                        '        End If

                                        'End Select
                                        'If lConstituentKey.Contains("F.Coliform") Then
                                        '    lMult = 1 / 1000000000.0 '10^9
                                        'End If

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
                                                If lConstituentKey.Contains("LABILE ORGN - SEDIMENT ASSOC OUTFLOW") Then lMult = 0
                                                'This condition is for a specific case in IRW and I may need to revisit it, soon.
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
                                           lConstituentKey.Length > 5 AndAlso
                                           IsNumeric(lConstituentKey.Substring(5, 1)) Then
                                        Dim lTotalCount As Integer = lConstituentKey.Substring(5, 1)
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
                                        Dim lSkipTo As String = FindSkipTo(lConstituentKey)
                                        If lSkipTo IsNot Nothing Then
                                            Dim lSkipToIndex As Integer = lConstituentsToOutput.IndexOf(lSkipTo)
                                            If lSkipToIndex > lConstituentIndex Then
                                                lConstituentIndex = lSkipToIndex - 1
                                            End If
                                            lPendingOutput = ""
                                        End If
                                    End If
                                End If
                            Next
                            If lHaveData AndAlso lOutputTable.NumFields > 0 Then
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
                        Logger.Dbg(" BalanceType " & aBalanceType & " at " & lLocation & " has no timeseries to output in script!")
                    Else
                        If lPendingOutput.Length > 0 Then
                            If lNeedHeader Then
                                lReport.AppendLine("No data for " & aBalanceType & " balance report at " & lLocation & "!")
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
        Next lOperationKey

        Return lReport
    End Function
    Private Function FindMassLinkFactor(ByVal aUCI As HspfUci, ByVal aMassLink As Integer, ByVal aConstituent As String, _
                                           ByVal aBalanceType As String, ByVal aConversionFactor As Double, ByVal aMultipleIndex As Integer) As Double
        Dim lMassLinkFactor As Double = 0.0
        For Each lMassLink As HspfMassLink In aUCI.MassLinks

            If lMassLink.MassLinkId = aMassLink Then

                Select aBalanceType
                    Case "Sediment"
                        Select Case aConstituent & "_" & lMassLink.Target.Member.ToString
                            Case "WSSD_ISED"
                                lMassLinkFactor += lMassLink.MFact
                            Case "SCRSD_ISED"
                                lMassLinkFactor += lMassLink.MFact
                            Case "SOSLD_ISED"
                                lMassLinkFactor += lMassLink.MFact
                        End Select
                    Case "Water"
                        If (aConstituent = "PERO" Or aConstituent = "SURO" Or aConstituent = "IFWO" Or aConstituent = "AGWO") _
                            And lMassLink.Target.Member = "IVOL" Then
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        End If

                    Case "TotalN"

                        Select Case aConstituent & "_" & lMassLink.Target.Member.ToString & _
                            "_" & lMassLink.Target.MemSub1
                            Case "NO3+NO2-N - SURFACE LAYER OUTFLOW_NUIF1_1", "NO3+NO2-N - UPPER LAYER OUTFLOW_NUIF1_1", "NO3+NO2-N - GROUNDWATER OUTFLOW_NUIF1_1", _
                                "NO3-N - TOTAL OUTFLOW_NUIF1_1"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "NH4-N IN SOLUTION - SURFACE LAYER OUTFLOW_NUIF1_2", "NH4-N IN SOLUTION - UPPER LAYER OUTFLOW_NUIF1_2", _
                                "NH4-N IN SOLUTION - GROUNDWATER OUTFLOW_NUIF1_2"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "NH4-N ADS - SEDIMENT ASSOC OUTFLOW_NUIF2_1"
                                If lMassLink.Target.MemSub2 = 1 Then
                                    lMassLinkFactor += lMassLink.MFact
                                End If
                            Case "NH4-N ADS - SEDIMENT ASSOC OUTFLOW_NUIF2_2"
                                If lMassLink.Target.MemSub2 = 1 Then
                                    lMassLinkFactor += lMassLink.MFact
                                End If
                            Case "NH4-N ADS - SEDIMENT ASSOC OUTFLOW_NUIF2_3"
                                If lMassLink.Target.MemSub2 = 1 Then
                                    lMassLinkFactor += lMassLink.MFact
                                End If

                            Case "POQUAL-NH3+NH4_NUIF1_2", "SOQUAL-NH3+NH4_NUIF1_2", "AOQUAL-NH3+NH4_NUIF1_2", "IOQUAL-NH3+NH4_NUIF1_2"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "POQUAL-NO3_NUIF1_1", "SOQUAL-NO3_NUIF1_1", "IOQUAL-NO3_NUIF1_1", "AOQUAL-NO3_NUIF1_1"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "WASHQS-BOD_PKIF_3", "SOQUAL-BOD_PKIF_3", "IOQUAL-BOD_PKIF_3", "AOQUAL-BOD_PKIF_3", "POQUAL-BOD_PKIF_3"
                                If aMultipleIndex = 1 Then
                                    lMassLinkFactor = lMassLink.MFact
                                ElseIf aMultipleIndex = 2 Then
                                    lMassLinkFactor = BODMFact(aUCI, aConstituent, lMassLink.MassLinkId) * aConversionFactor
                                End If
                                Return lMassLinkFactor
                            Case "ORGN - TOTAL OUTFLOW_PKIF_3"
                                If aMultipleIndex = 2 Then
                                    lMassLinkFactor = lMassLink.MFact
                                ElseIf aMultipleIndex = 1 Then
                                    lMassLinkFactor = 1 - lMassLink.MFact
                                ElseIf aMultipleIndex = 0 Then
                                    lMassLinkFactor = 1
                                End If
                                Return lMassLinkFactor
                            Case "LABILE ORGN - SEDIMENT ASSOC OUTFLOW_PKIF_3", "REFRAC ORGN - SEDIMENT ASSOC OUTFLOW_PKIF_3"
                                If aConstituent.Contains("REFRAC") Then
                                    lMassLinkFactor = lMassLink.MFact
                                Else
                                    lMassLinkFactor = 1 - lMassLink.MFact
                                End If
                                Return lMassLinkFactor
                        End Select
                        If aConstituent.Contains("NITROGEN - TOTAL OUTFLOW") Then
                            lMassLinkFactor = 1
                            Return lMassLinkFactor

                        End If


                    Case "TotalP"
                        Select Case aConstituent & "_" & lMassLink.Target.Member.ToString & _
                            "_" & lMassLink.Target.MemSub1
                            Case "POQUAL-ORTHO P_NUIF1_4", "SOQUAL-ORTHO P_NUIF1_4", "IOQUAL-ORTHO P_NUIF1_4", "AOQUAL-ORTHO P_NUIF1_4"
                                lMassLinkFactor = lMassLink.MFact
                                Return lMassLinkFactor
                            Case "WASHQS-BOD_PKIF_4", "SOQUAL-BOD_PKIF_4", "IOQUAL-BOD_PKIF_4", "AOQUAL-BOD_PKIF_4", "POQUAL-BOD_PKIF_4"
                                If aMultipleIndex = 1 Then
                                    lMassLinkFactor = lMassLink.MFact
                                ElseIf aMultipleIndex = 2 Then
                                    lMassLinkFactor = BODMFact(aUCI, aConstituent, lMassLink.MassLinkId) * aConversionFactor
                                End If
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
                            Case Else
                                lMassLinkFactor = 1.0
                        End Select

                End Select


            End If

        Next

        Return lMassLinkFactor


    End Function
    Private Function BODMFact(ByVal aUCI As HspfUci, ByVal aconstituent As String, ByVal aMassLinkID As Integer) As Double
        For Each lMasslink As HspfMassLink In aUCI.MassLinks
            If lMasslink.MassLinkId = aMassLinkID AndAlso _
                (lMasslink.Source.Member.Substring(0, 2) = "PO" Or lMasslink.Source.Member.Substring(0, 2) = aconstituent.Substring(0, 2)) AndAlso _
                lMasslink.Target.Member = "OXIF" AndAlso _
                lMasslink.Target.MemSub1 = 2 Then
                Return lMasslink.MFact

            End If
        Next lMasslink
    End Function
End Module
