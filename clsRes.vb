Partial Class SwatInput
    Private pRes As clsRes = New clsRes(Me)
    ReadOnly Property Res() As clsRes
        Get
            Return pRes
        End Get
    End Property

    ''' <summary>
    ''' Gw Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsRes
        Private pSwatInput As SwatInput
        Private pTableName As String = "res"

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn.DBLayer.createRes
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
                    .Append("MORES", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IYRES", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("RES_ESA", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_EVOL", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_PSA", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_PVOL", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_VOL", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_SED", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_NSED", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_K", ADOX.DataTypeEnum.adSingle)
                    .Append("IRESCO", ADOX.DataTypeEnum.adInteger, 4)

                    Append12DBColumnsDouble(lTable.Columns, "OFLOWMX")
                    Append12DBColumnsDouble(lTable.Columns, "OFLOWMN")

                    .Append("RES_RR", ADOX.DataTypeEnum.adSingle)
                    .Append("RESMONO", ADOX.DataTypeEnum.adVarWChar, 254)
                    .Append("IFLOOD1R", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IFLOOD2R", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("NDTARGR", ADOX.DataTypeEnum.adInteger, 4)

                    Append12DBColumnsDouble(lTable.Columns, "STARG")

                    .Append("RESDAYO", ADOX.DataTypeEnum.adVarWChar, 254)

                    Append12DBColumnsDouble(lTable.Columns, "WURESN")

                    .Append("WURTNF", ADOX.DataTypeEnum.adSingle)
                    .Append("IRES1", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IRES2", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("PSETLR1", ADOX.DataTypeEnum.adSingle)
                    .Append("PSETLR2", ADOX.DataTypeEnum.adSingle)
                    .Append("NSETLR1", ADOX.DataTypeEnum.adSingle)
                    .Append("NSETLR2", ADOX.DataTypeEnum.adSingle)
                    .Append("CHLAR", ADOX.DataTypeEnum.adSingle)
                    .Append("SECCIR", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_ORGP", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_SOLP", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_ORGN", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_NO3", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_NH3", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_NO2", ADOX.DataTypeEnum.adSingle)
                    .Append("LKPST_CONC", ADOX.DataTypeEnum.adSingle)
                    .Append("LKPST_REA", ADOX.DataTypeEnum.adSingle)
                    .Append("LKPST_VOL", ADOX.DataTypeEnum.adSingle)
                    .Append("LKPST_KOC", ADOX.DataTypeEnum.adSingle)
                    .Append("LKPST_STL", ADOX.DataTypeEnum.adSingle)
                    .Append("LKPST_RSP", ADOX.DataTypeEnum.adSingle)
                    .Append("LKPST_MIX", ADOX.DataTypeEnum.adSingle)
                    .Append("LKSPSTCONC", ADOX.DataTypeEnum.adSingle)
                    .Append("LKSPST_REA", ADOX.DataTypeEnum.adSingle)
                    .Append("LKSPST_BRY", ADOX.DataTypeEnum.adSingle)
                    .Append("LKSPST_ACT", ADOX.DataTypeEnum.adSingle)
                    .Append("RES_D50", ADOX.DataTypeEnum.adSingle)
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

        Public Sub Add(ByVal SUBBASIN As Double, _
                ByVal MORES As Long, _
                ByVal IYRES As Long, _
                ByVal RES_ESA As Single, _
                ByVal RES_EVOL As Single, _
                ByVal RES_PSA As Single, _
                ByVal RES_PVOL As Single, _
                ByVal RES_VOL As Single, _
                ByVal RES_SED As Single, _
                ByVal RES_NSED As Single, _
                ByVal RES_K As Single, _
                ByVal IRESCO As Long, _
                ByVal OFLOWMX() As Double, _
                ByVal OFLOWMN() As Double, _
                ByVal RES_RR As Single, _
                ByVal RESMONO As String, _
                ByVal IFLOOD1R As Long, _
                ByVal IFLOOD2R As Long, _
                ByVal NDTARGR As Long, _
                ByVal STARG() As Double, _
                ByVal RESDAYO As String, _
                ByVal WURESN() As Double, _
                ByVal WURTNF As Single, _
                ByVal IRES1 As Long, _
                ByVal IRES2 As Long, _
                ByVal PSETLR1 As Single, _
                ByVal PSETLR2 As Single, _
                ByVal NSETLR1 As Single, _
                ByVal NSETLR2 As Single, _
                ByVal CHLAR As Single, _
                ByVal SECCIR As Single, _
                ByVal RES_ORGP As Single, _
                ByVal RES_SOLP As Single, _
                ByVal RES_ORGN As Single, _
                ByVal RES_NO3 As Single, _
                ByVal RES_NH3 As Single, _
                ByVal RES_NO2 As Single, _
                ByVal LKPST_CONC As Single, _
                ByVal LKPST_REA As Single, _
                ByVal LKPST_VOL As Single, _
                ByVal LKPST_KOC As Single, _
                ByVal LKPST_STL As Single, _
                ByVal LKPST_RSP As Single, _
                ByVal LKPST_MIX As Single, _
                ByVal LKSPSTCONC As Single, _
                ByVal LKSPST_REA As Single, _
                ByVal LKSPST_BRY As Single, _
                ByVal LKSPST_ACT As Single, _
                ByVal RES_D50 As Single)
            Me.Add(SUBBASIN, MORES, IYRES, RES_ESA, RES_EVOL, RES_PSA, _
                   RES_PVOL, RES_VOL, RES_SED, RES_NSED, RES_K, IRESCO, _
                   OFLOWMX(0), OFLOWMX(1), OFLOWMX(2), OFLOWMX(3), OFLOWMX(4), OFLOWMX(5), OFLOWMX(6), OFLOWMX(7), OFLOWMX(8), OFLOWMX(9), OFLOWMX(10), OFLOWMX(11), _
                   OFLOWMN(0), OFLOWMN(1), OFLOWMN(2), OFLOWMN(3), OFLOWMN(4), OFLOWMN(5), OFLOWMN(6), OFLOWMN(7), OFLOWMN(8), OFLOWMN(9), OFLOWMN(10), OFLOWMN(11), _
                   RES_RR, RESMONO, IFLOOD1R, IFLOOD2R, NDTARGR, _
                   STARG(0), STARG(1), STARG(2), STARG(3), STARG(4), STARG(5), STARG(6), STARG(7), STARG(8), STARG(9), STARG(10), STARG(11), _
                   RESDAYO, _
                   WURESN(0), WURESN(1), WURESN(2), WURESN(3), WURESN(4), WURESN(5), WURESN(6), WURESN(7), WURESN(8), WURESN(9), WURESN(10), WURESN(11), _
                   WURTNF, IRES1, IRES2, PSETLR1, PSETLR2, NSETLR1, NSETLR2, CHLAR, SECCIR, _
                   RES_ORGP, RES_SOLP, RES_ORGN, RES_NO3, RES_NH3, RES_NO2, _
                   LKPST_CONC, LKPST_REA, LKPST_VOL, LKPST_KOC, LKPST_STL, LKPST_RSP, LKPST_MIX, LKSPSTCONC, LKSPST_REA, LKSPST_BRY, LKSPST_ACT, RES_D50)
        End Sub

        Public Sub Add(ByVal SUBBASIN As Double, _
                        ByVal MORES As Long, _
                        ByVal IYRES As Long, _
                        ByVal RES_ESA As Single, _
                        ByVal RES_EVOL As Single, _
                        ByVal RES_PSA As Single, _
                        ByVal RES_PVOL As Single, _
                        ByVal RES_VOL As Single, _
                        ByVal RES_SED As Single, _
                        ByVal RES_NSED As Single, _
                        ByVal RES_K As Single, _
                        ByVal IRESCO As Long, _
                        ByVal OFLOWMX1 As Double, _
                        ByVal OFLOWMX2 As Double, _
                        ByVal OFLOWMX3 As Double, _
                        ByVal OFLOWMX4 As Double, _
                        ByVal OFLOWMX5 As Double, _
                        ByVal OFLOWMX6 As Double, _
                        ByVal OFLOWMX7 As Double, _
                        ByVal OFLOWMX8 As Double, _
                        ByVal OFLOWMX9 As Double, _
                        ByVal OFLOWMX10 As Double, _
                        ByVal OFLOWMX11 As Double, _
                        ByVal OFLOWMX12 As Double, _
                        ByVal OFLOWMN1 As Double, _
                        ByVal OFLOWMN2 As Double, _
                        ByVal OFLOWMN3 As Double, _
                        ByVal OFLOWMN4 As Double, _
                        ByVal OFLOWMN5 As Double, _
                        ByVal OFLOWMN6 As Double, _
                        ByVal OFLOWMN7 As Double, _
                        ByVal OFLOWMN8 As Double, _
                        ByVal OFLOWMN9 As Double, _
                        ByVal OFLOWMN10 As Double, _
                        ByVal OFLOWMN11 As Double, _
                        ByVal OFLOWMN12 As Double, _
                        ByVal RES_RR As Single, _
                        ByVal RESMONO As String, _
                        ByVal IFLOOD1R As Long, _
                        ByVal IFLOOD2R As Long, _
                        ByVal NDTARGR As Long, _
                        ByVal STARG1 As Double, _
                        ByVal STARG2 As Double, _
                        ByVal STARG3 As Double, _
                        ByVal STARG4 As Double, _
                        ByVal STARG5 As Double, _
                        ByVal STARG6 As Double, _
                        ByVal STARG7 As Double, _
                        ByVal STARG8 As Double, _
                        ByVal STARG9 As Double, _
                        ByVal STARG10 As Double, _
                        ByVal STARG11 As Double, _
                        ByVal STARG12 As Double, _
                        ByVal RESDAYO As String, _
                        ByVal WURESN1 As Double, _
                        ByVal WURESN2 As Double, _
                        ByVal WURESN3 As Double, _
                        ByVal WURESN4 As Double, _
                        ByVal WURESN5 As Double, _
                        ByVal WURESN6 As Double, _
                        ByVal WURESN7 As Double, _
                        ByVal WURESN8 As Double, _
                        ByVal WURESN9 As Double, _
                        ByVal WURESN10 As Double, _
                        ByVal WURESN11 As Double, _
                        ByVal WURESN12 As Double, _
                        ByVal WURTNF As Single, _
                        ByVal IRES1 As Long, _
                        ByVal IRES2 As Long, _
                        ByVal PSETLR1 As Single, _
                        ByVal PSETLR2 As Single, _
                        ByVal NSETLR1 As Single, _
                        ByVal NSETLR2 As Single, _
                        ByVal CHLAR As Single, _
                        ByVal SECCIR As Single, _
                        ByVal RES_ORGP As Single, _
                        ByVal RES_SOLP As Single, _
                        ByVal RES_ORGN As Single, _
                        ByVal RES_NO3 As Single, _
                        ByVal RES_NH3 As Single, _
                        ByVal RES_NO2 As Single, _
                        ByVal LKPST_CONC As Single, _
                        ByVal LKPST_REA As Single, _
                        ByVal LKPST_VOL As Single, _
                        ByVal LKPST_KOC As Single, _
                        ByVal LKPST_STL As Single, _
                        ByVal LKPST_RSP As Single, _
                        ByVal LKPST_MIX As Single, _
                        ByVal LKSPSTCONC As Single, _
                        ByVal LKSPST_REA As Single, _
                        ByVal LKSPST_BRY As Single, _
                        ByVal LKSPST_ACT As Single, _
                        ByVal RES_D50 As Single)

            Dim lSQL As String = "INSERT INTO res ( SUBBASIN , MORES , IYRES , RES_ESA , RES_EVOL , RES_PSA , RES_PVOL , RES_VOL , RES_SED , RES_NSED , RES_K , IRESCO , OFLOWMX1 , OFLOWMX2 , OFLOWMX3 , OFLOWMX4 , OFLOWMX5 , OFLOWMX6 , OFLOWMX7 , OFLOWMX8 , OFLOWMX9 , OFLOWMX10 , OFLOWMX11 , OFLOWMX12 , OFLOWMN1 , OFLOWMN2 , OFLOWMN3 , OFLOWMN4 , OFLOWMN5 , OFLOWMN6 , OFLOWMN7 , OFLOWMN8 , OFLOWMN9 , OFLOWMN10 , OFLOWMN11 , OFLOWMN12 , RES_RR , RESMONO , IFLOOD1R , IFLOOD2R , NDTARGR , STARG1 , STARG2 , STARG3 , STARG4 , STARG5 , STARG6 , STARG7 , STARG8 , STARG9 , STARG10 , STARG11 , STARG12 , RESDAYO , WURESN1 , WURESN2 , WURESN3 , WURESN4 , WURESN5 , WURESN6 , WURESN7 , WURESN8 , WURESN9 , WURESN10 , WURESN11 , WURESN12 , WURTNF , IRES1 , IRES2 , PSETLR1 , PSETLR2 , NSETLR1 , NSETLR2 , CHLAR , SECCIR , RES_ORGP , RES_SOLP , RES_ORGN , RES_NO3 , RES_NH3 , RES_NO2 , LKPST_CONC , LKPST_REA , LKPST_VOL , LKPST_KOC , LKPST_STL , LKPST_RSP , LKPST_MIX , LKSPSTCONC , LKSPST_REA , LKSPST_BRY , LKSPST_ACT , RES_D50  ) " _
                               & "Values ('" & SUBBASIN & "'  ,'" & MORES & "'  ,'" & IYRES & "'  ,'" & RES_ESA & "'  ,'" & RES_EVOL & "'  ,'" & RES_PSA & "'  ,'" & RES_PVOL & "'  ,'" & RES_VOL & "'  ,'" & RES_SED & "'  ,'" & RES_NSED & "'  ,'" & RES_K & "'  ,'" & IRESCO & "'  ,'" & OFLOWMX1 & "'  ,'" & OFLOWMX2 & "'  ,'" & OFLOWMX3 & "'  ,'" & OFLOWMX4 & "'  ,'" & OFLOWMX5 & "'  ,'" & OFLOWMX6 & "'  ,'" & OFLOWMX7 & "'  ,'" & OFLOWMX8 & "'  ,'" & OFLOWMX9 & "'  ,'" & OFLOWMX10 & "'  ,'" & OFLOWMX11 & "'  ,'" & OFLOWMX12 & "'  ,'" & OFLOWMN1 & "'  ,'" & OFLOWMN2 & "'  ,'" & OFLOWMN3 & "'  ,'" & OFLOWMN4 & "'  ,'" & OFLOWMN5 & "'  ,'" & OFLOWMN6 & "'  ,'" & OFLOWMN7 & "'  ,'" & OFLOWMN8 & "'  ,'" & OFLOWMN9 & "'  ,'" & OFLOWMN10 & "'  ,'" & OFLOWMN11 & "'  ,'" & OFLOWMN12 & "'  ,'" & RES_RR & "'  ,'" & RESMONO & "'  ,'" & IFLOOD1R & "'  ,'" & IFLOOD2R & "'  ,'" & NDTARGR & "'  ,'" & STARG1 & "'  ,'" & STARG2 & "'  ,'" & STARG3 & "'  ,'" & STARG4 & "'  ,'" & STARG5 & "'  ,'" & STARG6 & "'  ,'" & STARG7 & "'  ,'" & STARG8 & "'  ,'" & STARG9 & "'  ,'" & STARG10 & "'  ,'" & STARG11 & "'  ,'" & STARG12 & "'  ,'" & RESDAYO & "'  ,'" & WURESN1 & "'  ,'" & WURESN2 & "'  ,'" & WURESN3 & "'  ,'" & WURESN4 & "'  ,'" & WURESN5 & "'  ,'" & WURESN6 & "'  ,'" & WURESN7 & "'  ,'" & WURESN8 & "'  ,'" & WURESN9 & "'  ,'" & WURESN10 & "'  ,'" & WURESN11 & "'  ,'" & WURESN12 & "'  ,'" & WURTNF & "'  ,'" & IRES1 & "'  ,'" & IRES2 & "'  ,'" & PSETLR1 & "'  ,'" & PSETLR2 & "'  ,'" & NSETLR1 & "'  ,'" & NSETLR2 & "'  ,'" & CHLAR & "'  ,'" & SECCIR & "'  ,'" & RES_ORGP & "'  ,'" & RES_SOLP & "'  ,'" & RES_ORGN & "'  ,'" & RES_NO3 & "'  ,'" & RES_NH3 & "'  ,'" & RES_NO2 & "'  ,'" & LKPST_CONC & "'  ,'" & LKPST_REA & "'  ,'" & LKPST_VOL & "'  ,'" & LKPST_KOC & "'  ,'" & LKPST_STL & "'  ,'" & LKPST_RSP & "'  ,'" & LKPST_MIX & "'  ,'" & LKSPSTCONC & "'  ,'" & LKSPST_REA & "'  ,'" & LKSPST_BRY & "'  ,'" & LKSPST_ACT & "'  ,'" & RES_D50 & "'  )"
            Dim lCommand As New System.Data.OleDb.OleDbCommand(lSQL, pSwatInput.CnSwatInput)
            lCommand.ExecuteNonQuery()
        End Sub

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()

            pSwatInput.Status("Writing " & pTableName & " text ...")

            For Each lRow As DataRow In aTable.Rows
                Dim lSubBasin As String = lRow.Item(1)
                Dim lHruNum As String = lRow.Item(2)
                Dim lTextFilename As String = StringFnameHRUs(lSubBasin, lHruNum) & "." & pTableName

                Dim lSB As New Text.StringBuilder
                lSB.AppendLine("Reservoir data:" & Space(10) & " .res file " & "Subbasin " & lSubBasin & " " & HeaderString())

                lSB.AppendLine(lSubBasin.Substring(0, 8).PadLeft(16) & "    | RES_SUB : Number of the subbasin the reservoir is in")
                lSB.AppendLine(CStr(lRow.Item(2)).PadLeft(16) & "    | MORES : Month the reservoir became operational (1-12)")
                lSB.AppendLine(CStr(lRow.Item(3)).PadLeft(16) & "    | IYRES : Year of the simulation the reservoir became operational ")
                lSB.AppendLine(CStr(lRow.Item(4)).PadLeft(16) & "    | RES_ESA : Reservoir surface area when the reservoir is filled to the  emergency spillway [ha]")
                lSB.AppendLine(FormatNumber(lRow.Item(5), 1).PadLeft(16) & "    | RES_EVOL : Volume of water needed to fill the reservoir to the emergency spillway (104 m3)")
                lSB.AppendLine(FormatNumber(lRow.Item(6), 1).PadLeft(16) & "    | RES_PSA : Reservoir surface area when the reservoir is filled to the principal spillway [ha]")
                lSB.AppendLine(FormatNumber(lRow.Item(7), 1).PadLeft(16) & "    | RES_PVOL : Volume of water needed to fill the reservoir to the principal spillway [104 m3]")
                lSB.AppendLine(FormatNumber(lRow.Item(8), 1).PadLeft(16) & "    | RES_VOL : Initial reservoir volume [104 m3]")
                lSB.AppendLine(FormatNumber(lRow.Item(9), 1).PadLeft(16) & "    | RES_SED : Initial sediment concentration in the reservoir [mg/l]")
                lSB.AppendLine(FormatNumber(lRow.Item(10), 1).PadLeft(16) & "    | RES_NSED : Normal sediment concentration in the reservoir [mg/l]")
                lSB.AppendLine(FormatNumber(lRow.Item(93), 1).PadLeft(16) & "    | RES_D50 : Median particle diameter of sediment [um]")
                lSB.AppendLine(FormatNumber(lRow.Item(11), 1).PadLeft(16) & "    | RES_K : Hydraulic conductivity of the reservoir bottom [mm/hr]")
                lSB.AppendLine(FormatNumber(lRow.Item(12), 1).PadLeft(16) & "    | IRESCO : Outflow simulation code")
                lSB.AppendLine("OFLOWMX: maximum daily outflow for January - June [m3/s]")
                Append6(lSB, lRow, 13)

                lSB.AppendLine("OFLOWMX: maximum daily outflow for July - December [m3/s]")
                Append6(lSB, lRow, 19)

                lSB.AppendLine("OFLOWMN: minimum daily outflow for January - June [m3/s]")
                Append6(lSB, lRow, 25)

                lSB.AppendLine("OFLOWMN: minimum daily outflow for July - December [m3/s]")
                Append6(lSB, lRow, 31)

                lSB.AppendLine(FormatNumber(lRow.Item(37), 1).PadLeft(16) & "    | RES_RR : Average daily principal spillway release rate [m3/s]")

                If lRow.Item(12) = 1 Then
                    lSB.Append(IO.Path.ChangeExtension(lTextFilename, "mon").PadLeft(13))
                Else
                    lSB.Append(Space(13))
                End If
                lSB.AppendLine("       | RESMONO : Name of monthly reservoir outflow file")

                lSB.AppendLine(lRow.Item(39).PadLeft(16) & "    | IFLOD1R : Beginning month of non-flood season")
                lSB.AppendLine(lRow.Item(40).PadLeft(16) & "    | IFLOD2R : Ending month of non-flood season")
                lSB.AppendLine(lRow.Item(41).PadLeft(16) & "    | NDTARGR : Number of days to reach target storage from current reservoir storage")

                lSB.AppendLine("STARG: target reservoir storage for January - June [10^4 m3]")
                Append6(lSB, lRow, 42)

                lSB.AppendLine("STARG: target reservoir storage for July - December [10^4 m3]")
                Append6(lSB, lRow, 48)

                If lRow.Item(12) = 3 Then
                    lSB.Append(IO.Path.ChangeExtension(lTextFilename, "day").PadLeft(13))
                Else
                    lSB.Append(Space(13))
                End If
                lSB.AppendLine("       | RESDAYO : Name of daily reservoir outflow file")

                lSB.AppendLine("WURESN: consumptive water use  for January - June [10^4 m3]")
                Append6(lSB, lRow, 55)

                lSB.AppendLine("WURESN: consumptive water use  for July - December [10^4 m3]")
                Append6(lSB, lRow, 61)

                lSB.AppendLine(lRow.Item(67).PadLeft(16) & "    | WURTNF : Fraction of water removed from the reservoir via WURESN that is returned and becomes flow out of reservoir [m3/m3]")

                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & lTextFilename, lSB.ToString)
                'ReplaceNonAscii(lTextFilename)

                'Write lwq file ==================================================================================================================================
                lSB = New Text.StringBuilder
                lTextFilename = IO.Path.ChangeExtension(lTextFilename, "lwq")

                lSB.AppendLine("Reservoir water quality data:" & Space(10) & " .lwq file " & "Subbasin " & lSubBasin & " " & HeaderString())
                lSB.AppendLine("Nutrient information:")
                lSB.AppendLine(lRow.Item(68).PadLeft(16) & "    | IRES1 : Beginning month of mid-year nutrient settling period")
                lSB.AppendLine(lRow.Item(69).PadLeft(16) & "    | IRES2 : Ending month of mid-year nutrient settling period")
                lSB.AppendLine(FormatNumber(lRow.Item(70), 3).PadLeft(16) & "    | PSETLR1 : Phosphorus settling rate in reservoir for months IRES1 through IRES2 [m/year]")
                lSB.AppendLine(FormatNumber(lRow.Item(71), 3).PadLeft(16) & "    | PSETLR2 : Phosphorus settling rate in reservoir for months other than IRES1-IRES2 [m/year]")
                lSB.AppendLine(FormatNumber(lRow.Item(72), 3).PadLeft(16) & "    | NSETLR1 : Nitrogen settling rate in reservoir for months IRES1 through IRES2 [m/year]")
                lSB.AppendLine(FormatNumber(lRow.Item(73), 3).PadLeft(16) & "    | NSETLR2 : Nitrogen settling rate in reservoir for months other than IRES1-IRES2 [m/year]")
                lSB.AppendLine(FormatNumber(lRow.Item(74), 3).PadLeft(16) & "    | CHLAR : Chlorophyll a production coefficient for reservoir")
                lSB.AppendLine(FormatNumber(lRow.Item(75), 3).PadLeft(16) & "    | SECCIR : Water clarity coefficient for the reservoir")
                lSB.AppendLine(FormatNumber(lRow.Item(76), 3).PadLeft(16) & "    | RES_ORGP : Initial concentration of organic P in reservoir [mg P/l]")
                lSB.AppendLine(FormatNumber(lRow.Item(77), 3).PadLeft(16) & "    | RES_SOLP : Initial concentration of soluble P in reservoir [mg P/l]")
                lSB.AppendLine(FormatNumber(lRow.Item(78), 3).PadLeft(16) & "    | RES_ORGN : Initial concentration of organic N in reservoir [mg N/l]")
                lSB.AppendLine(FormatNumber(lRow.Item(79), 3).PadLeft(16) & "    | RES_NO3 : Initial concentration of NO3-N in reservoir [mg N/L]")
                lSB.AppendLine(FormatNumber(lRow.Item(80), 3).PadLeft(16) & "    | RES_NH3 : Initial concentration of NH3-N in reservoir [mg N/l]")
                lSB.AppendLine(FormatNumber(lRow.Item(81), 3).PadLeft(16) & "    | RES_NO2 : Initial concentration of NO2-N in reservoir [mg N/l]")
                lSB.AppendLine("Pesticide information:")
                lSB.AppendLine(FormatNumber(lRow.Item(82), 3).PadLeft(16) & "    | LKPST_CONC : Initial pesticide concentration in the lake water for the first pesticide listed in file.cio. While up to ten pesticides may be applied in a SWAT simulation, only one pesticide (the first one listed in the file.cio) is routed through the reach. (mg/m3)")
                lSB.AppendLine(FormatNumber(lRow.Item(83), 3).PadLeft(16) & "    | LKPST_REA : Reaction coefficient of the pesticide in lake water [day-1]")
                lSB.AppendLine(FormatNumber(lRow.Item(84), 3).PadLeft(16) & "    | LKPST_VOL : Volatilization coefficient of the pesticide from the lake water [m/day]")
                lSB.AppendLine(FormatNumber(lRow.Item(85), 3).PadLeft(16) & "    | LKPST_KOC : Partition coefficient [m3/day]")
                lSB.AppendLine(FormatNumber(lRow.Item(86), 3).PadLeft(16) & "    | LKPST_STL : Settling velocity of pesticide sorbed to sediment [m/day]")
                lSB.AppendLine(FormatNumber(lRow.Item(87), 3).PadLeft(16) & "    | LKPST_RSP : Resuspension velocity of pesticide sorbed to sediment [m/day]")
                lSB.AppendLine(FormatNumber(lRow.Item(88), 3).PadLeft(16) & "    | LKPST_MIX : Mixing velocity [m/day]")
                lSB.AppendLine(FormatNumber(lRow.Item(89), 3).PadLeft(16) & "    | LKPST_CONC : Initial pesticide concentration in the lake bottom sediments [mg/m3]")
                lSB.AppendLine(FormatNumber(lRow.Item(90), 3).PadLeft(16) & "    | LKPST_REA : Reaction coefficient of pesticide in lake bottom sediment [day-1]")
                lSB.AppendLine(FormatNumber(lRow.Item(91), 3).PadLeft(16) & "    | LKPST_BRY : Burial velocity of pesticide in lake bottom sediment [m/day]")
                lSB.AppendLine(FormatNumber(lRow.Item(92), 3).PadLeft(16) & "    | LKPST_ACT : Depth of active sediment layer in lake [m]")

                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\" & lTextFilename, lSB.ToString)
                'ReplaceNonAscii(lTextFilename)
            Next
        End Sub

        Private Sub Append6(ByVal aSB As Text.StringBuilder, ByVal aRow As DataRow, ByVal aStartColumn As Integer)
            For lColumn As Integer = aStartColumn To aStartColumn + 5
                aSB.Append(FormatNumber(aRow.Item(lColumn), 1).PadLeft(10))
            Next
            aSB.AppendLine()
        End Sub

    End Class
End Class
