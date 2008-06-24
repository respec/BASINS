Partial Class SwatInput
    Private pGw As clsGw = New clsGw(Me)
    ReadOnly Property Gw() As clsGw
        Get
            Return pGw
        End Get
    End Property

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
                             & " " & DateNowString() & " ARCGIS-SWAT interface MAVZ")

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


                IO.File.WriteAllText(pSwatInput.OutputFolder & "\" & lTextFilename, lSB.ToString)
                'ReplaceNonAscii(lTextFilename)
            Next
        End Sub
    End Class
End Class
