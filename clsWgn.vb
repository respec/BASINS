Partial Class SwatInput
    Private pWgn As clsWgn = New clsWgn(Me)
    ReadOnly Property Wgn() As clsWgn
        Get
            Return pWgn
        End Get
    End Property

    ''' <summary>
    ''' WGN Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsWgn
        Private pSwatInput As SwatInput
        Private pTableName As String = "wgn"

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn:DBLayer:createRTE
            Try
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
                    .Append("SUBBASIN", ADOX.DataTypeEnum.adInteger)
                    .Append("STATION", ADOX.DataTypeEnum.adVarWChar, 80)
                    .Append("WLATITUDE", ADOX.DataTypeEnum.adSingle)
                    .Append("WLONGITUDE", ADOX.DataTypeEnum.adSingle)
                    .Append("WELEV", ADOX.DataTypeEnum.adSingle)
                    .Append("RAIN_YRS", ADOX.DataTypeEnum.adSingle)

                    .Append("TMPMX1", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX2", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX3", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX4", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX5", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX6", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX7", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX8", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX9", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX10", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX11", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMX12", ADOX.DataTypeEnum.adSingle)

                    .Append("TMPMN1", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN2", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN3", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN4", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN5", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN6", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN7", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN8", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN9", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN10", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN11", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPMN12", ADOX.DataTypeEnum.adSingle)

                    .Append("TMPSTDMX1", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX2", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX3", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX4", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX5", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX6", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX7", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX8", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX9", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX10", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX11", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMX12", ADOX.DataTypeEnum.adSingle)

                    .Append("TMPSTDMN1", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN2", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN3", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN4", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN5", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN6", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN7", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN8", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN9", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN10", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN11", ADOX.DataTypeEnum.adSingle)
                    .Append("TMPSTDMN12", ADOX.DataTypeEnum.adSingle)

                    .Append("PCPMM1", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM2", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM3", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM4", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM5", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM6", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM7", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM8", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM9", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM10", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM11", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPMM12", ADOX.DataTypeEnum.adSingle)

                    .Append("PCPSTD1", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD2", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD3", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD4", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD5", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD6", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD7", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD8", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD9", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD10", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD11", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSTD12", ADOX.DataTypeEnum.adSingle)

                    .Append("PCPSKW1", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW2", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW3", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW4", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW5", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW6", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW7", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW8", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW9", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW10", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW11", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPSKW12", ADOX.DataTypeEnum.adSingle)

                    .Append("PR_W1_1", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_2", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_3", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_4", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_5", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_6", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_7", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_8", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_9", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_10", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_11", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W1_12", ADOX.DataTypeEnum.adSingle)

                    .Append("PR_W2_1", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_2", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_3", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_4", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_5", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_6", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_7", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_8", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_9", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_10", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_11", ADOX.DataTypeEnum.adSingle)
                    .Append("PR_W2_12", ADOX.DataTypeEnum.adSingle)

                    .Append("PCPD1", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD2", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD3", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD4", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD5", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD6", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD7", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD8", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD9", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD10", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD11", ADOX.DataTypeEnum.adSingle)
                    .Append("PCPD12", ADOX.DataTypeEnum.adSingle)

                    .Append("RAINHHMX1", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX2", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX3", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX4", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX5", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX6", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX7", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX8", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX9", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX10", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX11", ADOX.DataTypeEnum.adSingle)
                    .Append("RAINHHMX12", ADOX.DataTypeEnum.adSingle)

                    .Append("SOLARAV1", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV2", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV3", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV4", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV5", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV6", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV7", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV8", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV9", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV10", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV11", ADOX.DataTypeEnum.adSingle)
                    .Append("SOLARAV12", ADOX.DataTypeEnum.adSingle)

                    .Append("DEWPT1", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT2", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT3", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT4", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT5", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT6", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT7", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT8", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT9", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT10", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT11", ADOX.DataTypeEnum.adSingle)
                    .Append("DEWPT12", ADOX.DataTypeEnum.adSingle)

                    .Append("WNDAV1", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV2", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV3", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV4", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV5", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV6", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV7", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV8", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV9", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV10", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV11", ADOX.DataTypeEnum.adSingle)
                    .Append("WNDAV12", ADOX.DataTypeEnum.adSingle)
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

        Sub Add(ByVal ConString As String, _
              ByVal SUBBASIN As Integer, _
              ByVal STATION As String, _
              ByVal WLATITUDE As Single, _
              ByVal WLONGITUDE As Single, _
              ByVal WELEV As Single, _
              ByVal RAIN_YRS As Single, _
              ByVal TMPMX1 As Single, _
              ByVal TMPMX2 As Single, _
              ByVal TMPMX3 As Single, _
              ByVal TMPMX4 As Single, _
              ByVal TMPMX5 As Single, _
              ByVal TMPMX6 As Single, _
              ByVal TMPMX7 As Single, _
              ByVal TMPMX8 As Single, _
              ByVal TMPMX9 As Single, _
              ByVal TMPMX10 As Single, _
              ByVal TMPMX11 As Single, _
              ByVal TMPMX12 As Single, _
              ByVal TMPMN1 As Single, _
              ByVal TMPMN2 As Single, _
              ByVal TMPMN3 As Single, _
              ByVal TMPMN4 As Single, _
              ByVal TMPMN5 As Single, _
              ByVal TMPMN6 As Single, _
              ByVal TMPMN7 As Single, _
              ByVal TMPMN8 As Single, _
              ByVal TMPMN9 As Single, _
              ByVal TMPMN10 As Single, _
              ByVal TMPMN11 As Single, _
              ByVal TMPMN12 As Single, _
              ByVal TMPSTDMX1 As Single, _
              ByVal TMPSTDMX2 As Single, _
              ByVal TMPSTDMX3 As Single, _
              ByVal TMPSTDMX4 As Single, _
              ByVal TMPSTDMX5 As Single, _
              ByVal TMPSTDMX6 As Single, _
              ByVal TMPSTDMX7 As Single, _
              ByVal TMPSTDMX8 As Single, _
              ByVal TMPSTDMX9 As Single, _
              ByVal TMPSTDMX10 As Single, _
              ByVal TMPSTDMX11 As Single, _
              ByVal TMPSTDMX12 As Single, _
              ByVal TMPSTDMN1 As Single, _
              ByVal TMPSTDMN2 As Single, _
              ByVal TMPSTDMN3 As Single, _
              ByVal TMPSTDMN4 As Single, _
              ByVal TMPSTDMN5 As Single, _
              ByVal TMPSTDMN6 As Single, _
              ByVal TMPSTDMN7 As Single, _
              ByVal TMPSTDMN8 As Single, _
              ByVal TMPSTDMN9 As Single, _
              ByVal TMPSTDMN10 As Single, _
              ByVal TMPSTDMN11 As Single, _
              ByVal TMPSTDMN12 As Single, _
              ByVal PCPMM1 As Single, _
              ByVal PCPMM2 As Single, _
              ByVal PCPMM3 As Single, _
              ByVal PCPMM4 As Single, _
              ByVal PCPMM5 As Single, _
              ByVal PCPMM6 As Single, _
              ByVal PCPMM7 As Single, _
              ByVal PCPMM8 As Single, _
              ByVal PCPMM9 As Single, _
              ByVal PCPMM10 As Single, _
              ByVal PCPMM11 As Single, _
              ByVal PCPMM12 As Single, _
              ByVal PCPSTD1 As Single, _
              ByVal PCPSTD2 As Single, _
              ByVal PCPSTD3 As Single, _
              ByVal PCPSTD4 As Single, _
              ByVal PCPSTD5 As Single, _
              ByVal PCPSTD6 As Single, _
              ByVal PCPSTD7 As Single, _
              ByVal PCPSTD8 As Single, _
              ByVal PCPSTD9 As Single, _
              ByVal PCPSTD10 As Single, _
              ByVal PCPSTD11 As Single, _
              ByVal PCPSTD12 As Single, _
              ByVal PCPSKW1 As Single, _
              ByVal PCPSKW2 As Single, _
              ByVal PCPSKW3 As Single, _
              ByVal PCPSKW4 As Single, _
              ByVal PCPSKW5 As Single, _
              ByVal PCPSKW6 As Single, _
              ByVal PCPSKW7 As Single, _
              ByVal PCPSKW8 As Single, _
              ByVal PCPSKW9 As Single, _
              ByVal PCPSKW10 As Single, _
              ByVal PCPSKW11 As Single, _
              ByVal PCPSKW12 As Single, _
              ByVal PR_W1_1 As Single, _
              ByVal PR_W1_2 As Single, _
              ByVal PR_W1_3 As Single, _
              ByVal PR_W1_4 As Single, _
              ByVal PR_W1_5 As Single, _
              ByVal PR_W1_6 As Single, _
              ByVal PR_W1_7 As Single, _
              ByVal PR_W1_8 As Single, _
              ByVal PR_W1_9 As Single, _
              ByVal PR_W1_10 As Single, _
              ByVal PR_W1_11 As Single, _
              ByVal PR_W1_12 As Single, _
              ByVal PR_W2_1 As Single, _
              ByVal PR_W2_2 As Single, _
              ByVal PR_W2_3 As Single, _
              ByVal PR_W2_4 As Single, _
              ByVal PR_W2_5 As Single, _
              ByVal PR_W2_6 As Single, _
              ByVal PR_W2_7 As Single, _
              ByVal PR_W2_8 As Single, _
              ByVal PR_W2_9 As Single, _
              ByVal PR_W2_10 As Single, _
              ByVal PR_W2_11 As Single, _
              ByVal PR_W2_12 As Single, _
              ByVal PCPD1 As Single, _
              ByVal PCPD2 As Single, _
              ByVal PCPD3 As Single, _
              ByVal PCPD4 As Single, _
              ByVal PCPD5 As Single, _
              ByVal PCPD6 As Single, _
              ByVal PCPD7 As Single, _
              ByVal PCPD8 As Single, _
              ByVal PCPD9 As Single, _
              ByVal PCPD10 As Single, _
              ByVal PCPD11 As Single, _
              ByVal PCPD12 As Single, _
              ByVal RAINHHMX1 As Single, _
              ByVal RAINHHMX2 As Single, _
              ByVal RAINHHMX3 As Single, _
              ByVal RAINHHMX4 As Single, _
              ByVal RAINHHMX5 As Single, _
              ByVal RAINHHMX6 As Single, _
              ByVal RAINHHMX7 As Single, _
              ByVal RAINHHMX8 As Single, _
              ByVal RAINHHMX9 As Single, _
              ByVal RAINHHMX10 As Single, _
              ByVal RAINHHMX11 As Single, _
              ByVal RAINHHMX12 As Single, _
              ByVal SOLARAV1 As Single, _
              ByVal SOLARAV2 As Single, _
              ByVal SOLARAV3 As Single, _
              ByVal SOLARAV4 As Single, _
              ByVal SOLARAV5 As Single, _
              ByVal SOLARAV6 As Single, _
              ByVal SOLARAV7 As Single, _
              ByVal SOLARAV8 As Single, _
              ByVal SOLARAV9 As Single, _
              ByVal SOLARAV10 As Single, _
              ByVal SOLARAV11 As Single, _
              ByVal SOLARAV12 As Single, _
              ByVal DEWPT1 As Single, _
              ByVal DEWPT2 As Single, _
              ByVal DEWPT3 As Single, _
              ByVal DEWPT4 As Single, _
              ByVal DEWPT5 As Single, _
              ByVal DEWPT6 As Single, _
              ByVal DEWPT7 As Single, _
              ByVal DEWPT8 As Single, _
              ByVal DEWPT9 As Single, _
              ByVal DEWPT10 As Single, _
              ByVal DEWPT11 As Single, _
              ByVal DEWPT12 As Single, _
              ByVal WNDAV1 As Single, _
              ByVal WNDAV2 As Single, _
              ByVal WNDAV3 As Single, _
              ByVal WNDAV4 As Single, _
              ByVal WNDAV5 As Single, _
              ByVal WNDAV6 As Single, _
              ByVal WNDAV7 As Single, _
              ByVal WNDAV8 As Single, _
              ByVal WNDAV9 As Single, _
              ByVal WNDAV10 As Single, _
              ByVal WNDAV11 As Single, _
              ByVal WNDAV12 As Single)

            Dim lSQL As String = "INSERT INTO wgn( SUBBASIN , STATION , WLATITUDE , WLONGITUDE , WELEV , RAIN_YRS , TMPMX1 , TMPMX2 , TMPMX3 , TMPMX4 , TMPMX5 , TMPMX6 , TMPMX7 , TMPMX8 , TMPMX9 , TMPMX10 , TMPMX11 , TMPMX12 , TMPMN1 , TMPMN2 , TMPMN3 , TMPMN4 , TMPMN5 , TMPMN6 , TMPMN7 , TMPMN8 , TMPMN9 , TMPMN10 , TMPMN11 , TMPMN12 , TMPSTDMX1 , TMPSTDMX2 , TMPSTDMX3 , TMPSTDMX4 , TMPSTDMX5 , TMPSTDMX6 , TMPSTDMX7 , TMPSTDMX8 , TMPSTDMX9 , TMPSTDMX10 , TMPSTDMX11 , TMPSTDMX12 , TMPSTDMN1 , TMPSTDMN2 , TMPSTDMN3 , TMPSTDMN4 , TMPSTDMN5 , TMPSTDMN6 , TMPSTDMN7 , TMPSTDMN8 , TMPSTDMN9 , TMPSTDMN10 , TMPSTDMN11 , TMPSTDMN12 , PCPMM1 , PCPMM2 , PCPMM3 , PCPMM4 , PCPMM5 , PCPMM6 , PCPMM7 , PCPMM8 , PCPMM9 , PCPMM10 , PCPMM11 , PCPMM12 , PCPSTD1 , PCPSTD2 , PCPSTD3 , PCPSTD4 , PCPSTD5 , PCPSTD6 , PCPSTD7 , PCPSTD8 , PCPSTD9 , PCPSTD10 , PCPSTD11 , PCPSTD12 , PCPSKW1 , PCPSKW2 , PCPSKW3 , PCPSKW4 , PCPSKW5 , PCPSKW6 , PCPSKW7 , PCPSKW8 , PCPSKW9 , PCPSKW10 , PCPSKW11 , PCPSKW12 , PR_W1_1 , PR_W1_2 , PR_W1_3 , PR_W1_4 , PR_W1_5 , PR_W1_6 , PR_W1_7 , PR_W1_8 , PR_W1_9 , PR_W1_10 , PR_W1_11 , PR_W1_12 , PR_W2_1 , PR_W2_2 , PR_W2_3 , PR_W2_4 , PR_W2_5 , PR_W2_6 , PR_W2_7 , PR_W2_8 , PR_W2_9 , PR_W2_10 , PR_W2_11 , PR_W2_12 , PCPD1 , PCPD2 , PCPD3 , PCPD4 , PCPD5 , PCPD6 , PCPD7 , PCPD8 , PCPD9 , PCPD10 , PCPD11 , PCPD12 , RAINHHMX1 , RAINHHMX2 , RAINHHMX3 , RAINHHMX4 , RAINHHMX5 , RAINHHMX6 , RAINHHMX7 , RAINHHMX8 , RAINHHMX9 , RAINHHMX10 , RAINHHMX11 , RAINHHMX12 , SOLARAV1 , SOLARAV2 , SOLARAV3 , SOLARAV4 , SOLARAV5 , SOLARAV6 , SOLARAV7 , SOLARAV8 , SOLARAV9 , SOLARAV10 , SOLARAV11 , SOLARAV12 , DEWPT1 , DEWPT2 , DEWPT3 , DEWPT4 , DEWPT5 , DEWPT6 , DEWPT7 , DEWPT8 , DEWPT9 , DEWPT10 , DEWPT11 , DEWPT12 , WNDAV1 , WNDAV2 , WNDAV3 , WNDAV4 , WNDAV5 , WNDAV6 , WNDAV7 , WNDAV8 , WNDAV9 , WNDAV10 , WNDAV11 , WNDAV12  ) " _
                & "Values ('" & SUBBASIN & "'  ,'" & STATION & "'  ,'" & WLATITUDE & "'  ,'" & WLONGITUDE & "'  ,'" & WELEV & "'  ,'" & RAIN_YRS & "'  ,'" _
                & TMPMX1 & "'  ,'" & TMPMX2 & "'  ,'" & TMPMX3 & "'  ,'" & TMPMX4 & "'  ,'" & TMPMX5 & "'  ,'" & TMPMX6 & "'  ,'" & TMPMX7 & "'  ,'" & TMPMX8 & "'  ,'" & TMPMX9 & "'  ,'" & TMPMX10 & "'  ,'" & TMPMX11 & "'  ,'" & TMPMX12 & "'  ,'" _
                & TMPMN1 & "'  ,'" & TMPMN2 & "'  ,'" & TMPMN3 & "'  ,'" & TMPMN4 & "'  ,'" & TMPMN5 & "'  ,'" & TMPMN6 & "'  ,'" & TMPMN7 & "'  ,'" & TMPMN8 & "'  ,'" & TMPMN9 & "'  ,'" & TMPMN10 & "'  ,'" & TMPMN11 & "'  ,'" & TMPMN12 & "'  ,'" _
                & TMPSTDMX1 & "'  ,'" & TMPSTDMX2 & "'  ,'" & TMPSTDMX3 & "'  ,'" & TMPSTDMX4 & "'  ,'" & TMPSTDMX5 & "'  ,'" & TMPSTDMX6 & "'  ,'" & TMPSTDMX7 & "'  ,'" & TMPSTDMX8 & "'  ,'" & TMPSTDMX9 & "'  ,'" & TMPSTDMX10 & "'  ,'" & TMPSTDMX11 & "'  ,'" & TMPSTDMX12 & "'  ,'" _
                & TMPSTDMN1 & "'  ,'" & TMPSTDMN2 & "'  ,'" & TMPSTDMN3 & "'  ,'" & TMPSTDMN4 & "'  ,'" & TMPSTDMN5 & "'  ,'" & TMPSTDMN6 & "'  ,'" & TMPSTDMN7 & "'  ,'" & TMPSTDMN8 & "'  ,'" & TMPSTDMN9 & "'  ,'" & TMPSTDMN10 & "'  ,'" & TMPSTDMN11 & "'  ,'" & TMPSTDMN12 & "'  ,'" _
                & PCPMM1 & "'  ,'" & PCPMM2 & "'  ,'" & PCPMM3 & "'  ,'" & PCPMM4 & "'  ,'" & PCPMM5 & "'  ,'" & PCPMM6 & "'  ,'" & PCPMM7 & "'  ,'" & PCPMM8 & "'  ,'" & PCPMM9 & "'  ,'" & PCPMM10 & "'  ,'" & PCPMM11 & "'  ,'" & PCPMM12 & "'  ,'" _
                & PCPSTD1 & "'  ,'" & PCPSTD2 & "'  ,'" & PCPSTD3 & "'  ,'" & PCPSTD4 & "'  ,'" & PCPSTD5 & "'  ,'" & PCPSTD6 & "'  ,'" & PCPSTD7 & "'  ,'" & PCPSTD8 & "'  ,'" & PCPSTD9 & "'  ,'" & PCPSTD10 & "'  ,'" & PCPSTD11 & "'  ,'" & PCPSTD12 & "'  ,'" _
                & PCPSKW1 & "'  ,'" & PCPSKW2 & "'  ,'" & PCPSKW3 & "'  ,'" & PCPSKW4 & "'  ,'" & PCPSKW5 & "'  ,'" & PCPSKW6 & "'  ,'" & PCPSKW7 & "'  ,'" & PCPSKW8 & "'  ,'" & PCPSKW9 & "'  ,'" & PCPSKW10 & "'  ,'" & PCPSKW11 & "'  ,'" & PCPSKW12 & "'  ,'" _
                & PR_W1_1 & "'  ,'" & PR_W1_2 & "'  ,'" & PR_W1_3 & "'  ,'" & PR_W1_4 & "'  ,'" & PR_W1_5 & "'  ,'" & PR_W1_6 & "'  ,'" & PR_W1_7 & "'  ,'" & PR_W1_8 & "'  ,'" & PR_W1_9 & "'  ,'" & PR_W1_10 & "'  ,'" & PR_W1_11 & "'  ,'" & PR_W1_12 & "'  ,'" _
                & PR_W2_1 & "'  ,'" & PR_W2_2 & "'  ,'" & PR_W2_3 & "'  ,'" & PR_W2_4 & "'  ,'" & PR_W2_5 & "'  ,'" & PR_W2_6 & "'  ,'" & PR_W2_7 & "'  ,'" & PR_W2_8 & "'  ,'" & PR_W2_9 & "'  ,'" & PR_W2_10 & "'  ,'" & PR_W2_11 & "'  ,'" & PR_W2_12 & "'  ,'" _
                & PCPD1 & "'  ,'" & PCPD2 & "'  ,'" & PCPD3 & "'  ,'" & PCPD4 & "'  ,'" & PCPD5 & "'  ,'" & PCPD6 & "'  ,'" & PCPD7 & "'  ,'" & PCPD8 & "'  ,'" & PCPD9 & "'  ,'" & PCPD10 & "'  ,'" & PCPD11 & "'  ,'" & PCPD12 & "'  ,'" _
                & RAINHHMX1 & "'  ,'" & RAINHHMX2 & "'  ,'" & RAINHHMX3 & "'  ,'" & RAINHHMX4 & "'  ,'" & RAINHHMX5 & "'  ,'" & RAINHHMX6 & "'  ,'" & RAINHHMX7 & "'  ,'" & RAINHHMX8 & "'  ,'" & RAINHHMX9 & "'  ,'" & RAINHHMX10 & "'  ,'" & RAINHHMX11 & "'  ,'" & RAINHHMX12 & "'  ,'" _
                & SOLARAV1 & "'  ,'" & SOLARAV2 & "'  ,'" & SOLARAV3 & "'  ,'" & SOLARAV4 & "'  ,'" & SOLARAV5 & "'  ,'" & SOLARAV6 & "'  ,'" & SOLARAV7 & "'  ,'" & SOLARAV8 & "'  ,'" & SOLARAV9 & "'  ,'" & SOLARAV10 & "'  ,'" & SOLARAV11 & "'  ,'" & SOLARAV12 & "'  ,'" _
                & DEWPT1 & "'  ,'" & DEWPT2 & "'  ,'" & DEWPT3 & "'  ,'" & DEWPT4 & "'  ,'" & DEWPT5 & "'  ,'" & DEWPT6 & "'  ,'" & DEWPT7 & "'  ,'" & DEWPT8 & "'  ,'" & DEWPT9 & "'  ,'" & DEWPT10 & "'  ,'" & DEWPT11 & "'  ,'" & DEWPT12 & "'  ,'" _
                & WNDAV1 & "'  ,'" & WNDAV2 & "'  ,'" & WNDAV3 & "'  ,'" & WNDAV4 & "'  ,'" & WNDAV5 & "'  ,'" & WNDAV6 & "'  ,'" & WNDAV7 & "'  ,'" & WNDAV8 & "'  ,'" & WNDAV9 & "'  ,'" & WNDAV10 & "'  ,'" & WNDAV11 & "'  ,'" & WNDAV12 & "'   )"

            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing " & pTableName & " text ...")

            For Each lRow As DataRow In aTable.Rows
                Dim lSubBasin As String = lRow.Item("SUBBASIN")

                Dim lSB As New Text.StringBuilder
                lSB.AppendLine(" .Wgn file Subbasin: " & lSubBasin _
                             & " STATION NAME:" & lRow.Item(("STATION")) & " " _
                             & DateNowString() & " AVSWAT2003 -SWAT INTERFACE MAVZ")
                lSB.AppendLine("  LATITUDE =" & Format(lRow.Item(("WLATITUDE")), "0.00").PadLeft(7) _
                             & " LONGITUDE =" & Format(lRow.Item(("WLONGITUDE")), "0.00").PadLeft(7))
                lSB.AppendLine("  ELEV [m] =" & Format(lRow.Item(("WELEV")), "0.00").PadLeft(7))
                lSB.AppendLine("  RAIN_YRS =" & Format(lRow.Item(("RAIN_YRS")), "0.00").PadLeft(7))
                Append12(lSB, lRow, "TMPMX")
                Append12(lSB, lRow, "TMPMN")
                Append12(lSB, lRow, "TMPSTDMX")
                Append12(lSB, lRow, "TMPSTDMN")
                Append12(lSB, lRow, "PCPMM")
                Append12(lSB, lRow, "PCPSTD")
                Append12(lSB, lRow, "PCPSKW")
                Append12(lSB, lRow, "PR_W1_")
                Append12(lSB, lRow, "PR_W2_")
                Append12(lSB, lRow, "PCPD")
                Append12(lSB, lRow, "RAINHHMX")
                Append12(lSB, lRow, "SOLARAV")
                Append12(lSB, lRow, "DEWPT")
                Append12(lSB, lRow, "WNDAV")

                IO.File.WriteAllText(pSwatInput.OutputFolder & "\" & StringFname(lSubBasin, pTableName), lSB.ToString)
            Next
        End Sub

        Private Sub Append12(ByVal aSB As Text.StringBuilder, ByVal aRow As DataRow, ByVal aSection As String)
            For i As Integer = 1 To 12
                aSB.Append(Format(aRow.Item(aSection & i), "0.00").PadLeft(6))
            Next
            aSB.AppendLine()
        End Sub

    End Class
End Class
