Partial Class SwatInput
    ''' <summary>
    ''' SubBasin (SUB) input section
    ''' </summary>
    ''' <remarks></remarks>
    Private pSubBsn As clsSubBsn = New clsSubBsn(Me)
    ReadOnly Property SubBsn() As clsSubBsn
        Get
            Return pSubBsn
        End Get
    End Property

    Public Class clsSubBsn
        Private pSwatInput As SwatInput
        Private pTableName As String = "sub"
        'Private pFieldNames() As String = {"SUBBASIN", "SUB_KM", "SUB_LAT", "SUB_ELEV", "IRGAGE", "ITGAGE", "ISGAGE", "IHGAGE", "IWGAGE", _
        '                                   "ELEVB1", "ELEVB2", "ELEVB3", "ELEVB4", "ELEVB5", "ELEVB6", "ELEVB7", "ELEVB8", "ELEVB9", "ELEVB10", _
        '                                   "ELEVB_FR1", "ELEVB_FR2", "ELEVB_FR3", "ELEVB_FR4", "ELEVB_FR5", "ELEVB_FR6", "ELEVB_FR7", "ELEVB_FR8", "ELEVB_FR9", "ELEVB_FR10", _
        '                                   "SNOEB1", "SNOEB2", "SNOEB3", "SNOEB4", "SNOEB5", "SNOEB6", "SNOEB7", "SNOEB8", "SNOEB9", "SNOEB10", _
        '                                   "PLAPS", "TLAPS", "SNO_SUB", "CH_L1", "CH_S1", "CH_W1", "CH_K1", "CH_N1", "CO2", _
        '                                   "RFINC1", "RFINC2", "RFINC3", "RFINC4", "RFINC5", "RFINC6", "RFINC7", "RFINC8", "RFINC9", "RFINC10", "RFINC11", "RFINC12", _
        '                                   "TMPINC1", "TMPINC2", "TMPINC3", "TMPINC4", "TMPINC5", "TMPINC6", "TMPINC7", "TMPINC8", "TMPINC9", "TMPINC10", "TMPINC11", "TMPINC12", _
        '                                   "RADINC1", "RADINC2", "RADINC3", "RADINC4", "RADINC5", "RADINC6", "RADINC7", "RADINC8", "RADINC9", "RADINC10", "RADINC11", "RADINC12", _
        '                                   "HUMINC1", "HUMINC2", "HUMINC3", "HUMINC4", "HUMINC5", "HUMINC6", "HUMINC7", "HUMINC8", "HUMINC9", "HUMINC10", "HUMINC11", "HUMINC12", _
        '                                   "HRUTOT", "IPOT", "FCST_REG"}

        Private pFieldNames() As String = {"SUBBASIN", "SUB_KM", "SUB_LAT", "SUB_ELEV", "IRGAGE", "ITGAGE", "ISGAGE", "IHGAGE", "IWGAGE", _
                                           "ELEVB", _
                                           "ELEVB_FR", _
                                           "SNOEB", _
                                           "PLAPS", "TLAPS", "SNO_SUB", "CH_L1", "CH_S1", "CH_W1", "CH_K1", "CH_N1", "CO2", _
                                           "RFINC", _
                                           "TMPINC", _
                                           "RADINC", _
                                           "HUMINC", _
                                           "HRUTOT", "IPOT", "FCST_REG"}

        Private pFieldCounts() As Integer = {1, 1, 1, 1, 1, 1, 1, 1, 1, _
                                   10, _
                                   10, _
                                   10, _
                                   1, 1, 1, 1, 1, 1, 1, 1, 1, _
                                   12, _
                                   12, _
                                   12, _
                                   12, _
                                   1, 1, 1}

        Private pFieldTypes() As ADOX.DataTypeEnum = {ADOX.DataTypeEnum.adDouble, ADOX.DataTypeEnum.adDouble, ADOX.DataTypeEnum.adDouble, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adDouble, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger}

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn:DBLayer:createHruTable
            Try
                pSwatInput.dropTable(pTableName, pSwatInput.CnSwatInput)

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
                    Dim lFieldNum As Integer = 0
                    For Each lFieldName As String In pFieldNames
                        For lSameNameFieldIndex As Integer = 1 To pFieldCounts(lFieldNum)
                            Dim lUseName As String = lFieldName
                            If pFieldCounts(lFieldNum) > 1 Then lUseName &= lSameNameFieldIndex
                            If pFieldTypes(lFieldNum) = ADOX.DataTypeEnum.adInteger Then
                                .Append(lUseName, pFieldTypes(lFieldNum), 4)
                            Else
                                .Append(lUseName, pFieldTypes(lFieldNum))
                            End If
                        Next
                        lFieldNum += 1
                    Next
                    .Append("COMID", ADOX.DataTypeEnum.adInteger)
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

        Public Sub Add(ByVal SUBBASIN As Double, _
                        ByVal SUB_KM As Double, _
                        ByVal SUB_LAT As Double, _
                        ByVal SUB_ELEV As Double, _
                        ByVal IRGAGE As Long, _
                        ByVal ITGAGE As Long, _
                        ByVal ISGAGE As Long, _
                        ByVal IHGAGE As Long, _
                        ByVal IWGAGE As Long, _
                        ByVal ELEVB() As Single, _
                        ByVal ELEVB_FR() As Single, _
                        ByVal SNOEB() As Single, _
                        ByVal PLAPS As Single, _
                        ByVal TLAPS As Single, _
                        ByVal SNO_SUB As Single, _
                        ByVal CH_L1 As Single, _
                        ByVal CH_S1 As Single, _
                        ByVal CH_W1 As Single, _
                        ByVal CH_K1 As Single, _
                        ByVal CH_N1 As Single, _
                        ByVal CO2 As Single, _
                        ByVal RFINC() As Single, _
                        ByVal TMPINC() As Single, _
                        ByVal RADINC() As Single, _
                        ByVal HUMINC() As Single, _
                        ByVal HRUTOT As Long, _
                        ByVal IPOT As Long, _
                        ByVal FCST_REG As Long, _
                        ByVal COMID As Integer)

            Me.Add(SUBBASIN, SUB_KM, SUB_LAT, SUB_ELEV, IRGAGE, ITGAGE, ISGAGE, IHGAGE, IWGAGE, _
                   ELEVB(0), ELEVB(1), ELEVB(2), ELEVB(3), ELEVB(4), ELEVB(5), ELEVB(6), ELEVB(7), ELEVB(8), ELEVB(9), _
                   ELEVB_FR(0), ELEVB_FR(1), ELEVB_FR(2), ELEVB_FR(3), ELEVB_FR(4), ELEVB_FR(5), ELEVB_FR(6), ELEVB_FR(7), ELEVB_FR(8), ELEVB_FR(9), _
                   SNOEB(0), SNOEB(1), SNOEB(2), SNOEB(3), SNOEB(4), SNOEB(5), SNOEB(6), SNOEB(7), SNOEB(8), SNOEB(9), _
                   PLAPS, TLAPS, SNO_SUB, CH_L1, CH_S1, CH_W1, CH_K1, CH_N1, CO2, _
                   RFINC(0), RFINC(1), RFINC(2), RFINC(3), RFINC(4), RFINC(5), RFINC(6), RFINC(7), RFINC(8), RFINC(9), RFINC(10), RFINC(11), _
                   TMPINC(0), TMPINC(1), TMPINC(2), TMPINC(3), TMPINC(4), TMPINC(5), TMPINC(6), TMPINC(7), TMPINC(8), TMPINC(9), TMPINC(10), TMPINC(11), _
                   RADINC(0), RADINC(1), RADINC(2), RADINC(3), RADINC(4), RADINC(5), RADINC(6), RADINC(7), RADINC(8), RADINC(9), RADINC(10), RADINC(11), _
                   HUMINC(0), HUMINC(1), HUMINC(2), HUMINC(3), HUMINC(4), HUMINC(5), HUMINC(6), HUMINC(7), HUMINC(8), HUMINC(9), HUMINC(10), HUMINC(11), _
                   HRUTOT, IPOT, FCST_REG, COMID)
        End Sub

        Public Sub Add(ByVal SUBBASIN As Double, _
                        ByVal SUB_KM As Double, _
                        ByVal SUB_LAT As Double, _
                        ByVal SUB_ELEV As Double, _
                        ByVal IRGAGE As Long, _
                        ByVal ITGAGE As Long, _
                        ByVal ISGAGE As Long, _
                        ByVal IHGAGE As Long, _
                        ByVal IWGAGE As Long, _
                        ByVal ELEVB1 As Single, _
                        ByVal ELEVB2 As Single, _
                        ByVal ELEVB3 As Single, _
                        ByVal ELEVB4 As Single, _
                        ByVal ELEVB5 As Single, _
                        ByVal ELEVB6 As Single, _
                        ByVal ELEVB7 As Single, _
                        ByVal ELEVB8 As Single, _
                        ByVal ELEVB9 As Single, _
                        ByVal ELEVB10 As Single, _
                        ByVal ELEVB_FR1 As Single, _
                        ByVal ELEVB_FR2 As Single, _
                        ByVal ELEVB_FR3 As Single, _
                        ByVal ELEVB_FR4 As Single, _
                        ByVal ELEVB_FR5 As Single, _
                        ByVal ELEVB_FR6 As Single, _
                        ByVal ELEVB_FR7 As Single, _
                        ByVal ELEVB_FR8 As Single, _
                        ByVal ELEVB_FR9 As Single, _
                        ByVal ELEVB_FR10 As Single, _
                        ByVal SNOEB1 As Single, _
                        ByVal SNOEB2 As Single, _
                        ByVal SNOEB3 As Single, _
                        ByVal SNOEB4 As Single, _
                        ByVal SNOEB5 As Single, _
                        ByVal SNOEB6 As Single, _
                        ByVal SNOEB7 As Single, _
                        ByVal SNOEB8 As Single, _
                        ByVal SNOEB9 As Single, _
                        ByVal SNOEB10 As Single, _
                        ByVal PLAPS As Single, _
                        ByVal TLAPS As Single, _
                        ByVal SNO_SUB As Single, _
                        ByVal CH_L1 As Single, _
                        ByVal CH_S1 As Single, _
                        ByVal CH_W1 As Single, _
                        ByVal CH_K1 As Single, _
                        ByVal CH_N1 As Single, _
                        ByVal CO2 As Single, _
                        ByVal RFINC1 As Single, _
                        ByVal RFINC2 As Single, _
                        ByVal RFINC3 As Single, _
                        ByVal RFINC4 As Single, _
                        ByVal RFINC5 As Single, _
                        ByVal RFINC6 As Single, _
                        ByVal RFINC7 As Single, _
                        ByVal RFINC8 As Single, _
                        ByVal RFINC9 As Single, _
                        ByVal RFINC10 As Single, _
                        ByVal RFINC11 As Single, _
                        ByVal RFINC12 As Single, _
                        ByVal TMPINC1 As Single, _
                        ByVal TMPINC2 As Single, _
                        ByVal TMPINC3 As Single, _
                        ByVal TMPINC4 As Single, _
                        ByVal TMPINC5 As Single, _
                        ByVal TMPINC6 As Single, _
                        ByVal TMPINC7 As Single, _
                        ByVal TMPINC8 As Single, _
                        ByVal TMPINC9 As Single, _
                        ByVal TMPINC10 As Single, _
                        ByVal TMPINC11 As Single, _
                        ByVal TMPINC12 As Single, _
                        ByVal RADINC1 As Single, _
                        ByVal RADINC2 As Single, _
                        ByVal RADINC3 As Single, _
                        ByVal RADINC4 As Single, _
                        ByVal RADINC5 As Single, _
                        ByVal RADINC6 As Single, _
                        ByVal RADINC7 As Single, _
                        ByVal RADINC8 As Single, _
                        ByVal RADINC9 As Single, _
                        ByVal RADINC10 As Single, _
                        ByVal RADINC11 As Single, _
                        ByVal RADINC12 As Single, _
                        ByVal HUMINC1 As Single, _
                        ByVal HUMINC2 As Single, _
                        ByVal HUMINC3 As Single, _
                        ByVal HUMINC4 As Single, _
                        ByVal HUMINC5 As Single, _
                        ByVal HUMINC6 As Single, _
                        ByVal HUMINC7 As Single, _
                        ByVal HUMINC8 As Single, _
                        ByVal HUMINC9 As Single, _
                        ByVal HUMINC10 As Single, _
                        ByVal HUMINC11 As Single, _
                        ByVal HUMINC12 As Single, _
                        ByVal HRUTOT As Long, _
                        ByVal IPOT As Long, _
                        ByVal FCST_REG As Long, _
                        ByVal COMID As Integer)

            Dim lSQL As String = "INSERT INTO sub ( "
            Dim lFieldNum As Integer = 0
            For Each lFieldName As String In pFieldNames
                For lSameNameFieldIndex As Integer = 1 To pFieldCounts(lFieldNum)
                    Dim lUseName As String = lFieldName
                    If pFieldCounts(lFieldNum) > 1 Then lUseName &= lSameNameFieldIndex
                    lSQL &= lUseName & ", "
                Next
                lFieldNum += 1
            Next
            lSQL &= "COMID ) Values (" _
                 & "'" & SUBBASIN & "' ,'" & SUB_KM & "' ,'" & SUB_LAT & "' ,'" & SUB_ELEV & "' ,'" & IRGAGE & "' ,'" & ITGAGE & "' ,'" & ISGAGE & "' ,'" & IHGAGE & "' ,'" & IWGAGE & "' ,'" _
                 & ELEVB1 & "' ,'" & ELEVB2 & "' ,'" & ELEVB3 & "' ,'" & ELEVB4 & "' ,'" & ELEVB5 & "' ,'" & ELEVB6 & "' ,'" & ELEVB7 & "' ,'" & ELEVB8 & "' ,'" & ELEVB9 & "' ,'" & ELEVB10 & "' ,'" _
                 & ELEVB_FR1 & "' ,'" & ELEVB_FR2 & "' ,'" & ELEVB_FR3 & "' ,'" & ELEVB_FR4 & "' ,'" & ELEVB_FR5 & "' ,'" & ELEVB_FR6 & "' ,'" & ELEVB_FR7 & "' ,'" & ELEVB_FR8 & "' ,'" & ELEVB_FR9 & "' ,'" & ELEVB_FR10 & "' ,'" _
                 & SNOEB1 & "' ,'" & SNOEB2 & "' ,'" & SNOEB3 & "' ,'" & SNOEB4 & "' ,'" & SNOEB5 & "' ,'" & SNOEB6 & "' ,'" & SNOEB7 & "' ,'" & SNOEB8 & "' ,'" & SNOEB9 & "' ,'" & SNOEB10 & "' ,'" _
                 & PLAPS & "' ,'" & TLAPS & "' ,'" & SNO_SUB & "' ,'" & CH_L1 & "' ,'" & CH_S1 & "' ,'" & CH_W1 & "' ,'" & CH_K1 & "' ,'" & CH_N1 & "' ,'" & CO2 & "' ,'" _
                 & RFINC1 & "' ,'" & RFINC2 & "' ,'" & RFINC3 & "' ,'" & RFINC4 & "' ,'" & RFINC5 & "' ,'" & RFINC6 & "' ,'" & RFINC7 & "' ,'" & RFINC8 & "' ,'" & RFINC9 & "' ,'" & RFINC10 & "' ,'" & RFINC11 & "' ,'" & RFINC12 & "' ,'" _
                 & TMPINC1 & "' ,'" & TMPINC2 & "' ,'" & TMPINC3 & "' ,'" & TMPINC4 & "' ,'" & TMPINC5 & "' ,'" & TMPINC6 & "' ,'" & TMPINC7 & "' ,'" & TMPINC8 & "' ,'" & TMPINC9 & "' ,'" & TMPINC10 & "' ,'" & TMPINC11 & "' ,'" & TMPINC12 & "' ,'" _
                 & RADINC1 & "' ,'" & RADINC2 & "' ,'" & RADINC3 & "' ,'" & RADINC4 & "' ,'" & RADINC5 & "' ,'" & RADINC6 & "' ,'" & RADINC7 & "' ,'" & RADINC8 & "' ,'" & RADINC9 & "' ,'" & RADINC10 & "' ,'" & RADINC11 & "' ,'" & RADINC12 & "' ,'" _
                 & HUMINC1 & "' ,'" & HUMINC2 & "' ,'" & HUMINC3 & "' ,'" & HUMINC4 & "' ,'" & HUMINC5 & "' ,'" & HUMINC6 & "' ,'" & HUMINC7 & "' ,'" & HUMINC8 & "' ,'" & HUMINC9 & "' ,'" & HUMINC10 & "' ,'" & HUMINC11 & "' ,'" & HUMINC12 & "' ,'" _
                 & HRUTOT & "' ,'" & IPOT & "' ,'" & FCST_REG & "' ,'" & COMID & "'  )"

            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

        Public Function TableWithArea(ByVal aAggregationFieldName As String) As DataTable
            Dim lAggregationFromTable As DataTable
            Dim lAggregationField As Integer = pSwatInput.Hru.Table.Columns.IndexOf(aAggregationFieldName)
            If lAggregationField > 0 Then
                lAggregationFromTable = pSwatInput.Hru.UniqueValues(aAggregationFieldName)
                Debug.Print("AggregationRowCount " & lAggregationFromTable.Rows.Count)

                Dim lArea As Double
                Dim lTable As New DataTable
                With lTable
                    .Columns.Add("SubBasinId", "".GetType)
                    .Columns.Add("Area", lArea.GetType)
                    For Each lAggregationRow As DataRow In lAggregationFromTable.Rows
                        .Columns.Add(lAggregationRow.Item(0), lArea.GetType)
                    Next
                    'summarize areas by subbasin
                    Dim lAreaTotal As Double = 0.0
                    For Each lSubBasinRow As DataRow In Table().Rows
                        lArea = lSubBasinRow.Item(2)
                        lAreaTotal += lArea
                        Dim lReportRow As DataRow = .NewRow
                        With lReportRow
                            .Item(0) = lSubBasinRow.Item(1)
                            .Item(1) = lArea
                            For lindex As Integer = 0 To lAggregationFromTable.Rows.Count - 1
                                .Item(lindex + 2) = 0.0
                            Next
                        End With
                        .Rows.Add(lReportRow)
                    Next

                    'summarize areas by subbasin/aggrat
                    For Each lHruRow As DataRow In pSwatInput.Hru.Table.Rows
                        Dim lSubBasin As Integer = lHruRow.Item(1)
                        Dim lLandUse As String = lHruRow.Item(3)
                        Dim lReportRow As DataRow = .Rows(lSubBasin - 1)
                        lReportRow.Item(lHruRow.Item(lAggregationField)) += lHruRow.Item(6) * lReportRow.Item(1)
                    Next
                End With
                Return lTable
            Else 'TODO: add exception here?
                Return Nothing
            End If
        End Function

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & " ORDER BY SUBBASIN;")
        End Function

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing " & pTableName & " text ...")

            Dim lField As Integer
            For Each lRow As DataRow In aTable.Rows
                Dim lSB As New System.Text.StringBuilder
                Dim lSubBasin As String = lRow.Item("SUBBASIN")
                Dim lFileName As String = StringFname(lSubBasin, pTableName)
                '1st line
                lSB.AppendLine(" ." & pTableName & " file Subbasin: " & lSubBasin & " " & DateNowString() & " AVSWAT2003 -SWAT INTERFACE MAVZ")
                '---2. SUB_KM
                lSB.AppendLine(Format(lRow.Item(2), "0.000000").PadLeft(16) & Strings.StrDup(4, " ") & "| SUB_KM : Subbasin area [km2]")

                lSB.AppendLine("")
                lSB.AppendLine("Climate in subbasin")

                '---3. LATITUDE
                lSB.AppendLine(Format(lRow.Item(3), "0.000000").PadLeft(16) & "    | LATITUDE : Latitude of subbasin [degrees]")
                '---4. ELEV
                lSB.AppendLine(Format(lRow.Item(4), "0.00").PadLeft(16) & "    | ELEV : Elevation of subbasin [m]")
                '---5. IRGAGE
                lSB.AppendLine(Format(lRow.Item(5), "0").PadLeft(16) & "    | IRGAGE: precip gage data used in subbasin")
                '---6. ITGAGE
                lSB.AppendLine(Format(lRow.Item(6), "0").PadLeft(16) & "    | ITGAGE: temp gage data used in subbasin")
                '---7. ISGAGE
                lSB.AppendLine(Format(lRow.Item(7), "0").PadLeft(16) & "    | ISGAGE: solar radiation gage data used in subbasin")
                '---8. IHGAGE
                lSB.AppendLine(Format(lRow.Item(8), "0").PadLeft(16) & "    | IHGAGE: relative humidity gage data used in subbasin")
                '---9. IWGAGE
                lSB.AppendLine(Format(lRow.Item(9), "0").PadLeft(16) & "    | IWGAGE: wind speed gage data used in subbasin")

                'WGN file name
                lSB.AppendLine(Left(lFileName, 9) & ".wgn" & Strings.StrDup(7, " ") & "| WGNFILE: name of weather generator data file")

                'FCST region number
                lSB.AppendLine(Format(lRow.Item(99), 0).PadLeft(16) & "    | FCST_REG: Region number used to assign forecast data to the subbasin")

                '---10. ELEVB
                lSB.AppendLine("Elevation Bands")
                lSB.AppendLine("| ELEVB: Elevation at center of elevation bands [m]")
                '---6.
                For lField = 10 To 19
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---7. ELEVB_FR
                lSB.AppendLine("| ELEVB_FR: Fraction of subbasin area within elevation band")
                '---8.
                For lField = 20 To 29
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---9. SNOEB
                lSB.AppendLine("| SNOEB: Initial snow water content in elevation band [mm]")
                '---10.
                For lField = 30 To 39
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---11. PLAPS
                lSB.AppendLine(Format(lRow.Item(40), "0.000").PadLeft(16) & "    | PLAPS : Precipitation lapse rate [mm/km]")
                '---12. TLAPS
                lSB.AppendLine(Format(lRow.Item(41), "0.000").PadLeft(16) & "    | TLAPS : Temperature lapse rate [°C/km]")
                '---13. SNO_SUB
                lSB.AppendLine(Format(lRow.Item(42), "0.000").PadLeft(16) & "    | SNO_SUB : Initial snow water content [mm]")

                '---14. CH_L1
                lSB.AppendLine("Tributary Channels")
                lSB.AppendLine(Format(lRow.Item(43), "0.000").PadLeft(16) & "    | CH_L1 : Longest tributary channel length [km]")
                '---15. CH_S1
                lSB.AppendLine(Format(lRow.Item(44), "0.000").PadLeft(16) & "    | CH_S1 : Average slope of tributary channel [m/m]")
                '---16. CH_W1
                lSB.AppendLine(Format(lRow.Item(45), "0.000").PadLeft(16) & "    | CH_W1 : Average width of tributary channel [mm/km]")
                '---17. CH_K1
                lSB.AppendLine(Format(lRow.Item(46), "0.000").PadLeft(16) & "    | CH_K1 : Effective hydraulic conductivity in tributary channel [mm/hr]")
                '---18. CH_N1
                lSB.AppendLine(Format(lRow.Item(47), "0.000").PadLeft(16) & "    | CH_N1 : Manning's ""n"" value for the tributary channels")

                ' Impoundments
                lSB.AppendLine("Impoundments")
                lSB.AppendLine(Left(lFileName, 9) & ".pnd" & Strings.StrDup(7, " ") & "| PNDFILE: name of subbasin impoundment file")

                ' Water Use
                lSB.AppendLine("Consumptive Water Use")
                lSB.AppendLine(Left(lFileName, 9) & ".wus" & Strings.StrDup(7, " ") & "| WUSFILE: name of subbasin water use file")

                '---19. CO2
                lSB.AppendLine("Climate Change")
                lSB.AppendLine(Format(lRow.Item(48), "0.000").PadLeft(16) & "    | CO2 : Carbon dioxide concentration [ppmv]")
                '---20. RFINC
                lSB.AppendLine("| RFINC:  Climate change monthly rainfall adjustment (January - June)")
                '---21.
                For lField = 49 To 54
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---22.
                lSB.AppendLine("| RFINC:  Climate change monthly rainfall adjustment (July - December)")
                '---23.
                For lField = 55 To 60
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---24. TMPINC
                lSB.AppendLine("| TMPINC: Climate change monthly temperature adjustment (January - June)")
                '---25.
                For lField = 61 To 66
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---26.
                lSB.AppendLine("| TMPINC: Climate change monthly temperature adjustment (July - December)")
                '---27.
                For lField = 67 To 72
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---28. RADINC
                lSB.AppendLine("| RADINC: Climate change monthly radiation adjustment (January - June)")
                '---29.
                For lField = 73 To 78
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---30.
                lSB.AppendLine("| RADINC: Climate change monthly radiation adjustment (July - December)")
                '---31.
                For lField = 79 To 84
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---32. HUMINC
                lSB.AppendLine("| HUMINC: Climate change monthly humidity adjustment (January - June)")
                '---33.
                For lField = 85 To 90
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---34.
                lSB.AppendLine("| HUMINC: Climate change monthly humidity adjustment (July - December)")
                '---35.
                For lField = 91 To 96
                    lSB.Append(Format(lRow.Item(lField), "0.000").PadLeft(8))
                Next
                lSB.AppendLine()
                '---36. HRU data files
                lSB.AppendLine("| HRU data")

                'MFW print the HRUTOT value
                lSB.AppendLine(Format(lRow.Item(97), "0").PadLeft(16) & "    | HRUTOT : Total number of HRUs modeled in subbasin")

                lSB.AppendLine("")
                lSB.AppendLine("HRU: Depressional Storage/Pothole")
                If lRow.Item(98) = 0 Then
                    lSB.AppendLine("")
                Else
                    Dim lHruName As String = StringFnameHRUs(lSubBasin, lRow.Item(98))
                    lSB.AppendLine(lHruName & ".hru" & lHruName & ".mgt" & lHruName & ".sol" & lHruName & ".chm" & lHruName & ".gw")
                End If
                lSB.AppendLine("Floodplain")
                lSB.AppendLine("")
                lSB.AppendLine("HRU: Riparian")
                lSB.AppendLine("")
                lSB.AppendLine("HRU: General")

                '---37.
                For lField = 1 To lRow.Item(97)
                    Dim lHruName As String = StringFnameHRUs(lSubBasin, lField.ToString)
                    lSB.AppendLine(lHruName & ".hru" & lHruName & ".mgt" & lHruName & ".sol" & lHruName & ".chm" & lHruName & ".gw")
                Next
                IO.File.WriteAllText(pSwatInput.OutputFolder & "\" & lFileName, lSB.ToString)
            Next
        End Sub
    End Class
End Class
