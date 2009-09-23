Imports atcData
Imports atcUtility
Imports atcWDM


Public Module UCIExample

    'Public Sub ScriptMain(ByRef aMapWin As IMapWin)
    Public Sub ScriptMain()
        Dim lWorkingDir As String = "C:\FTB\wdm\"
        ChDir(lWorkingDir)

        Dim lWDMDataSource As New atcWDM.atcDataSourceWDM
        If lWDMDataSource.Open("FBmet.wdm") Then    'use the WDM file name
            'successfully opened the wdm
            Dim lWDMDataSetIndex As Integer = lWDMDataSource.DataSets.IndexFromKey(102)  'use the WDM Dsn
            Dim lWDMDataSet As atcTimeseries = lWDMDataSource.DataSets.ItemByIndex(lWDMDataSetIndex)

            Dim lFlattenPath As String = lWorkingDir & "wdm.txt"
            Dim lJulianDate As Double
            Dim lDate(5) As Integer

            ' Create a file to write to.
            Dim lsw As System.IO.StreamWriter = System.IO.File.CreateText(lFlattenPath)

            'Set arrays of dates for begin/end of model
            'currently designed to work for midnight begin/end
            Dim lModelBegin() As Integer = {1999, 10, 1, 0, 0, 0}
            Dim lModelEnd() As Integer = {2006, 9, 30, 24, 0, 0}
            Dim lTableDelimiter As String = "  "

            Dim lDummy As New Collection
            lDummy.Add(100)
            lDummy.Add(200)
            lDummy.Add(300)

            MsgBox(lDummy.Cast(Of Double)().Average)


            Dim lMaxTEMPCurrentMonthTab As New Collection
            Dim lMinTEMPCurrentMonthTab As New Collection
            Dim lSOLRCurrentMonthTab As New Collection
            Dim lPRECCurrentMonthTab As New Collection

            'make timeseries

            'PREC
            Dim lTSPREC As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(107), Date2J(lModelBegin), Date2J(lModelEnd), lWDMDataSource)
            'ATEM
            Dim lTSATEM As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(13), Date2J(lModelBegin), Date2J(lModelEnd), lWDMDataSource)
            'DEWP
            Dim lTSDEWP As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(17), Date2J(lModelBegin), Date2J(lModelEnd), lWDMDataSource)
            'WIND
            Dim lTSWIND As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(14), Date2J(lModelBegin), Date2J(lModelEnd), lWDMDataSource)
            'SOLR
            Dim lTSSOLR As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(15), Date2J(lModelBegin), Date2J(lModelEnd), lWDMDataSource)
            'PEVT
            Dim lTSPEVT As atcTimeseries = modTimeseriesMath.SubsetByDate(lWDMDataSource.DataSets.ItemByKey(16), Date2J(lModelBegin), Date2J(lModelEnd), lWDMDataSource)

            'TempDayStatResults indexes:
            '0: min temperature
            '1: max temperature
            '2: solar radiation
            '3: wind velocity
            '4: wind direction
            '5: dew point temp
            Dim lTempDayStatResults(5) As Double
            Dim lTempRecord As Double
            Dim lDayOutString As String = ""
            Dim lDayPrecipAccumulator As Double

            Dim lHourIndexEnd As Integer = lTSPREC.Values.Length
            For i = 1 To lHourIndexEnd - 1
                RenderProgress(lHourIndexEnd, i, 0, 1)
                lJulianDate = lTSPREC.Dates.Values(i - 1)  'date associated with the first value 
                J2Date(lJulianDate, lDate)

                If lDate(3) = 0 AndAlso i <> lHourIndexEnd - 1 Then

                    'set all averaged values equal to zero
                    lTempDayStatResults(2) = 0
                    lTempDayStatResults(3) = 0
                    lTempDayStatResults(4) = 0
                    lTempDayStatResults(5) = 0
                    lDayPrecipAccumulator = 0

                    For j = 1 To 24
                        'ATEM: min and max temperature for the current day
                        lTempRecord = lTSATEM.Values(i + j - 1)
                        If j = 1 Then
                            lTempDayStatResults(0) = lTempRecord
                            lTempDayStatResults(1) = lTempRecord
                        Else
                            If lTempRecord < lTempDayStatResults(0) Then lTempDayStatResults(0) = lTempRecord
                            If lTempRecord > lTempDayStatResults(1) Then lTempDayStatResults(1) = lTempRecord
                        End If

                        'SOLR: daily radiation - assuming units are langleys/hour => find daily average and multiply by 24
                        lTempRecord = lTSSOLR.Values(i + j - 1)
                        lTempDayStatResults(2) = lTempDayStatResults(2) + lTempRecord

                        'WIND: wind velocity
                        lTempRecord = lTSWIND.Values(i + j - 1)
                        lTempDayStatResults(3) = lTempDayStatResults(3) + lTempRecord

                        'wind direction (set to zero for now)
                        'DNE -- lWDMDataSetTemp = lWDMDataSource.DataSets.ItemByIndex(xx)
                        lTempDayStatResults(4) = 0

                        'DEWP: dewpoint temp
                        lTempRecord = lTSDEWP.Values(i + j - 1)
                        lTempDayStatResults(5) = lTempDayStatResults(5) + lTempRecord

                        'write breakpoint line for this j hour
                        lDayOutString = lDayOutString & j & lTableDelimiter & lTSPREC.Values(i + j - 1)
                        lDayPrecipAccumulator += lTSPREC.Values(i + j - 1)

                        If j = 24 Then
                            'average things
                            lTempDayStatResults(2) = lTempDayStatResults(2) / 24
                            lTempDayStatResults(3) = lTempDayStatResults(3) / 24
                            lTempDayStatResults(5) = lTempDayStatResults(5) / 24
                        Else
                            lDayOutString = lDayOutString & vbCrLf
                        End If
                    Next

                    'write day line to table
                    lDayOutString = lDate(2) & lTableDelimiter _
                                  & lDate(1) & lTableDelimiter _
                                  & lDate(0) & lTableDelimiter _
                                  & "24" & lTableDelimiter _
                                  & lTempDayStatResults(1) & lTableDelimiter _
                                  & lTempDayStatResults(0) & lTableDelimiter _
                                  & lTempDayStatResults(2) & lTableDelimiter _
                                  & lTempDayStatResults(3) & lTableDelimiter _
                                  & lTempDayStatResults(4) & lTableDelimiter _
                                  & lTempDayStatResults(5) & lTableDelimiter _
                                  & vbCrLf _
                                  & lDayOutString
                    lsw.WriteLine(lDayOutString)
                    lDayOutString = ""

                    lMaxTEMPCurrentMonthTab.Add(lTempDayStatResults(1))
                    lMinTEMPCurrentMonthTab.Add(lTempDayStatResults(0))
                    lSOLRCurrentMonthTab.Add(lTempDayStatResults(2))
                    lPRECCurrentMonthTab.Add(lDayPrecipAccumulator)

                    If lDate(1) <> lTSPREC.Values(i + 1) Then
                        'month change, therefore we average the month.
                        'For k = 0 To lMaxTEMPCurrentMonthTab.Count - 1
                        'lMaxTEMPCurrentMonthTab.Cast(Of Double)().Average()
                        'Next

                    End If

                End If

            Next


            'clean up
            lsw.Flush()
            lsw.Close()
            lWDMDataSet = Nothing
            lWDMDataSource = Nothing
        End If

    End Sub

    Sub RenderProgress(ByVal intMaxValue As Integer, ByVal intProgress As Integer, ByVal intLeftPos As Integer, ByVal intTopPos As Integer)
        Dim intPercent As Integer
        If (intMaxValue > 0) Then
            intPercent = Math.Round((intProgress / intMaxValue) * 100, 0)
        Else
            intPercent = intProgress
        End If
        Console.CursorVisible = False
        Console.SetCursorPosition(intLeftPos, intTopPos)
        Console.Write(intProgress & "/" & intMaxValue & vbCrLf & "(" & intPercent & "%)" & vbCrLf)
        Console.CursorVisible = True
    End Sub

End Module