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
        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
        End Sub
        Public Function Table() As DataTable
            pSwatInput.Status("Reading WGN from database ...")
            Return pSwatInput.QueryInputDB("SELECT * FROM wgn;")
        End Function
        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            If aTable Is Nothing Then aTable = Table()
            pSwatInput.Status("Writing WGN text ...")

            Dim i As Integer
            Dim strSub As String

            For Each lRow As DataRow In aTable.Rows
                strSub = lRow.Item("SUBBASIN")

                Dim lSB As New System.Text.StringBuilder
                '1st line
                lSB.AppendLine(" .Wgn file Subbasin: " + strSub _
                             + " STATION NAME:" + lRow.Item(("STATION")) + " " _
                             + DateNowString + " AVSWAT2003 -SWAT INTERFACE MAVZ")
                lSB.AppendLine("  LATITUDE =" + Format(lRow.Item(("WLATITUDE")), "0.00").PadLeft(7) _
                             + " LONGITUDE =" + Format(lRow.Item(("WLONGITUDE")), "0.00").PadLeft(7))
                ' ----------------------- Line 3
                lSB.AppendLine("  ELEV [m] =" + Format(lRow.Item(("WELEV")), "0.00").PadLeft(7))
                ' ----------------------- Line 4
                lSB.AppendLine("  RAIN_YRS =" + Format(lRow.Item(("RAIN_YRS")), "0.00").PadLeft(7))
                ' ---------------------- Line 5
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("TMPMX" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next
                ' ---------------------- Line 6
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("TMPMN" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next
                ' ---------------------- Line 7
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("TMPSTDMX" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next
                ' ---------------------- Line 8
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("TMPSTDMN" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next
                ' ---------------------- Line 9
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("PCPMM" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next
                ' ---------------------- Line 10
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("PCPSTD" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next
                ' ---------------------- Line 11
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("PCPSKW" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next i
                ' ---------------------- Line 12
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("PR_W1_" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next i
                ' ---------------------- Line 13
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("PR_W2_" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next i
                ' ---------------------- Line 14
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("PCPD" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next i
                ' ---------------------- Line 15
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("RAINHHMX" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next i
                ' ---------------------- Line 16
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("SOLARAV" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next i
                ' ---------------------- Line 17
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("DEWPT" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next i
                ' ---------------------- Line 18
                lSB.AppendLine()
                For i = 1 To 12
                    lSB.Append(Format(lRow.Item(("WNDAV" & Trim(Str(i)))), "0.00").PadLeft(6))
                Next i
                lSB.AppendLine()

                IO.File.WriteAllText(pSwatInput.OutputFolder & "\" & StringFname(strSub, "wgn"), lSB.ToString)
            Next

        End Sub
    End Class
End Class
