Partial Class SwatInput
    Private pWus As clsWus = New clsWus(Me)
    ReadOnly Property Wus() As clsWus
        Get
            Return pWus
        End Get
    End Property

    Public Class clsWusItem
        Public SUBBASIN As Double
        Public WUPND(11) As Double
        Public WURCH(11) As Double
        Public WUSHAL(11) As Double
        Public WUDEEP(11) As Double

        Public Sub New(ByVal aSUBBASIN As Double)
            SUBBASIN = aSUBBASIN
        End Sub

        Public Sub New(ByVal aSUBBASIN As Double, _
                       ByVal aWUPND() As Double, _
                       ByVal aWURCH() As Double, _
                       ByVal aWUSHAL() As Double, _
                       ByVal aWUDEEP() As Double)
            SUBBASIN = aSUBBASIN
            WUPND = aWUPND
            WURCH = aWURCH
            WUSHAL = aWUSHAL
            WUDEEP = aWUDEEP
        End Sub
    End Class

    ''' <summary>
    ''' Gw Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsWus
        Private pSwatInput As SwatInput
        Private pTableName As String = "wus"

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn.DBLayer.createWus
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
                    Append12DBColumnsDouble(lTable.Columns, "WUPND")
                    Append12DBColumnsDouble(lTable.Columns, "WURCH")
                    Append12DBColumnsDouble(lTable.Columns, "WUSHAL")
                    Append12DBColumnsDouble(lTable.Columns, "WUDEEP")
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

        Private Sub Append12DBColumnsDouble(ByVal aColumns As ADOX.Columns, ByVal aSection As String)
            For i As Integer = 1 To 12
                aColumns.Append(aSection & i, ADOX.DataTypeEnum.adDouble)
            Next
        End Sub

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Sub Add(ByVal aWusItem As clsWusItem)
            With aWusItem
                Me.Add(.SUBBASIN, _
                       .WUPND(0), .WUPND(1), .WUPND(2), .WUPND(3), .WUPND(4), .WUPND(5), .WUPND(6), .WUPND(7), .WUPND(8), .WUPND(9), .WUPND(10), .WUPND(11), _
                       .WURCH(0), .WURCH(1), .WURCH(2), .WURCH(3), .WURCH(4), .WURCH(5), .WURCH(6), .WURCH(7), .WURCH(8), .WURCH(9), .WURCH(10), .WURCH(11), _
                       .WUSHAL(0), .WUSHAL(1), .WUSHAL(2), .WUSHAL(3), .WUSHAL(4), .WUSHAL(5), .WUSHAL(6), .WUSHAL(7), .WUSHAL(8), .WUSHAL(9), .WUSHAL(10), .WUSHAL(11), _
                       .WUDEEP(0), .WUDEEP(1), .WUDEEP(2), .WUDEEP(3), .WUDEEP(4), .WUDEEP(5), .WUDEEP(6), .WUDEEP(7), .WUDEEP(8), .WUDEEP(9), .WUDEEP(10), .WUDEEP(11))
            End With
        End Sub

        'Public Sub Add(ByVal SUBBASIN As Double, _
        '               ByVal WUPND() As Double, _
        '               ByVal WURCH() As Double, _
        '               ByVal WUSHAL() As Double, _
        '               ByVal WUDEEP() As Double)
        '    Me.Add(SUBBASIN, _
        '           WUPND(0), WUPND(1), WUPND(2), WUPND(3), WUPND(4), WUPND(5), WUPND(6), WUPND(7), WUPND(8), WUPND(9), WUPND(10), WUPND(11), _
        '           WURCH(0), WURCH(1), WURCH(2), WURCH(3), WURCH(4), WURCH(5), WURCH(6), WURCH(7), WURCH(8), WURCH(9), WURCH(10), WURCH(11), _
        '           WUSHAL(0), WUSHAL(1), WUSHAL(2), WUSHAL(3), WUSHAL(4), WUSHAL(5), WUSHAL(6), WUSHAL(7), WUSHAL(8), WUSHAL(9), WUSHAL(10), WUSHAL(11), _
        '           WUDEEP(0), WUDEEP(1), WUDEEP(2), WUDEEP(3), WUDEEP(4), WUDEEP(5), WUDEEP(6), WUDEEP(7), WUDEEP(8), WUDEEP(9), WUDEEP(10), WUDEEP(11))
        'End Sub

        Public Sub Add(ByVal SUBBASIN As Double, _
                        ByVal WUPND1 As Double, _
                        ByVal WUPND2 As Double, _
                        ByVal WUPND3 As Double, _
                        ByVal WUPND4 As Double, _
                        ByVal WUPND5 As Double, _
                        ByVal WUPND6 As Double, _
                        ByVal WUPND7 As Double, _
                        ByVal WUPND8 As Double, _
                        ByVal WUPND9 As Double, _
                        ByVal WUPND10 As Double, _
                        ByVal WUPND11 As Double, _
                        ByVal WUPND12 As Double, _
                        ByVal WURCH1 As Double, _
                        ByVal WURCH2 As Double, _
                        ByVal WURCH3 As Double, _
                        ByVal WURCH4 As Double, _
                        ByVal WURCH5 As Double, _
                        ByVal WURCH6 As Double, _
                        ByVal WURCH7 As Double, _
                        ByVal WURCH8 As Double, _
                        ByVal WURCH9 As Double, _
                        ByVal WURCH10 As Double, _
                        ByVal WURCH11 As Double, _
                        ByVal WURCH12 As Double, _
                        ByVal WUSHAL1 As Double, _
                        ByVal WUSHAL2 As Double, _
                        ByVal WUSHAL3 As Double, _
                        ByVal WUSHAL4 As Double, _
                        ByVal WUSHAL5 As Double, _
                        ByVal WUSHAL6 As Double, _
                        ByVal WUSHAL7 As Double, _
                        ByVal WUSHAL8 As Double, _
                        ByVal WUSHAL9 As Double, _
                        ByVal WUSHAL10 As Double, _
                        ByVal WUSHAL11 As Double, _
                        ByVal WUSHAL12 As Double, _
                        ByVal WUDEEP1 As Double, _
                        ByVal WUDEEP2 As Double, _
                        ByVal WUDEEP3 As Double, _
                        ByVal WUDEEP4 As Double, _
                        ByVal WUDEEP5 As Double, _
                        ByVal WUDEEP6 As Double, _
                        ByVal WUDEEP7 As Double, _
                        ByVal WUDEEP8 As Double, _
                        ByVal WUDEEP9 As Double, _
                        ByVal WUDEEP10 As Double, _
                        ByVal WUDEEP11 As Double, _
                        ByVal WUDEEP12 As Double)

            Dim lSQL As String = "INSERT INTO wus ( SUBBASIN , WUPND1 , WUPND2 , WUPND3 , WUPND4 , WUPND5 , WUPND6 , WUPND7 , WUPND8 , WUPND9 , WUPND10 , WUPND11 , WUPND12 , WURCH1 , WURCH2 , WURCH3 , WURCH4 , WURCH5 , WURCH6 , WURCH7 , WURCH8 , WURCH9 , WURCH10 , WURCH11 , WURCH12 , WUSHAL1 , WUSHAL2 , WUSHAL3 , WUSHAL4 , WUSHAL5 , WUSHAL6 , WUSHAL7 , WUSHAL8 , WUSHAL9 , WUSHAL10 , WUSHAL11 , WUSHAL12 , WUDEEP1 , WUDEEP2 , WUDEEP3 , WUDEEP4 , WUDEEP5 , WUDEEP6 , WUDEEP7 , WUDEEP8 , WUDEEP9 , WUDEEP10 , WUDEEP11 , WUDEEP12  )" _
                               & "Values ('" & SUBBASIN & "'  ,'" & WUPND1 & "'  ,'" & WUPND2 & "'  ,'" & WUPND3 & "'  ,'" & WUPND4 & "'  ,'" & WUPND5 & "'  ,'" & WUPND6 & "'  ,'" & WUPND7 & "'  ,'" & WUPND8 & "'  ,'" & WUPND9 & "'  ,'" & WUPND10 & "'  ,'" & WUPND11 & "'  ,'" & WUPND12 & "'  ,'" _
                               & WURCH1 & "'  ,'" & WURCH2 & "'  ,'" & WURCH3 & "'  ,'" & WURCH4 & "'  ,'" & WURCH5 & "'  ,'" & WURCH6 & "'  ,'" & WURCH7 & "'  ,'" & WURCH8 & "'  ,'" & WURCH9 & "'  ,'" & WURCH10 & "'  ,'" & WURCH11 & "'  ,'" & WURCH12 & "'  ,'" _
                               & WUSHAL1 & "'  ,'" & WUSHAL2 & "'  ,'" & WUSHAL3 & "'  ,'" & WUSHAL4 & "'  ,'" & WUSHAL5 & "'  ,'" & WUSHAL6 & "'  ,'" & WUSHAL7 & "'  ,'" & WUSHAL8 & "'  ,'" & WUSHAL9 & "'  ,'" & WUSHAL10 & "'  ,'" & WUSHAL11 & "'  ,'" & WUSHAL12 & "'  ,'" _
                               & WUDEEP1 & "'  ,'" & WUDEEP2 & "'  ,'" & WUDEEP3 & "'  ,'" & WUDEEP4 & "'  ,'" & WUDEEP5 & "'  ,'" & WUDEEP6 & "'  ,'" & WUDEEP7 & "'  ,'" & WUDEEP8 & "'  ,'" & WUDEEP9 & "'  ,'" & WUDEEP10 & "'  ,'" & WUDEEP11 & "'  ,'" & WUDEEP12 & "'  )"
            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()

            pSwatInput.Status("Writing " & pTableName & " text ...")

            Dim i As Integer

            For Each lRow As DataRow In aTable.Rows
                Dim lSubBasin As String = lRow.Item(1)
                Dim lHruNum As String = lRow.Item(2)
                Dim lTextFilename As String = StringFnameHRUs(lSubBasin, lHruNum) & "." & pTableName

                Dim lSB As New Text.StringBuilder
                lSB.AppendLine(" .wus file Subbasin: " & lSubBasin & " " & HeaderString())
                lSB.AppendLine()
                lSB.AppendLine()

                '---4,5 WUPND

                For i = 2 To 7
                    lSB.Append(Format(lRow.Item(i), "0.0").PadLeft(10))
                Next
                lSB.AppendLine()

                For i = 8 To 13
                    lSB.Append(Format(lRow.Item(i), "0.0").PadLeft(10))
                Next
                lSB.AppendLine()

                '---6,7 WURCH

                For i = 14 To 19
                    lSB.Append(Format(lRow.Item(i), "0.0").PadLeft(10))
                Next
                lSB.AppendLine()

                For i = 20 To 25
                    lSB.Append(Format(lRow.Item(i), "0.0").PadLeft(10))
                Next
                lSB.AppendLine()

                '---8,9 WUSHAL

                For i = 26 To 31
                    lSB.Append(Format(lRow.Item(i), "0.0").PadLeft(10))
                Next
                lSB.AppendLine()

                For i = 32 To 37
                    lSB.Append(Format(lRow.Item(i), "0.0").PadLeft(10))
                Next
                lSB.AppendLine()

                '---10,11 WUDEEP

                For i = 38 To 43
                    lSB.Append(Format(lRow.Item(i), "0.0").PadLeft(10))
                Next
                lSB.AppendLine()

                For i = 44 To 49
                    lSB.Append(Format(lRow.Item(i), "0.0").PadLeft(10))
                Next
                lSB.AppendLine()

                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & lTextFilename, lSB.ToString)
                'ReplaceNonAscii(lTextFilename)
            Next
        End Sub


    End Class
End Class
