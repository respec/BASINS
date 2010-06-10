Imports SwatObject

''' <summary>
''' Default values for tables
''' </summary>
''' <remarks>TODO: make more of these come from FRAMES</remarks>
Module modDefaults
    Friend Function CioDefault() As SwatInput.clsCIOItem
        Dim lCioItem As New SwatInput.clsCIOItem
        With lCioItem
            .NBYR = g_SimulationEndYear - g_SimulationStartYear + 1
            .IYR = g_SimulationStartYear
            .IDAF = 1
            .IDAL = IIf(DateTime.IsLeapYear(g_SimulationEndYear), 366, 365)
            .IGEN = 0
            .PCPSIM = 1 'measured precip
            .IDT = 0
            .IDIST = 0
            .REXP = 1.3
            .NRGAGE = 1
            .NRTOT = 1
            .NRGFIL = 1
            .TMPSIM = 1 'measured air temp
            .NTGAGE = 1
            .NTTOT = 1
            .NTGFIL = 1
            .SLRSIM = 1 'measured solar rad
            .NSTOT = 1
            .RHSIM = 2
            .NHTOT = 0
            .WNDSIM = 1 'measured wind
            .NWTOT = 1
            .FCSTYR = 0
            .FCSTDAY = 0
            .FCSTCYCLES = 0
            .DATES = "1/1/" & g_SimulationStartYear
            .DATEF = "12/31/" & g_SimulationEndYear
            .FDATES = ""
            .ISPROJ = 0
            .ICLB = 0
            .IPRINT = 1
            .NYSKIP = 0
            .ILOG = 1
            .IPRP = 1
            .IPRS = 1
        End With
        Return lCioItem
    End Function

    Friend Function WwqDefault() As SwatInput.clsWwqItem
        Dim lWwqItem As New SwatInput.clsWwqItem
        With lWwqItem
            .Lao = 2
            .Igropt = 2
            .Ai0 = 50
            .Ai1 = 0.08
            .Ai2 = 0.015
            .Ai3 = 1.6
            .Ai4 = 2
            .Ai5 = 3.5
            .Ai6 = 1.07
            .Mumax = 2
            .Rhoq = 0.3
            .Tfact = 0.3
            .K_l = 0.75
            .K_n = 0.02
            .K_p = 0.025
            .Lambda0 = 1
            .Lambda1 = 0.03
            .Lambda2 = 0.054
            .P_n = 0.5
        End With
        Return lWwqItem
    End Function

    Friend Function BsnDefault() As SwatInput.clsBsnItem
        Dim lBsnItem As New SwatInput.clsBsnItem
        With lBsnItem
            .SFTMP = 1
            .SMTMP = 0.5
            .SMFMX = 4.5
            .SMFMN = 4.5
            .TIMP = 1
            .SNOCOVMX = 1
            .SNO50COV = 0.5
            .IPET = 1
            .ESCO = 0.95
            .EPCO = 1
            .EVLAI = 3
            .FFCB = 0
            .IEVENT = 0
            .ICRK = 0
            .SURLAG = 4
            .ADJ_PKR = 0
            .PRF = 1
            .SPCON = 0.0001
            .SPEXP = 1
            .RCN = 1
            .CMN = 0.0003
            .N_UPDIS = 20
            .P_UPDIS = 20
            .NPERCO = 0.2
            .PPERCO = 10
            .PHOSKD = 175
            .PSP = 0.4
            .RSDCO = 0.05
            .PERCOP = 0.5
            .ISUBWQ = 1
            .WDPQ = 0
            .WGPQ = 0
            .WDLPQ = 0
            .WGLPQ = 0
            .WDPS = 0
            .WGPS = 0
            .WDLPS = 0
            .WGLPS = 0
            .BACTKDQ = 175
            .THBACT = 1.07
            .WOF_P = 0
            .WOF_LP = 0
            .WDPF = 0
            .WGPF = 0
            .WDLPF = 0
            .WGLPF = 0
            .IRTE = 0
            .MSK_COL1 = 0
            .MSK_COL2 = 3.5
            .MSK_X = 0.2
            .IDEG = 0
            .IWQ = 1
            .TRNSRCH = 0
            .EVRCH = 1
            .IRTPEST = 0
            .ICN = 0
            .CNCOEF = 1
            .CDN = 0
            .SDNCO = 0
            .BACT_SWF = 0
            .BACTMX = 10
            .BACTMINLP = 0
            .BACTMINP = 0
            .WDPRCH = 0
            .WDPRES = 0
            .TB_ADJ = 0
            .DEPIMP_BSN = 0
            .DDRAIN_BSN = 0
            .TDRAIN_BSN = 0
            .GDRAIN_BSN = 0
            .CN_FROZ = 0
            .ISED_DET = 0
            .ETFILE = ""
        End With
        Return lBsnItem
    End Function

    Friend Function PndDefault(ByVal aSubBasin As Double) As SwatInput.clsPndItem
        Dim lPndItem As New SwatInput.clsPndItem(aSubBasin)
        With lPndItem
            'all others are 0.0 or 0
            .PSETLP1 = 10
            .PSETLP2 = 10
            .NSETLP1 = 5.5
            .NSETLP2 = 5.5
            .CHLAP = 1
            .SECCIP = 1
            .IPND1 = 1
            .IPND2 = 1
        End With
        Return lPndItem
    End Function

    Friend Function SwqDefault(ByVal aSubBasin As Double) As SwatInput.clsSwqItem
        Dim lSwqItem As New SwatInput.clsSwqItem(aSubBasin)
        With lSwqItem
            .RS1 = 1.0
            .RS2 = 0.05
            .RS3 = 0.5
            .RS4 = 0.05
            .RS5 = 0.05
            .RS6 = 2.5
            .RS7 = 2.5
            .RK1 = 1.71
            .RK2 = 50.0
            .RK3 = 0.36
            .RK4 = 2.0
            .RK5 = 2.0
            .RK6 = 1.71
            .BC1 = 0.55
            .BC2 = 1.1
            .BC3 = 0.21
            .BC4 = 0.35
            .CHPST_REA = 0.007
            .CHPST_VOL = 0.01
            .CHPST_KOC = 0.0
            .CHPST_STL = 1.0
            .CHPST_RSP = 0.002
            .CHPST_MIX = 0.001
            .SEDPST_CONC = 0.0
            .SEDPST_REA = 0.05
            .SEDPST_BRY = 0.002
            .SEDPST_ACT = 0.03
        End With
        Return lSwqItem
    End Function

    Friend Function GwDefault(ByVal aSUBBASIN As Double, _
                              ByVal aHRU As Double, _
                              ByVal aLANDUSE As String, _
                              ByVal aSOIL As String, _
                              ByVal aSLOPE_CD As String) As SwatInput.clsGwItem
        Dim lGwItem As New SwatInput.clsGwItem(aSUBBASIN, aHRU, aLANDUSE, aSOIL, aSLOPE_CD)
        With lGwItem
            'all others are 0.0 or 0
            .SHALLST = 0.5
            .DEEPST = 1000.0
            .GW_DELAY = 31.0
            .ALPHA_BF = 0.048
            .GWQMN = 0.0
            .GW_REVAP = 0.02
            .REVAPMN = 1.0
            .RCHRG_DP = 0.05
            .GWHT = 1
            .GW_SPYLD = 0.003
            .SHALLST_N = 0.0
            .GWSOLP = 0.0
            .HLIFE_NGW = 0.0
        End With
        Return lGwItem
    End Function
End Module
