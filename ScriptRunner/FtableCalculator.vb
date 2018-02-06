Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports atcManDelin
Imports System.IO
Imports System.Text
Imports atcSegmentation
Imports atcUCI


Module FtableCalculator
    Private Const pInputPath As String = "C:\Basins\data\20610\hydrography"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        ChDriveDir(pInputPath)
        Logger.Dbg(" CurDir:" & CurDir())

        'Dim lStreamsLayerName As String = "Reaches_Table"
        'open the dbf of shapefile attributes
        Dim lDBF As New atcTableDBF
        lDBF.OpenFile("H:\FTABLEComp\Reaches_Table.dbf")

        Dim i As Integer
        'read each value like this
        Dim MAXELE_ft, MINELE_ft, LENGTH_mi, BANKFULLDEP_ft, BANKFULLWID_ft, BOTTOMWID_ft, CHANNEL_n, _
            FLOODPLAIN_n, FLOODPLAIN_SL, Channel_slope, Theta1, Theta2, WP1, WP2, AREAIN, WETPIN As Double
        Dim Inc, Twid, XSAreaIN, WetP, HydRad, XSAreaOV, HydRadOV, Depth, SurfaceArea, Volume, Discharge As Double
        Dim Reach As Integer

        Dim lSB As New StringBuilder
        lSB.AppendLine("FTABLES")

        For i = 1 To lDBF.NumRecords
            lDBF.CurrentRecord = i
            Reach = lDBF.Value(5)
            MAXELE_ft = lDBF.Value(15)
            MINELE_ft = lDBF.Value(14)
            LENGTH_mi = lDBF.Value(2)
            BANKFULLDEP_ft = lDBF.Value(18)
            BANKFULLWID_ft = lDBF.Value(17)
            BOTTOMWID_ft = lDBF.Value(24)
            CHANNEL_n = lDBF.Value(20)
            FLOODPLAIN_n = lDBF.Value(23)
            FLOODPLAIN_SL = lDBF.Value(19)

            Depth = 0.0
            Discharge = 0.0
            SurfaceArea = 0.0
            Volume = 0.0
            Inc = 0
            Channel_slope = (MAXELE_ft - MINELE_ft) / LENGTH_mi / 5280
            Theta1 = Math.Atan((BANKFULLWID_ft - BOTTOMWID_ft) / 2 / BANKFULLDEP_ft)
            Theta2 = Math.Atan(FLOODPLAIN_SL)
            WP1 = Math.Cos(Theta1)
            WP2 = Math.Sin(Theta2)
            AREAIN = (BANKFULLWID_ft + BOTTOMWID_ft) * BANKFULLDEP_ft / 2
            WETPIN = BOTTOMWID_ft + 2 * BANKFULLDEP_ft / WP1

            lSB.AppendLine(" ")
            lSB.AppendLine("  FTABLE    " & CStr(Reach).PadLeft(3))
            Dim lRows As Integer = 20
            Dim lCols As Integer = 4
            lSB.AppendLine(" rows cols" & Space((lCols - 1) * 10) & " ***")
            lSB.AppendLine(CStr(lRows).PadLeft(5) & CStr(lCols).PadLeft(5))
            lSB.AppendLine("     depth      area    volume  outflow ***")
            Dim lFmt As String = "#0.0#"

            Dim k As Integer
            For k = 1 To 20
                Select Case k
                    Case 2 To 7
                        Inc = BANKFULLDEP_ft / 12
                    Case 8 To 10
                        Inc = 2 * BANKFULLDEP_ft / 12
                    Case 11 To 20
                        Inc = 4 * BANKFULLDEP_ft / 12
                End Select

                Depth = Depth + Inc
                If k > 1 And k <= 10 Then
                    Twid = BOTTOMWID_ft + ((BANKFULLWID_ft - BOTTOMWID_ft) / BANKFULLDEP_ft) * Depth
                    XSAreaIN = ((Twid + BOTTOMWID_ft) / 2) * Depth
                    WetP = BOTTOMWID_ft + 2 * (Depth / WP1)
                    HydRad = XSAreaIN / WetP
                    Volume = XSAreaIN * LENGTH_mi * 5280 / 43560
                    Discharge = 1.49 * XSAreaIN * (HydRad ^ 0.667) * (Channel_slope ^ 0.5) / CHANNEL_n
                    SurfaceArea = Twid * LENGTH_mi * 5280 / 43560
                ElseIf k > 10 Then
                    Twid = BANKFULLWID_ft + 2 * (Depth - BANKFULLDEP_ft) / FLOODPLAIN_SL
                    XSAreaIN = AREAIN + BANKFULLWID_ft * (Depth - BANKFULLDEP_ft)
                    HydRad = XSAreaIN / WETPIN
                    XSAreaOV = (Depth - BANKFULLDEP_ft) * (Depth - BANKFULLDEP_ft) / FLOODPLAIN_SL
                    HydRadOV = XSAreaOV / 2 / (Depth - BANKFULLDEP_ft) * WP2
                    Volume = XSAreaIN * LENGTH_mi * 5280 / 43560 + XSAreaOV * LENGTH_mi * 5280 / 43560
                    Discharge = 1.49 * XSAreaIN * (HydRad ^ 0.667) * Channel_slope ^ 0.5 / CHANNEL_n + 1.49 * XSAreaOV * (HydRadOV ^ 0.667) * Channel_slope ^ 0.5 / FLOODPLAIN_n
                    SurfaceArea = Twid * LENGTH_mi * 5280 / 43560
                End If


                'now write this row to the stringbuilder
                Dim lStr As String = Format(Depth, lFmt).PadLeft(10) & Format(SurfaceArea, lFmt).PadLeft(10) & Format(Volume, lFmt).PadLeft(10)
                Dim lTempStr As String = Format(Discharge, lFmt).PadLeft(10)
                If lTempStr.Length > 10 Then 'too many digits in the number
                    lTempStr = atcUCI.HspfTable.NumFmtRE(CSng(Discharge), 10).PadLeft(10)
                End If
                lStr &= lTempStr
                lSB.AppendLine(lStr)
            Next

            lSB.AppendLine("  END FTABLE" & CStr(Reach).PadLeft(3))
        Next

        lSB.AppendLine("END FTABLES")

        'now write string to a file
        IO.File.WriteAllText("H:\FTABLEComp\xxxFTABLES.txt", lSB.ToString)

    End Sub

End Module
