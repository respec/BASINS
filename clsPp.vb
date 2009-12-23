Partial Class SwatInput
    Private pPp As clsPp = New clsPp(Me)
    ReadOnly Property Pp() As clsPp
        Get
            Return pPp
        End Get
    End Property

    Public Class clsPpItem
        Public FLOCNST As Double
        Public SEDCNST As Double
        Public ORGNCNST As Double
        Public ORGPCNST As Double
        Public NO3CNST As Double
        Public NH3CNST As Double
        Public NO2CNST As Double
        Public MINPCNST As Double
        Public CBODCNST As Double
        Public DISOXCNST As Double
        Public CHLACNST As Double
        Public SOLPSTCNST As Double
        Public SRBPSTCNST As Double
        Public BACTPCNST As Double
        Public BACTLPCNST As Double
        Public CMTL1CNST As Double
        Public CMTL2CNST As Double
        Public CMTL3CNST As Double
        Public PCSIDS As String
        Public ANNUALREC As String
        Public MONTHLYREC As String
        Public DAILYREC As String
        Public TYPE As Integer

        Sub New(ByVal aRow As DataRow)
            PopulateObject(aRow, Me)
        End Sub

        Public Function AddSQL() As String

            Return "INSERT INTO pnd ( FLOCNST, FLOCNST, ORGNCNST, ORGPCNST, NO3CNST, NH3CNST, NO2CNST, MINPCNST, CBODCNST, DISOXCNST, CHLACNST, SOLPSTCNST, SRBPSTCNST, BACTPCNST, BACTLPCNST, CMTL1CNST, CMTL2CNST, CMTL3CNST, PCSIDS, ANNUALREC, MONTHLYREC, DAILYREC, TYPE ) " _
                 & "Values ('" & FLOCNST & "', '" & FLOCNST & "', '" & ORGNCNST & "', '" & ORGPCNST & "', '" & NO3CNST & "', '" & NH3CNST & "', '" & NO2CNST & "', '" & MINPCNST & "', '" & CBODCNST & "', '" & SOLPSTCNST & "', '" & SRBPSTCNST & "', '" & BACTPCNST & "', '" & BACTLPCNST & "', '" & CMTL1CNST & "', '" & CMTL3CNST & "', '" & CMTL3CNST & "', '" & PCSIDS & "', '" & ANNUALREC & "', '" & MONTHLYREC & "', '" & DAILYREC & "', '" & TYPE & "'   )"
        End Function
    End Class

    ''' <summary>
    ''' point source (pp) input section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsPp
        Private pSwatInput As SwatInput
        Private pTableName As String = "pp"

        Friend Shared Columns As Generic.List(Of clsDataColumn)

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn.DBLayer.createPp
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
                    .Name = "OID"
                    .Type = ADOX.DataTypeEnum.adInteger
                    .ParentCatalog = lCatalog
                    .Properties("AutoIncrement").Value = True
                End With

                With lTable.Columns
                    .Append(lKeyColumn)
                    .Append("FLOCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("SEDCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("ORGNCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("ORGPCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("NO3CNST", ADOX.DataTypeEnum.adDouble)
                    .Append("NH3CNST", ADOX.DataTypeEnum.adDouble)
                    .Append("NO2CNST", ADOX.DataTypeEnum.adDouble)
                    .Append("MINPCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("CBODCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("DISOXCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("CHLACNST", ADOX.DataTypeEnum.adDouble)
                    .Append("SOLPSTCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("SRBPSTCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("BACTPCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("BACTLPCNST", ADOX.DataTypeEnum.adDouble)
                    .Append("CMTL1CNST", ADOX.DataTypeEnum.adDouble)
                    .Append("CMTL2CNST", ADOX.DataTypeEnum.adDouble)
                    .Append("CMTL3CNST", ADOX.DataTypeEnum.adDouble)
                    .Append("PCSIDS", ADOX.DataTypeEnum.adVarWChar, 254)
                    .Append("ANNUALREC", ADOX.DataTypeEnum.adVarWChar, 254)
                    .Append("MONTHLYREC", ADOX.DataTypeEnum.adVarWChar, 254)
                    .Append("DAILYREC", ADOX.DataTypeEnum.adVarWChar, 254)
                    .Append("TYPE", ADOX.DataTypeEnum.adInteger, 4)
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

        Public Sub Add(ByVal aItem As clsPndItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatInput)
        End Sub

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()

            pSwatInput.Status("Writing " & pTableName & " text ...")

            For Each lRow As DataRow In aTable.Rows
                Dim lSubBasin As String = lRow.Item(1)
                Dim lHruNum As String = lRow.Item(2)
                Dim lTextFilename As String = StringFnameHRUs(lSubBasin, lHruNum) & "." & pTableName

                Dim lSB As New Text.StringBuilder

                lSB.AppendLine(" .Pnd file Subbasin: " & lSubBasin & " " & HeaderString())
                lSB.AppendLine("Pond inputs:")
                lSB.AppendLine(Format(lRow.Item(2), "0.000").PadLeft(16) & "    | PND_FR : Fraction of subbasin area that drains into ponds. The value for PND_FR should be between 0.0 and 1.0. If PND_FR = 1.0, the pond is at the outlet of the subbasin on the main channel")
                lSB.AppendLine(Format(lRow.Item(3), "0.000").PadLeft(16) & "    | PND_PSA: Surface area of ponds when filled to principal spillway [ha]")
                lSB.AppendLine(Format(lRow.Item(4), "0.000").PadLeft(16) & "    | PND_PVOL: Volume of water stored in ponds when filled to the principal spillway [104 m3]")
                lSB.AppendLine(Format(lRow.Item(5), "0.000").PadLeft(16) & "    | PND_ESA: Surface area of ponds when filled to emergency spillway [ha]")
                lSB.AppendLine(Format(lRow.Item(6), "0.000").PadLeft(16) & "    | PND_EVOL: Volume of water stored in ponds when filled to the emergency spillway [104 m3]")
                lSB.AppendLine(Format(lRow.Item(7), "0.000").PadLeft(16) & "    | PND_VOL: Initial volume of water in ponds [104 m3]")
                lSB.AppendLine(Format(lRow.Item(8), "0.000").PadLeft(16) & "    | PND_SED: Initial sediment concentration in pond water [mg/l]")
                lSB.AppendLine(Format(lRow.Item(9), "0.000").PadLeft(16) & "    | PND_NSED: Normal sediment concentration in pond water [mg/l]")
                lSB.AppendLine(Format(lRow.Item(10), "0.000").PadLeft(16) & "    | PND_K: Hydraulic conductivity through bottom of ponds [mm/hr].")
                lSB.AppendLine(Format(lRow.Item(11), "0").PadLeft(16) & "    | IFLOD1: Beginning month of non-flood season")
                lSB.AppendLine(Format(lRow.Item(12), "0").PadLeft(16) & "    | IFLOD2: Ending month of non-flood season")
                lSB.AppendLine(Format(lRow.Item(13), "0.000").PadLeft(16) & "    | NDTARG: Number of days needed to reach target storage from current pond storage")
                lSB.AppendLine(Format(lRow.Item(14), "0.000").PadLeft(16) & "    | PSETLP1: Phosphorus settling rate in pond for months IPND1 through IPND2 [m/year]")
                lSB.AppendLine(Format(lRow.Item(15), "0.000").PadLeft(16) & "    | PSETLP2: Phosphorus settling rate in pond for months other than IPND1-IPND2 [m/year]")
                lSB.AppendLine(Format(lRow.Item(16), "0.000").PadLeft(16) & "    | NSETLP1: Initial dissolved oxygen concentration in the reach [mg O2/l]")
                lSB.AppendLine(Format(lRow.Item(17), "0.000").PadLeft(16) & "    | NSETLP2: Initial dissolved oxygen concentration in the reach [mg O2/l]")
                lSB.AppendLine(Format(lRow.Item(18), "0.000").PadLeft(16) & "    | CHLAP: Chlorophyll a production coefficient for ponds [ ] ")
                lSB.AppendLine(Format(lRow.Item(19), "0.000").PadLeft(16) & "    | SECCIP: Water clarity coefficient for ponds [m]")
                lSB.AppendLine(Format(lRow.Item(20), "0.000").PadLeft(16) & "    | PND_NO3: Initial concentration of NO3-N in pond [mg N/l]")
                lSB.AppendLine(Format(lRow.Item(21), "0.000").PadLeft(16) & "    | PND_SOLP: Initial concentration of soluble P in pond [mg P/L]")
                lSB.AppendLine(Format(lRow.Item(22), "0.000").PadLeft(16) & "    | PND_ORGN: Initial concentration of organic N in pond [mg N/l]")
                lSB.AppendLine(Format(lRow.Item(23), "0.000").PadLeft(16) & "    | PND_ORGP: Initial concentration of organic P in pond [mg P/l]")
                lSB.AppendLine("Inputs used in both ponds and wetlands:")
                lSB.AppendLine(Format(lRow.Item(24), "0").PadLeft(16) & "    | IPND1: Beginning month of mid-year nutrient settling ""season""")
                lSB.AppendLine(Format(lRow.Item(25), "0").PadLeft(16) & "    | IPND2: Ending month of mid-year nutrient settling ""season""")
                lSB.AppendLine("Wetland inputs:")
                lSB.AppendLine(Format(lRow.Item(26), "0.000").PadLeft(16) & "    | WET_FR : Fraction of subbasin area that drains into wetlands")
                lSB.AppendLine(Format(lRow.Item(27), "0.000").PadLeft(16) & "    | WET_NSA: Surface area of wetlands at normal water level [ha]")
                lSB.AppendLine(Format(lRow.Item(28), "0.000").PadLeft(16) & "    | WET_NVOL: Volume of water stored in wetlands when filled to normal water level [104 m3] ")
                lSB.AppendLine(Format(lRow.Item(29), "0.000").PadLeft(16) & "    | WET_MXSA: Surface area of wetlands at maximum water level [ha]")
                lSB.AppendLine(Format(lRow.Item(30), "0.000").PadLeft(16) & "    | WET_MXVOL: Volume of water stored in wetlands when filled to maximum water level [104 m3]")
                lSB.AppendLine(Format(lRow.Item(31), "0.000").PadLeft(16) & "    | WET_VOL: Initial volume of water in wetlands [104 m3]")
                lSB.AppendLine(Format(lRow.Item(32), "0.000").PadLeft(16) & "    | WET_SED: Initial sediment concentration in wetland water [mg/l]")
                lSB.AppendLine(Format(lRow.Item(33), "0.000").PadLeft(16) & "    | WET_NSED: Normal sediment concentration in wetland water [mg/l]")
                lSB.AppendLine(Format(lRow.Item(34), "0.000").PadLeft(16) & "    | WET_K: Hydraulic conductivity of bottom of wetlands [mm/hr]")
                lSB.AppendLine(Format(lRow.Item(35), "0.000").PadLeft(16) & "    | PSETLW1: Phosphorus settling rate in wetland for months IPND1 through IPND2 [m/year]")
                lSB.AppendLine(Format(lRow.Item(36), "0.000").PadLeft(16) & "    | PSETLW2: Phosphorus settling rate in wetlands for months other than IPND1-IPND2 [m/year]")
                lSB.AppendLine(Format(lRow.Item(37), "0.000").PadLeft(16) & "    | NSETLW1: Nitrogen settling rate in wetlands for months IPND1 through IPND2 [m/year]")
                lSB.AppendLine(Format(lRow.Item(38), "0.000").PadLeft(16) & "    | NSETLW2: Nitrogen settling rate in wetlands for months other than IPND1-IPND2 [m/year]")
                lSB.AppendLine(Format(lRow.Item(39), "0.000").PadLeft(16) & "    | CHLAW: Chlorophyll a production coefficient for wetlands [ ]")
                lSB.AppendLine(Format(lRow.Item(40), "0.000").PadLeft(16) & "    | SECCIW: Water clarity coefficient for wetlands [m]")
                lSB.AppendLine(Format(lRow.Item(41), "0.000").PadLeft(16) & "    | WET_NO3: Initial concentration of NO3-N in wetland [mg N/l]")
                lSB.AppendLine(Format(lRow.Item(42), "0.000").PadLeft(16) & "    | WET_SOLP: Initial concentration of soluble P in wetland [mg P/l]")
                lSB.AppendLine(Format(lRow.Item(43), "0.000").PadLeft(16) & "    | WET_ORGN: Initial concentration of organic N in wetland [mg N/l]")
                lSB.AppendLine(Format(lRow.Item(44), "0.000").PadLeft(16) & "    | WET_ORGP: Initial concentration of organic P in wetland [mg P/l]")

                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & lTextFilename, lSB.ToString)
                'ReplaceNonAscii(lTextFilename)
            Next
        End Sub
    End Class
End Class

