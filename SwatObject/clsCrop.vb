'OBJECTID,ICNUM,CPNM,IDC,CROPNAME,BIO_E,HVSTI,BLAI,FRGRW1,LAIMX1,FRGRW2,LAIMX2,DLAI,CHTMX,RDMX,T_OPT,T_BASE,CNYLD,CPYLD,BN1,BN2,BN3,BP1,BP2,BP3,WSYF,USLE_C,GSI,VPDFR,FRGMAX,WAVP,CO2HI,BIOEHI,RSDCO_PL,OV_N,CN2A,CN2B,CN2C,CN2D,FERTFIELD,ALAI_MIN,BIO_LEAF,MAT_YRS,BMX_TREES,EXT_COEF
Partial Class SwatInput
    Private pCrop As clsCrop = New clsCrop(Me)
    ReadOnly Property Crop() As clsCrop
        Get
            Return pCrop
        End Get
    End Property

    Public Class clsCropItem
        Public ICNUM As Double
        Public CPNM As String
        Public IDC As Integer
        Public CROPNAME As String
        Public BIO_E As Single
        Public HVSTI As Single
        Public BLAI As Single
        Public FRGRW1 As Single
        Public LAIMX1 As Single
        Public FRGRW2 As Single
        Public LAIMX2 As Single
        Public DLAI As Single
        Public CHTMX As Single
        Public RDMX As Single
        Public T_OPT As Single
        Public T_BASE As Single
        Public CNYLD As Single
        Public CPYLD As Single
        Public BN1 As Single
        Public BN2 As Single
        Public BN3 As Single
        Public BP1 As Single
        Public BP2 As Single
        Public BP3 As Single
        Public WSYF As Single
        Public USLE_C As Single
        Public GSI As Single
        Public VPDFR As Single
        Public FRGMAX As Single
        Public WAVP As Single
        Public CO2HI As Single
        Public BIOEHI As Single
        Public RSDCO_PL As Single
        Public OV_N As Single
        Public CN2A As Single
        Public CN2B As Single
        Public CN2C As Single
        Public CN2D As Single
        Public FERTFIELD As Integer
        Public ALAI_MIN As Single
        Public BIO_LEAF As Single
        Public MAT_YRS As Double
        Public BMX_TREES As Double
        Public EXT_COEF As Double

        Public Sub New()
        End Sub

        Public Sub New(ByVal aRow As DataRow)
            PopulateObject(aRow, Me)
        End Sub

        Public Function AddSQL() As String
            Dim lSQL As String = "INSERT INTO crop ( " & clsDataColumn.ColumnNames(clsHru.Columns) & " ) Values (" _
                               & ICNUM & ", " _
                               & CPNM & ", " _
                               & IDC & ", " _
                               & CROPNAME & ", " _
                               & BIO_E & ", " _
                               & HVSTI & ", " _
                               & BLAI & ", " _
                               & FRGRW1 & ", " _
                               & LAIMX1 & ", " _
                               & FRGRW2 & ", " _
                               & LAIMX2 & ", " _
                               & DLAI & ", " _
                               & CHTMX & ", " _
                               & RDMX & ", " _
                               & T_OPT & ", " _
                               & T_BASE & ", " _
                               & CNYLD & ", " _
                               & CPYLD & ", " _
                               & BN1 & ", " _
                               & BN2 & ", " _
                               & BN3 & ", " _
                               & BP1 & ", " _
                               & BP2 & ", " _
                               & BP3 & ", " _
                               & WSYF & ", " _
                               & USLE_C & ", " _
                               & GSI & ", " _
                               & VPDFR & ", " _
                               & FRGMAX & ", " _
                               & WAVP & ", " _
                               & CO2HI & ", " _
                               & BIOEHI & ", " _
                               & RSDCO_PL & ", " _
                               & OV_N & ", " _
                               & CN2A & ", " _
                               & CN2B & ", " _
                               & CN2C & ", " _
                               & CN2D & ", " _
                               & FERTFIELD & ", " _
                               & ALAI_MIN & ", " _
                               & BIO_LEAF & ", " _
                               & MAT_YRS & ", " _
                               & BMX_TREES & ", " _
                               & EXT_COEF & " )"
            Return lSQL
        End Function
    End Class

    Public Class clsCrop
        Private pSwatInput As SwatInput
        Private pTableName As String = "crop"

        Friend Shared Columns As Generic.List(Of clsDataColumn)

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
            If Columns Is Nothing Then InitColumns()
        End Sub

        Private Shared Sub InitColumns()
            Columns = New Generic.List(Of clsDataColumn)
            Columns.Add(New clsDataColumn("ICNUM", 1, "Double", "0", 4, ""))
            Columns.Add(New clsDataColumn("CPNM", 1, "VARCHAR(4)", "%s", 6, ""))
            Columns.Add(New clsDataColumn("IDC", 1, "Integer", "0", 4, vbCrLf))
            Columns.Add(New clsDataColumn("CROPNAME", 1, "VARCHAR(30)", "", 0, ""))
            Columns.Add(New clsDataColumn("BIO_E", 1, "Single", "0.00", 7, ""))
            Columns.Add(New clsDataColumn("HVSTI", 1, "Single", "0.000", 7, ""))
            Columns.Add(New clsDataColumn("BLAI", 1, "Single", "0.00", 8, ""))
            Columns.Add(New clsDataColumn("FRGRW1", 1, "Single", "0.00", 7, ""))
            Columns.Add(New clsDataColumn("LAIMX1", 1, "Single", "0.00", 7, ""))
            Columns.Add(New clsDataColumn("FRGRW2", 1, "Single", "0.00", 7, ""))
            Columns.Add(New clsDataColumn("LAIMX2", 1, "Single", "0.00", 7, ""))
            Columns.Add(New clsDataColumn("DLAI", 1, "Single", "0.00", 7, ""))
            Columns.Add(New clsDataColumn("CHTMX", 1, "Single", "0.00", 8, ""))
            Columns.Add(New clsDataColumn("RDMX", 1, "Single", "0.00", 7, vbCrLf))
            Columns.Add(New clsDataColumn("T_OPT", 1, "Single", "0.00", 7, ""))
            Columns.Add(New clsDataColumn("T_BASE", 1, "Single", "0.00", 8, ""))
            Columns.Add(New clsDataColumn("CNYLD", 1, "Single", "0.0000", 9, ""))
            Columns.Add(New clsDataColumn("CPYLD", 1, "Single", "0.0000", 9, ""))
            Columns.Add(New clsDataColumn("BN1", 1, "Single", "0.0000", 9, ""))
            Columns.Add(New clsDataColumn("BN2", 1, "Single", "0.0000", 9, ""))
            Columns.Add(New clsDataColumn("BN3", 1, "Single", "0.0000", 9, ""))
            Columns.Add(New clsDataColumn("BP1", 1, "Single", "0.0000", 9, ""))
            Columns.Add(New clsDataColumn("BP2", 1, "Single", "0.0000", 9, ""))
            Columns.Add(New clsDataColumn("BP3", 1, "Single", "0.0000", 9, vbCrLf))
            Columns.Add(New clsDataColumn("WSYF", 1, "Single", "0.000", 7, ""))
            Columns.Add(New clsDataColumn("USLE_C", 1, "Single", "0.0000", 9, ""))
            Columns.Add(New clsDataColumn("GSI", 1, "Single", "0.0000", 9, ""))
            Columns.Add(New clsDataColumn("VPDFR", 1, "Single", "0.00", 7, ""))
            Columns.Add(New clsDataColumn("FRGMAX", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("WAVP", 1, "Single", "0.00", 8, ""))
            Columns.Add(New clsDataColumn("CO2HI", 1, "Single", "0.00", 10, ""))
            Columns.Add(New clsDataColumn("BIOEHI", 1, "Single", "0.00", 9, ""))
            Columns.Add(New clsDataColumn("RSDCO_PL", 1, "Single", "0.0000", 9, ""))

            Columns.Add(New clsDataColumn("OV_N", 1, "Single", "", 0, ""))
            Columns.Add(New clsDataColumn("CN2A", 1, "Single", "", 0, ""))
            Columns.Add(New clsDataColumn("CN2B", 1, "Single", "", 0, ""))
            Columns.Add(New clsDataColumn("CN2C", 1, "Single", "", 0, ""))
            Columns.Add(New clsDataColumn("CN2D", 1, "Single", "", 0, ""))
            Columns.Add(New clsDataColumn("FERTFIELD", 1, "Integer", "", 0, ""))

            Columns.Add(New clsDataColumn("ALAI_MIN", 1, "Single", "0.000", 8, vbCrLf))
            Columns.Add(New clsDataColumn("BIO_LEAF", 1, "Single", "0.000", 7, ""))
            Columns.Add(New clsDataColumn("MAT_YRS", 1, "Double", "0", 6, ""))
            Columns.Add(New clsDataColumn("BMX_TREES", 1, "Double", "0.00", 8, ""))
            Columns.Add(New clsDataColumn("EXT_COEF", 1, "Double", "0.000", 8, ""))
        End Sub

        Public Function TableCreate() As Boolean
            '    'based on mwSWATPlugIn.DBLayer.createChmTable
            '    Try
            '        DropTable(pTableName, pSwatInput.CnSwatInput)

            '        Dim lConnection As New ADODB.Connection
            '        lConnection.Open(pSwatInput.CnSwatParm.ConnectionString)

            '        'Open the Catalog
            '        Dim lCatalog As New ADOX.Catalog
            '        lCatalog.ActiveConnection = lConnection

            '        'Create the table
            '        Dim lTable As New ADOX.Table
            '        lTable.Name = pTableName

            '        Dim lKeyColumn As New ADOX.Column
            '        With lKeyColumn
            '            .Name = "OBJECTID"
            '            .Type = ADOX.DataTypeEnum.adInteger
            '            .ParentCatalog = lCatalog
            '            .Properties("AutoIncrement").Value = True
            '        End With

            '        Dim i As Integer

            '        With lTable.Columns
            '            .Append(lKeyColumn)

            '            .Append("ICNUM", ADOX.DataTypeEnum.adDouble)
            '            .Append("CPNM", ADOX.DataTypeEnum.adVarWChar, 4)
            '            .Append("IDC", ADOX.DataTypeEnum.adInteger)
            '            .Append("CROPNAME", ADOX.DataTypeEnum.adVarWChar, 30)
            '            .Append("BIO_E", ADOX.DataTypeEnum.adSingle)
            '            .Append("HVSTI", ADOX.DataTypeEnum.adSingle)
            '            .Append("BLAI", ADOX.DataTypeEnum.adSingle)
            '            .Append("FRGRW1", ADOX.DataTypeEnum.adSingle)
            '            .Append("LAIMX1", ADOX.DataTypeEnum.adSingle)
            '            .Append("FRGRW2", ADOX.DataTypeEnum.adSingle)
            '            .Append("LAIMX2", ADOX.DataTypeEnum.adSingle)
            '            .Append("DLAI", ADOX.DataTypeEnum.adSingle)
            '            .Append("CHTMX", ADOX.DataTypeEnum.adSingle)
            '            .Append("RDMX", ADOX.DataTypeEnum.adSingle)
            '            .Append("T_OPT", ADOX.DataTypeEnum.adSingle)
            '            .Append("T_BASE", ADOX.DataTypeEnum.adSingle)
            '            .Append("CNYLD", ADOX.DataTypeEnum.adSingle)
            '            .Append("CPYLD", ADOX.DataTypeEnum.adSingle)
            '            .Append("BN1", ADOX.DataTypeEnum.adSingle)
            '            .Append("BN2", ADOX.DataTypeEnum.adSingle)
            '            .Append("BN3", ADOX.DataTypeEnum.adSingle)
            '            .Append("BP1", ADOX.DataTypeEnum.adSingle)
            '            .Append("BP2", ADOX.DataTypeEnum.adSingle)
            '            .Append("BP3", ADOX.DataTypeEnum.adSingle)
            '            .Append("WSYF", ADOX.DataTypeEnum.adSingle)
            '            .Append("USLE_C", ADOX.DataTypeEnum.adSingle)
            '            .Append("GSI", ADOX.DataTypeEnum.adSingle)
            '            .Append("VPDFR", ADOX.DataTypeEnum.adSingle)
            '            .Append("FRGMAX", ADOX.DataTypeEnum.adSingle)
            '            .Append("WAVP", ADOX.DataTypeEnum.adSingle)
            '            .Append("CO2HI", ADOX.DataTypeEnum.adSingle)
            '            .Append("BIOEHI", ADOX.DataTypeEnum.adSingle)
            '            .Append("RSDCO_PL", ADOX.DataTypeEnum.adSingle)
            '            .Append("OV_N", ADOX.DataTypeEnum.adSingle)
            '            .Append("CN2A", ADOX.DataTypeEnum.adSingle)
            '            .Append("CN2B", ADOX.DataTypeEnum.adSingle)
            '            .Append("CN2C", ADOX.DataTypeEnum.adSingle)
            '            .Append("CN2D", ADOX.DataTypeEnum.adSingle)
            '            .Append("FERTFIELD", ADOX.DataTypeEnum.adInteger)
            '            .Append("ALAI_MIN", ADOX.DataTypeEnum.adSingle)
            '            .Append("BIO_LEAF", ADOX.DataTypeEnum.adSingle)
            '            .Append("MAT_YRS", ADOX.DataTypeEnum.adDouble)
            '            .Append("BMX_TREES", ADOX.DataTypeEnum.adDouble)
            '            .Append("EXT_COEF", ADOX.DataTypeEnum.adDouble)
            '        End With

            '        lTable.Keys.Append("PrimaryKey", ADOX.KeyTypeEnum.adKeyPrimary, lKeyColumn.Name)
            '        lCatalog.Tables.Append(lTable)
            '        lTable = Nothing
            '        lCatalog = Nothing
            '        lConnection.Close()
            '        lConnection = Nothing
            '        Return True
            '    Catch lEx As ApplicationException
            '        Debug.Print(lEx.Message)
            '        Return False
            '    End Try
        End Function

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryGDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Sub Add(ByVal aItem As clsCropItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatParm)
        End Sub

        Public Function FindCrop(ByVal aCropName As String, Optional ByVal aTable As DataTable = Nothing) As clsCropItem
            If aTable Is Nothing Then aTable = Table()
            For Each lRow As DataRow In aTable.Rows
                If lRow.Item("CPNM").ToString = aCropName Then
                    Return New clsCropItem(lRow)
                End If
            Next
            Return Nothing
        End Function

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            pSwatInput.Status("Writing " & pTableName & " text ...")
            If aTable Is Nothing Then aTable = Table()
            SaveTableAsText(aTable, Columns, pSwatInput.TxtInOutFolder & "\" & pTableName & ".dat")
        End Sub
    End Class
End Class
