Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcUCI


Public Module ConstituentBalance
    Public Sub ReportsToFiles(ByVal aUci As atcUCI.HspfUci,
                              ByVal aOperations As atcCollection,
                              ByVal aBalanceTypes As atcCollection,
                              ByVal aScenario As String,
                              ByVal aScenarioResults As atcDataSource,'ByVal aLocations As atcCollection,
                              ByVal aRunMade As String,
                              ByVal aSDateJ As Double,
                              ByVal aEDateJ As Double,
                              ByVal aConstProperties As List(Of ConstituentProperties),
                     Optional ByVal aDateColumns As Boolean = False,
                     Optional ByVal aDecimalPlaces As Integer = 3,
                     Optional ByVal aSignificantDigits As Integer = 5,
                     Optional ByVal aFieldWidth As Integer = 12)

        For Each lBalanceType As String In aBalanceTypes
            Dim lReport As atcReport.ReportText = Report(aUci, lBalanceType, aOperations,
                                                         aScenario, aScenarioResults, 'aLocations,
                                                         aRunMade, aSDateJ, aEDateJ, aConstProperties,
                                                         aDateColumns, aDecimalPlaces, aSignificantDigits, aFieldWidth)
            Dim lOutFileName As String = aScenario & "_" & lBalanceType & "_Balance.txt"
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lReport.ToString)
        Next lBalanceType
    End Sub

    Public Function Report(ByVal aUci As atcUCI.HspfUci,
                           ByVal aBalanceType As String,
                           ByVal aOperationTypes As atcCollection,
                           ByVal aScenario As String,
                           ByVal aScenarioResults As atcDataSource, 'ByVal aLocations As atcCollection
                           ByVal aRunMade As String,
                           ByVal aSDateJ As Double,
                           ByVal aEDateJ As Double,
                           ByVal aConstProperties As List(Of ConstituentProperties),
                  Optional ByVal aDateRows As Boolean = False,
                  Optional ByVal aDecimalPlaces As Integer = 3,
                  Optional ByVal aSignificantDigits As Integer = 5,
                  Optional ByVal aFieldWidth As Integer = 12) As atcReport.IReport

        Dim lUnits As String = GQualUnits(aUci, aBalanceType)   'if not a gqual, will return a blank string
        Dim lConstituentsToOutput As atcCollection = ConstituentsToOutput(aBalanceType, aConstProperties, , lUnits)

        Dim lReport As New atcReport.ReportText
        lReport.AppendLine(aScenario & " " & "Annual Loading Rates of " & aBalanceType & " For Each PERLND, and IMPLND, and")
        lReport.AppendLine("Annual Loadings of " & aBalanceType & " For Each Reach.")
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
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
        Dim aLocations As New atcCollection
        aLocations.AddRange(aScenarioResults.DataSets.SortedAttributeValues("Location"))
        For Each lOperationKey As String In aOperationTypes.Keys
            Dim lLocationProgress As Integer = 0
            Dim lLastLocation As Integer = aLocations.Count
            For Each lLocation As String In aLocations
                If lLocation.StartsWith(lOperationKey) Then
                    'Logger.Dbg(aOperations(lOperationIndex) & " " & lLocation)
                    Dim lLocationDataGroup As New atcTimeseriesGroup
                    lLocationDataGroup.Add(aScenarioResults.DataSets.FindData("Location", lLocation))
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
                                    Dim lConstituentDataName As String = lConstituentKey
                                    Dim lMultipleIndex As Integer = 0
                                    If Not (lConstituentKey.ToLower.Contains("header") Or lConstituentName.ToLower.Contains("exit")) Then 'Or lConstituentKey.ToLower.StartsWith("total")) Then
                                        If lConstituentKey.EndsWith("1") Or lConstituentKey.EndsWith("2") Then
                                            lMultipleIndex = lConstituentKey.Substring(lConstituentKey.Length - 1)
                                            lConstituentDataName = lConstituentDataName.Substring(0, lConstituentDataName.Length - 1)
                                        End If
                                    End If
                                    lConstituentKey = lConstituentKey.Remove(0, 2)
                                    lConstituentDataName = lConstituentDataName.Remove(0, 2)
                                    Dim lConstituentDataGroup As New atcTimeseriesGroup
                                    lConstituentDataGroup.Add(lLocationDataGroup.FindData("Constituent", lConstituentDataName))
                                    If lConstituentDataGroup.Count > 0 Then
                                        lHaveData = True
                                        Dim lTempDataSet As atcDataSet = lConstituentDataGroup.Item(0)
                                        lTempDataSet = SubsetByDate(lTempDataSet, aSDateJ, aEDateJ, Nothing)
                                        Dim lSeasons As atcSeasonBase
                                        Dim lDate(5) As Integer
                                        J2Date(aSDateJ, lDate)

                                        If lDate(1) = 10 Then 'month Oct
                                            lSeasons = New atcSeasonsWaterYear
                                        Else
                                            lSeasons = New atcSeasonsCalendarYear
                                        End If
                                        Dim lSeasonalAttributes As New atcDataAttributes
                                        If ConstituentsThatUseLast.Contains(lConstituentDataName) Then
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
                                                Case "Sediment_PERLND", "Sediment_IMPLND"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (tons/ac)"
                                                Case "Sediment_RCHRES"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (tons)"
                                                Case "TN_PERLND", "TN_IMPLND", "TP_PERLND", "TP_IMPLND", "BOD-Labile_PERLND", "BOD-Labile_IMPLND"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (lbs/ac)"
                                                Case "TN_RCHRES", "TP_RCHRES", "BOD-Labile_RCHRES"
                                                    .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (lbs)"
                                                Case Else
                                                    Dim lPrefix As String = ""
                                                    If aBalanceType.ToUpper.Contains("F.COLIFORM") Or aBalanceType.ToUpper.StartsWith("BACT") Then 'Assuming this is f.coli or bacteria
                                                        lPrefix = "10^9 "
                                                    End If
                                                    If lOperName = "PERLND" Or lOperName = "IMPLND" Then
                                                        .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (" & lPrefix & lUnits & "/ac)"
                                                    ElseIf lOperName = "RCHRES" Then
                                                        .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ") (" & lPrefix & lUnits & ")"
                                                    End If
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
                                        Dim MassLinkExists As Boolean = True

                                        If lConstituentDataName.ToUpper.Contains("QUAL") OrElse
                                           lConstituentDataName.ToUpper.Contains("SOQO") OrElse
                                           lConstituentDataName.ToUpper.Contains("WASHQS") OrElse
                                           ConstituentsThatNeedMassLink.Contains(lConstituentDataName.ToUpper) Then

                                            For Each lConnection As HspfConnection In lOperation.Targets
                                                If lConnection.Target.VolName = "RCHRES" Then
                                                    Dim aReach As HspfOperation = aUci.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                                                    If aReach Is Nothing Then
                                                        Continue For
                                                        'Anurag added the continue for here to take care of the cases when a PERLND
                                                        'or IMPLD connects to a reach that does not exist in OPN SEQUENCE
                                                        Logger.Dbg("The Reach " & lConnection.Target.VolId & " does not exist. Constituent Balance reports will not be generated 
                                                                    for Operation " & lOperation.Caption)

                                                    End If
                                                    Dim aConversionFactor As Double = 0.0
                                                    If aBalanceType = "TN" Or aBalanceType = "TP" Then
                                                        aConversionFactor = ConversionFactorfromOxygen(aUci, aBalanceType, aReach)
                                                    End If

                                                    lMassLinkID = lConnection.MassLink

                                                    If Not lMassLinkID = 0 Then

                                                        Dim ConstNameMassLink As String = lConstituentDataName.ToUpper
                                                        If Not (aConstProperties.Count = 0 OrElse (lConnection.Source.Opn.Name = "PERLND" AndAlso
                                                                lConnection.Source.Opn.Tables("ACTIVITY").Parms("PQALFG").Value = "0")) Then
                                                            ConstNameMassLink = Split(lConstituentDataName.ToUpper, "-", 2)(1)
                                                            Dim ConstNameEXP As String = ""
                                                            For Each constt As ConstituentProperties In aConstProperties
                                                                If constt.ConstituentNameInUCI.ToUpper = ConstNameMassLink Then
                                                                    ConstNameEXP = constt.ConstNameForEXPPlus
                                                                    If ConstNameEXP = "TAM" Then ConstNameEXP = "NH3+NH4"
                                                                    ConstNameMassLink = Split(lConstituentDataName.ToUpper, "-", 2)(0) & "-" & ConstNameEXP
                                                                End If
                                                            Next
                                                        End If

                                                        lMassLinkFactor = FindMassLinkFactor(aUci, lMassLinkID, ConstNameMassLink, aBalanceType,
                                                                                       aConversionFactor, lMultipleIndex)
                                                    Else
                                                        MassLinkExists = False
                                                    End If
                                                    Dim lArea As Double = lConnection.MFact
                                                    If lArea = 0 Then lArea = 0.0000000001
                                                    If aBalanceType = "Water" OrElse aBalanceType = "WAT" Then lMassLinkFactor *= 12
                                                    lTotalLoad += lArea * lMassLinkFactor
                                                    lTotalArea += lArea
                                                End If
                                            Next
                                            lMult = lTotalLoad / lTotalArea
                                        End If

                                        If lConstituentDataName.ToUpper.Contains("F.COLIFORM") OrElse
                                            lConstituentDataName.ToUpper.StartsWith("BACT") Then 'Assuming that unit of F.Coliform unit is #ORG
                                            lMult = 1 / 1000000000.0 '10^9
                                        End If

                                        If ConstituentsThatUseLast.Contains(lConstituentDataName) Then
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
                                    ElseIf lConstituentKey.StartsWith("Total") AndAlso
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
                                        If lPendingOutput.Length > 0 Then lPendingOutput &= vbCr
                                        If lConstituentDataName.StartsWith("Header") Then lPendingOutput &= vbCr

                                        lPendingOutput &= lConstituentName
                                        Dim lSkipTo As String = FindSkipTo(lConstituentDataName)
                                        If lSkipTo IsNot Nothing Then
                                            Dim lSkipToindex2 As Integer = 2000
                                            If lSkipTo.StartsWith("NO3+NO2") Then
                                                lSkipTo = "NO3+NO2 (PQUAL)"
                                                Dim lSkip2 As String = "NH3+NH4 (PQUAL)"
                                                lSkipToindex2 = lConstituentsToOutput.IndexOf(lSkip2)
                                            End If
                                            Dim lSkipToindex As Integer = lConstituentsToOutput.IndexOf(lSkipTo)

                                            If lSkipToindex > lSkipToindex2 Then lSkipToindex = lSkipToindex2
                                            If lSkipToindex > lConstituentIndex Then lConstituentIndex = lSkipToindex - 1
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
                'lLocationProgress += 1
                'Logger.Progress(lLocation, lLocationProgress, lLastLocation)
            Next lLocation
        Next lOperationKey

        Return lReport
    End Function

End Module
