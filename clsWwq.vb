Partial Class SwatInput
    Private pWwq As clsWwq = New clsWwq(Me)
    ReadOnly Property Wwq() As clsWwq
        Get
            Return pWwq
        End Get
    End Property

    Public Class clsWwqItem
        Public Lao As Long
        Public Igropt As Long
        Public Ai0 As Double
        Public Ai1 As Double
        Public Ai2 As Double
        Public Ai3 As Double
        Public Ai4 As Double
        Public Ai5 As Double
        Public Ai6 As Double
        Public Mumax As Double
        Public Rhoq As Double
        Public Tfact As Double
        Public K_l As Double
        Public K_n As Double
        Public K_p As Double
        Public Lambda0 As Double
        Public Lambda1 As Double
        Public Lambda2 As Double
        Public P_n As Double

        Public Sub New()
        End Sub

        Public Function AddSQL() As String
            Return "INSERT INTO wwq ( Lao , Igropt , Ai0 , Ai1 , Ai2 , Ai3 , Ai4 , Ai5 , Ai6 , Mumax , Rhoq , Tfact , K_l , K_n , K_p , Lambda0 , Lambda1 , Lambda2 , P_n   )" _
                   & "Values ('" & Lao & "'  ,'" & Igropt & "'  ,'" & Ai0 & "'  ,'" & Ai1 & "'  ,'" & Ai2 & "'  ,'" & Ai3 & "'  ,'" & Ai4 & "'  ,'" & Ai5 & "'  ,'" & Ai6 & "'  ,'" & Mumax & "'  ,'" & Rhoq & "'  ,'" & Tfact & "'  ,'" & K_l & "'  ,'" & K_n & "'  ,'" & K_p & "'  ,'" & Lambda0 & "'  ,'" & Lambda1 & "'  ,'" & Lambda2 & "'  ,'" & P_n & "'  )"
        End Function
    End Class

    ''' <summary>
    ''' WWQ Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsWwq
        Private pSwatInput As SwatInput
        Private pTableName As String = "wwq"

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn.DBLayer.createwwq
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
                    .Name = "OBJECTID"
                    .Type = ADOX.DataTypeEnum.adInteger
                    .ParentCatalog = lCatalog
                    .Properties("AutoIncrement").Value = True
                End With

                With lTable.Columns
                    .Append(lKeyColumn)
                    .Append("Lao", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("Igropt", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("Ai0", ADOX.DataTypeEnum.adDouble)
                    .Append("Ai1", ADOX.DataTypeEnum.adDouble)
                    .Append("Ai2", ADOX.DataTypeEnum.adDouble)
                    .Append("Ai3", ADOX.DataTypeEnum.adDouble)
                    .Append("Ai4", ADOX.DataTypeEnum.adDouble)
                    .Append("Ai5", ADOX.DataTypeEnum.adDouble)
                    .Append("Ai6", ADOX.DataTypeEnum.adDouble)
                    .Append("Mumax", ADOX.DataTypeEnum.adDouble)
                    .Append("Rhoq", ADOX.DataTypeEnum.adDouble)
                    .Append("Tfact", ADOX.DataTypeEnum.adDouble)
                    .Append("K_l", ADOX.DataTypeEnum.adDouble)
                    .Append("K_n", ADOX.DataTypeEnum.adDouble)
                    .Append("K_p", ADOX.DataTypeEnum.adDouble)
                    .Append("Lambda0", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("Lambda1", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("Lambda2", ADOX.DataTypeEnum.adDouble)
                    .Append("P_n", ADOX.DataTypeEnum.adDouble)
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

        Public Sub Add(ByVal aItem As clsWwqItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatInput)
        End Sub

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()

            Dim lRow As DataRow = aTable.Rows(0)
            pSwatInput.Status("Writing " & pTableName & " text ...")

            Dim lSB As New System.Text.StringBuilder

            ' --------------------------------------------------------------------------------
            ' 1st Line
            ' --------------------------------------------------------------------------------
            lSB.AppendLine("Watershed water quality file" & Space(10) & ".wwq file " & HeaderString())
            ' --------------------------------------------------------------------------------
            ' 2nd Line
            ' ------2-LAO
            lSB.AppendLine(MakeString(lRow.Item(1), 0, 4, 4) & "| LAO : Light averaging option")
            ' ------3-IGROPT
            lSB.AppendLine(MakeString(lRow.Item(2), 0, 4, 4) & "| IGROPT : Algal specific growth rate option")
            ' ------ 4-AI0
            lSB.AppendLine(MakeString(lRow.Item(3), 3, 4, 8) & "| AI0 : Ratio of chlorophyll-a to algal biomass [µg-chla/mg algae]")
            ' ------ 5-AI1
            lSB.AppendLine(MakeString(lRow.Item(4), 3, 4, 8) & "| AI1 : Fraction of algal biomass that is nitrogen [mg N/mg alg]")
            ' ------ 6-AI2
            lSB.AppendLine(MakeString(lRow.Item(5), 3, 4, 8) & "| AI2 : Fraction of algal biomass that is phosphorus [mg P/mg alg]")
            ' ------ 7-AI3
            lSB.AppendLine(MakeString(lRow.Item(6), 3, 4, 8) & "| AI3 : The rate of oxygen production per unit of algal photosynthesis [mg O2/mg alg)]")
            ' ------ 8-AI4
            lSB.AppendLine(MakeString(lRow.Item(7), 3, 4, 8) & "| AI4 : The rate of oxygen uptake per unit of algal respiration [mg O2/mg alg)]")
            ' ------ 9-AI5
            lSB.AppendLine(MakeString(lRow.Item(8), 3, 4, 8) & "| AI5 : The rate of oxygen uptake per unit of NH3-N oxidation [mg O2/mg NH3-N]")
            ' ------ 10-AI6
            lSB.AppendLine(MakeString(lRow.Item(9), 3, 4, 8) & "| AI6 : The rate of oxygen uptake per unit of NO2-N oxidation [mg O2/mg NO2-N]")
            ' ------ 11-MUMAX
            lSB.AppendLine(MakeString(lRow.Item(10), 3, 4, 8) & "| MUMAX : Maximum specific algal growth rate at 20º C [day-1]")
            ' ------ 12-RHOQ
            lSB.AppendLine(MakeString(lRow.Item(11), 3, 4, 8) & "| RHOQ : Algal respiration rate at 20º C [day-1]")
            ' ------ 13-TFACT
            lSB.AppendLine(MakeString(lRow.Item(12), 3, 4, 8) & "| TFACT : Fraction of solar radiation computed in the temperature heat balance that is photosynthetically active")
            ' ------14-K_1
            lSB.AppendLine(MakeString(lRow.Item(13), 3, 4, 8) & "| K_1 : Half-saturation coefficient for light [kJ/(m2·min)]")
            ' ------15-K_N
            lSB.AppendLine(MakeString(lRow.Item(14), 3, 4, 8) & "| K_N : Michaelis-Menton half-saturation constant for nitrogen [mg N/lL]")
            ' ------16-K_P
            lSB.AppendLine(MakeString(lRow.Item(15), 3, 4, 8) & "| K_P : Michaelis-Menton half-saturation constant for phosphorus [mg P/l]")
            ' ------17-LAMBDA0
            lSB.AppendLine(MakeString(lRow.Item(16), 3, 4, 8) & "| LAMBDA0 : Non-algal portion of the light extinction coefficient [m-1]")
            ' ------ 18-LAMBDA1
            lSB.AppendLine(MakeString(lRow.Item(17), 3, 4, 8) & "| LAMBDA1 : Linear algal self-shading coefficient [m-1·(µg chla/l)-1)]")
            ' ------ 19-LAMBDA2
            lSB.AppendLine(MakeString(lRow.Item(18), 3, 4, 8) & "| LAMBDA2 : Nonlinear algal self-shading coefficient [m-1·(µg chla/l)-2]")
            ' ------ 20-P_N
            lSB.AppendLine(MakeString(lRow.Item(19), 3, 4, 8) & "| P_N : Algal preference factor for ammonia")

            Dim lTextFilename As String = pSwatInput.TxtInOutFolder & "\basins." & pTableName
            IO.File.WriteAllText(lTextFilename, lSB.ToString)
            ReplaceNonAscii(lTextFilename)
        End Sub
    End Class
End Class
