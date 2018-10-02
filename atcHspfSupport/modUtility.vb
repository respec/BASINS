Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcUCI
Imports System.Data

Public Module Utility
    Public Class ConstituentProperties
        ''' <summary>
        ''' Kind of constituent report, like Water, Sediment, DO, Heat, TN, TP etc. 
        ''' </summary>
        ''' <returns></returns>
        Public Property ReportType As String = ""
        ''' <summary>
        ''' Use a specific name in EXP+ to differentiate between constituents
        ''' </summary>
        ''' <returns></returns>
        Public Property ConstNameForEXPPlus As String = ""
        ''' <summary>
        ''' UCI may have a different name for the Quality Constituent
        ''' </summary>
        ''' <returns></returns>
        Public Property ConstituentNameInUCI As String = ""
        ''' <summary>
        ''' Unit of the Constituent specifiied in the UCI file
        ''' </summary>
        ''' <returns></returns>
        Public Property ConstituentUnit As String = ""

    End Class

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
                "ADS REFR ORGANIC N - TOTAL STORAGE", _
                "ABOVE-GROUND PLANT STORAGE", _
                "LITTER STORAGE", _
                "PLANT N - SURFACE LAYER STORAGE", _
                "PLANT N - UPPER PRINCIPAL STORAGE", _
                "PLANT N - LOWER LAYER STORAGE", _
                "PLANT N - ACTIVE GROUNDWATER STORAGE", _
                "PLANT N - TOTAL STORAGE", _
                "PLANT P - SURFACE LAYER", _
                "PLANT P - UPPER PRINCIPAL", _
                "PLANT P - LOWER LAYER", _
                "PLANT P - ACTIVE GROUNDWATER", _
                "PLANT P - TOTALS", _
                "PO4-P SOL - SURFACE LAYER", _
                "PO4-P SOL - UPPER PRINCIPAL", _
                "PO4-P SOL - UPPER TRANSITORY", _
                "PO4-P SOL - LOWER LAYER", _
                "PO4-P SOL - ACTIVE GROUNDWATER", _
                "PO4-P SOL - TOTALS", _
                "PO4-P ADS - SURFACE LAYER", _
                "PO4-P ADS - UPPER PRINCIPAL", _
                "PO4-P ADS - LOWER LAYER", _
                "PO4-P ADS - ACTIVE GROUNDWATER", _
                "PO4-P ADS - TOTALS", _
                "ORGANIC P - SURFACE LAYER", _
                "ORGANIC P - UPPER PRINCIPAL", _
                "ORGANIC P - LOWER LAYER", _
                "ORGANIC P - ACTIVE GROUNDWATER", _
                "ORGANIC P - TOTALS" _
                }
            pConstituentsThatUseLast = New Generic.List(Of String)(lConstituentsThatUseLastArray)
        End If
        Return pConstituentsThatUseLast
    End Function

    Public Function ConstituentsThatNeedMassLink() As Generic.List(Of String)
        Static pConstituentsThatNeedMassLink As Generic.List(Of String) = Nothing
        If pConstituentsThatNeedMassLink Is Nothing Then
            Dim lConstituentsThatNeedMassLink() As String =
                {"WSSD", "SCRSD", "SOSLD", "PERO", "SURO", "IFWO", "AGWO", "NO3+NO2-N - SURFACE LAYER OUTFLOW",
                "NO3+NO2-N - UPPER LAYER OUTFLOW", "NO3+NO2-N - GROUNDWATER OUTFLOW", "NO3-N - TOTAL OUTFLOW", "NH4-N IN SOLUTION - SURFACE LAYER OUTFLOW",
                "NH4-N IN SOLUTION - UPPER LAYER OUTFLOW", "NH4-N IN SOLUTION - GROUNDWATER OUTFLOW", "NH4-N ADS - SEDIMENT ASSOC OUTFLOW",
                 "NH4-N ADS - SEDIMENT ASSOC OUTFLOW", "NH4-N ADS - SEDIMENT ASSOC OUTFLOW", "POQUAL-NH3+NH4", "SOQUAL-NH3+NH4", "SOQO-NH3+NH4", "WASHQS-NH3+NH4",
                 "IOQUAL-NH3+NH4", "AOQUAL-NH3+NH4", "POQUAL-NO3", "SOQUAL-NO3", "SOQO-NO3", "WASHQS-NO3", "AOQUAL-NO3", "IOQUAL-NO3", "WASHQS-BOD",
                 "SOQUAL-BOD", "SOQO-BOD", "IOQUAL-BOD", "AOQUAL-BOD", "POQUAL-BOD", "ORGN - TOTAL OUTFLOW", "NITROGEN - TOTAL OUTFLOW",
                "LABILE ORGN - SEDIMENT ASSOC OUTFLOW", "REFRAC ORGN - SEDIMENT ASSOC OUTFLOW", "POQUAL-ORTHO P", "SOQUAL-ORTHO P",
                 "IOQUAL-ORTHO P", "AOQUAL-ORTHO P", "PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW",
                 "PO4-P IN SOLUTION - INTERFLOW - OUTFLOW", "PO4-P IN SOLUTION - GROUNDWATER - OUTFLOW", "SDP4A", "SDORP"}
            pConstituentsThatNeedMassLink = New Generic.List(Of String)(lConstituentsThatNeedMassLink)
        End If
        Return pConstituentsThatNeedMassLink
    End Function
    Public Function ConstituentsToOutput(ByVal aType As String, ByVal aConstProperties As List(Of ConstituentProperties),
                                Optional ByVal aCategory As Boolean = False,
                                Optional ByVal aUnits As String = "") As atcCollection
        Dim lConstituentsToOutput As New atcCollection
        Select Case aType
#Region "Case Water"
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
                    .Add("P:SUPY", "    Rainfall+SnowMelt")
                    .Add("P:IRRAPP6", "    Irrigation")
                    '.Add("P:SURLI", "    Surface Lateral Inflow") If needed for a specific project then think about the details.
                    .Add("P:Header1", "Runoff")
                    .Add("P:SURO", "    Surface  ")
                    .Add("P:IFWO", "    Interflow")
                    .Add("P:AGWO", "    Baseflow ")
                    .Add("P:Total3", "    Total    ")
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
                    .Add("R:OVOL-1", "    OutVolumeExit1")
                    .Add("R:OVOL-2", "    OutVolumeExit2")
                    .Add("R:OVOL-3", "    OutVolumeExit3")
                    .Add("R:OVOL-4", "    OutVolumeExit4")
                    .Add("R:OVOL-5", "    OutVolumeExit5")
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
#End Region
#Region "Case Sediment"
            Case "Sediment"
                With lConstituentsToOutput
                    .Add("I:Header0", "Storage(tons/acre)")
                    .Add("I:SLDS", "  All")
                    .Add("I:Header1", "Washoff(tons/acre)")
                    .Add("I:SOSLD", "  Total")


                    .Add("P:Header0", "Storage (tons/acre)")
                    .Add("P:DETS", "  Detached ")
                    .Add("P:Header1", "Washoff (tons/acre)")
                    .Add("P:WSSD", "  Washoff of Detached Sediment")
                    .Add("P:SCRSD", "  Scour of Matrix (attached) soil")
                    .Add("P:Total2", "Total Sediment Loss (tons/ac)")

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
                    .Add("R:OSED-TOT-EXIT1", "  TotalExit1")
                    .Add("R:OSED-TOT-EXIT2", "  TotalExit2")
                    .Add("R:OSED-TOT-EXIT3", "  TotalExit3")
                    .Add("R:OSED-TOT-EXIT4", "  TotalExit4")
                    .Add("R:OSED-TOT-EXIT5", "  TotalExit5")
                End With
#End Region
#Region "Case SedimentCopper"
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
#End Region
#Region "Case N-PQUAL"
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
#End Region
#Region "Case P-PQUAL"
            Case "P-PQUAL"
                With lConstituentsToOutput
                    .Add("P:Header1", "ORTHO P (lb/ac)")
                    .Add("P:WASHQS-ORTHO P", "  Sediment Attached")
                    .Add("P:SOQUAL-ORTHO P", "  Surface Flow")
                    .Add("P:IOQUAL-ORTHO P", "  Interflow")
                    .Add("P:AOQUAL-ORTHO P", "  Groundwater Flow")
                    .Add("P:POQUAL-ORTHO P", "  Total")
                    .Add("I:Header2", "ORTHO P (lb/ac)")
                    .Add("I:SOQUAL-ORTHO P", "  Surface Flow")
                    .Add("R:Header3", "Total P")
                    .Add("R:P-TOT-IN", "  Total P Inflow")
                    .Add("R:P-TOT-OUT", "  Total P Outflow")
                End With
