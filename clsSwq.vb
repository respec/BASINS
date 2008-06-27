Partial Class SwatInput
    ''' <summary>
    ''' SubBasin (SUB) input section
    ''' </summary>
    ''' <remarks></remarks>
    Private pSwq As clsSwq = New clsSwq(Me)
    ReadOnly Property Swq() As clsSwq
        Get
            Return pSwq
        End Get
    End Property

    Public Class clsSwq
        Private pSwatInput As SwatInput
        Private pTableName As String = "swq"
        Private pItemLabels() As String = {"", "", _
             "    | RS1:   Local algal settling rate in the reach at 20ºC [m/day]", _
             "    | RS2:   Benthic (sediment) source rate for dissolved phosphorus in the reach at 20ºC [mg dissolved P/[m2·day]]", _
             "    | RS3:   Benthic source rate for NH4-N in the reach at 20ºC [mg NH4-N/[m2·day]]", _
             "    | RS4:   Rate coefficient for organic N settling in the reach at 20ºC [day-1]", _
             "    | RS5: Organic phosphorus settling rate in the reach at 20ºC [day-1]", _
             "    | RS6: Rate coefficient for settling of arbitrary non-conservative constituent in the reach at 20ºC [day-1]", _
             "    | RS7:   Benthic source rate for arbitrary non-conservative constituent in the reach at 20º C [mg ANC/[m2·day]]", _
             "    | RK1:   Carbonaceous biological oxygen demand deoxygenation rate coefficient in the reach at 20º C [day-1]", _
             "    | RK2:   Oxygen reaeration rate in accordance with Fickian diffusion in the reach at 20º C [day-1]", _
             "    | RK3:   Rate of loss of carbonaceous biological oxygen demand due to settling in the reach at 20º C [day-1]", _
             "    | RK4:   Benthic oxygen demand rate in the reach at 20º C [mg O2/[m2·day]]", _
             "    | RK5:   Coliform die-off rate in the reach at 20º C [day-1]", _
             "    | RK6:   Decay rate for arbitrary non-conservative constituent in the reach at 20º C [day-1]", _
             "    | BC1:   Rate constant for biological oxidation of NH4 to NO2 in the reach at 20º C [day-1]", _
             "    | BC2:   Rate constant for biological oxidation of NO2 to NO3 in the reach at 20º C [day-1]", _
             "    | BC3:   Rate constant for hydrolysis of organic N to NH4 in the reach at 20º C [day-1]", _
             "    | BC4:   Rate constant for mineralization of organic P to dissolved P in the reach at 20º C [day-1]", _
             "    | CHPST_REA: Pesticide reaction coefficient in reach [day-1]", _
             "    | CHPST_VOL: Pesticide volatilization coefficient in reach [m/day]", _
             "    | CHPST_KOC: Pesticide partition coefficient between water and air in reach [m3/day]", _
             "    | CHPST_STL: Settling velocity for pesticide sorbed to sediment [m/day]", _
             "    | CHPST_RSP: Resuspension velocity for pesticide sorbed to sediment [m/day]", _
             "    | CHPST_MIX: Mixing velocity (diffusion/dispersion) for pesticide in reach [m/day]", _
             "    | SEDPST_CONC: Initial pesticide concentration in reach bed sediment [mg/m3 sediment]", _
             "    | SEDPST_REA: Pesticide reaction coefficient in reach bed sediment [day-1]", _
             "    | SEDPST_BRY: Pesticide burial velocity in reach bed sediment [m/day]", _
             "    | SEDPST_ACT: Depth of active sediment layer for pesticide [m]"}

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

                With lTable.Columns
                    .Append(lKeyColumn)
                    .Append("SUBBASIN", ADOX.DataTypeEnum.adDouble)
                    .Append("RS1", ADOX.DataTypeEnum.adSingle)
                    .Append("RS2", ADOX.DataTypeEnum.adSingle)
                    .Append("RS3", ADOX.DataTypeEnum.adSingle)
                    .Append("RS4", ADOX.DataTypeEnum.adSingle)
                    .Append("RS5", ADOX.DataTypeEnum.adSingle)
                    .Append("RS6", ADOX.DataTypeEnum.adSingle)
                    .Append("RS7", ADOX.DataTypeEnum.adSingle)
                    .Append("RK1", ADOX.DataTypeEnum.adSingle)
                    .Append("RK2", ADOX.DataTypeEnum.adSingle)
                    .Append("RK3", ADOX.DataTypeEnum.adSingle)
                    .Append("RK4", ADOX.DataTypeEnum.adSingle)
                    .Append("RK5", ADOX.DataTypeEnum.adSingle)
                    .Append("RK6", ADOX.DataTypeEnum.adSingle)
                    .Append("BC1", ADOX.DataTypeEnum.adSingle)
                    .Append("BC2", ADOX.DataTypeEnum.adSingle)
                    .Append("BC3", ADOX.DataTypeEnum.adSingle)
                    .Append("BC4", ADOX.DataTypeEnum.adSingle)
                    .Append("CHPST_REA", ADOX.DataTypeEnum.adSingle)
                    .Append("CHPST_VOL", ADOX.DataTypeEnum.adSingle)
                    .Append("CHPST_KOC", ADOX.DataTypeEnum.adSingle)
                    .Append("CHPST_STL", ADOX.DataTypeEnum.adSingle)
                    .Append("CHPST_RSP", ADOX.DataTypeEnum.adSingle)
                    .Append("CHPST_MIX", ADOX.DataTypeEnum.adSingle)
                    .Append("SEDPST_CONC", ADOX.DataTypeEnum.adSingle)
                    .Append("SEDPST_REA", ADOX.DataTypeEnum.adSingle)
                    .Append("SEDPST_BRY", ADOX.DataTypeEnum.adSingle)
                    .Append("SEDPST_ACT", ADOX.DataTypeEnum.adSingle)
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

        Public Sub Add(ByVal SUBBASIN As Double, _
                        ByVal RS1 As Single, _
                        ByVal RS2 As Single, _
                        ByVal RS3 As Single, _
                        ByVal RS4 As Single, _
                        ByVal RS5 As Single, _
                        ByVal RS6 As Single, _
                        ByVal RS7 As Single, _
                        ByVal RK1 As Single, _
                        ByVal RK2 As Single, _
                        ByVal RK3 As Single, _
                        ByVal RK4 As Single, _
                        ByVal RK5 As Single, _
                        ByVal RK6 As Single, _
                        ByVal BC1 As Single, _
                        ByVal BC2 As Single, _
                        ByVal BC3 As Single, _
                        ByVal BC4 As Single, _
                        ByVal CHPST_REA As Single, _
                        ByVal CHPST_VOL As Single, _
                        ByVal CHPST_KOC As Single, _
                        ByVal CHPST_STL As Single, _
                        ByVal CHPST_RSP As Single, _
                        ByVal CHPST_MIX As Single, _
                        ByVal SEDPST_CONC As Single, _
                        ByVal SEDPST_REA As Single, _
                        ByVal SEDPST_BRY As Single, _
                        ByVal SEDPST_ACT As Single)

            Dim lSQL As String = "INSERT INTO swq ( SUBBASIN , RS1 , RS2 , RS3 , RS4 , RS5 , RS6 , RS7 , RK1 , RK2 , RK3 , RK4 , RK5 , RK6 , BC1 , BC2 , BC3 , BC4 , CHPST_REA , CHPST_VOL , CHPST_KOC , CHPST_STL , CHPST_RSP , CHPST_MIX , SEDPST_CONC , SEDPST_REA , SEDPST_BRY , SEDPST_ACT ) " _
                               & "Values ('" & SUBBASIN & "'  ,'" & RS1 & "'  ,'" & RS2 & "'  ,'" & RS3 & "'  ,'" & RS4 & "'  ,'" & RS5 & "'  ,'" & RS6 & "'  ,'" & RS7 & "'  ,'" & RK1 & "'  ,'" & RK2 & "'  ,'" & RK3 & "'  ,'" & RK4 & "'  ,'" & RK5 & "'  ,'" & RK6 & "'  ,'" & BC1 & "'  ,'" & BC2 & "'  ,'" & BC3 & "'  ,'" & BC4 & "'  ,'" & CHPST_REA & "'  ,'" & CHPST_VOL & "'  ,'" & CHPST_KOC & "'  ,'" & CHPST_STL & "'  ,'" & CHPST_RSP & "'  ,'" & CHPST_MIX & "'  ,'" & SEDPST_CONC & "'  ,'" & SEDPST_REA & "'  ,'" & SEDPST_BRY & "'  ,'" & SEDPST_ACT & "'  );"
            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing " & pTableName & " text ...")
            Dim lParam As Integer

            For Each lRow As DataRow In aTable.Rows
                Dim lSB As New System.Text.StringBuilder
                Dim lSubBasin As String = lRow.Item("SUBBASIN")

                '1st line
                lSB.AppendLine(" .Swq file Subbasin: " & lSubBasin & " " & HeaderString())
                lSB.AppendLine("Nutrient (QUAL2E parameters)")

                For lParam = 2 To 18
                    lSB.AppendLine(Format(lRow.Item(lParam), "0.000").PadLeft(16) & pItemLabels(lParam))
                Next

                lSB.AppendLine("Pesticide Parameters:")

                For lParam = 19 To 28
                    lSB.AppendLine(Format(lRow.Item(lParam), "0.000").PadLeft(16) & pItemLabels(lParam))
                Next

                Dim lTextFilename As String = pSwatInput.TxtInOutFolder & "\" & StringFname(lSubBasin, pTableName)
                IO.File.WriteAllText(lTextFilename, lSB.ToString)
                ReplaceNonAscii(lTextFilename)
            Next
        End Sub
    End Class
End Class
