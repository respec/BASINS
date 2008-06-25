Imports System.Data.OleDb

Partial Class SwatInput
    Private pSol As clsSol = New clsSol(Me)
    ReadOnly Property Sol() As clsSol
        Get
            Return pSol
        End Get
    End Property

    ''' <summary>
    ''' Sol Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsSol
        Private pSwatInput As SwatInput
        Private pTableName As String = "sol"

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn.DBLayer.createSolTable
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
                    .Name = "OBJECTID"
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
                    .Append("SNAM", ADOX.DataTypeEnum.adVarWChar, 30)
                    .Append("NLAYERS", ADOX.DataTypeEnum.adInteger, 2)
                    .Append("HYDGRP", ADOX.DataTypeEnum.adVarWChar, 1)
                    .Append("SOL_ZMX", ADOX.DataTypeEnum.adSingle)
                    .Append("ANION_EXCL", ADOX.DataTypeEnum.adSingle)
                    .Append("SOL_CRK", ADOX.DataTypeEnum.adSingle)
                    .Append("TEXTURE", ADOX.DataTypeEnum.adVarWChar, 80)
                    .Append("SOL_Z1", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_BD1", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_AWC1", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_K1", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_CBN1", ADOX.DataTypeEnum.adDouble)
                    .Append("CLAY1", ADOX.DataTypeEnum.adDouble)
                    .Append("SILT1", ADOX.DataTypeEnum.adDouble)
                    .Append("SAND1", ADOX.DataTypeEnum.adDouble)
                    .Append("ROCK1", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_ALB1", ADOX.DataTypeEnum.adDouble)
                    .Append("USLE_K1", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_EC1", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_Z2", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_BD2", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_AWC2", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_K2", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_CBN2", ADOX.DataTypeEnum.adDouble)
                    .Append("CLAY2", ADOX.DataTypeEnum.adDouble)
                    .Append("SILT2", ADOX.DataTypeEnum.adDouble)
                    .Append("SAND2", ADOX.DataTypeEnum.adDouble)
                    .Append("ROCK2", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_ALB2", ADOX.DataTypeEnum.adDouble)
                    .Append("USLE_K2", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_EC2", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_Z3", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_BD3", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_AWC3", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_K3", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_CBN3", ADOX.DataTypeEnum.adDouble)
                    .Append("CLAY3", ADOX.DataTypeEnum.adDouble)
                    .Append("SILT3", ADOX.DataTypeEnum.adDouble)
                    .Append("SAND3", ADOX.DataTypeEnum.adDouble)
                    .Append("ROCK3", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_ALB3", ADOX.DataTypeEnum.adDouble)
                    .Append("USLE_K3", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_EC3", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_Z4", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_BD4", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_AWC4", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_K4", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_CBN4", ADOX.DataTypeEnum.adDouble)
                    .Append("CLAY4", ADOX.DataTypeEnum.adDouble)
                    .Append("SILT4", ADOX.DataTypeEnum.adDouble)
                    .Append("SAND4", ADOX.DataTypeEnum.adDouble)
                    .Append("ROCK4", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_ALB4", ADOX.DataTypeEnum.adDouble)
                    .Append("USLE_K4", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_EC4", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_Z5", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_BD5", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_AWC5", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_K5", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_CBN5", ADOX.DataTypeEnum.adDouble)
                    .Append("CLAY5", ADOX.DataTypeEnum.adDouble)
                    .Append("SILT5", ADOX.DataTypeEnum.adDouble)
                    .Append("SAND5", ADOX.DataTypeEnum.adDouble)
                    .Append("ROCK5", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_ALB5", ADOX.DataTypeEnum.adDouble)
                    .Append("USLE_K5", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_EC5", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_Z6", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_BD6", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_AWC6", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_K6", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_CBN6", ADOX.DataTypeEnum.adDouble)
                    .Append("CLAY6", ADOX.DataTypeEnum.adDouble)
                    .Append("SILT6", ADOX.DataTypeEnum.adDouble)
                    .Append("SAND6", ADOX.DataTypeEnum.adDouble)
                    .Append("ROCK6", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_ALB6", ADOX.DataTypeEnum.adDouble)
                    .Append("USLE_K6", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_EC6", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_Z7", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_BD7", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_AWC7", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_K7", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_CBN7", ADOX.DataTypeEnum.adDouble)
                    .Append("CLAY7", ADOX.DataTypeEnum.adDouble)
                    .Append("SILT7", ADOX.DataTypeEnum.adDouble)
                    .Append("SAND7", ADOX.DataTypeEnum.adDouble)
                    .Append("ROCK7", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_ALB7", ADOX.DataTypeEnum.adDouble)
                    .Append("USLE_K7", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_EC7", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_Z8", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_BD8", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_AWC8", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_K8", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_CBN8", ADOX.DataTypeEnum.adDouble)
                    .Append("CLAY8", ADOX.DataTypeEnum.adDouble)
                    .Append("SILT8", ADOX.DataTypeEnum.adDouble)
                    .Append("SAND8", ADOX.DataTypeEnum.adDouble)
                    .Append("ROCK8", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_ALB8", ADOX.DataTypeEnum.adDouble)
                    .Append("USLE_K8", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_EC8", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_9", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_BD9", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_AWC9", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_K9", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_CBN9", ADOX.DataTypeEnum.adDouble)
                    .Append("CLAY9", ADOX.DataTypeEnum.adDouble)
                    .Append("SILT9", ADOX.DataTypeEnum.adDouble)
                    .Append("SAND9", ADOX.DataTypeEnum.adDouble)
                    .Append("ROCK9", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_ALB9", ADOX.DataTypeEnum.adDouble)
                    .Append("USLE_K9", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_EC9", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_Z10", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_BD10", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_AWC10", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_K10", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_CBN10", ADOX.DataTypeEnum.adDouble)
                    .Append("CLAY10", ADOX.DataTypeEnum.adDouble)
                    .Append("SILT10", ADOX.DataTypeEnum.adDouble)
                    .Append("SAND10", ADOX.DataTypeEnum.adDouble)
                    .Append("ROCK10", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_ALB10", ADOX.DataTypeEnum.adDouble)
                    .Append("USLE_K10", ADOX.DataTypeEnum.adDouble)
                    .Append("SOL_EC10", ADOX.DataTypeEnum.adDouble)
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
            ByVal HRU As Double, _
            ByVal LANDUSE As String, _
            ByVal SOIL As String, _
            ByVal SLOPE_CD As String, _
            ByVal SNAM As String, _
            ByVal NLAYERS As Integer, _
            ByVal HYDGRP As String, _
            ByVal SOL_ZMX As Single, _
            ByVal ANION_EXCL As Single, _
            ByVal SOL_CRK As Single, _
            ByVal TEXTURE As String, _
            ByVal SOL_Z1 As Double, _
            ByVal SOL_BD1 As Double, _
            ByVal SOL_AWC1 As Double, _
            ByVal SOL_K1 As Double, _
            ByVal SOL_CBN1 As Double, _
            ByVal CLAY1 As Double, _
            ByVal SILT1 As Double, _
            ByVal SAND1 As Double, _
            ByVal ROCK1 As Double, _
            ByVal SOL_ALB1 As Double, _
            ByVal USLE_K1 As Double, _
            ByVal SOL_EC1 As Double, _
            ByVal SOL_Z2 As Double, _
            ByVal SOL_BD2 As Double, _
            ByVal SOL_AWC2 As Double, _
            ByVal SOL_K2 As Double, _
            ByVal SOL_CBN2 As Double, _
            ByVal CLAY2 As Double, _
            ByVal SILT2 As Double, _
            ByVal SAND2 As Double, _
            ByVal ROCK2 As Double, _
            ByVal SOL_ALB2 As Double, _
            ByVal USLE_K2 As Double, _
            ByVal SOL_EC2 As Double, _
            ByVal SOL_Z3 As Double, _
            ByVal SOL_BD3 As Double, _
            ByVal SOL_AWC3 As Double, _
            ByVal SOL_K3 As Double, _
            ByVal SOL_CBN3 As Double, _
            ByVal CLAY3 As Double, _
            ByVal SILT3 As Double, _
            ByVal SAND3 As Double, _
            ByVal ROCK3 As Double, _
            ByVal SOL_ALB3 As Double, _
            ByVal USLE_K3 As Double, _
            ByVal SOL_EC3 As Double, _
            ByVal SOL_Z4 As Double, _
            ByVal SOL_BD4 As Double, _
            ByVal SOL_AWC4 As Double, _
            ByVal SOL_K4 As Double, _
            ByVal SOL_CBN4 As Double, _
            ByVal CLAY4 As Double, _
            ByVal SILT4 As Double, _
            ByVal SAND4 As Double, _
            ByVal ROCK4 As Double, _
            ByVal SOL_ALB4 As Double, _
            ByVal USLE_K4 As Double, _
            ByVal SOL_EC4 As Double, _
            ByVal SOL_Z5 As Double, _
            ByVal SOL_BD5 As Double, _
            ByVal SOL_AWC5 As Double, _
            ByVal SOL_K5 As Double, _
            ByVal SOL_CBN5 As Double, _
            ByVal CLAY5 As Double, _
            ByVal SILT5 As Double, _
            ByVal SAND5 As Double, _
            ByVal ROCK5 As Double, _
            ByVal SOL_ALB5 As Double, _
            ByVal USLE_K5 As Double, _
            ByVal SOL_EC5 As Double, _
            ByVal SOL_Z6 As Double, _
            ByVal SOL_BD6 As Double, _
            ByVal SOL_AWC6 As Double, _
            ByVal SOL_K6 As Double, _
            ByVal SOL_CBN6 As Double, _
            ByVal CLAY6 As Double, _
            ByVal SILT6 As Double, _
            ByVal SAND6 As Double, _
            ByVal ROCK6 As Double, _
            ByVal SOL_ALB6 As Double, _
            ByVal USLE_K6 As Double, _
            ByVal SOL_EC6 As Double, _
            ByVal SOL_Z7 As Double, _
            ByVal SOL_BD7 As Double, _
            ByVal SOL_AWC7 As Double, _
            ByVal SOL_K7 As Double, _
            ByVal SOL_CBN7 As Double, _
            ByVal CLAY7 As Double, _
            ByVal SILT7 As Double, _
            ByVal SAND7 As Double, _
            ByVal ROCK7 As Double, _
            ByVal SOL_ALB7 As Double, _
            ByVal USLE_K7 As Double, _
            ByVal SOL_EC7 As Double, _
            ByVal SOL_Z8 As Double, _
            ByVal SOL_BD8 As Double, _
            ByVal SOL_AWC8 As Double, _
            ByVal SOL_K8 As Double, _
            ByVal SOL_CBN8 As Double, _
            ByVal CLAY8 As Double, _
            ByVal SILT8 As Double, _
            ByVal SAND8 As Double, _
            ByVal ROCK8 As Double, _
            ByVal SOL_ALB8 As Double, _
            ByVal USLE_K8 As Double, _
            ByVal SOL_EC8 As Double, _
            ByVal SOL_9 As Double, _
            ByVal SOL_BD9 As Double, _
            ByVal SOL_AWC9 As Double, _
            ByVal SOL_K9 As Double, _
            ByVal SOL_CBN9 As Double, _
            ByVal CLAY9 As Double, _
            ByVal SILT9 As Double, _
            ByVal SAND9 As Double, _
            ByVal ROCK9 As Double, _
            ByVal SOL_ALB9 As Double, _
            ByVal USLE_K9 As Double, _
            ByVal SOL_EC9 As Double, _
            ByVal SOL_Z10 As Double, _
            ByVal SOL_BD10 As Double, _
            ByVal SOL_AWC10 As Double, _
            ByVal SOL_K10 As Double, _
            ByVal SOL_CBN10 As Double, _
            ByVal CLAY10 As Double, _
            ByVal SILT10 As Double, _
            ByVal SAND10 As Double, _
            ByVal ROCK10 As Double, _
            ByVal SOL_ALB10 As Double, _
            ByVal USLE_K10 As Double, _
            ByVal SOL_EC10 As Double)

            Dim lSQL As String = "INSERT INTO sol ( SUBBASIN ,HRU ,LANDUSE ,SOIL ,SLOPE_CD ,SNAM ,NLAYERS ,HYDGRP ,SOL_ZMX ,ANION_EXCL ,SOL_CRK ,TEXTURE ,SOL_Z1 ,SOL_BD1 ,SOL_AWC1 ,SOL_K1 ,SOL_CBN1 ,CLAY1 ,SILT1 ,SAND1 ,ROCK1 ,SOL_ALB1 ,USLE_K1 ,SOL_EC1 ,SOL_Z2 ,SOL_BD2 ,SOL_AWC2 ,SOL_K2 ,SOL_CBN2 ,CLAY2 ,SILT2 ,SAND2 ,ROCK2 ,SOL_ALB2 ,USLE_K2 ,SOL_EC2 ,SOL_Z3 ,SOL_BD3 ,SOL_AWC3 ,SOL_K3 ,SOL_CBN3 ,CLAY3 ,SILT3 ,SAND3 ,ROCK3 ,SOL_ALB3 ,USLE_K3 ,SOL_EC3 ,SOL_Z4 ,SOL_BD4 ,SOL_AWC4 ,SOL_K4 ,SOL_CBN4 ,CLAY4 ,SILT4 ,SAND4 ,ROCK4 ,SOL_ALB4 ,USLE_K4 ,SOL_EC4 ,SOL_Z5 ,SOL_BD5 ,SOL_AWC5 ,SOL_K5 ,SOL_CBN5 ,CLAY5 ,SILT5 ,SAND5 ,ROCK5 ,SOL_ALB5 ,USLE_K5 ,SOL_EC5 ,SOL_Z6 ,SOL_BD6 ,SOL_AWC6 ,SOL_K6 ,SOL_CBN6 ,CLAY6 ,SILT6 ,SAND6 ,ROCK6 ,SOL_ALB6 ,USLE_K6 ,SOL_EC6 ,SOL_Z7 ,SOL_BD7 ,SOL_AWC7 ,SOL_K7 ,SOL_CBN7 ,CLAY7 ,SILT7 ,SAND7 ,ROCK7 ,SOL_ALB7 ,USLE_K7 ,SOL_EC7 ,SOL_Z8 ,SOL_BD8 ,SOL_AWC8 ,SOL_K8 ,SOL_CBN8 ,CLAY8 ,SILT8 ,SAND8 ,ROCK8 ,SOL_ALB8 ,USLE_K8 ,SOL_EC8 ,SOL_9 ,SOL_BD9 ,SOL_AWC9 ,SOL_K9 ,SOL_CBN9 ,CLAY9 ,SILT9 ,SAND9 ,ROCK9 ,SOL_ALB9 ,USLE_K9 ,SOL_EC9 ,SOL_Z10 ,SOL_BD10 ,SOL_AWC10 ,SOL_K10 ,SOL_CBN10 ,CLAY10 ,SILT10 ,SAND10 ,ROCK10 ,SOL_ALB10 ,USLE_K10 ,SOL_EC10)" & _
            "Values ( '" & SUBBASIN & "' ,'" & HRU & "' ,'" & LANDUSE & "' ,'" & SOIL & "' ,'" & SLOPE_CD & "' ,'" & SNAM & "' ,'" & NLAYERS & "' ,'" & HYDGRP & "' ,'" & SOL_ZMX & "' ,'" & ANION_EXCL & "' ,'" & SOL_CRK & "' ,'" & TEXTURE & "' ,'" & SOL_Z1 & "' ,'" & SOL_BD1 & "' ,'" & SOL_AWC1 & "' ,'" & SOL_K1 & "' ,'" & SOL_CBN1 & "' ,'" & CLAY1 & "' ,'" & SILT1 & "' ,'" & SAND1 & "' ,'" & ROCK1 & "' ,'" & SOL_ALB1 & "' ,'" & USLE_K1 & "' ,'" & SOL_EC1 & "' ,'" & SOL_Z2 & "' ,'" & SOL_BD2 & "' ,'" & SOL_AWC2 & "' ,'" & SOL_K2 & "' ,'" & SOL_CBN2 & "' ,'" & CLAY2 & "' ,'" & SILT2 & "' ,'" & SAND2 & "' ,'" & ROCK2 & "' ,'" & SOL_ALB2 & "' ,'" & USLE_K2 & "' ,'" & SOL_EC2 & "' ,'" & SOL_Z3 & "' ,'" & SOL_BD3 & "' ,'" & SOL_AWC3 & "' ,'" & SOL_K3 & "' ,'" & SOL_CBN3 & "' ,'" & CLAY3 & "' ,'" & SILT3 & "' ,'" & SAND3 & "' ,'" & ROCK3 & "' ,'" & SOL_ALB3 & "' ,'" & USLE_K3 & "' ,'" & SOL_EC3 & "' ,'" & SOL_Z4 & "' ,'" & SOL_BD4 & "' ,'" & SOL_AWC4 & "' ,'" & SOL_K4 & "' ,'" & SOL_CBN4 & "' ,'" & CLAY4 & "' ,'" & SILT4 & "' ,'" & SAND4 & "' ,'" & ROCK4 & "' ,'" & SOL_ALB4 & "' ,'" & USLE_K4 & "' ,'" & SOL_EC4 & "' ,'" & SOL_Z5 & "' ,'" & SOL_BD5 & "' ,'" & SOL_AWC5 & "' ,'" & SOL_K5 & "' ,'" & SOL_CBN5 & "' ,'" & CLAY5 & "' ,'" & SILT5 & "' ,'" & SAND5 & "' ,'" & ROCK5 & "' ,'" & SOL_ALB5 & "' ,'" & USLE_K5 & "' ,'" & SOL_EC5 & "' ,'" & SOL_Z6 & "' ,'" & SOL_BD6 & "' ,'" & SOL_AWC6 & "' ,'" & SOL_K6 & "' ,'" & SOL_CBN6 & "' ,'" & CLAY6 & "' ,'" & SILT6 & "' ,'" & SAND6 & "' ,'" & ROCK6 & "' ,'" & SOL_ALB6 & "' ,'" & USLE_K6 & "' ,'" & SOL_EC6 & "' ,'" & SOL_Z7 & "' ,'" & SOL_BD7 & "' ,'" & SOL_AWC7 & "' ,'" & SOL_K7 & "' ,'" & SOL_CBN7 & "' ,'" & CLAY7 & "' ,'" & SILT7 & "' ,'" & SAND7 & "' ,'" & ROCK7 & "' ,'" & SOL_ALB7 & "' ,'" & USLE_K7 & "' ,'" & SOL_EC7 & "' ,'" & SOL_Z8 & "' ,'" & SOL_BD8 & "' ,'" & SOL_AWC8 & "' ,'" & SOL_K8 & "' ,'" & SOL_CBN8 & "' ,'" & CLAY8 & "' ,'" & SILT8 & "' ,'" & SAND8 & "' ,'" & ROCK8 & "' ,'" & SOL_ALB8 & "' ,'" & USLE_K8 & "' ,'" & SOL_EC8 & "' ,'" & SOL_9 & "' ,'" & SOL_BD9 & "' ,'" & SOL_AWC9 & "' ,'" & SOL_K9 & "' ,'" & SOL_CBN9 & "' ,'" & CLAY9 & "' ,'" & SILT9 & "' ,'" & SAND9 & "' ,'" & ROCK9 & "' ,'" & SOL_ALB9 & "' ,'" & USLE_K9 & "' ,'" & SOL_EC9 & "' ,'" & SOL_Z10 & "' ,'" & SOL_BD10 & "' ,'" & SOL_AWC10 & "' ,'" & SOL_K10 & "' ,'" & SOL_CBN10 & "' ,'" & CLAY10 & "' ,'" & SILT10 & "' ,'" & SAND10 & "' ,'" & ROCK10 & "' ,'" & SOL_ALB10 & "' ,'" & USLE_K10 & "' ,'" & SOL_EC10 & "')"

            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & " ORDER BY SUBBASIN, HRU;")
        End Function

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()

            pSwatInput.Status("Writing " & pTableName & " text ...")

            For Each lRow As DataRow In aTable.Rows
                Dim lSubBasin As String = lRow.Item(1)
                Dim lHruNum As String = lRow.Item(2)
                Dim lTextFilename As String = StringFnameHRUs(lSubBasin, lHruNum) & "." & pTableName

                Dim lSB As New Text.StringBuilder
                lSB.AppendLine(" .Sol file Subbasin:" & lSubBasin & " HRU:" & lHruNum & " Luse:" & lRow.Item(3) _
                             & " Soil: " & lRow.Item(4) & " Slope: " & lRow.Item(5) _
                             & " " & DateNowString() & " ARCGIS-SWAT interface MAVZ")
                lSB.AppendLine(" Soil Name: " & lRow.Item("SNAM"))

                lSB.AppendLine(" Soil Hydrologic Group: " & lRow.Item("HYDGRP"))
                lSB.AppendLine(" Maximum rooting depth(m) : " & Format(lRow.Item("SOL_ZMX"), "0.00").PadLeft(7))
                lSB.AppendLine(" Porosity fraction from which anions are excluded: " & Format(lRow.Item("ANION_EXCL"), "0.000").PadLeft(5))
                lSB.AppendLine(" Crack volume potential of soil: " & Format(lRow.Item("SOL_CRK"), "0.000").PadLeft(5))
                lSB.AppendLine(" Texture 1                : " & lRow.Item("TEXTURE"))

                Dim lNLyrs As Integer = lRow.Item("NLAYERS")

                lSB.Append(" Depth                [mm]:") : AppendN(lSB, lRow, lNLyrs, "SOL_Z")
                lSB.Append(" Bulk Density Moist [g/cc]:") : AppendN(lSB, lRow, lNLyrs, "SOL_BD")
                lSB.Append(" Ave. AW Incl. Rock Frag  :") : AppendN(lSB, lRow, lNLyrs, "SOL_AWC")
                lSB.Append(" Ksat. (est.)      [mm/hr]:")
                For i As Integer = 1 To lNLyrs
                    If lRow.Item(("SOL_K" & Trim(Str(i)))) = 0 Then
                        lSB.Append(Format(0.01, "0.00").PadLeft(12))
                    Else
                        lSB.Append(Format(lRow.Item(("SOL_K" & Trim(Str(i)))), "0.00").PadLeft(12))
                    End If
                Next
                lSB.AppendLine()
                lSB.Append(" Organic Carbon [weight %]:") : AppendN(lSB, lRow, lNLyrs, "SOL_CBN")
                lSB.Append(" Clay           [weight %]:") : AppendN(lSB, lRow, lNLyrs, "CLAY")
                lSB.Append(" Silt           [weight %]:") : AppendN(lSB, lRow, lNLyrs, "SILT")
                lSB.Append(" Sand           [weight %]:") : AppendN(lSB, lRow, lNLyrs, "SAND")
                lSB.Append(" Rock Fragments   [vol. %]:") : AppendN(lSB, lRow, lNLyrs, "ROCK")
                lSB.Append(" Soil Albedo (Moist)      :") : AppendN(lSB, lRow, lNLyrs, "SOL_ALB")
                lSB.Append(" Erosion K                :") : AppendN(lSB, lRow, lNLyrs, "USLE_K")
                lSB.Append(" Salinity (EC, Form 5)    :") : AppendN(lSB, lRow, lNLyrs, "SOL_EC")
                lSB.AppendLine(Space(30))

                IO.File.WriteAllText(pSwatInput.OutputFolder & "\" & lTextFilename, lSB.ToString)
                'ReplaceNonAscii(lTextFilename)
            Next
        End Sub

        Private Sub AppendN(ByVal aSB As Text.StringBuilder, ByVal aRow As DataRow, ByVal aNumber As Integer, ByVal aSection As String)
            For i As Integer = 1 To aNumber
                aSB.Append(Format(aRow.Item(aSection & i), "0.00").PadLeft(12))
            Next
            aSB.AppendLine()
        End Sub
    End Class

    Public Sub createSoilDataTable()
        If IsTableExist("sol", CnSwatInput) Then
            dropTable("sol", CnSwatInput)
        End If
        Me.Sol.TableCreate()
        'Find the state that soil class belongs to
        'TODO: dont hard code this
        Dim htSTMUID_MUID As New Hashtable
        htSTMUID_MUID.Add(1929, 210) 'key is from soil grid, value is OBJECTID in NcMUIDs
        htSTMUID_MUID.Add(1944, 0)
        htSTMUID_MUID.Add(1945, 0)
        htSTMUID_MUID.Add(1950, 0)
        'TODO: dont hard code this!
        Dim conStrUSSoilDB As String = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source=" & "C:\Program Files\SWAT 2005 Editor\Databases\SWAT_US_Soils.mdb"

        Dim rstSol As New DataSet
        Dim strSol As String
        Dim rstUSSoil As New DataSet
        Dim strUSSol As String = ""
        strSol = "SELECT dahrus.SUBBASIN, dahrus.LANDUSE, dahrus.SOIL, dahrus.SLP FROM dahrus;"
        rstSol = FetchData(CnSwatInput, strSol)
        Dim rstSolRowCount As Integer = 0
        Dim SBID As Double = 0.0
        Dim LUClass As String = ""
        Dim SoilClass As String = ""
        Dim oldSoilClass As String = ""
        Dim SlopeClass As String = ""
        Dim hruID As Integer = 0
        Dim subbasinID As Integer = 0
        'Consideration subbasinIDs are starting with 0s.
        Dim oldSubbasinID As Integer = 0
        Dim stmuid As Integer = 0
        Dim oldStmuid As Integer = 0
        Dim state As String = ""
        rstSolRowCount = (rstSol.Tables(0).Rows.Count)
        For solCounter As Integer = 0 To rstSolRowCount - 1
            System.Windows.Forms.Application.DoEvents()
            subbasinID = (rstSol.Tables(0).Rows(solCounter)("SUBBASIN"))
            'Function add HRUs one by one for each subbasin
            If oldSubbasinID = subbasinID Then
                hruID = hruID + 1
            Else
                hruID = 1
                oldSubbasinID = subbasinID
            End If
            LUClass = (rstSol.Tables(0).Rows(solCounter)("LANDUSE"))
            stmuid = Integer.Parse(rstSol.Tables(0).Rows(solCounter)("SOIL"))
            If oldStmuid <> stmuid Then
                SoilClass = htSTMUID_MUID.Item(stmuid).ToString
                oldStmuid = stmuid
            End If
            'Find the STATE that soil Calss belong to
            state = Trim(Left(SoilClass, 1) & Right(Left(SoilClass, 2), 1).ToLower)

            SlopeClass = (rstSol.Tables(0).Rows(solCounter)("SLP"))

            'Soil Table Fields
            'Dim SUBBASIN As Double
            'Dim HRU As Double
            'Dim LANDUSE As String
            'Dim SOIL As String
            'Dim SLOPE_CD As String
            Dim SNAM As String
            Dim NLAYERS As Integer
            Dim HYDGRP As String
            Dim SOL_ZMX As Single
            Dim ANION_EXCL As Single
            Dim SOL_CRK As Single
            Dim TEXTURE As String
            Dim SOL_Z1 As Double
            Dim SOL_BD1 As Double
            Dim SOL_AWC1 As Double
            Dim SOL_K1 As Double
            Dim SOL_CBN1 As Double
            Dim CLAY1 As Double
            Dim SILT1 As Double
            Dim SAND1 As Double
            Dim ROCK1 As Double
            Dim SOL_ALB1 As Double
            Dim USLE_K1 As Double
            Dim SOL_EC1 As Double
            Dim SOL_Z2 As Double
            Dim SOL_BD2 As Double
            Dim SOL_AWC2 As Double
            Dim SOL_K2 As Double
            Dim SOL_CBN2 As Double
            Dim CLAY2 As Double
            Dim SILT2 As Double
            Dim SAND2 As Double
            Dim ROCK2 As Double
            Dim SOL_ALB2 As Double
            Dim USLE_K2 As Double
            Dim SOL_EC2 As Double
            Dim SOL_Z3 As Double
            Dim SOL_BD3 As Double
            Dim SOL_AWC3 As Double
            Dim SOL_K3 As Double
            Dim SOL_CBN3 As Double
            Dim CLAY3 As Double
            Dim SILT3 As Double
            Dim SAND3 As Double
            Dim ROCK3 As Double
            Dim SOL_ALB3 As Double
            Dim USLE_K3 As Double
            Dim SOL_EC3 As Double
            Dim SOL_Z4 As Double
            Dim SOL_BD4 As Double
            Dim SOL_AWC4 As Double
            Dim SOL_K4 As Double
            Dim SOL_CBN4 As Double
            Dim CLAY4 As Double
            Dim SILT4 As Double
            Dim SAND4 As Double
            Dim ROCK4 As Double
            Dim SOL_ALB4 As Double
            Dim USLE_K4 As Double
            Dim SOL_EC4 As Double
            Dim SOL_Z5 As Double
            Dim SOL_BD5 As Double
            Dim SOL_AWC5 As Double
            Dim SOL_K5 As Double
            Dim SOL_CBN5 As Double
            Dim CLAY5 As Double
            Dim SILT5 As Double
            Dim SAND5 As Double
            Dim ROCK5 As Double
            Dim SOL_ALB5 As Double
            Dim USLE_K5 As Double
            Dim SOL_EC5 As Double
            Dim SOL_Z6 As Double
            Dim SOL_BD6 As Double
            Dim SOL_AWC6 As Double
            Dim SOL_K6 As Double
            Dim SOL_CBN6 As Double
            Dim CLAY6 As Double
            Dim SILT6 As Double
            Dim SAND6 As Double
            Dim ROCK6 As Double
            Dim SOL_ALB6 As Double
            Dim USLE_K6 As Double
            Dim SOL_EC6 As Double
            Dim SOL_Z7 As Double
            Dim SOL_BD7 As Double
            Dim SOL_AWC7 As Double
            Dim SOL_K7 As Double
            Dim SOL_CBN7 As Double
            Dim CLAY7 As Double
            Dim SILT7 As Double
            Dim SAND7 As Double
            Dim ROCK7 As Double
            Dim SOL_ALB7 As Double
            Dim USLE_K7 As Double
            Dim SOL_EC7 As Double
            Dim SOL_Z8 As Double
            Dim SOL_BD8 As Double
            Dim SOL_AWC8 As Double
            Dim SOL_K8 As Double
            Dim SOL_CBN8 As Double
            Dim CLAY8 As Double
            Dim SILT8 As Double
            Dim SAND8 As Double
            Dim ROCK8 As Double
            Dim SOL_ALB8 As Double
            Dim USLE_K8 As Double
            Dim SOL_EC8 As Double
            Dim SOL_9 As Double
            Dim SOL_BD9 As Double
            Dim SOL_AWC9 As Double
            Dim SOL_K9 As Double
            Dim SOL_CBN9 As Double
            Dim CLAY9 As Double
            Dim SILT9 As Double
            Dim SAND9 As Double
            Dim ROCK9 As Double
            Dim SOL_ALB9 As Double
            Dim USLE_K9 As Double
            Dim SOL_EC9 As Double
            Dim SOL_Z10 As Double
            Dim SOL_BD10 As Double
            Dim SOL_AWC10 As Double
            Dim SOL_K10 As Double
            Dim SOL_CBN10 As Double
            Dim CLAY10 As Double
            Dim SILT10 As Double
            Dim SAND10 As Double
            Dim ROCK10 As Double
            Dim SOL_ALB10 As Double
            Dim USLE_K10 As Double
            Dim SOL_EC10 As Double

            'Check the new soil class with the old soil class
            If oldSoilClass = SoilClass Then
                Me.Sol.add(subbasinID, hruID, Replace(LUClass, vbCrLf, ""), SoilClass, SlopeClass, SNAM, NLAYERS, HYDGRP, SOL_ZMX, ANION_EXCL, SOL_CRK, TEXTURE, SOL_Z1, SOL_BD1, SOL_AWC1, SOL_K1, SOL_CBN1, CLAY1, SILT1, SAND1, ROCK1, SOL_ALB1, USLE_K1, SOL_EC1, SOL_Z2, SOL_BD2, SOL_AWC2, SOL_K2, SOL_CBN2, CLAY2, SILT2, SAND2, ROCK2, SOL_ALB2, USLE_K2, SOL_EC2, SOL_Z3, SOL_BD3, SOL_AWC3, SOL_K3, SOL_CBN3, CLAY3, SILT3, SAND3, ROCK3, SOL_ALB3, USLE_K3, SOL_EC3, SOL_Z4, SOL_BD4, SOL_AWC4, SOL_K4, SOL_CBN4, CLAY4, SILT4, SAND4, ROCK4, SOL_ALB4, USLE_K4, SOL_EC4, SOL_Z5, SOL_BD5, SOL_AWC5, SOL_K5, SOL_CBN5, CLAY5, SILT5, SAND5, ROCK5, SOL_ALB5, USLE_K5, SOL_EC5, SOL_Z6, SOL_BD6, SOL_AWC6, SOL_K6, SOL_CBN6, CLAY6, SILT6, SAND6, ROCK6, SOL_ALB6, USLE_K6, SOL_EC6, SOL_Z7, SOL_BD7, SOL_AWC7, SOL_K7, SOL_CBN7, CLAY7, SILT7, SAND7, ROCK7, SOL_ALB7, USLE_K7, SOL_EC7, SOL_Z8, SOL_BD8, SOL_AWC8, SOL_K8, SOL_CBN8, CLAY8, SILT8, SAND8, ROCK8, SOL_ALB8, USLE_K8, SOL_EC8, SOL_9, SOL_BD9, SOL_AWC9, SOL_K9, SOL_CBN9, CLAY9, SILT9, SAND9, ROCK9, SOL_ALB9, USLE_K9, SOL_EC9, SOL_Z10, SOL_BD10, SOL_AWC10, SOL_K10, SOL_CBN10, CLAY10, SILT10, SAND10, ROCK10, SOL_ALB10, USLE_K10, SOL_EC10)
            Else
                strUSSol = "SELECT TxMUIDs.MUID, TxMUIDs.SEQN, TxMUIDs.SNAM, TxMUIDs.S5ID, TxMUIDs.CMPPCT, TxMUIDs.NLAYERS, TxMUIDs.HYDGRP, TxMUIDs.SOL_ZMX, TxMUIDs.ANION_EXCL, TxMUIDs.SOL_CRK, TxMUIDs.TEXTURE, TxMUIDs.SOL_Z1, TxMUIDs.SOL_BD1, TxMUIDs.SOL_AWC1, TxMUIDs.SOL_K1, TxMUIDs.SOL_CBN1, TxMUIDs.CLAY1, TxMUIDs.SILT1, TxMUIDs.SAND1, TxMUIDs.ROCK1, TxMUIDs.SOL_ALB1, TxMUIDs.USLE_K1, TxMUIDs.SOL_EC1, TxMUIDs.SOL_Z2, TxMUIDs.SOL_BD2, TxMUIDs.SOL_AWC2, TxMUIDs.SOL_K2, TxMUIDs.SOL_CBN2, TxMUIDs.CLAY2, TxMUIDs.SILT2, TxMUIDs.SAND2, TxMUIDs.ROCK2, TxMUIDs.SOL_ALB2, TxMUIDs.USLE_K2, TxMUIDs.SOL_EC2, TxMUIDs.SOL_Z3, TxMUIDs.SOL_BD3, TxMUIDs.SOL_AWC3, TxMUIDs.SOL_K3, TxMUIDs.SOL_CBN3, TxMUIDs.CLAY3, TxMUIDs.SILT3, TxMUIDs.SAND3, TxMUIDs.ROCK3, TxMUIDs.SOL_ALB3, TxMUIDs.USLE_K3, TxMUIDs.SOL_EC3, TxMUIDs.SOL_Z4, TxMUIDs.SOL_BD4, TxMUIDs.SOL_AWC4, TxMUIDs.SOL_K4, TxMUIDs.SOL_CBN4, TxMUIDs.CLAY4, TxMUIDs.SILT4, TxMUIDs.SAND4, TxMUIDs.ROCK4, TxMUIDs.SOL_ALB4, TxMUIDs.USLE_K4, TxMUIDs.SOL_EC4, TxMUIDs.SOL_Z5, TxMUIDs.SOL_BD5, TxMUIDs.SOL_AWC5, TxMUIDs.SOL_K5, TxMUIDs.SOL_CBN5, TxMUIDs.CLAY5, TxMUIDs.SILT5, TxMUIDs.SAND5, TxMUIDs.ROCK5, TxMUIDs.SOL_ALB5, TxMUIDs.USLE_K5, TxMUIDs.SOL_EC5, TxMUIDs.SOL_Z6, TxMUIDs.SOL_BD6, TxMUIDs.SOL_AWC6, TxMUIDs.SOL_K6, TxMUIDs.SOL_CBN6, TxMUIDs.CLAY6, TxMUIDs.SILT6, TxMUIDs.SAND6, TxMUIDs.ROCK6, TxMUIDs.SOL_ALB6, TxMUIDs.USLE_K6, TxMUIDs.SOL_EC6, TxMUIDs.SOL_Z7, TxMUIDs.SOL_BD7, TxMUIDs.SOL_AWC7, TxMUIDs.SOL_K7, TxMUIDs.SOL_CBN7, TxMUIDs.CLAY7, TxMUIDs.SILT7, TxMUIDs.SAND7, TxMUIDs.ROCK7, TxMUIDs.SOL_ALB7, TxMUIDs.USLE_K7, TxMUIDs.SOL_EC7, TxMUIDs.SOL_Z8, TxMUIDs.SOL_BD8, TxMUIDs.SOL_AWC8, TxMUIDs.SOL_K8, TxMUIDs.SOL_CBN8, TxMUIDs.CLAY8, TxMUIDs.SILT8, TxMUIDs.SAND8, TxMUIDs.ROCK8, TxMUIDs.SOL_ALB8, TxMUIDs.USLE_K8, TxMUIDs.SOL_EC8, TxMUIDs.SOL_Z9, TxMUIDs.SOL_BD9, TxMUIDs.SOL_AWC9, TxMUIDs.SOL_K9, TxMUIDs.SOL_CBN9, TxMUIDs.CLAY9, TxMUIDs.SILT9, TxMUIDs.SAND9, TxMUIDs.ROCK9, TxMUIDs.SOL_ALB9, TxMUIDs.USLE_K9, TxMUIDs.SOL_EC9, TxMUIDs.SOL_Z10, TxMUIDs.SOL_BD10, TxMUIDs.SOL_AWC10, TxMUIDs.SOL_K10, TxMUIDs.SOL_CBN10, TxMUIDs.CLAY10, TxMUIDs.SILT10, TxMUIDs.SAND10, TxMUIDs.ROCK10, TxMUIDs.SOL_ALB10, TxMUIDs.USLE_K10, TxMUIDs.SOL_EC10" & _
                " FROM( " & state & "MUIDs) WHERE TxMUIDs.MUID = '" & Trim(SoilClass) & "' ORDER BY TxMUIDs.CMPPCT DESC;"
                rstUSSoil = FetchData(CnSwatInput, strUSSol)

                'SNAM, S5ID, CMPPCT, NLAYERS, HYDGRP, SOL_ZMX, ANION_EXCL, SOL_CRK, TEXTURE, SOL_Z1, SOL_BD1, SOL_AWC1, SOL_K1, SOL_CBN1, CLAY1, SILT1, SAND1, ROCK1, SOL_ALB1, USLE_K1, SOL_EC1, SOL_Z2, SOL_BD2, SOL_AWC2, SOL_K2, SOL_CBN2, CLAY2, SILT2, SAND2, ROCK2, SOL_ALB2, USLE_K2, SOL_EC2, SOL_Z3, SOL_BD3, SOL_AWC3, SOL_K3, SOL_CBN3, CLAY3, SILT3, SAND3, ROCK3, SOL_ALB3, USLE_K3, SOL_EC3, SOL_Z4, SOL_BD4, SOL_AWC4, SOL_K4, SOL_CBN4, CLAY4, SILT4, SAND4, ROCK4, SOL_ALB4, USLE_K4, SOL_EC4, SOL_Z5, SOL_BD5, SOL_AWC5, SOL_K5, SOL_CBN5, CLAY5, SILT5, SAND5, ROCK5, SOL_ALB5, USLE_K5, SOL_EC5, SOL_Z6, SOL_BD6, SOL_AWC6, SOL_K6, SOL_CBN6, CLAY6, SILT6, SAND6, ROCK6, SOL_ALB6, USLE_K6, SOL_EC6, SOL_Z7, SOL_BD7, SOL_AWC7, SOL_K7, SOL_CBN7, CLAY7, SILT7, SAND7, ROCK7, SOL_ALB7, USLE_K7, SOL_EC7, SOL_Z8, SOL_BD8, SOL_AWC8, SOL_K8, SOL_CBN8, CLAY8, SILT8, SAND8, ROCK8, SOL_ALB8, USLE_K8, SOL_EC8, SOL_Z9, SOL_BD9, SOL_AWC9, SOL_K9, SOL_CBN9, CLAY9, SILT9, SAND9, ROCK9, SOL_ALB9, USLE_K9, SOL_EC9, SOL_Z10, SOL_BD10, SOL_AWC10, SOL_K10, SOL_CBN10, CLAY10, SILT10, SAND10, ROCK10, SOL_ALB10, USLE_K10, SOL_EC10"
                SNAM = rstUSSoil.Tables(0).Rows(0)("SNAM")
                NLAYERS = rstUSSoil.Tables(0).Rows(0)("NLAYERS")
                HYDGRP = rstUSSoil.Tables(0).Rows(0)("HYDGRP")
                SOL_ZMX = rstUSSoil.Tables(0).Rows(0)("SOL_ZMX")
                ANION_EXCL = rstUSSoil.Tables(0).Rows(0)("ANION_EXCL")
                SOL_CRK = rstUSSoil.Tables(0).Rows(0)("SOL_CRK")
                TEXTURE = rstUSSoil.Tables(0).Rows(0)("TEXTURE")
                SOL_Z1 = rstUSSoil.Tables(0).Rows(0)("SOL_Z1")
                SOL_BD1 = rstUSSoil.Tables(0).Rows(0)("SOL_BD1")
                SOL_AWC1 = rstUSSoil.Tables(0).Rows(0)("SOL_AWC1")
                SOL_K1 = rstUSSoil.Tables(0).Rows(0)("SOL_K1")
                SOL_CBN1 = rstUSSoil.Tables(0).Rows(0)("SOL_CBN1")
                CLAY1 = rstUSSoil.Tables(0).Rows(0)("CLAY1")
                SILT1 = rstUSSoil.Tables(0).Rows(0)("SILT1")
                SAND1 = rstUSSoil.Tables(0).Rows(0)("SAND1")
                ROCK1 = rstUSSoil.Tables(0).Rows(0)("ROCK1")
                SOL_ALB1 = rstUSSoil.Tables(0).Rows(0)("SOL_ALB1")
                USLE_K1 = rstUSSoil.Tables(0).Rows(0)("USLE_K1")
                SOL_EC1 = rstUSSoil.Tables(0).Rows(0)("SOL_EC1")
                SOL_Z2 = rstUSSoil.Tables(0).Rows(0)("SOL_Z2")
                SOL_BD2 = rstUSSoil.Tables(0).Rows(0)("SOL_BD2")
                SOL_AWC2 = rstUSSoil.Tables(0).Rows(0)("SOL_AWC2")
                SOL_K2 = rstUSSoil.Tables(0).Rows(0)("SOL_K2")
                SOL_CBN2 = rstUSSoil.Tables(0).Rows(0)("SOL_CBN2")
                CLAY2 = rstUSSoil.Tables(0).Rows(0)("CLAY2")
                SILT2 = rstUSSoil.Tables(0).Rows(0)("SILT2")
                SAND2 = rstUSSoil.Tables(0).Rows(0)("SAND2")
                ROCK2 = rstUSSoil.Tables(0).Rows(0)("ROCK2")
                SOL_ALB2 = rstUSSoil.Tables(0).Rows(0)("SOL_ALB2")
                USLE_K2 = rstUSSoil.Tables(0).Rows(0)("USLE_K2")
                SOL_EC2 = rstUSSoil.Tables(0).Rows(0)("SOL_EC2")
                SOL_Z3 = rstUSSoil.Tables(0).Rows(0)("SOL_Z3")
                SOL_BD3 = rstUSSoil.Tables(0).Rows(0)("SOL_BD3")
                SOL_AWC3 = rstUSSoil.Tables(0).Rows(0)("SOL_AWC3")
                SOL_K3 = rstUSSoil.Tables(0).Rows(0)("SOL_K3")
                SOL_CBN3 = rstUSSoil.Tables(0).Rows(0)("SOL_CBN3")
                CLAY3 = rstUSSoil.Tables(0).Rows(0)("CLAY3")
                SILT3 = rstUSSoil.Tables(0).Rows(0)("SILT3")
                SAND3 = rstUSSoil.Tables(0).Rows(0)("SAND3")
                ROCK3 = rstUSSoil.Tables(0).Rows(0)("ROCK3")
                SOL_ALB3 = rstUSSoil.Tables(0).Rows(0)("SOL_ALB3")
                USLE_K3 = rstUSSoil.Tables(0).Rows(0)("USLE_K3")
                SOL_EC3 = rstUSSoil.Tables(0).Rows(0)("SOL_EC3")
                SOL_Z4 = rstUSSoil.Tables(0).Rows(0)("SOL_Z4")
                SOL_BD4 = rstUSSoil.Tables(0).Rows(0)("SOL_BD4")
                SOL_AWC4 = rstUSSoil.Tables(0).Rows(0)("SOL_AWC4")
                SOL_K4 = rstUSSoil.Tables(0).Rows(0)("SOL_K4")
                SOL_CBN4 = rstUSSoil.Tables(0).Rows(0)("SOL_CBN4")
                CLAY4 = rstUSSoil.Tables(0).Rows(0)("CLAY4")
                SILT4 = rstUSSoil.Tables(0).Rows(0)("SILT4")
                SAND4 = rstUSSoil.Tables(0).Rows(0)("SAND4")
                ROCK4 = rstUSSoil.Tables(0).Rows(0)("ROCK4")
                SOL_ALB4 = rstUSSoil.Tables(0).Rows(0)("SOL_ALB4")
                USLE_K4 = rstUSSoil.Tables(0).Rows(0)("USLE_K4")
                SOL_EC4 = rstUSSoil.Tables(0).Rows(0)("SOL_EC4")
                SOL_Z5 = rstUSSoil.Tables(0).Rows(0)("SOL_Z5")
                SOL_BD5 = rstUSSoil.Tables(0).Rows(0)("SOL_BD5")
                SOL_AWC5 = rstUSSoil.Tables(0).Rows(0)("SOL_AWC5")
                SOL_K5 = rstUSSoil.Tables(0).Rows(0)("SOL_K5")
                SOL_CBN5 = rstUSSoil.Tables(0).Rows(0)("SOL_CBN5")
                CLAY5 = rstUSSoil.Tables(0).Rows(0)("CLAY5")
                SILT5 = rstUSSoil.Tables(0).Rows(0)("SILT5")
                SAND5 = rstUSSoil.Tables(0).Rows(0)("SAND5")
                ROCK5 = rstUSSoil.Tables(0).Rows(0)("ROCK5")
                SOL_ALB5 = rstUSSoil.Tables(0).Rows(0)("SOL_ALB5")
                USLE_K5 = rstUSSoil.Tables(0).Rows(0)("USLE_K5")
                SOL_EC5 = rstUSSoil.Tables(0).Rows(0)("SOL_EC5")
                SOL_Z6 = rstUSSoil.Tables(0).Rows(0)("SOL_Z6")
                SOL_BD6 = rstUSSoil.Tables(0).Rows(0)("SOL_BD6")
                SOL_AWC6 = rstUSSoil.Tables(0).Rows(0)("SOL_AWC6")
                SOL_K6 = rstUSSoil.Tables(0).Rows(0)("SOL_K6")
                SOL_CBN6 = rstUSSoil.Tables(0).Rows(0)("SOL_CBN6")
                CLAY6 = rstUSSoil.Tables(0).Rows(0)("CLAY6")
                SILT6 = rstUSSoil.Tables(0).Rows(0)("SILT6")
                SAND6 = rstUSSoil.Tables(0).Rows(0)("SAND6")
                ROCK6 = rstUSSoil.Tables(0).Rows(0)("ROCK6")
                SOL_ALB6 = rstUSSoil.Tables(0).Rows(0)("SOL_ALB6")
                USLE_K6 = rstUSSoil.Tables(0).Rows(0)("USLE_K6")
                SOL_EC6 = rstUSSoil.Tables(0).Rows(0)("SOL_EC6")
                SOL_Z7 = rstUSSoil.Tables(0).Rows(0)("SOL_Z7")
                SOL_BD7 = rstUSSoil.Tables(0).Rows(0)("SOL_BD7")
                SOL_AWC7 = rstUSSoil.Tables(0).Rows(0)("SOL_AWC7")
                SOL_K7 = rstUSSoil.Tables(0).Rows(0)("SOL_K7")
                SOL_CBN7 = rstUSSoil.Tables(0).Rows(0)("SOL_CBN7")
                CLAY7 = rstUSSoil.Tables(0).Rows(0)("CLAY7")
                SILT7 = rstUSSoil.Tables(0).Rows(0)("SILT7")
                SAND7 = rstUSSoil.Tables(0).Rows(0)("SAND7")
                ROCK7 = rstUSSoil.Tables(0).Rows(0)("ROCK7")
                SOL_ALB7 = rstUSSoil.Tables(0).Rows(0)("SOL_ALB7")
                USLE_K7 = rstUSSoil.Tables(0).Rows(0)("USLE_K7")
                SOL_EC7 = rstUSSoil.Tables(0).Rows(0)("SOL_EC7")
                SOL_Z8 = rstUSSoil.Tables(0).Rows(0)("SOL_Z8")
                SOL_BD8 = rstUSSoil.Tables(0).Rows(0)("SOL_BD8")
                SOL_AWC8 = rstUSSoil.Tables(0).Rows(0)("SOL_AWC8")
                SOL_K8 = rstUSSoil.Tables(0).Rows(0)("SOL_K8")
                SOL_CBN8 = rstUSSoil.Tables(0).Rows(0)("SOL_CBN8")
                CLAY8 = rstUSSoil.Tables(0).Rows(0)("CLAY8")
                SILT8 = rstUSSoil.Tables(0).Rows(0)("SILT8")
                SAND8 = rstUSSoil.Tables(0).Rows(0)("SAND8")
                ROCK8 = rstUSSoil.Tables(0).Rows(0)("ROCK8")
                SOL_ALB8 = rstUSSoil.Tables(0).Rows(0)("SOL_ALB8")
                USLE_K8 = rstUSSoil.Tables(0).Rows(0)("USLE_K8")
                SOL_EC8 = rstUSSoil.Tables(0).Rows(0)("SOL_EC8")
                'This is field is not consistent with other fields
                SOL_9 = rstUSSoil.Tables(0).Rows(0)("SOL_Z9")
                SOL_BD9 = rstUSSoil.Tables(0).Rows(0)("SOL_BD9")
                SOL_AWC9 = rstUSSoil.Tables(0).Rows(0)("SOL_AWC9")
                SOL_K9 = rstUSSoil.Tables(0).Rows(0)("SOL_K9")
                SOL_CBN9 = rstUSSoil.Tables(0).Rows(0)("SOL_CBN9")
                CLAY9 = rstUSSoil.Tables(0).Rows(0)("CLAY9")
                SILT9 = rstUSSoil.Tables(0).Rows(0)("SILT9")
                SAND9 = rstUSSoil.Tables(0).Rows(0)("SAND9")
                ROCK9 = rstUSSoil.Tables(0).Rows(0)("ROCK9")
                SOL_ALB9 = rstUSSoil.Tables(0).Rows(0)("SOL_ALB9")
                USLE_K9 = rstUSSoil.Tables(0).Rows(0)("USLE_K9")
                SOL_EC9 = rstUSSoil.Tables(0).Rows(0)("SOL_EC9")
                SOL_Z10 = rstUSSoil.Tables(0).Rows(0)("SOL_Z10")
                SOL_BD10 = rstUSSoil.Tables(0).Rows(0)("SOL_BD10")
                SOL_AWC10 = rstUSSoil.Tables(0).Rows(0)("SOL_AWC10")
                SOL_K10 = rstUSSoil.Tables(0).Rows(0)("SOL_K10")
                SOL_CBN10 = rstUSSoil.Tables(0).Rows(0)("SOL_CBN10")
                CLAY10 = rstUSSoil.Tables(0).Rows(0)("CLAY10")
                SILT10 = rstUSSoil.Tables(0).Rows(0)("SILT10")
                SAND10 = rstUSSoil.Tables(0).Rows(0)("SAND10")
                ROCK10 = rstUSSoil.Tables(0).Rows(0)("ROCK10")
                SOL_ALB10 = rstUSSoil.Tables(0).Rows(0)("SOL_ALB10")
                USLE_K10 = rstUSSoil.Tables(0).Rows(0)("USLE_K10")
                SOL_EC10 = rstUSSoil.Tables(0).Rows(0)("SOL_EC10")

                pSol.Add(subbasinID, hruID, Replace(LUClass, vbCrLf, ""), SoilClass, SlopeClass, SNAM, NLAYERS, HYDGRP, SOL_ZMX, ANION_EXCL, SOL_CRK, TEXTURE, SOL_Z1, SOL_BD1, SOL_AWC1, SOL_K1, SOL_CBN1, CLAY1, SILT1, SAND1, ROCK1, SOL_ALB1, USLE_K1, SOL_EC1, SOL_Z2, SOL_BD2, SOL_AWC2, SOL_K2, SOL_CBN2, CLAY2, SILT2, SAND2, ROCK2, SOL_ALB2, USLE_K2, SOL_EC2, SOL_Z3, SOL_BD3, SOL_AWC3, SOL_K3, SOL_CBN3, CLAY3, SILT3, SAND3, ROCK3, SOL_ALB3, USLE_K3, SOL_EC3, SOL_Z4, SOL_BD4, SOL_AWC4, SOL_K4, SOL_CBN4, CLAY4, SILT4, SAND4, ROCK4, SOL_ALB4, USLE_K4, SOL_EC4, SOL_Z5, SOL_BD5, SOL_AWC5, SOL_K5, SOL_CBN5, CLAY5, SILT5, SAND5, ROCK5, SOL_ALB5, USLE_K5, SOL_EC5, SOL_Z6, SOL_BD6, SOL_AWC6, SOL_K6, SOL_CBN6, CLAY6, SILT6, SAND6, ROCK6, SOL_ALB6, USLE_K6, SOL_EC6, SOL_Z7, SOL_BD7, SOL_AWC7, SOL_K7, SOL_CBN7, CLAY7, SILT7, SAND7, ROCK7, SOL_ALB7, USLE_K7, SOL_EC7, SOL_Z8, SOL_BD8, SOL_AWC8, SOL_K8, SOL_CBN8, CLAY8, SILT8, SAND8, ROCK8, SOL_ALB8, USLE_K8, SOL_EC8, SOL_9, SOL_BD9, SOL_AWC9, SOL_K9, SOL_CBN9, CLAY9, SILT9, SAND9, ROCK9, SOL_ALB9, USLE_K9, SOL_EC9, SOL_Z10, SOL_BD10, SOL_AWC10, SOL_K10, SOL_CBN10, CLAY10, SILT10, SAND10, ROCK10, SOL_ALB10, USLE_K10, SOL_EC10)

                oldSoilClass = SoilClass
            End If
        Next
    End Sub

    Public Function FetchData(ByVal aConnection As OleDb.OleDbConnection, ByVal QueryString As String) As DataSet

        'Define the connectors
        Dim oComm As OleDbCommand
        Dim oData As OleDbDataAdapter
        Dim resultSet As New DataSet
        Dim oQuery As String


        'Query String
        oQuery = QueryString

        'Instantiate the connectors
        oComm = New OleDbCommand(oQuery, aConnection)
        oData = New OleDbDataAdapter(oQuery, aConnection)

        Try

            'Fill dataset
            oData.Fill(resultSet)

        Catch ex As OleDb.OleDbException
        Catch ex As Exception
            MapWinUtility.Logger.Dbg(ex.Message & vbCrLf & ex.StackTrace)
        End Try

        'Return results
        Return resultSet

    End Function

    'Function find whether a table exist or not
    Private Function IsTableExist(ByVal strTable As String, ByVal aConnection As OleDb.OleDbConnection) As Boolean
        Try

            Dim schemaTable As DataTable = aConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
            Dim I As Int32
            For I = 0 To schemaTable.Rows.Count - 1
                Dim rd As DataRow = schemaTable.Rows(I)
                If rd("TABLE_TYPE").ToString = "TABLE" Then
                    If (rd("TABLE_NAME").ToString) = strTable Then
                        'Table exists
                        IsTableExist = True
                        Exit For
                    Else
                        'Table not exists
                        IsTableExist = False
                    End If
                End If
            Next
        Catch ex As Exception
            MapWinUtility.Logger.Dbg(ex.Message)
        End Try
    End Function

    Private Function dropTable(ByVal tableName As String, ByVal aConnection As OleDb.OleDbConnection) As Boolean
        If IsTableExist(tableName, aConnection) Then
            Dim SQL As String
            Dim objCmd As New OleDbCommand

            SQL = "DROP TABLE " + "" & tableName & ""

            objCmd = New OleDbCommand(SQL, aConnection)
            objCmd.ExecuteNonQuery()
        End If
    End Function
End Class
