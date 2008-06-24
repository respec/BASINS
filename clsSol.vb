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
End Class
