Partial Class SwatInput
    Private pSol As clsSol = New clsSol(Me)
    ReadOnly Property Sol() As clsSol
        Get
            Return pSol
        End Get
    End Property

    Public Class clsSolItem
        Public SUBBASIN As Double
        Public HRU As Double
        Public LANDUSE As String
        Public SOIL As String
        Public SLOPE_CD As String
        Public SNAM As String
        Public NLAYERS As Integer
        Public HYDGRP As String
        Public SOL_ZMX As Single
        Public ANION_EXCL As Single
        Public SOL_CRK As Single
        Public TEXTURE As String
        Public SOL_Z(9) As Double
        Public SOL_BD(9) As Double
        Public SOL_AWC(9) As Double
        Public SOL_K(9) As Double
        Public SOL_CBN(9) As Double
        Public CLAY(9) As Double
        Public SILT(9) As Double
        Public SAND(9) As Double
        Public ROCK(9) As Double
        Public SOL_ALB(9) As Double
        Public USLE_K(9) As Double
        Public SOL_EC(9) As Double

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

        Public Sub New(ByVal aDataRow As DataRow)
            With aDataRow
                SNAM = .Item("SNAM")
                NLAYERS = .Item("NLAYERS")
                HYDGRP = .Item("HYDGRP")
                SOL_ZMX = .Item("SOL_ZMX")
                ANION_EXCL = .Item("ANION_EXCL")
                SOL_CRK = .Item("SOL_CRK")
                TEXTURE = .Item("TEXTURE")
                For i As Integer = 0 To 9
                    Try
                        SOL_Z(i) = .Item("SOL_Z" & i + 1)
                    Catch
                        SOL_Z(i) = .Item("SOL_" & i + 1)
                    End Try
                    SOL_BD(i) = .Item("SOL_BD" & i + 1)
                    SOL_AWC(i) = .Item("SOL_AWC" & i + 1)
                    SOL_K(i) = .Item("SOL_K" & i + 1)
                    SOL_CBN(i) = .Item("SOL_CBN" & i + 1)
                    CLAY(i) = .Item("CLAY" & i + 1)
                    SILT(i) = .Item("SILT" & i + 1)
                    SAND(i) = .Item("SAND" & i + 1)
                    ROCK(i) = .Item("ROCK" & i + 1)
                    SOL_ALB(i) = .Item("SOL_ALB" & i + 1)
                    USLE_K(i) = .Item("USLE_K" & i + 1)
                    SOL_EC(i) = .Item("SOL_EC" & i + 1)
                Next
            End With
        End Sub

        Public Function AddSQL() As String
            Dim lSQL As String = "INSERT INTO sol ( SUBBASIN, HRU, LANDUSE, SOIL, SLOPE_CD, SNAM, NLAYERS, HYDGRP, SOL_ZMX, ANION_EXCL, SOL_CRK, TEXTURE"
            Dim i As Integer
            For i = 1 To 10
                lSQL &= ", SOL_Z" & i & ", SOL_BD" & i & ", SOL_AWC" & i & ", SOL_K" & i & ", SOL_CBN" & i & ", CLAY" & i & ", SILT" & i & ", SAND" & i & ", ROCK" & i & ", SOL_ALB" & i & ", USLE_K" & i & ", SOL_EC" & i
            Next
            lSQL &= ") Values ( '" & SUBBASIN & "' ,'" & HRU & "' ,'" & LANDUSE & "' ,'" & SOIL & "' ,'" & SLOPE_CD & "' ,'" & SNAM & "' ,'" & NLAYERS & "' ,'" & HYDGRP & "' ,'" & SOL_ZMX & "' ,'" & ANION_EXCL & "' ,'" & SOL_CRK & "' ,'" & TEXTURE
            For i = 0 To 9
                lSQL &= "' ,'" & SOL_Z(i) & "' ,'" & SOL_BD(i) & "' ,'" & SOL_AWC(i) & "' ,'" & SOL_K(i) & "' ,'" & SOL_CBN(i) & "' ,'" & CLAY(i) & "' ,'" & SILT(i) & "' ,'" & SAND(i) & "' ,'" & ROCK(i) & "' ,'" & SOL_ALB(i) & "' ,'" & USLE_K(i) & "' ,'" & SOL_EC(i)
            Next
            Return lSQL & "')"
        End Function
    End Class

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
                DropTable(pTableName, pSwatInput.CnSwatInput)

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
                    Append10DBColumnsDouble(lTable.Columns, _
                                            "SOL_Z", _
                                            "SOL_BD", _
                                            "SOL_AWC", _
                                            "SOL_K", _
                                            "SOL_CBN", _
                                            "CLAY", _
                                            "SILT", _
                                            "SAND", _
                                            "ROCK", _
                                            "SOL_ALB", _
                                            "USLE_K", _
                                            "SOL_EC")
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

        Private Sub Append10DBColumnsDouble(ByVal aColumns As ADOX.Columns, ByVal ParamArray aSections() As String)
            For i As Integer = 1 To 10
                For Each lSection As String In aSections
                    aColumns.Append(lSection & i, ADOX.DataTypeEnum.adDouble)
                Next
            Next
        End Sub

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & " ORDER BY SUBBASIN, HRU;")
        End Function

        Public Sub Add(ByVal aItem As clsSolItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatInput)
        End Sub

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
                             & " " & HeaderString())
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

                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & lTextFilename, lSB.ToString)
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
