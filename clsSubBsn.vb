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

    Public Class clsSubBsnItem
        Friend Shared KEYFIELD As String = "OID"
        Public OID As Integer
        Public SUBBASIN As Double
        Public SUB_KM As Double
        Public SUB_LAT As Double
        Public SUB_ELEV As Double
        Public IRGAGE As Long
        Public ITGAGE As Long
        Public ISGAGE As Long
        Public IHGAGE As Long
        Public IWGAGE As Long
        Public ELEVB(9) As Single
        Public ELEVB_FR(9) As Single
        Public SNOEB(9) As Single
        Public PLAPS As Single
        Public TLAPS As Single
        Public SNO_SUB As Single
        Public CH_L1 As Single
        Public CH_S1 As Single
        Public CH_W1 As Single
        Public CH_K1 As Single
        Public CH_N1 As Single
        Public CO2 As Single
        Public RFINC(11) As Single
        Public TMPINC(11) As Single
        Public RADINC(11) As Single
        Public HUMINC(11) As Single
        Public HRUTOT As Long
        Public IPOT As Long
        Public FCST_REG As Long
        Public COMID As Integer

        Public Sub New(ByVal aSubBasin As Double)
            SUBBASIN = aSubBasin
        End Sub

        Public Sub New(ByVal aRow As DataRow)
            OID = aRow.Item("OID")
            SUBBASIN = aRow.Item("SUBBASIN")
            SUB_KM = aRow.Item("SUB_KM")
            SUB_LAT = aRow.Item("SUB_LAT")
            SUB_ELEV = aRow.Item("SUB_ELEV")
            IRGAGE = aRow.Item("IRGAGE")
            ITGAGE = aRow.Item("ITGAGE")
            ISGAGE = aRow.Item("ISGAGE")
            IHGAGE = aRow.Item("IHGAGE")
            IWGAGE = aRow.Item("IWGAGE")
            PLAPS = aRow.Item("PLAPS")
            TLAPS = aRow.Item("TLAPS")
            SNO_SUB = aRow.Item("SNO_SUB")
            CH_L1 = aRow.Item("CH_L1")
            CH_S1 = aRow.Item("CH_S1")
            CH_W1 = aRow.Item("CH_W1")
            CH_K1 = aRow.Item("CH_K1")
            CH_N1 = aRow.Item("CH_N1")
            CO2 = aRow.Item("CO2")
            HRUTOT = aRow.Item("HRUTOT")
            IPOT = aRow.Item("IPOT")
            FCST_REG = aRow.Item("FCST_REG")

            For i As Integer = 0 To 9
                ELEVB(i) = aRow.Item("ELEVB" & i + 1)
                ELEVB_FR(i) = aRow.Item("ELEVB_FR" & i + 1)
                SNOEB(i) = aRow.Item("SNOEB" & i + 1)
            Next
            For i As Integer = 0 To 11
                RFINC(i) = aRow.Item("RFINC" & i + 1)
                TMPINC(i) = aRow.Item("TMPINC" & i + 1)
                RADINC(i) = aRow.Item("RADINC" & i + 1)
                HUMINC(i) = aRow.Item("HUMINC" & i + 1)
            Next
        End Sub

        Public Function AddSQL() As String
            Dim lSQL As String = "INSERT INTO sub ( "
            Dim lFieldNum As Integer = 0
            For Each lFieldName As String In clsSubBsn.FieldNames
                For lSameNameFieldIndex As Integer = 1 To clsSubBsn.FieldCounts(lFieldNum)
                    Dim lUseName As String = lFieldName
                    If clsSubBsn.FieldCounts(lFieldNum) > 1 Then lUseName &= lSameNameFieldIndex
                    lSQL &= lUseName & ", "
                Next
                lFieldNum += 1
            Next
            lSQL &= "COMID ) Values (" _
                 & "'" & SUBBASIN & "' ,'" & SUB_KM & "' ,'" & SUB_LAT & "' ,'" & SUB_ELEV & "' ,'" & IRGAGE & "' ,'" & ITGAGE & "' ,'" & ISGAGE & "' ,'" & IHGAGE & "' ,'" & IWGAGE & "' ,'" _
                 & ArrayToString(ELEVB, "', '") & "', '" _
                 & ArrayToString(ELEVB_FR, "', '") & "', '" _
                 & ArrayToString(SNOEB, "', '") & "', '" _
                 & PLAPS & "' ,'" & TLAPS & "' ,'" & SNO_SUB & "' ,'" & CH_L1 & "' ,'" & CH_S1 & "' ,'" & CH_W1 & "' ,'" & CH_K1 & "' ,'" & CH_N1 & "' ,'" & CO2 & "' ,'" _
                 & ArrayToString(RFINC, "', '") & "', '" _
                 & ArrayToString(TMPINC, "', '") & "', '" _
                 & ArrayToString(RADINC, "', '") & "', '" _
                 & ArrayToString(HUMINC, "', '") & "', '" _
                 & HRUTOT & "' ,'" & IPOT & "' ,'" & FCST_REG & "' ,'" & COMID & "'  )"
            Return lSQL
        End Function

        Public Function UpdateSQL() As String
            Return SqlAddToUpdate(AddSQL, KEYFIELD & " = " & OID)
        End Function
    End Class

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

        Friend Shared FieldNames() As String = {"SUBBASIN", "SUB_KM", "SUB_LAT", "SUB_ELEV", "IRGAGE", "ITGAGE", "ISGAGE", "IHGAGE", "IWGAGE", _
                                                   "ELEVB", _
                                                   "ELEVB_FR", _
                                                   "SNOEB", _
                                                   "PLAPS", "TLAPS", "SNO_SUB", "CH_L1", "CH_S1", "CH_W1", "CH_K1", "CH_N1", "CO2", _
                                                   "RFINC", _
                                                   "TMPINC", _
                                                   "RADINC", _
                                                   "HUMINC", _
                                                   "HRUTOT", "IPOT", "FCST_REG"}

        Friend Shared FieldCounts() As Integer = {1, 1, 1, 1, 1, 1, 1, 1, 1, _
                                                   10, _
                                                   10, _
                                                   10, _
                                                   1, 1, 1, 1, 1, 1, 1, 1, 1, _
                                                   12, _
                                                   12, _
                                                   12, _
                                                   12, _
                                                   1, 1, 1}

        Friend Shared FieldTypes() As ADOX.DataTypeEnum = {ADOX.DataTypeEnum.adDouble, ADOX.DataTypeEnum.adDouble, ADOX.DataTypeEnum.adDouble, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adDouble, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adSingle, _
                                   ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger, ADOX.DataTypeEnum.adInteger}

        Public Function Items() As IEnumerable
            Dim lTable As DataTable = Table()
            Dim lItems As New Generic.List(Of clsSubBsnItem)
            For Each lRow As DataRow In lTable.Rows
                lItems.Add(New clsSubBsnItem(lRow))
            Next
            Return lItems
        End Function

        Public Function Item(ByVal aSubBasinId As Integer) As clsSubBsnItem
            Return Item(aSubBasinId, Nothing)
        End Function
        Public Function Item(ByVal aSubBasinId As Integer, ByVal aTable As DataTable) As clsSubBsnItem
            If aTable Is Nothing Then aTable = Table()
            Return New clsSubBsnItem(aTable.Rows(aSubBasinId))
        End Function

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn:DBLayer:createHruTable
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
                    Dim lFieldNum As Integer = 0
                    For Each lFieldName As String In FieldNames
                        For lSameNameFieldIndex As Integer = 1 To FieldCounts(lFieldNum)
                            Dim lUseName As String = lFieldName
                            If FieldCounts(lFieldNum) > 1 Then lUseName &= lSameNameFieldIndex
                            If FieldTypes(lFieldNum) = ADOX.DataTypeEnum.adInteger Then
                                .Append(lUseName, FieldTypes(lFieldNum), 4)
                            Else
                                .Append(lUseName, FieldTypes(lFieldNum))
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

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & " ORDER BY SUBBASIN;")
        End Function

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

        Public Sub Add(ByVal aItem As clsSubBsnItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatInput)
        End Sub

        Public Sub Update(ByVal aItem As clsSubBsnItem)
            ExecuteNonQuery(aItem.UpdateSQL, pSwatInput.CnSwatInput)
        End Sub

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing " & pTableName & " text ...")

            Dim lField As Integer
            For Each lRow As DataRow In aTable.Rows
                Dim lSB As New System.Text.StringBuilder
                Dim lSubBasin As String = lRow.Item("SUBBASIN")
                Dim lFileName As String = StringFname(lSubBasin, pTableName)
                '1st line
                lSB.AppendLine(" ." & pTableName & " file Subbasin: " & lSubBasin & " " & HeaderString())
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
                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & lFileName, lSB.ToString)
            Next
        End Sub
    End Class
End Class