#End Region
#Region "Case TotalN"
            Case "TotalN", "TN" 'use PQUAL
                With lConstituentsToOutput
                    .Add("P:Header1", "Nitrogen Loss (lb/ac)")
                    .Add("P:Header1a", "  NO3 Loss")
                    .Add("P:NO3+NO2-N - SURFACE LAYER OUTFLOW", "    Surface")
                    .Add("P:NO3+NO2-N - UPPER LAYER OUTFLOW", "    Interflow")
                    .Add("P:NO3+NO2-N - GROUNDWATER OUTFLOW", "    Baseflow")
                    .Add("P:Total3", "    Total")
                    .Add("P:Header1b", "  NH3 Loss")
                    .Add("P:NH4-N IN SOLUTION - SURFACE LAYER OUTFLOW", "    Surface")
                    .Add("P:NH4-N IN SOLUTION - UPPER LAYER OUTFLOW", "    Interflow")
                    .Add("P:NH4-N IN SOLUTION - GROUNDWATER OUTFLOW", "    Baseflow")
                    .Add("P:NH4-N ADS - SEDIMENT ASSOC OUTFLOW", "    Sediment")
                    .Add("P:Total4", "    Total")
                    .Add("P:Header1c", "  Labile ORGN Loss")
                    .Add("P:LABILE ORGN - SURFACE LAYER OUTFLOW", "    Surface")
                    .Add("P:LABILE ORGN - UPPER LAYER OUTFLOW", "    Interflow")
                    .Add("P:LABILE ORGN - GROUNDWATER OUTFLOW", "    Baseflow")
                    .Add("P:LABILE ORGN - SEDIMENT ASSOC OUTFLOW", "    Sediment")
                    .Add("P:ORGN - TOTAL OUTFLOW1", "    Labile N as fraction of TORN")

                    .Add("P:Header1d", "  Refractory ORGN Loss")
                    .Add("P:REFRAC ORGN - SURFACE LAYER OUTFLOW", "    Surface")
                    .Add("P:REFRAC ORGN - UPPER LAYER OUTFLOW", "    Interflow")
                    .Add("P:REFRAC ORGN - GROUNDWATER OUTFLOW", "    Baseflow")
                    .Add("P:REFRAC ORGN - SEDIMENT ASSOC OUTFLOW", "    Sediment")
                    .Add("P:ORGN - TOTAL OUTFLOW2", "    Refractory N as fraction of TORN")
                    .Add("P:Header1e", "    ")
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

                    .Add("P:Header3i", "  Labile/Refractory ORGN Conversion")
                    .Add("P:SREFON", "    Surface")
                    .Add("P:UREFON", "    Upper")
                    .Add("P:LREFON", "    Lower")
                    .Add("P:AREFON", "    Groundwater")
                    .Add("P:TREFON", "    Total")

                    .Add("P:Header3j", "  Labile ORGN Mineralization")
                    .Add("P:SORNMN", "    Surface")
                    .Add("P:UORNMN", "    Upper")
                    .Add("P:LORNMN", "    Lower")
                    .Add("P:AORNMN", "    Groundwater")
                    .Add("P:TORNMN", "    Total")

                    .Add("P:Header3k", "  Denitrification")
                    .Add("P:SDENI", "    Surface")
                    .Add("P:UDENI", "    Upper")
                    .Add("P:LDENI", "    Lower")
                    .Add("P:ADENI", "    Groundwater")
                    .Add("P:TDENI", "    Total")

                    .Add("P:Header3l", "  NH3 Nitrification")
                    .Add("P:SAMNIT", "    Surface")
                    .Add("P:UAMNIT", "    Upper")
                    .Add("P:LAMNIT", "    Lower")
                    .Add("P:AAMNIT", "    Groundwater")
                    .Add("P:TAMNIT", "    Total")

                    .Add("P:Header3m", "  NH3 Immobilization")
                    .Add("P:SAMIMB", "    Surface")
                    .Add("P:UAMIMB", "    Upper")
                    .Add("P:LAMIMB", "    Lower")
                    .Add("P:AAMIMB", "    Groundwater")
                    .Add("P:TAMIMB", "    Total")

                    .Add("P:Header3n", "  NO3 Immobilization")
                    .Add("P:SNIIMB", "    Surface")
                    .Add("P:UNIIMB", "    Upper")
                    .Add("P:LNIIMB", "    Lower")
                    .Add("P:ANIIMB", "    Groundwater")
                    .Add("P:TNIIMB", "    Total")

                    .Add("P:Header3o", "  NH3 Volatilization")
                    .Add("P:SAMVOL", "    Surface")
                    .Add("P:UAMVOL", "    Upper")
                    .Add("P:LAMVOL", "    Lower")
                    .Add("P:AAMVOL", "    Groundwater")
                    .Add("P:TAMVOL", "    Total")

                    Dim headerLabel() As String = {"a", "b", "c", "d"}
                    Dim headerLabelCount As Integer = 0
                    For Each ConstProperty As ConstituentProperties In aConstProperties
                        If ConstProperty.ConstNameForEXPPlus = "TN" Then Continue For
                        Select Case ConstProperty.ConstNameForEXPPlus
                            Case "NO3"
                                .Add("P:Header4", "NO3+NO2 (PQUAL)")
                                .Add("P:SOQO-" & ConstProperty.ConstituentNameInUCI, "  Dissolved with Surface Flow")
                                .Add("P:IOQUAL-" & ConstProperty.ConstituentNameInUCI, "  Dissolved with Interflow")
                                .Add("P:AOQUAL-" & ConstProperty.ConstituentNameInUCI, "  Dissolved with Groundwater Flow")
                            Case "TAM"
                                .Add("P:Header5", "NH3+NH4 (PQUAL)")
                                .Add("P:WASHQS-" & ConstProperty.ConstituentNameInUCI, "  Sediment Attached")
                                .Add("P:SCRQS-" & ConstProperty.ConstituentNameInUCI, "  Scoured Sediment Attached")
                                .Add("P:SOQO-" & ConstProperty.ConstituentNameInUCI, "  Dissolved with Surface Flow")
                                .Add("P:IOQUAL-" & ConstProperty.ConstituentNameInUCI, "  Dissolved with Interflow")
                                .Add("P:AOQUAL-" & ConstProperty.ConstituentNameInUCI, "  Dissolved with Groundwater Flow")

                            Case "lab-OrgN"
                                .Add("P:Header6", "LabileOrgN (PQUAL)")
                                .Add("P:WASHQS-" & ConstProperty.ConstituentNameInUCI & "2", "  Sediment Attached")
                                .Add("P:SCRQS-" & ConstProperty.ConstituentNameInUCI & "2", "  Scoured Sediment Attached")
                                .Add("P:SOQUAL-" & ConstProperty.ConstituentNameInUCI & "2", "  Dissolved with Surface Flow")
                                .Add("P:IOQUAL-" & ConstProperty.ConstituentNameInUCI & "2", "  Dissolved with Interflow")
                                .Add("P:AOQUAL-" & ConstProperty.ConstituentNameInUCI & "2", "  Dissolved with Groundwater Flow")

                            Case "Ref-OrgN"
                                .Add("P:Header7", "RefOrgN (PQUAL)")
                                .Add("P:WASHQS-" & ConstProperty.ConstituentNameInUCI & "1", "  Sediment Attached")
                                .Add("P:SCRQS-" & ConstProperty.ConstituentNameInUCI & "1", "  Scoured Sediment Attached")
                                .Add("P:SOQO-" & ConstProperty.ConstituentNameInUCI & "1", "  Dissolved with Surface Flow")
                                .Add("P:IOQUAL-" & ConstProperty.ConstituentNameInUCI & "1", "  Dissolved with Interflow")
                                .Add("P:AOQUAL-" & ConstProperty.ConstituentNameInUCI & "1", "  Dissolved with Groundwater Flow")

                        End Select
                        .Add("P:Total3" & headerLabel(headerLabelCount), "  Total")
                        headerLabelCount += 1
                    Next

                    For Each ConstProperty As ConstituentProperties In aConstProperties
                        Select Case ConstProperty.ConstNameForEXPPlus
                            Case "NO3"
                                .Add("I:Header8", "NO3+NO2 (IQUAL)")
                                .Add("I:SOQUAL-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow")

                            Case "TAM"
                                .Add("I:Header9", "NH3+NH4 (IQUAL)")
                                .Add("I:WASHQS-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow with Sediment")
                                .Add("I:SOQO-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow as Dissolved")

                            Case "lab-OrgN"
                                .Add("I:Header10", "RefOrgN (IQUAL)")
                                .Add("I:WASHQS-" & ConstProperty.ConstituentNameInUCI & "2", "  Surface Flow with Sediment")
                                .Add("I:SOQO-" & ConstProperty.ConstituentNameInUCI & "2", "  Surface Flow as Dissolved")

                            Case "Ref-OrgN"
                                .Add("I:Header11", "LabileOrgN (IQUAL)")
                                .Add("I:WASHQS-" & ConstProperty.ConstituentNameInUCI & "1", "  Surface Flow with Sediment")
                                .Add("I:SOQO-" & ConstProperty.ConstituentNameInUCI & "1", "  Surface Flow as Dissolved")
                        End Select
                    Next

                    .Add("R:Header12", "NO3 as N")
                    .Add("R:NO3-INTOT", "  Total NO3 Inflow")
                    .Add("R:NO3-PROCFLUX-TOT", "  NO3 Process Fluxes")
                    .Add("R:NO3-ATMDEPTOT", " Atmospheric NO3 Deposition")
                    .Add("R:NO3-OUTTOT", " Total NO3 Outflow")


                    .Add("R:Header13", "Total NH3 as N")
                    .Add("R:TAM-INTOT", "  Total Ammonia Inflow")
                    .Add("R:TAM-INDIS", "  Dissolved TAM inflow")
                    .Add("R:NH4-INPART-TOT", "  Particulate NH3 Inflow")
                    .Add("R:TAM-PROCFLUX-TOT", "  TAM Process Fluxes")
                    .Add("R:TAM-ADSDES-TOT", "  TAM Adsorption/Desorption")
                    .Add("R:TAM-SCOURDEP-TOT", "  TAM Scour/Deposition")
                    .Add("R:TAM-ATMDEPTOT", " Atmospheric TAM Deposition")
                    .Add("R:TAM-OUTTOT", "  Total TAM Outflow")
                    .Add("R:TAM-OUTDIS", "  Dissolved TAM Outflow")
                    .Add("R:TAM-OUTPART-TOT", "  Particulate TAM Outflow")

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
                    .Add("R:N-TOT-OUT-EXIT1", "  Total N OutflowExit1")
                    .Add("R:N-TOT-OUT-EXIT2", "  Total N OutflowExit2")
                    .Add("R:N-TOT-OUT-EXIT3", "  Total N OutflowExit3")
                    .Add("R:N-TOT-OUT-EXIT4", "  Total N OutflowExit4")
                    .Add("R:N-TOT-OUT-EXIT5", "  Total N OutflowExit5")

                End With
            '.Add("R:TAM-OUTTOT-EXIT3", "  Total NH3 Outflow-Exit3")
            '.Add("R:TAM-OUTDIS-EXIT3", "  Dissolved NH3 Outflow-Exit3")
            '.Add("R:TAM-OUTPART-TOT-EXIT3", "  Particulate NH3 Outflow-Exit3")

            '.Add("R:NO3-OUTTOT-EXIT3", "  Total NO3 Outflow - Exit 3")

            '.Add("R:N-TOT-OUT-EXIT1", "  N-TOT-OUT-EXIT1")
            '.Add("R:N-TOT-OUT-EXIT2", "  N-TOT-OUT-EXIT2")
            '.Add("R:N-TOT-OUT-EXIT3", "  N-TOT-OUT-EXIT3")
#End Region
#Region "Case TotalP"
            Case "TP"

                With lConstituentsToOutput
                    .Add("P:Header1", "Phosphorus Loss (lb/ac")
                    .Add("P:PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW", "    Surface")
                    .Add("P:PO4-P IN SOLUTION - INTERFLOW - OUTFLOW", "    Interflow")
                    .Add("P:PO4-P IN SOLUTION - GROUNDWATER - OUTFLOW", "    Baseflow")
                    .Add("P:SDP4A", "    Sediment")
                    .Add("P:Total4", "    Total")


                    .Add("P:ORGN - TOTAL OUTFLOW", "    Labile Org P from POORN")
                    .Add("P:SDORP", "    Refractory Org P from SEDP 1")

                    .Add("P:Total3", "    Total P Loss")

                    .Add("P:Header2", "Plant P Uptake")
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
                    .Add("P:Header6d", "    ORGP Mineralization")
                    .Add("P:SORPMN", "    Surface")
                    .Add("P:UORPMN", "    Upper")
                    .Add("P:LORPMN", "    Lower")
                    .Add("P:AORPMN", "    Groundwater")
                    .Add("P:TORPMN", "    Total")

                    .Add("P:Header6e", "    PO4-P Immobilization")
                    .Add("P:SP4IMB", "    Surface")
                    .Add("P:UP4IMB", "    Upper")
                    .Add("P:LP4IMB", "    Lower")
                    .Add("P:AP4IMB", "    Groundwater")
                    .Add("P:TP4IMB", "    Total")

                    For Each ConstProperty As ConstituentProperties In aConstProperties
                        Select Case ConstProperty.ConstNameForEXPPlus
                            Case "PO4"
                                .Add("P:Header7", "ORTHO P (PQUAL)")
                                .Add("P:WASHQS-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow with Sediment")
                                .Add("P:SCRQS-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow with Scoured Sediment")
                                .Add("P:SOQO-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow as Dissolved")
                                .Add("P:IOQUAL-" & ConstProperty.ConstituentNameInUCI, "  Interflow")
                                .Add("P:AOQUAL-" & ConstProperty.ConstituentNameInUCI, "  Groundwater Flow")
                                .Add("P:Total3c", "  Total")
                            Case "Ref-OrgP"
                                .Add("P:Header8", "RefOrgP (PQUAL)")
                                .Add("P:WASHQS-" & ConstProperty.ConstituentNameInUCI & "1", "  Surface Flow with Sediment")
                                .Add("P:SCRQS-" & ConstProperty.ConstituentNameInUCI & "1", "  Surface Flow with Scoured Sediment")
                                .Add("P:SOQO-" & ConstProperty.ConstituentNameInUCI & "1", "  Surface Flow as Dissolved")
                                .Add("P:IOQUAL-" & ConstProperty.ConstituentNameInUCI & "1", "  Interflow")
                                .Add("P:AOQUAL-" & ConstProperty.ConstituentNameInUCI & "1", "  Groundwater Flow")
                                .Add("P:Total3d", "  Total")

                            Case "lab-OrgP"
                                .Add("P:Header9", "LabileOrgP (PQUAL)")
                                .Add("P:WASHQS-" & ConstProperty.ConstituentNameInUCI & "2", "  Surface Flow with Sediment")
                                .Add("P:SCRQS-" & ConstProperty.ConstituentNameInUCI & "2", "  Surface Flow with Scoured Sediment")
                                .Add("P:SOQO-" & ConstProperty.ConstituentNameInUCI & "2", "  Surface Flow as Dissolved")
                                .Add("P:IOQUAL-" & ConstProperty.ConstituentNameInUCI & "2", "  Interflow")
                                .Add("P:AOQUAL-" & ConstProperty.ConstituentNameInUCI & "2", "  Groundwater Flow")
                                .Add("P:Total3e", "  Total")

                        End Select
                    Next
                    For Each ConstProperty As ConstituentProperties In aConstProperties
                        Select Case ConstProperty.ConstNameForEXPPlus
                            Case "PO4"
                                .Add("I:Header10", "ORTHO P (IQUAL)")
                                .Add("I:WASHQS-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow with Sediment")
                                .Add("I:SOQO-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow as Dissolved")


                            Case "Ref-OrgP"
                                .Add("I:Header11", "RefOrgP (IQUAL)")
                                .Add("I:WASHQS-" & ConstProperty.ConstituentNameInUCI & "1", "  Surface Flow with Sediment")
                                .Add("I:SOQO-" & ConstProperty.ConstituentNameInUCI & "1", "  Surface Flow as Dissolved")

                            Case "lab-OrgP"
                                .Add("I:Header12", "LabileOrgP (IQUAL)")
                                .Add("I:WASHQS-" & ConstProperty.ConstituentNameInUCI & "2", "  Surface Flow with Sediment")
                                .Add("I:SOQUAL-" & ConstProperty.ConstituentNameInUCI & "2", "  Surface Flow as Dissolved")

                        End Select

                    Next

                    .Add("R:Header13", "Total PO4 as P")
                    .Add("R:PO4-INTOT", "  Total PO4 Inflow")
                    .Add("R:PO4-INDIS", "  Dissolved PO4 Inflow")
                    .Add("R:PO4-INPART-TOT", "  Particulate PO4 Inflow")
                    .Add("R:PO4-PROCFLUX-TOT", "  PO4 Process Fluxes")
                    .Add("R:PO4-ADSDES-TOT", "  PO4 Adsorption/Desorption")
                    .Add("R:PO4-SCOURDEP-TOT", "  PO4 Scour/Deposition")
                    .Add("R:PO4-OUTTOT", "  Total PO4 Outflow")
                    .Add("R:PO4-ATMDEPTOT", " Atmospheric PO4 Deposition")
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
                    .Add("R:P-TOT-OUT-EXIT1", "  Total P OutflowExit1")
                    .Add("R:P-TOT-OUT-EXIT2", "  Total P OutflowExit2")
                    .Add("R:P-TOT-OUT-EXIT3", "  Total P OutflowExit3")
                    .Add("R:P-TOT-OUT-EXIT4", "  Total P OutflowExit4")
                    .Add("R:P-TOT-OUT-EXIT5", "  Total P OutflowExit5")

                End With
