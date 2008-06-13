Partial Class SwatInput
    Private pMgt As clsMgt = New clsMgt(Me)
    ReadOnly Property Mgt() As clsMgt
        Get
            Return pMgt
        End Get
    End Property

    ''' <summary>
    ''' Management (MGT) input section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsMgt
        Private pSwatInput As SwatInput
        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub
        Public Function Table1() As DataTable
            pSwatInput.Status("Reading MGT1 from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM mgt1 ORDER BY SUBBASIN, HRU;")
        End Function
        Public Function Table2() As DataTable
            pSwatInput.Status("Reading MGT2 from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM mgt2 ORDER BY SUBBASIN, HRU, [YEAR], [MONTH], [DAY], HUSC;")
        End Function
        ''' <summary>
        ''' Save MGT information to set of .mgt text input files
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Save(Optional ByVal aTable1 As DataTable = Nothing, Optional ByVal aTable2 As DataTable = Nothing)
            If aTable1 Is Nothing Then aTable1 = Table1()
            If aTable2 Is Nothing Then aTable2 = Table2()

            pSwatInput.Status("Writing MGT text ...")
            For Each lMgt1Row As DataRow In aTable1.Rows
                Dim lSubNum As String = lMgt1Row.Item(1).ToString.Trim
                Dim lHruNum As String = lMgt1Row.Item(2).ToString.Trim
                Dim lMgtName As String = StringFnameHRUs(lSubNum, lHruNum) + ".mgt"

                Dim lSB As New System.Text.StringBuilder
                '1st line
                lSB.AppendLine(" .mgt file Subbasin:" + lSubNum + " HRU:" + lHruNum + " Luse:" + lMgt1Row.Item(3) + " Soil: " + lMgt1Row.Item(4) + " Slope: " + lMgt1Row.Item(5) + _
                               " " + DateNowString + " ARCGIS-SWAT2003 interface MAVZ")
                '2. NMGT
                lSB.AppendLine(Format(0, "0").PadLeft(16) + Strings.StrDup(4, " ") + "| NMGT:Management code")

                lSB.AppendLine("Initial Plant Growth Parameters")
                'IGRO
                lSB.AppendLine(Format(lMgt1Row.Item(("IGRO")), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| IGRO: Land cover status: 0-none growing; 1-growing")
                'PLANT_ID
                lSB.AppendLine(Format(lMgt1Row.Item(("PLANT_ID")), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| PLANT_ID: Land cover ID number (IGRO = 1)")
                'LAI_INIT
                lSB.AppendLine(Format(lMgt1Row.Item(("LAI_INIT")), "0.00").PadLeft(16) + Strings.StrDup(4, " ") + "| LAI_INIT: Initial leaf are index (IGRO = 1)")
                'BIO_INIT
                lSB.AppendLine(Format(lMgt1Row.Item(("BIO_INIT")), "0.00").PadLeft(16) + Strings.StrDup(4, " ") + "| BIO_INIT: Initial biomass (kg/ha) (IGRO = 1)")
                'PHU_PLT
                lSB.AppendLine(Format(lMgt1Row.Item(("PHU_PLT")), "0.00").PadLeft(16) + Strings.StrDup(4, " ") + "| PHU_PLT: Number of heat units to bring plant to maturity (IGRO = 1)")

                lSB.AppendLine("General Management Parameters")
                'BIOMIX
                lSB.AppendLine(Format(lMgt1Row.Item(("BIOMIX")), "0.00").PadLeft(16) + Strings.StrDup(4, " ") + "| BIOMIX: Biological mixing efficiency")
                'CN2
                lSB.AppendLine(Format(lMgt1Row.Item(("CN2")), "0.00").PadLeft(16) + Strings.StrDup(4, " ") + "| CN2: Initial SCS CN II value")
                'USLE_P
                lSB.AppendLine(Format(lMgt1Row.Item(("USLE_P")), "0.00").PadLeft(16) + Strings.StrDup(4, " ") + "| USLE_P: USLE support practice factor")
                'BIO_MIN
                lSB.AppendLine(Format(lMgt1Row.Item(("BIO_MIN")), "0.00").PadLeft(16) + Strings.StrDup(4, " ") + "| BIO_MIN: Minimum biomass for grazing (kg/ha)")
                'FILTERW
                lSB.AppendLine(Format(lMgt1Row.Item(("FILTERW")), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| FILTERW: width of edge of field filter strip (m)")

                lSB.AppendLine("Urban Management Parameters")
                'IURBAN
                lSB.AppendLine(Format(lMgt1Row.Item(("IURBAN")), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| IURBAN: urban simulation code, 0-none, 1-USGS, 2-buildup/washoff")
                'URBLU
                lSB.AppendLine(Format(lMgt1Row.Item(("URBLU")), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| URBLU: urban land type")

                lSB.AppendLine("Irrigation Management Parameters")
                'IRRSC
                lSB.AppendLine(Format(lMgt1Row.Item(("IRRSC")), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| IRRSC: irrigation code")
                'IRRNO
                lSB.AppendLine(Format(lMgt1Row.Item(("IRRNO")), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| IRRNO: irrigation source location")
                'FLOWMIN
                lSB.AppendLine(Format(lMgt1Row.Item(("FLOWMIN")), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| FLOWMIN: min in-stream flow for irr diversions (m^3/s)")
                'DIVMAX
                lSB.AppendLine(Format(lMgt1Row.Item(("DIVMAX")), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| DIVMAX: max irrigation diversion from reach (+mm/-10^4m^3)")
                'FLOWFR
                lSB.AppendLine(Format(lMgt1Row.Item(("FLOWFR")), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| FLOWFR: : fraction of flow allowed to be pulled for irr")

                lSB.AppendLine("Tile Drain Management Parameters")

                'DDRAIN
                lSB.AppendLine(Format(lMgt1Row.Item(("DDRAIN")), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| DDRAIN: depth to subsurface tile drain (mm)")
                'TDRAIN
                lSB.AppendLine(Format(lMgt1Row.Item(("TDRAIN")), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| TDRAIN: time to drain soil to field capacity (hr)")
                'GDRAIN
                lSB.AppendLine(Format(lMgt1Row.Item(("GDRAIN")), "0.000").PadLeft(16) + Strings.StrDup(4, " ") + "| GDRAIN: drain tile lag time (hr)")

                lSB.AppendLine("Management Operations:")
                'NROT
                lSB.AppendLine(Format(lMgt1Row.Item(("NROT")), "0").PadLeft(16) + Strings.StrDup(4, " ") + "| NROT: number of years of rotation")
                lSB.AppendLine("Operation Schedule:")

                If lMgt1Row.Item(("NROT")) > 0 Then 'Print operations schedule if 1 or more operation years
                    '3d line 'from mgt2
                    Dim lBlnHu As Boolean
                    If lMgt1Row.Item(("HUSC")) = 0 Then
                        lBlnHu = True
                    Else
                        lBlnHu = False
                    End If

                    Dim lRowsMgt2() As DataRow = aTable2.Select("SUBBASIN = " & lMgt1Row.Item(1) & " AND HRU = " & lMgt1Row.Item(2), "[YEAR] ASC, [MONTH] ASC, [DAY] ASC, HUSC ASC")
                    Dim lYear As Integer = lRowsMgt2(0).Item(("YEAR"))

                    Dim lLine As String = ""

                    For Each lRowMgt2 As DataRow In lRowsMgt2
                        If lBlnHu = True Then
                            lLine = Strings.StrDup(7, " ") + Format(lRowMgt2.Item(("HUSC")), "##0.000").PadLeft(8)
                        Else
                            lLine = Format(lRowMgt2.Item(("MONTH")), "##").PadLeft(3) + _
                            Format(lRowMgt2.Item(("DAY")), "##").PadLeft(3) + Strings.StrDup(9, " ")
                        End If
                        lLine = lLine + Format(lRowMgt2.Item(("MGT_OP")), "##").PadLeft(3)

                        Dim lMgtOp As Integer = lRowMgt2.Item(("MGT_OP"))
                        Select Case lMgtOp
                            Case Is = -1 'skipped year
                                lLine = Strings.StrDup(19, " ") + "0" + Strings.StrDup(76, " ")
                            Case Is = 1
                                lLine = lLine + Format(lRowMgt2.Item(("PLANT_ID")), "####").PadLeft(5) + _
                                Format(lRowMgt2.Item(("CURYR_MAT")), "##").PadLeft(7) + Format(lRowMgt2.Item(("HEATUNITS")), "#####0.00000").PadLeft(13) + _
                                Format(lRowMgt2.Item(("LAI_INIT")), "##0.00").PadLeft(7) + Format(lRowMgt2.Item(("BIO_INIT")), "####0.00000").PadLeft(12) + _
                                Format(lRowMgt2.Item(("HI_TARG")), "0.00").PadLeft(5) + Format(lRowMgt2.Item(("BIO_TARG")), "##0.00").PadLeft(7) + _
                                Format(lRowMgt2.Item(("CNOP")), "#0.00").PadLeft(6)
                            Case Is = 2 'irrigation
                                lLine = lLine + Strings.StrDup(13, " ") + Format(lRowMgt2.Item(("IRR_AMT")), "#####0.00000").PadLeft(12)
                            Case Is = 3 'fertilizer: there are several variables that are not included in SWAT 2002 but are existing in the table i.e. FRMINN, etc. Those are not shown in the interface and it seems to be expected to be on the next version. In this version they are not printed in the files.
                                lLine = lLine + Format(lRowMgt2.Item(("FERT_ID")), "###0").PadLeft(5) + _
                                Format(lRowMgt2.Item(("FRT_KG")), "#####0.00000").PadLeft(20) + Format(lRowMgt2.Item(("FRT_SURFACE")), "##0.00").PadLeft(7)
                            Case Is = 4 'pesticide
                                lLine = lLine + Format(lRowMgt2.Item(("PEST_ID")), "###0").PadLeft(5) + _
                                Format(lRowMgt2.Item(("PST_KG")), "#####0.00000").PadLeft(20)
                            Case Is = 5 'Harvest and kill operation
                                lLine = lLine + Format(lRowMgt2.Item(("CNOP")), "#####0.00000").PadLeft(25)
                            Case Is = 6 'Tillage operation
                                lLine = lLine + Format(lRowMgt2.Item(("TILLAGE_ID")), "###0").PadLeft(5) + _
                                Format(lRowMgt2.Item(("CNOP")), "#####0.00000").PadLeft(20)
                            Case Is = 7 ' Harvest only operation
                                lLine = lLine + Format(lRowMgt2.Item(("HARVEFF")), "#####0.00000").PadLeft(25) + _
                                Format(lRowMgt2.Item(("HI_OVR")), "##0.00").PadLeft(17)
                            Case Is = 8 'KILL OPERATION

                            Case Is = 9 ' Grazing operation
                                lLine = lLine + Format(lRowMgt2.Item(("GRZ_DAYS")), "###0").PadLeft(5) + _
                                Format(lRowMgt2.Item(("MANURE_ID")), "##0").PadLeft(4) + _
                                Format(lRowMgt2.Item(("BIO_EAT")), "#####0.00000").PadLeft(16) + _
                                Format(lRowMgt2.Item(("BIO_TRMP")), "##0.00").PadLeft(7) + _
                                Format(lRowMgt2.Item(("MANURE_KG")), "####0.00000").PadLeft(12)
                            Case Is = 10 'Auto irrigation: AUTO_RNF is a variable showing in AVSWAT but not shown in SWAT2002. It is not included here.
                                lLine = lLine + Format(lRowMgt2.Item(("WSTRS_ID")), "###0").PadLeft(5) + _
                                Format(lRowMgt2.Item(("AUTO_WSTRS")), "#####0.00000").PadLeft(20)
                            Case Is = 11 'Auto fert option
                                lLine = lLine + Format(lRowMgt2.Item(("AFERT_ID")), "###0").PadLeft(5) + _
                                Format(lRowMgt2.Item(("AUTO_NSTRS")), "#####0.00000").PadLeft(20) + _
                                Format(lRowMgt2.Item(("AUTO_NAPP")), "##0.00").PadLeft(7) + _
                                Format(lRowMgt2.Item(("AUTO_NYR")), "####0.00000").PadLeft(12) + _
                                Format(lRowMgt2.Item(("AUTO_EFF")), "0.00").PadLeft(5) + _
                                Format(lRowMgt2.Item(("AFRT_SURFACE")), "####0.00").PadLeft(9)
                            Case Is = 12 'street sweeping operation
                                lLine = lLine + Format(lRowMgt2.Item(("SWEEPEFF")), "#####0.00000").PadLeft(25) + _
                                Format(lRowMgt2.Item(("FR_CURB")), "##0.00").PadLeft(7)
                            Case Is = 13 'realease/impound
                                lLine = lLine + Format(lRowMgt2.Item(("IMP_TRIG")), "###0").PadLeft(5)
                            Case Is = 14 'Continuous fertilizer
                                lLine = lLine + Format(lRowMgt2.Item(("FERT_DAYS")), "###0").PadLeft(5) + _
                                Format(lRowMgt2.Item(("CFRT_ID")), "##0").PadLeft(4) + _
                                Format(lRowMgt2.Item(("IFRT_FREQ")), "#0").PadLeft(3) + _
                                Format(lRowMgt2.Item(("CFRT_KG")), "#####0.0000").PadLeft(13)
                        End Select
                        'SKIP A LINE IN NEW YEAR
                        Dim lNYear As Integer = lRowMgt2.Item(("YEAR"))
                        If lNYear <> lYear Then
                            lYear = lNYear
                            If lMgtOp <> -1 Then
                                lSB.AppendLine(Strings.StrDup(19, " ") + "0" + Strings.StrDup(76, " "))
                            End If
                        End If
                        lSB.AppendLine(lLine)
                    Next
                End If
                lSB.AppendLine("")
                IO.File.WriteAllText(pSwatInput.OutputFolder & "\" & lMgtName, lSB.ToString)
            Next
        End Sub
    End Class
End Class
