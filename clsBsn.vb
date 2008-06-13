Partial Class SwatInput
    Private pBsn As clsBsn = New clsBsn(Me)
    ReadOnly Property Bsn() As clsBsn
        Get
            Return pBsn
        End Get
    End Property

    ''' <summary>
    ''' Basin (BSN) Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsBsn
        Private pSwatInput As SwatInput
        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub
        Public Function Table() As DataTable
            pSwatInput.Status("Reading BSN from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM bsn;")
        End Function
        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            Dim lBsnRow As DataRow = aTable.Rows(0)
            pSwatInput.Status("Writing BSN text ...")

            Dim lSB As New System.Text.StringBuilder
            lSB.AppendLine("Basin data" + Space(10) + " .bsn file " & DateNowString & " ARCGIS-SWAT - SWAT interface")
            lSB.AppendLine("Modeling Options: Land Area")
            lSB.AppendLine("Water Balance:")
            lSB.AppendLine(MakeString(lBsnRow.Item(1), 3, 4, 8) + "| SFTMP : Snowfall temperature [ºC]")
            lSB.AppendLine(MakeString(lBsnRow.Item(2), 3, 4, 8) + "| SMTMP : Snow melt base temperature [ºC]")
            lSB.AppendLine(MakeString(lBsnRow.Item(3), 3, 4, 8) + "| SMFMX : Melt factor for snow on June 21 [mm H2O/ºC-day]")
            lSB.AppendLine(MakeString(lBsnRow.Item(4), 3, 4, 8) + "| SMFMN : Melt factor for snow on December 21 [mm H2O/ºC-day]")
            lSB.AppendLine(MakeString(lBsnRow.Item(5), 3, 4, 8) + "| TIMP : Snow pack temperature lag factor")
            lSB.AppendLine(MakeString(lBsnRow.Item(6), 3, 4, 8) + "| SNOCOVMX : Minimum snow water content that corresponds to 100% snow cover [mm]")
            lSB.AppendLine(MakeString(lBsnRow.Item(7), 3, 4, 8) + "| SNO50COV : Fraction of snow volume represented by SNOCOVMX that corresponds to 50% snow cover")
            lSB.AppendLine(MakeString(lBsnRow.Item(8), 0, 4, 4) + "| IPET: PET method: 0=priest-t, 1=pen-m, 2=har, 3=read into model")
            lSB.AppendLine(lBsnRow.Item(75).ToString.PadLeft(16) + "    | PETFILE: name of potential ET input file")
            lSB.AppendLine(MakeString(lBsnRow.Item(9), 3, 4, 8) + "| ESCO: soil evaporation compensation factor")
            lSB.AppendLine(MakeString(lBsnRow.Item(10), 3, 4, 8) + "| EPCO: plant water uptake compensation factor")
            lSB.AppendLine(MakeString(lBsnRow.Item(11), 3, 4, 8) + "| EVLAI : Leaf area index at which no evaporation occurs from water surface [m2/m2]")
            lSB.AppendLine(MakeString(lBsnRow.Item(12), 3, 4, 8) + "| FFCB : Initial soil water storage expressed as a fraction of field capacity water content")
            lSB.AppendLine("Surface Runoff:")
            lSB.AppendLine(MakeString(lBsnRow.Item(13), 0, 4, 4) + "| IEVENT: rainfall/runoff code: 0=daily rainfall/CN")
            lSB.AppendLine(MakeString(lBsnRow.Item(14), 0, 4, 4) + "| ICRK: crack flow code: 1=model crack flow in soil")
            lSB.AppendLine(MakeString(lBsnRow.Item(15), 3, 4, 8) + "| SURLAG : Surface runoff lag time [days]")
            lSB.AppendLine(MakeString(lBsnRow.Item(16), 3, 4, 8) + "| ADJ_PKR : Peak rate adjustment factor for sediment routing in the subbasin (tributary channels)")
            lSB.AppendLine(MakeString(lBsnRow.Item(17), 3, 4, 8) + "| PRF : Peak rate adjustment factor for sediment routing in the main channel")
            lSB.AppendLine(MakeString(lBsnRow.Item(18), 3, 4, 8) + "| SPCON : Linear parameter for calculating the maximum amount of sediment that can be reentrained during channel sediment routing")
            lSB.AppendLine(MakeString(lBsnRow.Item(19), 3, 4, 8) + "| SPEXP : Exponent parameter for calculating sediment reentrained in channel sediment routing")
            lSB.AppendLine("Nutrient Cycling:")
            lSB.AppendLine(MakeString(lBsnRow.Item(20), 3, 4, 8) + "| RCN : Concentration of nitrogen in rainfall [mg N/l]")
            lSB.AppendLine(MakeString(lBsnRow.Item(21), 3, 4, 8) + "| CMN : Rate factor for humus mineralization of active organic nitrogen")
            lSB.AppendLine(MakeString(lBsnRow.Item(22), 3, 4, 8) + "| N_UPDIS : Nitrogen uptake distribution parameter")
            lSB.AppendLine(MakeString(lBsnRow.Item(23), 3, 4, 8) + "| P_UPDIS : Phosphorus uptake distribution parameter")
            lSB.AppendLine(MakeString(lBsnRow.Item(24), 3, 4, 8) + "| NPERCO : Nitrogen percolation coefficient")
            lSB.AppendLine(MakeString(lBsnRow.Item(25), 3, 4, 8) + "| PPERCO : Phosphorus percolation coefficient")
            lSB.AppendLine(MakeString(lBsnRow.Item(26), 3, 4, 8) + "| PHOSKD : Phosphorus soil partitioning coefficient")
            lSB.AppendLine(MakeString(lBsnRow.Item(27), 3, 4, 8) + "| PSP : Phosphorus sorption coefficient")
            lSB.AppendLine(MakeString(lBsnRow.Item(28), 3, 4, 8) + "| RSDCO : Residue decomposition coefficient")
            lSB.AppendLine("Pesticide Cycling:")
            lSB.AppendLine(MakeString(lBsnRow.Item(29), 3, 4, 8) + "| PERCOP : Pesticide percolation coefficient")
            lSB.AppendLine("Algae/CBOD/Dissolved Oxygen:")
            lSB.AppendLine(MakeString(lBsnRow.Item(30), 0, 4, 4) + "| ISUBWQ: subbasin water quality parameter")
            lSB.AppendLine("Bacteria:")
            lSB.AppendLine(MakeString(lBsnRow.Item(31), 3, 4, 8) + "| WDPQ : Die-off factor for persistent bacteria in soil solution. [1/day]")
            lSB.AppendLine(MakeString(lBsnRow.Item(32), 3, 4, 8) + "| WGPQ : Growth factor for persistent bacteria in soil solution [1/day]")
            lSB.AppendLine(MakeString(lBsnRow.Item(33), 3, 4, 8) + "| WDLPQ : Die-off factor for less persistent bacteria in soil solution [1/day]")
            lSB.AppendLine(MakeString(lBsnRow.Item(34), 3, 4, 8) + "| WGLPQ : Growth factor for less persistent bacteria in soil solution. [1/day] ")
            lSB.AppendLine(MakeString(lBsnRow.Item(35), 3, 4, 8) + "| WDPS : Die-off factor for persistent bacteria adsorbed to soil particles. [1/day]")
            lSB.AppendLine(MakeString(lBsnRow.Item(36), 3, 4, 8) + "| WGPS : Growth factor for persistent bacteria adsorbed to soil particles. [1/day]")
            lSB.AppendLine(MakeString(lBsnRow.Item(37), 3, 4, 8) + "| WDLPS : Die-off factor for less persistent bacteria adsorbed to soil particles. [1/day] ")
            lSB.AppendLine(MakeString(lBsnRow.Item(38), 3, 4, 8) + "| WGLPS : Growth factor for less persistent bacteria adsorbed to soil particles. [1/day]")
            lSB.AppendLine(MakeString(lBsnRow.Item(39), 3, 4, 8) + "| BACTKDQ : Bacteria partition coefficient")
            lSB.AppendLine(MakeString(lBsnRow.Item(40), 3, 4, 8) + "| THBACT : Temperature adjustment factor for bacteria die-off/growth")
            lSB.AppendLine(MakeString(lBsnRow.Item(41), 3, 4, 8) + "| WOF_P: wash-off fraction for persistent bacteria on foliage")
            lSB.AppendLine(MakeString(lBsnRow.Item(42), 3, 4, 8) + "| WOF_LP: wash-off fraction for less persistent bacteria on foliage")
            lSB.AppendLine(MakeString(lBsnRow.Item(43), 3, 4, 8) + "| WDPF: persistent bacteria die-off factor on foliage")
            lSB.AppendLine(MakeString(lBsnRow.Item(44), 3, 4, 8) + "| WGPF: persistent bacteria growth factor on foliage ")
            lSB.AppendLine(MakeString(lBsnRow.Item(45), 3, 4, 8) + "| WDLPF: less persistent bacteria die-off factor on foliage")
            lSB.AppendLine(MakeString(lBsnRow.Item(46), 3, 4, 8) + "| WGLPF: less persistent bacteria growth factor on foliage")
            lSB.AppendLine(MakeString(lBsnRow.Item(74), 0, 4, 4) + "| ISED_DET: ")
            lSB.AppendLine("Modeling Options: Reaches")
            lSB.AppendLine(MakeString(lBsnRow.Item(47), 0, 4, 4) + "| IRTE: water routing method 0=variable travel-time 1=Muskingum")
            lSB.AppendLine(MakeString(lBsnRow.Item(48), 3, 4, 8) + "| MSK_CO1 : Calibration coefficient used to control impact of the storage time constant (Km) for normal flow ")
            lSB.AppendLine(MakeString(lBsnRow.Item(49), 3, 4, 8) + "| MSK_CO2 : Calibration coefficient used to control impact of the storage time constant (Km) for low flow ")
            lSB.AppendLine(MakeString(lBsnRow.Item(50), 3, 4, 8) + "| MSK_X : Weighting factor controlling relative importance of inflow rate and outflow rate in determining water storage in reach segment")
            lSB.AppendLine(MakeString(lBsnRow.Item(51), 0, 4, 4) + "| IDEG: channel degradation code ")
            lSB.AppendLine(MakeString(lBsnRow.Item(52), 0, 4, 4) + "| IWQ: in-stream water quality: 1=model in-stream water quality ")
            lSB.AppendLine("   basins.wwq       | WWQFILE: name of watershed water quality file")
            lSB.AppendLine(MakeString(lBsnRow.Item(53), 3, 4, 8) + "| TRNSRCH: reach transmission loss partitioning to deep aquifer")
            lSB.AppendLine(MakeString(lBsnRow.Item(54), 3, 4, 8) + "| EVRCH : Reach evaporation adjustment factor")
            lSB.AppendLine(MakeString(lBsnRow.Item(55), 0, 4, 4) + "| IRTPEST : Number of pesticide to be routed through the watershed channel network")
            lSB.AppendLine(MakeString(lBsnRow.Item(56), 0, 4, 4) + "| ICN")
            lSB.AppendLine(MakeString(lBsnRow.Item(57), 3, 4, 8) + "| CNCOEF")
            lSB.AppendLine(MakeString(lBsnRow.Item(58), 3, 4, 8) + "| CDN")
            lSB.AppendLine(MakeString(lBsnRow.Item(59), 3, 4, 8) + "| SDNCO")
            lSB.AppendLine(MakeString(lBsnRow.Item(60), 3, 4, 8) + "| BACT_SWF")
            lSB.AppendLine(MakeString(lBsnRow.Item(61), 3, 4, 8) + "| BACTMX")
            lSB.AppendLine(MakeString(lBsnRow.Item(62), 3, 4, 8) + "| BACTMINLP")
            lSB.AppendLine(MakeString(lBsnRow.Item(63), 3, 4, 8) + "| BACTMINP")
            lSB.AppendLine(MakeString(lBsnRow.Item(64), 3, 4, 8) + "| WDLPRCH")
            lSB.AppendLine(MakeString(lBsnRow.Item(65), 3, 4, 8) + "| WDPRCH")
            lSB.AppendLine(MakeString(lBsnRow.Item(66), 3, 4, 8) + "| WDLPRES")
            lSB.AppendLine(MakeString(lBsnRow.Item(67), 3, 4, 8) + "| WDPRES")
            lSB.AppendLine(MakeString(lBsnRow.Item(68), 3, 4, 8) + "| TB_ADJ")
            lSB.AppendLine(MakeString(lBsnRow.Item(69), 0, 4, 8) + "| DEPIMP_BSN")
            lSB.AppendLine(MakeString(lBsnRow.Item(70), 3, 4, 8) + "| DDRAIN_BSN")
            lSB.AppendLine(MakeString(lBsnRow.Item(71), 3, 4, 8) + "| TDRAIN_BSN")
            lSB.AppendLine(MakeString(lBsnRow.Item(72), 3, 4, 8) + "| GDRAIN_BSN")
            lSB.AppendLine(MakeString(lBsnRow.Item(73), 3, 4, 8) + "| CN_FROZ")

            IO.File.WriteAllText(pSwatInput.OutputFolder & "\basins.bsn", lSB.ToString)
        End Sub
    End Class
End Class
