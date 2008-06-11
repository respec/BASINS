Partial Class SwatInput
    Private pRte As clsRte = New clsRte(Me)
    ReadOnly Property Rte() As clsRte
        Get
            Return pRte
        End Get
    End Property

    ''' <summary>
    ''' (Rte) Input Section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class clsRte
        Private pSwatInput As SwatInput
        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub
        Public Function Table() As DataTable
            pSwatInput.Status("Reading Rte tables from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM rte;")
        End Function
        Public Sub Save()
            pSwatInput.Status("Writing rte table ...")

            Dim lTable As DataTable = Table()
            For Each lRow As DataRow In lTable.Rows

                Dim sRteName As String
                Dim strSub As String

                strSub = lRow.Item("SUBBASIN")
                sRteName = StringFnameSubBasins(strSub)
                If sRteName = "" Then
                    Exit Sub
                End If
                sRteName &= ".rte"

                Dim lSB As New System.Text.StringBuilder
                '1st line
                lSB.AppendLine(" .rte file Subbasin: " + strSub + " " + Date.Today.ToString + " AVSWAT2003 -SWAT INTERFACE MAVZ")
                '---2. CHW2
                lSB.AppendLine(Format(lRow.Item(2), "0.000").PadLeft(14) + Strings.StrDup(4, " ") + "| CHW2 : Main channel width [m]")
                '---3. CHD
                lSB.AppendLine(Format(lRow.Item(3), "0.000").PadLeft(14) + Strings.StrDup(4, " ") + "| CHD : Main channel depth [m]")
                '---4. CH_S2
                lSB.AppendLine(Format(lRow.Item(4), "0.000").PadLeft(14) + Strings.StrDup(4, " ") + "| CH_S2 : Main channel slope [m/m]")
                '---5. CH_L2
                lSB.AppendLine(Format(lRow.Item(5), "0.000").PadLeft(14) + Strings.StrDup(4, " ") + "| CH_L2 : Main channel length [km]")
                '---6. CH_N2
                lSB.AppendLine(Format(lRow.Item(6), "0.000").PadLeft(14) + Strings.StrDup(4, " ") + "| CH_N2 : Manning's nvalue for main channel")
                '---7. CH_K2
                lSB.AppendLine(Format(lRow.Item(7), "0.000").PadLeft(14) + Strings.StrDup(4, " ") + "| CH_K2 : Effective hydraulic conductivity [mm/hr]")
                '---8. CH_EROD
                lSB.AppendLine(Format(lRow.Item(8), "0.000").PadLeft(14) + Strings.StrDup(4, " ") + "| CH_EROD: Channel erodibility factor")
                '---9. CH_COV
                lSB.AppendLine(Format(lRow.Item(9), "0.000").PadLeft(14) + Strings.StrDup(4, " ") + "| CH_COV : Channel cover factor")
                '---10. CH_WDR
                lSB.AppendLine(Format(lRow.Item(10), "0.000").PadLeft(14) + Strings.StrDup(4, " ") + "| CH_WDR : Channel width:depth ratio [m/m]")
                '---11. CH_WDR
                lSB.AppendLine(Format(lRow.Item(11), "0.000").PadLeft(14) + Strings.StrDup(4, " ") + "| ALPHA_BNK : Baseflow alpha factor for bank storage [days]")

                IO.File.WriteAllText(pSwatInput.OutputFolder & "\" & sRteName, lSB.ToString)
            Next

        End Sub
    End Class
End Class
