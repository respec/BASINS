Partial Class SwatInput
    Private pChm As clsChm = New clsChm(Me)
    ReadOnly Property Chm() As clsChm
        Get
            Return pChm
        End Get
    End Property

    ''' <summary>
    ''' Chm Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsChm
        Private pSwatInput As SwatInput
        Private pTableName As String = "chm"

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn.DBLayer.createChmTable
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
                    .Append("HRU", ADOX.DataTypeEnum.adDouble)
                    .Append("LANDUSE", ADOX.DataTypeEnum.adVarWChar, 4)
                    .Append("SOIL", ADOX.DataTypeEnum.adVarWChar, 40)
                    .Append("SLOPE_CD", ADOX.DataTypeEnum.adVarWChar, 20)
                    For i = 1 To 10
                        .Append("SOL_NO3" & i, ADOX.DataTypeEnum.adDouble)
                    Next
                    For i = 1 To 10
                        .Append("SOL_ORGN" & i, ADOX.DataTypeEnum.adDouble)
                    Next
                    For i = 1 To 10
                        .Append("SOL_LABP" & i, ADOX.DataTypeEnum.adDouble)
                    Next
                    For i = 1 To 10
                        .Append("SOL_ORGP" & i, ADOX.DataTypeEnum.adDouble)
                    Next
                    For i = 1 To 10
                        .Append("PESTNAME" & i, ADOX.DataTypeEnum.adVarWChar, 16)
                    Next
                    For i = 1 To 10
                        .Append("PLTPST" & i, ADOX.DataTypeEnum.adDouble)
                    Next
                    For i = 1 To 10
                        .Append("SOLPST" & i, ADOX.DataTypeEnum.adDouble)
                    Next
                    For i = 1 To 10
                        .Append("PSTENR" & i, ADOX.DataTypeEnum.adDouble)
                    Next
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

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing, Optional ByVal aPestTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            If aPestTable Is Nothing Then aPestTable = pSwatInput.QueryGDB("SELECT * FROM pest;")

            pSwatInput.Status("Writing " & pTableName & " text ...")

            'fill pest dictionary
            Dim dictPest As New Collections.Generic.Dictionary(Of String, String)
            For Each lRow As DataRow In aPestTable.Rows
                dictPest.Add(lRow.Item("PESTNAME"), lRow.Item("IPNUM"))
            Next

            For Each lRow As DataRow In aTable.Rows
                Dim lSubNum As Integer = lRow.Item(1)
                Dim sSubNum As String = lSubNum
                Dim lHruNum As Integer = lRow.Item(2)
                Dim sHruNum As String = lHruNum

                Dim lTextFilename As String = StringFnameHRUs(sSubNum, lHruNum) & "." & pTableName

                Dim lSB As New Text.StringBuilder
                lSB.AppendLine(" .chm file Subbasin:" & sSubNum & " HRU:" & sHruNum & " Luse:" & lRow.Item(3) _
                             & " Soil: " & lRow.Item(4) & " Slope: " & lRow.Item(5) _
                             & " " & DateNowString() & " ARCGIS-SWAT interface MAVZ")
                lSB.AppendLine("Soil Nutrient Data")
                lSB.AppendLine(" Soil Layer               :           1           2           3           4           5           6           7           8           9          10")

                lSB.Append(" Soil NO3 [mg/kg]         :") : Append10(lSB, lRow, "SOL_NO3")  '4th line
                lSB.Append(" Soil organic N [mg/kg]   :") : Append10(lSB, lRow, "SOL_ORGN") '5th Line
                lSB.Append(" Soil labile P [mg/kg]    :") : Append10(lSB, lRow, "SOL_LABP") '6th Line
                lSB.Append(" Soil organic P [mg/kg]   :") : Append10(lSB, lRow, "SOL_ORGP") '7th Line

                lSB.AppendLine()
                lSB.AppendLine("Soil Pesticide Data")
                lSB.AppendLine(" Pesticide  Pst on plant    Pst in 1st soil layer Pst enrichment")
                lSB.AppendLine("   #           [kg/ha]           [kg/ha]           [kg/ha]")
                For i As Integer = 1 To 10
                    If lRow.Item("PESTNAME" & i) Is System.DBNull.Value Then
                        lSB.AppendLine("   0" & "0.00".PadLeft(18) & "0.00".PadLeft(18) & "0.00".PadLeft(18))
                    Else
                        lSB.AppendLine(dictPest.Item(lRow.Item("PESTNAME" & i)).PadLeft(4) _
                                     & Format(lRow.Item("PLTPST" & i), "0.00").PadLeft(18) _
                                     & Format(lRow.Item("SOLPST" & i), "0.00").PadLeft(18) _
                                     & Format(lRow.Item("PSTENR" & i), "0.00").PadLeft(18))
                    End If
                Next

                IO.File.WriteAllText(pSwatInput.OutputFolder & "\" & lTextFilename, lSB.ToString)
                'ReplaceNonAscii(lTextFilename)
            Next
        End Sub

        Private Sub Append10(ByVal aSB As Text.StringBuilder, ByVal aRow As DataRow, ByVal aSection As String)
            For i As Integer = 1 To 10
                aSB.Append(Format(aRow.Item(aSection & i), "0.00").PadLeft(12))
            Next
            aSB.AppendLine()
        End Sub

    End Class
End Class
