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
        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
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
            pSwatInput.Status("Reading SUB tables from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM sub ORDER BY SUBBASIN;")
        End Function
        Public Sub Save()
            Dim lSubTable As DataTable = Table()
            pSwatInput.Status("Writing SUB tables ...")

            Dim lLine As String
            For Each lRow As DataRow In lSubTable.Rows
                Dim lSB As New System.Text.StringBuilder
                Dim lSubNum As String = lRow.Item("SUBBASIN")
                Dim lSubName As String = StringFnameSubBasins(lSubNum) & ".sub"
                '1st line
                lSB.AppendLine(" .sub file Subbasin: " + lSubNum + " " + Date.Now.ToString + " AVSWAT2003 -SWAT INTERFACE MAVZ")
                '---2. SUB_KM
                lSB.AppendLine(Format(lRow.Item(2), "0.000000").PadLeft(16) + Strings.StrDup(4, " ") + "| SUB_KM : Subbasin area [km2]")

                lSB.AppendLine("")
                lSB.AppendLine("Climate in subbasin")

                '---3. LATITUDE
                lSB.AppendLine(Format(lRow.Item(3), "0.000000").PadLeft(16) + Strings.StrDup(4, " ") + "| LATITUDE : Latitude of subbasin [degrees]")
                '---4. ELEV
                lSB.AppendLine(Format(lRow.Item(4), "0.00").PadLeft(16) + Strings.StrDup(4, " ") + "| ELEV : Elevation of subbasin [m]")
                '---5. IRGAGE
                lSB.AppendLine(Format(lRow.Item(5), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| IRGAGE: precip gage data used in subbasin")
                '---6. ITGAGE
                lSB.AppendLine(Format(lRow.Item(6), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| ITGAGE: temp gage data used in subbasin")
                '---7. ISGAGE
                lSB.AppendLine(Format(lRow.Item(7), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| ISGAGE: solar radiation gage data used in subbasin")
                '---8. IHGAGE
                lSB.AppendLine(Format(lRow.Item(8), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| IHGAGE: relative humidity gage data used in subbasin")
                '---9. IWGAGE
                lSB.AppendLine(Format(lRow.Item(9), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| IWGAGE: wind speed gage data used in subbasin")

                'WGN file name
                lSB.AppendLine(Left(lSubName, 9) & ".wgn" + Strings.StrDup(7, " ") + "| WGNFILE: name of weather generator data file")

                'FCST region number
                lSB.AppendLine(Format(lRow.Item(99), 0).PadLeft(16) + Strings.StrDup(4, " ") + "| FCST_REG: Region number used to assign forecast data to the subbasin")

                '---10. ELEVB
                lSB.AppendLine("Elevation Bands")
                lSB.AppendLine("| ELEVB: Elevation at center of elevation bands [m]")
                '---6.
                lLine = ""
                For i As Integer = 10 To 19
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---7. ELEVB_FR
                lSB.AppendLine("| ELEVB_FR: Fraction of subbasin area within elevation band")
                '---8.
                lLine = ""
                For i As Integer = 20 To 29
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---9. SNOEB
                lSB.AppendLine("| SNOEB: Initial snow water content in elevation band [mm]")
                '---10.
                lLine = ""
                For i As Integer = 30 To 39
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---11. PLAPS
                lSB.AppendLine(Format(lRow.Item(40), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| PLAPS : Precipitation lapse rate [mm/km]")
                '---12. TLAPS
                lSB.AppendLine(Format(lRow.Item(41), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| TLAPS : Temperature lapse rate [°C/km]")
                '---13. SNO_SUB
                lSB.AppendLine(Format(lRow.Item(42), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| SNO_SUB : Initial snow water content [mm]")

                '---14. CH_L1
                lSB.AppendLine("Tributary Channels")
                lLine = Format(lRow.Item(43), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| CH_L1 : Longest tributary channel length [km]"
                lSB.AppendLine(lLine)
                '---15. CH_S1
                lSB.AppendLine(Format(lRow.Item(44), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| CH_S1 : Average slope of tributary channel [m/m]")
                '---16. CH_W1
                lSB.AppendLine(Format(lRow.Item(45), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| CH_W1 : Average width of tributary channel [mm/km]")
                '---17. CH_K1
                lSB.AppendLine(Format(lRow.Item(46), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| CH_K1 : Effective hydraulic conductivity in tributary channel [mm/hr]")
                '---18. CH_N1
                lSB.AppendLine(Format(lRow.Item(47), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| CH_N1 : Manning's ""n"" value for the tributary channels")

                ' Impoundments
                lSB.AppendLine("Impoundments")
                lSB.AppendLine(Left(lSubName, 9) & ".pnd" + Strings.StrDup(7, " ") + "| PNDFILE: name of subbasin impoundment file")

                ' Water Use
                lSB.AppendLine("Consumptive Water Use")
                lSB.AppendLine(Left(lSubName, 9) & ".wus" + Strings.StrDup(7, " ") + "| WUSFILE: name of subbasin water use file")

                '---19. CO2
                lSB.AppendLine("Climate Change")
                lSB.AppendLine(Format(lRow.Item(48), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| CO2 : Carbon dioxide concentration [ppmv]")
                '---20. RFINC
                lSB.AppendLine("| RFINC:  Climate change monthly rainfall adjustment (January - June)")
                '---21.
                lLine = ""
                For i As Integer = 49 To 54
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---22.
                lSB.AppendLine("| RFINC:  Climate change monthly rainfall adjustment (July - December)")
                '---23.
                lLine = ""
                For i As Integer = 55 To 60
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---24. TMPINC
                lSB.AppendLine("| TMPINC: Climate change monthly temperature adjustment (January - June)")
                '---25.
                lLine = ""
                For i As Integer = 61 To 66
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---26.
                lSB.AppendLine("| TMPINC: Climate change monthly temperature adjustment (July - December)")
                '---27.
                lLine = ""
                For i As Integer = 67 To 72
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---28. RADINC
                lSB.AppendLine("| RADINC: Climate change monthly radiation adjustment (January - June)")
                '---29.
                lLine = ""
                For i As Integer = 73 To 78
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---30.
                lSB.AppendLine("| RADINC: Climate change monthly radiation adjustment (July - December)")
                '---31.
                lLine = ""
                For i As Integer = 79 To 84
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---32. HUMINC
                lSB.AppendLine("| HUMINC: Climate change monthly humidity adjustment (January - June)")
                '---33.
                lLine = ""
                For i As Integer = 85 To 90
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---34.
                lSB.AppendLine("| HUMINC: Climate change monthly humidity adjustment (July - December)")
                '---35.
                lLine = ""
                For i As Integer = 91 To 96
                    lLine = lLine + Format(lRow.Item(i), "0.000").PadLeft(8)
                Next i
                lSB.AppendLine(lLine)
                '---36. HRU data files
                lSB.AppendLine("| HRU data")

                'MFW print the HRUTOT value
                lSB.AppendLine(Format(lRow.Item(97), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| HRUTOT : Total number of HRUs modeled in subbasin")

                lSB.AppendLine("")
                lSB.AppendLine("HRU: Depressional Storage/Pothole")
                If lRow.Item(98) = 0 Then
                    lSB.AppendLine("")
                Else
                    Dim lHruName As String = StringFnameHRUs(lSubNum, lRow.Item(98))
                    lLine = lHruName + ".hru" + lHruName + ".mgt" + lHruName + ".sol" + lHruName + ".chm" + lHruName + ".gw"
                    lSB.AppendLine(lLine)
                End If
                lSB.AppendLine("Floodplain")
                lSB.AppendLine("")
                lSB.AppendLine("HRU: Riparian")
                lSB.AppendLine("")
                lSB.AppendLine("HRU: General")

                '---37.
                For i As Integer = 1 To lRow.Item(97)
                    Dim lHruName As String = StringFnameHRUs(lSubNum, i.ToString)
                    lSB.AppendLine(lHruName + ".hru" + lHruName + ".mgt" + lHruName + ".sol" + lHruName + ".chm" + lHruName + ".gw")
                Next
                IO.File.WriteAllText(pSwatInput.OutputFolder & "\" & lSubName, lSB.ToString)
            Next
        End Sub
    End Class
End Class