#End Region
#Region "Case BOD-Labile"
            Case "BOD-Labile"
                With lConstituentsToOutput

                    For Each ConstProperty As ConstituentProperties In aConstProperties
                        Select Case ConstProperty.ConstNameForEXPPlus
                            Case "BOD-Labile"
                                .Add("P:Header1", "BOD-Labile (PQUAL)")
                                .Add("P:WASHQS-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow with Sediment")
                                .Add("P:SOQO-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow as Dissolved")
                                .Add("P:IOQUAL-" & ConstProperty.ConstituentNameInUCI, "  Interflow")
                                .Add("P:AOQUAL-" & ConstProperty.ConstituentNameInUCI, "  Groundwater Flow")
                                .Add("P:Total1", "  Total")
                        End Select
                    Next

                    .Add("P:ORGN - TOTAL OUTFLOW", "  BOD from OrganicN")
                    For Each ConstProperty As ConstituentProperties In aConstProperties
                        Select Case ConstProperty.ConstNameForEXPPlus
                            Case "BOD-Labile"
                                .Add("I:Header1", "BOD-Labile (IQUAL)")
                                .Add("P:WASHQS-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow with Sediment")
                                .Add("P:SOQO-" & ConstProperty.ConstituentNameInUCI, "  Surface Flow as Dissolved")
                                .Add("P:Total2", "  Total")
                        End Select
                    Next

                    .Add("R:Header3", "BOD")
                    .Add("R:BODIN", "  BODIN")
                    .Add("R:BODFLUX-BODDEC", "  BODFLUX-BODDEC")
                    .Add("R:BODFLUX-SINK", "  BODFLUX-SINK")
                    .Add("R:BODFLUX-BENTHAL", "  BODFLUX-BENTHAL")
                    .Add("R:BODFLUX-DENITR", "  BODFLUX-DENITR")
                    .Add("R:BODFLUX-PHYTO", "  BODFLUX-PHYTO")
                    .Add("R:BODFLUX-ZOO", "  R:BODFLUX-ZOO")
                    .Add("R:BODFLUX-BENTHIC", "  BODFLUX-BENTHIC")
                    .Add("R:BODFLUX-TOT", "  BODFLUX-Total")
                    .Add("R:BODOUTTOT", "  BODOUTTOT")
                End With
#End Region
#Region "Case Else"
            Case Else
                Dim lUnits As String = aUnits
                If aType.ToUpper.Contains("F.COLIFORM") Or aType.ToUpper.StartsWith("BACT") Then 'Assuming this is f.coli or bacteria
                    lUnits = "10^9 " & aUnits
                End If
                With lConstituentsToOutput
                    .Add("P:Header1", aType & " (" & lUnits & "/ac)")
                    .Add("P:SOQUAL-" & aType, "  Surface")
                    .Add("P:IOQUAL-" & aType, "  Interflow")
                    .Add("P:AOQUAL-" & aType, "  Baseflow")
                    .Add("P:Total3", "  Total")
                    .Add("I:Header2", aType & " (" & lUnits & "/ac)")
                    .Add("I:SOQUAL-" & aType, "  ImpervSurf")
                    .Add("R:Header3", aType & " (" & lUnits & ")")
                    .Add("R:" & aType & "-TIQAL", "  Inflow")
                    .Add("R:" & aType & "-DDQAL-TOT", "  Decay")
                    .Add("R:" & aType & "-TROQAL", "  Outflow")
                End With
