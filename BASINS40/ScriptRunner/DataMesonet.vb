Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports System.IO
Imports System.Text

Imports System.Collections.Specialized
Imports atcMetCmp
Imports atcData

Module DataMesonet
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        Dim lDataDir As String = "G:\Admin\Mesonet\1994_1996Mesonet\" '<<< update this to your data directory


        Dim lSW As New StreamWriter(lDataDir & "aaaMesonetLog.txt", False)
        Dim lStations() As String = {"cook", "tahl", "webb", "webr", "west"}
        Dim lFieldNames() = {"", "STID", "STNM", "TIME", "RELH", "TAIR", "WSPD", "WVEC", "WDIR", "WDSD", "WSSD", "WMAX", "RAIN", "PRES", "SRAD", "TA9M", "WS2M", "TS10", "TB10", "TS05", "TB05", "TS30"}
        Dim lFieldWidth() = {0, 5, 6, 6, 7, 7, 7, 7, 6, 7, 7, 7, 8, 9, 6, 7, 7, 7, 7, 7, 7, 7}
        Dim lNewWDM As String = IO.Path.Combine(lDataDir, "new.wdm")
        Dim lStationWDM As String = ""
        Dim lFiles As NameValueCollection = Nothing

        For Each lStation As String In lStations
            lSW.WriteLine("Start processing station: " & lStation & " ...")

            'make a working copy of WDM
            lStationWDM = IO.Path.Combine(lDataDir, lStation & ".wdm")
            While Not TryCopy(lNewWDM, lStationWDM)
            End While

            Dim lStationWDMFH As New atcWDM.atcDataSourceWDM
            lStationWDMFH.Open(lStationWDM)

            lFiles = New NameValueCollection
            AddFilesInDir(lFiles, lDataDir, True, "*" & lStation & ".mts")
            lSW.WriteLine("Station " & lStation & " has " & lFiles.Count & " days of records.")
            Dim lDates As New atcTimeseries(Nothing)
            Dim lTsTAIR As New atcTimeseries(Nothing)
            With lTsTAIR.Attributes
                .SetValue("ID", 3)
                .SetValue("Constituent", "ATEM")
                .SetValue("Units", "deg C at 1.5m")
                .SetValue("Location", lStation)
                .SetValue("Scenario", "OBSERVED")
                .SetValue("tu", atcTimeUnit.TUMinute)
                .SetValue("ts", 5)
            End With
            Dim lTsRAIN As New atcTimeseries(Nothing)
            With lTsRAIN.Attributes
                .SetValue("ID", 1)
                .SetValue("Constituent", "PREC")
                .SetValue("Units", "mm")
                .SetValue("Location", lStation)
                .SetValue("Scenario", "OBSERVED")
                .SetValue("tu", atcTimeUnit.TUMinute)
                .SetValue("ts", 5)
            End With
            Dim lTsSRAD As New atcTimeseries(Nothing)
            With lTsSRAD.Attributes
                .SetValue("ID", 5)
                .SetValue("Constituent", "SOLR")
                .SetValue("Units", "W/m^2")
                .SetValue("Location", lStation)
                .SetValue("Scenario", "OBSERVED")
                .SetValue("tu", atcTimeUnit.TUMinute)
                .SetValue("ts", 5)
            End With
            Dim lTsWSPD As New atcTimeseries(Nothing)
            With lTsWSPD.Attributes
                .SetValue("ID", 4)
                .SetValue("Constituent", "WIND")
                .SetValue("Units", "m per s")
                .SetValue("Location", lStation)
                .SetValue("Scenario", "OBSERVED")
                .SetValue("tu", atcTimeUnit.TUMinute)
                .SetValue("ts", 5)
            End With
            Dim lTsWVEC As New atcTimeseries(Nothing)
            With lTsWVEC.Attributes
                .SetValue("ID", 41)
                .SetValue("Constituent", "WVEC")
                .SetValue("Units", "m per s")
                .SetValue("Location", lStation)
                .SetValue("Scenario", "OBSERVED")
                .SetValue("tu", atcTimeUnit.TUMinute)
                .SetValue("ts", 5)
            End With
            Dim lTsWDIR As New atcTimeseries(Nothing)
            With lTsWDIR.Attributes
                .SetValue("ID", 42)
                .SetValue("Constituent", "WDIR")
                .SetValue("Units", "deg at 10m")
                .SetValue("Location", lStation)
                .SetValue("Scenario", "OBSERVED")
                .SetValue("tu", atcTimeUnit.TUMinute)
                .SetValue("ts", 5)
            End With

            'Figure out the size

            Dim lValTAIR() As Double = Nothing
            Dim lValRAIN() As Double = Nothing
            Dim lValSRAD() As Double = Nothing
            Dim lValWSPD() As Double = Nothing
            Dim lValWVEC() As Double = Nothing
            Dim lValWDIR() As Double = Nothing
            Dim lValDates() As Double = Nothing
            Dim lDate(5) As Integer

            'lMTSTab.OpenFile(lFiles(0))
            'Dim lBegYear As Integer = Integer.Parse(lMTSTab.Header(2).Substring(5, 4))
            'Dim lBegMon As Integer = Integer.Parse(lMTSTab.Header(2).Substring(10, 2))
            'Dim lBegDay As Integer = Integer.Parse(lMTSTab.Header(2).Substring(13, 2))

            Dim lFileName As String = IO.Path.GetFileName(lFiles(0))
            Dim lBegYear As Integer = Integer.Parse(lFileName.Substring(0, 4))
            Dim lBegMon As Integer = Integer.Parse(lFileName.Substring(4, 2))
            Dim lBegDay As Integer = Integer.Parse(lFileName.Substring(6, 2))
            Dim lDate1 As Date = New Date(lBegYear, lBegMon, lBegDay)
            lDate(0) = lBegYear : lDate(1) = lBegMon : lDate(2) = lBegDay

            lFileName = IO.Path.GetFileName(lFiles(lFiles.Count - 1))
            Dim lEndYear As Integer = Integer.Parse(lFileName.Substring(0, 4))
            Dim lEndMon As Integer = Integer.Parse(lFileName.Substring(4, 2))
            Dim lEndDay As Integer = Integer.Parse(lFileName.Substring(6, 2))
            Dim lDate2 As Date = New Date(lEndYear, lEndMon, lEndDay)

            Dim lNumOfDaysInRecords As Integer = DateDiff(DateInterval.Day, lDate1, lDate2) + 1
            Dim lNumValues As Integer = lNumOfDaysInRecords * 288 '288 records in a day at 5 min step

            ReDim lValTAIR(lNumValues) : lValTAIR(0) = Double.NaN
            ReDim lValRAIN(lNumValues) : lValRAIN(0) = Double.NaN
            ReDim lValSRAD(lNumValues) : lValSRAD(0) = Double.NaN
            ReDim lValWSPD(lNumValues) : lValWSPD(0) = Double.NaN
            ReDim lValWVEC(lNumValues) : lValWVEC(0) = Double.NaN
            ReDim lValWDIR(lNumValues) : lValWDIR(0) = Double.NaN
            ReDim lValDates(lNumValues) : lValDates(0) = Date2J(lDate)

            lSW.WriteLine("Station " & lStation & " records from " & lBegYear & "/" & lBegMon & "/" & lBegDay & " to " & lEndYear & "/" & lEndMon & "/" & lEndDay)
            Dim lRecordCount As Integer = 1
            For Each lFile As String In lFiles
                Dim lMTSTab As New atcTableFixed
                With lMTSTab
                    .NumHeaderRows = 3
                    .NumFields = 21
                    Dim lStartIndex As Integer = 0
                    For I As Integer = 1 To .NumFields
                        .FieldName(I) = lFieldNames(I)
                        .FieldLength(I) = lFieldWidth(I)
                        If I = 1 Then
                            .FieldStart(I) = 1
                        Else
                            .FieldStart(I) = lStartIndex + 1
                        End If
                        lStartIndex += lFieldWidth(I)
                    Next
                End With
                lMTSTab.OpenFile(lFile) : lMTSTab.MoveFirst()
                While Not lMTSTab.EOF
                    lValDates(lRecordCount) = TimAddJ(lValDates(0), atcTimeUnit.TUMinute, 1, 5 * lRecordCount)
                    lValTAIR(lRecordCount) = lMTSTab.Value(5)
                    lValRAIN(lRecordCount) = lMTSTab.Value(12)
                    lValSRAD(lRecordCount) = lMTSTab.Value(14)
                    lValWSPD(lRecordCount) = lMTSTab.Value(6)
                    lValWVEC(lRecordCount) = lMTSTab.Value(7)
                    lValWDIR(lRecordCount) = lMTSTab.Value(8)

                    lMTSTab.MoveNext()
                    lRecordCount += 1
                End While

                'Clean up this table
                lMTSTab.Clear() : lMTSTab = Nothing
            Next

            'Construct TS and puts in WDM
            lDates.Values = lValDates
            lTsTAIR.Dates = lDates
            lTsTAIR.Values = lValTAIR
            lTsRAIN.Dates = lDates
            lTsRAIN.Values = lValRAIN
            lTsSRAD.Dates = lDates
            lTsSRAD.Values = lValSRAD
            lTsWSPD.Dates = lDates
            lTsWSPD.Values = lValWSPD
            lTsWVEC.Dates = lDates
            lTsWVEC.Values = lValWVEC
            lTsWDIR.Dates = lDates
            lTsWDIR.Values = lValWDIR

            Dim lAddSuccessful As Boolean = True

            If Not lStationWDMFH.AddDataset(lTsRAIN) Then
                lSW.WriteLine("Station " & lStation & " failed writing RAIN record to WDM.")
                lAddSuccessful = False
            End If

            If Not lStationWDMFH.AddDataset(lTsTAIR) Then
                lSW.WriteLine("Station " & lStation & " failed writing TAIR record to WDM.")
                lAddSuccessful = False
            End If

            If Not lStationWDMFH.AddDataset(lTsSRAD) Then
                lSW.WriteLine("Station " & lStation & " failed writing SRAD record to WDM.")
                lAddSuccessful = False
            End If

            If Not lStationWDMFH.AddDataset(lTsWSPD) Then
                lSW.WriteLine("Station " & lStation & " failed writing WSPD record to WDM.")
                lAddSuccessful = False
            End If

            If Not lStationWDMFH.AddDataset(lTsWVEC) Then
                lSW.WriteLine("Station " & lStation & " failed writing WVEC record to WDM.")
                lAddSuccessful = False
            End If

            If Not lStationWDMFH.AddDataset(lTsWDIR) Then
                lSW.WriteLine("Station " & lStation & " failed writing WDIR record to WDM.")
                lAddSuccessful = False
            End If

            If lAddSuccessful Then
                lSW.WriteLine("Station " & lStation & " DONE writing records to WDM.")
                lStationWDMFH.Clear() : lStationWDMFH = Nothing
            Else
                lSW.WriteLine("Station " & lStation & " writing data to WDM failed.")
            End If
            lSW.Flush()

            lDates.Clear() : lDates = Nothing
            lTsRAIN.Clear() : lTsRAIN = Nothing
            lTsSRAD.Clear() : lTsRAIN = Nothing
            lTsTAIR.Clear() : lTsTAIR = Nothing
            lTsWDIR.Clear() : lTsWDIR = Nothing
            lTsWSPD.Clear() : lTsWSPD = Nothing
            lTsWVEC.Clear() : lTsWVEC = Nothing

            ReDim lValTAIR(0)
            ReDim lValRAIN(0)
            ReDim lValSRAD(0)
            ReDim lValWSPD(0)
            ReDim lValWVEC(0)
            ReDim lValWDIR(0)
            ReDim lValDates(0)

            lFiles.Clear()
            lFiles = Nothing
        Next

        lSW.Flush()
        lSW.Close()
    End Sub
End Module
