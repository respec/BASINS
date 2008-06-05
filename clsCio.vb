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

    Public Class clsCIO
        Private pSwatInput As SwatInput
        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub
        Public Function Table() As DataTable
            pSwatInput.Status("Reading CIO table from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM cio;")
        End Function
        Public Sub Save(ByVal pSwatInput As SwatInput, ByVal aPrintHRU As Boolean)
            Dim lCIOTable As DataTable = Table()
            pSwatInput.Status("Writing CIO table ...")
            Dim lSB As New System.Text.StringBuilder
            Dim lCIORow As DataRow = lCIOTable.Rows(0)
            If lCIORow Is Nothing Then
                Throw New ApplicationException("Error in writing CIO file - Row Undefined")
            Else
                ' --------------------------------------------------------------------------------
                ' Intro
                ' --------------------------------------------------------------------------------
                lSB.AppendLine("Master Watershed File: file.cio")
                lSB.AppendLine("Project Description:")
                lSB.AppendLine("General Input/Output section (file.cio):")
                lSB.AppendLine(Date.Today.ToString & "ARCGIS-SWAT interface AV")
                lSB.AppendLine("")
                lSB.AppendLine("General Information/Watershed Configuration:")
                lSB.AppendLine("fig.fig")
                ' --------------------------------------------------------------------------------
                ' Params
                ' ------2-NBYR
                lSB.AppendLine(MakeString(lCIORow.Item(1), 0, 4, 4) + "| NBYR : Number of years simulated")
                ' ------3-IGROPT
                lSB.AppendLine(MakeString(lCIORow.Item(2), 0, 4, 4) + "| IYR : Beginning year of simulation")
                ' ------ 4-AI0
                lSB.AppendLine(MakeString(lCIORow.Item(3), 0, 4, 4) + "| IDAF : Beginning julian day of simulation")
                ' ------ 5-AI1
                lSB.AppendLine(MakeString(lCIORow.Item(4), 0, 4, 4) + "| IDAL : Ending julian day of simulation")

                lSB.AppendLine("Climate:")
                ' ------ 6-AI2
                lSB.AppendLine(MakeString(lCIORow.Item(5), 0, 4, 4) + "| IGEN : Random number seed cycle code")
                ' ------ 7-AI3
                lSB.AppendLine(MakeString(lCIORow.Item(6), 0, 4, 4) + "| PCPSIM : precipitation simulation code: 1=measured, 2=simulated")
                ' ------ 8-AI4
                lSB.AppendLine(MakeString(lCIORow.Item(7), 0, 4, 4) + "| IDT : Rainfall data time step")
                ' ------ 9-AI5
                lSB.AppendLine(MakeString(lCIORow.Item(8), 0, 4, 4) + "| IDIST : rainfall distribution code: 0 skewed, 1 exponential")
                ' ------ 10-AI6
                lSB.AppendLine(MakeString(lCIORow.Item(9), 3, 4, 8) + "| REXP : Exponent for IDIST=1")
                ' ------ 11-MUMAX
                lSB.AppendLine(MakeString(lCIORow.Item(10), 0, 4, 4) + "| NRGAGE: number of pcp files used in simulation")
                ' ------ 12-RHOQ
                lSB.AppendLine(MakeString(lCIORow.Item(11), 0, 4, 4) + "| NRTOT: number of precip gage records used in simulation")
                ' ------ 13-TFACT
                lSB.AppendLine(MakeString(lCIORow.Item(12), 0, 4, 4) + "| NRGFIL: number of gage records in each pcp file")
                ' ------14-K_1
                lSB.AppendLine(MakeString(lCIORow.Item(13), 0, 4, 4) + "| TMPSIM: temperature simulation code: 1=measured, 2=simulated")
                ' ------15-K_N
                lSB.AppendLine(MakeString(lCIORow.Item(14), 0, 4, 4) + "| NTGAGE: number of tmp files used in simulation")
                ' ------16-K_P
                lSB.AppendLine(MakeString(lCIORow.Item(15), 0, 4, 4) + "| NTTOT: number of temp gage records used in simulation")
                ' ------17-LAMBDA0
                lSB.AppendLine(MakeString(lCIORow.Item(16), 0, 4, 4) + "| NTGFIL: number of gage records in each tmp file")
                ' ------ 18-LAMBDA1
                lSB.AppendLine(MakeString(lCIORow.Item(17), 0, 4, 4) + "| SLRSIM : Solar radiation simulation Code: 1=measured, 2=simulated")
                ' ------ 19-LAMBDA2
                lSB.AppendLine(MakeString(lCIORow.Item(18), 0, 4, 4) + "| NSTOT: number of solar radiation records in slr file")
                ' ------ 20-P_N
                lSB.AppendLine(MakeString(lCIORow.Item(19), 0, 4, 4) + "| RHSIM : relative humidity simulation code: 1=measured, 2=simulated")
                ' ------ 11-MUMAX
                lSB.AppendLine(MakeString(lCIORow.Item(20), 0, 4, 4) + "| NHTOT: number of relative humidity records in hmd file")
                ' ------ 12-RHOQ
                lSB.AppendLine(MakeString(lCIORow.Item(21), 0, 4, 4) + "| WINDSIM : Windspeed simulation code: 1=measured, 2=simulated")
                ' ------ 13-TFACT
                lSB.AppendLine(MakeString(lCIORow.Item(22), 0, 4, 4) + "| NWTOT: number of wind speed records in wnd file")
                ' ------14-K_1
                lSB.AppendLine(MakeString(lCIORow.Item(23), 0, 4, 4) + "| FCSTYR: beginning year of forecast period")
                ' ------15-K_N
                lSB.AppendLine(MakeString(lCIORow.Item(24), 0, 4, 4) + "| FCSTDAY: beginning julian date of forecast period")
                ' ------16-K_P
                lSB.AppendLine(MakeString(lCIORow.Item(25), 0, 4, 4) + "| FCSTCYCLES: number of time to simulate forecast period")

                lSB.AppendLine("Precipitation Files:")
                If lCIORow.Item(6) = 1 Then
                    If lCIORow.Item(10) = 1 Then
                        lSB.AppendLine("pcp1.pcp")
                        lSB.AppendLine("")
                        lSB.AppendLine("")
                    Else
                        Dim lStr As String = ""
                        For lIndex As Integer = 1 To lCIORow.Item(10)
                            lStr &= ("pcp" & lIndex & ".pcp").PadLeft(13)
                            If Math.IEEERemainder(lIndex, 6) = 0 OrElse _
                               lIndex = lCIORow.Item(10) Then
                                lSB.AppendLine(lStr)
                                lStr = ""
                            End If
                        Next

                        If lCIORow.Item(10) <= 6 Then
                            lSB.AppendLine("")
                            lSB.AppendLine("")
                        ElseIf lCIORow.Item(10) > 6 And lCIORow.Item(10) <= 12 Then
                            lSB.AppendLine("")
                        End If
                    End If
                Else
                    lSB.AppendLine("")
                    lSB.AppendLine("")
                    lSB.AppendLine("")
                End If

                lSB.AppendLine("Temperature Files:")
                If lCIORow.Item(13) = 1 Then
                    If lCIORow.Item(14) = 1 Then
                        lSB.AppendLine("tmp1.tmp")
                        lSB.AppendLine("")
                        lSB.AppendLine("")
                    Else
                        Dim lStr As String = ""
                        For lIndex As Integer = 1 To lCIORow.Item(14)
                            lStr &= ("tmp" & lIndex & ".tmp").PadRight(13)
                            If Math.IEEERemainder(lIndex, 6) = 0 OrElse _
                               lIndex = lCIORow.Item(14) Then
                                lSB.AppendLine(lStr)
                                lStr = ""
                            End If
                        Next

                        If lCIORow.Item(14) <= 6 Then
                            lSB.AppendLine("")
                            lSB.AppendLine("")
                        ElseIf lCIORow.Item(14) > 6 And lCIORow.Item(14) <= 12 Then
                            lSB.AppendLine("")
                        End If
                    End If
                Else
                    lSB.AppendLine("")
                    lSB.AppendLine("")
                    lSB.AppendLine("")
                End If

                If lCIORow.Item(17) = 1 Then
                    lSB.AppendLine("slr.slr             | SLRFILE: name of solar radiation file")
                Else
                    lSB.AppendLine("                    | SLRFILE: name of solar radiation file")
                End If

                If lCIORow.Item(19) = 1 Then
                    lSB.AppendLine("hmd.hmd             | RHFILE: name of relative humidity file")
                Else
                    lSB.AppendLine("                    | RHFILE: name of relative humidity file")
                End If

                If lCIORow.Item(21) = 1 Then
                    lSB.AppendLine("wnd.wnd             | WNDFILE: name of wind speed file")
                Else
                    lSB.AppendLine("                    | WNDFILE: name of wind speed file")
                End If

                lSB.AppendLine("cst.cst             | FCSTFILE: name of forecast data file")

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
                lSB.AppendLine(MakeString(lCIORow.Item(29), 0, 4, 4) + "| ISPROJ: special project: 1=repeat simulation")
                ' ------ 18-LAMBDA1
                lSB.AppendLine(MakeString(lCIORow.Item(30), 0, 4, 4) + "| ICLB: auto-calibration option: 0=no, 1=yes")
                ' ------ 19-LAMBDA2
                lSB.AppendLine("                    | CALFILE: auto-calibration parameter file")

                lSB.AppendLine("Output Information:")
                ' ------ 12-RHOQ
                lSB.AppendLine(MakeString(lCIORow.Item(31), 0, 4, 4) + "| IPRINT: print code (month, day, year)")
                ' ------ 13-TFACT
                lSB.AppendLine(MakeString(lCIORow.Item(32), 0, 4, 4) + "| NYSKIP: number of years to skip output printing/summarization")
                ' ------14-K_1
                lSB.AppendLine(MakeString(lCIORow.Item(33), 0, 4, 4) + "| ILOG: streamflow print code: 1=print log of streamflow")
                ' ------15-K_N
                lSB.AppendLine(MakeString(lCIORow.Item(34), 0, 4, 4) + "| IPRP: print code for output.pst file: 1= print pesticide output")
                ' ------16-K_P
                lSB.AppendLine(MakeString(lCIORow.Item(35), 0, 4, 4) + "| IPRS: print code for final soil chemical data (.chm format)")

                lSB.AppendLine("Reach output variables:")
                lSB.AppendLine("   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                lSB.AppendLine("Subbasin output variables:")
                lSB.AppendLine("   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                lSB.AppendLine("HRU output variables:")
                If aPrintHRU = False Then
                    lSB.AppendLine("   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                Else
                    lSB.AppendLine("   1   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                End If
                lSB.AppendLine("HRU data to be printed:")
                If aPrintHRU = False Then
                    lSB.AppendLine("   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                Else
                    lSB.AppendLine("   1   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0")
                End If
                IO.File.WriteAllText(pSwatInput.OutputFolder & "\file.cio", lSB.ToString)
            End If
        End Sub
    End Class
End Class
