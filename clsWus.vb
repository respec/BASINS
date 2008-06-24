Partial Class SwatInput
    Private pWus As clsWus = New clsWus(Me)
    ReadOnly Property Wus() As clsWus
        Get
            Return pWus
        End Get
    End Property

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
                    Append12Columns(lTable.Columns, "WUPND")
                    Append12Columns(lTable.Columns, "WURCH")
                    Append12Columns(lTable.Columns, "WUSHAL")
                    Append12Columns(lTable.Columns, "WUDEEP")
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

        Private Sub Append12Columns(ByVal aColumns As ADOX.Columns, ByVal aSection As String)
            For i As Integer = 1 To 12
                aColumns.Append(aSection & i, ADOX.DataTypeEnum.adDouble)
            Next
        End Sub

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()

            pSwatInput.Status("Writing " & pTableName & " text ...")

            Dim i As Integer

            For Each lRow As DataRow In aTable.Rows
                Dim lSubBasin As String = lRow.Item(1)
                Dim lHruNum As String = lRow.Item(2)
                Dim lTextFilename As String = StringFnameHRUs(lSubBasin, lHruNum) & "." & pTableName

                Dim lSB As New Text.StringBuilder
                lSB.AppendLine(" .wus file Subbasin: " + lSubBasin + " " + DateNowString() + " AVSWAT2003 -SWAT INTERFACE MAVZ")
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

                IO.File.WriteAllText(pSwatInput.OutputFolder & "\" & lTextFilename, lSB.ToString)
                'ReplaceNonAscii(lTextFilename)
            Next
        End Sub


    End Class
End Class
