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

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn:DBLayer:createHruTable
            Try
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

                Dim i As Integer

                With lTable.Columns
                    .Append(lKeyColumn)
                    .Append("SUBBASIN", ADOX.DataTypeEnum.adDouble)
                    .Append("SUB_KM", ADOX.DataTypeEnum.adDouble)
                    .Append("SUB_LAT", ADOX.DataTypeEnum.adDouble)
                    .Append("SUB_ELEV", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IRGAGE", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("ITGAGE", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("ISGAGE", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IHGAGE", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IWGAGE", ADOX.DataTypeEnum.adDouble)
                    For i = 1 To 10
                        .Append("ELEVB" & i, ADOX.DataTypeEnum.adSingle)
                    Next
                    For i = 1 To 10
                        .Append("ELEVB_FR" & i, ADOX.DataTypeEnum.adSingle)
                    Next
                    For i = 1 To 10
                        .Append("SNOEB" & i, ADOX.DataTypeEnum.adSingle)
                    Next
                    .Append("PLAPS", ADOX.DataTypeEnum.adSingle)
                    .Append("TLAPS", ADOX.DataTypeEnum.adSingle)
                    .Append("SNO_SUB", ADOX.DataTypeEnum.adSingle)
                    .Append("CH_L1", ADOX.DataTypeEnum.adSingle)
                    .Append("CH_S1", ADOX.DataTypeEnum.adSingle)
                    .Append("CH_W1", ADOX.DataTypeEnum.adSingle)
                    .Append("CH_K1", ADOX.DataTypeEnum.adSingle)
                    .Append("CH_N1", ADOX.DataTypeEnum.adSingle)
                    .Append("CO2", ADOX.DataTypeEnum.adSingle)
                    For i = 1 To 12
                        .Append("RFINC" & i, ADOX.DataTypeEnum.adSingle)
                    Next
                    For i = 1 To 12
                        .Append("TMPINC" & i, ADOX.DataTypeEnum.adSingle)
                    Next
                    For i = 1 To 12
                        .Append("RADINC" & i, ADOX.DataTypeEnum.adSingle)
                    Next
                    For i = 1 To 12
                        .Append("HUMINC" & i, ADOX.DataTypeEnum.adSingle)
                    Next
                    .Append("HRUTOT", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IPOT", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("FCST_REG", ADOX.DataTypeEnum.adInteger, 4)
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
