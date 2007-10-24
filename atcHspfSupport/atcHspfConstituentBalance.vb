Imports atcUtility
Imports atcData
Imports atcSeasons
Imports MapWinUtility

Public Module ConstituentBalance
    Public Sub ReportsToFiles(ByVal aOperations As atcCollection, _
                              ByVal aBalanceTypes As atcCollection, _
                              ByVal aScenario As String, _
                              ByVal aScenarioResults As atcDataSource, _
                              ByVal aLocations As atcCollection, _
                              ByVal aRunMade As String)

        'make uci available 
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")
        Dim lHspfUci As New atcUCI.HspfUci
        lHspfUci.FastReadUciForStarter(lMsg, aScenario & ".uci")

        For Each lBalanceType As String In aBalanceTypes
            Dim lString As Text.StringBuilder = Report(lHspfUci, lBalanceType, aOperations, _
                                                       aScenario, aScenarioResults, aLocations, _
                                                       aRunMade)
            Dim lOutFileName As String = aScenario & "_" & lBalanceType & "_" & "Balance.txt"
            Logger.Dbg("  WriteReportTo " & lOutFileName)
            SaveFileString(lOutFileName, lString.ToString)
        Next lBalanceType
    End Sub

    Public Function Report(ByVal aUci As atcUCI.HspfUci, _
                           ByVal aBalanceType As String, _
                           ByVal aOperations As atcCollection, _
                           ByVal aScenario As String, _
                           ByVal aScenarioResults As atcDataSource, _
                           ByVal aLocations As atcCollection, _
                           ByVal aRunMade As String) As Text.StringBuilder
        Dim lConstituents2Output As New atcCollection
        Select Case aBalanceType
            Case "Water"
                lConstituents2Output.Add("I:SUPY", "Rainfall")
                lConstituents2Output.Add("I:SURO", "Runoff  ")
                lConstituents2Output.Add("I:PET", "ET Potential")
                lConstituents2Output.Add("I:IMPEV", "ET Actual")
                lConstituents2Output.Add("P:SUPY", "Rainfall")
                'lConstituents2Output.Add("P:Header0", "Irrigation")
                'lConstituents2Output.Add("P:SURLI", "  Surface")
                lConstituents2Output.Add("P:Header1", "Runoff")
                lConstituents2Output.Add("P:SURO", "    Surface")
                lConstituents2Output.Add("P:IFWO", "    Interflow")
                lConstituents2Output.Add("P:AGWO", "    Baseflow")
                lConstituents2Output.Add("P:PERO", "    Total")
                lConstituents2Output.Add("P:IGWI", "Deep Grnd Water")
                lConstituents2Output.Add("P:Header2", "Evaporation")
                lConstituents2Output.Add("P:PET", "    Potential")
                lConstituents2Output.Add("P:CEPE", "    Intercep St")
                lConstituents2Output.Add("P:UZET", "    Upper Zone")
                lConstituents2Output.Add("P:LZET", "    Lower Zone")
                lConstituents2Output.Add("P:AGWET", "    Grnd Water")
                lConstituents2Output.Add("P:BASET", "    Baseflow")
                lConstituents2Output.Add("P:TAET", "    Total")
            Case "SedimentCopper"
                lConstituents2Output.Add("I:SOSLD", "Solids   ")
                lConstituents2Output.Add("I:SOQUAL-Copper", "Copper   ")
                lConstituents2Output.Add("I:SOQS-Copper", "Sed-Assoc Cu")
                lConstituents2Output.Add("I:SOQO-Copper", "Flow-Assoc Cu")
                lConstituents2Output.Add("P:SOSED", "Sediment")
                lConstituents2Output.Add("P:SOQUAL-Copper", "  Surface Cu")
                lConstituents2Output.Add("P:IOQUAL-Copper", "  Interflow Cu")
                lConstituents2Output.Add("P:AOQUAL-Copper", "  Groundwater Cu")
                lConstituents2Output.Add("P:SOQS-Copper", "Sed-Assoc Cu")
                lConstituents2Output.Add("P:SOQO-Copper", "Flow-Assoc Cu")
                lConstituents2Output.Add("P:POQUAL-Copper", "Total Cu")
                lConstituents2Output.Add("R:ROSED-SAND", "  Sand")
                lConstituents2Output.Add("R:ROSED-SILT", "  Silt")
                lConstituents2Output.Add("R:ROSED-CLAY", "  Clay")
                lConstituents2Output.Add("R:ROSED-TOT", "Total Sediment")
                lConstituents2Output.Add("R:Copper-RODQAL", "Disolved Cu")
                lConstituents2Output.Add("R:Copper-ROSQAL-SAND", "  Sand Cu")
                lConstituents2Output.Add("R:Copper-ROSQAL-SILT", "  Silt Cu")
                lConstituents2Output.Add("R:Copper-ROSQAL-CLAY", "  Clay Cu")
                lConstituents2Output.Add("R:Copper-ROSQAL-Tot", "Total Sediment Cu")
                lConstituents2Output.Add("R:Copper-TROQAL", "Total Cu")
            Case "NfromPQUAL"
                lConstituents2Output.Add("P:Header1", "NH4 (lb/ac)")
                lConstituents2Output.Add("P:WASHQS-NH4", "WASHQS")
                lConstituents2Output.Add("P:SCRQS-NH4", "SCRQS")
                lConstituents2Output.Add("P:SOQS-NH4", "SOQS")
                lConstituents2Output.Add("P:SOQO-NH4", "SOQO")
                lConstituents2Output.Add("P:SOQUAL-NH4", "SOQUAL")
                lConstituents2Output.Add("P:IOQUAL-NH4", "IOQUAL")
                lConstituents2Output.Add("P:AOQUAL-NH4", "AOQUAL")
                lConstituents2Output.Add("P:POQUAL-NH4", "POQUAL")
                lConstituents2Output.Add("P:Header2", "NO3 (lb/ac)")
                lConstituents2Output.Add("P:WASHQS-NO3", "WASHQS")
                lConstituents2Output.Add("P:SCRQS-NO3", "SCRQS")
                lConstituents2Output.Add("P:SOQS-NO3", "SOQS")
                lConstituents2Output.Add("P:SOQO-NO3", "SOQO")
                lConstituents2Output.Add("P:SOQUAL-NO3", "SOQUAL")
                lConstituents2Output.Add("P:IOQUAL-NO3", "IOQUAL")
                lConstituents2Output.Add("P:AOQUAL-NO3", "AOQUAL")
                lConstituents2Output.Add("P:POQUAL-NO3", "POQUAL")
            Case "TotalN"
                lConstituents2Output.Add("P:Header1", "Atmospheric Deposition (lb/ac)")
                lConstituents2Output.Add("P:NH4-N - SURFACE LAYER - TOTAL AD", "NH4-N - Surface Layer")
                lConstituents2Output.Add("P:NH4-N - UPPER LAYER - TOTAL AD", "NH4-N - Upper Layer")
                lConstituents2Output.Add("P:NO3-N - SURFACE LAYER - TOTAL AD", "NO3-N - Surface Layer")
                lConstituents2Output.Add("P:NO3-N - UPPER LAYER - TOTAL AD", "NO3-N - Upper Layer")
                lConstituents2Output.Add("P:ORGN - SURFACE LAYER - TOTAL AD", "ORGN - Surface Layer")
                lConstituents2Output.Add("P:ORGN - UPPER LAYER - TOTAL AD", "ORGN - Upper Layer")

                lConstituents2Output.Add("P:", "")
                lConstituents2Output.Add("P:TOTAL NITROGEN APPLICATION", "N Application (lb/ac)")

                lConstituents2Output.Add("P:Header1", "Nutrient Loss (lb/ac)")
                lConstituents2Output.Add("P:Header2", "NO3 Loss")
                lConstituents2Output.Add("P:NO3+NO2-N - SURFACE LAYER OUTFLOW", "Surface")
                lConstituents2Output.Add("P:NO3+NO2-N - UPPER LAYER OUTFLOW", "Interflow")
                lConstituents2Output.Add("P:NO3+NO2-N - GROUNDWATER OUTFLOW", "Baseflow")
                lConstituents2Output.Add("P:NO3-N - TOTAL OUTFLOW", "Total")
                lConstituents2Output.Add("P:Header2", "NH3 Loss")
                lConstituents2Output.Add("P:NH4-N IN SOLUTION - SURFACE LAYER OUTFLOW", "Surface")
                lConstituents2Output.Add("P:NH4-N IN SOLUTION - UPPER LAYER OUTFLOW", "Interflow")
                lConstituents2Output.Add("P:NH4-N IN SOLUTION - GROUNDWATER OUTFLOW", "Baseflow")
                lConstituents2Output.Add("P:NH4-N ADS - SEDIMENT ASSOC OUTFLOW", "Sediment")
                lConstituents2Output.Add("P:NH4-N - TOTAL OUTFLOW", "Total")
                lConstituents2Output.Add("P:Header2", "ORGN")
                lConstituents2Output.Add("P:LABILE ORGN - SEDIMENT ASSOC OUTFLOW", "Sediment Labile")
                lConstituents2Output.Add("P:REFRAC ORGN - SEDIMENT ASSOC OUTFLOW", "Sediment Refrac")
                lConstituents2Output.Add("P:ORGN - TOTAL OUTFLOW", "Total")

                lConstituents2Output.Add("P:", "")
                lConstituents2Output.Add("P:NITROGEN - TOTAL OUTFLOW", "Total N Loss (lb/ac)")

                lConstituents2Output.Add("P:Header1", "Below Ground Plant N Return (lb/ac)")
                lConstituents2Output.Add("P:TRTLBN", "To Labile ORGN")
                lConstituents2Output.Add("P:TRTRBN", "To Refrac ORGN")

                lConstituents2Output.Add("P:Header1", "Plant Uptake (lb/ac)")
                lConstituents2Output.Add("P:Header2", "NH3")
                lConstituents2Output.Add("P:SAMUPB", "Surface")
                lConstituents2Output.Add("P:UAMUPB", "Upper Zone")
                lConstituents2Output.Add("P:LAMUPB", "Lower Zone")
                lConstituents2Output.Add("P:AAMUPB", "Active GW")
                lConstituents2Output.Add("P:TAMUPB", "Total")
                lConstituents2Output.Add("P:Header2", "NO3")
                lConstituents2Output.Add("P:SNIUPB", "Surface")
                lConstituents2Output.Add("P:UNIUPB", "Upper Zone")
                lConstituents2Output.Add("P:LNIUPB", "Lower Zone")
                lConstituents2Output.Add("P:ANIUPB", "Active GW")
                lConstituents2Output.Add("P:TNIUPB", "Total")

                lConstituents2Output.Add("P:Header1", "Other Fluxes (lb/ac)")
                lConstituents2Output.Add("P:TDENI", "Denitrification")
                lConstituents2Output.Add("P:TAMNIT", "NH3 Nitrification")
                lConstituents2Output.Add("P:TAMIMB", "NH3 Immobilization")
                lConstituents2Output.Add("P:TORNMN", "ORGN Mineralization")
                lConstituents2Output.Add("P:TNIIMB", "NO3 Immobilization")
                lConstituents2Output.Add("P:TREFON", "Labile/Refr ORGN Conversion")
                lConstituents2Output.Add("P:TFIXN", "Nitrogen Fixation")
                lConstituents2Output.Add("P:TAMVOL", "NH3 Volatilization")
                'lConstituents2Output.Add("R:Header1", "TAM")
                'lConstituents2Output.Add("R:TAM-INTOT", "  TAM-INTOT")
                'lConstituents2Output.Add("R:TAM-INDIS", "  TAM-INDIS")
                'lConstituents2Output.Add("R:NH4-INPART-TOT", "  NH4-INPART-TOT")
                'lConstituents2Output.Add("R:TAM-OUTTOT", "  TAM-OUTTOT")
                'lConstituents2Output.Add("R:TAM-OUTDIS", "  TAM-OUTDIS")
                'lConstituents2Output.Add("R:TAM-OUTPART-TOT", "  TAM-OUTPART-TOT")
                'lConstituents2Output.Add("R:TAM-OUTTOT-EXIT3", "  TAM-OUTTOT-EXIT3")
                'lConstituents2Output.Add("R:TAM-OUTDIS-EXIT3", "  TAM-OUTDIS-EXIT3")
                'lConstituents2Output.Add("R:TAM-OUTPART-TOT-EXIT3", "  TAM-OUTPART-TOT-EXIT3")
                'lConstituents2Output.Add("R:Header2", "NO3")
                'lConstituents2Output.Add("R:NO3-INTOT", "  NO3-INTOT")
                'lConstituents2Output.Add("R:NO3-PROCFLUX-TOT", "  NO3-PROCFLUX-TOT")
                'lConstituents2Output.Add("R:NO3-OUTTOT", "  NO3-OUTTOT")
                'lConstituents2Output.Add("R:NO3-OUTTOT-EXIT3", "  NO3-OUTTOT-EXIT3")
                lConstituents2Output.Add("R:Header3", "Totals")
                lConstituents2Output.Add("R:N-TOT-IN", "  N-TOT-IN")
                lConstituents2Output.Add("R:N-TOT-OUT", "  N-TOT-OUT")
                lConstituents2Output.Add("R:N-TOT-OUT-EXIT1", "  N-TOT-OUT-EXIT1")
                lConstituents2Output.Add("R:N-TOT-OUT-EXIT2", "  N-TOT-OUT-EXIT2")
                lConstituents2Output.Add("R:N-TOT-OUT-EXIT3", "  N-TOT-OUT-EXIT3")
            Case "TotalP"
        End Select

        Dim lString As New Text.StringBuilder
        lString.AppendLine(aBalanceType & " Balance Report For " & aScenario)
        lString.AppendLine("   Run Made " & aRunMade)
        lString.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lString.AppendLine("   " & aUci.GlobalBlock.RunPeriod)
        If aBalanceType = "Water" Then
            If aUci.GlobalBlock.emfg = 1 Then
                lString.AppendLine("   (Units:Inches)")
            Else
                lString.AppendLine("   (Units:mm)")
            End If
        End If
        lString.AppendLine(vbCrLf)

        Dim lMatchConstituentGroup As atcDataGroup
        Dim lTempDataSet As atcDataSet

        For lOperationIndex As Integer = 0 To aOperations.Count - 1
            Dim lOperationKey As String = aOperations.Keys(lOperationIndex)
            For Each lLocation As String In aLocations
                If lLocation.StartsWith(lOperationKey) Then
                    Logger.Dbg(aOperations(lOperationIndex) & " " & lLocation)
                    Dim lTempDataGroup As atcDataGroup = aScenarioResults.DataSets.FindData("Location", lLocation)
                    Logger.Dbg("     MatchingDatasetCount " & lTempDataGroup.Count)
                    Dim lNeedHeader As Boolean = True
                    Dim lPendingOutput As String = ""
                    For lIndex As Integer = 0 To lConstituents2Output.Count - 1
                        Dim lConstituentKey As String = lConstituents2Output.Keys(lIndex)
                        If lConstituentKey.StartsWith(lOperationKey) Then
                            lConstituentKey = lConstituentKey.Remove(0, 2)
                            Dim lConstituentName As String = lConstituents2Output(lIndex)
                            lMatchConstituentGroup = lTempDataGroup.FindData("Constituent", lConstituentKey)
                            If lMatchConstituentGroup.Count > 0 Then
                                lTempDataSet = lMatchConstituentGroup.Item(0)
                                Dim lSeasons As New atcSeasonsCalendarYear
                                Dim lSeasonalAttributes As New atcDataAttributes
                                Dim lCalculatedAttributes As New atcDataAttributes
                                lSeasonalAttributes.SetValue("Sum", 0) 'fluxes are summed from daily, monthly or annual to annual
                                lSeasons.SetSeasonalAttributes(lTempDataSet, lSeasonalAttributes, lCalculatedAttributes)

                                If lNeedHeader Then
                                    'get operation description for header
                                    Dim lDesc As String = ""
                                    Dim lOperName As String = ""
                                    If lLocation.Substring(0, 1) = "P" Then
                                        lOperName = "PERLND"
                                    ElseIf lLocation.Substring(0, 1) = "I" Then
                                        lOperName = "IMPLND"
                                    ElseIf lLocation.Substring(0, 1) = "R" Then
                                        lOperName = "RCHRES"
                                    End If
                                    If lOperName.Length > 0 Then
                                        lDesc = aUci.OpnBlks(lOperName).operfromid(lLocation.Substring(2)).description
                                    End If

                                    lString.AppendLine(aBalanceType & " Balance Report For " & lLocation & " (" & lDesc & ")" & vbCrLf)
                                    lString.Append("Date    " & vbTab & "      Mean")
                                    For Each lAttribute As atcDefinedValue In lCalculatedAttributes
                                        Dim s As String = lAttribute.Arguments(1).Value
                                        lString.Append(vbTab & s.PadLeft(10))
                                    Next
                                    lString.AppendLine()
                                    lNeedHeader = False
                                End If
                                If lPendingOutput.Length > 0 Then
                                    lString.AppendLine(lPendingOutput)
                                    lPendingOutput = ""
                                End If

                                lString.Append(lConstituentName & vbTab & DecimalAlign(lTempDataSet.Attributes.GetDefinedValue("SumAnnual").Value))
                                For Each lAttribute As atcDefinedValue In lCalculatedAttributes
                                    lString.Append(vbTab & DecimalAlign(lAttribute.Value))
                                Next
                                lString.AppendLine()
                            Else
                                lPendingOutput &= vbCrLf & lConstituentName
                            End If
                        End If
                    Next
                    If lConstituents2Output.Count = 0 Then
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
                    lTempDataGroup = Nothing
                    lString.AppendLine(vbCrLf)
                Else
                    'Logger.Dbg("   SKIP " & lLocation)
                End If
            Next lLocation
        Next lOperationIndex
        Return lString
    End Function
End Module
