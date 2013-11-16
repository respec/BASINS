Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcUCI

Public Module Utility

    Public Function ConstituentsThatUseLast() As Generic.List(Of String)
        Static pConstituentsThatUseLast As Generic.List(Of String) = Nothing
        If pConstituentsThatUseLast Is Nothing Then
            Dim lConstituentsThatUseLastArray() As String = _
                {"BEDDEP", "RSED-BED-SAND", "RSED-BED-SILT", "RSED-BED-CLAY", "RSED-BED-TOT", "DETS", "SLDS", _
                "ABOVE-GROUND PLANT STORAGE", _
                "LITTER STORAGE", _
                "PLANT N - SURFACE LAYER STORAGE", _
                "PLANT N - UPPER PRINCIPAL STORAGE", _
                "PLANT N - LOWER LAYER STORAGE", _
                "PLANT N - ACTIVE GROUNDWATER STORAGE", _
                "PLANT N - TOTAL STORAGE", _
                "TOTAL N - TOTAL STORAGE", _
                "NH4-N SOL - SURFACE LAYER STORAGE", _
                "NH4-N SOL - UPPER PRINCIPAL STORAGE", _
                "NH4-N SOL - UPPER TRANSITORY STORAGE", _
                "NH4-N SOL - LOWER LAYER STORAGE", _
                "NH4-N SOL - ACTIVE GROUNDWATER STORAGE", _
                "NH4-N SOL - TOTAL STORAGE", _
                "NH4-N ADS - SURFACE LAYER STORAGE", _
                "NH4-N ADS - UPPER PRINCIPAL STORAGE", _
                "NH4-N ADS - UPPER TRANSITORY STORAGE", _
                "NH4-N ADS - LOWER LAYER STORAGE", _
                "NH4-N ADS - ACTIVE GROUNDWATER STORAGE", _
                "NH4-N ADS - TOTAL STORAGE", _
                "NO3/2-N - SURFACE LAYER STORAGE", _
                "NO3/2-N - UPPER PRINCIPAL STORAGE", _
                "NO3/2-N - UPPER TRANSITORY STORAGE", _
                "NO3/2-N - LOWER LAYER STORAGE", _
                "NO3/2-N - ACTIVE GROUNDWATER STORAGE", _
                "NO3/2-N - TOTAL STORAGE", _
                "SOL LABIL ORGANIC N - SURFACE LAYER STORAGE", _
                "SOL LABIL ORGANIC N - UPPER PRINCIPAL STORAGE", _
                "SOL LABIL ORGANIC N - UPPER TRANSITORY STORAGE", _
                "SOL LABIL ORGANIC N - LOWER LAYER STORAGE", _
                "SOL LABIL ORGANIC N - ACTIVE GROUNDWATER STORAGE", _
                "SOL LABIL ORGANIC N - TOTAL STORAGE", _
                "ADS LABIL ORGANIC N - SURFACE LAYER STORAGE", _
                "ADS LABIL ORGANIC N - UPPER PRINCIPAL STORAGE", _
                "ADS LABIL ORGANIC N - LOWER LAYER STORAGE", _
                "ADS LABIL ORGANIC N - ACTIVE GROUNDWATER STORAGE", _
                "ADS LABIL ORGANIC N - TOTAL STORAGE", _
                "SOL REFR ORGANIC N - SURFACE LAYER STORAGE", _
                "SOL REFR ORGANIC N - UPPER PRINCIPAL STORAGE", _
                "SOL REFR ORGANIC N - UPPER TRANSITORY STORAGE", _
                "SOL REFR ORGANIC N - LOWER LAYER STORAGE", _
                "SOL REFR ORGANIC N - ACTIVE GROUNDWATER STORAGE", _
                "SOL REFR ORGANIC N - TOTAL STORAGE", _
                "ADS REFR ORGANIC N - SURFACE LAYER STORAGE", _
                "ADS REFR ORGANIC N - UPPER PRINCIPAL STORAGE", _
                "ADS REFR ORGANIC N - LOWER LAYER STORAGE", _
                "ADS REFR ORGANIC N - ACTIVE GROUNDWATER STORAGE", _
                "ADS REFR ORGANIC N - TOTAL STORAGE"}
            pConstituentsThatUseLast = New Generic.List(Of String)(lConstituentsThatUseLastArray)
        End If
        Return pConstituentsThatUseLast
    End Function

    Friend Function ConstituentsToOutput(ByVal aType As String, _
                                Optional ByVal aCategory As Boolean = False) As atcCollection
        Dim lConstituentsToOutput As New atcCollection
        Select Case aType
            Case "Water"
            With lConstituentsToOutput
                      .Add("I:Header0", "Influx")
                      .Add("I:SUPY", "    Rainfall")
                      .Add("I:Header1", "Runoff")
                      .Add("I:SURO", "    Surface ")
                      .Add("I:Header2", "Evaporation")
                      .Add("I:PET", "    Potential")
                      .Add("I:IMPEV", "    Actual  ")
                      .Add("P:Header0", "Influx")
                      .Add("P:SUPY", "    Rainfall")
                      .Add("P:SURLI", "    Irrigation")
                      .Add("P:Header1", "Runoff")
                      .Add("P:SURO", "    Surface  ")
                      .Add("P:IFWO", "    Interflow")
                      .Add("P:AGWO", "    Baseflow ")
                      .Add("P:PERO", "    Total    ")
                      .Add("P:Header2", "GW Inflow")
                      .Add("P:IGWI", "    Deep    ")
                      .Add("P:AGWI", "    Active  ")
                    '.Add("P:Total2", "    Total   ") 'need total of prev 2 rows
                      .Add("P:AGWLI", "    Pumping")
                      .Add("P:Header3", "Evaporation")
                      .Add("P:PET", "    Potential")
                      .Add("P:CEPE", "    Intercep St")
                      .Add("P:UZET", "    Upper Zone")
                      .Add("P:LZET", "    Lower Zone")
                      .Add("P:AGWET", "    Grnd Water")
                      .Add("P:BASET", "    Baseflow")
                      .Add("P:TAET", "    Total   ")
                      .Add("R:Header0", "Flow")
                      .Add("R:ROVOL", "    OutVolume")
                If aCategory Then 'user used categories to indicate where water came from
                     .Add("R:ROVOL1", "    OVolPtIn")
                     .Add("R:ROVOL1-1", "    OVolPtInX1")
                     .Add("R:ROVOL1-2", "    OVolPtInX2")
                     .Add("R:ROVOL2", "    OVolNPIn")
                     .Add("R:ROVOL2-1", "    OVolNPInX1")
                     .Add("R:ROVOL2-2", "    OVolNPInX2")
                     .Add("R:ROVOL3", "    OVolPmpIn")
                     .Add("R:ROVOL3-1", "    OVolPmpInX1")
                     .Add("R:ROVOL3-2", "    OVolPmpInX2")
                     .Add("R:ROVOL4", "    OVolRsvIn")
                     .Add("R:ROVOL4-1", "    OVolRsvInX1")
                     .Add("R:ROVOL4-2", "    OVolRsvInX2")
                Else
                     .Add("R:IVOL", "     InVolume")
                End If
                     .Add("R:PRSUPY", "    SurfPrecVol")
                     .Add("R:VOLEV", "    SurfEvapVol")
              End With
            Case "Sediment"
                With lConstituentsToOutput
                     .Add("I:Header0", "Storage(tons/acre)")
                     .Add("I:SLDS", "  All")
                     .Add("I:Header1", "Washoff(tons/acre)")
                     .Add("I:SOSLD", "  Total")
                     
                     .Add("P:Header0", "Storage (tons/acre)")
                     .Add("P:DETS", "  Detached ")
                     .Add("P:Header1", "Washoff (tons/acre)")
                     .Add("P:SOSED", "  Total")
                     
                     .Add("R:BEDDEP", "Bed Depth (ft)")
                     .Add("R:Header0", "Bed Storage (tons)")
                     .Add("R:RSED-BED-SAND", "  Sand")
                     .Add("R:RSED-BED-SILT", "  Silt")
                     .Add("R:RSED-BED-CLAY", "  Clay")
                     .Add("R:RSED-BED-TOT", "  Total")
                     .Add("R:Header1", "Inflow (tons)")
                     .Add("R:ISED-SAND", "  Sand")
                     .Add("R:ISED-SILT", "  Silt")
                     .Add("R:ISED-CLAY", "  Clay")
                     .Add("R:ISED-TOT", "  Total")
                     .Add("R:Header2", "Dep(+)/Scour(-) (tons)")
                     .Add("R:DEPSCOUR-SAND", "  Sand")
                     .Add("R:DEPSCOUR-SILT", "  Silt")
                     .Add("R:DEPSCOUR-CLAY", "  Clay")
                     .Add("R:DEPSCOUR-TOT", "  Total")
                     .Add("R:Header3", "Outflow (tons)")
                     .Add("R:ROSED-SAND", "  Sand")
                     .Add("R:ROSED-SILT", "  Silt")
                     .Add("R:ROSED-CLAY", "  Clay")
                     .Add("R:ROSED-TOT", "  Total")
            End With
            Case "SedimentCopper"
            With lConstituentsToOutput
                     .Add("I:SOSLD", "Solids   ")
                     .Add("I:SOQUAL-Copper", "Copper   ")
                     .Add("I:SOQS-Copper", "Sed-Assoc Cu")
                     .Add("I:SOQO-Copper", "Flow-Assoc Cu")
                     .Add("P:SOSED", "Sediment")
                     .Add("P:SOQUAL-Copper", "  Surface Cu")
                     .Add("P:IOQUAL-Copper", "  Interflow Cu")
                     .Add("P:AOQUAL-Copper", "  Groundwater Cu")
                     .Add("P:SOQS-Copper", "Sed-Assoc Cu")
                     .Add("P:SOQO-Copper", "Flow-Assoc Cu")
                     .Add("P:POQUAL-Copper", "Total Cu")
                     .Add("R:ROSED-SAND", "  Sand")
                     .Add("R:ROSED-SILT", "  Silt")
                     .Add("R:ROSED-CLAY", "  Clay")
                     .Add("R:ROSED-TOT", "Total Sediment")
                     .Add("R:Copper-RODQAL", "Disolved Cu")
                     .Add("R:Copper-ROSQAL-SAND", "  Sand Cu")
                     .Add("R:Copper-ROSQAL-SILT", "  Silt Cu")
                     .Add("R:Copper-ROSQAL-CLAY", "  Clay Cu")
                     .Add("R:Copper-ROSQAL-Tot", "Total Sediment Cu")
                     .Add("R:Copper-TROQAL", "Total Cu")
                   End With
            Case "FColi"
            With lConstituentsToOutput
                      .Add("P:Header1", "Perv Load")
                      .Add("P:SOQUAL-F.Coliform", "Surface")
                      .Add("P:IOQUAL-F.Coliform", "Interflow")
                      .Add("P:AOQUAL-F.Coliform", "Baseflow")
                      .Add("P:POQUAL-F.Coliform", "Total")
                      .Add("I:Header2", "Imperv Load")
                      .Add("I:SOQUAL-F.Coliform", "ImpervSurf")
                      .Add("R:Header3", "Instream")
                      .Add("R:F.Coliform-TIQAL", "  Inflow")
                      .Add("R:F.Coliform-DDQAL-TOT", "  Decay")
                      .Add("R:F.Coliform-TROQAL", "  Outflow")
            End With
            Case "N-PQUAL"
            With lConstituentsToOutput
                                 
                    .Add("P:Header1", "NO3 (lb/ac)")
                    .Add("P:SOQUAL-NO3", "  Surface Flow")
                    .Add("P:IOQUAL-NO3", "  Interflow")
                    .Add("P:AOQUAL-NO3", "  Groundwater Flow")
                    .Add("P:POQUAL-NO3", "  Total")
                    .Add("P:Header2", "NH3+NH4 (lb/ac)")
                    .Add("P:SOQUAL-NH3+NH4", "  Surface Flow")
                    .Add("P:IOQUAL-NH3+NH4", "  Interflow")
                    .Add("P:AOQUAL-NH3+NH4", "  Groundwater Flow")
                    .Add("P:POQUAL-NH3+NH4", "  Total")
                    .Add("I:Header3", "NO3 (lb/ac)")
                    .Add("I:SOQUAL-NO3", "  Surface Flow")
                    .Add("I:Header4", "NH3+NH4 (lb/ac)")
                    .Add("I:SOQUAL-NH3+NH4", "  Surface Flow")
                    .Add("R:Header5", "Totals")
                    .Add("R:N-TOT-IN", "  Total N Inflow")
                    .Add("R:N-TOT-OUT", "  Total N Outflow")
            End With
            Case "P-PQUAL"
            With lConstituentsToOutput
                    .Add("P:Header1", "Ortho P (lb/ac)")
                    .Add("P:WASHQS-ORTHO P", "  Sediment Attached")
                    .Add("P:SOQUAL-ORTHO P", "  Surface Flow")
                    .Add("P:IOQUAL-ORTHO P", "  Interflow")
                    .Add("P:AOQUAL-ORTHO P", "  Groundwater Flow")
                    .Add("P:POQUAL-ORTHO P", "  Total")
                    .Add("I:Header2", "Ortho P (lb/ac)")
                    .Add("I:SOQUAL-ORTHO P", "  Surface Flow")
                    .Add("R:Header3", "Total P")
                    .Add("R:P-TOT-IN", "  Total P Inflow")
                    .Add("R:P-TOT-OUT", "  Total P Outflow")
            End With
            Case "TotalN" 'use PQUAL
                With lConstituentsToOutput
                    .Add("P:Header1", "Nitrogen Loss (lb/ac)")
                    .Add("P:Header1a", "  NO3 Loss")
                    .Add("P:NO3+NO2-N - SURFACE LAYER OUTFLOW", "    Surface")
                    .Add("P:NO3+NO2-N - UPPER LAYER OUTFLOW", "    Interflow")
                    .Add("P:NO3+NO2-N - GROUNDWATER OUTFLOW", "    Baseflow")
                    .Add("P:NO3-N - TOTAL OUTFLOW", "    Total")
                    .Add("P:Header1b", "  NH3 Loss")
                    .Add("P:NH4-N IN SOLUTION - SURFACE LAYER OUTFLOW", "    Surface")
                    .Add("P:NH4-N IN SOLUTION - UPPER LAYER OUTFLOW", "    Interflow")
                    .Add("P:NH4-N IN SOLUTION - GROUNDWATER OUTFLOW", "    Baseflow")
                    .Add("P:NH4-N ADS - SEDIMENT ASSOC OUTFLOW", "    Sediment")
                    .Add("P:NH4-N - TOTAL OUTFLOW", "    Total")
                    .Add("P:Header1c", "  Labile ORGN Loss")
                    .Add("P:LABILE ORGN - SURFACE LAYER OUTFLOW", "    Surface")
                    .Add("P:LABILE ORGN - UPPER LAYER OUTFLOW", "    Interflow")
                    .Add("P:LABILE ORGN - GROUNDWATER OUTFLOW", "    Baseflow")
                    .Add("P:LABILE ORGN - SEDIMENT ASSOC OUTFLOW", "    Sediment")
                    .Add("P:Header1d", "  Refractory ORGN Loss")
                    .Add("P:REFRAC ORGN - SURFACE LAYER OUTFLOW", "    Surface")
                    .Add("P:REFRAC ORGN - UPPER LAYER OUTFLOW", "    Interflow")
                    .Add("P:REFRAC ORGN - GROUNDWATER OUTFLOW", "    Baseflow")
                    .Add("P:REFRAC ORGN - SEDIMENT ASSOC OUTFLOW", "    Sediment")
                    .Add("P:ORGN - TOTAL OUTFLOW", "  Total ORGN Loss")
                    .Add("P:NITROGEN - TOTAL OUTFLOW", "  Total N Loss")

                    .Add("P:Header2", "Nitrogen Storages (lb/ac)")
                    
                    .Add("P:Header2a", "  NH4-N Soln Storage")
                    .Add("P:NH4-N SOL - SURFACE LAYER STORAGE", "    Surface")
                    .Add("P:NH4-N SOL - UPPER PRINCIPAL STORAGE", "    Upper")
                    .Add("P:NH4-N SOL - UPPER TRANSITORY STORAGE", "    Interflow")
                    .Add("P:NH4-N SOL - LOWER LAYER STORAGE", "    Lower")
                    .Add("P:NH4-N SOL - ACTIVE GROUNDWATER STORAGE", "    Groundwater")
                    .Add("P:NH4-N SOL - TOTAL STORAGE", "    Total")
                    .Add("P:Header2b", "  NH4-N Ads Storage")
                    .Add("P:NH4-N ADS - SURFACE LAYER STORAGE", "    Surface")
                    .Add("P:NH4-N ADS - UPPER PRINCIPAL STORAGE", "    Upper")
                    .Add("P:NH4-N ADS - UPPER TRANSITORY STORAGE", "    Interflow")
                    .Add("P:NH4-N ADS - LOWER LAYER STORAGE", "    Lower")
                    .Add("P:NH4-N ADS - ACTIVE GROUNDWATER STORAGE", "    Groundwater")
                    .Add("P:NH4-N ADS - TOTAL STORAGE", "    Total")
                    .Add("P:Header2c", "  NO3/2-N Storage")
                    .Add("P:NO3/2-N - SURFACE LAYER STORAGE", "    Surface")
                    .Add("P:NO3/2-N - UPPER PRINCIPAL STORAGE", "    Upper")
                    .Add("P:NO3/2-N - UPPER TRANSITORY STORAGE", "    Interflow")
                    .Add("P:NO3/2-N - LOWER LAYER STORAGE", "    Lower")
                    .Add("P:NO3/2-N - ACTIVE GROUNDWATER STORAGE", "    Groundwater")
                    .Add("P:NO3/2-N - TOTAL STORAGE", "    Total")
                    .Add("P:Header2d", "  Labile ORGN Soln")
                    .Add("P:SOL LABIL ORGANIC N - SURFACE LAYER STORAGE", "    Surface")
                    .Add("P:SOL LABIL ORGANIC N - UPPER PRINCIPAL STORAGE", "    Upper")
                    .Add("P:SOL LABIL ORGANIC N - UPPER TRANSITORY STORAGE", "    Interflow")
                    .Add("P:SOL LABIL ORGANIC N - LOWER LAYER STORAGE", "    Lower")
                    .Add("P:SOL LABIL ORGANIC N - ACTIVE GROUNDWATER STORAGE", "    Groundwater")
                    .Add("P:SOL LABIL ORGANIC N - TOTAL STORAGE", "    Total")
                    .Add("P:Header2e", "  Labile ORGN Ads")
                    .Add("P:ADS LABIL ORGANIC N - SURFACE LAYER STORAGE", "    Surface")
                    .Add("P:ADS LABIL ORGANIC N - UPPER PRINCIPAL STORAGE", "    Upper")
                    .Add("P:ADS LABIL ORGANIC N - LOWER LAYER STORAGE", "    Lower")
                    .Add("P:ADS LABIL ORGANIC N - ACTIVE GROUNDWATER STORAGE", "    Groundwater")
                    .Add("P:ADS LABIL ORGANIC N - TOTAL STORAGE", "    Total")
                    .Add("P:Header2f", "  Refractory ORGN Soln")
                    .Add("P:SOL REFR ORGANIC N - SURFACE LAYER STORAGE", "    Surface")
                    .Add("P:SOL REFR ORGANIC N - UPPER PRINCIPAL STORAGE", "    Upper")
                    .Add("P:SOL REFR ORGANIC N - UPPER TRANSITORY STORAGE", "    Interflow")
                    .Add("P:SOL REFR ORGANIC N - LOWER LAYER STORAGE", "    Lower")
                    .Add("P:SOL REFR ORGANIC N - ACTIVE GROUNDWATER STORAGE", "    Groundwater")
                    .Add("P:SOL REFR ORGANIC N - TOTAL STORAGE", "    Total")
                    .Add("P:Header2g", "  Refractory ORGN Ads")
                    .Add("P:ADS REFR ORGANIC N - SURFACE LAYER STORAGE", "    Surface")
                    .Add("P:ADS REFR ORGANIC N - UPPER PRINCIPAL STORAGE", "    Upper")
                    .Add("P:ADS REFR ORGANIC N - LOWER LAYER STORAGE", "    Lower")
                    .Add("P:ADS REFR ORGANIC N - ACTIVE GROUNDWATER STORAGE", "    Groundwater")
                    .Add("P:ADS REFR ORGANIC N - TOTAL STORAGE", "    Total")
                    .Add("P:ABOVE-GROUND PLANT STORAGE", "  Above Ground Plant N")
                    .Add("P:LITTER STORAGE", "  Litter N")
                    .Add("P:Header2h", "  Below Ground Plant N")
                    .Add("P:PLANT N - SURFACE LAYER STORAGE", "    Surface")
                    .Add("P:PLANT N - UPPER PRINCIPAL STORAGE", "    Upper")
                    .Add("P:PLANT N - LOWER LAYER STORAGE", "    Lower")
                    .Add("P:PLANT N - ACTIVE GROUNDWATER STORAGE", "    Groundwater")
                    .Add("P:PLANT N - TOTAL STORAGE", "    Total AG, BG, Litter PLTN")
                    .Add("P:TOTAL N - TOTAL STORAGE", "    Total Soil, Litter, & Plant N")

                    .Add("P:Header3", "Nitrogen Fluxes")
                    .Add("P:Header3a", "  Atmospheric Deposition")
                    .Add("P:NO3-N - SURFACE LAYER - TOTAL AD", "    NO3-N - SURFACE")
                    .Add("P:NO3-N - UPPER LAYER - TOTAL AD", "    NO3-N - UPPER")
                    .Add("P:NH4-N - SURFACE LAYER - TOTAL AD", "    NH3-N - SURFACE")
                    .Add("P:NH4-N - UPPER LAYER - TOTAL AD", "    NH3-N - UPPER")
                    .Add("P:ORGN - SURFACE LAYER - TOTAL AD", "    ORGN - SURFACE")
                    .Add("P:ORGN - UPPER LAYER - TOTAL AD", "    ORGN - UPPER")
                    '.Add("P:add these up to get a total of each species ?", "")
                    .Add("P:Header3b", "  Applications (lb/a)")
                    .Add("P:NITRATE APPLICATION", "    NO3-N")
                    .Add("P:AMMONIA APPLICATION", "    NH3-N")
                    .Add("P:ORGANIC N APPLICATION", "    ORGN")
                    .Add("P:Header3c", "  Above Ground Plant Uptake")
                    .Add("P:TNIUPA", "    NO3-N")
                    .Add("P:TAMUPA", "    NH3-N")
                    .Add("P:Header3d", "  Below Ground Plant Uptake")
                    .Add("P:TNIUPB", "    NO3-N")
                    .Add("P:TAMUPB", "    NH3-N")
                    .Add("P:RETAGN", "  Above Gr Plant N to Litter")
                    .Add("P:Header3e", "  Litter N Return to Labile ORGN")
                    .Add("P:SRTLLN", "    Surface")
                    .Add("P:URTLLN", "    Upper")
                    .Add("P:TRTLLN", "    Total")
                    .Add("P:Header3f", "  Litter N Return to Refractory ORGN")
                    .Add("P:SRTRLN", "    Surface")
                    .Add("P:URTRLN", "    Upper")
                    .Add("P:TRTRLN", "    Total")
                    .Add("P:Header3g", "  BG Plant N Return to Labile ORGN")
                    .Add("P:SRTLBN", "    Surface")
                    .Add("P:URTLBN", "    Upper")
                    .Add("P:LRTLBN", "    Lower")
                    .Add("P:TRTLBN", "    Total")
                    .Add("P:Header3h", "  BG Plant N Return to Refractory ORGN")
                    .Add("P:SRTRBN", "    Surface")
                    .Add("P:URTRBN", "    Upper")
                    .Add("P:LRTRBN", "    Lower")
                    .Add("P:TRTRBN", "    Total")
                    .Add("P:TREFON", "  Labile/Refractory ORGN Conversion")
                    .Add("P:TORNMN", "  Labile ORGN Mineralization")
                    .Add("P:TDENI", "  Denitrification")
                    .Add("P:TAMNIT", "  NH3 Nitrification")
                    .Add("P:TAMIMB", "  NH3 Immobilization")
                    .Add("P:TNIIMB", "  NO3 Immobilization")

                    .Add("P:Header4", "NO3 (PQUAL)")
                    .Add("P:SOQUAL-NO3", "  Surface Flow")
                    .Add("P:IOQUAL-NO3", "  Interflow")        
                    .Add("P:AOQUAL-NO3", "  Groundwater Flow") 
                    .Add("P:POQUAL-NO3", "  Total")            

                    .Add("P:Header5", "NH3+NH4 (PQUAL)")
                    .Add("P:SOQUAL-NH3+NH4", "  Surface Flow")
                    .Add("P:IOQUAL-NH3+NH4", "  Interflow")
                    .Add("P:AOQUAL-NH3+NH4", "  Groundwater Flow")
                    .Add("P:POQUAL-NH3+NH4", "  Total")


                    .Add("P:Header6", "LabileOrgN(PQUAL)")
                    .Add("P:WASHQS-BOD2", "  Sediment Attached")
                    .Add("P:SOQUAL-BOD2", "  Surface Flow")     
                    .Add("P:IOQUAL-BOD2", "  Interflow")        
                    .Add("P:AOQUAL-BOD2", "  Groundwater Flow") 
                    .Add("P:POQUAL-BOD2", "  Total")            

                    .Add("P:Header7", "RefOrgN (PQUAL)")
                    .Add("P:WASHQS-BOD1", "  Sediment Attached")
                    .Add("P:SOQUAL-BOD1", "  Surface Flow")
                    .Add("P:IOQUAL-BOD1", "  Interflow")
                    .Add("P:AOQUAL-BOD1", "  Groundwater Flow")
                    .Add("P:POQUAL-BOD1", "  Total")

                    .Add("I:Header8", "NO3 (IQUAL)")
                    .Add("I:SOQUAL-NO3", "  Surface Flow")
                    .Add("I:Header9", "NH3+NH4 (IQUAL)")
                    .Add("I:SOQUAL-NH3+NH4", "  Surface Flow")
                    .Add("I:Header10", "LabileOrgN (IQUAL)")
                    .Add("I:SOQUAL-BOD2", "  Surface Flow")
                    .Add("I:Header11", "RefOrgN (IQUAL)")
                    .Add("I:SOQUAL-BOD1", "  Surface Flow")


                    .Add("R:Header12", "NO3 as N")
                    .Add("R:NO3-INTOT", "  Total NO3 Inflow")
                    .Add("R:NO3-PROCFLUX-TOT", "  NO3 Process Fluxes")
                    .Add("R:NO3-OUTTOT", "  Total NO3 Outflow")

                    .Add("R:Header13", "Total NH3 as N")
                    .Add("R:TAM-INTOT", "  Total NH3 Inflow")
                    .Add("R:TAM-INDIS", "  Dissolved NH3 inflow")
                    .Add("R:NH4-INPART-TOT", "  Particulate NH3 Inflow")
                    .Add("R:TAM-PROCFLUX-TOT", "  NH3 Process Fluxes")
                    .Add("R:TAM-OUTTOT", "  Total NH3 Outflow")
                    .Add("R:TAM-OUTDIS", "  Dissolved NH3 Outflow")
                    .Add("R:TAM-OUTPART-TOT", "  Particulate NH3 Outflow")

                    .Add("R:Header14", "Refractory-N")
                    .Add("R:N-REFORG-IN", "  Refr-N Inflow")
                    .Add("R:N-REFORG-TOTPROCFLUX-TOT", "  Refr-N Process Fluxes")
                    .Add("R:N-REFORG-OUT", "  Refr-N Outflow")

                    .Add("R:Header15", "Org-N")
                    .Add("R:N-TOTORG-IN", "  Total Org-N Inflow")
                    .Add("R:N-TOTORG-OUT", "  Total Org-N Outflow")

                    .Add("R:Header16", "Total N")
                    .Add("R:N-TOT-IN", "  Total N Inflow")
                    .Add("R:N-TOT-OUT", "  Total N Outflow")
              End With
                '.Add("R:TAM-OUTTOT-EXIT3", "  Total NH3 Outflow-Exit3")
                '.Add("R:TAM-OUTDIS-EXIT3", "  Dissolved NH3 Outflow-Exit3")
                '.Add("R:TAM-OUTPART-TOT-EXIT3", "  Particulate NH3 Outflow-Exit3")

                '.Add("R:NO3-OUTTOT-EXIT3", "  Total NO3 Outflow - Exit 3")

                '.Add("R:N-TOT-OUT-EXIT1", "  N-TOT-OUT-EXIT1")
                '.Add("R:N-TOT-OUT-EXIT2", "  N-TOT-OUT-EXIT2")
                '.Add("R:N-TOT-OUT-EXIT3", "  N-TOT-OUT-EXIT3")
            Case "TotalP"
                With lConstituentsToOutput
                    .Add("P:Header1", "Phosphorus Loss (lb/ac")
                    .Add("P:PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW", "    Surface")
                    .Add("P:PO4-P IN SOLUTION - INTERFLOW - OUTFLOW", "    Interflow")
                    .Add("P:PO4-P IN SOLUTION - GROUNDWATER - OUTFLOW", "    Baseflow")
                    .Add("P:SDP4A", "    Sediment")
                    .Add("P:POPO4", "    Total")
                    .Add("P:SDORP", "    ORGP Sediment")
                    .Add("P:POPHOS", "    Total P Loss")
                 
                    .Add("P:Header2", "Plant P")
                    .Add("P:PLANT P - SURFACE LAYER", "    Surface")
                    .Add("P:PLANT P - UPPER PRINCIPAL", "    Upper")
                    .Add("P:PLANT P - LOWER LAYER", "    Lower")
                    .Add("P:PLANT P - ACTIVE GROUNDWATER", "    Groundwater")
                    .Add("P:PLANT P - TOTALS", "    Total")
                    .Add("P:Header3a", "P Storages (lb/ac)")
                    .Add("P:Header3b", "  PO4-P Soln Storage")
                    .Add("P:PO4-P SOL - SURFACE LAYER", "    Surface")
                    .Add("P:PO4-P SOL - UPPER PRINCIPAL", "    Upper")
                    .Add("P:PO4-P SOL - UPPER TRANSITORY", "    Interflow")
                    .Add("P:PO4-P SOL - LOWER LAYER", "    Lower")
                    .Add("P:PO4-P SOL - ACTIVE GROUNDWATER", "    GW")
                    .Add("P:PO4-P SOL - TOTALS", "    Total")
                    .Add("P:Header4", "  PO4-P Ads Storage")
                    .Add("P:PO4-P ADS - SURFACE LAYER", "    Surface")
                    .Add("P:PO4-P ADS - UPPER PRINCIPAL", "    Upper")
                    .Add("P:PO4-P ADS - LOWER LAYER", "    Lower")
                    .Add("P:PO4-P ADS - ACTIVE GROUNDWATER", "    GW")
                    .Add("P:PO4-P ADS - TOTALS", "    Total")
                    .Add("P:Header5", "  ORGP Storage")
                    .Add("P:ORGANIC P - SURFACE LAYER", "    Surface")
                    .Add("P:ORGANIC P - UPPER PRINCIPAL", "    Upper")
                    .Add("P:ORGANIC P - LOWER LAYER", "    Lower")
                    .Add("P:ORGANIC P - ACTIVE GROUNDWATER", "    GW")
                    .Add("P:ORGANIC P - TOTALS", "    Total")
                     
                    .Add("P:Header6", "P FLUXES")
                    .Add("P:Header6a", "  Atmospheric Deposition (lb/a)")
                    .Add("P:PO4-P - SURFACELAYER - TOTAL", "    PO4-P - Surface")
                    .Add("P:ORG-P - SURFACELAYER -TOTAL", "    ORGP - Surface")
                    .Add("P:PO4-P - UPPER LAYER - TOTAL", "    PO4-P - Upper")
                    .Add("P:ORG-P - UPPER LAYER - TOTAL", "    ORGP - Upper")
                    '.Add("P:add these up to get a total of each species ?", "")
                    .Add("P:Header6b", "  Applications (lb/a)")
                    .Add("P:IPO4", "    PO4-P")
                    .Add("P:IORP", "    ORGP")
                    '.Add("P:Header6c", "  Plant Uptake")
                    '.Add("P:PLANT P - SURFACE LAYER", "    Surface")
                    '.Add("P:PLANT P - UPPER PRINCIPAL", "    Upper")
                    '.Add("P:PLANT P - LOWER LAYER", "    Lower")
                    '.Add("P:PLANT P - ACTIVE GROUNDWATER", "    GW")
                    '.Add("P:PLANT P - TOTALS", "    Total")
                    .Add("P:Header6c", "  Other Fluxes (lb/ac)")
                    .Add("P:TORPMN", "    ORGP Mineralization")
                    .Add("P:TP4IMB", "    PO4-P Immobilization")
                    
                    .Add("P:Header7", "Ortho P (PQUAL)")
                    .Add("P:SOQUAL-Ortho P", "  Surface Flow")
                    .Add("P:IOQUAL-Ortho P", "  Interflow")
                    .Add("P:AOQUAL-Ortho P", "  Groundwater Flow")
                    .Add("P:POQUAL-Ortho P", "  Total")

                    .Add("P:Header8", "RefOrgP (PQUAL)")
                    .Add("P:WASHQS-BOD1", "  Sediment Attached")
                    .Add("P:SOQUAL-BOD1", "  Surface Flow")    
                    .Add("P:IOQUAL-BOD1", "  Interflow")       
                    .Add("P:AOQUAL-BOD1", "  Groundwater Flow")
                    .Add("P:POQUAL-BOD1", "  Total")           

                    .Add("P:Header9", "LabileOrgP (PQUAL)")
                    .Add("P:WASHQS-BOD2", "  Sediment Attached")
                    .Add("P:SOQUAL-BOD2", "  Surface Flow")     
                    .Add("P:IOQUAL-BOD2", "  Interflow")        
                    .Add("P:AOQUAL-BOD2", "  Groundwater Flow") 
                    .Add("P:POQUAL-BOD2", "  Total")            

                    .Add("I:Header10", "Ortho P (IQUAL)")
                    .Add("I:SOQUAL-Ortho P", "  Surface Flow")

                    .Add("I:Header11", "RefOrgP (IQUAL)")
                    .Add("I:SOQUAL-BOD1", "  Surface Flow")

                    .Add("I:Header12", "LabileOrgP (IQUAL)")
                    .Add("I:SOQUAL-BOD2", "  Surface Flow")

                    .Add("R:Header13", "Total PO4 as P")
                    .Add("R:PO4-INTOT", "  Total PO4 Inflow")
                    .Add("R:PO4-INDIS", "  Dissolved PO4 Inflow")
                    .Add("R:PO4-INPART-TOT", "  Particulate PO4 Inflow")
                    .Add("R:PO4-PROCFLUX-TOT", "  PO4 Process Fluxes")
                    .Add("R:PO4-OUTTOT", "  Total PO4 Outflow")
                    .Add("R:PO4-OUTDIS", "  Dissolved PO4 Outflow")
                    .Add("R:PO4-OUTPART-TOT", "  Particulate PO4 Outflow")

                    .Add("R:Header14", "ORGN-P")
                    .Add("R:P-TOTORG-IN", "  Total Org-P Inflow")
                    .Add("R:P-TOTORG-OUT", "  Total Org-P Outflow")

                    .Add("R:Header15", "Refractory-P")
                    .Add("R:P-REFORG-IN", "  Refractory-P Inflow")
                    .Add("R:P-REFORG-TOTPROCFLUX-TOT", "  Refractory-P Process Fluxes")
                    .Add("R:P-REFORG-OUT", "  Refractory-P Outflow")

                    .Add("R:Header16", "Total P")
                    .Add("R:P-TOT-IN", "  Total P Inflow")
                    .Add("R:P-TOT-OUT", "  Total P Outflow")
                End With
            Case "BOD-PQUAL"
                With lConstituentsToOutput
                    .Add("P:Header1", "BOD")
                    .Add("P:WASHQS-BOD", "  WASHQS")
                    .Add("P:SOQUAL-BOD", "  SOQUAL")
                    .Add("P:IOQUAL-BOD", "  IOQUAL")
                    .Add("P:AOQUAL-BOD", "  AOQUAL")
                    .Add("P:POQUAL-BOD", "  POQUAL")
                    
                    .Add("I:Header2", "BOD")
                    .Add("I:SOQUAL-BOD", "  SOQUAL")
                    
                    .Add("R:Header3", "BOD")
                    .Add("R:BODIN", "  BODIN")
                    .Add("R:BODFLUX-BODDEC", "  BODFLUX-BODDEC")
                    .Add("R:BODFLUX-SINK", "  BODFLUX-SINK")
                    .Add("R:BODFLUX-BENTHAL", "  BODFLUX-BENTHAL")
                    .Add("R:BODFLUX-PHYTO", "  BODFLUX-PHYTO")
                    .Add("R:BODFLUX-ZOO", "  R:BODFLUX-ZOO")
                    .Add("R:BODFLUX-BENTHIC", "  BODFLUX-BENTHIC")
                    .Add("R:BODOUTTOT", "  BODOUTTOT")
            End With
        End Select
        Return lConstituentsToOutput
    End Function

    Public Function LandUses(ByVal aUci As HspfUci, _
                             ByVal aOperationTypes As atcCollection, _
                    Optional ByVal aOutletLocation As String = "") As atcCollection
        Dim lLocations As New atcCollection
        If aOutletLocation.Length > 0 Then
            lLocations = UpstreamLocations(aUci, aOperationTypes, aOutletLocation)
        End If
        Dim lLandUsesSortedById As New SortedList
        Dim lLandUses As New atcCollection
        Dim lOperations As atcCollection
        For lOperationIndex As Integer = 0 To aUci.OpnSeqBlock.Opns.Count - 1
            Dim lOperation As atcUCI.HspfOperation = aUci.OpnSeqBlock.Opns(lOperationIndex)
            If lOperation.Name = "PERLND" OrElse lOperation.Name = "IMPLND" OrElse lOperation.Name = "RCHRES" Then
                Dim lLocationKey As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
                If lLocations.Count = 0 OrElse lLocations.IndexFromKey(lLocationKey) >= 0 Then

                    Dim lLandUse As String = lOperation.Name.Substring(0, 1) & ":"
                    If lLandUse = "C:" Then Stop
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
                    Dim lLandUseKeyIndex As Integer = lLandUses.IndexFromKey(lLandUse)
                    If lLandUseKeyIndex = -1 Then
                        lOperations = New atcCollection
                        lOperations.Add(lOperationKey, lOperationArea) 'e.g. R:116 0.0
                        Dim lOperationId As String = lOperation.Id.ToString
                        If lOperationId.Length = 1 Then lOperationId = Format(lOperation.Id Mod 100, "00")
                        'Becky had a concern about this, but her code seems functionally the same
                        Dim lId As String = lLocationKey.Substring(0, 2) & lOperationId
                        If lLandUsesSortedById.IndexOfKey(lId) = -1 Then
                            lLandUsesSortedById.Add(lId, lLandUse)
                        End If
                        lLandUses.Add(lLandUse, lOperations)
                    Else
                        lOperations = lLandUses.Item(lLandUseKeyIndex)
                        lOperations.Add(lOperationKey, lOperationArea)
                        'For Debug
                        'lLandUses.Keys -> lLandUses.ItemByKey(lu).Keys -> "R:69"
                    End If
                End If
            End If
        Next
        Dim lLandUsesTemp As New atcCollection
        For Each lLandUseKey As String In lLandUsesSortedById.Keys
            Dim lKey As String = lLandUsesSortedById.Item(lLandUseKey)
            lLandUsesTemp.Add(lKey, lLandUses.ItemByKey(lKey))
        Next
        Return lLandUsesTemp
    End Function

    Public Function UpstreamLocations(ByVal aUci As HspfUci, _
                                      ByVal aOperationTypes As atcCollection, _
                                      ByVal aLocation As String) As atcCollection
        Dim lLocations As New atcCollection 'key-location, value-total area
        UpstreamLocationAreaCalc(aUci, aLocation, aOperationTypes, lLocations)
        Return lLocations
    End Function

    Private Sub UpstreamLocationAreaCalc(ByVal aUci As HspfUci, _
                                         ByVal aLocation As String, _
                                         ByVal aOperationTypes As atcCollection, _
                                         ByRef aLocations As atcCollection)
        LocationAreaCalc(aUci, aLocation, aOperationTypes, aLocations, True)
    End Sub

    Public Sub LocationAreaCalc(ByVal aUci As HspfUci, _
                                ByVal aLocation As String, _
                                ByVal aOperationTypes As atcCollection, _
                                ByRef aLocations As atcCollection, _
                                ByVal aUpstream As Boolean)
        Dim lOperName As String = aOperationTypes.ItemByKey(aLocation.Substring(0, 2))
        Dim lOperation As HspfOperation = aUci.OpnBlks(lOperName).OperFromID(aLocation.Substring(2))
        If Not lOperation Is Nothing Then
            Dim lUpstreamChecked As New atcCollection
            For Each lConnection As HspfConnection In lOperation.Sources
                Dim lSourceVolName As String = lConnection.Source.VolName
                Dim lLocationKey As String = lSourceVolName.Substring(0, 1) & ":" & lConnection.Source.VolId
                If lSourceVolName = "PERLND" Or lSourceVolName = "IMPLND" Then
                    If lConnection.MFact > 0 Then
                        aLocations.Increment(lLocationKey, lConnection.MFact)
                    End If
                ElseIf lSourceVolName = "RCHRES" Then
                    If aUpstream Then
                        If lUpstreamChecked.Contains(lLocationKey) Then
                            Logger.Dbg("SkipDuplicate:" & lLocationKey)
                        ElseIf aUci.Name.ToLower.Contains("scr") AndAlso _
                               lConnection.Source.VolId = 229 AndAlso _
                               lConnection.Target.VolId = 516 Then
                            'TODO: figure out a way not to hardcode this!
                            Logger.Dbg("Skip 229 to 516 in SantaClara")
                        Else
                            LocationAreaCalc(aUci, lLocationKey, aOperationTypes, aLocations, True)
                            lUpstreamChecked.Add(lLocationKey)
                        End If
                    Else
                        aLocations.Add(lLocationKey, 0)
                    End If
                End If
            Next
        End If

        'For Debug:
        'Detect if the final outlet is included
        'If aLocation = "R:69" Then
        aLocations.Add(aLocation, 0.0)
    End Sub

    'Compute total area of an implnd or perlnd by totaling all MFact of connections from it
    Friend Function OperationArea(ByVal aUci As HspfUci, ByVal aOperation As HspfOperation) As Double
        Dim lArea As Double = 0
        For Each lReach As HspfOperation In aUci.OpnBlks("RCHRES").Ids
            For Each lConnection As HspfConnection In lReach.Sources
                If Not lConnection.Source.Opn Is Nothing AndAlso lConnection.Source.Opn.Equals(aOperation) Then
                    lArea += lConnection.MFact
                End If
            Next
        Next
        Return lArea
    End Function

    Public Function CfsToInches(ByVal aTSerIn As atcTimeseries, _
                                ByVal aArea As Double) As atcTimeseries
        Dim lConversionFactor As Double = (12.0# * 24.0# * 3600.0#) / (aArea * 43560.0#)   'cfs days to inches
        Dim lTsMath As atcTimeseriesSource = New atcTimeseriesMath.atcTimeseriesMath
        Dim lArgsMath As New atcDataAttributes
        lArgsMath.SetValue("timeseries", aTSerIn)
        lArgsMath.SetValue("number", lConversionFactor)
        lTsMath.Open("multiply", lArgsMath)
        Return lTsMath.DataSets(0)
    End Function

    Public Function InchesToCfs(ByVal aTSerIn As atcTimeseries, _
                                ByVal aArea As Double) As atcTimeseries
        Dim lConversionFactor As Double = (aArea * 43560.0#) / (12.0# * 24.0# * 3600.0#) 'inches to cfs days
        Dim lInterval As Double = aTSerIn.Attributes.GetValue("interval", 1.0)
        lConversionFactor /= lInterval
        Dim lTsMath As atcTimeseriesSource = New atcTimeseriesMath.atcTimeseriesMath
        Dim lArgsMath As New atcDataAttributes
        lArgsMath.SetValue("timeseries", aTSerIn)
        lArgsMath.SetValue("number", lConversionFactor)
        lTsMath.Open("multiply", lArgsMath)
        Return lTsMath.DataSets(0)
    End Function

    Friend Sub CheckDateJ(ByVal aTSer As atcTimeseries, _
                          ByVal aName As String, _
                          ByRef aSDateJ As Double, _
                          ByRef aEDateJ As Double, _
                          ByRef aStr As String)
        Dim lDateTmp As Double = aTSer.Dates.Values(0)
        If aSDateJ < lDateTmp Then
            aStr &= "   Adjusted Start Date from " & Format(Date.FromOADate(aSDateJ), "yyyy/MM/dd") & _
                                             "to " & Format(Date.FromOADate(lDateTmp), "yyyy/MM/dd") & _
                                        " due to " & aName & vbCrLf & vbCrLf
            aSDateJ = lDateTmp
        End If
        lDateTmp = aTSer.Dates.Values(aTSer.numValues)
        If aEDateJ > lDateTmp Then
            aStr &= "   Adjusted End Date from " & Format(Date.FromOADate(aEDateJ), "yyyy/MM/dd") & _
                                          " to " & Format(Date.FromOADate(lDateTmp), "yyyy/MM/dd") & _
                                      " due to " & aName & vbCrLf & vbCrLf
            aEDateJ = lDateTmp
        End If
    End Sub

    Public Function AreaReport(ByVal aUci As HspfUci, ByVal aRunMade As String, _
                               ByVal aOperationTypes As atcCollection, ByVal aLocations As atcCollection, _
                               ByVal aLandUseReport As Boolean, ByVal aReportPath As String) As atcReport.IReport
        Dim lReport As New atcReport.ReportText
        lReport.AppendLine("Area Summary Report")
        lReport.AppendLine("   UCI File Name " & aUci.Name)
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("")

        lReport.AppendLine("Location" & vbTab & "TotalArea".PadLeft(12) & vbTab & "LocalArea".PadLeft(12) & vbTab & "UpstreamReaches")
        'Becky changed the following to cycle through all locations
        'Dim lLocation As String = aLocations.Item(aLocations.Count - 1)
        For Each lLocation As String In aLocations 'added by Becky - should force land use reports for ALL relevant reaches 
            'and also add the total for all relevant reaches to a single overall report
            lReport.AppendLine(AreaReportLocation(aUci, aOperationTypes, lLocation, True, aReportPath, aRunMade))
        Next lLocation
        Return lReport
    End Function

    Private Function AreaReportLocation(ByVal aUci As HspfUci, ByVal aOperationtypes As atcCollection, _
                                        ByVal aLocation As String, ByVal aLandUseReport As Boolean, _
                                        ByVal aReportPath As String, ByVal aRunMade As String) As String
        If aLandUseReport Then
            Dim lReport As New atcReport.ReportText
            lReport.AppendLine("LanduseArea Summary Report at " & aLocation)
            lReport.AppendLine("   UCI File Name " & aUci.Name)
            lReport.AppendLine("   Run Made " & aRunMade)
            lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
            lReport.AppendLine("")
            lReport.AppendLine("Landuse".PadLeft(20) & vbTab & _
                           "PervArea".PadLeft(12) & vbTab & _
                           "ImpvArea".PadLeft(12) & vbTab & _
                           "TotalArea".PadLeft(12))
            Dim lLandUses As atcCollection = LandUses(aUci, aOperationtypes, aLocation)
            Dim lLandUsesCombinePervImpv As atcCollection = LandUsesCombined(lLandUses)
            Dim lTotalAreaFromLandUses As Double = 0
            Dim lTotalAreaPerv As Double = 0.0
            Dim lTotalAreaImpr As Double = 0.0
            For lLandUseIndex As Integer = 0 To lLandUsesCombinePervImpv.Count - 1
                Dim lLandUseAreaString As String = lLandUsesCombinePervImpv.Item(lLandUseIndex)
                Dim lImprArea As Double = StrRetRem(lLandUseAreaString)
                Dim lPervArea As Double = 0
                If lLandUseAreaString.Length > 0 Then
                    lPervArea = lLandUseAreaString
                End If


                Dim lLandUseArea As Double = lPervArea + lImprArea
                lReport.AppendLine(lLandUsesCombinePervImpv.Keys(lLandUseIndex).ToString.PadLeft(20) & vbTab & _
                               DecimalAlign(lPervArea, , 2, 7) & vbTab & _
                               DecimalAlign(lImprArea, , 2, 7) & vbTab & _
                               DecimalAlign(lLandUseArea, , 2, 7))
                lTotalAreaPerv += lPervArea
                lTotalAreaImpr += lImprArea
                lTotalAreaFromLandUses += lLandUseArea
            Next
            lLandUsesCombinePervImpv.Clear()
            lLandUses.Clear()
            lReport.AppendLine("")
            lReport.AppendLine("Total".PadLeft(20) & vbTab & _
                           DecimalAlign(lTotalAreaPerv, , 2, 7) & vbTab & _
                           DecimalAlign(lTotalAreaImpr, , 2, 7) & vbTab & _
                           DecimalAlign(lTotalAreaFromLandUses, , 2, 7))
            SaveFileString(aReportPath & SafeFilename("AreaLanduse_" & aLocation & ".txt"), lReport.ToString)
        End If

        Dim lLocations As atcCollection = UpstreamLocations(aUci, aOperationtypes, aLocation)
        Dim lTotalArea As Double = 0.0
        For lIndex As Integer = 0 To lLocations.Count - 1
            If lLocations.Item(lIndex) IsNot Nothing Then
                lTotalArea += lLocations.Item(lIndex)
            End If
        Next

        lLocations.Clear()
        Dim lUpstreamLocations As New atcCollection
        Dim lUpstreamLocationsString As String = ""
        Dim lLocalArea As Double = 0.0
        LocationAreaCalc(aUci, aLocation, aOperationtypes, lLocations, False)
        For lIndex As Integer = 0 To lLocations.Count - 1
            lLocalArea += lLocations.Item(lIndex)
            If lLocations.Item(lIndex) < 0.00001 Then
                Dim lLocation As String = lLocations.Keys(lIndex)
                If lLocation <> aLocation Then
                    lUpstreamLocations.Add(lLocation)
                    lUpstreamLocationsString &= lLocations.Keys(lIndex) & ", "
                End If
            End If
        Next

        If lUpstreamLocationsString.Length > 0 Then
            lUpstreamLocationsString = lUpstreamLocationsString.Remove(lUpstreamLocationsString.Length - 2, 2)
        End If
        Dim lStr As String = ""
        For Each lUpstreamLocation As String In lUpstreamLocations
            lStr &= AreaReportLocation(aUci, aOperationtypes, lUpstreamLocation, aLandUseReport, aReportPath, aRunMade)
        Next
        lUpstreamLocations.Clear()
        lStr &= aLocation.PadRight(8) & vbTab & _
                DecimalAlign(lTotalArea, , 2, 7) & vbTab & _
                DecimalAlign(lLocalArea, , 2, 7) & vbTab & _
                lUpstreamLocationsString & vbCrLf
        Return lStr
    End Function

    Private Function LandUsesCombined(ByVal aLandUses As atcCollection) As atcCollection
        Dim lLUCombined As New atcCollection
        For lIndex As Integer = 0 To aLandUses.Count - 1
            Dim lLandUseName As String = aLandUses.Keys(lIndex).ToString
            Dim lLandUse As atcCollection = aLandUses.Item(lIndex)
            Dim lLandUseArea As Double = 0.0
            For lOperationIndex As Integer = 0 To lLandUse.Count - 1
                lLandUseArea += lLandUse.Item(lOperationIndex)
            Next
            If lLandUseArea > 0 Then
                Dim lLandUseKey As Integer = lLUCombined.IndexFromKey(lLandUseName.Remove(0, 2))
                Dim lLandUseString As String = ""
                If lLandUseKey = -1 Then
                    lLandUseString = lLandUseArea
                    If lLandUseName.StartsWith("P") Then
                        lLandUseString = "0.0," & lLandUseString
                    End If
                    lLUCombined.Add(lLandUseName.Remove(0, 2), lLandUseString)
                Else
                    lLUCombined(lLandUseKey) &= "," & lLandUseArea
                End If
            End If
        Next
        For lIndex As Integer = 0 To lLUCombined.Count - 1
            If Not lLUCombined.ItemByIndex(lIndex).Contains(",") Then
                lLUCombined.ItemByIndex(lIndex) &= ",0.0"
            End If
        Next
        Return lLUCombined
    End Function
End Module
