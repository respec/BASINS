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

    Public Function CfsToInches(ByVal aTSerIn As atcTimeseries, ByVal aArea As Double) As atcTimeseries
        Dim lConversionFactor As Double = (12.0# * 24.0# * 3600.0#) / (aArea * 43560.0#)   'cfs days to inches
        Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
        Dim lArgsMath As New atcDataAttributes
        lArgsMath.SetValue("timeseries", aTSerIn)
        lArgsMath.SetValue("number", lConversionFactor)
        lTsMath.Open("multiply", lArgsMath)
        Return lTsMath.DataSets(0)
    End Function

    Public Function InchesToCfs(ByVal aTSerIn As atcTimeseries, ByVal aArea As Double) As atcTimeseries
        Dim lConversionFactor As Double = (aArea * 43560.0#) / (12.0# * 24.0# * 3600.0#) 'inches to cfs days
        Dim lTsMath As atcDataSource = New atcTimeseriesMath.atcTimeseriesMath
        Dim lArgsMath As New atcDataAttributes
        lArgsMath.SetValue("timeseries", aTSerIn)
        lArgsMath.SetValue("number", lConversionFactor)
        lTsMath.Open("multiply", lArgsMath)
        Return lTsMath.DataSets(0)
    End Function

    Friend Sub CheckDateJ(ByVal aTSer As atcTimeseries, ByVal aName As String, _
                           ByRef aSDateJ As Double, ByRef aEDateJ As Double, ByRef aStr As String)
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

    Friend Function Attributes(ByVal aName As String, _
                               ByVal aTSer1 As atcTimeseries, _
                               ByVal aTSer2 As atcTimeseries, _
                      Optional ByVal aTSerOpt As atcTimeseries = Nothing) As String
        Dim lStr As String = ""
        If aTSerOpt Is Nothing Then
            lStr = aName.PadLeft(36)
        Else
            lStr = aName.PadLeft(24) & DecimalAlign(aTSerOpt.Attributes.GetFormattedValue(aName), 12, 2)
        End If
        lStr &= DecimalAlign(aTSer1.Attributes.GetValue(aName), 12, 2) & _
                DecimalAlign(aTSer2.Attributes.GetValue(aName), 12, 2) & vbCrLf
        Return lStr
    End Function

    Friend Function IntervalReport(ByVal aSite As String, ByVal aTimeUnit As atcTimeUnit, _
                                   ByVal aTser1 As atcTimeseries, _
                                   ByVal aTSer2 As atcTimeseries, _
                          Optional ByVal aListIntervalValues As Boolean = False, _
                          Optional ByVal aTserOpt As atcTimeseries = Nothing) As String
        Dim lInterval As String = ""
        Select Case aTimeUnit
            Case atcTimeUnit.TUDay : lInterval = "Daily"
            Case atcTimeUnit.TUMonth : lInterval = "Monthly"
            Case atcTimeUnit.TUYear : lInterval = "Annual"
        End Select
        Dim lUnits As String = aTser1.Attributes.GetValue("Units", "cfs")
        Dim lTran As atcTran = atcTran.TranAverSame
        If lUnits.Contains("inches") Then lTran = atcTran.TranSumDiv

        Dim lStr As String = ""
        lStr &= aSite & ":" & lInterval.PadLeft(16) & vbCrLf

        Dim lPadLength As Integer = 36
        Dim lTserOptCons As String = ""
        If aTserOpt IsNot Nothing Then lTserOptCons = aTserOpt.Attributes.GetValue("Constituent", "")
        If lTserOptCons = "PREC" Then lTserOptCons = "Precip"
        If lTserOptCons.Length > 0 Then
            lPadLength = 24
        End If
        If aListIntervalValues Then
            If lInterval = "Annual" Then
                lStr &= "Year".PadLeft(lPadLength)
            Else
                lStr &= "Date".PadLeft(lPadLength)
            End If
            If lTserOptCons.Length > 0 Then
                lStr &= lTserOptCons.PadLeft(36 - lPadLength)
            End If
        Else
            lStr &= Space(lPadLength)
        End If
        lStr &= "Simulated".PadLeft(12) & "Observed".PadLeft(12)
        If aListIntervalValues Then
            lStr &= "Residual".PadLeft(12) & "% Error".PadLeft(12)
        End If
        lStr &= vbCrLf

        Dim lTSer1 As atcTimeseries = Aggregate(aTser1, aTimeUnit, 1, lTran)
        Dim lTSer2 As atcTimeseries = Aggregate(aTSer2, aTimeUnit, 1, lTran)
        Dim lTSerOpt As atcTimeseries = Nothing
        If aTserOpt IsNot Nothing Then
            lTSerOpt = Aggregate(aTserOpt, aTimeUnit, 1, lTran)
        End If

        If aListIntervalValues Then
            Dim lDateFormat As New atcDateFormat
            With lDateFormat
                .IncludeMonths = False
                .IncludeDays = False
                .IncludeHours = False
                .IncludeMinutes = False
            End With
            For lIndex As Integer = 1 To lTSer1.numValues
                Dim lValue1 As Double = lTSer1.Value(lIndex)
                Dim lValue2 As Double = lTSer2.Value(lIndex)
                Dim lResidual As Double = lValue1 - lValue2
                lStr &= lDateFormat.JDateToString(lTSer1.Dates.Value(lIndex)).PadLeft(lPadLength)
                If lTSerOpt IsNot Nothing Then
                    lStr &= DecimalAlign(lTSerOpt.Value(lIndex), 12, 2).PadLeft(36 - lPadLength)
                End If
                lStr &= DecimalAlign(lValue1, 12, 2).PadLeft(12) _
                      & DecimalAlign(lValue2, 12, 2).PadLeft(12) _
                      & DecimalAlign(lResidual, 12, 2).PadLeft(12) _
                      & DecimalAlign(100 * lResidual / lValue2, 11, 2).PadLeft(11) & "%" & vbCrLf
            Next
            lStr &= vbCrLf
        End If

        lStr &= Attributes("Count", lTSer1, lTSer2, lTSerOpt)
        If lTran = atcTran.TranSumDiv Then
            lStr &= Attributes("Sum", lTSer1, lTSer2, lTSerOpt)
        End If
        lStr &= Attributes("Mean", lTSer1, lTSer2, lTSerOpt)
        lStr &= Attributes("Geometric Mean", lTSer1, lTSer2, lTSerOpt)

        lStr &= vbCrLf & CompareStats(lTSer1, lTSer2)

        lTSer1.Clear()
        lTSer1.Dates.Clear()
        lTSer1 = Nothing
        lTSer2.Clear()
        lTSer2.Dates.Clear()
        lTSer2 = Nothing
        lStr &= vbCrLf & vbCrLf
        Return lStr
    End Function

    Function CompareStats(ByVal aTSer1 As atcTimeseries, _
                          ByVal aTSer2 As atcTimeseries) As String
        Dim lStr As String = ""
        Dim lNote As String = ""
        Dim lMeanError As Double = 0.0#
        Dim lMeanAbsoluteError As Double = 0.0#
        Dim lRmsError As Double = 0.0#
        Dim lValDiff As Double
        Dim lVal1 As Double
        Dim lVal2 As Double
        Dim lSkipCount As Integer = 0
        Dim lGoodCount As Integer = 0

        For lIndex As Integer = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)
            lVal2 = aTSer2.Values(lIndex)
            If Not Double.IsNaN(lVal1) And Not Double.IsNaN(lVal2) Then
                lValDiff = lVal1 - lVal2
                lMeanError += lValDiff
                lMeanAbsoluteError += Math.Abs(lValDiff)
                lRmsError += lValDiff * lValDiff
                lGoodCount += 1
            Else
                lSkipCount += 1
                If lSkipCount = 1 Then
                    lNote = "*** Note - compare skipped index " & lIndex
                End If
            End If
        Next
        If lNote.Length > 0 Then
            lNote &= " and " & lSkipCount - 1 & " more" & vbCrLf
        End If

        Dim lNashSutcliffeNumerator As Double = lRmsError

        lMeanError /= lGoodCount
        lMeanAbsoluteError /= lGoodCount
        lRmsError /= lGoodCount

        If lRmsError > 0 Then
            lRmsError = Math.Sqrt(lRmsError)
        End If

        Dim lCorrelationCoefficient As Double = 0.0#
        Dim lNashSutcliffe As Double = 0.0#
        Dim lMean1 As Double = aTSer1.Attributes.GetValue("Mean")
        Dim lMean2 As Double = aTSer2.Attributes.GetValue("Mean")
        For lIndex As Integer = 1 To aTSer1.numValues
            lVal1 = aTSer1.Values(lIndex)
            lVal2 = aTSer2.Values(lIndex)
            If Not Double.IsNaN(lVal1) And Not Double.IsNaN(lVal2) Then
                lCorrelationCoefficient += (lVal1 - lMean1) * (lVal2 - lMean2)
                lNashSutcliffe += (lVal2 - lMean2) ^ 2
            End If
        Next
        lCorrelationCoefficient /= (aTSer1.numValues - 1)
        Dim lSD1 As Double = aTSer1.Attributes.GetValue("Standard Deviation")
        Dim lSD2 As Double = aTSer2.Attributes.GetValue("Standard Deviation")
        If Math.Abs(lSD1 * lSD2) > 0.0001 Then
            lCorrelationCoefficient /= (lSD1 * lSD2)
        End If
        If lNashSutcliffe > 0 Then
            lNashSutcliffe = lNashSutcliffeNumerator / lNashSutcliffe
        End If

        lStr &= "Correlation Coefficient".PadLeft(36) & DecimalAlign(lCorrelationCoefficient, 18) & vbCrLf
        lStr &= "Coefficient of Determination".PadLeft(36) & DecimalAlign(lCorrelationCoefficient ^ 2, 18) & vbCrLf
        lStr &= "Mean Error".PadLeft(36) & DecimalAlign(lMeanError, 18) & vbCrLf
        lStr &= "Mean Absolute Error".PadLeft(36) & DecimalAlign(lMeanAbsoluteError, 18) & vbCrLf
        lStr &= "RMS Error".PadLeft(36) & DecimalAlign(lRmsError, 18) & vbCrLf
        lStr &= "Model Fit Efficiency".PadLeft(36) & DecimalAlign(1 - lNashSutcliffe, 18) & vbCrLf
        If lNote.Length > 0 Then
            lStr &= lNote
        End If
        Return lStr
    End Function

    Friend Function SimulationPeriodString(ByVal aSDatej As Double, ByVal aEDatej As Double, ByVal aYrCnt As Integer) As String
        Dim lDate(5) As Integer
        J2Date(aSDatej, lDate)
        Dim lYearType As String = " "
        If lDate(1) = 10 AndAlso lDate(2) = 1 AndAlso lDate(3) = 0 Then
            lYearType &= "Water "
        ElseIf lDate(1) = 10 AndAlso lDate(2) = 1 AndAlso lDate(3) = 0 Then
            lYearType &= "Calendar "
        End If
        Dim lStr As String = "Simulation Period: " & aYrCnt & lYearType & "years"
        Dim lDateFormat As New atcDateFormat
        With lDateFormat
            .IncludeHours = False
            .IncludeMinutes = False
            .Midnight24 = False
            lStr &= " from " & .JDateToString(aSDatej)
            .Midnight24 = True
            lStr &= " to " & .JDateToString(aEDatej) & vbCrLf
        End With
        Return lStr
    End Function
End Module
