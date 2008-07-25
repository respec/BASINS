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

        Public Function Units() As Dictionary(Of String, String)
            Dim unitsDictionary As New Dictionary(Of String, String)

            unitsDictionary.Add("SFTMP", "C")
            unitsDictionary.Add("SMTMP", "C")
            unitsDictionary.Add("SMFMX", "mm H2O/degC-day")
            unitsDictionary.Add("SMFMN", "mm H2O/degC-day")
            unitsDictionary.Add("SNOCOVMX", "mm")
            unitsDictionary.Add("EVLAI", "m2/m2")
            unitsDictionary.Add("SURLAG", "days")
            unitsDictionary.Add("RCN", "mg N/l")
            unitsDictionary.Add("WDPQ", "1/day")
            unitsDictionary.Add("WGPQ", "1/day")
            unitsDictionary.Add("WDLPQ", "1/day")
            unitsDictionary.Add("WGLPQ", "1/day")
            unitsDictionary.Add("WDPS", "1/day")
            unitsDictionary.Add("WGPS", "1/day")
            unitsDictionary.Add("WDLPS", "1/day")
            unitsDictionary.Add("WGLPS", "1/day")

            Return unitsDictionary
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

        Public Function Item() As clsBsnItem
            Return Item(Nothing)
        End Function
        Public Function Item(ByVal aTable As DataTable) As clsBsnItem
            If aTable Is Nothing Then aTable = Table()
            Return New clsBsnItem(aTable.Rows(0))
        End Function

        Public Sub Add(ByVal aItem As clsBsnItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatInput)
        End Sub

        Public Sub Save()
            Save(Nothing)
        End Sub
        Public Sub Save(ByVal aTable As DataTable)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing " & pTableName & " text ...")

            Dim lRow As DataRow = aTable.Rows(0)
            Dim lItem As New clsBsnItem(lRow)
            Dim lSB As New System.Text.StringBuilder
            With lItem
                lSB.AppendLine("Basin data" & Space(10) & " ." & pTableName & " file " & HeaderString())
                lSB.AppendLine("Modeling Options: Land Area")
                lSB.AppendLine("Water Balance:")
                lSB.AppendLine(MakeString(.SFTMP, 3, 4, 8) & "| SFTMP : Snowfall temperature [ºC]")
                lSB.AppendLine(MakeString(.SMTMP, 3, 4, 8) & "| SMTMP : Snow melt base temperature [ºC]")
                lSB.AppendLine(MakeString(.SMFMX, 3, 4, 8) & "| SMFMX : Melt factor for snow on June 21 [mm H2O/ºC-day]")
                lSB.AppendLine(MakeString(.SMFMN, 3, 4, 8) & "| SMFMN : Melt factor for snow on December 21 [mm H2O/ºC-day]")
                lSB.AppendLine(MakeString(.TIMP, 3, 4, 8) & "| TIMP : Snow pack temperature lag factor")
                lSB.AppendLine(MakeString(.SNOCOVMX, 3, 4, 8) & "| SNOCOVMX : Minimum snow water content that corresponds to 100% snow cover [mm]")
                lSB.AppendLine(MakeString(.SNO50COV, 3, 4, 8) & "| SNO50COV : Fraction of snow volume represented by SNOCOVMX that corresponds to 50% snow cover")
                lSB.AppendLine(MakeString(.IPET, 0, 4, 4) & "| IPET: PET method: 0=priest-t, 1=pen-m, 2=har, 3=read into model")
                lSB.AppendLine(.ETFILE.PadLeft(16) & "    | PETFILE: name of potential ET input file")
                lSB.AppendLine(MakeString(.ESCO, 3, 4, 8) & "| ESCO: soil evaporation compensation factor")
                lSB.AppendLine(MakeString(.EPCO, 3, 4, 8) & "| EPCO: plant water uptake compensation factor")
                lSB.AppendLine(MakeString(.EVLAI, 3, 4, 8) & "| EVLAI : Leaf area index at which no evaporation occurs from water surface [m2/m2]")
                lSB.AppendLine(MakeString(.FFCB, 3, 4, 8) & "| FFCB : Initial soil water storage expressed as a fraction of field capacity water content")
                lSB.AppendLine("Surface Runoff:")
                lSB.AppendLine(MakeString(.IEVENT, 0, 4, 4) & "| IEVENT: rainfall/runoff code: 0=daily rainfall/CN")
                lSB.AppendLine(MakeString(.ICRK, 0, 4, 4) & "| ICRK: crack flow code: 1=model crack flow in soil")
                lSB.AppendLine(MakeString(.SURLAG, 3, 4, 8) & "| SURLAG : Surface runoff lag time [days]")
                lSB.AppendLine(MakeString(.ADJ_PKR, 3, 4, 8) & "| ADJ_PKR : Peak rate adjustment factor for sediment routing in the subbasin (tributary channels)")
                lSB.AppendLine(MakeString(.PRF, 3, 4, 8) & "| PRF : Peak rate adjustment factor for sediment routing in the main channel")
                lSB.AppendLine(MakeString(.SPCON, 3, 4, 8) & "| SPCON : Linear parameter for calculating the maximum amount of sediment that can be reentrained during channel sediment routing")
                lSB.AppendLine(MakeString(.SPEXP, 3, 4, 8) & "| SPEXP : Exponent parameter for calculating sediment reentrained in channel sediment routing")
                lSB.AppendLine("Nutrient Cycling:")
                lSB.AppendLine(MakeString(.RCN, 3, 4, 8) & "| RCN : Concentration of nitrogen in rainfall [mg N/l]")
                lSB.AppendLine(MakeString(.CMN, 3, 4, 8) & "| CMN : Rate factor for humus mineralization of active organic nitrogen")
                lSB.AppendLine(MakeString(.N_UPDIS, 3, 4, 8) & "| N_UPDIS : Nitrogen uptake distribution parameter")
                lSB.AppendLine(MakeString(.P_UPDIS, 3, 4, 8) & "| P_UPDIS : Phosphorus uptake distribution parameter")
                lSB.AppendLine(MakeString(.NPERCO, 3, 4, 8) & "| NPERCO : Nitrogen percolation coefficient")
                lSB.AppendLine(MakeString(.PPERCO, 3, 4, 8) & "| PPERCO : Phosphorus percolation coefficient")
                lSB.AppendLine(MakeString(.PHOSKD, 3, 4, 8) & "| PHOSKD : Phosphorus soil partitioning coefficient")
                lSB.AppendLine(MakeString(.PSP, 3, 4, 8) & "| PSP : Phosphorus sorption coefficient")
                lSB.AppendLine(MakeString(.RSDCO, 3, 4, 8) & "| RSDCO : Residue decomposition coefficient")
                lSB.AppendLine("Pesticide Cycling:")
                lSB.AppendLine(MakeString(.PERCOP, 3, 4, 8) & "| PERCOP : Pesticide percolation coefficient")
                lSB.AppendLine("Algae/CBOD/Dissolved Oxygen:")
                lSB.AppendLine(MakeString(.ISUBWQ, 0, 4, 4) & "| ISUBWQ: subbasin water quality parameter")
                lSB.AppendLine("Bacteria:")
                lSB.AppendLine(MakeString(.WDPQ, 3, 4, 8) & "| WDPQ : Die-off factor for persistent bacteria in soil solution. [1/day]")
                lSB.AppendLine(MakeString(.WGPQ, 3, 4, 8) & "| WGPQ : Growth factor for persistent bacteria in soil solution [1/day]")
                lSB.AppendLine(MakeString(.WDLPQ, 3, 4, 8) & "| WDLPQ : Die-off factor for less persistent bacteria in soil solution [1/day]")
                lSB.AppendLine(MakeString(.WGLPQ, 3, 4, 8) & "| WGLPQ : Growth factor for less persistent bacteria in soil solution. [1/day] ")
                lSB.AppendLine(MakeString(.WDPS, 3, 4, 8) & "| WDPS : Die-off factor for persistent bacteria adsorbed to soil particles. [1/day]")
                lSB.AppendLine(MakeString(.WGPS, 3, 4, 8) & "| WGPS : Growth factor for persistent bacteria adsorbed to soil particles. [1/day]")
                lSB.AppendLine(MakeString(.WDLPS, 3, 4, 8) & "| WDLPS : Die-off factor for less persistent bacteria adsorbed to soil particles. [1/day] ")
                lSB.AppendLine(MakeString(.WGLPS, 3, 4, 8) & "| WGLPS : Growth factor for less persistent bacteria adsorbed to soil particles. [1/day]")
                lSB.AppendLine(MakeString(.BACTKDQ, 3, 4, 8) & "| BACTKDQ : Bacteria partition coefficient")
                lSB.AppendLine(MakeString(.THBACT, 3, 4, 8) & "| THBACT : Temperature adjustment factor for bacteria die-off/growth")
                lSB.AppendLine(MakeString(.WOF_P, 3, 4, 8) & "| WOF_P: wash-off fraction for persistent bacteria on foliage")
                lSB.AppendLine(MakeString(.WOF_LP, 3, 4, 8) & "| WOF_LP: wash-off fraction for less persistent bacteria on foliage")
                lSB.AppendLine(MakeString(.WDPF, 3, 4, 8) & "| WDPF: persistent bacteria die-off factor on foliage")
                lSB.AppendLine(MakeString(.WGPF, 3, 4, 8) & "| WGPF: persistent bacteria growth factor on foliage ")
                lSB.AppendLine(MakeString(.WDLPF, 3, 4, 8) & "| WDLPF: less persistent bacteria die-off factor on foliage")
                lSB.AppendLine(MakeString(.WGLPF, 3, 4, 8) & "| WGLPF: less persistent bacteria growth factor on foliage")
                lSB.AppendLine(MakeString(.ISED_DET, 0, 4, 4) & "| ISED_DET: ")
                lSB.AppendLine("Modeling Options: Reaches")
                lSB.AppendLine(MakeString(.IRTE, 0, 4, 4) & "| IRTE: water routing method 0=variable travel-time 1=Muskingum")
                lSB.AppendLine(MakeString(.MSK_COL1, 3, 4, 8) & "| MSK_CO1 : Calibration coefficient used to control impact of the storage time constant (Km) for normal flow ")
                lSB.AppendLine(MakeString(.MSK_COL2, 3, 4, 8) & "| MSK_CO2 : Calibration coefficient used to control impact of the storage time constant (Km) for low flow ")
                lSB.AppendLine(MakeString(.MSK_X, 3, 4, 8) & "| MSK_X : Weighting factor controlling relative importance of inflow rate and outflow rate in determining water storage in reach segment")
                lSB.AppendLine(MakeString(.IDEG, 0, 4, 4) & "| IDEG: channel degradation code ")
                lSB.AppendLine(MakeString(.IWQ, 0, 4, 4) & "| IWQ: in-stream water quality: 1=model in-stream water quality ")
                lSB.AppendLine("   basins.wwq       | WWQFILE: name of watershed water quality file")
                lSB.AppendLine(MakeString(.TRNSRCH, 3, 4, 8) & "| TRNSRCH: reach transmission loss partitioning to deep aquifer")
                lSB.AppendLine(MakeString(.EVRCH, 3, 4, 8) & "| EVRCH : Reach evaporation adjustment factor")
                lSB.AppendLine(MakeString(.IRTPEST, 0, 4, 4) & "| IRTPEST : Number of pesticide to be routed through the watershed channel network")
                lSB.AppendLine(MakeString(.ICN, 0, 4, 4) & "| ICN")
                lSB.AppendLine(MakeString(.CNCOEF, 3, 4, 8) & "| CNCOEF")
                lSB.AppendLine(MakeString(.CDN, 3, 4, 8) & "| CDN")
                lSB.AppendLine(MakeString(.SDNCO, 3, 4, 8) & "| SDNCO")
                lSB.AppendLine(MakeString(.BACT_SWF, 3, 4, 8) & "| BACT_SWF")
                lSB.AppendLine(MakeString(.BACTMX, 3, 4, 8) & "| BACTMX")
                lSB.AppendLine(MakeString(.BACTMINLP, 3, 4, 8) & "| BACTMINLP")
                lSB.AppendLine(MakeString(.BACTMINP, 3, 4, 8) & "| BACTMINP")
                lSB.AppendLine(MakeString(.WDLPRCH, 3, 4, 8) & "| WDLPRCH")
                lSB.AppendLine(MakeString(.WDPRCH, 3, 4, 8) & "| WDPRCH")
                lSB.AppendLine(MakeString(.WDLPRES, 3, 4, 8) & "| WDLPRES")
                lSB.AppendLine(MakeString(.WDPRES, 3, 4, 8) & "| WDPRES")
                lSB.AppendLine(MakeString(.TB_ADJ, 3, 4, 8) & "| TB_ADJ")
                lSB.AppendLine(MakeString(.DEPIMP_BSN, 0, 4, 8) & "| DEPIMP_BSN")
                lSB.AppendLine(MakeString(.DDRAIN_BSN, 3, 4, 8) & "| DDRAIN_BSN")
                lSB.AppendLine(MakeString(.TDRAIN_BSN, 3, 4, 8) & "| TDRAIN_BSN")
                lSB.AppendLine(MakeString(.GDRAIN_BSN, 3, 4, 8) & "| GDRAIN_BSN")
                lSB.AppendLine(MakeString(.CN_FROZ, 3, 4, 8) & "| CN_FROZ")
            End With
            IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\basins.bsn", lSB.ToString)
        End Sub

        
    End Class
End Class