#End Region
        End Select
        Return lConstituentsToOutput
    End Function

    Public Function LandUses(ByVal aUci As HspfUci,
                             ByVal aOperationTypes As atcCollection,
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
            If aOperationTypes.Contains(lOperation.Name) Then
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

    Public Function UpstreamLocations(ByVal aUci As HspfUci,
                                      ByVal aOperationTypes As atcCollection,
                                      ByVal aLocation As String) As atcCollection
        Dim lLocations As New atcCollection 'key-location, value-total area
        UpstreamLocationAreaCalc(aUci, aLocation, aOperationTypes, lLocations)
        Return lLocations
    End Function

    Private Sub UpstreamLocationAreaCalc(ByVal aUci As HspfUci,
                                         ByVal aLocation As String,
                                         ByVal aOperationTypes As atcCollection,
                                         ByRef aLocations As atcCollection)
        LocationAreaCalc(aUci, aLocation, aOperationTypes, aLocations, True)
    End Sub

    Public Sub LocationAreaCalc(ByVal aUci As HspfUci,
                                ByVal aLocation As String,
                                ByVal aOperationTypes As atcCollection,
                                ByRef aLocations As atcCollection,
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


                ElseIf lSourceVolName = "RCHRES" Or lSourceVolName = "BMPRAC" Then
                    If aUpstream Then
                        If lUpstreamChecked.Contains(lLocationKey) Then
                            Logger.Dbg("SkipDuplicate:" & lLocationKey)
                        ElseIf aUci.Name.ToLower.Contains("scr") AndAlso
                               lConnection.Source.VolId = 229 AndAlso
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

    Public Function CfsToInches(ByVal aTSerIn As atcTimeseries,
                                ByVal aArea As Double) As atcTimeseries
        Dim lConversionFactor As Double = (12.0# * 24.0# * 3600.0#) / (aArea * 43560.0#)   'cfs days to inches
        Dim lTsMath As atcTimeseriesSource = New atcTimeseriesMath.atcTimeseriesMath
        Dim lArgsMath As New atcDataAttributes
        lArgsMath.SetValue("timeseries", aTSerIn)
        lArgsMath.SetValue("number", lConversionFactor)
        lTsMath.Open("multiply", lArgsMath)
        Return lTsMath.DataSets(0)
    End Function

    Public Function InchesToCfs(ByVal aTSerIn As atcTimeseries,
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

    Friend Sub CheckDateJ(ByVal aTSer As atcTimeseries,
                          ByVal aName As String,
                          ByRef aSDateJ As Double,
                          ByRef aEDateJ As Double,
                          ByRef aStr As String)
        Dim lDateTmp As Double = aTSer.Dates.Values(0)
        If aSDateJ < lDateTmp Then
            aStr &= "   Adjusted Start Date from " & Format(Date.FromOADate(aSDateJ), "yyyy/MM/dd") &
                                             "to " & Format(Date.FromOADate(lDateTmp), "yyyy/MM/dd") &
                                        " due to " & aName & vbCrLf & vbCrLf
            aSDateJ = lDateTmp
        End If
        lDateTmp = aTSer.Dates.Values(aTSer.numValues)
        If aEDateJ > lDateTmp Then
            aStr &= "   Adjusted End Date from " & Format(Date.FromOADate(aEDateJ), "yyyy/MM/dd") &
                                          " to " & Format(Date.FromOADate(lDateTmp), "yyyy/MM/dd") &
                                      " due to " & aName & vbCrLf & vbCrLf
            aEDateJ = lDateTmp
        End If
    End Sub

    Public Function AreaReport(ByVal aUci As HspfUci, ByVal aRunMade As String,
                               ByVal aOperationTypes As atcCollection, ByVal aLocations As atcCollection,
                               ByVal aLandUseReport As Boolean, ByVal aReportPath As String) As atcReport.IReport
        Dim lReport As New atcReport.ReportText
        lReport.AppendLine("Area Summary Report")
        lReport.AppendLine("   UCI File Name " & aUci.Name)
        lReport.AppendLine("   Run Made " & aRunMade)
        lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
        lReport.AppendLine("")

        lReport.AppendLine("Location" & vbTab & "TotalArea".PadLeft(12) & vbTab & "LocalArea".PadLeft(12) & vbTab & "UpstreamReaches")

        For Each lLocation As String In aLocations 'added by Becky - should force land use reports for ALL relevant reaches 
            'and also add the total for all relevant reaches to a single overall report
            lReport.AppendLine(AreaReportLocation(aUci, aOperationTypes, lLocation, True, aReportPath, aRunMade))
        Next lLocation

        Return lReport
    End Function

    Private Function AreaReportLocation(ByVal aUci As HspfUci, ByVal aOperationtypes As atcCollection,
                                        ByVal aLocation As String, ByVal aLandUseReport As Boolean,
                                        ByVal aReportPath As String, ByVal aRunMade As String) As String
        If aLandUseReport Then
            Dim lReport As New atcReport.ReportText
            lReport.AppendLine("LanduseArea Summary Report at " & aLocation)
            lReport.AppendLine("   UCI File Name " & aUci.Name)
            lReport.AppendLine("   Run Made " & aRunMade)
            lReport.AppendLine("   " & aUci.GlobalBlock.RunInf.Value)
            lReport.AppendLine("")
            lReport.AppendLine("Landuse".PadLeft(20) & vbTab &
                           "PervArea".PadLeft(12) & vbTab &
                           "ImpvArea".PadLeft(12) & vbTab &
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


                lReport.AppendLine(lLandUsesCombinePervImpv.Keys(lLandUseIndex).ToString.PadLeft(20) & vbTab &
                               DecimalAlign(lPervArea, , 2, 7) & vbTab &
                               DecimalAlign(lImprArea, , 2, 7) & vbTab &
                               DecimalAlign(lLandUseArea, , 2, 7))
                lTotalAreaPerv += lPervArea
                lTotalAreaImpr += lImprArea
                lTotalAreaFromLandUses += lLandUseArea
            Next
            lLandUsesCombinePervImpv.Clear()
            lLandUses.Clear()
            lReport.AppendLine("")
            lReport.AppendLine("Total".PadLeft(20) & vbTab &
                           DecimalAlign(lTotalAreaPerv, , 2, 7) & vbTab &
                           DecimalAlign(lTotalAreaImpr, , 2, 7) & vbTab &
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
        lStr &= aLocation.PadRight(8) & vbTab &
                DecimalAlign(lTotalArea, , 2, 7) & vbTab &
                DecimalAlign(lLocalArea, , 2, 7) & vbTab &
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

    'given a uci and an outlet location (like R:930), return a collection of contributing areas 
    Public Function ContributingLandUseAreas(ByVal aUci As HspfUci,
                                             ByVal aOperationTypes As atcCollection,
                                             ByVal aOutletLocation As String) As atcCollection
        Dim lLandUses As atcCollection = LandUses(aUci, aOperationTypes, aOutletLocation)
        Dim lLandUsesCombinePervImpv As atcCollection = LandUsesCombined(lLandUses)
        Return lLandUsesCombinePervImpv
    End Function

    Public Function FindMassLinkFactor(ByVal aUCI As HspfUci, ByVal aMassLink As Integer, ByVal aConstituent As String,
                                               ByVal aBalanceType As String, ByVal aConversionFactor As Double, ByVal aMultipleIndex As Integer,
                                       Optional ByVal aGQALID As Integer = 0) As Double
        'If aConstituent = "TAM" Then aConstituent = "NH3+NH4"
        Dim lMassLinkFactor As Double = 0.0
        'If aConstituent <> "SOSLD" Then Stop
        For Each lMassLink As HspfMassLink In aUCI.MassLinks

            If lMassLink.MassLinkId <> aMassLink Then Continue For
            'If aMassLink = 1 Then Stop
            Select Case aBalanceType
                Case "Sediment", "SED"
                    Select Case aConstituent & "_" & lMassLink.Source.Member.ToString & "_" & lMassLink.Target.Member.ToString
                        Case "WSSD_WSSD_ISED", "WSSD_SOSED_ISED", "SCRSD_SCRSD_ISED", "SCRSD_SOSED_ISED", "SOSLD_SOSLD_ISED"
                            lMassLinkFactor += lMassLink.MFact
                        Case "GENER_TIMSER_ISED"
                            lMassLinkFactor += lMassLink.MFact
                    End Select

                Case "Water", "WAT"
                    'If lMassLink.Source.Member = "SURO" Then Stop
                    If (lMassLink.Source.Member = "PERO" Or lMassLink.Source.Member = aConstituent) AndAlso
                            lMassLink.Target.Member = "IVOL" Then
                        lMassLinkFactor = lMassLink.MFact
                        Exit For
                    ElseIf (lMassLink.Source.VolName = aConstituent AndAlso lMassLink.Target.Member = "IVOL") Then
                        lMassLinkFactor = lMassLink.MFact
                        Exit For
                    End If

                Case "TN"
                    Select Case True
                        Case (aConstituent = "SOQUAL-NH3+NH4" OrElse aConstituent = "SOQO-NH3+NH4") AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 2 AndAlso
                                (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "AOQUAL-NH3+NH4" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 2 AndAlso
                                (lMassLink.Source.Member = "AOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "IOQUAL-NH3+NH4" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 2 AndAlso
                                (lMassLink.Source.Member = "IOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor

                            'When dissolved Nh3+NH4 enters into strean as sediment attached.

                        Case aConstituent = "SOQO-NH3+NH4" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub1 = 1 AndAlso
                                (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor += lMassLink.MFact


                            'When sediment associated NH3+NH4 enters into stream as dissolved
                        Case aConstituent = "WASHQS-NH3+NH4" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 2 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                        Case aConstituent = "SCRQS-NH3+NH4" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 2 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                        Case aConstituent = "SOQS-NH3+NH4" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 2 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor = lMassLink.MFact

                            'When sediment associated NH3+NH4 enters into stream as associated with sediment
                        Case aConstituent = "WASHQS-NH3+NH4" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 1 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "WASHQS" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact
                        Case aConstituent = "SCRQS-NH3+NH4" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 1 AndAlso
                             (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "SCRQS" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact


                        Case (aConstituent = "SOQUAL-NO3" OrElse aConstituent = "SOQO-NO3") AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 1 AndAlso
                                (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "AOQUAL-NO3" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 1 AndAlso
                                (lMassLink.Source.Member = "AOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "IOQUAL-NO3" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 1 AndAlso
                                (lMassLink.Source.Member = "IOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor

                        Case (aConstituent = "WASHQS-BOD" OrElse aConstituent = "SCRQS-BOD" OrElse aConstituent = "SOQO-BOD" OrElse aConstituent = "SOQUAL-BOD" OrElse
                            aConstituent = "WASHQS-Ref-OrgN" OrElse aConstituent = "SCRQS-Ref-OrgN" OrElse aConstituent = "SOQO-Ref-OrgN" OrElse aConstituent = "SOQUAL-Ref-OrgN" OrElse
                            aConstituent = "WASHQS-lab-OrgN" OrElse aConstituent = "SCRQS-lab-OrgN" OrElse aConstituent = "SOQO-lab-OrgN" OrElse aConstituent = "SOQUAL-lab-OrgN") AndAlso
                            lMassLink.Target.Member.ToString = "PKIF" AndAlso lMassLink.Target.MemSub1 = 3 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            If aMultipleIndex = 1 Then
                                lMassLinkFactor = lMassLink.MFact
                            ElseIf aMultipleIndex = 2 Then
                                lMassLinkFactor = BODMFact(aUCI, "SOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            ElseIf aMultipleIndex = 0 Then
                                lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, "SOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            End If
                            Return lMassLinkFactor
                        Case (aConstituent = "IOQUAL-BOD" OrElse aConstituent = "IOQUAL-Ref-OrgN" OrElse aConstituent = "IOQUAL-lab-OrgN") AndAlso
                            lMassLink.Target.Member.ToString = "PKIF" AndAlso lMassLink.Target.MemSub1 = 3 AndAlso
                            (lMassLink.Source.Member = "IOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            If aMultipleIndex = 1 Then
                                lMassLinkFactor = lMassLink.MFact
                            ElseIf aMultipleIndex = 2 Then
                                lMassLinkFactor = BODMFact(aUCI, "IOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            ElseIf aMultipleIndex = 0 Then
                                lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, "IOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            End If
                            Return lMassLinkFactor
                        Case (aConstituent = "AOQUAL-BOD" OrElse aConstituent = "AOQUAL-Ref-OrgN" OrElse aConstituent = "AOQUAL-lab-OrgN") AndAlso
                            lMassLink.Target.Member.ToString = "PKIF" AndAlso lMassLink.Target.MemSub1 = 3 AndAlso
                            (lMassLink.Source.Member = "AOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            If aMultipleIndex = 1 Then
                                lMassLinkFactor = lMassLink.MFact
                            ElseIf aMultipleIndex = 2 Then
                                lMassLinkFactor = BODMFact(aUCI, "AOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            ElseIf aMultipleIndex = 0 Then
                                lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, "AOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            End If
                            Return lMassLinkFactor

                            'Following are AGCHEM cases
                        Case (aConstituent = "NO3+NO2-N - SURFACE LAYER OUTFLOW" OrElse aConstituent = "NO3+NO2-N - UPPER LAYER OUTFLOW" OrElse
                            aConstituent = "NO3+NO2-N - GROUNDWATER OUTFLOW" OrElse aConstituent = "NO3-N - TOTAL OUTFLOW") AndAlso
                            lMassLink.Target.Member.ToString = "NUIF1" And lMassLink.Target.MemSub1 = 1
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case (aConstituent = "NH4-N IN SOLUTION - SURFACE LAYER OUTFLOW" OrElse aConstituent = "NH4-N IN SOLUTION - UPPER LAYER OUTFLOW" OrElse
                            aConstituent = "NH4-N IN SOLUTION - GROUNDWATER OUTFLOW") AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 2
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "NH4-N ADS - SEDIMENT ASSOC OUTFLOW" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso
                                             lMassLink.Target.MemSub2 = 1 'lMassLink.Target.MemSub1 = 1 AndAlso 
                            lMassLinkFactor += lMassLink.MFact
                        Case aConstituent = "ORGN - TOTAL OUTFLOW" AndAlso lMassLink.Target.Member.ToString = "PKIF" AndAlso lMassLink.Target.MemSub1 = 3
                            If aMultipleIndex = 2 Then
                                lMassLinkFactor = lMassLink.MFact
                            ElseIf aMultipleIndex = 1 Then
                                lMassLinkFactor = 1 - lMassLink.MFact
                            ElseIf aMultipleIndex = 0 Then
                                lMassLinkFactor = 1
                            End If
                            Return lMassLinkFactor
                        Case (aConstituent = "LABILE ORGN - SEDIMENT ASSOC OUTFLOW" OrElse aConstituent = "REFRAC ORGN - SEDIMENT ASSOC OUTFLOW") AndAlso
                                lMassLink.Target.Member.ToString = "PKIF" AndAlso lMassLink.Target.MemSub1 = 3
                            If aConstituent.Contains("REFRAC") Then
                                lMassLinkFactor = 0
                            Else
                                lMassLinkFactor = 0
                            End If
                            Return lMassLinkFactor
                        Case aConstituent.Contains("NITROGEN - TOTAL OUTFLOW")
                            lMassLinkFactor = 1
                            Return lMassLinkFactor


                    End Select

                    '    'Anurag does not remember what specific cases do three following lines address.
                    'ElseIf (lMassLink.Source.VolName = aConstituent AndAlso lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 1) Then
                    '    lMassLinkFactor += lMassLink.MFact

                    'ElseIf (lMassLink.Source.VolName = aConstituent AndAlso lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 2) Then
                    '    lMassLinkFactor += lMassLink.MFact

                    'ElseIf (lMassLink.Source.VolName = aConstituent AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2) Then
                    '    lMassLinkFactor += lMassLink.MFact * aConversionFactor

                    'End If


                Case "TP"
                    'If aConstituent.Contains("SOQUAL") Then Stop
                    Select Case True
                        Case (aConstituent = "SOQUAL-PO4" OrElse aConstituent = "SOQO-PO4") AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                                (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor += lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "AOQUAL-PO4" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                                (lMassLink.Source.Member = "AOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "IOQUAL-PO4" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                                (lMassLink.Source.Member = "IOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor += lMassLink.MFact

                            'if dissolved PO4 enters into stream as sediment attached PO4
                        Case aConstituent = "SOQO-PO4" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 2 AndAlso
                                (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor += lMassLink.MFact


                            'If sediment associated PO4 enters into stream as dissolved PO4
                        Case (aConstituent = "WASHQS-PO4" OrElse aConstituent = "SCRQS-PO4" OrElse aConstituent = "SOQS-PO4") AndAlso
                            lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor += lMassLink.MFact
                            'Return lMassLinkFactor

                             'If sediment associated PO4 enters into stream as sediment associated
                        Case aConstituent = "WASHQS-PO4" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 2 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "WASHQS" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact

                        Case aConstituent = "SCRQS-PO4" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 2 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "SCRQS" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact

                        Case aConstituent = "SOQS-PO4" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 2 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact

                        Case (aConstituent = "SOQUAL-ORTHO P" OrElse aConstituent = "SOQO-ORTHO P") AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "AOQUAL-ORTHO P" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                                (lMassLink.Source.Member = "AOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "IOQUAL-ORTHO P" AndAlso lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                                (lMassLink.Source.Member = "IOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor

                                'if dissolved PO4 enters into stream as sediment attached PO4
                        Case aConstituent = "SOQO-ORTHO P" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 2 AndAlso
                                (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor += lMassLink.MFact


                            'If sediment associated PO4 enters into stream as dissolved PO4
                        Case (aConstituent = "WASHQS-ORTHO P" OrElse aConstituent = "SCRQS-ORTHO P" OrElse aConstituent = "SOQS-ORTHO P") AndAlso
                            lMassLink.Target.Member.ToString = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                            'If sediment associated PO4 enters into stream as sediment associated
                        Case aConstituent = "WASHQS-ORTHO P" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 2 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "WASHQS" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact

                        Case aConstituent = "SCRQS-ORTHO P" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 2 AndAlso
                            (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "SCRQS" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact

                        Case aConstituent = "SOQS-ORTHO P" AndAlso lMassLink.Target.Member.ToString = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 2 AndAlso
                        (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact

                        Case (aConstituent = "WASHQS-BOD" OrElse aConstituent = "SCRQS-BOD" OrElse aConstituent = "SOQO-BOD" OrElse aConstituent = "SOQUAL-BOD" OrElse
                        aConstituent = "WASHQS-Ref-OrgP" OrElse aConstituent = "SCRQS-Ref-OrgP" OrElse aConstituent = "SOQO-Ref-OrgP" OrElse aConstituent = "SOQUAL-Ref-OrgP" OrElse
                        aConstituent = "WASHQS-lab-OrgP" OrElse aConstituent = "SCRQS-lab-OrgP" OrElse aConstituent = "SOQO-lab-OrgP" OrElse aConstituent = "SOQUAL-lab-OrgP") AndAlso
                        lMassLink.Target.Member.ToString = "PKIF" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                        (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            If aMultipleIndex = 1 Then
                                lMassLinkFactor = lMassLink.MFact
                            ElseIf aMultipleIndex = 2 Then
                                lMassLinkFactor = BODMFact(aUCI, "SOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            ElseIf aMultipleIndex = 0 Then
                                lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, "SOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            End If
                            Return lMassLinkFactor
                        Case (aConstituent = "IOQUAL-BOD" OrElse aConstituent = "IOQUAL-Ref-OrgP" OrElse aConstituent = "IOQUAL-lab-OrgP") AndAlso
                            lMassLink.Target.Member.ToString = "PKIF" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                            (lMassLink.Source.Member = "IOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            If aMultipleIndex = 1 Then
                                lMassLinkFactor = lMassLink.MFact
                            ElseIf aMultipleIndex = 2 Then
                                lMassLinkFactor = BODMFact(aUCI, "IOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            ElseIf aMultipleIndex = 0 Then
                                lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, "IOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            End If
                            Return lMassLinkFactor
                        Case (aConstituent = "AOQUAL-BOD" OrElse aConstituent = "AOQUAL-Ref-OrgP" OrElse aConstituent = "AOQUAL-lab-OrgP") AndAlso
                            lMassLink.Target.Member.ToString = "PKIF" AndAlso lMassLink.Target.MemSub1 = 4 AndAlso
                            (lMassLink.Source.Member = "AOQUAL" OrElse lMassLink.Source.Member = "POQUAL")
                            If aMultipleIndex = 1 Then
                                lMassLinkFactor = lMassLink.MFact
                            ElseIf aMultipleIndex = 2 Then
                                lMassLinkFactor = BODMFact(aUCI, "AOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            ElseIf aMultipleIndex = 0 Then
                                lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, "AOQUAL-BOD", lMassLink.MassLinkId) * aConversionFactor
                            End If
                            Return lMassLinkFactor
                        Case aConstituent = "PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW" AndAlso lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor

                        Case aConstituent = "PO4-P IN SOLUTION - INTERFLOW - OUTFLOW" AndAlso lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "PO4-P IN SOLUTION - GROUNDWATER - OUTFLOW" AndAlso lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "SDP4A" AndAlso lMassLink.Target.Member = "NUIF2" AndAlso lMassLink.Target.MemSub2 = 2

                            lMassLinkFactor += lMassLink.MFact

                        Case aConstituent = "SDORP" AndAlso lMassLink.Target.Member = "PKIF" AndAlso lMassLink.Target.MemSub1 = 4
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "ORGN - TOTAL OUTFLOW" AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2
                            lMassLinkFactor = lMassLink.MFact * aConversionFactor
                            Return lMassLinkFactor
                    End Select

                    'Select Case aConstituent & "_" & lMassLink.Target.Member.ToString & "_" & lMassLink.Target.MemSub1

                    '    'Case "POQUAL-ORTHO P_NUIF1_4", "SOQO-ORTHO P_NUIF1_4", "SOQUAL-ORTHO P_NUIF1_4", "IOQUAL-ORTHO P_NUIF1_4",
                    '    '         "AOQUAL-ORTHO P_NUIF1_4", "SCRQS-ORTHO P_NUIF1_4", "WASHQS-ORTHO P_NUIF1_4"
                    '    '    If lMassLink.Source.Member = aConstituent.Substring(0, 6) Or lMassLink.Source.Member = "POQUAL" Or
                    '    '            (lMassLink.Source.Member = "SOQUAL" AndAlso lMassLink.Source.Group = "IQUAL") Then
                    '    '        lMassLinkFactor = lMassLink.MFact
                    '    '        'Return lMassLinkFactor
                    '    '    End If

                    '    'Case "POQUAL-PO4_NUIF1_4", "SOQO-PO4_NUIF1_4", "SOQUAL-PO4_NUIF1_4", "IOQUAL-PO4_NUIF1_4",
                    '    '     "AOQUAL-PO4_NUIF1_4", "SCRQS-PO4_NUIF1_4", "WASHQS-PO4_NUIF1_4"
                    '    '    If lMassLink.Source.Member = aConstituent.Substring(0, 6) OrElse lMassLink.Source.Member = "POQUAL" OrElse
                    '    '            lMassLink.Source.Member = "SOQUAL" Then 'AndAlso lMassLink.Source.Group = "IQUAL") Then
                    '    '        lMassLinkFactor = lMassLink.MFact
                    '    '        'Return lMassLinkFactor
                    '    '    End If

                    '    'Case "POQUAL-ORTHO P_NUIF2_2", "SOQUAL-ORTHO P_NUIF2_2", "AOQUAL-ORTHO P_NUIF2_2", "IOQUAL-ORTHO P_NUIF2_2",
                    '    '        "POQUAL-ORTHO P_NUIF2_3", "SOQUAL-ORTHO P_NUIF2_3", "AOQUAL-ORTHO P_NUIF2_3", "IOQUAL-ORTHO P_NUIF2_3",
                    '    '        "POQUAL-ORTHO P_NUIF2_1", "SOQUAL-ORTHO P_NUIF2_1", "AOQUAL-ORTHO P_NUIF2_1", "IOQUAL-ORTHO P_NUIF2_1",
                    '    '         "WASHQS-ORTHO P_NUIF2_1", "WASHQS-ORTHO P_NUIF2_2", "WASHQS-ORTHO P_NUIF2_3",
                    '    '         "SCRQS-ORTHO P_NUIF2_1", "SCRQS-ORTHO P_NUIF2_2", "SCRQS-ORTHO P_NUIF2_3"

                    '    '    If lMassLink.Target.MemSub2 = 2 Then
                    '    '        If lMassLink.Source.Member = aConstituent.Substring(0, 6) Or lMassLink.Source.Member = "POQUAL" Then
                    '    '            lMassLinkFactor += lMassLink.MFact
                    '    '            'Return lMassLinkFactor
                    '    '        End If
                    '    '    End If


                    '    'Case "POQUAL-PO4_NUIF2_2", "SOQUAL-PO4_NUIF2_2", "AOQUAL-PO4_NUIF2_2", "IOQUAL-PO4_NUIF2_2",
                    '    '    "POQUAL-PO4_NUIF2_3", "SOQUAL-PO4_NUIF2_3", "AOQUAL-PO4_NUIF2_3", "IOQUAL-PO4_NUIF2_3",
                    '    '    "POQUAL-PO4_NUIF2_1", "SOQUAL-PO4_NUIF2_1", "AOQUAL-PO4_NUIF2_1", "IOQUAL-PO4_NUIF2_1",
                    '    '     "WASHQS-PO4_NUIF2_1", "WASHQS-PO4_NUIF2_2", "WASHQS-PO4_NUIF2_3",
                    '    '     "SCRQS-PO4_NUIF2_1", "SCRQS-PO4_NUIF2_2", "SCRQS-PO4_NUIF2_3"

                    '    '    If lMassLink.Target.MemSub2 = 2 Then
                    '    '        If lMassLink.Source.Member = aConstituent.Substring(0, 6) OrElse lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "SOQUAL" Then
                    '    '            lMassLinkFactor += lMassLink.MFact
                    '    '            'Return lMassLinkFactor
                    '    '        End If
                    '    '    End If

                    '    'Case "WASHQS-BOD_PKIF_4", "SOQUAL-BOD_PKIF_4", "IOQUAL-BOD_PKIF_4", "AOQUAL-BOD_PKIF_4", "POQUAL-BOD_PKIF_4"
                    '    '    If (lMassLink.Source.Member = aConstituent.Substring(0, 6) Or lMassLink.Source.Member = "POQUAL") Or
                    '    '            (lMassLink.Source.Member = "SOQUAL" AndAlso lMassLink.Source.Group = "IQUAL") Then
                    '    '        If aMultipleIndex = 1 Then
                    '    '            lMassLinkFactor = lMassLink.MFact
                    '    '        ElseIf aMultipleIndex = 2 Then
                    '    '            lMassLinkFactor = BODMFact(aUCI, aConstituent, lMassLink.MassLinkId) * aConversionFactor
                    '    '        ElseIf aMultipleIndex = 0 Then
                    '    '            lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, aConstituent, lMassLink.MassLinkId) * aConversionFactor
                    '    '        End If
                    '    '        Return lMassLinkFactor
                    '    '    End If

                    '    'Case "WASHQS-Ref-OrgP_PKIF_4", "SOQO-Ref-OrgP_PKIF_4", "IOQUAL-Ref-OrgP_PKIF_4", "AOQUAL-Ref-OrgP_PKIF_4", "POQUAL-Ref-OrgP_PKIF_4"
                    '    '    If (lMassLink.Source.Member = aConstituent.Substring(0, 6) Or lMassLink.Source.Member = "POQUAL") Or
                    '    '            (lMassLink.Source.Member = "SOQUAL" AndAlso lMassLink.Source.Group = "IQUAL") Then
                    '    '        If aMultipleIndex = 1 Then
                    '    '            lMassLinkFactor = lMassLink.MFact
                    '    '        ElseIf aMultipleIndex = 2 Then
                    '    '            lMassLinkFactor = BODMFact(aUCI, aConstituent, lMassLink.MassLinkId) * aConversionFactor
                    '    '        ElseIf aMultipleIndex = 0 Then
                    '    '            lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, aConstituent, lMassLink.MassLinkId) * aConversionFactor
                    '    '        End If
                    '    '        Return lMassLinkFactor
                    '    '    End If

                    '    'Case "WASHQS-lab-OrgP_PKIF_4", "SOQO-lab-OrgP_PKIF_4", "IOQUAL-lab-OrgP_PKIF_4", "AOQUAL-lab-OrgP_PKIF_4", "POQUAL-lab-OrgP_PKIF_4"
                    '    '    If (lMassLink.Source.Member = aConstituent.Substring(0, 6) Or lMassLink.Source.Member = "POQUAL") Or
                    '    '            (lMassLink.Source.Member = "SOQUAL" AndAlso lMassLink.Source.Group = "IQUAL") Then
                    '    '        If aMultipleIndex = 1 Then
                    '    '            lMassLinkFactor = lMassLink.MFact
                    '    '        ElseIf aMultipleIndex = 2 Then
                    '    '            lMassLinkFactor = BODMFact(aUCI, aConstituent, lMassLink.MassLinkId) * aConversionFactor
                    '    '        ElseIf aMultipleIndex = 0 Then
                    '    '            lMassLinkFactor = lMassLink.MFact + BODMFact(aUCI, aConstituent, lMassLink.MassLinkId) * aConversionFactor
                    '    '        End If
                    '    '        Return lMassLinkFactor
                    '    '    End If


                    '    'Case "PO4-P IN SOLUTION - SURFACE LAYER - OUTFLOW_NUIF1_4"
                    '    '    lMassLinkFactor = lMassLink.MFact
                    '    '    Return lMassLinkFactor
                    '    Case "PO4-P IN SOLUTION - INTERFLOW - OUTFLOW_NUIF1_4"
                    '        lMassLinkFactor = lMassLink.MFact
                    '        Return lMassLinkFactor
                    '    Case "PO4-P IN SOLUTION - GROUNDWATER - OUTFLOW_NUIF1_4"
                    '        lMassLinkFactor = lMassLink.MFact
                    '        Return lMassLinkFactor
                    '    Case "SDP4A_NUIF2_1", "SDP4A_NUIF2_2", "SDP4A_NUIF2_3", "SOQUAL-ORTHO P_NUIF2_1",
                    '            "SOQUAL-ORTHO P_NUIF2_2", "SOQUAL-ORTHO P_NUIF2_3"
                    '        If lMassLink.Target.MemSub2 = 2 Then
                    '            lMassLinkFactor += lMassLink.MFact
                    '        End If

                    '    Case "SDORP_PKIF_4"
                    '        lMassLinkFactor = lMassLink.MFact
                    '        Return lMassLinkFactor
                    '    Case "ORGN - TOTAL OUTFLOW_OXIF_2"
                    '        lMassLinkFactor = lMassLink.MFact * aConversionFactor
                    '        Return lMassLinkFactor

                    'End Select

                    'Anurag is not sure about following two cases.
                    'If (lMassLink.Source.VolName = aConstituent AndAlso lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4) Then
                    '    lMassLinkFactor += lMassLink.MFact

                    'ElseIf (lMassLink.Source.VolName = aConstituent AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2) Then
                    '    lMassLinkFactor += lMassLink.MFact * aConversionFactor

                    'End If

                Case "BOD-Labile"

                    Select Case True
                        Case (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "WASHQS" OrElse lMassLink.Source.Member = "SOQS") AndAlso
                            aConstituent = "WASHQS-BOD-Labile" AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "SCRQS" OrElse lMassLink.Source.Member = "SOQS") AndAlso
                            aConstituent = "SCRQS-BOD-Labile" AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "POQUAL") AndAlso
                            aConstituent = "SOQO-BOD-Labile" AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case (lMassLink.Source.Member = "IOQUAL" OrElse lMassLink.Source.Member = "POQUAL") AndAlso
                            aConstituent = "IOQUAL-BOD-Labile" AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case (lMassLink.Source.Member = "AOQUAL" OrElse lMassLink.Source.Member = "POQUAL") AndAlso
                            aConstituent = "AOQUAL-BOD-Labile" AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent = "ORGN - TOTAL OUTFLOW" AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                    End Select

                    'Select Case aConstituent & "_" & lMassLink.Target.Member.ToString & "_" & lMassLink.Target.MemSub1

                    '    'Case "WASHQS-BOD-Labile_OXIF_2", "SOQUAL-BOD-Labile_OXIF_2", "IOQUAL-BOD-Labile_OXIF_2", "AOQUAL-BOD-Labile_OXIF_2",
                    '    '             "POQUAL-BOD-Labile_OXIF_2", "SOQO-BOD-Labile_OXIF_2"
                    '    '    If lMassLink.Source.Member.Substring(0, 3) = aConstituent.Substring(0, 3) Or lMassLink.Source.Member = "POQUAL" Or
                    '    '                (lMassLink.Source.Member = "SOQUAL" AndAlso lMassLink.Source.Group = "IQUAL") Then
                    '    '        lMassLinkFactor = lMassLink.MFact
                    '    '        Return lMassLinkFactor
                    '    '    End If
                    '    'Case "WASHQS-BOD_OXIF_2", "SOQUAL-BOD_OXIF_2", "IOQUAL-BOD_OXIF_2", "AOQUAL-BOD_OXIF_2",
                    '    '         "POQUAL-BOD_OXIF_2", "SOQO-BOD_OXIF_2"
                    '    '    lMassLinkFactor = lMassLink.MFact
                    '    '    Return lMassLinkFactor
                    '    Case "ORGN - TOTAL OUTFLOW_OXIF_2"
                    '        lMassLinkFactor = lMassLink.MFact
                    '        Return lMassLinkFactor
                    'End Select
                Case "DO"
                    Select Case aConstituent & "_" & lMassLink.Source.Member.ToString & "_" & lMassLink.Target.Member.ToString &
                            "_" & lMassLink.Target.MemSub1
                        Case "SODOXM_SODOXM_OXIF_1", "IODOXM_IODOXM_OXIF_1", "AODOXM_AODOXM_OXIF_1",
                                 "SODOXM_PODOXM_OXIF_1", "IODOXM_PODOXM_OXIF_1", "AODOXM_PODOXM_OXIF_1"
                            lMassLinkFactor = lMassLink.MFact

                    End Select

                Case "Heat"
                    Select Case aConstituent & "_" & lMassLink.Source.Member.ToString & "_" & lMassLink.Target.Member.ToString &
                            "_" & lMassLink.Target.MemSub1
                        Case "SOHT_SOHT_IHEAT_1", "IOHT_IOHT_IHEAT_1", "AOHT_AOHT_IHEAT_1",
                                 "SOHT_POHT_IHEAT_1", "IOHT_POHT_IHEAT_1", "AOHT_POHT_IHEAT_1"
                            lMassLinkFactor = lMassLink.MFact
                    End Select
                Case Else

                    Select Case True
                        'Need to do it for sediment associated GQUAL
                        Case aConstituent.Contains("SOQO") AndAlso lMassLink.Target.Member = "IDQAL" AndAlso lMassLink.Target.MemSub1 = aGQALID AndAlso
                                (lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "SOQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent.Contains("IOQUAL") AndAlso lMassLink.Target.Member = "IDQAL" AndAlso lMassLink.Target.MemSub1 = aGQALID AndAlso
                            (lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "IOQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent.Contains("AOQUAL") AndAlso lMassLink.Target.Member = "IDQAL" AndAlso lMassLink.Target.MemSub1 = aGQALID AndAlso
                            (lMassLink.Source.Member = "POQUAL" OrElse lMassLink.Source.Member = "AOQUAL")
                            lMassLinkFactor = lMassLink.MFact
                            Return lMassLinkFactor
                        Case aConstituent.Contains("WASHQS") AndAlso lMassLink.Target.Member = "ISQAL" AndAlso lMassLink.Target.MemSub2 = aGQALID AndAlso
                                (lMassLink.Source.Member = "POQUAL" Or lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "WASHQS" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact
                        Case aConstituent.Contains("SCRQS") AndAlso lMassLink.Target.Member = "ISQAL" AndAlso lMassLink.Target.MemSub2 = aGQALID AndAlso
                            (lMassLink.Source.Member = "POQUAL" Or lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "SCRQS" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact
                        Case aConstituent.Contains("SOQS") AndAlso lMassLink.Target.Member = "ISQAL" AndAlso lMassLink.Target.MemSub2 = aGQALID AndAlso
                        (lMassLink.Source.Member = "SOQUAL" OrElse lMassLink.Source.Member = "SOQS")
                            lMassLinkFactor += lMassLink.MFact
                    End Select
            End Select

        Next

        Return lMassLinkFactor


    End Function
    Public Function BODMFact(ByVal aUCI As HspfUci, ByVal aconstituent As String, ByVal aMassLinkID As Integer) As Double
        For Each lMasslink As HspfMassLink In aUCI.MassLinks
            If lMasslink.MassLinkId = aMassLinkID AndAlso
                (lMasslink.Source.Member.Substring(0, 2) = "PO" Or lMasslink.Source.Member.Substring(0, 2) = aconstituent.Substring(0, 2)) AndAlso
                lMasslink.Target.Member = "OXIF" AndAlso
                lMasslink.Target.MemSub1 = 2 Then
                Return lMasslink.MFact

            End If
        Next lMasslink
    End Function

    Public Function ConversionFactorfromOxygen(ByVal aUCI As HspfUci, ByVal aBalanceType As String, ByVal aReach As HspfOperation) As Double
        Dim aConversionFactorFromOxygen As Double = 0.0
        Dim lOperationIndex As Integer = aUCI.OpnSeqBlock.Opns.IndexOf(aReach)
        Dim CVBO As Double = aUCI.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBO").Value
        'conversion from mg biomass to mg oxygen
        Dim CVBPC As Double = aUCI.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBPC").Value
        'conversion from biomass expressed as P to C = ratio of moles of C to moles of P in biomass		
        Dim CVBPN As Double = aUCI.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBPN").Value
        'conversion from biomass expressed as P to N = ratio of moles of N to moles of P in biomass		
        Dim BPCNTC As Double = aUCI.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("BPCNTC").Value
        'conversion from biomass expressed as P to N = ratio of moles of N to moles of P in biomass		
        Dim CVBN As Double = 14 * CVBPN * BPCNTC / 1200 / CVBPC
        'conversion from biomass to N
        Dim CVON As Double = CVBN / CVBO
        'conversion from oxygen to N
        Dim CVBP As Double = 31 * BPCNTC / 1200 / CVBPC
        'conversion from biomass to P
        Dim CVOP As Double = CVBP / CVBO
        If aBalanceType = "TN" Then
            aConversionFactorFromOxygen = CVON
        ElseIf aBalanceType = "TP" Then
            aConversionFactorFromOxygen = CVOP
        End If

        Return aConversionFactorFromOxygen


    End Function
    Public Function ConversionFactorfromBiomass(ByVal aUCI As HspfUci, ByVal aBalanceType As String, ByVal aReach As HspfOperation) As Double
        Dim aConversionFactorFromBiomass As Double = 0.0
        Dim lOperationIndex As Integer = aUCI.OpnSeqBlock.Opns.IndexOf(aReach)
        Dim CVBO As Double = aUCI.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBO").Value
        'conversion from mg biomass to mg oxygen
        Dim CVBPC As Double = aUCI.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBPC").Value
        'conversion from biomass expressed as P to C = ratio of moles of C to moles of P in biomass		
        Dim CVBPN As Double = aUCI.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("CVBPN").Value
        'conversion from biomass expressed as P to N = ratio of moles of N to moles of P in biomass		
        Dim BPCNTC As Double = aUCI.OpnSeqBlock.Opns(lOperationIndex).Tables("CONV-VAL1").Parms("BPCNTC").Value
        'conversion from biomass expressed as P to N = ratio of moles of N to moles of P in biomass		
        Dim CVBN As Double = 14 * CVBPN * BPCNTC / 1200 / CVBPC
        'conversion from biomass to N
        Dim CVON As Double = CVBN / CVBO
        'conversion from oxygen to N
        Dim CVBP As Double = 31 * BPCNTC / 1200 / CVBPC
        'conversion from biomass to P
        Dim CVOP As Double = CVBP / CVBO
        If aBalanceType = "TN" Then
            aConversionFactorFromBiomass = CVBN
        ElseIf aBalanceType = "TP" Then
            aConversionFactorFromBiomass = CVBP
        End If

        Return aConversionFactorFromBiomass


    End Function

    Public Function LocateConstituentNames(ByVal aUCI As HspfUci, ByVal aBalanceType As String, Optional ByVal aGQALID As Integer = 0) As List(Of ConstituentProperties)
        Dim QUALs As New List(Of ConstituentProperties)
        Dim QUALNames As ConstituentProperties = Nothing
        Dim QUALName As String = ""
        Dim QUALUnit As String = ""
        Dim NQUALS As Integer = 0
        Dim lOper As New HspfOperation
        For Each Oper As HspfOperation In aUCI.OpnBlks("PERLND").Ids
            If Oper.Tables("ACTIVITY").Parms("PQALFG").Value = "1" Then
                NQUALS = Oper.Tables("NQUALS").Parms("NQUAL").Value
                lOper = Oper
                'Find First operation with active PQAL
                Exit For
            End If
        Next
        'Assuming NQAL stay same for IMPLND

        Dim QUALID As Int16
        Dim lTableName As String = ""
        Dim ListContains As Boolean = False

        For Each lML As HspfMassLink In aUCI.MassLinks
            Select Case aBalanceType
                Case "BOD-Labile"
                    If (lML.Source.Group = "PQUAL" OrElse lML.Source.Group = "IQUAL") AndAlso
                            lML.Target.Group = "INFLOW" AndAlso lML.Target.Member = "OXIF" AndAlso lML.Target.MemSub1 = 2 Then

                        QUALNames = New ConstituentProperties
                        QUALID = lML.Source.MemSub1
                        If QUALID <> 1 Then
                            lTableName = "QUAL-PROPS" & ":" & QUALID
                        Else
                            lTableName = "QUAL-PROPS"
                        End If

                        QUALNames.ConstNameForEXPPlus = "BOD-Labile"
                        QUALNames.ConstituentNameInUCI = Trim(lOper.Tables(lTableName).Parms("QUALID").Value)
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ac"
                        Else
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ha"
                        End If

                        QUALNames.ReportType = aBalanceType
                        ListContains = CheckQUALList(QUALNames, QUALs)
                        If ListContains = False Then QUALs.Add(QUALNames)

                    End If

                Case "TN"

                    If (lML.Source.Group = "PQUAL" OrElse lML.Source.Group = "IQUAL") AndAlso
                        lML.Target.Group = "INFLOW" AndAlso lML.Target.Member = "NUIF1" AndAlso lML.Target.MemSub1 = 1 Then
                        QUALNames = New ConstituentProperties
                        QUALID = lML.Source.MemSub1
                        If QUALID <> 1 Then
                            lTableName = "QUAL-PROPS" & ":" & QUALID
                        Else
                            lTableName = "QUAL-PROPS"
                        End If

                        QUALNames.ConstNameForEXPPlus = "NO3"
                        QUALNames.ConstituentNameInUCI = Trim(lOper.Tables(lTableName).Parms("QUALID").Value)
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ac"
                        Else
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ha"
                        End If
                        QUALNames.ReportType = aBalanceType
                        ListContains = CheckQUALList(QUALNames, QUALs)
                        If ListContains = False Then QUALs.Add(QUALNames)

                    ElseIf (lML.Source.Group = "PQUAL" OrElse lML.Source.Group = "IQUAL") AndAlso
                        lML.Target.Group = "INFLOW" AndAlso lML.Target.Member = "NUIF1" AndAlso lML.Target.MemSub1 = 2 Then
                        QUALNames = New ConstituentProperties
                        QUALID = lML.Source.MemSub1
                        If QUALID <> 1 Then
                            lTableName = "QUAL-PROPS" & ":" & QUALID
                        Else
                            lTableName = "QUAL-PROPS"
                        End If
                        QUALNames.ConstNameForEXPPlus = "TAM"
                        QUALNames.ConstituentNameInUCI = Trim(lOper.Tables(lTableName).Parms("QUALID").Value)
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ac"
                        Else
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ha"
                        End If
                        QUALNames.ReportType = aBalanceType
                        ListContains = CheckQUALList(QUALNames, QUALs)
                        If ListContains = False Then QUALs.Add(QUALNames)

                    ElseIf (lML.Source.Group = "PQUAL" OrElse lML.Source.Group = "IQUAL") AndAlso
                        lML.Target.Group = "INFLOW" AndAlso lML.Target.Member = "PKIF" AndAlso lML.Target.MemSub1 = 3 Then
                        QUALNames = New ConstituentProperties
                        QUALID = lML.Source.MemSub1
                        If QUALID <> 1 Then
                            lTableName = "QUAL-PROPS" & ":" & QUALID
                        Else
                            lTableName = "QUAL-PROPS"
                        End If
                        QUALNames.ConstNameForEXPPlus = "Ref-OrgN"
                        QUALNames.ConstituentNameInUCI = Trim(lOper.Tables(lTableName).Parms("QUALID").Value)
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ac"
                        Else
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ha"
                        End If
                        QUALNames.ReportType = aBalanceType
                        ListContains = CheckQUALList(QUALNames, QUALs)
                        If ListContains = False Then QUALs.Add(QUALNames)

                    ElseIf (lML.Source.Group = "PQUAL" OrElse lML.Source.Group = "IQUAL") AndAlso
                        lML.Target.Group = "INFLOW" AndAlso lML.Target.Member = "OXIF" AndAlso lML.Target.MemSub1 = 2 Then
                        QUALNames = New ConstituentProperties
                        QUALID = lML.Source.MemSub1
                        If QUALID <> 1 Then
                            lTableName = "QUAL-PROPS" & ":" & QUALID
                        Else
                            lTableName = "QUAL-PROPS"
                        End If
                        QUALNames.ConstNameForEXPPlus = "lab-OrgN"
                        QUALNames.ConstituentNameInUCI = Trim(lOper.Tables(lTableName).Parms("QUALID").Value)
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ac"
                        Else
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ha"
                        End If
                        QUALNames.ReportType = aBalanceType
                        ListContains = CheckQUALList(QUALNames, QUALs)
                        If ListContains = False Then QUALs.Add(QUALNames)

                    End If

                Case "TP"

                    If (lML.Source.Group = "PQUAL" OrElse lML.Source.Group = "IQUAL") AndAlso
                            lML.Target.Group = "INFLOW" AndAlso lML.Target.Member = "NUIF1" AndAlso lML.Target.MemSub1 = 4 Then
                        QUALNames = New ConstituentProperties
                        QUALID = lML.Source.MemSub1
                        If QUALID <> 1 Then
                            lTableName = "QUAL-PROPS" & ":" & QUALID
                        Else
                            lTableName = "QUAL-PROPS"
                        End If
                        QUALNames.ConstNameForEXPPlus = "PO4"
                        QUALNames.ConstituentNameInUCI = Trim(lOper.Tables(lTableName).Parms("QUALID").Value)
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ac"
                        Else
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ha"
                        End If
                        QUALNames.ReportType = aBalanceType
                        ListContains = CheckQUALList(QUALNames, QUALs)
                        If ListContains = False Then QUALs.Add(QUALNames)

                    ElseIf (lML.Source.Group = "PQUAL" OrElse lML.Source.Group = "IQUAL") AndAlso
                            lML.Target.Group = "INFLOW" AndAlso lML.Target.Member = "PKIF" AndAlso lML.Target.MemSub1 = 4 Then
                        QUALNames = New ConstituentProperties
                        QUALID = lML.Source.MemSub1
                        If QUALID <> 1 Then
                            lTableName = "QUAL-PROPS" & ":" & QUALID
                        Else
                            lTableName = "QUAL-PROPS"
                        End If
                        QUALNames.ConstNameForEXPPlus = "Ref-OrgP"
                        QUALNames.ConstituentNameInUCI = Trim(lOper.Tables(lTableName).Parms("QUALID").Value)
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ac"
                        Else
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ha"
                        End If
                        QUALNames.ReportType = aBalanceType
                        ListContains = CheckQUALList(QUALNames, QUALs)
                        If ListContains = False Then QUALs.Add(QUALNames)

                    ElseIf (lML.Source.Group = "PQUAL" OrElse lML.Source.Group = "IQUAL") AndAlso
                            lML.Target.Group = "INFLOW" AndAlso lML.Target.Member = "OXIF" AndAlso lML.Target.MemSub1 = 2 Then
                        QUALNames = New ConstituentProperties
                        QUALID = lML.Source.MemSub1
                        If QUALID <> 1 Then
                            lTableName = "QUAL-PROPS" & ":" & QUALID
                        Else
                            lTableName = "QUAL-PROPS"
                        End If
                        QUALNames.ConstNameForEXPPlus = "lab-OrgP"
                        QUALNames.ConstituentNameInUCI = Trim(lOper.Tables(lTableName).Parms("QUALID").Value)
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ac"
                        Else
                            QUALNames.ConstituentUnit = Trim(lOper.Tables(lTableName).Parms("QTYID").Value) & "/ha"
                        End If
                        QUALNames.ReportType = aBalanceType
                        ListContains = CheckQUALList(QUALNames, QUALs)
                        If ListContains = False Then QUALs.Add(QUALNames)

                    End If

            End Select

        Next lML
        If QUALNames Is Nothing Then
            Logger.Msg("No appropriate MASS-LINK connections were found for " & aBalanceType & ". The program will quit", vbOKOnly)
            Return Nothing
        End If


        Select Case aBalanceType
            Case "Water"
                QUALNames = New ConstituentProperties
                QUALNames.ConstNameForEXPPlus = "Water"
                QUALNames.ConstituentNameInUCI = "Water"
                If aUCI.GlobalBlock.EmFg = 1 Then
                    QUALNames.ConstituentUnit = "in"
                Else
                    QUALNames.ConstituentUnit = "mm"
                End If
                QUALNames.ReportType = aBalanceType
                QUALs.Add(QUALNames)
                Return QUALs

            Case "Sediment"
                QUALNames = New ConstituentProperties
                QUALNames.ConstNameForEXPPlus = "Sediment"
                QUALNames.ConstituentNameInUCI = "Sediment"
                If aUCI.GlobalBlock.EmFg = 1 Then
                    QUALNames.ConstituentUnit = "ton/ac"
                Else
                    QUALNames.ConstituentUnit = "tonne/ha"
                End If
                QUALNames.ReportType = aBalanceType
                QUALs.Add(QUALNames)
                Return QUALs
            Case "DO"
                QUALNames = New ConstituentProperties
                QUALNames.ConstNameForEXPPlus = "DO"
                QUALNames.ConstituentNameInUCI = "DO"
                If aUCI.GlobalBlock.EmFg = 1 Then
                    QUALNames.ConstituentUnit = "lbs/ac"
                Else
                    QUALNames.ConstituentUnit = "kg/ha"
                End If
                QUALNames.ReportType = aBalanceType
                QUALs.Add(QUALNames)
                Return QUALs

            Case "Heat"
                QUALNames = New ConstituentProperties
                QUALNames.ConstNameForEXPPlus = "Heat"
                QUALNames.ConstituentNameInUCI = "Heat"
                If aUCI.GlobalBlock.EmFg = 1 Then
                    QUALNames.ConstituentUnit = "BTU/ac"
                Else
                    QUALNames.ConstituentUnit = "kcal/ha"
                End If
                QUALNames.ReportType = aBalanceType
                QUALs.Add(QUALNames)
                Return QUALs
            Case "TN"
                QUALNames = New ConstituentProperties
                QUALNames.ConstNameForEXPPlus = "TN"
                QUALNames.ConstituentNameInUCI = "TN"

                If aUCI.GlobalBlock.EmFg = 1 Then
                    QUALNames.ConstituentUnit = QUALs(0).ConstituentUnit
                Else
                    QUALNames.ConstituentUnit = QUALs(0).ConstituentUnit
                End If


                QUALNames.ReportType = aBalanceType
                QUALs.Add(QUALNames)
                Return QUALs

            Case "TP"
                QUALNames = New ConstituentProperties
                QUALNames.ConstNameForEXPPlus = "TP"
                QUALNames.ConstituentNameInUCI = "TP"
                If aUCI.GlobalBlock.EmFg = 1 Then
                    QUALNames.ConstituentUnit = QUALs(0).ConstituentUnit
                Else
                    QUALNames.ConstituentUnit = QUALs(0).ConstituentUnit
                End If
                QUALNames.ReportType = aBalanceType
                QUALs.Add(QUALNames)
                Return QUALs
            Case "BOD-Labile"
                Return QUALs

            Case Else
                QUALNames = New ConstituentProperties
                QUALNames.ConstNameForEXPPlus = aBalanceType
                QUALNames.ConstituentNameInUCI = aBalanceType
                lTableName = "QUAL-PROPS"
                'If aGQALID > 1 Then lTableName = lTableName & ":" & aGQALID
                Dim lTempQual As String = ""
                Dim lUnits As String = ""

                If lOper.TableExists(lTableName) Then
                    lTempQual = Trim(lOper.Tables(lTableName).Parms("QUALID").Value)
                    If aBalanceType = lTempQual Then
                        'found it
                        lUnits = Trim(lOper.Tables(lTableName).Parms("QTYID").Value)
                    End If
                End If
                Do While lUnits.Length = 0
                    For lIndex As Integer = 2 To 10
                        If lOper.TableExists(lTableName & ":" & lIndex.ToString) Then
                            lTempQual = Trim(lOper.Tables(lTableName & ":" & lIndex.ToString).Parms("QUALID").Value)
                            If aBalanceType = lTempQual Then
                                'found it
                                lUnits = Trim(lOper.Tables(lTableName & ":" & lIndex.ToString).Parms("QTYID").Value)
                                Exit For
                            End If
                        End If
                    Next
                Loop

                If aUCI.GlobalBlock.EmFg = 1 Then
                    QUALNames.ConstituentUnit = lUnits & "/ac"
                Else
                    QUALNames.ConstituentUnit = lUnits & "/ha"
                End If

                QUALNames.ReportType = aBalanceType
                QUALs.Add(QUALNames)
                Return QUALs
        End Select


        Return QUALs
    End Function
    Private Function CheckQUALList(item1 As ConstituentProperties, item2 As List(Of ConstituentProperties)) As Boolean
        Dim itemsEqual As Boolean = False
        For Each item As ConstituentProperties In item2
            If item.ConstNameForEXPPlus = item1.ConstNameForEXPPlus AndAlso
                                    item.ConstituentNameInUCI = item1.ConstituentNameInUCI AndAlso
                                    item.ConstituentUnit = item1.ConstituentUnit AndAlso
                                    item.ReportType = item1.ReportType Then
                itemsEqual = True
                Exit For
            End If
        Next

        Return itemsEqual
    End Function

    Public Function ConstituentList(ByVal aBalanceType As String, Optional ByVal QualityConstituent As String = "",
                                     Optional ByVal EXPPlusName As String = "", Optional ByVal AgChemConst As Boolean = False,
                                    Optional ByVal aOperName As String = "") As Dictionary(Of String, String)
        'Design for AGCHEM Case as well.
        Dim lOutflowDataType As New Dictionary(Of String, String)
        Select Case aBalanceType
            Case "Water", "WAT"
                lOutflowDataType.Add("SUPY", "SUPY")
                lOutflowDataType.Add("IRRAPP6", "IRRAPP6")
                lOutflowDataType.Add("SURO", "SURO")
                lOutflowDataType.Add("IFWO", "IFWO")
                lOutflowDataType.Add("AGWO", "AGWO")
                lOutflowDataType.Add("TotalOutflow", "TotalOutflow")
                lOutflowDataType.Add("IGWI", "IGWI")
                lOutflowDataType.Add("AGWI", "AGWI")
                lOutflowDataType.Add("AGWLI", "AGWLI")
                lOutflowDataType.Add("PET", "PET")
                lOutflowDataType.Add("CEPE", "CEPE")
                lOutflowDataType.Add("UZET", "UZET")
                lOutflowDataType.Add("LZET", "LZET")
                lOutflowDataType.Add("AGWET", "AGWET")
                lOutflowDataType.Add("BASET", "BASET")
                lOutflowDataType.Add("TAET", "TAET")
                lOutflowDataType.Add("IMPEV", "IMPEV")
            Case "Sediment", "SED"
                lOutflowDataType.Add("WSSD", "WSSD")
                lOutflowDataType.Add("SCRSD", "SCRSD")
                lOutflowDataType.Add("SOSLD", "SOSLD")
                lOutflowDataType.Add("TotalOutflow", "TotalOutflow")
            Case "DO"
                lOutflowDataType.Add("SODOXM", "SODOXM")
                lOutflowDataType.Add("IODOXM", "IODOXM")
                lOutflowDataType.Add("AODOXM", "AODOXM")
                lOutflowDataType.Add("TotalOutflow", "TotalOutflow")

            Case "Heat"
                lOutflowDataType.Add("SOHT", "SOHT")
                lOutflowDataType.Add("IOHT", "IOHT")
                lOutflowDataType.Add("AOHT", "AOHT")
                lOutflowDataType.Add("TotalOutflow", "TotalOutflow")

            Case "TotalN", "TotalP", "TN", "TP"
                If EXPPlusName = "TAM" Then EXPPlusName = "NH3+NH4"
                If aOperName = "PERLND" Then
                    lOutflowDataType.Add("WASHQS" & "-" & EXPPlusName, "WASHQS" & "-" & QualityConstituent)
                    lOutflowDataType.Add("SCRQS" & "-" & EXPPlusName, "SCRQS" & "-" & QualityConstituent)
                    lOutflowDataType.Add("SOQO" & "-" & EXPPlusName, "SOQO" & "-" & QualityConstituent)
                    lOutflowDataType.Add("IOQUAL" & "-" & EXPPlusName, "IOQUAL" & "-" & QualityConstituent)
                    lOutflowDataType.Add("AOQUAL" & "-" & EXPPlusName, "AOQUAL" & "-" & QualityConstituent)
                ElseIf aOperName = "IMPLND" Then
                    lOutflowDataType.Add("SOQS" & "-" & EXPPlusName, "SOQS" & "-" & QualityConstituent)
                    lOutflowDataType.Add("SOQO" & "-" & EXPPlusName, "SOQO" & "-" & QualityConstituent)
                End If
                lOutflowDataType.Add("TotalOutflow" & "-" & EXPPlusName, "TotalOutflow" & "-" & QualityConstituent)
            Case Else
                'case for GQuals
                If aOperName = "PERLND" Then
                    lOutflowDataType.Add("WASHQS" & "-" & EXPPlusName, "WASHQS" & "-" & QualityConstituent)
                    lOutflowDataType.Add("SCRQS" & "-" & EXPPlusName, "SCRQS" & "-" & QualityConstituent)
                    lOutflowDataType.Add("SOQO" & "-" & EXPPlusName, "SOQO" & "-" & QualityConstituent)
                    lOutflowDataType.Add("IOQUAL" & "-" & EXPPlusName, "IOQUAL" & "-" & QualityConstituent)
                    lOutflowDataType.Add("AOQUAL" & "-" & EXPPlusName, "AOQUAL" & "-" & QualityConstituent)
                ElseIf aOperName = "IMPLND" Then
                    lOutflowDataType.Add("SOQS" & "-" & EXPPlusName, "SOQS" & "-" & QualityConstituent)
                    lOutflowDataType.Add("SOQO" & "-" & EXPPlusName, "SOQO" & "-" & QualityConstituent)
                End If
                lOutflowDataType.Add("TotalOutflow" & "-" & EXPPlusName, "TotalOutflow" & "-" & QualityConstituent)
        End Select

        Return lOutflowDataType
    End Function

    Public Function FindDownStreamExitNumber(ByVal aUCI As HspfUci,
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

    Public Function GQualUnits(ByVal aUCI As HspfUci, ByVal aGQualName As String) As String
        'given a uci and gqualname, return the units of the gqual
        Dim lUnits As String = ""
        'Dim GQALID As Integer = Right(aGQualName, 1)
        Dim lOper As New HspfOperation
        For Each Oper As HspfOperation In aUCI.OpnBlks("RCHRES").Ids
            If Oper.Tables("ACTIVITY").Parms("GQALFG").Value = "1" Then
                lOper = Oper
                'Find First operation with active GQALFG
                Exit For
            End If
        Next

        If lOper IsNot Nothing Then
            Dim lTableName As String = "GQ-QALDATA"
            'If GQALID > 1 Then lTableName = lTableName & ":" & GQALID
            Dim lTempQual As String = ""
            If lOper.TableExists(lTableName) Then
                lTempQual = Trim(lOper.Tables(lTableName).Parms("GQID").Value)
                If aGQualName = lTempQual Then
                    'found it
                    lUnits = Trim(lOper.Tables(lTableName).Parms("QTYID").Value)
                End If
            End If
            'Do While lUnits.Length = 0
            For lIndex As Integer = 2 To 10
                If lOper.TableExists(lTableName & ":" & lIndex.ToString) Then
                    lTempQual = Trim(lOper.Tables(lTableName & ":" & lIndex.ToString).Parms("GQID").Value)
                    If aGQualName = lTempQual Then
                        'found it
                        lUnits = Trim(lOper.Tables(lTableName & ":" & lIndex.ToString).Parms("QTYID").Value)
                        Exit For
                    End If
                End If
            Next
            'Loop
        End If

        Return lUnits
    End Function


    Public Function AreaReportInTableFormat(ByVal aUCI As HspfUci, ByVal aOperationTypes As atcCollection, ByVal aLocation As String) As DataTable

        Dim lAreaTable As DataTable
        lAreaTable = New DataTable("ModelAreaTable")
        Dim lColumn As DataColumn

        lColumn = New DataColumn()
        lColumn.ColumnName = "Landuse"
        lColumn.Caption = "Landuse Category"
        lColumn.DataType = Type.GetType("System.String")
        lAreaTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.ColumnName = "PervArea"
        lColumn.Caption = "Pervious Area (ac)"
        lColumn.DataType = Type.GetType("System.Double")
        lAreaTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.ColumnName = "ImpervArea"
        lColumn.Caption = "Impervious Area (ac)"
        lColumn.DataType = Type.GetType("System.Double")
        lAreaTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.ColumnName = "TotalArea"
        lColumn.Caption = "Total Area (ac)"
        lColumn.DataType = Type.GetType("System.Double")
        lAreaTable.Columns.Add(lColumn)

        Dim lRow As DataRow

        Dim lContributingLandUseAreas As atcCollection = ContributingLandUseAreas(aUCI, aOperationTypes, aLocation)

        Dim lTotalAreaFromLandUses As Double = 0
        Dim lTotalAreaPerv As Double = 0.0
        Dim lTotalAreaImpr As Double = 0.0
        For lLandUseIndex As Integer = 0 To lContributingLandUseAreas.Count - 1
            Dim lLandUseAreaString As String = lContributingLandUseAreas.Item(lLandUseIndex)
            Dim lImprArea As Double = StrRetRem(lLandUseAreaString)
            Dim lPervArea As Double = 0
            If lLandUseAreaString.Length > 0 Then
                lPervArea = lLandUseAreaString
            End If

            Dim lLandUseArea As Double = lPervArea + lImprArea

            lRow = lAreaTable.NewRow
            lRow("Landuse") = lContributingLandUseAreas.Keys(lLandUseIndex).ToString.PadLeft(20)
            lRow("PervArea") = DecimalAlign(lPervArea, , 2, 7)
            lRow("ImpervArea") = DecimalAlign(lImprArea, , 2, 7)
            lRow("TotalArea") = DecimalAlign(lLandUseArea, , 2, 7)
            lAreaTable.Rows.Add(lRow)

            lTotalAreaPerv += lPervArea
            lTotalAreaImpr += lImprArea
            lTotalAreaFromLandUses += lLandUseArea
        Next
        lContributingLandUseAreas.Clear()

        lRow = lAreaTable.NewRow
        lAreaTable.Rows.Add(lRow)

        lRow = lAreaTable.NewRow
        lRow("Landuse") = "Total"
        lRow("PervArea") = DecimalAlign(lTotalAreaPerv, , 2, 7)
        lRow("ImpervArea") = DecimalAlign(lTotalAreaImpr, , 2, 7)
        lRow("TotalArea") = DecimalAlign(lTotalAreaFromLandUses, , 2, 7)
        lAreaTable.Rows.Add(lRow)
        Return lAreaTable
    End Function

End Module
