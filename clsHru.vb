Partial Class SwatInput
    Private pHru As clsHru = New clsHru(Me)
    ReadOnly Property Hru() As clsHru
        Get
            Return pHru
        End Get
    End Property

    Public Class clsHruItem
        Public SUBBASIN As Double
        Public HRU As Double
        Public LANDUSE As String
        Public SOIL As String
        Public SLOPE_CD As String
        Public HRU_FR As Double
        Public SLSUBBSN As Single
        Public HRU_SLP As Single
        Public OV_N As Single
        Public LAT_TTIME As Single
        Public LAT_SED As Single
        Public SLSOIL As Single
        Public CANMX As Single
        Public ESCO As Single
        Public EPCO As Single
        Public RSDIN As Single
        Public ERORGN As Single
        Public ERORGP As Single
        Public POT_FR As Single
        Public FLD_FR As Single
        Public RIP_FR As Single
        Public POT_TILE As Single
        Public POT_VOLX As Single
        Public POT_VOL As Single
        Public POT_NSED As Single
        Public POT_NO3L As Single
        Public DEP_IMP As Long

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

        Sub New(ByVal aRow As DataRow)
            SUBBASIN = aRow.Item("SUBBASIN")
            HRU = aRow.item("HRU")
            LANDUSE = aRow.item("LANDUSE")
            SOIL = aRow.item("SOIL")
            SLOPE_CD = aRow.item("SLOPE_CD")
            HRU_FR = aRow.item("HRU_FR")
            SLSUBBSN = aRow.item("SLSUBBSN")
            HRU_SLP = aRow.item("HRU_SLP")
            OV_N = aRow.item("OV_N")
            LAT_TTIME = aRow.item("LAT_TTIME")
            LAT_SED = aRow.item("LAT_SED")
            SLSOIL = aRow.item("SLSOIL")
            CANMX = aRow.item("CANMX")
            ESCO = aRow.item("ESCO")
            EPCO = aRow.item("EPCO")
            RSDIN = aRow.item("RSDIN")
            ERORGN = aRow.item("ERORGN")
            ERORGP = aRow.item("ERORGP")
            POT_FR = aRow.item("POT_FR")
            FLD_FR = aRow.item("FLD_FR")
            RIP_FR = aRow.item("RIP_FR")
            POT_TILE = aRow.item("POT_TILE")
            POT_VOLX = aRow.item("POT_VOLX")
            POT_VOL = aRow.item("POT_VOL")
            POT_NSED = aRow.item("POT_NSED")
            POT_NO3L = aRow.item("POT_NO3L")
            DEP_IMP = aRow.item("DEP_IMP")
        End Sub

        Public Function AddSQL() As String
            Return "INSERT INTO hru ( " & clsDataColumn.ColumnNames(clsHru.Columns) & ") " _
                 & "Values ('" & SUBBASIN & "', '" & HRU & "', '" & LANDUSE & "', '" & SOIL & "', '" & SLOPE_CD & "', '" _
                 & HRU_FR & "', '" & SLSUBBSN & "', '" & HRU_SLP & "', '" & OV_N & "', '" & LAT_TTIME & "', '" & LAT_SED & "', '" _
                 & SLSOIL & "', '" & CANMX & "', '" & ESCO & "', '" & EPCO & "', '" & RSDIN & "', '" _
                 & ERORGN & "', '" & ERORGP & "', '" & POT_FR & "', '" & FLD_FR & "', '" & RIP_FR & "', '" _
                 & POT_TILE & "', '" & POT_VOLX & "', '" & POT_VOL & "', '" & POT_NSED & "', '" & POT_NO3L & "', '" & DEP_IMP & "'  )"
        End Function
    End Class

    ''' <summary>
    ''' Hydrologic Response Unit (HRU) input section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsHru
        Private pSwatInput As SwatInput
        Private pTableName As String = "hru"

        Friend Shared Columns As Generic.List(Of clsDataColumn)

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
            If Columns Is Nothing Then InitColumns()
        End Sub

        Private Shared Sub InitColumns()
            Columns = New Generic.List(Of clsDataColumn)
            Columns.Add(New clsDataColumn("SUBBASIN", 1, "Double", "", 0, ""))
            Columns.Add(New clsDataColumn("HRU", 1, "Double", "", 0, ""))
            Columns.Add(New clsDataColumn("LANDUSE", 1, "VARCHAR(4)", "%s", 0, ""))
            Columns.Add(New clsDataColumn("SOIL", 1, "VARCHAR(40)", "%s", 0, ""))
            Columns.Add(New clsDataColumn("SLOPE_CD", 1, "VARCHAR(20)", "%s", 0, ""))
            Columns.Add(New clsDataColumn("HRU_FR", 1, "Double", "0.0000000", 16, "    | HRU_FR : Fraction of subbasin area contained in HRU"))
            Columns.Add(New clsDataColumn("SLSUBBSN", 1, "Single", "0.000", 16, "    | SLSUBBSN : Average slope length [m]"))
            Columns.Add(New clsDataColumn("HRU_SLP", 1, "Single", "0.000", 16, "    | HRU_SLP : Average slope stepness [m/m]"))
            Columns.Add(New clsDataColumn("OV_N", 1, "Single", "0.000", 16, "    | OV_N : Manning's ""n"" value for overland flow"))
            Columns.Add(New clsDataColumn("LAT_TTIME", 1, "Single", "0.000", 16, "    | LAT_TTIME : Lateral flow travel time [days]"))
            Columns.Add(New clsDataColumn("LAT_SED", 1, "Single", "0.000", 16, "    | LAT_SED : Sediment concentration in lateral flow and groundwater flow [mg/l]"))
            Columns.Add(New clsDataColumn("SLSOIL", 1, "Single", "0.000", 16, "    | SLSOIL : Slope length for lateral subsurface flow [m]"))
            Columns.Add(New clsDataColumn("CANMX", 1, "Single", "0.000", 16, "    | CANMX : Maximum canopy storage [mm]"))
            Columns.Add(New clsDataColumn("ESCO", 1, "Single", "0.000", 16, "    | ESCO : Soil evaporation compensation factor"))
            Columns.Add(New clsDataColumn("EPCO", 1, "Single", "0.000", 16, "    | EPCO : Plant uptake compensation factor"))
            Columns.Add(New clsDataColumn("RSDIN", 1, "Single", "0.000", 16, "    | RSDIN : Initial residue cover [kg/ha]"))
            Columns.Add(New clsDataColumn("ERORGN", 1, "Single", "0.000", 16, "    | ERORGN : Organic N enrichment ratio"))
            Columns.Add(New clsDataColumn("ERORGP", 1, "Single", "0.000", 16, "    | ERORGP : Organic P enrichment ratio"))
            Columns.Add(New clsDataColumn("POT_FR", 1, "Single", "0.000", 16, "    | POT_FR : Fraction of HRU are that drains into pothole"))
            Columns.Add(New clsDataColumn("FLD_FR", 1, "Single", "0.000", 16, "    | FLD_FR : Fraction of HRU that drains into floodplain"))
            Columns.Add(New clsDataColumn("RIP_FR", 1, "Single", "0.000", 16, "    | RIP_FR : Fraction of HRU that drains into riparian zone"))
            Columns.Add(New clsDataColumn("POT_TILE", 1, "Single", "0.000", 16, "    | POT_TILE : Average daily outflow to main channel from tile flow [m3/s]"))
            Columns.Add(New clsDataColumn("POT_VOLX", 1, "Single", "0.000", 16, "    | POT_VOLX : Maximum volume of water stored in the pothole [104m3]"))
            Columns.Add(New clsDataColumn("POT_VOL", 1, "Single", "0.000", 16, "    | POT_VOL : Initial volume of water stored in pothole [104m3]"))
            Columns.Add(New clsDataColumn("POT_NSED", 1, "Single", "0.000", 16, "    | POT_NSED : Normal sediment concentration in pothole [mg/l]"))
            Columns.Add(New clsDataColumn("POT_NO3L", 1, "Single", "0.000", 16, "    | POT_NO3L : Nitrate decay rate in pothole [1/day]"))
            Columns.Add(New clsDataColumn("DEP_IMP", 1, "Long", "0", 16, "    | DEP_IMP : Depth to impervious layer in soil profile [mm]"))
        End Sub

        Public Function TableCreate() As Boolean
            Return CreateTable(pTableName, "OID", Columns, pSwatInput.CnSwatInput)
        End Function

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & " ORDER BY SUBBASIN, HRU;")
        End Function

        Public Sub Add(ByVal aItem As clsHruItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatInput)
        End Sub

        ''' <summary>
        ''' Save HRU information to set of .hru text input files
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing " & pTableName & " text ...")
            For Each lRow As DataRow In aTable.Rows
                'Dim lHruItem As New clsHruItem(lRow)
                Dim lSubBasin As String = lRow.Item(1).ToString.Trim
                Dim lHruNum As String = lRow.Item(2).ToString.Trim
                Dim lHruName As String = StringFnameHRUs(lSubBasin, lHruNum) & "." & pTableName
                Dim lSB As New System.Text.StringBuilder
                lSB.AppendLine(" .hru file Subbasin:" & lSubBasin & " HRU:" & lHruNum _
                             & " Luse:" & lRow.Item(3) & " Soil: " & lRow.Item(4) & " Slope " & lRow.Item(5) _
                             & " " & HeaderString())
                For lColumn As Integer = 6 To 27
                    If lColumn = 22 Then lSB.AppendLine("Special HRU: Pothole")
                    With Columns(lColumn - 1)
                        lSB.AppendLine(Format(lRow.Item(lColumn), .TextFormat).PadLeft(.TextPad) & .TextSuffix)
                    End With
                Next

                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & lHruName, lSB.ToString)
            Next
        End Sub

        Public Function UniqueValues(ByVal aFieldName As String) As DataTable
            Return pSwatInput.QueryInputDB("Select hru." & aFieldName & " FROM(hru) GROUP BY hru." & aFieldName & " ORDER BY hru." & aFieldName & ";")
        End Function
    End Class
End Class
