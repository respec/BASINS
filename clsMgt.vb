Partial Class SwatInput
    Private pMgt As clsMgt = New clsMgt(Me)
    ReadOnly Property Mgt() As clsMgt
        Get
            Return pMgt
        End Get
    End Property

    ''' <summary>
    ''' Management (MGT) input section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsMgt
        Private pSwatInput As SwatInput

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            Return Table1Create() And Table2Create()
        End Function

        Public Function Table1Create() As Boolean
            'based on mwSWATPlugIn:DBLayer:createHruTable
            Try
                'Open the connection
                Dim lConnection As ADODB.Connection = pSwatInput.OpenADOConnection()

                'Open the Catalog
                Dim lCatalog As New ADOX.Catalog
                lCatalog.ActiveConnection = lConnection

                'Create the table
                Dim lTable As New ADOX.Table
                lTable.Name = "mgt1"

                Dim lKeyColumn As New ADOX.Column
                With lKeyColumn
                    .Name = "OID"
                    .Type = ADOX.DataTypeEnum.adInteger
                    .ParentCatalog = lCatalog
                    .Properties("AutoIncrement").Value = True
                End With

                With lTable.Columns
                    .Append(lKeyColumn)
                    .Append("SUBBASIN", ADOX.DataTypeEnum.adDouble)
                    .Append("HRU", ADOX.DataTypeEnum.adDouble)
                    .Append("LANDUSE", ADOX.DataTypeEnum.adVarWChar, 4)
                    .Append("SOIL", ADOX.DataTypeEnum.adVarWChar, 40)
                    .Append("SLOPE_CD", ADOX.DataTypeEnum.adVarWChar, 20)
                    .Append("IGRO", ADOX.DataTypeEnum.adInteger)
                    .Append("PLANT_ID", ADOX.DataTypeEnum.adInteger)
                    .Append("LAI_INIT", ADOX.DataTypeEnum.adSingle)
                    .Append("BIO_INIT", ADOX.DataTypeEnum.adSingle)
                    .Append("PHU_PLT", ADOX.DataTypeEnum.adSingle)
                    .Append("BIOMIX", ADOX.DataTypeEnum.adSingle)
                    .Append("CN2", ADOX.DataTypeEnum.adSingle)
                    .Append("USLE_P", ADOX.DataTypeEnum.adSingle)
                    .Append("BIO_MIN", ADOX.DataTypeEnum.adSingle)
                    .Append("FILTERW", ADOX.DataTypeEnum.adSingle)
                    .Append("IURBAN", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("URBLU", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IRRSC", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IRRNO", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("FLOWMIN", ADOX.DataTypeEnum.adSingle)
                    .Append("DIVMAX", ADOX.DataTypeEnum.adSingle)
                    .Append("FLOWFR", ADOX.DataTypeEnum.adSingle)
                    .Append("DDRAIN", ADOX.DataTypeEnum.adSingle)
                    .Append("TDRAIN", ADOX.DataTypeEnum.adSingle)
                    .Append("GDRAIN", ADOX.DataTypeEnum.adSingle)
                    .Append("NROT", ADOX.DataTypeEnum.adInteger)
                    .Append("HUSC", ADOX.DataTypeEnum.adInteger)
                    .Append("ISCROP", ADOX.DataTypeEnum.adInteger)
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

        Public Function Table2Create() As Boolean
            'based on mwSWATPlugIn:DBLayer:createHruTable
            Try
                'Open the connection
                Dim lConnection As ADODB.Connection = pSwatInput.OpenADOConnection()

                'Open the Catalog
                Dim lCatalog As New ADOX.Catalog
                lCatalog.ActiveConnection = lConnection

                'Create the table
                Dim lTable As New ADOX.Table
                lTable.Name = "mgt2"

                Dim lKeyColumn As New ADOX.Column
                With lKeyColumn
                    .Name = "OID"
                    .Type = ADOX.DataTypeEnum.adInteger
                    .ParentCatalog = lCatalog
                    .Properties("AutoIncrement").Value = True
                End With

                With lTable.Columns
                    .Append(lKeyColumn)
                    .Append("SUBBASIN", ADOX.DataTypeEnum.adDouble)
                    .Append("HRU", ADOX.DataTypeEnum.adDouble)
                    .Append("LANDUSE", ADOX.DataTypeEnum.adVarWChar, 4)
                    .Append("SOIL", ADOX.DataTypeEnum.adVarWChar, 40)
                    .Append("SLOPE_CD", ADOX.DataTypeEnum.adVarWChar, 20)
                    .Append("CROP", ADOX.DataTypeEnum.adVarWChar, 4)
                    .Append("YEAR", ADOX.DataTypeEnum.adInteger)
                    Dim clxMonth As New ADOX.Column
                    With clxMonth
                        .Name = "MONTH"
                        .Type = ADOX.DataTypeEnum.adInteger
                        .ParentCatalog = lCatalog
                        .Properties("Nullable").Value = True
                    End With
                    .Append(clxMonth)

                    Dim clxDay As New ADOX.Column
                    With clxDay
                        .Name = "DAY"
                        .Type = ADOX.DataTypeEnum.adInteger
                        .ParentCatalog = lCatalog
                        .Properties("Nullable").Value = True
                    End With
                    .Append(clxDay)
                    .Append("HUSC", ADOX.DataTypeEnum.adSingle)
                    .Append("MGT_OP", ADOX.DataTypeEnum.adInteger)
                    .Append("HEATUNITS", ADOX.DataTypeEnum.adSingle)
                    .Append("PLANT_ID", ADOX.DataTypeEnum.adInteger)
                    .Append("CURYR_MAT", ADOX.DataTypeEnum.adInteger)
                    .Append("LAI_INIT", ADOX.DataTypeEnum.adSingle)
                    .Append("BIO_INIT", ADOX.DataTypeEnum.adSingle)
                    .Append("HI_TARG", ADOX.DataTypeEnum.adSingle)
                    .Append("BIO_TARG", ADOX.DataTypeEnum.adSingle)
                    .Append("CNOP", ADOX.DataTypeEnum.adDouble)
                    .Append("IRR_AMT", ADOX.DataTypeEnum.adSingle)
                    .Append("FERT_ID", ADOX.DataTypeEnum.adInteger)
                    .Append("FRT_KG", ADOX.DataTypeEnum.adSingle)
                    .Append("FRT_SURFACE", ADOX.DataTypeEnum.adSingle)
                    .Append("PEST_ID", ADOX.DataTypeEnum.adInteger)
                    .Append("PST_KG", ADOX.DataTypeEnum.adSingle)
                    .Append("TILLAGE_ID", ADOX.DataTypeEnum.adInteger)
                    .Append("HARVEFF", ADOX.DataTypeEnum.adSingle)
                    .Append("HI_OVR", ADOX.DataTypeEnum.adSingle)
                    .Append("GRZ_DAYS", ADOX.DataTypeEnum.adInteger)
                    .Append("MANURE_ID", ADOX.DataTypeEnum.adInteger)
                    .Append("BIO_EAT", ADOX.DataTypeEnum.adSingle)
                    .Append("BIO_TRMP", ADOX.DataTypeEnum.adSingle)
                    .Append("MANURE_KG", ADOX.DataTypeEnum.adSingle)
                    .Append("WSTRS_ID", ADOX.DataTypeEnum.adInteger)
                    .Append("AUTO_WSTRS", ADOX.DataTypeEnum.adSingle)
                    .Append("AFERT_ID", ADOX.DataTypeEnum.adInteger)
                    .Append("AUTO_NSTRS", ADOX.DataTypeEnum.adSingle)
                    .Append("AUTO_NAPP", ADOX.DataTypeEnum.adSingle)
                    .Append("AUTO_NYR", ADOX.DataTypeEnum.adInteger)
                    .Append("AUTO_EFF", ADOX.DataTypeEnum.adSingle)
                    .Append("AFRT_SURFACE", ADOX.DataTypeEnum.adSingle)
                    .Append("SWEEPEFF", ADOX.DataTypeEnum.adSingle)
                    .Append("FR_CURB", ADOX.DataTypeEnum.adSingle)
                    .Append("IMP_TRIG", ADOX.DataTypeEnum.adDouble)
                    .Append("FERT_DAYS", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("CFRT_ID", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IFRT_FREQ", ADOX.DataTypeEnum.adInteger, 4)

                    Dim clxCFRT_KG As New ADOX.Column
                    With clxCFRT_KG
                        .Name = "CFRT_KG"
                        .Type = ADOX.DataTypeEnum.adSingle
                        .ParentCatalog = lCatalog
                        .Properties("Nullable").Value = True
                    End With
                    .Append(clxCFRT_KG)
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

        Public Function Table1() As DataTable
            pSwatInput.Status("Reading MGT1 from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM mgt1 ORDER BY SUBBASIN, HRU;")
        End Function

        Public Function Table2() As DataTable
            pSwatInput.Status("Reading MGT2 from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM mgt2 ORDER BY SUBBASIN, HRU, [YEAR], [MONTH], [DAY], HUSC;")
        End Function

        Public Sub Add1(ByVal SUBBASIN As Double, _
                        ByVal HRU As Double, _
                        ByVal LANDUSE As String, _
                        ByVal SOIL As String, _
                        ByVal SLOPE_CD As String, _
                        ByVal IGRO As Integer, _
                        ByVal PLANT_ID As Integer, _
                        ByVal LAI_INIT As Single, _
                        ByVal BIO_INIT As Single, _
                        ByVal PHU_PLT As Single, _
                        ByVal BIOMIX As Single, _
                        ByVal CN2 As Single, _
                        ByVal USLE_P As Single, _
                        ByVal BIO_MIN As Single, _
                        ByVal FILTERW As Single, _
                        ByVal IURBAN As Long, _
                        ByVal URBLU As Long, _
                        ByVal IRRSC As Long, _
                        ByVal IRRNO As Long, _
                        ByVal FLOWMIN As Single, _
                        ByVal DIVMAX As Single, _
                        ByVal FLOWFR As Single, _
                        ByVal DDRAIN As Single, _
                        ByVal TDRAIN As Single, _
                        ByVal GDRAIN As Single, _
                        ByVal NROT As Integer, _
                        ByVal HUSC As Integer, _
                        ByVal ISCROP As Integer)

            Dim lSQL As String = "INSERT INTO mgt1 ( SUBBASIN , HRU , LANDUSE , SOIL , SLOPE_CD , IGRO , PLANT_ID , LAI_INIT , BIO_INIT , PHU_PLT , BIOMIX , CN2 , USLE_P , BIO_MIN , FILTERW , IURBAN , URBLU , IRRSC , IRRNO , FLOWMIN , DIVMAX , FLOWFR , DDRAIN , TDRAIN , GDRAIN , NROT , HUSC , ISCROP  ) " _
                               & "Values ('" & SUBBASIN & "'  ,'" & HRU & "'  ,'" & LANDUSE & "'  ,'" & SOIL & "'  ,'" & SLOPE_CD & "'  ,'" & IGRO & "'  ,'" & PLANT_ID & "'  ,'" & LAI_INIT & "'  ,'" & BIO_INIT & "'  ,'" & PHU_PLT & "'  ,'" & BIOMIX & "'  ,'" & CN2 & "'  ,'" & USLE_P & "'  ,'" & BIO_MIN & "'  ,'" & FILTERW & "'  ,'" & IURBAN & "'  ,'" & URBLU & "'  ,'" & IRRSC & "'  ,'" & IRRNO & "'  ,'" & FLOWMIN & "'  ,'" & DIVMAX & "'  ,'" & FLOWFR & "'  ,'" & DDRAIN & "'  ,'" & TDRAIN & "'  ,'" & GDRAIN & "'  ,'" & NROT & "'  ,'" & HUSC & "'  ,'" & ISCROP & "'  )"
            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

        Public Sub Add2(ByVal SUBBASIN As Double, _
                        ByVal HRU As Double, _
                        ByVal LANDUSE As String, _
                        ByVal SOIL As String, _
                        ByVal SLOPE_CD As String, _
                        ByVal CROP As String, _
                        ByVal YEAR As Integer, _
                        ByVal MONTH As Integer, _
                        ByVal DAY As Integer, _
                        ByVal HUSC As Single, _
                        ByVal MGT_OP As Integer, _
                        ByVal HEATUNITS As Single, _
                        ByVal PLANT_ID As Integer, _
                        ByVal CURYR_MAT As Integer, _
                        ByVal LAI_INIT As Single, _
                        ByVal BIO_INIT As Single, _
                        ByVal HI_TARG As Single, _
                        ByVal BIO_TARG As Single, _
                        ByVal CNOP As Double, _
                        ByVal IRR_AMT As Single, _
                        ByVal FERT_ID As Integer, _
                        ByVal FRT_KG As Single, _
                        ByVal FRT_SURFACE As Single, _
                        ByVal PEST_ID As Integer, _
                        ByVal PST_KG As Single, _
                        ByVal TILLAGE_ID As Integer, _
                        ByVal HARVEFF As Single, _
                        ByVal HI_OVR As Single, _
                        ByVal GRZ_DAYS As Integer, _
                        ByVal MANURE_ID As Integer, _
                        ByVal BIO_EAT As Single, _
                        ByVal BIO_TRMP As Single, _
                        ByVal MANURE_KG As Single, _
                        ByVal WSTRS_ID As Integer, _
                        ByVal AUTO_WSTRS As Single, _
                        ByVal AFERT_ID As Integer, _
                        ByVal AUTO_NSTRS As Single, _
                        ByVal AUTO_NAPP As Single, _
                        ByVal AUTO_NYR As Single, _
                        ByVal AUTO_EFF As Single, _
                        ByVal AFRT_SURFACE As Single, _
                        ByVal SWEEPEFF As Single, _
                        ByVal FR_CURB As Single, _
                        ByVal IMP_TRIG As Double, _
                        ByVal FERT_DAYS As Long, _
                        ByVal CFRT_ID As Long, _
                        ByVal IFRT_FREQ As Long, _
                        ByVal CFRT_KG As Single)

            Dim lSQL As String = "INSERT INTO mgt2 ( SUBBASIN , HRU , LANDUSE , SOIL , SLOPE_CD , CROP , [YEAR] , [MONTH] , [DAY] , HUSC , MGT_OP , HEATUNITS , PLANT_ID , CURYR_MAT , LAI_INIT , BIO_INIT , HI_TARG , BIO_TARG , CNOP , IRR_AMT , FERT_ID , FRT_KG , FRT_SURFACE , PEST_ID , PST_KG , TILLAGE_ID , HARVEFF , HI_OVR , GRZ_DAYS , MANURE_ID , BIO_EAT , BIO_TRMP , MANURE_KG , WSTRS_ID , AUTO_WSTRS , AFERT_ID , AUTO_NSTRS , AUTO_NAPP , AUTO_NYR , AUTO_EFF , AFRT_SURFACE , SWEEPEFF , FR_CURB , IMP_TRIG , FERT_DAYS , CFRT_ID , IFRT_FREQ , CFRT_KG  ) " _
                               & "Values (" & SUBBASIN & "  ," & HRU & "  ,'" & LANDUSE & "'  ,'" & SOIL & "'  ,'" & SLOPE_CD & "'  ,'" & CROP & "'  ," & YEAR & "  ," & MONTH & "  ," & DAY & "  ," & HUSC & "  ," & MGT_OP & "  ," & HEATUNITS & "  ," & PLANT_ID & "  ," & CURYR_MAT & "  ," & LAI_INIT & "  ," & BIO_INIT & "  ," & HI_TARG & "  ," & BIO_TARG & "  ," & CNOP & "  ," & IRR_AMT & "  ," & FERT_ID & "  ," & FRT_KG & "  ," & FRT_SURFACE & "  ," & PEST_ID & "  ," & PST_KG & "  ," & TILLAGE_ID & "  ," & HARVEFF & "  ," & HI_OVR & "  ," & GRZ_DAYS & "  ," & MANURE_ID & "  ," & BIO_EAT & "  ," & BIO_TRMP & "  ," & MANURE_KG & "  ," & WSTRS_ID & "  ," & AUTO_WSTRS & "  ," & AFERT_ID & "  ," & AUTO_NSTRS & "  ," & AUTO_NAPP & "  ," & AUTO_NYR & "  ," & AUTO_EFF & "  ," & AFRT_SURFACE & "  ," & SWEEPEFF & "  ," & FR_CURB & "  ," & IMP_TRIG & "  ," & FERT_DAYS & "  ," & CFRT_ID & "  ," & IFRT_FREQ & "  ," & CFRT_KG & ")"        ' Create the Command and set its properties
            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

        ''' <summary>
        ''' Save MGT information to set of .mgt text input files
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Save(Optional ByVal aTable1 As DataTable = Nothing, Optional ByVal aTable2 As DataTable = Nothing)
            If aTable1 Is Nothing Then aTable1 = Table1()
            If aTable2 Is Nothing Then aTable2 = Table2()

            pSwatInput.Status("Writing MGT text ...")
            For Each lMgt1Row As DataRow In aTable1.Rows
                Dim lSubBasin As String = lMgt1Row.Item(1).ToString.Trim
                Dim lHruNum As String = lMgt1Row.Item(2).ToString.Trim

                Dim lSB As New System.Text.StringBuilder
                '1st line
                lSB.AppendLine(" .mgt file Subbasin:" & lSubBasin & " HRU:" & lHruNum & " Luse:" & lMgt1Row.Item(3) _
                             & " Soil: " & lMgt1Row.Item(4) & " Slope: " & lMgt1Row.Item(5) _
                             & " " & HeaderString())
                '2. NMGT
                lSB.AppendLine(Format(0, "0").PadLeft(16) & "    | NMGT:Management code")

                lSB.AppendLine("Initial Plant Growth Parameters")
                'IGRO
                lSB.AppendLine(Format(lMgt1Row.Item(("IGRO")), "0").PadLeft(16) & "    | IGRO: Land cover status: 0-none growing; 1-growing")
                'PLANT_ID
                lSB.AppendLine(Format(lMgt1Row.Item(("PLANT_ID")), "0").PadLeft(16) & "    | PLANT_ID: Land cover ID number (IGRO = 1)")
                'LAI_INIT
                lSB.AppendLine(Format(lMgt1Row.Item(("LAI_INIT")), "0.00").PadLeft(16) & "    | LAI_INIT: Initial leaf are index (IGRO = 1)")
                'BIO_INIT
                lSB.AppendLine(Format(lMgt1Row.Item(("BIO_INIT")), "0.00").PadLeft(16) & "    | BIO_INIT: Initial biomass (kg/ha) (IGRO = 1)")
                'PHU_PLT
                lSB.AppendLine(Format(lMgt1Row.Item(("PHU_PLT")), "0.00").PadLeft(16) & "    | PHU_PLT: Number of heat units to bring plant to maturity (IGRO = 1)")

                lSB.AppendLine("General Management Parameters")
                'BIOMIX
                lSB.AppendLine(Format(lMgt1Row.Item(("BIOMIX")), "0.00").PadLeft(16) & "    | BIOMIX: Biological mixing efficiency")
                'CN2
                lSB.AppendLine(Format(lMgt1Row.Item(("CN2")), "0.00").PadLeft(16) & "    | CN2: Initial SCS CN II value")
                'USLE_P
                lSB.AppendLine(Format(lMgt1Row.Item(("USLE_P")), "0.00").PadLeft(16) & "    | USLE_P: USLE support practice factor")
                'BIO_MIN
                lSB.AppendLine(Format(lMgt1Row.Item(("BIO_MIN")), "0.00").PadLeft(16) & "    | BIO_MIN: Minimum biomass for grazing (kg/ha)")
                'FILTERW
                lSB.AppendLine(Format(lMgt1Row.Item(("FILTERW")), "0.000").PadLeft(16) & "    | FILTERW: width of edge of field filter strip (m)")

                lSB.AppendLine("Urban Management Parameters")
                'IURBAN
                lSB.AppendLine(Format(lMgt1Row.Item(("IURBAN")), "0").PadLeft(16) & "    | IURBAN: urban simulation code, 0-none, 1-USGS, 2-buildup/washoff")
                'URBLU
                lSB.AppendLine(Format(lMgt1Row.Item(("URBLU")), "0").PadLeft(16) & "    | URBLU: urban land type")

                lSB.AppendLine("Irrigation Management Parameters")
                'IRRSC
                lSB.AppendLine(Format(lMgt1Row.Item(("IRRSC")), "0").PadLeft(16) & "    | IRRSC: irrigation code")
                'IRRNO
                lSB.AppendLine(Format(lMgt1Row.Item(("IRRNO")), "0").PadLeft(16) & "    | IRRNO: irrigation source location")
                'FLOWMIN
                lSB.AppendLine(Format(lMgt1Row.Item(("FLOWMIN")), "0.000").PadLeft(16) & "    | FLOWMIN: min in-stream flow for irr diversions (m^3/s)")
                'DIVMAX
                lSB.AppendLine(Format(lMgt1Row.Item(("DIVMAX")), "0.000").PadLeft(16) & "    | DIVMAX: max irrigation diversion from reach (+mm/-10^4m^3)")
                'FLOWFR
                lSB.AppendLine(Format(lMgt1Row.Item(("FLOWFR")), "0.000").PadLeft(16) & "    | FLOWFR: : fraction of flow allowed to be pulled for irr")

                lSB.AppendLine("Tile Drain Management Parameters")

                'DDRAIN
                lSB.AppendLine(Format(lMgt1Row.Item(("DDRAIN")), "0.000").PadLeft(16) & "    | DDRAIN: depth to subsurface tile drain (mm)")
                'TDRAIN
                lSB.AppendLine(Format(lMgt1Row.Item(("TDRAIN")), "0.000").PadLeft(16) & "    | TDRAIN: time to drain soil to field capacity (hr)")
                'GDRAIN
                lSB.AppendLine(Format(lMgt1Row.Item(("GDRAIN")), "0.000").PadLeft(16) & "    | GDRAIN: drain tile lag time (hr)")

                lSB.AppendLine("Management Operations:")
                'NROT
                lSB.AppendLine(Format(lMgt1Row.Item(("NROT")), "0").PadLeft(16) & "    | NROT: number of years of rotation")
                lSB.AppendLine("Operation Schedule:")

                If lMgt1Row.Item(("NROT")) > 0 Then 'Print operations schedule if 1 or more operation years
                    '3d line 'from mgt2
                    Dim lBlnHu As Boolean
                    If lMgt1Row.Item(("HUSC")) = 0 Then
                        lBlnHu = True
                    Else
                        lBlnHu = False
                    End If

                    Dim lRowsMgt2() As DataRow = aTable2.Select("SUBBASIN = " & lMgt1Row.Item(1) & " AND HRU = " & lMgt1Row.Item(2), "[YEAR] ASC, [MONTH] ASC, [DAY] ASC, HUSC ASC")
                    Dim lYear As Integer = lRowsMgt2(0).Item(("YEAR"))

                    Dim lLine As String = ""

                    For Each lRowMgt2 As DataRow In lRowsMgt2
                        If lBlnHu = True Then
                            lLine = Strings.StrDup(7, " ") & Format(lRowMgt2.Item(("HUSC")), "##0.000").PadLeft(8)
                        Else
                            lLine = Format(lRowMgt2.Item(("MONTH")), "##").PadLeft(3) _
                                  & Format(lRowMgt2.Item(("DAY")), "##").PadLeft(3) & Strings.StrDup(9, " ")
                        End If
                        lLine &= Format(lRowMgt2.Item(("MGT_OP")), "##").PadLeft(3)

                        Dim lMgtOp As Integer = lRowMgt2.Item(("MGT_OP"))
                        Select Case lMgtOp
                            Case Is = -1 'skipped year
                                lLine = Strings.StrDup(19, " ") & "0" & Strings.StrDup(76, " ")
                            Case Is = 1
                                lLine &= Format(lRowMgt2.Item(("PLANT_ID")), "####").PadLeft(5) _
                                       & Format(lRowMgt2.Item(("CURYR_MAT")), "##").PadLeft(7) _
                                       & Format(lRowMgt2.Item(("HEATUNITS")), "#####0.00000").PadLeft(13) _
                                       & Format(lRowMgt2.Item(("LAI_INIT")), "##0.00").PadLeft(7) _
                                       & Format(lRowMgt2.Item(("BIO_INIT")), "####0.00000").PadLeft(12) _
                                       & Format(lRowMgt2.Item(("HI_TARG")), "0.00").PadLeft(5) _
                                       & Format(lRowMgt2.Item(("BIO_TARG")), "##0.00").PadLeft(7) _
                                       & Format(lRowMgt2.Item(("CNOP")), "#0.00").PadLeft(6)
                            Case Is = 2 'irrigation
                                lLine &= Space(13) & Format(lRowMgt2.Item(("IRR_AMT")), "#####0.00000").PadLeft(12)
                            Case Is = 3 'fertilizer: there are several variables that are not included in SWAT 2002 but are existing in the table i.e. FRMINN, etc. Those are not shown in the interface and it seems to be expected to be on the next version. In this version they are not printed in the files.
                                lLine &= Format(lRowMgt2.Item(("FERT_ID")), "###0").PadLeft(5) _
                                       & Format(lRowMgt2.Item(("FRT_KG")), "#####0.00000").PadLeft(20) _
                                       & Format(lRowMgt2.Item(("FRT_SURFACE")), "##0.00").PadLeft(7)
                            Case Is = 4 'pesticide
                                lLine &= Format(lRowMgt2.Item(("PEST_ID")), "###0").PadLeft(5) _
                                       & Format(lRowMgt2.Item(("PST_KG")), "#####0.00000").PadLeft(20)
                            Case Is = 5 'Harvest and kill operation
                                lLine &= Format(lRowMgt2.Item(("CNOP")), "#####0.00000").PadLeft(25)
                            Case Is = 6 'Tillage operation
                                lLine &= Format(lRowMgt2.Item(("TILLAGE_ID")), "###0").PadLeft(5) _
                                       & Format(lRowMgt2.Item(("CNOP")), "#####0.00000").PadLeft(20)
                            Case Is = 7 ' Harvest only operation
                                lLine &= Format(lRowMgt2.Item(("HARVEFF")), "#####0.00000").PadLeft(25) _
                                       & Format(lRowMgt2.Item(("HI_OVR")), "##0.00").PadLeft(17)
                            Case Is = 8 'KILL OPERATION

                            Case Is = 9 ' Grazing operation
                                lLine &= Format(lRowMgt2.Item(("GRZ_DAYS")), "###0").PadLeft(5) _
                                       & Format(lRowMgt2.Item(("MANURE_ID")), "##0").PadLeft(4) _
                                       & Format(lRowMgt2.Item(("BIO_EAT")), "#####0.00000").PadLeft(16) _
                                       & Format(lRowMgt2.Item(("BIO_TRMP")), "##0.00").PadLeft(7) _
                                       & Format(lRowMgt2.Item(("MANURE_KG")), "####0.00000").PadLeft(12)
                            Case Is = 10 'Auto irrigation: AUTO_RNF is a variable showing in AVSWAT but not shown in SWAT2002. It is not included here.
                                lLine &= Format(lRowMgt2.Item(("WSTRS_ID")), "###0").PadLeft(5) _
                                       & Format(lRowMgt2.Item(("AUTO_WSTRS")), "#####0.00000").PadLeft(20)
                            Case Is = 11 'Auto fert option
                                lLine &= Format(lRowMgt2.Item(("AFERT_ID")), "###0").PadLeft(5) _
                                       & Format(lRowMgt2.Item(("AUTO_NSTRS")), "#####0.00000").PadLeft(20) _
                                       & Format(lRowMgt2.Item(("AUTO_NAPP")), "##0.00").PadLeft(7) _
                                       & Format(lRowMgt2.Item(("AUTO_NYR")), "####0.00000").PadLeft(12) _
                                       & Format(lRowMgt2.Item(("AUTO_EFF")), "0.00").PadLeft(5) _
                                       & Format(lRowMgt2.Item(("AFRT_SURFACE")), "####0.00").PadLeft(9)
                            Case Is = 12 'street sweeping operation
                                lLine &= Format(lRowMgt2.Item(("SWEEPEFF")), "#####0.00000").PadLeft(25) _
                                       & Format(lRowMgt2.Item(("FR_CURB")), "##0.00").PadLeft(7)
                            Case Is = 13 'realease/impound
                                lLine &= Format(lRowMgt2.Item(("IMP_TRIG")), "###0").PadLeft(5)
                            Case Is = 14 'Continuous fertilizer
                                lLine &= Format(lRowMgt2.Item(("FERT_DAYS")), "###0").PadLeft(5) _
                                       & Format(lRowMgt2.Item(("CFRT_ID")), "##0").PadLeft(4) _
                                       & Format(lRowMgt2.Item(("IFRT_FREQ")), "#0").PadLeft(3) _
                                       & Format(lRowMgt2.Item(("CFRT_KG")), "#####0.0000").PadLeft(13)
                        End Select
                        'SKIP A LINE IN NEW YEAR
                        Dim lNYear As Integer = lRowMgt2.Item(("YEAR"))
                        If lNYear <> lYear Then
                            lYear = lNYear
                            If lMgtOp <> -1 Then
                                lSB.AppendLine(Strings.StrDup(19, " ") & "0" & Strings.StrDup(76, " "))
                            End If
                        End If
                        lSB.AppendLine(lLine)
                    Next
                End If
                lSB.AppendLine("")
                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & StringFnameHRUs(lSubBasin, lHruNum) & ".mgt", lSB.ToString)
            Next
        End Sub
    End Class
End Class
