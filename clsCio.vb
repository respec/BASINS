Partial Class SwatInput
    ''' <summary>
    ''' Master Watershed (CIO) input section
    ''' </summary>
    ''' <remarks></remarks>
    Private pCIO As clsCIO = New clsCIO(Me)
    ReadOnly Property CIO() As clsCIO
        Get
            Return pCIO
        End Get
    End Property

    Public Class clsCIOItem
        Public NBYR As Long
        Public IYR As Long
        Public IDAF As Long
        Public IDAL As Long
        Public IGEN As Long
        Public PCPSIM As Long
        Public IDT As Long
        Public IDIST As Long
        Public REXP As Double
        Public NRGAGE As Long
        Public NRTOT As Long
        Public NRGFIL As Long
        Public TMPSIM As Long
        Public NTGAGE As Long
        Public NTTOT As Long
        Public NTGFIL As Long
        Public SLRSIM As Long
        Public NSTOT As Long
        Public RHSIM As Long
        Public NHTOT As Long
        Public WNDSIM As Long
        Public NWTOT As Long
        Public FCSTYR As Long
        Public FCSTDAY As Long
        Public FCSTCYCLES As Long
        Public DATES As String
        Public DATEF As String
        Public FDATES As String
        Public ISPROJ As Long
        Public ICLB As Long
        Public IPRINT As Long
        Public NYSKIP As Long
        Public ILOG As Long
        Public IPRP As Long
        Public IPRS As Long

        Public Sub New()
        End Sub

        Sub New(ByVal aRow As DataRow)
            PopulateObject(aRow, Me)
        End Sub

        Public Function AddSQL() As String
            Return "INSERT INTO cio ( NBYR, IYR, IDAF, IDAL, IGEN, PCPSIM, IDT, IDIST, REXP, NRGAGE, NRTOT, NRGFIL, TMPSIM, NTGAGE, NTTOT, NTGFIL, SLRSIM, NSTOT, RHSIM, NHTOT, WNDSIM, NWTOT, FCSTYR, FCSTDAY, FCSTCYCLES, DATES, DATEF, FDATES, ISPROJ, ICLB, IPRINT, NYSKIP, ILOG, IPRP, IPRS  ) " _
                 & "Values ('" & NBYR & "', '" & IYR & "', '" & IDAF & "', '" & IDAL & "', '" & IGEN & "', '" & PCPSIM & "', '" & IDT & "', '" & IDIST & "', '" & REXP & "', '" & NRGAGE & "', '" & NRTOT & "', '" & NRGFIL & "', '" & TMPSIM & "', '" & NTGAGE & "', '" & NTTOT & "', '" & NTGFIL & "', '" & SLRSIM & "', '" & NSTOT & "', '" & RHSIM & "', '" & NHTOT & "', '" & WNDSIM & "', '" & NWTOT & "', '" & FCSTYR & "', '" & FCSTDAY & "', '" & FCSTCYCLES & "', '" & DATES & "', '" & DATEF & "', '" & FDATES & "', '" & ISPROJ & "', '" & ICLB & "', '" & IPRINT & "', '" & NYSKIP & "', '" & ILOG & "', '" & IPRP & "', '" & IPRS & "')"
        End Function
    End Class

    Public Class clsCIO
        Private pSwatInput As SwatInput
        Private pTableName As String = "cio"
        Public PrintHru As Boolean = True ' not saved in DB

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub

        Public Function TableCreate() As Boolean
            'based on mwSWATPlugIn.DBLayer.createCioTable
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
                    .Append("NBYR", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IYR", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IDAF", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IDAL", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IGEN", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("PCPSIM", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IDT", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IDIST", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("REXP", ADOX.DataTypeEnum.adDouble)
                    .Append("NRGAGE", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("NRTOT", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("NRGFIL", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("TMPSIM", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("NTGAGE", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("NTTOT", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("NTGFIL", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("SLRSIM", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("NSTOT", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("RHSIM", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("NHTOT", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("WNDSIM", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("NWTOT", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("FCSTYR", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("FCSTDAY", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("FCSTCYCLES", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("DATES", ADOX.DataTypeEnum.adVarWChar, 12)
                    .Append("DATEF", ADOX.DataTypeEnum.adVarWChar, 12)
                    .Append("FDATES", ADOX.DataTypeEnum.adVarWChar, 12)
                    .Append("ISPROJ", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("ICLB", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IPRINT", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("NYSKIP", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("ILOG", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IPRP", ADOX.DataTypeEnum.adInteger, 4)
                    .Append("IPRS", ADOX.DataTypeEnum.adInteger, 4)
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
            Return pSwatInput.QueryInputDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Function Item() As clsCIOItem
            Return Item(Nothing)
        End Function
        Public Function Item(ByVal aTable As DataTable) As clsCIOItem
            If aTable Is Nothing Then aTable = Table()
            Return New clsCIOItem(aTable.Rows(0))
        End Function

        Public Sub Add(ByVal aItem As clsCIOItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatInput)
        End Sub

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing " & pTableName & " text ...")
            Dim lSB As New System.Text.StringBuilder
            Dim lRow As DataRow = aTable.Rows(0)
            If lRow Is Nothing Then
                Throw New ApplicationException("Error in writing CIO file - Row Undefined")
            Else
                ' --------------------------------------------------------------------------------
                ' Intro
                ' --------------------------------------------------------------------------------
                lSB.AppendLine("Master Watershed File: file.cio")
                lSB.AppendLine("Project Description:")
                lSB.AppendLine("General Input/Output section (file.cio):")
                lSB.AppendLine(HeaderString)
                lSB.AppendLine("")
                lSB.AppendLine("General Information/Watershed Configuration:")
                lSB.AppendLine("fig.fig")
                ' --------------------------------------------------------------------------------
                ' Params
                ' ------2-NBYR
                lSB.AppendLine(MakeString(lRow.Item(1), 0, 4, 4) & "| NBYR : Number of years simulated")
                ' ------3-IGROPT
                lSB.AppendLine(MakeString(lRow.Item(2), 0, 4, 4) & "| IYR : Beginning year of simulation")
                ' ------ 4-AI0
                lSB.AppendLine(MakeString(lRow.Item(3), 0, 4, 4) & "| IDAF : Beginning julian day of simulation")
                ' ------ 5-AI1
                lSB.AppendLine(MakeString(lRow.Item(4), 0, 4, 4) & "| IDAL : Ending julian day of simulation")

                lSB.AppendLine("Climate:")
                ' ------ 6-AI2
                lSB.AppendLine(MakeString(lRow.Item(5), 0, 4, 4) & "| IGEN : Random number seed cycle code")
                ' ------ 7-AI3
                lSB.AppendLine(MakeString(lRow.Item(6), 0, 4, 4) & "| PCPSIM : precipitation simulation code: 1=measured, 2=simulated")
                ' ------ 8-AI4
                lSB.AppendLine(MakeString(lRow.Item(7), 0, 4, 4) & "| IDT : Rainfall data time step")
                ' ------ 9-AI5
                lSB.AppendLine(MakeString(lRow.Item(8), 0, 4, 4) & "| IDIST : rainfall distribution code: 0 skewed, 1 exponential")
                ' ------ 10-AI6
                lSB.AppendLine(MakeString(lRow.Item(9), 3, 4, 8) & "| REXP : Exponent for IDIST=1")
                ' ------ 11-MUMAX
                lSB.AppendLine(MakeString(lRow.Item(10), 0, 4, 4) & "| NRGAGE: number of pcp files used in simulation")
                ' ------ 12-RHOQ
                lSB.AppendLine(MakeString(lRow.Item(11), 0, 4, 4) & "| NRTOT: number of precip gage records used in simulation")
                ' ------ 13-TFACT
                lSB.AppendLine(MakeString(lRow.Item(12), 0, 4, 4) & "| NRGFIL: number of gage records in each pcp file")
                ' ------14-K_1
                lSB.AppendLine(MakeString(lRow.Item(13), 0, 4, 4) & "| TMPSIM: temperature simulation code: 1=measured, 2=simulated")
                ' ------15-K_N
                lSB.AppendLine(MakeString(lRow.Item(14), 0, 4, 4) & "| NTGAGE: number of tmp files used in simulation")
                ' ------16-K_P
                lSB.AppendLine(MakeString(lRow.Item(15), 0, 4, 4) & "| NTTOT: number of temp gage records used in simulation")
                ' ------17-LAMBDA0
                lSB.AppendLine(MakeString(lRow.Item(16), 0, 4, 4) & "| NTGFIL: number of gage records in each tmp file")
                ' ------ 18-LAMBDA1
                lSB.AppendLine(MakeString(lRow.Item(17), 0, 4, 4) & "| SLRSIM : Solar radiation simulation Code: 1=measured, 2=simulated")
                ' ------ 19-LAMBDA2
                lSB.AppendLine(MakeString(lRow.Item(18), 0, 4, 4) & "| NSTOT: number of solar radiation records in slr file")
                ' ------ 20-P_N
                lSB.AppendLine(MakeString(lRow.Item(19), 0, 4, 4) & "| RHSIM : relative humidity simulation code: 1=measured, 2=simulated")
                ' ------ 11-MUMAX
                lSB.AppendLine(MakeString(lRow.Item(20), 0, 4, 4) & "| NHTOT: number of relative humidity records in hmd file")
                ' ------ 12-RHOQ
                lSB.AppendLine(MakeString(lRow.Item(21), 0, 4, 4) & "| WINDSIM : Windspeed simulation code: 1=measured, 2=simulated")
                ' ------ 13-TFACT
                lSB.AppendLine(MakeString(lRow.Item(22), 0, 4, 4) & "| NWTOT: number of wind speed records in wnd file")

                lSB.AppendLine(MakeString(lRow.Item(23), 0, 4, 4) & "| FCSTYR: beginning year of forecast period")
                lSB.AppendLine(MakeString(lRow.Item(24), 0, 4, 4) & "| FCSTDAY: beginning julian date of forecast period")
                lSB.AppendLine(MakeString(lRow.Item(25), 0, 4, 4) & "| FCSTCYCLES: number of time to simulate forecast period")

                lSB.AppendLine("Precipitation Files:")
                If lRow.Item(6) = 1 Then
                    If lRow.Item(10) = 1 Then
                        lSB.AppendLine("pcp1.pcp")
                        lSB.AppendLine("")
                        lSB.AppendLine("")
                    Else
                        Dim lStr As String = ""
                        For lIndex As Integer = 1 To lRow.Item(10)
                            lStr &= ("pcp" & lIndex & ".pcp").PadLeft(13)
                            If Math.IEEERemainder(lIndex, 6) = 0 OrElse _
                               lIndex = lRow.Item(10) Then
                                lSB.AppendLine(lStr)
                                lStr = ""
                            End If
                        Next

                        If lRow.Item(10) <= 6 Then
                            lSB.AppendLine("")
                            lSB.AppendLine("")
                        ElseIf lRow.Item(10) > 6 And lRow.Item(10) <= 12 Then
                            lSB.AppendLine("")
                        End If
                    End If
                Else
                    lSB.AppendLine("")
                    lSB.AppendLine("")
                    lSB.AppendLine("")
                End If

                lSB.AppendLine("Temperature Files:")
                If lRow.Item(13) = 1 Then
                    If lRow.Item(14) = 1 Then
                        lSB.AppendLine("tmp1.tmp")
                        lSB.AppendLine("")
                        lSB.AppendLine("")
                    Else
                        Dim lStr As String = ""
                        For lIndex As Integer = 1 To lRow.Item(14)
                            lStr &= ("tmp" & lIndex & ".tmp").PadRight(13)
                            If Math.IEEERemainder(lIndex, 6) = 0 OrElse _
                               lIndex = lRow.Item(14) Then
                                lSB.AppendLine(lStr)
                                lStr = ""
                            End If
                        Next

                        If lRow.Item(14) <= 6 Then
                            lSB.AppendLine("")
                            lSB.AppendLine("")
                        ElseIf lRow.Item(14) > 6 And lRow.Item(14) <= 12 Then
                            lSB.AppendLine("")
                        End If
                    End If
                Else
                    lSB.AppendLine("")
                    lSB.AppendLine("")
                    lSB.AppendLine("")
                End If

                If lRow.Item(17) = 1 Then
                    lSB.AppendLine("slr.slr             | SLRFILE: name of solar radiation file")
                Else
                    lSB.AppendLine("                    | SLRFILE: name of solar radiation file")
                End If

                If lRow.Item(19) = 1 Then
                    lSB.AppendLine("hmd.hmd             | RHFILE: name of relative humidity file")
                Else
                    lSB.AppendLine("                    | RHFILE: name of relative humidity file")
                End If

                If lRow.Item(21) = 1 Then
                    lSB.AppendLine("wnd.wnd             | WNDFILE: name of wind speed file")
                Else
                    lSB.AppendLine("                    | WNDFILE: name of wind speed file")
                End If

                If lRow.Item(23) > 0 AndAlso lRow.Item(24) > 0 AndAlso lRow.Item(25) > 0 Then
                    lSB.AppendLine("cst.cst             | FCSTFILE: name of forecast data file")
                Else
                    lSB.AppendLine("                    | FCSTFILE: name of forecast data file")
                End If

                lSB.AppendLine("Watershed Modeling Options:")
                lSB.AppendLine("basins.bsn          | BSNFILE: name of basin input file")
                lSB.AppendLine("Database Files:")
                lSB.AppendLine("crop.dat            | PLANTDB: name of plant growth database file")
                lSB.AppendLine("till.dat            | TILLDB: name of tillage database file")
                lSB.AppendLine("pest.dat            | PESTDB: name of pesticide database file")
                lSB.AppendLine("fert.dat            | FERTDB: name of fertilizer database file")
                lSB.AppendLine("urban.dat           | URBANDB: name of urban database file")

                lSB.AppendLine("Special Projects:")
                ' ------17-LAMBDA0
                lSB.AppendLine(MakeString(lRow.Item(29), 0, 4, 4) & "| ISPROJ: special project: 1=repeat simulation")
                ' ------ 18-LAMBDA1
                lSB.AppendLine(MakeString(lRow.Item(30), 0, 4, 4) & "| ICLB: auto-calibration option: 0=no, 1=yes")
                ' ------ 19-LAMBDA2
                lSB.AppendLine("                    | CALFILE: auto-calibration parameter file")

                lSB.AppendLine("Output Information:")
                ' ------ 12-RHOQ
                lSB.AppendLine(MakeString(lRow.Item(31), 0, 4, 4) & "| IPRINT: print code (month, day, year)")
                ' ------ 13-TFACT
                lSB.AppendLine(MakeString(lRow.Item(32), 0, 4, 4) & "| NYSKIP: number of years to skip output printing/summarization")
                ' ------14-K_1
                lSB.AppendLine(MakeString(lRow.Item(33), 0, 4, 4) & "| ILOG: streamflow print code: 1=print log of streamflow")
                ' ------15-K_N
                lSB.AppendLine(MakeString(lRow.Item(34), 0, 4, 4) & "| IPRP: print code for output.pst file: 1= print pesticide output")
                ' ------16-K_P
                lSB.AppendLine(MakeString(lRow.Item(35), 0, 4, 4) & "| IPRS: print code for final soil chemical data (.chm format)")

                lSB.AppendLine("Reach output variables:")
                lSB.AppendLine("   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                lSB.AppendLine("Subbasin output variables:")
                lSB.AppendLine("   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                lSB.AppendLine("HRU output variables:")

                'TODO: figure out why PrintHru was not propagating to here, hard coded to 0 for now
                If PrintHru Then
                    lSB.AppendLine("   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                Else
                    lSB.AppendLine("   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                End If
                lSB.AppendLine("HRU data to be printed:")
                'TODO: figure out why PrintHru was not propagating to here, hard coded to 0 for now
                If PrintHru Then
                    lSB.AppendLine("   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                Else
                    lSB.AppendLine("   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                End If
                IO.File.WriteAllText(pSwatInput.TxtInOutFolder & "\file.cio", lSB.ToString)
            End If
        End Sub
    End Class
End Class
