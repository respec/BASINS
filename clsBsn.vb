Partial Class SwatInput
    Private pBsn As clsBsn = New clsBsn(Me)
    ReadOnly Property Bsn() As clsBsn
        Get
            Return pBsn
        End Get
    End Property

    Public Class clsBsnItem
        Public SFTMP As Double
        Public SMTMP As Double
        Public SMFMX As Double
        Public SMFMN As Double
        Public TIMP As Double
        Public SNOCOVMX As Double
        Public SNO50COV As Double
        Public IPET As Long
        Public ESCO As Double
        Public EPCO As Double
        Public EVLAI As Double
        Public FFCB As Double
        Public IEVENT As Long
        Public ICRK As Long
        Public SURLAG As Double
        Public ADJ_PKR As Double
        Public PRF As Double
        Public SPCON As Double
        Public SPEXP As Double
        Public RCN As Double
        Public CMN As Double
        Public N_UPDIS As Double
        Public P_UPDIS As Double
        Public NPERCO As Double
        Public PPERCO As Double
        Public PHOSKD As Double
        Public PSP As Double
        Public RSDCO As Double
        Public PERCOP As Double
        Public ISUBWQ As Long
        Public WDPQ As Double
        Public WGPQ As Double
        Public WDLPQ As Double
        Public WGLPQ As Double
        Public WDPS As Double
        Public WGPS As Double
        Public WDLPS As Double
        Public WGLPS As Double
        Public BACTKDQ As Double
        Public THBACT As Double
        Public WOF_P As Double
        Public WOF_LP As Double
        Public WDPF As Double
        Public WGPF As Double
        Public WDLPF As Double
        Public WGLPF As Double
        Public IRTE As Long
        Public MSK_COL1 As Double
        Public MSK_COL2 As Double
        Public MSK_X As Double
        Public IDEG As Long
        Public IWQ As Long
        Public TRNSRCH As Double
        Public EVRCH As Double
        Public IRTPEST As Long
        Public ICN As Long
        Public CNCOEF As Double
        Public CDN As Double
        Public SDNCO As Double
        Public BACT_SWF As Double
        Public BACTMX As Double
        Public BACTMINLP As Double
        Public BACTMINP As Double
        Public WDLPRCH As Double
        Public WDPRCH As Double
        Public WDLPRES As Double
        Public WDPRES As Double
        Public TB_ADJ As Double
        Public DEPIMP_BSN As Double
        Public DDRAIN_BSN As Double
        Public TDRAIN_BSN As Double
        Public GDRAIN_BSN As Double
        Public CN_FROZ As Double
        Public ISED_DET As Double
        Public ETFILE As String

        Public Sub New()
        End Sub

        Sub New(ByVal aRow As DataRow)
            PopulateObject(aRow, Me)
        End Sub

        Public Function AddSQL() As String
            Return "INSERT INTO bsn ( " & atcUtility.FieldNames(Me.GetType, ", ") _
                       & " ) Values ('" & atcUtility.FieldValues(Me, "', '") & "'  )"
        End Function
    End Class

    ''' <summary>
    ''' Basin (BSN) Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsBsn
        Private pSwatInput As SwatInput
        Private pTableName As String = "bsn"

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn.DBLayer.createBsnTable
            Try
                DropTable(pTableName, pSwatInput.CnSwatInput)

                'Open the connection
                Dim lConnection As ADODB.Connection = pSwatInput.OpenADOConnection()

                'Open the Catalog
                Dim lCatalog As New ADOX.Catalog
                lCatalog.ActiveConnection = lConnection

                'Create the table
                Dim lTable As New ADOX.Table
                lTable.Name = pTableName

                Dim lKeyColumn As New ADOX.Column
                With lKeyColumn
                    .Name = "OBJECTID"
                    .Type = ADOX.DataTypeEnum.adInteger
                    .ParentCatalog = lCatalog
                    .Properties("AutoIncrement").Value = True
                End With

                With lTable.Columns
                    .Append(lKeyColumn)
                    .Append("SFTMP", ADOX.DataTypeEnum.adDouble)
                    .Append("SMTMP", ADOX.DataTypeEnum.adDouble)
                    .Append("SMFMX", ADOX.DataTypeEnum.adDouble)
                    .Append("SMFMN", ADOX.DataTypeEnum.adDouble)
                    .Append("TIMP", ADOX.DataTypeEnum.adDouble)
                    .Append("SNOCOVMX", ADOX.DataTypeEnum.adDouble)
                    .Append("SNO50COV", ADOX.DataTypeEnum.adDouble)
                    .Append("IPET", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("ESCO", ADOX.DataTypeEnum.adDouble)
                    .Append("EPCO", ADOX.DataTypeEnum.adDouble)
                    .Append("EVLAI", ADOX.DataTypeEnum.adDouble)
                    .Append("FFCB", ADOX.DataTypeEnum.adDouble)
                    .Append("IEVENT", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("ICRK", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("SURLAG", ADOX.DataTypeEnum.adDouble)
                    .Append("ADJ_PKR", ADOX.DataTypeEnum.adDouble)
                    .Append("PRF", ADOX.DataTypeEnum.adDouble)
                    .Append("SPCON", ADOX.DataTypeEnum.adDouble)
                    .Append("SPEXP", ADOX.DataTypeEnum.adDouble)
                    .Append("RCN", ADOX.DataTypeEnum.adDouble)
                    .Append("CMN", ADOX.DataTypeEnum.adDouble)
                    .Append("N_UPDIS", ADOX.DataTypeEnum.adDouble)
                    .Append("P_UPDIS", ADOX.DataTypeEnum.adDouble)
                    .Append("NPERCO", ADOX.DataTypeEnum.adDouble)
                    .Append("PPERCO", ADOX.DataTypeEnum.adDouble)
                    .Append("PHOSKD", ADOX.DataTypeEnum.adDouble)
                    .Append("PSP", ADOX.DataTypeEnum.adDouble)
                    .Append("RSDCO", ADOX.DataTypeEnum.adDouble)
                    .Append("PERCOP", ADOX.DataTypeEnum.adDouble)
                    .Append("ISUBWQ", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("WDPQ", ADOX.DataTypeEnum.adDouble)
                    .Append("WGPQ", ADOX.DataTypeEnum.adDouble)
                    .Append("WDLPQ", ADOX.DataTypeEnum.adDouble)
                    .Append("WGLPQ", ADOX.DataTypeEnum.adDouble)
                    .Append("WDPS", ADOX.DataTypeEnum.adDouble)
                    .Append("WGPS", ADOX.DataTypeEnum.adDouble)
                    .Append("WDLPS", ADOX.DataTypeEnum.adDouble)
                    .Append("WGLPS", ADOX.DataTypeEnum.adDouble)
                    .Append("BACTKDQ", ADOX.DataTypeEnum.adDouble)
                    .Append("THBACT", ADOX.DataTypeEnum.adDouble)
                    .Append("WOF_P", ADOX.DataTypeEnum.adDouble)
                    .Append("WOF_LP", ADOX.DataTypeEnum.adDouble)
                    .Append("WDPF", ADOX.DataTypeEnum.adDouble)
                    .Append("WGPF", ADOX.DataTypeEnum.adDouble)
                    .Append("WDLPF", ADOX.DataTypeEnum.adDouble)
                    .Append("WGLPF", ADOX.DataTypeEnum.adDouble)
                    .Append("IRTE", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("MSK_COL1", ADOX.DataTypeEnum.adDouble)
                    .Append("MSK_COL2", ADOX.DataTypeEnum.adDouble)
                    .Append("MSK_X", ADOX.DataTypeEnum.adDouble)
                    .Append("IDEG", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IWQ", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("TRNSRCH", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("EVRCH", ADOX.DataTypeEnum.adDouble)
                    .Append("IRTPEST", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("ICN", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("CNCOEF", ADOX.DataTypeEnum.adDouble)
                    .Append("CDN", ADOX.DataTypeEnum.adDouble)
                    .Append("SDNCO", ADOX.DataTypeEnum.adDouble)
                    .Append("BACT_SWF", ADOX.DataTypeEnum.adDouble)
                    .Append("BACTMX", ADOX.DataTypeEnum.adDouble)
                    .Append("BACTMINLP", ADOX.DataTypeEnum.adDouble)
                    .Append("BACTMINP", ADOX.DataTypeEnum.adDouble)
                    .Append("WDLPRCH", ADOX.DataTypeEnum.adDouble)
                    .Append("WDPRCH", ADOX.DataTypeEnum.adDouble)
                    .Append("WDLPRES", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("WDPRES", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("TB_ADJ", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("DEPIMP_BSN", ADOX.DataTypeEnum.adDouble)
                    .Append("DDRAIN_BSN", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("TDRAIN_BSN", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("GDRAIN_BSN", ADOX.DataTypeEnum.adDouble)
                    .Append("CN_FROZ", ADOX.DataTypeEnum.adDouble)
                    .Append("ISED_DET", ADOX.DataTypeEnum.adDouble)
                    .Append("ETFILE", ADOX.DataTypeEnum.adVarWChar, 100)
                End With

                lTable.Keys.Append("PrimaryKey", ADOX.KeyTypeEnum.adKeyPrimary, lKeyColumn.Name)
                lCatalog.Tables.Append(lTable)
                lTable = Nothing
                lCatalog = Nothing
                lConnection.Close()
                lConnection = Nothing
                Return True
            Catch lEx As ApplicationException
                Debug.Print(lEx.Message)
                Return False
            End Try
        End Function

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & ";")
        End Function
        Public Function BsnItem() As clsBsnItem           
            Dim dt As DataTable = Nothing
            Return BsnItem(dt)
        End Function

        Public Function BsnItem(ByVal aTable As DataTable) As clsBsnItem
            If aTable Is Nothing Then aTable = Table()
            Dim lRow As DataRow = aTable.Rows(0)
            pSwatInput.Status("Writing " & pTableName & " to BsnItem ...")

            Dim item As New clsBsnItem()
            item.SFTMP = lRow.Item(1)
            Dim myType As Type
            myType = lRow.Item(1).GetType


            item.SMTMP = lRow.Item(2)

            item.SMFMX = lRow.Item(3)
            item.SMFMN = lRow.Item(4)
            item.TIMP = lRow.Item(5)
            item.SNOCOVMX = lRow.Item(6)
            item.SNO50COV = lRow.Item(7)
            item.IPET = lRow.Item(8)
            item.ESCO = lRow.Item(9)
            item.EPCO = lRow.Item(10)
            item.EVLAI = lRow.Item(11)
            item.FFCB = lRow.Item(12)

            item.IEVENT = lRow.Item(13)
            item.ICRK = lRow.Item(14)
            item.SURLAG = lRow.Item(15)
            item.ADJ_PKR = lRow.Item(16)
            item.PRF = lRow.Item(17)
            item.SPCON = lRow.Item(18)
            item.SPEXP = lRow.Item(19)

            item.RCN = lRow.Item(20)
            item.CMN = lRow.Item(21)
            item.N_UPDIS = lRow.Item(22)
            item.P_UPDIS = lRow.Item(23)
            item.NPERCO = lRow.Item(24)
            item.PPERCO = lRow.Item(25)
            item.PHOSKD = lRow.Item(26)
            item.PSP = lRow.Item(27)
            item.RSDCO = lRow.Item(28)

            item.PERCOP = lRow.Item(29)

            item.ISUBWQ = lRow.Item(30)

            item.WDPQ = lRow.Item(31)
            item.WGPQ = lRow.Item(32)
            item.WDLPQ = lRow.Item(33)
            item.WGLPQ = lRow.Item(34)
            item.WDPS = lRow.Item(35)
            item.WGPS = lRow.Item(36)
            item.WDLPS = lRow.Item(37)
            item.WGLPS = lRow.Item(38)
            item.BACTKDQ = lRow.Item(39)
            item.THBACT = lRow.Item(40)
            item.WOF_P = lRow.Item(41)
            item.WOF_LP = lRow.Item(42)
            item.WDPF = lRow.Item(43)
            item.WGPF = lRow.Item(44)
            item.WDLPF = lRow.Item(45)
            item.WGLPF = lRow.Item(46)


            item.IRTE = lRow.Item(47)
            item.MSK_COL1 = lRow.Item(48)
            item.MSK_COL2 = lRow.Item(49)
            item.MSK_X = lRow.Item(50)
            item.IDEG = lRow.Item(51)
            item.IWQ = lRow.Item(52)

            item.TRNSRCH = lRow.Item(53)
            item.EVRCH = lRow.Item(54)
            item.IRTPEST = lRow.Item(55)
            item.ICN = lRow.Item(56)
            item.CNCOEF = lRow.Item(57)
            item.CDN = lRow.Item(58)
            item.SDNCO = lRow.Item(59)
            item.BACT_SWF = lRow.Item(60)
            item.BACTMX = lRow.Item(61)
            item.BACTMINLP = lRow.Item(62)
            item.BACTMINP = lRow.Item(63)
            item.WDLPRCH = lRow.Item(64)
            item.WDPRCH = lRow.Item(65)
            item.WDLPRES = lRow.Item(66)
            item.WDPRES = lRow.Item(67)
            item.TB_ADJ = lRow.Item(68)
            item.DEPIMP_BSN = lRow.Item(69)
            item.DDRAIN_BSN = lRow.Item(70)
            item.TDRAIN_BSN = lRow.Item(71)
            item.GDRAIN_BSN = lRow.Item(72)
            item.CN_FROZ = lRow.Item(73)
            item.ISED_DET = lRow.Item(74)
            item.ETFILE = lRow.Item(75)

            Return item

        End Function

        Public Sub Add(ByVal aItem As clsBsnItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatInput)
        End Sub

        Public Sub Save()
            Dim dt As DataTable = Nothing
            Save(dt)
        End Sub

        Public Sub Save(ByVal aTable As DataTable)
            If aTable Is Nothing Then aTable = Table()
            Dim lRow As DataRow = aTable.Rows(0)
            pSwatInput.Status("Writing " & pTableName & " text ...")

            Dim lSB As New System.Text.StringBuilder
            lSB.AppendLine("Basin data" & Space(10) & " ." & pTableName & " file " & HeaderString())
            lSB.AppendLine("Modeling Options: Land Area")
            lSB.AppendLine("Water Balance:")
            lSB.AppendLine(MakeString(lRow.Item(1), 3, 4, 8) & "| SFTMP : Snowfall temperature [ºC]")
            lSB.AppendLine(MakeString(lRow.Item(2), 3, 4, 8) & "| SMTMP : Snow melt base temperature [ºC]")
            lSB.AppendLine(MakeString(lRow.Item(3), 3, 4, 8) & "| SMFMX : Melt factor for snow on June 21 [mm H2O/ºC-day]")
            lSB.AppendLine(MakeString(lRow.Item(4), 3, 4, 8) & "| SMFMN : Melt factor for snow on December 21 [mm H2O/ºC-day]")
            lSB.AppendLine(MakeString(lRow.Item(5), 3, 4, 8) & "| TIMP : Snow pack temperature lag factor")
            lSB.AppendLine(MakeString(lRow.Item(6), 3, 4, 8) & "| SNOCOVMX : Minimum snow water content that corresponds to 100% snow cover [mm]")
            lSB.AppendLine(MakeString(lRow.Item(7), 3, 4, 8) & "| SNO50COV : Fraction of snow volume represented by SNOCOVMX that corresponds to 50% snow cover")
            lSB.AppendLine(MakeString(lRow.Item(8), 0, 4, 4) & "| IPET: PET method: 0=priest-t, 1=pen-m, 2=har, 3=read into model")
            lSB.AppendLine(lRow.Item(75).ToString.PadLeft(16) & "    | PETFILE: name of potential ET input file")
            lSB.AppendLine(MakeString(lRow.Item(9), 3, 4, 8) & "| ESCO: soil evaporation compensation factor")
            lSB.AppendLine(MakeString(lRow.Item(10), 3, 4, 8) & "| EPCO: plant water uptake compensation factor")
            lSB.AppendLine(MakeString(lRow.Item(11), 3, 4, 8) & "| EVLAI : Leaf area index at which no evaporation occurs from water surface [m2/m2]")
            lSB.AppendLine(MakeString(lRow.Item(12), 3, 4, 8) & "| FFCB : Initial soil water storage expressed as a fraction of field capacity water content")
            lSB.AppendLine("Surface Runoff:")
            lSB.AppendLine(MakeString(lRow.Item(13), 0, 4, 4) & "| IEVENT: rainfall/runoff code: 0=daily rainfall/CN")
            lSB.AppendLine(MakeString(lRow.Item(14), 0, 4, 4) & "| ICRK: crack flow code: 1=model crack flow in soil")
            lSB.AppendLine(MakeString(lRow.Item(15), 3, 4, 8) & "| SURLAG : Surface runoff lag time [days]")
            lSB.AppendLine(MakeString(lRow.Item(16), 3, 4, 8) & "| ADJ_PKR : Peak rate adjustment factor for sediment routing in the subbasin (tributary channels)")
            lSB.AppendLine(MakeString(lRow.Item(17), 3, 4, 8) & "| PRF : Peak rate adjustment factor for sediment routing in the main channel")
            lSB.AppendLine(MakeString(lRow.Item(18), 3, 4, 8) & "| SPCON : Linear parameter for calculating the maximum amount of sediment that can be reentrained during channel sediment routing")
            lSB.AppendLine(MakeString(lRow.Item(19), 3, 4, 8) & "| SPEXP : Exponent parameter for calculating sediment reentrained in channel sediment routing")
            lSB.AppendLine("Nutrient Cycling:")
            lSB.AppendLine(MakeString(lRow.Item(20), 3, 4, 8) & "| RCN : Concentration of nitrogen in rainfall [mg N/l]")
            lSB.AppendLine(MakeString(lRow.Item(21), 3, 4, 8) & "| CMN : Rate factor for humus mineralization of active organic nitrogen")
            lSB.AppendLine(MakeString(lRow.Item(22), 3, 4, 8) & "| N_UPDIS : Nitrogen uptake distribution parameter")
            lSB.AppendLine(MakeString(lRow.Item(23), 3, 4, 8) & "| P_UPDIS : Phosphorus uptake distribution parameter")
            lSB.AppendLine(MakeString(lRow.Item(24), 3, 4, 8) & "| NPERCO : Nitrogen percolation coefficient")
            lSB.AppendLine(MakeString(lRow.Item(25), 3, 4, 8) & "| PPERCO : Phosphorus percolation coefficient")
            lSB.AppendLine(MakeString(lRow.Item(26), 3, 4, 8) & "| PHOSKD : Phosphorus soil partitioning coefficient")
            lSB.AppendLine(MakeString(lRow.Item(27), 3, 4, 8) & "| PSP : Phosphorus sorption coefficient")
            lSB.AppendLine(MakeString(lRow.Item(28), 3, 4, 8) & "| RSDCO : Residue decomposition coefficient")
            lSB.AppendLine("Pesticide Cycling:")
            lSB.AppendLine(MakeString(lRow.Item(29), 3, 4, 8) & "| PERCOP : Pesticide percolation coefficient")
            lSB.AppendLine("Algae/CBOD/Dissolved Oxygen:")
            lSB.AppendLine(MakeString(lRow.Item(30), 0, 4, 4) & "| ISUBWQ: subbasin water quality parameter")
            lSB.AppendLine("Bacteria:")
            lSB.AppendLine(MakeString(lRow.Item(31), 3, 4, 8) & "| WDPQ : Die-off factor for persistent bacteria in soil solution. [1/day]")
            lSB.AppendLine(MakeString(lRow.Item(32), 3, 4, 8) & "| WGPQ : Growth factor for persistent bacteria in soil solution [1/day]")
            lSB.AppendLine(MakeString(lRow.Item(33), 3, 4, 8) & "| WDLPQ : Die-off factor for less persistent bacteria in soil solution [1/day]")
            lSB.AppendLine(MakeString(lRow.Item(34), 3, 4, 8) & "| WGLPQ : Growth factor for less persistent bacteria in soil solution. [1/day] ")
            lSB.AppendLine(MakeString(lRow.Item(35), 3, 4, 8) & "| WDPS : Die-off factor for persistent bacteria adsorbed to soil particles. [1/day]")
            lSB.AppendLine(MakeString(lRow.Item(36), 3, 4, 8) & "| WGPS : Growth factor for persistent bacteria adsorbed to soil particles. [1/day]")
            lSB.AppendLine(MakeString(lRow.Item(37), 3, 4, 8) & "| WDLPS : Die-off factor for less persistent bacteria adsorbed to soil particles. [1/day] ")
            lSB.AppendLine(MakeString(lRow.Item(38), 3, 4, 8) & "| WGLPS : Growth factor for less persistent bacteria adsorbed to soil particles. [1/day]")
            lSB.AppendLine(MakeString(lRow.Item(39), 3, 4, 8) & "| BACTKDQ : Bacteria partition coefficient")
            lSB.AppendLine(MakeString(lRow.Item(40), 3, 4, 8) & "| THBACT : Temperature adjustment factor for bacteria die-off/growth")
            lSB.AppendLine(MakeString(lRow.Item(41), 3, 4, 8) & "| WOF_P: wash-off fraction for persistent bacteria on foliage")
            lSB.AppendLine(MakeString(lRow.Item(42), 3, 4, 8) & "| WOF_LP: wash-off fraction for less persistent bacteria on foliage")
            lSB.AppendLine(MakeString(lRow.Item(43), 3, 4, 8) & "| WDPF: persistent bacteria die-off factor on foliage")
            lSB.AppendLine(MakeString(lRow.Item(44), 3, 4, 8) & "| WGPF: persistent bacteria growth factor on foliage ")
            lSB.AppendLine(MakeString(lRow.Item(45), 3, 4, 8) & "| WDLPF: less persistent bacteria die-off factor on foliage")
            lSB.AppendLine(MakeString(lRow.Item(46), 3, 4, 8) & "| WGLPF: less persistent bacteria growth factor on foliage")
            lSB.AppendLine(MakeString(lRow.Item(74), 0, 4, 4) & "| ISED_DET: ")
            lSB.AppendLine("Modeling Options: Reaches")
            lSB.AppendLine(MakeString(lRow.Item(47), 0, 4, 4) & "| IRTE: water routing method 0=variable travel-time 1=Muskingum")
            lSB.AppendLine(MakeString(lRow.Item(48), 3, 4, 8) & "| MSK_CO1 : Calibration coefficient used to control impact of the storage time constant (Km) for normal flow ")
            lSB.AppendLine(MakeString(lRow.Item(49), 3, 4, 8) & "| MSK_CO2 : Calibration coefficient used to control impact of the storage time constant (Km) for low flow ")
            lSB.AppendLine(MakeString(lRow.Item(50), 3, 4, 8) & "| MSK_X : Weighting factor controlling relative importance of inflow rate and outflow rate in determining water storage in reach segment")
            lSB.AppendLine(MakeString(lRow.Item(51), 0, 4, 4) & "| IDEG: channel degradation code ")
            lSB.AppendLine(MakeString(lRow.Item(52), 0, 4, 4) & "| IWQ: in-stream water quality: 1=model in-stream water quality ")
            lSB.AppendLine("   basins.wwq       | WWQFILE: name of watershed water quality file")
            lSB.AppendLine(MakeString(lRow.Item(53), 3, 4, 8) & "| TRNSRCH: reach transmission loss partitioning to deep aquifer")
            lSB.AppendLine(MakeString(lRow.Item(54), 3, 4, 8) & "| EVRCH : Reach evaporation adjustment factor")
            lSB.AppendLine(MakeString(lRow.Item(55), 0, 4, 4) & "| IRTPEST : Number of pesticide to be routed through the watershed channel network")
            lSB.AppendLine(MakeString(lRow.Item(56), 0, 4, 4) & "| ICN")
            lSB.AppendLine(MakeString(lRow.Item(57), 3, 4, 8) & "| CNCOEF")
            lSB.AppendLine(MakeString(lRow.Item(58), 3, 4, 8) & "| CDN")
            lSB.AppendLine(MakeString(lRow.Item(59), 3, 4, 8) & "| SDNCO")
            lSB.AppendLine(MakeString(lRow.Item(60), 3, 4, 8) & "| BACT_SWF")
            lSB.AppendLine(MakeString(lRow.Item(61), 3, 4, 8) & "| BACTMX")
            lSB.AppendLine(MakeString(lRow.Item(62), 3, 4, 8) & "| BACTMINLP")
            lSB.AppendLine(MakeString(lRow.Item(63), 3, 4, 8) & "| BACTMINP")
            lSB.AppendLine(MakeString(lRow.Item(64), 3, 4, 8) & "| WDLPRCH")
            lSB.AppendLine(MakeString(lRow.Item(65), 3, 4, 8) & "| WDPRCH")
            lSB.AppendLine(MakeString(lRow.Item(66), 3, 4, 8) & "| WDLPRES")
            lSB.AppendLine(MakeString(lRow.Item(67), 3, 4, 8) & "| WDPRES")
            lSB.AppendLine(MakeString(lRow.Item(68), 3, 4, 8) & "| TB_ADJ")
            lSB.AppendLine(MakeString(lRow.Item(69), 0, 4, 8) & "| DEPIMP_BSN")
            lSB.AppendLine(MakeString(lRow.Item(70), 3, 4, 8) & "| DDRAIN_BSN")
            lSB.AppendLine(MakeString(lRow.Item(71), 3, 4, 8) & "| TDRAIN_BSN")
            lSB.AppendLine(MakeString(lRow.Item(72), 3, 4, 8) & "| GDRAIN_BSN")
            lSB.AppendLine(MakeString(lRow.Item(73), 3, 4, 8) & "| CN_FROZ")

            IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\basins.bsn", lSB.ToString)
        End Sub
    End Class
End Class
