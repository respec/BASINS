Imports atcUtility
Imports atcData
Imports atcSeasons
Imports MapWinUtility

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
            Dim lString As Text.StringBuilder = Report(aUci, lBalanceType, aOperations, _
                                                       aScenario, aScenarioResults, aLocations, _
                                                       aRunMade, _
                                                       aDateColumns, aDecimalPlaces, aSignificantDigits, aFieldWidth)
            Dim lOutFileName As String = aScenario & "_" & lBalanceType & "_Balance.txt"
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lString.ToString)
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
                  Optional ByVal aFieldWidth As Integer = 12) As Text.StringBuilder
        Dim lConstituentsToOutput As atcCollection = ConstituentsToOutput(aBalanceType)

        Dim lString As New Text.StringBuilder
        lString.AppendLine(aBalanceType & " Balance Report For " & aScenario)
        lString.AppendLine("   Run Made " & aRunMade)
        lString.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lString.AppendLine("   " & aUci.GlobalBlock.RunPeriod)
        If aBalanceType = "Water" Then
            If aUci.GlobalBlock.EmFg = 1 Then
                lString.AppendLine("   (Units:Inches)")
            Else
                lString.AppendLine("   (Units:mm)")
            End If
        ElseIf aBalanceType = "Sediment" Then
            aDecimalPlaces = 1 'TODO: adjust in calling routine?
        End If
        lString.AppendLine(vbCrLf)

        For Each lOperationKey As String In aOperationTypes.Keys
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
                                        Dim lTempDataSet As atcDataSet = lConstituentDataGroup.Item(0)
                                        Dim lSeasons As atcSeasonBase
                                        If aUci.GlobalBlock.SDate(1) = 10 Then 'month Oct
                                            lSeasons = New atcSeasonsWaterYear
                                        Else
                                            lSeasons = New atcSeasonsCalendarYear
                                        End If
                                        Dim lSeasonalAttributes As New atcDataAttributes
                                        Select Case lConstituentKey
                                            Case "BEDDEP", "RSED-BED-SAND", "RSED-BED-SILT", "RSED-BED-CLAY", "RSED-BED-TOT"
                                                lSeasonalAttributes.SetValue("Last", 0) 'fluxes are last from daily, monthly or annual to annual
                                            Case Else
                                                lSeasonalAttributes.SetValue("Sum", 0) 'fluxes are summed from daily, monthly or annual to annual
                                        End Select
                                        Dim lYearlyAttributes As New atcDataAttributes
                                        lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lYearlyAttributes)

                                        If lNeedHeader Then  'get operation description for header
                                            Dim lOperName As String = ""
                                            If lLocation.Substring(0, 1) = "P" Then
                                                lOperName = "PERLND"
                                            ElseIf lLocation.Substring(0, 1) = "I" Then
                                                lOperName = "IMPLND"
                                            ElseIf lLocation.Substring(0, 1) = "R" Then
                                                lOperName = "RCHRES"
                                            End If
                                            Dim lDesc As String = ""
                                            If lOperName.Length > 0 Then
                                                lDesc = aUci.OpnBlks(lOperName).OperFromID(lLocation.Substring(2)).Description
                                            End If

                                            .Header = aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ")" & vbCrLf
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
                                        Select Case lConstituentKey
                                            Case "POQUAL-BOD", "SOQUAL-BOD", "IOQUAL-BOD", "AOQUAL-BOD"
                                                'might need another multiplier for bod
                                                If aBalanceType = "BOD" Then
                                                    lMult = 0.4
                                                ElseIf aBalanceType = "OrganicN" Or aBalanceType = "TotalN" Then
                                                    If lMultipleIndex = 1 Then
                                                        lMult = 0.048
                                                    ElseIf lMultipleIndex = 2 Then
                                                        lMult = 0.05294
                                                    End If
                                                ElseIf aBalanceType = "OrganicP" Or aBalanceType = "TotalP" Then
                                                    If lMultipleIndex = 1 Then
                                                        lMult = 0.0023
                                                    ElseIf lMultipleIndex = 2 Then
                                                        lMult = 0.007326
                                                    End If
                                                End If
                                            Case "POQUAL-F.Coliform", "SOQUAL-F.Coliform"
                                                lMult = 1 / 1000000000.0 '10^9
                                        End Select

                                        Select Case lConstituentKey
                                            Case "BEDDEP", "RSED-BED-SAND", "RSED-BED-SILT", "RSED-BED-CLAY", "RSED-BED-TOT"
                                                lAttribute = lTempDataSet.Attributes.GetDefinedValue("Last")
                                                lStateVariable = True
                                            Case Else
                                                lAttribute = lTempDataSet.Attributes.GetDefinedValue("SumAnnual")
                                                lStateVariable = False
                                        End Select

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
                                    lString.Append(.ToStringPivoted)
                                Else
                                    lString.Append(.ToString)
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
                                lString.AppendLine("No data for " & aBalanceType & " balance report at " & lLocation & "!")
                            Else
                                Logger.Dbg("  No data for " & lPendingOutput)
                            End If
                        End If
                    End If
                    lLocationDataGroup = Nothing
                    lString.AppendLine(vbCrLf)
                Else
                    'Logger.Dbg("   SKIP " & lLocation)
                End If
            Next lLocation
        Next lOperationKey

        Return lString
    End Function
End Module
