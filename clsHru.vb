Partial Class SwatInput
    Private pHru As clsHru = New clsHru(Me)
    ReadOnly Property Hru() As clsHru
        Get
            Return pHru
        End Get
    End Property

    Public Class clsHruItem
        Public SUBBASIN As Double
        Public HRU As Double
        Public LANDUSE As String
        Public SOIL As String
        Public SLOPE_CD As String
        Public HRU_FR As Double
        Public SLSUBBSN As Single
        Public HRU_SLP As Single
        Public OV_N As Single
        Public LAT_TTIME As Single
        Public LAT_SED As Single
        Public SLSOIL As Single
        Public CANMX As Single
        Public ESCO As Single
        Public EPCO As Single
        Public RSDIN As Single
        Public ERORGN As Single
        Public ERORGP As Single
        Public POT_FR As Single
        Public FLD_FR As Single
        Public RIP_FR As Single
        Public POT_TILE As Single
        Public POT_VOLX As Single
        Public POT_VOL As Single
        Public POT_NSED As Single
        Public POT_NO3L As Single
        Public DEP_IMP As Long

        Public Sub New(ByVal aSUBBASIN As Double, ByVal aHRU As Double)
            SUBBASIN = aSUBBASIN
            HRU = aHRU
        End Sub
    End Class

    ''' <summary>
    ''' Hydrologic Response Unit (HRU) input section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsHru
        Private pSwatInput As SwatInput
        Private pTableName As String = "hru"

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
                    .Append("SUBBASIN", ADOX.DataTypeEnum.adDouble)
                    .Append("HRU", ADOX.DataTypeEnum.adDouble)
                    .Append("LANDUSE", ADOX.DataTypeEnum.adVarWChar, 4)
                    .Append("SOIL", ADOX.DataTypeEnum.adVarWChar, 40)
                    .Append("SLOPE_CD", ADOX.DataTypeEnum.adVarWChar, 20)
                    .Append("HRU_FR", ADOX.DataTypeEnum.adDouble)
                    .Append("SLSUBBSN", ADOX.DataTypeEnum.adSingle)
                    .Append("HRU_SLP", ADOX.DataTypeEnum.adSingle)
                    .Append("OV_N", ADOX.DataTypeEnum.adSingle)
                    .Append("LAT_TTIME", ADOX.DataTypeEnum.adSingle)
                    .Append("LAT_SED", ADOX.DataTypeEnum.adSingle)
                    .Append("SLSOIL", ADOX.DataTypeEnum.adSingle)
                    .Append("CANMX", ADOX.DataTypeEnum.adSingle)
                    .Append("ESCO", ADOX.DataTypeEnum.adSingle)
                    .Append("EPCO", ADOX.DataTypeEnum.adSingle)
                    .Append("RSDIN", ADOX.DataTypeEnum.adSingle)
                    .Append("ERORGN", ADOX.DataTypeEnum.adSingle)
                    .Append("ERORGP", ADOX.DataTypeEnum.adSingle)
                    .Append("POT_FR", ADOX.DataTypeEnum.adSingle)
                    .Append("FLD_FR", ADOX.DataTypeEnum.adSingle)
                    .Append("RIP_FR", ADOX.DataTypeEnum.adSingle)
                    .Append("POT_TILE", ADOX.DataTypeEnum.adSingle)
                    .Append("POT_VOLX", ADOX.DataTypeEnum.adSingle)
                    .Append("POT_VOL", ADOX.DataTypeEnum.adSingle)
                    .Append("POT_NSED", ADOX.DataTypeEnum.adSingle)
                    .Append("POT_NO3L", ADOX.DataTypeEnum.adSingle)
                    .Append("DEP_IMP", ADOX.DataTypeEnum.adInteger, 4)
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
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & " ORDER BY SUBBASIN, HRU;")
        End Function

        Public Sub Add(ByVal aHruItem As clsHruItem)
            With aHruItem
                Me.Add(.SUBBASIN, .HRU, .LANDUSE, .SOIL, .SLOPE_CD, .HRU_FR, .SLSUBBSN, .HRU_SLP, .OV_N, _
                       .LAT_TTIME, .LAT_SED, .SLSOIL, .CANMX, .ESCO, .EPCO, .RSDIN, .ERORGN, .ERORGP, _
                       .POT_FR, .FLD_FR, .RIP_FR, .POT_TILE, .POT_VOLX, .POT_VOL, .POT_NSED, .POT_NO3L, .DEP_IMP)
            End With
        End Sub

        Public Sub Add(ByVal SUBBASIN As Double, _
                        ByVal HRU As Double, _
                        ByVal LANDUSE As String, _
                        ByVal SOIL As String, _
                        ByVal SLOPE_CD As String, _
                        ByVal HRU_FR As Double, _
                        ByVal SLSUBBSN As Single, _
                        ByVal HRU_SLP As Single, _
                        ByVal OV_N As Single, _
                        ByVal LAT_TTIME As Single, _
                        ByVal LAT_SED As Single, _
                        ByVal SLSOIL As Single, _
                        ByVal CANMX As Single, _
                        ByVal ESCO As Single, _
                        ByVal EPCO As Single, _
                        ByVal RSDIN As Single, _
                        ByVal ERORGN As Single, _
                        ByVal ERORGP As Single, _
                        ByVal POT_FR As Single, _
                        ByVal FLD_FR As Single, _
                        ByVal RIP_FR As Single, _
                        ByVal POT_TILE As Single, _
                        ByVal POT_VOLX As Single, _
                        ByVal POT_VOL As Single, _
                        ByVal POT_NSED As Single, _
                        ByVal POT_NO3L As Single, _
                        ByVal DEP_IMP As Long)

            Dim lSQL As String = "INSERT INTO hru ( SUBBASIN , HRU , LANDUSE , SOIL , SLOPE_CD , HRU_FR , SLSUBBSN , HRU_SLP , OV_N , LAT_TTIME , LAT_SED , SLSOIL , CANMX , ESCO , EPCO , RSDIN , ERORGN , ERORGP , POT_FR , FLD_FR , RIP_FR , POT_TILE , POT_VOLX , POT_VOL , POT_NSED , POT_NO3L , DEP_IMP ) " _
                               & "Values ('" & SUBBASIN & "'  ,'" & HRU & "'  ,'" & LANDUSE & "'  ,'" & SOIL & "'  ,'" & SLOPE_CD & "'  ,'" _
                               & HRU_FR & "'  ,'" & SLSUBBSN & "'  ,'" & HRU_SLP & "'  ,'" & OV_N & "'  ,'" & LAT_TTIME & "'  ,'" & LAT_SED & "'  ,'" _
                               & SLSOIL & "'  ,'" & CANMX & "'  ,'" & ESCO & "'  ,'" & EPCO & "'  ,'" & RSDIN & "'  ,'" _
                               & ERORGN & "'  ,'" & ERORGP & "'  ,'" & POT_FR & "'  ,'" & FLD_FR & "'  ,'" & RIP_FR & "'  ,'" _
                               & POT_TILE & "'  ,'" & POT_VOLX & "'  ,'" & POT_VOL & "'  ,'" & POT_NSED & "'  ,'" & POT_NO3L & "'  ,'" & DEP_IMP & "'  )"
            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

        ''' <summary>
        ''' Save HRU information to set of .hru text input files
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing " & pTableName & " text ...")
            For Each lHruRow As DataRow In aTable.Rows
                Dim lSubBasin As String = lHruRow.Item(1).ToString.Trim
                Dim lHruNum As String = lHruRow.Item(2).ToString.Trim
                Dim lHruName As String = StringFnameHRUs(lSubBasin, lHruNum) & "." & pTableName
                Dim lSB As New System.Text.StringBuilder
                '1st line
                lSB.AppendLine(" .hru file Subbasin:" & lSubBasin & " HRU:" & lHruNum _
                             & " Luse:" & lHruRow.Item(3) & " Soil: " & lHruRow.Item(4) & " Slope " & lHruRow.Item(5) _
                             & " " & DateNowString() & " ARCGIS-SWAT2003 interface MAVZ")
                '2. HRU_FR
                lSB.AppendLine(Format(lHruRow.Item(6), "0.0000000").PadLeft(16) & "    | HRU_FR : Fraction of subbasin area contained in HRU")
                '3. SLSUBBSN and so on....read comment
                lSB.AppendLine(Format(lHruRow.Item(7), "0.000").PadLeft(16) & "    | SLSUBBSN : Average slope length [m]")
                lSB.AppendLine(Format(lHruRow.Item(8), "0.000").PadLeft(16) & "    | HRU_SLP : Average slope stepness [m/m]")
                lSB.AppendLine(Format(lHruRow.Item(9), "0.000").PadLeft(16) & "    | OV_N : Manning's ""n"" value for overland flow")
                lSB.AppendLine(Format(lHruRow.Item(10), "0.000").PadLeft(16) & "    | LAT_TTIME : Lateral flow travel time [days]")
                lSB.AppendLine(Format(lHruRow.Item(11), "0.000").PadLeft(16) & "    | LAT_SED : Sediment concentration in lateral flow and groundwater flow [mg/l]")
                lSB.AppendLine(Format(lHruRow.Item(12), "0.000").PadLeft(16) & "    | SLSOIL : Slope length for lateral subsurface flow [m]")
                lSB.AppendLine(Format(lHruRow.Item(13), "0.000").PadLeft(16) & "    | CANMX : Maximum canopy storage [mm]")
                lSB.AppendLine(Format(lHruRow.Item(14), "0.000").PadLeft(16) & "    | ESCO : Soil evaporation compensation factor")
                lSB.AppendLine(Format(lHruRow.Item(15), "0.000").PadLeft(16) & "    | EPCO : Plant uptake compensation factor")
                lSB.AppendLine(Format(lHruRow.Item(16), "0.000").PadLeft(16) & "    | RSDIN : Initial residue cover [kg/ha]")
                lSB.AppendLine(Format(lHruRow.Item(17), "0.000").PadLeft(16) & "    | ERORGN : Organic N enrichment ratio")
                lSB.AppendLine(Format(lHruRow.Item(18), "0.000").PadLeft(16) & "    | ERORGP : Organic P enrichment ratio")
                lSB.AppendLine(Format(lHruRow.Item(19), "0.000").PadLeft(16) & "    | POT_FR : Fraction of HRU are that drains into pothole")
                lSB.AppendLine(Format(lHruRow.Item(20), "0.000").PadLeft(16) & "    | FLD_FR : Fraction of HRU that drains into floodplain")
                lSB.AppendLine(Format(lHruRow.Item(21), "0.000").PadLeft(16) & "    | RIP_FR : Fraction of HRU that drains into riparian zone")
                lSB.AppendLine("Special HRU: Pothole")
                lSB.AppendLine(Format(lHruRow.Item(22), "0.000").PadLeft(16) & "    | POT_TILE : Average daily outflow to main channel from tile flow [m3/s]")
                lSB.AppendLine(Format(lHruRow.Item(23), "0.000").PadLeft(16) & "    | POT_VOLX : Maximum volume of water stored in the pothole [104m3]")
                lSB.AppendLine(Format(lHruRow.Item(24), "0.000").PadLeft(16) & "    | POT_VOL : Initial volume of water stored in pothole [104m3]")
                lSB.AppendLine(Format(lHruRow.Item(25), "0.000").PadLeft(16) & "    | POT_NSED : Normal sediment concentration in pothole [mg/l]")
                lSB.AppendLine(Format(lHruRow.Item(26), "0.000").PadLeft(16) & "    | POT_NO3L : Nitrate decay rate in pothole [1/day]")
                lSB.AppendLine(Format(lHruRow.Item(27), "0").PadLeft(16) & "    | DEP_IMP : Depth to impervious layer in soil profile [mm]")

                Dim lHruFileName As String = pSwatInput.OutputFolder & "\" & lHruName
                IO.File.WriteAllText(lHruFileName, lSB.ToString)
            Next
        End Sub
        Public Function UniqueValues(ByVal aFieldName As String) As DataTable
            Return pSwatInput.QueryInputDB("Select hru." & aFieldName & " FROM(hru) GROUP BY hru." & aFieldName & " ORDER BY hru." & aFieldName & ";")
        End Function
    End Class
End Class
