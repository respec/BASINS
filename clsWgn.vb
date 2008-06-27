Partial Class SwatInput
    Private pWgn As clsWgn = New clsWgn(Me)
    ReadOnly Property Wgn() As clsWgn
        Get
            Return pWgn
        End Get
    End Property

    Public Class clsWgnItem
        Public SUBBASIN As Integer
        Public STATION As String
        Public WLATITUDE As Single
        Public WLONGITUDE As Single
        Public WELEV As Single
        Public RAIN_YRS As Single
        Public TMPMX(11) As Single
        Public TMPMN(11) As Single
        Public TMPSTDMX(11) As Single
        Public TMPSTDMN(11) As Single
        Public PCPMM(11) As Single
        Public PCPSTD(11) As Single
        Public PCPSKW(11) As Single
        Public PR_W1_(11) As Single
        Public PR_W2_(11) As Single
        Public PCPD(11) As Single
        Public RAINHHMX(11) As Single
        Public SOLARAV(11) As Single
        Public DEWPT(11) As Single
        Public WNDAV(11) As Single

        Public Sub New(ByVal aSUBBASIN As Integer)
            SUBBASIN = aSUBBASIN
        End Sub

        Public Sub New(ByVal aSUBBASIN As Integer, _
                       ByVal aSTATION As String, _
                       ByVal aWLATITUDE As Single, _
                       ByVal aWLONGITUDE As Single, _
                       ByVal aWELEV As Single, _
                       ByVal aRAIN_YRS As Single, _
                       ByVal aTMPMX() As Single, _
                       ByVal aTMPMN() As Single, _
                       ByVal aTMPSTDMX() As Single, _
                       ByVal aTMPSTDMN() As Single, _
                       ByVal aPCPMM() As Single, _
                       ByVal aPCPSTD() As Single, _
                       ByVal aPCPSKW() As Single, _
                       ByVal aPR_W1_() As Single, _
                       ByVal aPR_W2_() As Single, _
                       ByVal aPCPD() As Single, _
                       ByVal aRAINHHMX() As Single, _
                       ByVal aSOLARAV() As Single, _
                       ByVal aDEWPT() As Single, _
                       ByVal aWNDAV() As Single)
            SUBBASIN = aSUBBASIN
            STATION = aSTATION
            WLATITUDE = aWLATITUDE
            WLONGITUDE = aWLONGITUDE
            WELEV = aWELEV
            RAIN_YRS = aRAIN_YRS
            TMPMX = aTMPMX
            TMPMN = aTMPMN
            TMPSTDMX = aTMPSTDMX
            TMPSTDMN = aTMPSTDMN
            PCPMM = aPCPMM
            PCPSTD = aPCPSTD
            PCPSKW = aPCPSKW
            PR_W1_ = aPR_W1_
            PR_W2_ = aPR_W2_
            PCPD = aPCPD
            RAINHHMX = aRAINHHMX
            SOLARAV = aSOLARAV
            DEWPT = aDEWPT
            WNDAV = aWNDAV
        End Sub
    End Class

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

                    Append12DBColumnsSingle(lTable.Columns, "TMPMX")
                    Append12DBColumnsSingle(lTable.Columns, "TMPMN")
                    Append12DBColumnsSingle(lTable.Columns, "TMPSTDMX")
                    Append12DBColumnsSingle(lTable.Columns, "TMPSTDMN")
                    Append12DBColumnsSingle(lTable.Columns, "PCPMM")
                    Append12DBColumnsSingle(lTable.Columns, "PCPSTD")
                    Append12DBColumnsSingle(lTable.Columns, "PCPSKW")
                    Append12DBColumnsSingle(lTable.Columns, "PR_W1_")
                    Append12DBColumnsSingle(lTable.Columns, "PR_W2_")
                    Append12DBColumnsSingle(lTable.Columns, "PCPD")
                    Append12DBColumnsSingle(lTable.Columns, "RAINHHMX")
                    Append12DBColumnsSingle(lTable.Columns, "SOLARAV")
                    Append12DBColumnsSingle(lTable.Columns, "DEWPT")
                    Append12DBColumnsSingle(lTable.Columns, "WNDAV")
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

        Private Sub Append12DBColumnsSingle(ByVal aColumns As ADOX.Columns, ByVal aSection As String)
            For i As Integer = 1 To 12
                aColumns.Append(aSection & i, ADOX.DataTypeEnum.adSingle)
            Next
        End Sub

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Sub Add(ByVal aWgnItem As clsWgnItem)
            With aWgnItem
                Me.add(.SUBBASIN, .STATION, .WLATITUDE, .WLONGITUDE, .WELEV, .RAIN_YRS, _
                    .TMPMX(0), .TMPMX(1), .TMPMX(2), .TMPMX(3), .TMPMX(4), .TMPMX(5), .TMPMX(6), .TMPMX(7), .TMPMX(8), .TMPMX(9), .TMPMX(10), .TMPMX(11), _
                    .TMPMN(0), .TMPMN(1), .TMPMN(2), .TMPMN(3), .TMPMN(4), .TMPMN(5), .TMPMN(6), .TMPMN(7), .TMPMN(8), .TMPMN(9), .TMPMN(10), .TMPMN(11), _
                    .TMPSTDMX(0), .TMPSTDMX(1), .TMPSTDMX(2), .TMPSTDMX(3), .TMPSTDMX(4), .TMPSTDMX(5), .TMPSTDMX(6), .TMPSTDMX(7), .TMPSTDMX(8), .TMPSTDMX(9), .TMPSTDMX(10), .TMPSTDMX(11), _
                    .TMPSTDMN(0), .TMPSTDMN(1), .TMPSTDMN(2), .TMPSTDMN(3), .TMPSTDMN(4), .TMPSTDMN(5), .TMPSTDMN(6), .TMPSTDMN(7), .TMPSTDMN(8), .TMPSTDMN(9), .TMPSTDMN(10), .TMPSTDMN(11), _
                    .PCPMM(0), .PCPMM(1), .PCPMM(2), .PCPMM(3), .PCPMM(4), .PCPMM(5), .PCPMM(6), .PCPMM(7), .PCPMM(8), .PCPMM(9), .PCPMM(10), .PCPMM(11), _
                    .PCPSTD(0), .PCPSTD(1), .PCPSTD(2), .PCPSTD(3), .PCPSTD(4), .PCPSTD(5), .PCPSTD(6), .PCPSTD(7), .PCPSTD(8), .PCPSTD(9), .PCPSTD(10), .PCPSTD(11), _
                    .PCPSKW(0), .PCPSKW(1), .PCPSKW(2), .PCPSKW(3), .PCPSKW(4), .PCPSKW(5), .PCPSKW(6), .PCPSKW(7), .PCPSKW(8), .PCPSKW(9), .PCPSKW(10), .PCPSKW(11), _
                    .PR_W1_(0), .PR_W1_(1), .PR_W1_(2), .PR_W1_(3), .PR_W1_(4), .PR_W1_(5), .PR_W1_(6), .PR_W1_(7), .PR_W1_(8), .PR_W1_(9), .PR_W1_(10), .PR_W1_(11), _
                    .PR_W2_(0), .PR_W2_(1), .PR_W2_(2), .PR_W2_(3), .PR_W2_(4), .PR_W2_(5), .PR_W2_(6), .PR_W2_(7), .PR_W2_(8), .PR_W2_(9), .PR_W2_(10), .PR_W2_(11), _
                    .PCPD(0), .PCPD(1), .PCPD(2), .PCPD(3), .PCPD(4), .PCPD(5), .PCPD(6), .PCPD(7), .PCPD(8), .PCPD(9), .PCPD(10), .PCPD(11), _
                    .RAINHHMX(0), .RAINHHMX(1), .RAINHHMX(2), .RAINHHMX(3), .RAINHHMX(4), .RAINHHMX(5), .RAINHHMX(6), .RAINHHMX(7), .RAINHHMX(8), .RAINHHMX(9), .RAINHHMX(10), .RAINHHMX(11), _
                    .SOLARAV(0), .SOLARAV(1), .SOLARAV(2), .SOLARAV(3), .SOLARAV(4), .SOLARAV(5), .SOLARAV(6), .SOLARAV(7), .SOLARAV(8), .SOLARAV(9), .SOLARAV(10), .SOLARAV(11), _
                    .DEWPT(0), .DEWPT(1), .DEWPT(2), .DEWPT(3), .DEWPT(4), .DEWPT(5), .DEWPT(6), .DEWPT(7), .DEWPT(8), .DEWPT(9), .DEWPT(10), .DEWPT(11), _
                    .WNDAV(0), .WNDAV(1), .WNDAV(2), .WNDAV(3), .WNDAV(4), .WNDAV(5), .WNDAV(6), .WNDAV(7), .WNDAV(8), .WNDAV(9), .WNDAV(10), .WNDAV(11))
            End With
        End Sub

        'Public Sub Add(ByVal SUBBASIN As Integer, _
        '               ByVal STATION As String, _
        '               ByVal WLATITUDE As Single, _
        '               ByVal WLONGITUDE As Single, _
        '               ByVal WELEV As Single, _
        '               ByVal RAIN_YRS As Single, _
        '               ByVal TMPMX() As Single, _
        '               ByVal TMPMN() As Single, _
        '               ByVal TMPSTDMX() As Single, _
        '               ByVal TMPSTDMN() As Single, _
        '               ByVal PCPMM() As Single, _
        '               ByVal PCPSTD() As Single, _
        '               ByVal PCPSKW() As Single, _
        '               ByVal PR_W1_() As Single, _
        '               ByVal PR_W2_() As Single, _
        '               ByVal PCPD() As Single, _
        '               ByVal RAINHHMX() As Single, _
        '               ByVal SOLARAV() As Single, _
        '               ByVal DEWPT() As Single, _
        '               ByVal WNDAV() As Single)
        '    Me.Add(SUBBASIN, STATION, WLATITUDE, WLONGITUDE, WELEV, RAIN_YRS, _
        '            TMPMX(0), TMPMX(1), TMPMX(2), TMPMX(3), TMPMX(4), TMPMX(5), TMPMX(6), TMPMX(7), TMPMX(8), TMPMX(9), TMPMX(10), TMPMX(11), _
        '            TMPMN(0), TMPMN(1), TMPMN(2), TMPMN(3), TMPMN(4), TMPMN(5), TMPMN(6), TMPMN(7), TMPMN(8), TMPMN(9), TMPMN(10), TMPMN(11), _
        '            TMPSTDMX(0), TMPSTDMX(1), TMPSTDMX(2), TMPSTDMX(3), TMPSTDMX(4), TMPSTDMX(5), TMPSTDMX(6), TMPSTDMX(7), TMPSTDMX(8), TMPSTDMX(9), TMPSTDMX(10), TMPSTDMX(11), _
        '            TMPSTDMN(0), TMPSTDMN(1), TMPSTDMN(2), TMPSTDMN(3), TMPSTDMN(4), TMPSTDMN(5), TMPSTDMN(6), TMPSTDMN(7), TMPSTDMN(8), TMPSTDMN(9), TMPSTDMN(10), TMPSTDMN(11), _
        '            PCPMM(0), PCPMM(1), PCPMM(2), PCPMM(3), PCPMM(4), PCPMM(5), PCPMM(6), PCPMM(7), PCPMM(8), PCPMM(9), PCPMM(10), PCPMM(11), _
        '            PCPSTD(0), PCPSTD(1), PCPSTD(2), PCPSTD(3), PCPSTD(4), PCPSTD(5), PCPSTD(6), PCPSTD(7), PCPSTD(8), PCPSTD(9), PCPSTD(10), PCPSTD(11), _
        '            PCPSKW(0), PCPSKW(1), PCPSKW(2), PCPSKW(3), PCPSKW(4), PCPSKW(5), PCPSKW(6), PCPSKW(7), PCPSKW(8), PCPSKW(9), PCPSKW(10), PCPSKW(11), _
        '            PR_W1_(0), PR_W1_(1), PR_W1_(2), PR_W1_(3), PR_W1_(4), PR_W1_(5), PR_W1_(6), PR_W1_(7), PR_W1_(8), PR_W1_(9), PR_W1_(10), PR_W1_(11), _
        '            PR_W2_(0), PR_W2_(1), PR_W2_(2), PR_W2_(3), PR_W2_(4), PR_W2_(5), PR_W2_(6), PR_W2_(7), PR_W2_(8), PR_W2_(9), PR_W2_(10), PR_W2_(11), _
        '            PCPD(0), PCPD(1), PCPD(2), PCPD(3), PCPD(4), PCPD(5), PCPD(6), PCPD(7), PCPD(8), PCPD(9), PCPD(10), PCPD(11), _
        '            RAINHHMX(0), RAINHHMX(1), RAINHHMX(2), RAINHHMX(3), RAINHHMX(4), RAINHHMX(5), RAINHHMX(6), RAINHHMX(7), RAINHHMX(8), RAINHHMX(9), RAINHHMX(10), RAINHHMX(11), _
        '            SOLARAV(0), SOLARAV(1), SOLARAV(2), SOLARAV(3), SOLARAV(4), SOLARAV(5), SOLARAV(6), SOLARAV(7), SOLARAV(8), SOLARAV(9), SOLARAV(10), SOLARAV(11), _
        '            DEWPT(0), DEWPT(1), DEWPT(2), DEWPT(3), DEWPT(4), DEWPT(5), DEWPT(6), DEWPT(7), DEWPT(8), DEWPT(9), DEWPT(10), DEWPT(11), _
        '            WNDAV(0), WNDAV(1), WNDAV(2), WNDAV(3), WNDAV(4), WNDAV(5), WNDAV(6), WNDAV(7), WNDAV(8), WNDAV(9), WNDAV(10), WNDAV(11))
        'End Sub

        Public Sub Add(ByVal SUBBASIN As Integer, _
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

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing " & pTableName & " text ...")

            For Each lRow As DataRow In aTable.Rows
                Dim lSubBasin As String = lRow.Item("SUBBASIN")

                Dim lSB As New Text.StringBuilder
                lSB.AppendLine(" .Wgn file Subbasin: " & lSubBasin _
                             & " STATION NAME:" & lRow.Item(("STATION")) & " " _
                             & HeaderString())
                lSB.AppendLine("  LATITUDE =" & Format(lRow.Item(("WLATITUDE")), "0.00").PadLeft(7) _
                             & " LONGITUDE =" & Format(lRow.Item(("WLONGITUDE")), "0.00").PadLeft(7))
                lSB.AppendLine("  ELEV [m] =" & Format(lRow.Item(("WELEV")), "0.00").PadLeft(7))
                lSB.AppendLine("  RAIN_YRS =" & Format(lRow.Item(("RAIN_YRS")), "0.00").PadLeft(7))
                Append12TextColumns(lSB, lRow, "TMPMX")
                Append12TextColumns(lSB, lRow, "TMPMN")
                Append12TextColumns(lSB, lRow, "TMPSTDMX")
                Append12TextColumns(lSB, lRow, "TMPSTDMN")
                Append12TextColumns(lSB, lRow, "PCPMM")
                Append12TextColumns(lSB, lRow, "PCPSTD")
                Append12TextColumns(lSB, lRow, "PCPSKW")
                Append12TextColumns(lSB, lRow, "PR_W1_")
                Append12TextColumns(lSB, lRow, "PR_W2_")
                Append12TextColumns(lSB, lRow, "PCPD")
                Append12TextColumns(lSB, lRow, "RAINHHMX")
                Append12TextColumns(lSB, lRow, "SOLARAV")
                Append12TextColumns(lSB, lRow, "DEWPT")
                Append12TextColumns(lSB, lRow, "WNDAV")

                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & StringFname(lSubBasin, pTableName), lSB.ToString)
            Next
        End Sub

        Private Sub Append12TextColumns(ByVal aSB As Text.StringBuilder, ByVal aRow As DataRow, ByVal aSection As String)
            For i As Integer = 1 To 12
                aSB.Append(Format(aRow.Item(aSection & i), "0.00").PadLeft(6))
            Next
            aSB.AppendLine()
        End Sub

    End Class
End Class
