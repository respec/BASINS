Imports atcData
Imports atcUtility

Public Class atcUSGSScreen
    Public Shared Function PrintDataSummary(ByVal aTS As atcTimeseries) As String
        'print out data summary, usgs style
        'If pMissingDataMonth Is Nothing Then
        '    pMissingDataMonth = New atcCollection()
        'Else
        '    pMissingDataMonth.Clear()
        'End If
        'Dim lNeedToRecordMissingMonth As Boolean = (pMissingDataMonth.Count = 0)
        'Dim lNeedToRecordMissingMonth As Boolean = True
        Dim lFileName As String = IO.Path.GetFileName(aTS.Attributes.GetValue("History 1"))
        Dim lDate(5) As Integer
        Dim lStrBuilderDataSummary As New System.Text.StringBuilder
        lStrBuilderDataSummary.AppendLine("READING FILE NAMED " & lFileName)
        J2Date(aTS.Attributes.GetValue("SJDay"), lDate)
        lStrBuilderDataSummary.AppendLine("FIRST YEAR IN RECORD =  " & lDate(0))
        J2Date(aTS.Dates.Value(aTS.numValues - 1), lDate)
        lStrBuilderDataSummary.AppendLine(" LAST YEAR IN RECORD =  " & lDate(0))
        lStrBuilderDataSummary.AppendLine( _
            "                 MONTH        " & vbCrLf & _
            " YEAR   J F M A M J J A S O N D")

        For I As Integer = 0 To aTS.numValues - 1
            J2Date(aTS.Dates.Value(I), lDate)
            lStrBuilderDataSummary.Append(lDate(0).ToString.PadLeft(5, " "))
            Dim lDaysInMonth As Integer = DayMon(lDate(0), lDate(1))

            For M As Integer = 1 To 12
                Dim lMonthFlag As String = "."
                If lDate(1) = M Then
                    lDaysInMonth = DayMon(lDate(0), M)
                    Dim lDayInMonthDone As Integer = 0
                    While lDate(2) <= lDaysInMonth And lDate(1) = M
                        lDayInMonthDone += 1
                        If I = aTS.numValues Then Exit While
                        If Double.IsNaN(aTS.Value(I + 1)) OrElse aTS.Value(I + 1) < 0 Then
                            lMonthFlag = "X"
                        End If
                        I += 1
                        J2Date(aTS.Dates.Value(I), lDate)
                    End While
                    If lDayInMonthDone < lDaysInMonth Then
                        If lMonthFlag = "." Then lMonthFlag = "X"
                    End If
                    If M = 1 Then
                        lStrBuilderDataSummary.Append("   " & lMonthFlag)
                    ElseIf M = 12 Then
                        'End of one year
                        lStrBuilderDataSummary.AppendLine(" " & lMonthFlag)
                        I -= 1
                    Else
                        lStrBuilderDataSummary.Append(" " & lMonthFlag)
                    End If
                Else
                    If M = 1 Then
                        lStrBuilderDataSummary.Append("X".PadLeft(4, " "))
                    ElseIf M = 12 Then
                        lStrBuilderDataSummary.AppendLine(" X")
                    Else
                        lStrBuilderDataSummary.Append(" X")
                    End If
                End If
                'If lNeedToRecordMissingMonth Then
                '    Dim lKeyToBeAdded As String = lDate(0).ToString & "_" & M.ToString.PadLeft(2, "0")
                '    If Not pMissingDataMonth.Keys.Contains(lKeyToBeAdded) Then
                '        If lMonthFlag = "X" Then
                '            pMissingDataMonth.Add(lKeyToBeAdded, lMonthFlag)
                '        End If
                '    End If
                'End If
            Next 'month
        Next 'day
        lStrBuilderDataSummary.AppendLine("")
        lStrBuilderDataSummary.AppendLine(" COMPLETE RECORD = .      INCOMPLETE = X")

        'Dim lDataSummaryFilename As String = IO.Path.GetDirectoryName(aTS.Attributes.GetValue("History 1"))
        'lDataSummaryFilename = lDataSummaryFilename.Substring("Read from ".Length)
        'lDataSummaryFilename = IO.Path.Combine(lDataSummaryFilename, "DataSummary.txt")
        'Dim lSW As New StreamWriter(lDataSummaryFilename, False)
        'lSW.WriteLine(lStrBuilderDataSummary.ToString)
        'lSW.Flush()
        'lSW.Close()
        'lSW = Nothing
        'lStrBuilderDataSummary.Remove(0, lStrBuilderDataSummary.Length)

        Return lStrBuilderDataSummary.ToString()
    End Function
End Class
