Imports atcUtility
Imports atcData
Imports atcUCI

Public Module Utility
    Friend Function ConstituentsToOutput(ByVal aType As String) As atcCollection
        Dim lConstituentsToOutput As New atcCollection
        Select Case aType
            Case "Water"
                lConstituentsToOutput.Add("I:Header0", "Influx")
                lConstituentsToOutput.Add("I:SUPY", "    Rainfall")
                lConstituentsToOutput.Add("I:Header1", "Runoff")
                lConstituentsToOutput.Add("I:SURO", "    Surface")
                lConstituentsToOutput.Add("I:Header2", "Evaporation")
                lConstituentsToOutput.Add("I:PET", "    Potential")
                lConstituentsToOutput.Add("I:IMPEV", "    Actual")
                lConstituentsToOutput.Add("P:Header0", "Influx")
                lConstituentsToOutput.Add("P:SUPY", "    Rainfall")
                lConstituentsToOutput.Add("P:SURLI", "    Irrigation")
                lConstituentsToOutput.Add("P:Header1", "Runoff")
                lConstituentsToOutput.Add("P:SURO", "    Surface")
                lConstituentsToOutput.Add("P:IFWO", "    Interflow")
                lConstituentsToOutput.Add("P:AGWO", "    Baseflow")
                lConstituentsToOutput.Add("P:PERO", "    Total")
                lConstituentsToOutput.Add("P:Header2", "GW Inflow")
                lConstituentsToOutput.Add("P:IGWI", "    Deep")
                lConstituentsToOutput.Add("P:AGWI", "    Active")
                lConstituentsToOutput.Add("P:Total2", "    Total") 'need total of prev 2 rows
                lConstituentsToOutput.Add("P:AGWLI", "    Pumping")
                lConstituentsToOutput.Add("P:Header3", "Evaporation")
                lConstituentsToOutput.Add("P:PET", "    Potential")
                lConstituentsToOutput.Add("P:CEPE", "    Intercep St")
                lConstituentsToOutput.Add("P:UZET", "    Upper Zone")
                lConstituentsToOutput.Add("P:LZET", "    Lower Zone")
                lConstituentsToOutput.Add("P:AGWET", "    Grnd Water")
                lConstituentsToOutput.Add("P:BASET", "    Baseflow")
                lConstituentsToOutput.Add("P:TAET", "    Total")
                lConstituentsToOutput.Add("R:Header0", "Flow")
                lConstituentsToOutput.Add("R:ROVOL", "    OutVolume")
                lConstituentsToOutput.Add("R:ROVOL1", "    OVolPtIn")
                lConstituentsToOutput.Add("R:ROVOL1-1", "    OVolPtInX1")
                lConstituentsToOutput.Add("R:ROVOL1-2", "    OVolPtInX2")
                lConstituentsToOutput.Add("R:ROVOL2", "    OVolNPIn")
                lConstituentsToOutput.Add("R:ROVOL2-1", "    OVolNPInX1")
                lConstituentsToOutput.Add("R:ROVOL2-2", "    OVolNPInX2")
                lConstituentsToOutput.Add("R:ROVOL3", "    OVolPmpIn")
                lConstituentsToOutput.Add("R:ROVOL3-1", "    OVolPmpInX1")
                lConstituentsToOutput.Add("R:ROVOL3-2", "    OVolPmpInX2")
                lConstituentsToOutput.Add("R:ROVOL4", "    OVolRsvIn")
                lConstituentsToOutput.Add("R:ROVOL4-1", "    OVolRsvInX1")
                lConstituentsToOutput.Add("R:ROVOL4-2", "    OVolRsvInX2")
                lConstituentsToOutput.Add("R:PRSUPY", "    SurfPrecVol")
                lConstituentsToOutput.Add("R:VOLEV", "    SurfEvapVol")
            Case "SedimentCopper"
                lConstituentsToOutput.Add("I:SOSLD", "Solids   ")
                lConstituentsToOutput.Add("I:SOQUAL-Copper", "Copper   ")
                lConstituentsToOutput.Add("I:SOQS-Copper", "Sed-Assoc Cu")
                lConstituentsToOutput.Add("I:SOQO-Copper", "Flow-Assoc Cu")
                lConstituentsToOutput.Add("P:SOSED", "Sediment")
                lConstituentsToOutput.Add("P:SOQUAL-Copper", "  Surface Cu")
                lConstituentsToOutput.Add("P:IOQUAL-Copper", "  Interflow Cu")
                lConstituentsToOutput.Add("P:AOQUAL-Copper", "  Groundwater Cu")
                lConstituentsToOutput.Add("P:SOQS-Copper", "Sed-Assoc Cu")
                lConstituentsToOutput.Add("P:SOQO-Copper", "Flow-Assoc Cu")
                lConstituentsToOutput.Add("P:POQUAL-Copper", "Total Cu")
                lConstituentsToOutput.Add("R:ROSED-SAND", "  Sand")
                lConstituentsToOutput.Add("R:ROSED-SILT", "  Silt")
                lConstituentsToOutput.Add("R:ROSED-CLAY", "  Clay")
                lConstituentsToOutput.Add("R:ROSED-TOT", "Total Sediment")
                lConstituentsToOutput.Add("R:Copper-RODQAL", "Disolved Cu")
                lConstituentsToOutput.Add("R:Copper-ROSQAL-SAND", "  Sand Cu")
                lConstituentsToOutput.Add("R:Copper-ROSQAL-SILT", "  Silt Cu")
                lConstituentsToOutput.Add("R:Copper-ROSQAL-CLAY", "  Clay Cu")
                lConstituentsToOutput.Add("R:Copper-ROSQAL-Tot", "Total Sediment Cu")
                lConstituentsToOutput.Add("R:Copper-TROQAL", "Total Cu")
            Case "NfromPQUAL"
                lConstituentsToOutput.Add("P:Header1", "NH4 (lb/ac)")
                lConstituentsToOutput.Add("P:WASHQS-NH4", "WASHQS")
                lConstituentsToOutput.Add("P:SCRQS-NH4", "SCRQS")
                lConstituentsToOutput.Add("P:SOQS-NH4", "SOQS")
                lConstituentsToOutput.Add("P:SOQO-NH4", "SOQO")
                lConstituentsToOutput.Add("P:SOQUAL-NH4", "SOQUAL")
                lConstituentsToOutput.Add("P:IOQUAL-NH4", "IOQUAL")
                lConstituentsToOutput.Add("P:AOQUAL-NH4", "AOQUAL")
                lConstituentsToOutput.Add("P:POQUAL-NH4", "POQUAL")
                lConstituentsToOutput.Add("P:Header2", "NO3 (lb/ac)")
                lConstituentsToOutput.Add("P:WASHQS-NO3", "WASHQS")
                lConstituentsToOutput.Add("P:SCRQS-NO3", "SCRQS")
                lConstituentsToOutput.Add("P:SOQS-NO3", "SOQS")
                lConstituentsToOutput.Add("P:SOQO-NO3", "SOQO")
                lConstituentsToOutput.Add("P:SOQUAL-NO3", "SOQUAL")
                lConstituentsToOutput.Add("P:IOQUAL-NO3", "IOQUAL")
                lConstituentsToOutput.Add("P:AOQUAL-NO3", "AOQUAL")
                lConstituentsToOutput.Add("P:POQUAL-NO3", "POQUAL")
            Case "TotalN"
                lConstituentsToOutput.Add("P:Header1", "Atmospheric Deposition (lb/ac)")
                lConstituentsToOutput.Add("P:NH4-N - SURFACE LAYER - TOTAL AD", "NH4-N - Surface Layer")
                lConstituentsToOutput.Add("P:NH4-N - UPPER LAYER - TOTAL AD", "NH4-N - Upper Layer")
                lConstituentsToOutput.Add("P:NO3-N - SURFACE LAYER - TOTAL AD", "NO3-N - Surface Layer")
                lConstituentsToOutput.Add("P:NO3-N - UPPER LAYER - TOTAL AD", "NO3-N - Upper Layer")
                lConstituentsToOutput.Add("P:ORGN - SURFACE LAYER - TOTAL AD", "ORGN - Surface Layer")
                lConstituentsToOutput.Add("P:ORGN - UPPER LAYER - TOTAL AD", "ORGN - Upper Layer")

                lConstituentsToOutput.Add("P:", "")
                lConstituentsToOutput.Add("P:TOTAL NITROGEN APPLICATION", "N Application (lb/ac)")

                lConstituentsToOutput.Add("P:Header1", "Nutrient Loss (lb/ac)")
                lConstituentsToOutput.Add("P:Header2", "NO3 Loss")
                lConstituentsToOutput.Add("P:NO3+NO2-N - SURFACE LAYER OUTFLOW", "Surface")
                lConstituentsToOutput.Add("P:NO3+NO2-N - UPPER LAYER OUTFLOW", "Interflow")
                lConstituentsToOutput.Add("P:NO3+NO2-N - GROUNDWATER OUTFLOW", "Baseflow")
                lConstituentsToOutput.Add("P:NO3-N - TOTAL OUTFLOW", "Total")
                lConstituentsToOutput.Add("P:Header2", "NH3 Loss")
                lConstituentsToOutput.Add("P:NH4-N IN SOLUTION - SURFACE LAYER OUTFLOW", "Surface")
                lConstituentsToOutput.Add("P:NH4-N IN SOLUTION - UPPER LAYER OUTFLOW", "Interflow")
                lConstituentsToOutput.Add("P:NH4-N IN SOLUTION - GROUNDWATER OUTFLOW", "Baseflow")
                lConstituentsToOutput.Add("P:NH4-N ADS - SEDIMENT ASSOC OUTFLOW", "Sediment")
                lConstituentsToOutput.Add("P:NH4-N - TOTAL OUTFLOW", "Total")
                lConstituentsToOutput.Add("P:Header2", "ORGN")
                lConstituentsToOutput.Add("P:LABILE ORGN - SEDIMENT ASSOC OUTFLOW", "Sediment Labile")
                lConstituentsToOutput.Add("P:REFRAC ORGN - SEDIMENT ASSOC OUTFLOW", "Sediment Refrac")
                lConstituentsToOutput.Add("P:ORGN - TOTAL OUTFLOW", "Total")

                lConstituentsToOutput.Add("P:", "")
                lConstituentsToOutput.Add("P:NITROGEN - TOTAL OUTFLOW", "Total N Loss (lb/ac)")

                lConstituentsToOutput.Add("P:Header1", "Below Ground Plant N Return (lb/ac)")
                lConstituentsToOutput.Add("P:TRTLBN", "To Labile ORGN")
                lConstituentsToOutput.Add("P:TRTRBN", "To Refrac ORGN")

                lConstituentsToOutput.Add("P:Header1", "Plant Uptake (lb/ac)")
                lConstituentsToOutput.Add("P:Header2", "NH3")
                lConstituentsToOutput.Add("P:SAMUPB", "Surface")
                lConstituentsToOutput.Add("P:UAMUPB", "Upper Zone")
                lConstituentsToOutput.Add("P:LAMUPB", "Lower Zone")
                lConstituentsToOutput.Add("P:AAMUPB", "Active GW")
                lConstituentsToOutput.Add("P:TAMUPB", "Total")
                lConstituentsToOutput.Add("P:Header2", "NO3")
                lConstituentsToOutput.Add("P:SNIUPB", "Surface")
                lConstituentsToOutput.Add("P:UNIUPB", "Upper Zone")
                lConstituentsToOutput.Add("P:LNIUPB", "Lower Zone")
                lConstituentsToOutput.Add("P:ANIUPB", "Active GW")
                lConstituentsToOutput.Add("P:TNIUPB", "Total")

                lConstituentsToOutput.Add("P:Header1", "Other Fluxes (lb/ac)")
                lConstituentsToOutput.Add("P:TDENI", "Denitrification")
                lConstituentsToOutput.Add("P:TAMNIT", "NH3 Nitrification")
                lConstituentsToOutput.Add("P:TAMIMB", "NH3 Immobilization")
                lConstituentsToOutput.Add("P:TORNMN", "ORGN Mineralization")
                lConstituentsToOutput.Add("P:TNIIMB", "NO3 Immobilization")
                lConstituentsToOutput.Add("P:TREFON", "Labile/Refr ORGN Conversion")
                lConstituentsToOutput.Add("P:TFIXN", "Nitrogen Fixation")
                lConstituentsToOutput.Add("P:TAMVOL", "NH3 Volatilization")
                'lConstituentsToOutput.Add("R:Header1", "TAM")
                'lConstituentsToOutput.Add("R:TAM-INTOT", "  TAM-INTOT")
                'lConstituentsToOutput.Add("R:TAM-INDIS", "  TAM-INDIS")
                'lConstituentsToOutput.Add("R:NH4-INPART-TOT", "  NH4-INPART-TOT")
                'lConstituentsToOutput.Add("R:TAM-OUTTOT", "  TAM-OUTTOT")
                'lConstituentsToOutput.Add("R:TAM-OUTDIS", "  TAM-OUTDIS")
                'lConstituentsToOutput.Add("R:TAM-OUTPART-TOT", "  TAM-OUTPART-TOT")
                'lConstituentsToOutput.Add("R:TAM-OUTTOT-EXIT3", "  TAM-OUTTOT-EXIT3")
                'lConstituentsToOutput.Add("R:TAM-OUTDIS-EXIT3", "  TAM-OUTDIS-EXIT3")
                'lConstituentsToOutput.Add("R:TAM-OUTPART-TOT-EXIT3", "  TAM-OUTPART-TOT-EXIT3")
                'lConstituentsToOutput.Add("R:Header2", "NO3")
                'lConstituentsToOutput.Add("R:NO3-INTOT", "  NO3-INTOT")
                'lConstituentsToOutput.Add("R:NO3-PROCFLUX-TOT", "  NO3-PROCFLUX-TOT")
                'lConstituentsToOutput.Add("R:NO3-OUTTOT", "  NO3-OUTTOT")
                'lConstituentsToOutput.Add("R:NO3-OUTTOT-EXIT3", "  NO3-OUTTOT-EXIT3")
                lConstituentsToOutput.Add("R:Header3", "Totals")
                lConstituentsToOutput.Add("R:N-TOT-IN", "  N-TOT-IN")
                lConstituentsToOutput.Add("R:N-TOT-OUT", "  N-TOT-OUT")
                lConstituentsToOutput.Add("R:N-TOT-OUT-EXIT1", "  N-TOT-OUT-EXIT1")
                lConstituentsToOutput.Add("R:N-TOT-OUT-EXIT2", "  N-TOT-OUT-EXIT2")
                lConstituentsToOutput.Add("R:N-TOT-OUT-EXIT3", "  N-TOT-OUT-EXIT3")
            Case "TotalP"
        End Select
        Return lConstituentsToOutput
    End Function

    Public Function LandUses(ByVal aUci As HspfUci, ByVal aOperationTypes As atcCollection, Optional ByVal aOutletLocation As String = "") As atcCollection
        Dim lLocations As New atcCollection
        If aOutletLocation.Length > 0 Then
            lLocations = UpstreamLocations(aUci, aOperationTypes, aOutletLocation)
        End If
        Dim lLandUses As New atcCollection
        Dim lOperations As atcCollection
        For lOperationIndex As Integer = 0 To aUci.OpnSeqBlock.Opns.Count - 1
            Dim lOperation As atcUCI.HspfOperation = aUci.OpnSeqBlock.Opns(lOperationIndex)
            Dim lLocationKey As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
            If lLocations.Count = 0 OrElse lLocations.IndexFromKey(lLocationKey) >= 0 Then
                Dim lLandUse As String = lOperation.Name.Substring(0, 1) & ":"
                Dim lDecsriptonParts() As String = lOperation.Description.Split(" ")
                For lIndex As Integer = 0 To lDecsriptonParts.GetUpperBound(0)
                    If Not IsNumeric(lDecsriptonParts(lIndex)) Then
                        lLandUse &= lDecsriptonParts(lIndex) & " "
                    End If
                Next
                lLandUse = lLandUse.Trim(" ")
                Dim lOperationKey As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
                Dim lOperationArea As Double = 0.0
                If aOutletLocation.Length > 0 Then
                    lOperationArea = lLocations.ItemByKey(lLocationKey)
                End If
                Dim lLandUseKey As Integer = lLandUses.IndexFromKey(lLandUse)
                If lLandUseKey = -1 Then
                    lOperations = New atcCollection
                    lOperations.Add(lOperationKey, lOperationArea)
                    lLandUses.Add(lLandUse, lOperations)
                Else
                    lOperations = lLandUses.Item(lLandUseKey)
                    lOperations.Add(lOperationKey, lOperationArea)
                End If
            End If
        Next
        Return lLandUses
    End Function

    Public Function UpstreamLocations(ByVal aUci As HspfUci, ByVal aOperationTypes As atcCollection, ByVal aLocation As String) As atcCollection
        Dim lLocations As New atcCollection 'key-location, value-total area
        UpstreamLocationAreaCalc(aUci, aLocation, aOperationTypes, lLocations)
        Return lLocations
    End Function

    Private Sub UpstreamLocationAreaCalc(ByVal aUci As HspfUci, _
                                         ByVal aLocation As String, _
                                         ByVal aOperationTypes As atcCollection, _
                                         ByRef aLocations As atcCollection)
        Dim lOperName As String = aOperationTypes.ItemByKey(aLocation.Substring(0, 2))
        Dim lOperation As HspfOperation = aUci.OpnBlks(lOperName).operfromid(aLocation.Substring(2))
        If Not lOperation Is Nothing Then
            For Each lConnection As HspfConnection In lOperation.Sources
                Dim lSourceVolName As String = lConnection.Source.VolName
                Dim lLocationKey As String = lSourceVolName.Substring(0, 1) & ":" & lConnection.Source.VolId
                If lSourceVolName = "PERLND" Or lSourceVolName = "IMPLND" Then
                    If lConnection.MFact > 0 Then
                        aLocations.Increment(lLocationKey, lConnection.MFact)
                    End If
                ElseIf lSourceVolName = "RCHRES" Then
                    UpstreamLocationAreaCalc(aUci, lLocationKey, aOperationTypes, aLocations)
                End If
            Next
        End If
        aLocations.Add(aLocation, 1.0)
    End Sub

    Public Function InchesToCfs(ByVal aTSerIn As atcTimeseries, ByVal aArea As Double) As atcTimeseries
        Dim lSimConv As Double = aArea * 43560.0# / (12.0# * 24.0# * 3600.0#) 'inches to cfs days
        Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
        Dim lArgsMath As New atcDataAttributes
        lArgsMath.SetValue("timeseries", aTSerIn)
        lArgsMath.SetValue("number", lSimConv)
        lTsMath.Open("multiply", lArgsMath)
        Return lTsMath.DataSets(0)
    End Function
End Module
