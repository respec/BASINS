Partial Class SwatInput
    Private pWgn As clsWgn = New clsWgn(Me)
    ReadOnly Property Wgn() As clsWgn
        Get
            Return pWgn
        End Get
    End Property

    Public Class clsWgnItem
        Public SUBBASIN As Integer
        Public STATION As String
        Public WLATITUDE As Single
        Public WLONGITUDE As Single
        Public WELEV As Single
        Public RAIN_YRS As Single
        Public TMPMX(11) As Single
        Public TMPMN(11) As Single
        Public TMPSTDMX(11) As Single
        Public TMPSTDMN(11) As Single
        Public PCPMM(11) As Single
        Public PCPSTD(11) As Single
        Public PCPSKW(11) As Single
        Public PR_W1_(11) As Single
        Public PR_W2_(11) As Single
        Public PCPD(11) As Single
        Public RAINHHMX(11) As Single
        Public SOLARAV(11) As Single
        Public DEWPT(11) As Single
        Public WNDAV(11) As Single

        Public Sub New(ByVal aSUBBASIN As Integer)
            SUBBASIN = aSUBBASIN
        End Sub

        Public Function AddSQL() As String
            Return "INSERT INTO wgn( SUBBASIN, STATION, WLATITUDE, WLONGITUDE, WELEV, RAIN_YRS, " _
                    & GroupOfStrings("TMPMX#", 12, ", ") & ", " _
                    & GroupOfStrings("TMPMN#", 12, ", ") & ", " _
                    & GroupOfStrings("TMPSTDMX#", 12, ", ") & ", " _
                    & GroupOfStrings("TMPSTDMN#", 12, ", ") & ", " _
                    & GroupOfStrings("PCPMM#", 12, ", ") & ", " _
                    & GroupOfStrings("PCPSTD#", 12, ", ") & ", " _
                    & GroupOfStrings("PCPSKW#", 12, ", ") & ", " _
                    & GroupOfStrings("PR_W1_#", 12, ", ") & ", " _
                    & GroupOfStrings("PR_W2_#", 12, ", ") & ", " _
                    & GroupOfStrings("PCPD#", 12, ", ") & ", " _
                    & GroupOfStrings("RAINHHMX#", 12, ", ") & ", " _
                    & GroupOfStrings("SOLARAV#", 12, ", ") & ", " _
                    & GroupOfStrings("DEWPT#", 12, ", ") & ", " _
                    & GroupOfStrings("WNDAV#", 12, ", ") _
                    & " ) Values ('" & SUBBASIN & "', '" & STATION & "', '" & WLATITUDE & "', '" & WLONGITUDE & "', '" & WELEV & "', '" & RAIN_YRS & "', '" _
                    & ArrayToString(TMPMX, "', '") & "', '" _
                    & ArrayToString(TMPMN, "', '") & "', '" _
                    & ArrayToString(TMPSTDMX, "', '") & "', '" _
                    & ArrayToString(TMPSTDMN, "', '") & "', '" _
                    & ArrayToString(PCPMM, "', '") & "', '" _
                    & ArrayToString(PCPSTD, "', '") & "', '" _
                    & ArrayToString(PCPSKW, "', '") & "', '" _
                    & ArrayToString(PR_W1_, "', '") & "', '" _
                    & ArrayToString(PR_W2_, "', '") & "', '" _
                    & ArrayToString(PCPD, "', '") & "', '" _
                    & ArrayToString(RAINHHMX, "', '") & "', '" _
                    & ArrayToString(SOLARAV, "', '") & "', '" _
                    & ArrayToString(DEWPT, "', '") & "', '" _
                    & ArrayToString(WNDAV, "', '") & "' )"
        End Function
    End Class

    ''' <summary>
    ''' WGN Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsWgn
        Private pSwatInput As SwatInput
        Private pTableName As String = "wgn"

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn:DBLayer:createRTE
            Try
                DropTable(pTableName, pSwatInput.CnSwatInput)

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
                    .Append("SUBBASIN", ADOX.DataTypeEnum.adInteger)
                    .Append("STATION", ADOX.DataTypeEnum.adVarWChar, 80)
                    .Append("WLATITUDE", ADOX.DataTypeEnum.adSingle)
                    .Append("WLONGITUDE", ADOX.DataTypeEnum.adSingle)
                    .Append("WELEV", ADOX.DataTypeEnum.adSingle)
                    .Append("RAIN_YRS", ADOX.DataTypeEnum.adSingle)

                    Append12DBColumnsSingle(lTable.Columns, "TMPMX")
                    Append12DBColumnsSingle(lTable.Columns, "TMPMN")
                    Append12DBColumnsSingle(lTable.Columns, "TMPSTDMX")
                    Append12DBColumnsSingle(lTable.Columns, "TMPSTDMN")
                    Append12DBColumnsSingle(lTable.Columns, "PCPMM")
                    Append12DBColumnsSingle(lTable.Columns, "PCPSTD")
                    Append12DBColumnsSingle(lTable.Columns, "PCPSKW")
                    Append12DBColumnsSingle(lTable.Columns, "PR_W1_")
                    Append12DBColumnsSingle(lTable.Columns, "PR_W2_")
                    Append12DBColumnsSingle(lTable.Columns, "PCPD")
                    Append12DBColumnsSingle(lTable.Columns, "RAINHHMX")
                    Append12DBColumnsSingle(lTable.Columns, "SOLARAV")
                    Append12DBColumnsSingle(lTable.Columns, "DEWPT")
                    Append12DBColumnsSingle(lTable.Columns, "WNDAV")
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

        Private Sub Append12DBColumnsSingle(ByVal aColumns As ADOX.Columns, ByVal aSection As String)
            For i As Integer = 1 To 12
                aColumns.Append(aSection & i, ADOX.DataTypeEnum.adSingle)
            Next
        End Sub

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Sub Add(ByVal aItem As clsWgnItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatInput)
        End Sub

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing " & pTableName & " text ...")

            For Each lRow As DataRow In aTable.Rows
                Dim lSubBasin As String = lRow.Item("SUBBASIN")

                Dim lSB As New Text.StringBuilder
                lSB.AppendLine(" .Wgn file Subbasin: " & lSubBasin _
                             & " STATION NAME:" & lRow.Item(("STATION")) & " " _
                             & HeaderString())
                lSB.AppendLine("  LATITUDE =" & Format(lRow.Item(("WLATITUDE")), "0.00").PadLeft(7) _
                             & " LONGITUDE =" & Format(lRow.Item(("WLONGITUDE")), "0.00").PadLeft(7))
                lSB.AppendLine("  ELEV [m] =" & Format(lRow.Item(("WELEV")), "0.00").PadLeft(7))
                lSB.AppendLine("  RAIN_YRS =" & Format(lRow.Item(("RAIN_YRS")), "0.00").PadLeft(7))
                Append12TextColumns(lSB, lRow, "TMPMX")
                Append12TextColumns(lSB, lRow, "TMPMN")
                Append12TextColumns(lSB, lRow, "TMPSTDMX")
                Append12TextColumns(lSB, lRow, "TMPSTDMN")
                Append12TextColumns(lSB, lRow, "PCPMM")
                Append12TextColumns(lSB, lRow, "PCPSTD")
                Append12TextColumns(lSB, lRow, "PCPSKW")
                Append12TextColumns(lSB, lRow, "PR_W1_")
                Append12TextColumns(lSB, lRow, "PR_W2_")
                Append12TextColumns(lSB, lRow, "PCPD")
                Append12TextColumns(lSB, lRow, "RAINHHMX")
                Append12TextColumns(lSB, lRow, "SOLARAV")
                Append12TextColumns(lSB, lRow, "DEWPT")
                Append12TextColumns(lSB, lRow, "WNDAV")

                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & StringFname(lSubBasin, pTableName), lSB.ToString)
            Next
        End Sub

        Private Sub Append12TextColumns(ByVal aSB As Text.StringBuilder, ByVal aRow As DataRow, ByVal aSection As String)
            For i As Integer = 1 To 12
                aSB.Append(Format(aRow.Item(aSection & i), "0.00").PadLeft(6))
            Next
            aSB.AppendLine()
        End Sub

        Public Sub createWeatherGenData()
            'ONLY FOR US DATABASES, NOT FOR FOREIGN DATABASE, IF SOMEONE WANTS TO
            'HAVE THEIR OWN DATABASE, THEY MAY HAVE TO CREATE THEIR OWN WEATHER 
            'DATABASE AND REPLACE THAT WITH US WEATHER DATABASE.
            'Open the US Weather database dbf file
            Dim i As Integer
            Dim ConnectionString As String
            Dim conParameterDatabase As String
            Dim ds As New DataSet
            Dim da As OleDb.OleDbDataAdapter
            Dim sql As String
            Dim qryString As String = ""
            Dim rstStationName As New DataSet
            Dim rstStationNameCounter As Integer
            Dim stationID As Integer = 0
            Dim subbasinID As Integer = 0
            Dim stationName As String = ""


            If IsTableExist("wgn", conStrProject) Then
                dropTable("wgn", conStrProject)
            End If
            createWgn(conStrProject)

            '1.) US Database

            'Get the required data from weather database with the aid of subWgn table
            ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
            "Data Source= " & System.IO.Path.GetDirectoryName(SaveConFig.S2005ParamDBPath) & "\Weather" & ";Extended Properties=dBase IV"

            'SWAT2005 parameter database connections string
            conParameterDatabase = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" & SaveConFig.S2005ParamDBPath


            Dim dBaseConnection As New System.Data.OleDb.OleDbConnection(ConnectionString)
            dBaseConnection.Open()


            qryString = "SELECT  SubWgn.Subbasin, SubWgn.Station,SubWgn.MinRec  FROM(SubWgn) ORDER BY SubWgn.Subbasin;"
            rstStationName = FetchData(conStrProject, qryString)
            rstStationNameCounter = rstStationName.Tables(0).Rows.Count

            If Not g_USWeatherDatabaseON Then

                For i = 0 To rstStationNameCounter - 1
                    System.Windows.Forms.Application.DoEvents()
                    'Get the station records from access table
                    stationID = rstStationName.Tables(0).Rows(i)("MinRec")
                    subbasinID = rstStationName.Tables(0).Rows(i)("Subbasin")
                    stationName = rstStationName.Tables(0).Rows(i)("Station")
                    sql = "SELECT STATION, WLATITUDE, WLONGITUDE, WELEV, RAIN_YRS, TMPMX1, TMPMX2, TMPMX3, TMPMX4, TMPMX5, TMPMX6, TMPMX7, TMPMX8, TMPMX9, TMPMX10, TMPMX11, TMPMX12, 	TMPMN1, TMPMN2, TMPMN3, TMPMN4, TMPMN5, TMPMN6, TMPMN7,	TMPMN8, TMPMN9, TMPMN10, TMPMN11, TMPMN12, TMPSTDMX1, TMPSTDMX2, TMPSTDMX3, TMPSTDMX4, TMPSTDMX5, TMPSTDMX6, TMPSTDMX7, 	TMPSTDMX8, TMPSTDMX9, TMPSTDMX10, TMPSTDMX11, TMPSTDMX12, TMPSTDMN1, TMPSTDMN2, TMPSTDMN3, TMPSTDMN4, TMPSTDMN5, TMPSTDMN6, TMPSTDMN7, TMPSTDMN8, TMPSTDMN9, TMPSTDMN10, TMPSTDMN11, TMPSTDMN12, PCPMM1, PCPMM2, PCPMM3, PCPMM4, PCPMM5, PCPMM6, PCPMM7, PCPMM8, PCPMM9, PCPMM10, PCPMM11, PCPMM12, PCPSTD1, PCPSTD2, PCPSTD3, PCPSTD4, PCPSTD5, PCPSTD6, PCPSTD7, PCPSTD8, PCPSTD9, PCPSTD10, PCPSTD11, PCPSTD12, PCPSKW1, PCPSKW2, PCPSKW3, PCPSKW4, PCPSKW5, PCPSKW6, PCPSKW7, PCPSKW8, PCPSKW9, PCPSKW10, PCPSKW11, PCPSKW12, PR_W1_1, 	PR_W1_2, PR_W1_3, PR_W1_4, PR_W1_5, PR_W1_6, PR_W1_7, PR_W1_8, PR_W1_9, PR_W1_10, PR_W1_11, PR_W1_12, PR_W2_1, PR_W2_2, PR_W2_3, PR_W2_4, PR_W2_5, PR_W2_6, PR_W2_7, PR_W2_8, PR_W2_9, PR_W2_10, PR_W2_11, PR_W2_12, PCPD1, PCPD2, PCPD3, PCPD4, PCPD5, PCPD6, PCPD7, PCPD8, PCPD9, PCPD10, PCPD11, PCPD12, RAINHHMX1, RAINHHMX2, RAINHHMX3, RAINHHMX4, RAINHHMX5, RAINHHMX6, RAINHHMX7, RAINHHMX8, RAINHHMX9, RAINHHMX10, RAINHHMX11, RAINHHMX12, SOLARAV1, SOLARAV2, SOLARAV3, SOLARAV4, SOLARAV5, SOLARAV6, SOLARAV7, SOLARAV8, SOLARAV9, SOLARAV10, SOLARAV11, SOLARAV12, DEWPT1, DEWPT2, DEWPT3, DEWPT4, DEWPT5, DEWPT6, DEWPT7, DEWPT8, DEWPT9, DEWPT10, DEWPT11, DEWPT12, WNDAV1, WNDAV2, WNDAV3, WNDAV4, WNDAV5, WNDAV6, WNDAV7, WNDAV8, WNDAV9, WNDAV10, WNDAV11, WNDAV12 FROM userwgn WHERE STATION LIKE '" & stationName & "%'"
                    ds = FetchData(conParameterDatabase, sql)

                    For k As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        System.Windows.Forms.Application.DoEvents()
                        AddWgn(conStrProject, subbasinID, _
                                ds.Tables(0).Rows(k)("STATION"), _
                                ds.Tables(0).Rows(k)("WLATITUDE"), _
                                ds.Tables(0).Rows(k)("WLONGITUDE"), _
                                ds.Tables(0).Rows(k)("WELEV"), _
                                ds.Tables(0).Rows(k)("RAIN_YRS"), _
                                ds.Tables(0).Rows(k)("TMPMX1"), _
                                ds.Tables(0).Rows(k)("TMPMX2"), _
                                ds.Tables(0).Rows(k)("TMPMX3"), _
                                ds.Tables(0).Rows(k)("TMPMX4"), _
                                ds.Tables(0).Rows(k)("TMPMX5"), _
                                ds.Tables(0).Rows(k)("TMPMX6"), _
                                ds.Tables(0).Rows(k)("TMPMX7"), _
                                ds.Tables(0).Rows(k)("TMPMX8"), _
                                ds.Tables(0).Rows(k)("TMPMX9"), _
                                ds.Tables(0).Rows(k)("TMPMX10"), _
                                ds.Tables(0).Rows(k)("TMPMX11"), _
                                ds.Tables(0).Rows(k)("TMPMX12"), _
                                ds.Tables(0).Rows(k)("TMPMN1"), _
                                ds.Tables(0).Rows(k)("TMPMN2"), _
                                ds.Tables(0).Rows(k)("TMPMN3"), _
                                ds.Tables(0).Rows(k)("TMPMN4"), _
                                ds.Tables(0).Rows(k)("TMPMN5"), _
                                ds.Tables(0).Rows(k)("TMPMN6"), _
                                ds.Tables(0).Rows(k)("TMPMN7"), _
                                ds.Tables(0).Rows(k)("TMPMN8"), _
                                ds.Tables(0).Rows(k)("TMPMN9"), _
                                ds.Tables(0).Rows(k)("TMPMN10"), _
                                ds.Tables(0).Rows(k)("TMPMN11"), _
                                ds.Tables(0).Rows(k)("TMPMN12"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX1"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX2"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX3"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX4"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX5"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX6"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX7"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX8"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX9"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX10"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX11"), _
                                ds.Tables(0).Rows(k)("TMPSTDMX12"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN1"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN2"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN3"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN4"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN5"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN6"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN7"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN8"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN9"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN10"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN11"), _
                                ds.Tables(0).Rows(k)("TMPSTDMN12"), _
                                ds.Tables(0).Rows(k)("PCPMM1"), _
                                ds.Tables(0).Rows(k)("PCPMM2"), _
                                ds.Tables(0).Rows(k)("PCPMM3"), _
                                ds.Tables(0).Rows(k)("PCPMM4"), _
                                ds.Tables(0).Rows(k)("PCPMM5"), _
                                ds.Tables(0).Rows(k)("PCPMM6"), _
                                ds.Tables(0).Rows(k)("PCPMM7"), _
                                ds.Tables(0).Rows(k)("PCPMM8"), _
                                ds.Tables(0).Rows(k)("PCPMM9"), _
                                ds.Tables(0).Rows(k)("PCPMM10"), _
                                ds.Tables(0).Rows(k)("PCPMM11"), _
                                ds.Tables(0).Rows(k)("PCPMM12"), _
                                ds.Tables(0).Rows(k)("PCPSTD1"), _
                                ds.Tables(0).Rows(k)("PCPSTD2"), _
                                ds.Tables(0).Rows(k)("PCPSTD3"), _
                                ds.Tables(0).Rows(k)("PCPSTD4"), _
                                ds.Tables(0).Rows(k)("PCPSTD5"), _
                                ds.Tables(0).Rows(k)("PCPSTD6"), _
                                ds.Tables(0).Rows(k)("PCPSTD7"), _
                                ds.Tables(0).Rows(k)("PCPSTD8"), _
                                ds.Tables(0).Rows(k)("PCPSTD9"), _
                                ds.Tables(0).Rows(k)("PCPSTD10"), _
                                ds.Tables(0).Rows(k)("PCPSTD11"), _
                                ds.Tables(0).Rows(k)("PCPSTD12"), _
                                ds.Tables(0).Rows(k)("PCPSKW1"), _
                                ds.Tables(0).Rows(k)("PCPSKW2"), _
                                ds.Tables(0).Rows(k)("PCPSKW3"), _
                                ds.Tables(0).Rows(k)("PCPSKW4"), _
                                ds.Tables(0).Rows(k)("PCPSKW5"), _
                                ds.Tables(0).Rows(k)("PCPSKW6"), _
                                ds.Tables(0).Rows(k)("PCPSKW7"), _
                                ds.Tables(0).Rows(k)("PCPSKW8"), _
                                ds.Tables(0).Rows(k)("PCPSKW9"), _
                                ds.Tables(0).Rows(k)("PCPSKW10"), _
                                ds.Tables(0).Rows(k)("PCPSKW11"), _
                                ds.Tables(0).Rows(k)("PCPSKW12"), _
                                ds.Tables(0).Rows(k)("PR_W1_1"), _
                                ds.Tables(0).Rows(k)("PR_W1_2"), _
                                ds.Tables(0).Rows(k)("PR_W1_3"), _
                                ds.Tables(0).Rows(k)("PR_W1_4"), _
                                ds.Tables(0).Rows(k)("PR_W1_5"), _
                                ds.Tables(0).Rows(k)("PR_W1_6"), _
                                ds.Tables(0).Rows(k)("PR_W1_7"), _
                                ds.Tables(0).Rows(k)("PR_W1_8"), _
                                ds.Tables(0).Rows(k)("PR_W1_9"), _
                                ds.Tables(0).Rows(k)("PR_W1_10"), _
                                ds.Tables(0).Rows(k)("PR_W1_11"), _
                                ds.Tables(0).Rows(k)("PR_W1_12"), _
                                ds.Tables(0).Rows(k)("PR_W2_1"), _
                                ds.Tables(0).Rows(k)("PR_W2_2"), _
                                ds.Tables(0).Rows(k)("PR_W2_3"), _
                                ds.Tables(0).Rows(k)("PR_W2_4"), _
                                ds.Tables(0).Rows(k)("PR_W2_5"), _
                                ds.Tables(0).Rows(k)("PR_W2_6"), _
                                ds.Tables(0).Rows(k)("PR_W2_7"), _
                                ds.Tables(0).Rows(k)("PR_W2_8"), _
                                ds.Tables(0).Rows(k)("PR_W2_9"), _
                                ds.Tables(0).Rows(k)("PR_W2_10"), _
                                ds.Tables(0).Rows(k)("PR_W2_11"), _
                                ds.Tables(0).Rows(k)("PR_W2_12"), _
                                ds.Tables(0).Rows(k)("PCPD1"), _
                                ds.Tables(0).Rows(k)("PCPD2"), _
                                ds.Tables(0).Rows(k)("PCPD3"), _
                                ds.Tables(0).Rows(k)("PCPD4"), _
                                ds.Tables(0).Rows(k)("PCPD5"), _
                                ds.Tables(0).Rows(k)("PCPD6"), _
                                ds.Tables(0).Rows(k)("PCPD7"), _
                                ds.Tables(0).Rows(k)("PCPD8"), _
                                ds.Tables(0).Rows(k)("PCPD9"), _
                                ds.Tables(0).Rows(k)("PCPD10"), _
                                ds.Tables(0).Rows(k)("PCPD11"), _
                                ds.Tables(0).Rows(k)("PCPD12"), _
                                ds.Tables(0).Rows(k)("RAINHHMX1"), _
                                ds.Tables(0).Rows(k)("RAINHHMX2"), _
                                ds.Tables(0).Rows(k)("RAINHHMX3"), _
                                ds.Tables(0).Rows(k)("RAINHHMX4"), _
                                ds.Tables(0).Rows(k)("RAINHHMX5"), _
                                ds.Tables(0).Rows(k)("RAINHHMX6"), _
                                ds.Tables(0).Rows(k)("RAINHHMX7"), _
                                ds.Tables(0).Rows(k)("RAINHHMX8"), _
                                ds.Tables(0).Rows(k)("RAINHHMX9"), _
                                ds.Tables(0).Rows(k)("RAINHHMX10"), _
                                ds.Tables(0).Rows(k)("RAINHHMX11"), _
                                ds.Tables(0).Rows(k)("RAINHHMX12"), _
                                ds.Tables(0).Rows(k)("SOLARAV1"), _
                                ds.Tables(0).Rows(k)("SOLARAV2"), _
                                ds.Tables(0).Rows(k)("SOLARAV3"), _
                                ds.Tables(0).Rows(k)("SOLARAV4"), _
                                ds.Tables(0).Rows(k)("SOLARAV5"), _
                                ds.Tables(0).Rows(k)("SOLARAV6"), _
                                ds.Tables(0).Rows(k)("SOLARAV7"), _
                                ds.Tables(0).Rows(k)("SOLARAV8"), _
                                ds.Tables(0).Rows(k)("SOLARAV9"), _
                                ds.Tables(0).Rows(k)("SOLARAV10"), _
                                ds.Tables(0).Rows(k)("SOLARAV11"), _
                                ds.Tables(0).Rows(k)("SOLARAV12"), _
                                ds.Tables(0).Rows(k)("DEWPT1"), _
                                ds.Tables(0).Rows(k)("DEWPT2"), _
                                ds.Tables(0).Rows(k)("DEWPT3"), _
                                ds.Tables(0).Rows(k)("DEWPT4"), _
                                ds.Tables(0).Rows(k)("DEWPT5"), _
                                ds.Tables(0).Rows(k)("DEWPT6"), _
                                ds.Tables(0).Rows(k)("DEWPT7"), _
                                ds.Tables(0).Rows(k)("DEWPT8"), _
                                ds.Tables(0).Rows(k)("DEWPT9"), _
                                ds.Tables(0).Rows(k)("DEWPT10"), _
                                ds.Tables(0).Rows(k)("DEWPT11"), _
                                ds.Tables(0).Rows(k)("DEWPT12"), _
                                ds.Tables(0).Rows(k)("WNDAV1"), _
                                ds.Tables(0).Rows(k)("WNDAV2"), _
                                ds.Tables(0).Rows(k)("WNDAV3"), _
                                ds.Tables(0).Rows(k)("WNDAV4"), _
                                ds.Tables(0).Rows(k)("WNDAV5"), _
                                ds.Tables(0).Rows(k)("WNDAV6"), _
                                ds.Tables(0).Rows(k)("WNDAV7"), _
                                ds.Tables(0).Rows(k)("WNDAV8"), _
                                ds.Tables(0).Rows(k)("WNDAV9"), _
                                ds.Tables(0).Rows(k)("WNDAV10"), _
                                ds.Tables(0).Rows(k)("WNDAV11"), _
                                ds.Tables(0).Rows(k)("WNDAV12"))

                    Next

                Next

            Else



                For i = 0 To rstStationNameCounter - 1
                    'Get the records from weather dbf file for US Stations
                    stationID = rstStationName.Tables(0).Rows(i)("MinRec")
                    subbasinID = rstStationName.Tables(0).Rows(i)("Subbasin")
                    stationName = rstStationName.Tables(0).Rows(i)("Station")

                    sql = "SELECT STATE, STATION, LSTATION,ID ,WLATITUDE, WLONGITUDE, WELEV, RAIN_YRS, TMPMX1, TMPMX2, TMPMX3, TMPMX4, TMPMX5, TMPMX6, TMPMX7, TMPMX8, TMPMX9, TMPMX10, TMPMX11, TMPMX12, 	TMPMN1, TMPMN2, TMPMN3, TMPMN4, TMPMN5, TMPMN6, TMPMN7,	TMPMN8, TMPMN9, TMPMN10, TMPMN11, TMPMN12, TMPSTDMX1, TMPSTDMX2, TMPSTDMX3, TMPSTDMX4, TMPSTDMX5, TMPSTDMX6, TMPSTDMX7, 	TMPSTDMX8, TMPSTDMX9, TMPSTDMX10, TMPSTDMX11, TMPSTDMX12, TMPSTDMN1, TMPSTDMN2, TMPSTDMN3, TMPSTDMN4, TMPSTDMN5, TMPSTDMN6, TMPSTDMN7, TMPSTDMN8, TMPSTDMN9, TMPSTDMN10, TMPSTDMN11, TMPSTDMN12, PCPMM1, PCPMM2, PCPMM3, PCPMM4, PCPMM5, PCPMM6, PCPMM7, PCPMM8, PCPMM9, PCPMM10, PCPMM11, PCPMM12, PCPSTD1, PCPSTD2, PCPSTD3, PCPSTD4, PCPSTD5, PCPSTD6, PCPSTD7, PCPSTD8, PCPSTD9, PCPSTD10, PCPSTD11, PCPSTD12, PCPSKW1, PCPSKW2, PCPSKW3, PCPSKW4, PCPSKW5, PCPSKW6, PCPSKW7, PCPSKW8, PCPSKW9, PCPSKW10, PCPSKW11, PCPSKW12, PR_W1_1, 	PR_W1_2, PR_W1_3, PR_W1_4, PR_W1_5, PR_W1_6, PR_W1_7, PR_W1_8, PR_W1_9, PR_W1_10, PR_W1_11, PR_W1_12, PR_W2_1, PR_W2_2, PR_W2_3, PR_W2_4, PR_W2_5, PR_W2_6, PR_W2_7, PR_W2_8, PR_W2_9, PR_W2_10, PR_W2_11, PR_W2_12, PCPD1, PCPD2, PCPD3, PCPD4, PCPD5, PCPD6, PCPD7, PCPD8, PCPD9, PCPD10, PCPD11, PCPD12, RAINHHMX1, RAINHHMX2, RAINHHMX3, RAINHHMX4, RAINHHMX5, RAINHHMX6, RAINHHMX7, RAINHHMX8, RAINHHMX9, RAINHHMX10, RAINHHMX11, RAINHHMX12, SOLARAV1, SOLARAV2, SOLARAV3, SOLARAV4, SOLARAV5, SOLARAV6, SOLARAV7, SOLARAV8, SOLARAV9, SOLARAV10, SOLARAV11, SOLARAV12, DEWPT1, DEWPT2, DEWPT3, DEWPT4, DEWPT5, DEWPT6, DEWPT7, DEWPT8, DEWPT9, DEWPT10, DEWPT11, DEWPT12, WNDAV1, WNDAV2, WNDAV3, WNDAV4, WNDAV5, WNDAV6, WNDAV7, WNDAV8, WNDAV9, WNDAV10, WNDAV11, WNDAV12 FROM Statwgn WHERE ID = " & stationID
                    da = New OleDb.OleDbDataAdapter(sql, dBaseConnection)
                    da.Fill(ds, "tableData")
                Next



                For k As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    System.Windows.Forms.Application.DoEvents()
                    AddWgn(conStrProject, subbasinID, _
                            ds.Tables(0).Rows(k)("STATION"), _
                            ds.Tables(0).Rows(k)("WLATITUDE"), _
                            ds.Tables(0).Rows(k)("WLONGITUDE"), _
                            ds.Tables(0).Rows(k)("WELEV"), _
                            ds.Tables(0).Rows(k)("RAIN_YRS"), _
                            ds.Tables(0).Rows(k)("TMPMX1"), _
                            ds.Tables(0).Rows(k)("TMPMX2"), _
                            ds.Tables(0).Rows(k)("TMPMX3"), _
                            ds.Tables(0).Rows(k)("TMPMX4"), _
                            ds.Tables(0).Rows(k)("TMPMX5"), _
                            ds.Tables(0).Rows(k)("TMPMX6"), _
                            ds.Tables(0).Rows(k)("TMPMX7"), _
                            ds.Tables(0).Rows(k)("TMPMX8"), _
                            ds.Tables(0).Rows(k)("TMPMX9"), _
                            ds.Tables(0).Rows(k)("TMPMX10"), _
                            ds.Tables(0).Rows(k)("TMPMX11"), _
                            ds.Tables(0).Rows(k)("TMPMX12"), _
                            ds.Tables(0).Rows(k)("TMPMN1"), _
                            ds.Tables(0).Rows(k)("TMPMN2"), _
                            ds.Tables(0).Rows(k)("TMPMN3"), _
                            ds.Tables(0).Rows(k)("TMPMN4"), _
                            ds.Tables(0).Rows(k)("TMPMN5"), _
                            ds.Tables(0).Rows(k)("TMPMN6"), _
                            ds.Tables(0).Rows(k)("TMPMN7"), _
                            ds.Tables(0).Rows(k)("TMPMN8"), _
                            ds.Tables(0).Rows(k)("TMPMN9"), _
                            ds.Tables(0).Rows(k)("TMPMN10"), _
                            ds.Tables(0).Rows(k)("TMPMN11"), _
                            ds.Tables(0).Rows(k)("TMPMN12"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX1"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX2"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX3"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX4"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX5"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX6"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX7"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX8"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX9"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX10"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX11"), _
                            ds.Tables(0).Rows(k)("TMPSTDMX12"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN1"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN2"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN3"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN4"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN5"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN6"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN7"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN8"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN9"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN10"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN11"), _
                            ds.Tables(0).Rows(k)("TMPSTDMN12"), _
                            ds.Tables(0).Rows(k)("PCPMM1"), _
                            ds.Tables(0).Rows(k)("PCPMM2"), _
                            ds.Tables(0).Rows(k)("PCPMM3"), _
                            ds.Tables(0).Rows(k)("PCPMM4"), _
                            ds.Tables(0).Rows(k)("PCPMM5"), _
                            ds.Tables(0).Rows(k)("PCPMM6"), _
                            ds.Tables(0).Rows(k)("PCPMM7"), _
                            ds.Tables(0).Rows(k)("PCPMM8"), _
                            ds.Tables(0).Rows(k)("PCPMM9"), _
                            ds.Tables(0).Rows(k)("PCPMM10"), _
                            ds.Tables(0).Rows(k)("PCPMM11"), _
                            ds.Tables(0).Rows(k)("PCPMM12"), _
                            ds.Tables(0).Rows(k)("PCPSTD1"), _
                            ds.Tables(0).Rows(k)("PCPSTD2"), _
                            ds.Tables(0).Rows(k)("PCPSTD3"), _
                            ds.Tables(0).Rows(k)("PCPSTD4"), _
                            ds.Tables(0).Rows(k)("PCPSTD5"), _
                            ds.Tables(0).Rows(k)("PCPSTD6"), _
                            ds.Tables(0).Rows(k)("PCPSTD7"), _
                            ds.Tables(0).Rows(k)("PCPSTD8"), _
                            ds.Tables(0).Rows(k)("PCPSTD9"), _
                            ds.Tables(0).Rows(k)("PCPSTD10"), _
                            ds.Tables(0).Rows(k)("PCPSTD11"), _
                            ds.Tables(0).Rows(k)("PCPSTD12"), _
                            ds.Tables(0).Rows(k)("PCPSKW1"), _
                            ds.Tables(0).Rows(k)("PCPSKW2"), _
                            ds.Tables(0).Rows(k)("PCPSKW3"), _
                            ds.Tables(0).Rows(k)("PCPSKW4"), _
                            ds.Tables(0).Rows(k)("PCPSKW5"), _
                            ds.Tables(0).Rows(k)("PCPSKW6"), _
                            ds.Tables(0).Rows(k)("PCPSKW7"), _
                            ds.Tables(0).Rows(k)("PCPSKW8"), _
                            ds.Tables(0).Rows(k)("PCPSKW9"), _
                            ds.Tables(0).Rows(k)("PCPSKW10"), _
                            ds.Tables(0).Rows(k)("PCPSKW11"), _
                            ds.Tables(0).Rows(k)("PCPSKW12"), _
                            ds.Tables(0).Rows(k)("PR_W1_1"), _
                            ds.Tables(0).Rows(k)("PR_W1_2"), _
                            ds.Tables(0).Rows(k)("PR_W1_3"), _
                            ds.Tables(0).Rows(k)("PR_W1_4"), _
                            ds.Tables(0).Rows(k)("PR_W1_5"), _
                            ds.Tables(0).Rows(k)("PR_W1_6"), _
                            ds.Tables(0).Rows(k)("PR_W1_7"), _
                            ds.Tables(0).Rows(k)("PR_W1_8"), _
                            ds.Tables(0).Rows(k)("PR_W1_9"), _
                            ds.Tables(0).Rows(k)("PR_W1_10"), _
                            ds.Tables(0).Rows(k)("PR_W1_11"), _
                            ds.Tables(0).Rows(k)("PR_W1_12"), _
                            ds.Tables(0).Rows(k)("PR_W2_1"), _
                            ds.Tables(0).Rows(k)("PR_W2_2"), _
                            ds.Tables(0).Rows(k)("PR_W2_3"), _
                            ds.Tables(0).Rows(k)("PR_W2_4"), _
                            ds.Tables(0).Rows(k)("PR_W2_5"), _
                            ds.Tables(0).Rows(k)("PR_W2_6"), _
                            ds.Tables(0).Rows(k)("PR_W2_7"), _
                            ds.Tables(0).Rows(k)("PR_W2_8"), _
                            ds.Tables(0).Rows(k)("PR_W2_9"), _
                            ds.Tables(0).Rows(k)("PR_W2_10"), _
                            ds.Tables(0).Rows(k)("PR_W2_11"), _
                            ds.Tables(0).Rows(k)("PR_W2_12"), _
                            ds.Tables(0).Rows(k)("PCPD1"), _
                            ds.Tables(0).Rows(k)("PCPD2"), _
                            ds.Tables(0).Rows(k)("PCPD3"), _
                            ds.Tables(0).Rows(k)("PCPD4"), _
                            ds.Tables(0).Rows(k)("PCPD5"), _
                            ds.Tables(0).Rows(k)("PCPD6"), _
                            ds.Tables(0).Rows(k)("PCPD7"), _
                            ds.Tables(0).Rows(k)("PCPD8"), _
                            ds.Tables(0).Rows(k)("PCPD9"), _
                            ds.Tables(0).Rows(k)("PCPD10"), _
                            ds.Tables(0).Rows(k)("PCPD11"), _
                            ds.Tables(0).Rows(k)("PCPD12"), _
                            ds.Tables(0).Rows(k)("RAINHHMX1"), _
                            ds.Tables(0).Rows(k)("RAINHHMX2"), _
                            ds.Tables(0).Rows(k)("RAINHHMX3"), _
                            ds.Tables(0).Rows(k)("RAINHHMX4"), _
                            ds.Tables(0).Rows(k)("RAINHHMX5"), _
                            ds.Tables(0).Rows(k)("RAINHHMX6"), _
                            ds.Tables(0).Rows(k)("RAINHHMX7"), _
                            ds.Tables(0).Rows(k)("RAINHHMX8"), _
                            ds.Tables(0).Rows(k)("RAINHHMX9"), _
                            ds.Tables(0).Rows(k)("RAINHHMX10"), _
                            ds.Tables(0).Rows(k)("RAINHHMX11"), _
                            ds.Tables(0).Rows(k)("RAINHHMX12"), _
                            ds.Tables(0).Rows(k)("SOLARAV1"), _
                            ds.Tables(0).Rows(k)("SOLARAV2"), _
                            ds.Tables(0).Rows(k)("SOLARAV3"), _
                            ds.Tables(0).Rows(k)("SOLARAV4"), _
                            ds.Tables(0).Rows(k)("SOLARAV5"), _
                            ds.Tables(0).Rows(k)("SOLARAV6"), _
                            ds.Tables(0).Rows(k)("SOLARAV7"), _
                            ds.Tables(0).Rows(k)("SOLARAV8"), _
                            ds.Tables(0).Rows(k)("SOLARAV9"), _
                            ds.Tables(0).Rows(k)("SOLARAV10"), _
                            ds.Tables(0).Rows(k)("SOLARAV11"), _
                            ds.Tables(0).Rows(k)("SOLARAV12"), _
                            ds.Tables(0).Rows(k)("DEWPT1"), _
                            ds.Tables(0).Rows(k)("DEWPT2"), _
                            ds.Tables(0).Rows(k)("DEWPT3"), _
                            ds.Tables(0).Rows(k)("DEWPT4"), _
                            ds.Tables(0).Rows(k)("DEWPT5"), _
                            ds.Tables(0).Rows(k)("DEWPT6"), _
                            ds.Tables(0).Rows(k)("DEWPT7"), _
                            ds.Tables(0).Rows(k)("DEWPT8"), _
                            ds.Tables(0).Rows(k)("DEWPT9"), _
                            ds.Tables(0).Rows(k)("DEWPT10"), _
                            ds.Tables(0).Rows(k)("DEWPT11"), _
                            ds.Tables(0).Rows(k)("DEWPT12"), _
                            ds.Tables(0).Rows(k)("WNDAV1"), _
                            ds.Tables(0).Rows(k)("WNDAV2"), _
                            ds.Tables(0).Rows(k)("WNDAV3"), _
                            ds.Tables(0).Rows(k)("WNDAV4"), _
                            ds.Tables(0).Rows(k)("WNDAV5"), _
                            ds.Tables(0).Rows(k)("WNDAV6"), _
                            ds.Tables(0).Rows(k)("WNDAV7"), _
                            ds.Tables(0).Rows(k)("WNDAV8"), _
                            ds.Tables(0).Rows(k)("WNDAV9"), _
                            ds.Tables(0).Rows(k)("WNDAV10"), _
                            ds.Tables(0).Rows(k)("WNDAV11"), _
                            ds.Tables(0).Rows(k)("WNDAV12"))

                Next

                'MsgBox(ds.Tables(0).Rows.Count)

            End If
            dBaseConnection.Close()


        End Sub
    End Class
End Class
