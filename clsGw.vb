Partial Class SwatInput
    Private pGw As clsGw = New clsGw(Me)
    ReadOnly Property Gw() As clsGw
        Get
            Return pGw
        End Get
    End Property

    Public Class clsGwItem
        Public SUBBASIN As Double
        Public HRU As Double
        Public LANDUSE As String
        Public SOIL As String
        Public SLOPE_CD As String
        Public SHALLST As Double
        Public DEEPST As Double
        Public GW_DELAY As Double
        Public ALPHA_BF As Double
        Public GWQMN As Double
        Public GW_REVAP As Double
        Public REVAPMN As Double
        Public RCHRG_DP As Double
        Public GWHT As Double
        Public GW_SPYLD As Double
        Public SHALLST_N As Double
        Public GWSOLP As Double
        Public HLIFE_NGW As Double

        Public Sub New(ByVal aSUBBASIN As Double, _
                       ByVal aHRU As Double, _
                       ByVal aLANDUSE As String, _
                       ByVal aSOIL As String, _
                       ByVal aSLOPE_CD As String)
            SUBBASIN = aSUBBASIN
            HRU = aHRU
            LANDUSE = aLANDUSE
            SOIL = aSOIL
            SLOPE_CD = aSLOPE_CD
        End Sub

        Public Sub New(ByVal aSUBBASIN As Double, _
                        ByVal aHRU As Double, _
                        ByVal aLANDUSE As String, _
                        ByVal aSOIL As String, _
                        ByVal aSLOPE_CD As String, _
                        ByVal aSHALLST As Double, _
                        ByVal aDEEPST As Double, _
                        ByVal aGW_DELAY As Double, _
                        ByVal aALPHA_BF As Double, _
                        ByVal aGWQMN As Double, _
                        ByVal aGW_REVAP As Double, _
                        ByVal aREVAPMN As Double, _
                        ByVal aRCHRG_DP As Double, _
                        ByVal aGWHT As Double, _
                        ByVal aGW_SPYLD As Double, _
                        ByVal aSHALLST_N As Double, _
                        ByVal aGWSOLP As Double, _
                        ByVal aHLIFE_NGW As Double)
            SUBBASIN = aSUBBASIN
            HRU = aHRU
            LANDUSE = aLANDUSE
            SOIL = aSOIL
            SLOPE_CD = aSLOPE_CD
            SHALLST = aSHALLST
            DEEPST = aDEEPST
            GW_DELAY = aGW_DELAY
            ALPHA_BF = aALPHA_BF
            GWQMN = aGWQMN
            GW_REVAP = aGW_REVAP
            REVAPMN = aREVAPMN
            RCHRG_DP = aRCHRG_DP
            GWHT = aGWHT
            GW_SPYLD = aGW_SPYLD
            SHALLST_N = aSHALLST_N
            GWSOLP = aGWSOLP
            HLIFE_NGW = aHLIFE_NGW
        End Sub
    End Class

    ''' <summary>
    ''' Gw Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsGw
        Private pSwatInput As SwatInput
        Private pTableName As String = "gw"

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn.DBLayer.createGwTable
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

                With lTable.Columns
                    .Append(lKeyColumn)
                    .Append("SUBBASIN", ADOX.DataTypeEnum.adDouble)
                    .Append("HRU", ADOX.DataTypeEnum.adDouble)
                    .Append("LANDUSE", ADOX.DataTypeEnum.adVarWChar, 4)
                    .Append("SOIL", ADOX.DataTypeEnum.adVarWChar, 40)
                    .Append("SLOPE_CD", ADOX.DataTypeEnum.adVarWChar, 20)
                    .Append("SHALLST", ADOX.DataTypeEnum.adDouble)
                    .Append("DEEPST", ADOX.DataTypeEnum.adDouble)
                    .Append("GW_DELAY", ADOX.DataTypeEnum.adDouble)
                    .Append("ALPHA_BF", ADOX.DataTypeEnum.adDouble)
                    .Append("GWQMN", ADOX.DataTypeEnum.adDouble)
                    .Append("GW_REVAP", ADOX.DataTypeEnum.adDouble)
                    .Append("REVAPMN", ADOX.DataTypeEnum.adDouble)
                    .Append("RCHRG_DP", ADOX.DataTypeEnum.adDouble)
                    .Append("GWHT", ADOX.DataTypeEnum.adDouble)
                    .Append("GW_SPYLD", ADOX.DataTypeEnum.adDouble)
                    .Append("SHALLST_N", ADOX.DataTypeEnum.adDouble)
                    .Append("GWSOLP", ADOX.DataTypeEnum.adDouble)
                    .Append("HLIFE_NGW", ADOX.DataTypeEnum.adDouble)
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

        Public Sub Add(ByVal aGwItem As clsGwItem)
            With aGwItem
                Me.Add(.SUBBASIN, .HRU, .LANDUSE, .SOIL, .SLOPE_CD, .SHALLST, .DEEPST, .GW_DELAY, .ALPHA_BF, _
                       .GWQMN, .GW_REVAP, .REVAPMN, .RCHRG_DP, .GWHT, .GW_SPYLD, .SHALLST_N, .GWSOLP, .HLIFE_NGW)
            End With
        End Sub

        Private Sub Add(ByVal SUBBASIN As Double, _
                        ByVal HRU As Double, _
                        ByVal LANDUSE As String, _
                        ByVal SOIL As String, _
                        ByVal SLOPE_CD As String, _
                        ByVal SHALLST As Double, _
                        ByVal DEEPST As Double, _
                        ByVal GW_DELAY As Double, _
                        ByVal ALPHA_BF As Double, _
                        ByVal GWQMN As Double, _
                        ByVal GW_REVAP As Double, _
                        ByVal REVAPMN As Double, _
                        ByVal RCHRG_DP As Double, _
                        ByVal GWHT As Double, _
                        ByVal GW_SPYLD As Double, _
                        ByVal SHALLST_N As Double, _
                        ByVal GWSOLP As Double, _
                        ByVal HLIFE_NGW As Double)

            Dim lSQL As String = "INSERT INTO gw ( SUBBASIN , HRU , LANDUSE , SOIL , SLOPE_CD , SHALLST , DEEPST , GW_DELAY , ALPHA_BF , GWQMN , GW_REVAP , REVAPMN , RCHRG_DP , GWHT , GW_SPYLD , SHALLST_N , GWSOLP , HLIFE_NGW  ) " _
                               & "Values ('" & SUBBASIN & "'  ,'" & HRU & "'  ,'" & LANDUSE & "'  ,'" & SOIL & "'  ,'" & SLOPE_CD & "'  ,'" & SHALLST & "'  ,'" & DEEPST & "'  ,'" & GW_DELAY & "'  ,'" & ALPHA_BF & "'  ,'" & GWQMN & "'  ,'" & GW_REVAP & "'  ,'" & REVAPMN & "'  ,'" & RCHRG_DP & "'  ,'" & GWHT & "'  ,'" & GW_SPYLD & "'  ,'" & SHALLST_N & "'  ,'" & GWSOLP & "'  ,'" & HLIFE_NGW & "'  )"
            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()

            pSwatInput.Status("Writing " & pTableName & " text ...")

            For Each lRow As DataRow In aTable.Rows
                Dim lSubBasin As String = lRow.Item(1)
                Dim lHruNum As String = lRow.Item(2)
                Dim lTextFilename As String = StringFnameHRUs(lSubBasin, lHruNum) & "." & pTableName

                Dim lSB As New Text.StringBuilder
                lSB.AppendLine(" .gw file Subbasin:" & lSubBasin & " HRU:" & lHruNum & " Luse:" & lRow.Item(3) _
                             & " Soil: " & lRow.Item(4) & " Slope: " & lRow.Item(5) _
                             & " " & HeaderString())

                lSB.AppendLine(Format(lRow.Item(6), "0.0000").PadLeft(16) & "    | SHALLST : Initial depth of water in the shallow aquifer [mm]")
                lSB.AppendLine(Format(lRow.Item(7), "0.0000").PadLeft(16) & "    | DEEPST : Initial depth of water in the deep aquifer [mm]")
                lSB.AppendLine(Format(lRow.Item(8), "0.0000").PadLeft(16) & "    | GW_DELAY : Groundwater delay [days]")
                lSB.AppendLine(Format(lRow.Item(9), "0.0000").PadLeft(16) & "    | ALPHA_BF : BAseflow alpha factor [days]")
                lSB.AppendLine(Format(lRow.Item(10), "0.0000").PadLeft(16) & "    | GWQMN : Threshold depth of water in the shallow aquifer required for return flow to occur [mm]")
                lSB.AppendLine(Format(lRow.Item(11), "0.0000").PadLeft(16) & "    | GW_REVAP : Groundwater ""revap"" coefficient")
                lSB.AppendLine(Format(lRow.Item(12), "0.0000").PadLeft(16) & "    | REVAPMN: Threshold depth of water in the shallow aquifer for ""revap"" to occur [mm]")
                lSB.AppendLine(Format(lRow.Item(13), "0.0000").PadLeft(16) & "    | RCHRG_DP : Deep aquifer percolation fraction")
                lSB.AppendLine(Format(lRow.Item(14), "0.0000").PadLeft(16) & "    | GWHT : Initial groundwater height [m]")
                lSB.AppendLine(Format(lRow.Item(15), "0.0000").PadLeft(16) & "    | GW_SPYLD : Specific yield of the shallow aquifer [m3/m3]")
                lSB.AppendLine(Format(lRow.Item(16), "0.0000").PadLeft(16) & "    | SHALLST_N : Initial concentration of nitrate in shallow aquifer [mg N/l]")
                lSB.AppendLine(Format(lRow.Item(17), "0.0000").PadLeft(16) & "    | GWSOLP : Concentration of soluble phosphorus in groundwater contribution to streamflow from subbasin [mg P/l]")
                lSB.AppendLine(Format(lRow.Item(18), "0.0000").PadLeft(16) & "    | HLIFE_NGW : Ha;f-life of nitrate in the shallow aquifer [days]")


                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & lTextFilename, lSB.ToString)
                'ReplaceNonAscii(lTextFilename)
            Next
        End Sub
    End Class
End Class
