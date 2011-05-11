Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System.IO
Imports System.Text

Module MPCAUtil
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lDataTable As New atcTableDelimited()
        Dim lMPCADatafile As String = "G:\Admin\MPCA\CrowWingLpRPrecipOKAqTrFiltered.csv"
        'lMPCADatafile = "G:\Admin\MPCA\Short.csv"
        Dim lFixedWidthFile As String = ""
        With lDataTable
            .Delimiter = ","
            If .OpenFile(lMPCADatafile) Then
                lFixedWidthFile = IO.Path.ChangeExtension(lMPCADatafile, "dat")
                Dim lSW As New StreamWriter(lFixedWidthFile, False)
                Dim lOneLine As New StringBuilder
                Dim lYear As String = ""
                Dim lMonth As String = ""
                Dim lDayCol As Integer
                Dim lDayPrcp As Integer
                Dim lValue As String = ""
                .CurrentRecord = 1
                While Not .EOF
                    lOneLine.Append("MPA ")
                    'Take 'StationID, Coordinates/X,Y, STATYPE, Year, Mon, DDHH Value FLG1 FLG2
                    lOneLine.Append(.Value(7).Trim().PadLeft(4, " ") & " ") 'STATYPE - RecType
                    lOneLine.Append(.Value(6).Trim() & " ") 'StationID - Location
                    lOneLine.Append(.Value(1).Trim().PadLeft(20, " ") & " ") 'Coordinates
                    lOneLine.Append("PRCP HI ")
                    lYear = .Value(10).Trim() & " "
                    lMonth = .Value(11).Trim().PadLeft(2, "0") & "       " 'so as to make day1 record start on column 68 for the atcDataSourceNOAA's LongForm
                    lOneLine.Append(lYear) 'Year
                    lOneLine.Append(lMonth) 'Month

                    lDayCol = 12
                    For J As Integer = 1 To DayMon(lYear, lMonth)
                        'DD00 0.06 A _
                        'Fix value
                        If .Value(lDayCol) = "" Then
                            If .Value(lDayCol + 1) = "M" Then
                                lValue = "-99999"
                            ElseIf .Value(lDayCol + 1) = "|" Then
                                lValue = " 99999"
                            End If
                        Else
                            lDayPrcp = Double.Parse(.Value(lDayCol)) * 100.0
                            lValue = " " & lDayPrcp.ToString.PadLeft(5, "0")
                        End If

                        'Fix flag
                        If .Value(lDayCol + 1).Trim() = "|" Then 'replace continuation flag
                            .Value(lDayCol + 1) = "S"
                        ElseIf .Value(lDayCol + 1).Trim() = "S" Then 'we think these data values are already water equivalent
                            .Value(lDayCol + 1) = ""
                        End If
                        lOneLine.Append(Format(J, "00") & "00" & " " & lValue & " " & .Value(lDayCol + 1).PadLeft(1, " ") & " " & " " & " ")
                        lDayCol += 2
                    Next
                    lSW.WriteLine(lOneLine.ToString)
                    lOneLine.Clear()

                    .CurrentRecord += 1
                End While
                lSW.Flush()
                lSW.Close()
                lDataTable.Clear()
            End If
        End With
    End Sub
End Module
