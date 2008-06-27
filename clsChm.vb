Partial Class SwatInput
    Private pChm As clsChm = New clsChm(Me)
    ReadOnly Property Chm() As clsChm
        Get
            Return pChm
        End Get
    End Property

    Public Class clsChmItem
        Public SUBBASIN As Double
        Public HRU As Double
        Public LANDUSE As String
        Public SOIL As String
        Public SLOPE_CD As String
        Public SOL_NO3(9) As Double
        Public SOL_ORGN(9) As Double
        Public SOL_LABP(9) As Double
        Public SOL_ORGP(9) As Double
        Public PESTNAME(9) As String
        Public PLTPST(9) As Double
        Public SOLPST(9) As Double
        Public PSTENR(9) As Double

        Public Sub New(ByVal aSUBBASIN As Double, _
                  ByVal aHRU As Double)
            SUBBASIN = aSUBBASIN
            HRU = aHRU
        End Sub

        Public Sub New(ByVal aSUBBASIN As Double, _
                          ByVal aHRU As Double, _
                          ByVal aLANDUSE As String, _
                          ByVal aSOIL As String, _
                          ByVal aSLOPE_CD As String, _
                          ByVal aSOL_NO3() As Double, _
                          ByVal aSOL_ORGN() As Double, _
                          ByVal aSOL_LABP() As Double, _
                          ByVal aSOL_ORGP() As Double, _
                          ByVal aPESTNAME() As String, _
                          ByVal aPLTPST() As Double, _
                          ByVal aSOLPST() As Double, _
                          ByVal aPSTENR() As Double)
            SUBBASIN = aSUBBASIN
            HRU = aHRU
            LANDUSE = aLANDUSE
            SOIL = aSOIL
            SLOPE_CD = aSLOPE_CD
            SOL_NO3 = aSOL_NO3
            SOL_ORGN = aSOL_ORGN
            SOL_LABP = aSOL_LABP
            SOL_ORGP = aSOL_ORGP
            PESTNAME = aPESTNAME
            PLTPST = aPLTPST
            SOLPST = aSOLPST
            PSTENR = aPSTENR
        End Sub
    End Class

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

        Public Sub add(ByVal aChmItem As clsChmItem)
            With aChmItem
                Me.add(.SUBBASIN, .HRU, .LANDUSE, .SOIL, .SLOPE_CD, _
                       .SOL_NO3(0), .SOL_NO3(1), .SOL_NO3(2), .SOL_NO3(3), .SOL_NO3(4), .SOL_NO3(5), .SOL_NO3(6), .SOL_NO3(7), .SOL_NO3(8), .SOL_NO3(9), _
                       .SOL_ORGN(0), .SOL_ORGN(1), .SOL_ORGN(2), .SOL_ORGN(3), .SOL_ORGN(4), .SOL_ORGN(5), .SOL_ORGN(6), .SOL_ORGN(7), .SOL_ORGN(8), .SOL_ORGN(9), _
                       .SOL_LABP(0), .SOL_LABP(1), .SOL_LABP(2), .SOL_LABP(3), .SOL_LABP(4), .SOL_LABP(5), .SOL_LABP(6), .SOL_LABP(7), .SOL_LABP(8), .SOL_LABP(9), _
                       .SOL_ORGP(0), .SOL_ORGP(1), .SOL_ORGP(2), .SOL_ORGP(3), .SOL_ORGP(4), .SOL_ORGP(5), .SOL_ORGP(6), .SOL_ORGP(7), .SOL_ORGP(8), .SOL_ORGP(9), _
                       .PESTNAME(0), .PESTNAME(1), .PESTNAME(2), .PESTNAME(3), .PESTNAME(4), .PESTNAME(5), .PESTNAME(6), .PESTNAME(7), .PESTNAME(8), .PESTNAME(9), _
                       .PLTPST(0), .PLTPST(1), .PLTPST(2), .PLTPST(3), .PLTPST(4), .PLTPST(5), .PLTPST(6), .PLTPST(7), .PLTPST(8), .PLTPST(9), _
                       .SOLPST(0), .SOLPST(1), .SOLPST(2), .SOLPST(3), .SOLPST(4), .SOLPST(5), .SOLPST(6), .SOLPST(7), .SOLPST(8), .SOLPST(9), _
                       .PSTENR(0), .PSTENR(1), .PSTENR(2), .PSTENR(3), .PSTENR(4), .PSTENR(5), .PSTENR(6), .PSTENR(7), .PSTENR(8), .PSTENR(9))
            End With
        End Sub

        Private Sub Add(ByVal SUBBASIN As Double, _
                        ByVal HRU As Double, _
                        ByVal LANDUSE As String, _
                        ByVal SOIL As String, _
                        ByVal SLOPE_CD As String, _
                        ByVal SOL_NO31 As Double, _
                        ByVal SOL_NO32 As Double, _
                        ByVal SOL_NO33 As Double, _
                        ByVal SOL_NO34 As Double, _
                        ByVal SOL_NO35 As Double, _
                        ByVal SOL_NO36 As Double, _
                        ByVal SOL_NO37 As Double, _
                        ByVal SOL_NO38 As Double, _
                        ByVal SOL_NO39 As Double, _
                        ByVal SOL_NO310 As Double, _
                        ByVal SOL_ORGN1 As Double, _
                        ByVal SOL_ORGN2 As Double, _
                        ByVal SOL_ORGN3 As Double, _
                        ByVal SOL_ORGN4 As Double, _
                        ByVal SOL_ORGN5 As Double, _
                        ByVal SOL_ORGN6 As Double, _
                        ByVal SOL_ORGN7 As Double, _
                        ByVal SOL_ORGN8 As Double, _
                        ByVal SOL_ORGN9 As Double, _
                        ByVal SOL_ORGN10 As Double, _
                        ByVal SOL_LABP1 As Double, _
                        ByVal SOL_LABP2 As Double, _
                        ByVal SOL_LABP3 As Double, _
                        ByVal SOL_LABP4 As Double, _
                        ByVal SOL_LABP5 As Double, _
                        ByVal SOL_LABP6 As Double, _
                        ByVal SOL_LABP7 As Double, _
                        ByVal SOL_LABP8 As Double, _
                        ByVal SOL_LABP9 As Double, _
                        ByVal SOL_LABP10 As Double, _
                        ByVal SOL_ORGP1 As Double, _
                        ByVal SOL_ORGP2 As Double, _
                        ByVal SOL_ORGP3 As Double, _
                        ByVal SOL_ORGP4 As Double, _
                        ByVal SOL_ORGP5 As Double, _
                        ByVal SOL_ORGP6 As Double, _
                        ByVal SOL_ORGP7 As Double, _
                        ByVal SOL_ORGP8 As Double, _
                        ByVal SOL_ORGP9 As Double, _
                        ByVal SOL_ORGP10 As Double, _
                        ByVal PESTNAME1 As String, _
                        ByVal PESTNAME2 As String, _
                        ByVal PESTNAME3 As String, _
                        ByVal PESTNAME4 As String, _
                        ByVal PESTNAME5 As String, _
                        ByVal PESTNAME6 As String, _
                        ByVal PESTNAME7 As String, _
                        ByVal PESTNAME8 As String, _
                        ByVal PESTNAME9 As String, _
                        ByVal PESTNAME10 As String, _
                        ByVal PLTPST1 As Double, _
                        ByVal PLTPST2 As Double, _
                        ByVal PLTPST3 As Double, _
                        ByVal PLTPST4 As Double, _
                        ByVal PLTPST5 As Double, _
                        ByVal PLTPST6 As Double, _
                        ByVal PLTPST7 As Double, _
                        ByVal PLTPST8 As Double, _
                        ByVal PLTPST9 As Double, _
                        ByVal PLTPST10 As Double, _
                        ByVal SOLPST1 As Double, _
                        ByVal SOLPST2 As Double, _
                        ByVal SOLPST3 As Double, _
                        ByVal SOLPST4 As Double, _
                        ByVal SOLPST5 As Double, _
                        ByVal SOLPST6 As Double, _
                        ByVal SOLPST7 As Double, _
                        ByVal SOLPST8 As Double, _
                        ByVal SOLPST9 As Double, _
                        ByVal SOLPST10 As Double, _
                        ByVal PSTENR1 As Double, _
                        ByVal PSTENR2 As Double, _
                        ByVal PSTENR3 As Double, _
                        ByVal PSTENR4 As Double, _
                        ByVal PSTENR5 As Double, _
                        ByVal PSTENR6 As Double, _
                        ByVal PSTENR7 As Double, _
                        ByVal PSTENR8 As Double, _
                        ByVal PSTENR9 As Double, _
                        ByVal PSTENR10 As Double)

            Dim lSQL As String = "INSERT INTO chm ( SUBBASIN , HRU , LANDUSE , SOIL , SLOPE_CD , SOL_NO31 , SOL_NO32 , SOL_NO33 , SOL_NO34 , SOL_NO35 , SOL_NO36 , SOL_NO37 , SOL_NO38 , SOL_NO39 , SOL_NO310 , SOL_ORGN1 , SOL_ORGN2 , SOL_ORGN3 , SOL_ORGN4 , SOL_ORGN5 , SOL_ORGN6 , SOL_ORGN7 , SOL_ORGN8 , SOL_ORGN9 , SOL_ORGN10 , SOL_LABP1 , SOL_LABP2 , SOL_LABP3 , SOL_LABP4 , SOL_LABP5 , SOL_LABP6 , SOL_LABP7 , SOL_LABP8 , SOL_LABP9 , SOL_LABP10 , SOL_ORGP1 , SOL_ORGP2 , SOL_ORGP3 , SOL_ORGP4 , SOL_ORGP5 , SOL_ORGP6 , SOL_ORGP7 , SOL_ORGP8 , SOL_ORGP9 , SOL_ORGP10 , PESTNAME1 , PESTNAME2 , PESTNAME3 , PESTNAME4 , PESTNAME5 , PESTNAME6 , PESTNAME7 , PESTNAME8 , PESTNAME9 , PESTNAME10 , PLTPST1 , PLTPST2 , PLTPST3 , PLTPST4 , PLTPST5 , PLTPST6 , PLTPST7 , PLTPST8 , PLTPST9 , PLTPST10 , SOLPST1 , SOLPST2 , SOLPST3 , SOLPST4 , SOLPST5 , SOLPST6 , SOLPST7 , SOLPST8 , SOLPST9 , SOLPST10 , PSTENR1 , PSTENR2 , PSTENR3 , PSTENR4 , PSTENR5 , PSTENR6 , PSTENR7 , PSTENR8 , PSTENR9 , PSTENR10  ) " _
                               & "Values ('" & SUBBASIN & "'  ,'" & HRU & "'  ,'" & LANDUSE & "'  ,'" & SOIL & "'  ,'" & SLOPE_CD & "'  ,'" & SOL_NO31 & "'  ,'" & SOL_NO32 & "'  ,'" & SOL_NO33 & "'  ,'" & SOL_NO34 & "'  ,'" & SOL_NO35 & "'  ,'" & SOL_NO36 & "'  ,'" & SOL_NO37 & "'  ,'" & SOL_NO38 & "'  ,'" & SOL_NO39 & "'  ,'" & SOL_NO310 & "'  ,'" & SOL_ORGN1 & "'  ,'" & SOL_ORGN2 & "'  ,'" & SOL_ORGN3 & "'  ,'" & SOL_ORGN4 & "'  ,'" & SOL_ORGN5 & "'  ,'" & SOL_ORGN6 & "'  ,'" & SOL_ORGN7 & "'  ,'" & SOL_ORGN8 & "'  ,'" & SOL_ORGN9 & "'  ,'" & SOL_ORGN10 & "'  ,'" & SOL_LABP1 & "'  ,'" & SOL_LABP2 & "'  ,'" & SOL_LABP3 & "'  ,'" & SOL_LABP4 & "'  ,'" & SOL_LABP5 & "'  ,'" & SOL_LABP6 & "'  ,'" & SOL_LABP7 & "'  ,'" & SOL_LABP8 & "'  ,'" & SOL_LABP9 & "'  ,'" & SOL_LABP10 & "'  ,'" & SOL_ORGP1 & "'  ,'" & SOL_ORGP2 & "'  ,'" & SOL_ORGP3 & "'  ,'" & SOL_ORGP4 & "'  ,'" & SOL_ORGP5 & "'  ,'" & SOL_ORGP6 & "'  ,'" & SOL_ORGP7 & "'  ,'" & SOL_ORGP8 & "'  ,'" & SOL_ORGP9 & "'  ,'" & SOL_ORGP10 & "'  ,'" & PESTNAME1 & "'  ,'" & PESTNAME2 & "'  ,'" & PESTNAME3 & "'  ,'" & PESTNAME4 & "'  ,'" & PESTNAME5 & "'  ,'" & PESTNAME6 & "'  ,'" & PESTNAME7 & "'  ,'" & PESTNAME8 & "'  ,'" & PESTNAME9 & "'  ,'" & PESTNAME10 & "'  ,'" & PLTPST1 & "'  ,'" & PLTPST2 & "'  ,'" & PLTPST3 & "'  ,'" & PLTPST4 & "'  ,'" & PLTPST5 & "'  ,'" & PLTPST6 & "'  ,'" & PLTPST7 & "'  ,'" & PLTPST8 & "'  ,'" & PLTPST9 & "'  ,'" & PLTPST10 & "'  ,'" & SOLPST1 & "'  ,'" & SOLPST2 & "'  ,'" & SOLPST3 & "'  ,'" & SOLPST4 & "'  ,'" & SOLPST5 & "'  ,'" & SOLPST6 & "'  ,'" & SOLPST7 & "'  ,'" & SOLPST8 & "'  ,'" & SOLPST9 & "'  ,'" & SOLPST10 & "'  ,'" & PSTENR1 & "'  ,'" & PSTENR2 & "'  ,'" & PSTENR3 & "'  ,'" & PSTENR4 & "'  ,'" & PSTENR5 & "'  ,'" & PSTENR6 & "'  ,'" & PSTENR7 & "'  ,'" & PSTENR8 & "'  ,'" & PSTENR9 & "'  ,'" & PSTENR10 & "'  )"
            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

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

                lSB.Append(" Soil NO3 [mg/kg]         :") : Append10TextColumns(lSB, lRow, "SOL_NO3")  '4th line
                lSB.Append(" Soil organic N [mg/kg]   :") : Append10TextColumns(lSB, lRow, "SOL_ORGN") '5th Line
                lSB.Append(" Soil labile P [mg/kg]    :") : Append10TextColumns(lSB, lRow, "SOL_LABP") '6th Line
                lSB.Append(" Soil organic P [mg/kg]   :") : Append10TextColumns(lSB, lRow, "SOL_ORGP") '7th Line

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

        Private Sub Append10TextColumns(ByVal aSB As Text.StringBuilder, ByVal aRow As DataRow, ByVal aSection As String)
            For i As Integer = 1 To 10
                aSB.Append(Format(aRow.Item(aSection & i), "0.00").PadLeft(12))
            Next
            aSB.AppendLine()
        End Sub

    End Class
End Class
