Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Windows.Forms
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcMetCmp

Public Module SWMMCATHargraevesET
    Private Const pReportNanAsZero As Boolean = True
    Private pWorkingDir As String = "N:\Projects\W0164-ERG-SWMM-CAT\Data-Processing-for-SWC-and-SWMM-CAT\"
    'Private pWorkingDir As String = "C:\Projects\SWMM-CAT\test\"
    Private pOutputPath As String = pWorkingDir & "HargraevesET\"
    'Private pOutputPath As String = pWorkingDir & "WDMDebug\" '<<<Change here to your own WDM folder, lDataSource
    Private pAllStnFile As String = pWorkingDir & "resources\D4EM_PMET_updated.txt"
    'Private pSWATAllStnFile As String = pWorkingDir & "Stations\AllStns.txt" '<<< do not change this file name as it is the storage of all stations
    'Private pSWATStnFile As String = pOutputPath & "statwgn.txt" '<<< do not change this file name as it is required by the FORTRAN code
    'Private pAllSWATStnsList As Dictionary(Of String, String) = New Dictionary(Of String, String)

    Private pdoPET As Boolean = True ' Switch to do the actual PM PET calculation
    Private pdoReport As Boolean = True

    Private pStartYear As Integer = 1990
    Private pEndYear As Integer = 2020
    Private Const pFormatLL As String = "###0.00###"
    Private Const pFormat As String = "##,###,##0.000"

    Public pRunningTave As Double
    Public pRunningTrng As Double
    Public pRunningCount As Integer = 0
    Public pRunningFront As Integer = 0
    Public pMaxRunningCount As Integer = 7
    Public pRunningTaveVals(pMaxRunningCount - 1) As Double
    Public pRunningTrngVals(pMaxRunningCount - 1) As Double
    Public pJDay As Integer = 1
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("SWCHargET:Start")
        ChDriveDir(pOutputPath)
        Logger.StartToFile("SWCPMET.log")
        Logger.Dbg("SWCHargET:CurDir:" & CurDir())

        Dim lFileNames As New NameValueCollection
        Dim lStationNames As New atcCollection
        AddFilesInDir(lFileNames, pWorkingDir & "resources\temperature\", True, "*.txt")
        For Each lFileName As String In lFileNames
            lStationNames.Add(FilenameNoPath(FilenameNoExt(lFileName)))
        Next
        Logger.Dbg("SWCHargET: Found " & lFileNames.Count & " data files")

        Dim lProjectionFileNames As New NameValueCollection
        AddFilesInDir(lProjectionFileNames, pWorkingDir & "resources\", True, "TEMP*.txt")
        Logger.Dbg("SWCHargET: Found " & lProjectionFileNames.Count & " climate projection files")

        Dim lSJDSWCPMET As Double = Date2J(1990, 1, 1, 0, 0, 0)
        Dim lEJDSWCPMET As Double = Date2J(2020, 12, 31, 24, 0, 0)

        Dim lSw As System.IO.StreamWriter = New System.IO.StreamWriter(IO.Path.Combine(pOutputPath, "HET_historical.txt"), False)
        Dim lSW2035Central As System.IO.StreamWriter = New System.IO.StreamWriter(IO.Path.Combine(pOutputPath, "HET_2035Central.txt"), False)
        Dim lSW2035HotDry As System.IO.StreamWriter = New System.IO.StreamWriter(IO.Path.Combine(pOutputPath, "HET_2035HotDry.txt"), False)
        Dim lSW2035WetWarm As System.IO.StreamWriter = New System.IO.StreamWriter(IO.Path.Combine(pOutputPath, "HET_2035WetWarm.txt"), False)
        Dim lSW2060Central As System.IO.StreamWriter = New System.IO.StreamWriter(IO.Path.Combine(pOutputPath, "HET_2060Central.txt"), False)
        Dim lSW2060HotDry As System.IO.StreamWriter = New System.IO.StreamWriter(IO.Path.Combine(pOutputPath, "HET_2060HotDry.txt"), False)
        Dim lSW2060WetWarm As System.IO.StreamWriter = New System.IO.StreamWriter(IO.Path.Combine(pOutputPath, "HET_2060WetWarm.txt"), False)
        Dim lHeaderStr As String = "StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab &
            "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab &
            "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "SumAnnual"
        lSw.WriteLine(lHeaderStr)
        lSW2035Central.WriteLine(lHeaderStr)
        lSW2035HotDry.WriteLine(lHeaderStr)
        lSW2035WetWarm.WriteLine(lHeaderStr)
        lSW2060Central.WriteLine(lHeaderStr)
        lSW2060HotDry.WriteLine(lHeaderStr)
        lSW2060WetWarm.WriteLine(lHeaderStr)

        Dim lStationFile As New atcTableDelimited
        lStationFile.Delimiter = vbTab
        If lStationFile.OpenFile(pAllStnFile) Then
            Logger.Dbg("SWCHargET: Opened ATemp data station location file: " & pAllStnFile & " with " & lStationFile.NumRecords & " records")
        End If

        Dim lStnLat As Double
        Dim lYr As Integer
        Dim lMonth As Integer
        Dim lDay As Integer
        Dim lStnID As String

        'For Each lFile As String In lFileNames
        For iStnInd As Integer = 1 To lStationFile.NumRecords

            Dim lSJD As Double
            Dim lEJD As Double
            lStationFile.CurrentRecord = iStnInd
            lStnID = lStationFile.Value(1) ' FilenameNoPath(FilenameNoExt(lFile)).ToUpper
            Logger.Dbg("SWCHargET: Processing Station " & lStnID)
            Dim ltsHargET As New atcTimeseries

            If lStationNames.Contains(lStnID.ToLower) Then
                'If lStationFile.FindFirst(1, lStnID) Then
                Logger.Dbg("SWCHargET: Found matching station ID for data file " & lStnID)
                lStnLat = Double.Parse(lStationFile.Value(5))
                If lStnLat <= 65.2 Then
                    'Open temperature max/min data file
                    Dim lATempFile As New atcTableDelimited
                    lATempFile.Delimiter = vbTab
                    lATempFile.NumHeaderRows = 0
                    lATempFile.OpenFile(pWorkingDir & "resources\temperature\" & lStnID & ".txt")

                    'dimension 1 longer than NumRecords since 1st record is headers
                    Dim lTMin(lATempFile.NumRecords + 1) As Double
                    Dim lTMax(lATempFile.NumRecords + 1) As Double
                    Dim lETVals(lATempFile.NumRecords + 1) As Double
                    pRunningCount = 0
                    pRunningFront = 0

                    'get/save start date from 1st record, which is actualy field names
                    lYr = Integer.Parse(lATempFile.FieldName(2))
                    lMonth = Integer.Parse(lATempFile.FieldName(3))
                    lDay = Integer.Parse(lATempFile.FieldName(4))
                    lSJD = Date2J(lYr, lMonth, lDay)
                    lTMax(0) = Double.Parse(lATempFile.FieldName(5))
                    lTMin(0) = Double.Parse(lATempFile.FieldName(6))
                    UpdateTempMoveAve(lTMin(0), lTMax(0))
                    lETVals(1) = ETValueComputedbyHargraeves(pJDay, lStnLat, pRunningTave, pRunningTrng, True, -999)
                    'lSw.WriteLine("Hargraeves ET Values For Station " & lStnID & " at Latitude " & lStnLat)
                    'lSw.WriteLine(lYr & vbTab & lMonth & vbTab & lDay & vbTab & lETVals(1))

                    'assuming we're starting on Jan 1
                    pJDay = 1

                    'Calc Hargraeves ET for historic period
                    Logger.Dbg("SWCHargET:Hargraeves ET starts")
                    For iRec As Integer = 1 To lATempFile.NumRecords
                        lATempFile.CurrentRecord = iRec
                        lTMax(iRec) = Double.Parse(lATempFile.Value(5))
                        lTMin(iRec) = Double.Parse(lATempFile.Value(6))
                        lYr = Integer.Parse(lATempFile.Value(2))
                        lMonth = Integer.Parse(lATempFile.Value(3))
                        lDay = Integer.Parse(lATempFile.Value(4))
                        If lMonth = 1 AndAlso lDay = 1 Then
                            pJDay = 1
                        Else
                            pJDay += 1
                        End If
                        UpdateTempMoveAve(lTMin(iRec), lTMax(iRec))
                        lETVals(iRec + 1) = ETValueComputedbyHargraeves(pJDay, lStnLat, pRunningTave, pRunningTrng, True, -999)
                        'lSw.WriteLine(lYr & vbTab & lMonth & vbTab & lDay & vbTab & lETVals(iRec + 1))
                    Next
                    Logger.Dbg("SWCHargET:Hargraeves ET ends")
                    'get/save end date from last record
                    lYr = Integer.Parse(lATempFile.Value(2))
                    lEJD = Date2J(lYr, lMonth, lDay)

                    ltsHargET.Attributes.SetValue("Constituent", "HARGET")
                    'ltsHargET.Attributes.SetValue("ID", ltsAtemID + 6)
                    ltsHargET.Attributes.SetValue("TU", atcTimeUnit.TUDay)
                    ltsHargET.Attributes.SetValue("description", "Daily Hargraeves ET, in")
                    ltsHargET.Attributes.SetValue("interval", 1.0)
                    ltsHargET.Attributes.SetValue("TSTYPE", "PMET")
                    'J2Date(lSJD, lDate)
                    'ltsHargET.Attributes.SetValue("TSBYR", lDate(0))
                    ltsHargET.Attributes.SetValue("Latitude", lStnLat)

                    ltsHargET.Dates = New atcTimeseries
                    ltsHargET.Dates.Values = NewDates(lSJD, lEJD, atcTimeUnit.TUDay, 1)
                    ltsHargET.Values = lETVals

                    Dim lSeasonsMonth As New atcSeasonsMonth
                    Dim lMonthlyAnnualStr As String = lStnID & vbTab
                    Dim lMonthValues(12) As Double
                    For Each lTimeseriesMonth As atcTimeseries In lSeasonsMonth.Split(ltsHargET, Nothing)
                        Dim lMonthValue As Double = lTimeseriesMonth.Attributes.GetValue("SumAnnual")
                        If pReportNanAsZero AndAlso Double.IsNaN(lMonthValue) Then
                            lMonthValue = 0.0
                        ElseIf lMonthValue < 0.01 Then
                            lMonthValue = 0.0
                        End If
                        Dim lSeasonIndex As Integer = lTimeseriesMonth.Attributes.GetValue("SeasonIndex")
                        Select Case lSeasonIndex
                            Case 1, 3, 5, 7, 8, 10, 12
                                lMonthValues(lSeasonIndex) = lMonthValue / 31.0
                            Case 2
                                lMonthValues(lSeasonIndex) = lMonthValue / 28.25
                            Case Else
                                lMonthValues(lSeasonIndex) = lMonthValue / 30.0
                        End Select

                    Next

                    For I As Integer = 1 To 12
                        lMonthlyAnnualStr &= DoubleToString(lMonthValues(I), , pFormat) & vbTab
                    Next
                    lMonthlyAnnualStr &= DoubleToString(Double.Parse(ltsHargET.Attributes.GetValue("SumAnnual")),, pFormat)
                    lMonthlyAnnualStr.TrimEnd(vbTab)
                    lSw.WriteLine(lMonthlyAnnualStr)

                    Dim lTempAdj As Double
                    For Each lProjectionFile As String In lProjectionFileNames
                        'Calc Hargraeves ET for each projected period
                        Logger.Dbg("SWCHargET:Hargraeves ET starts for projection " & lProjectionFile)
                        Dim lTMinProj(lATempFile.NumRecords) As Double
                        Dim lTMaxProj(lATempFile.NumRecords) As Double
                        Dim lETValsProj(lATempFile.NumRecords + 1) As Double

                        'Open temperature adjustment projection file
                        Dim lATempAdjFile As New atcTableDelimited
                        lATempAdjFile.Delimiter = vbTab
                        lATempAdjFile.NumHeaderRows = 0
                        lATempAdjFile.OpenFile(lProjectionFile)
                        If lATempAdjFile.FindFirst(1, lStnID) Then

                            Dim lDate(5) As Integer
                            pRunningCount = 0
                            pRunningFront = 0
                            pJDay = 0
                            For iRec As Integer = 0 To lATempFile.NumRecords
                                J2Date(ltsHargET.Dates.Value(iRec), lDate)
                                lYr = Integer.Parse(lDate(0))
                                lMonth = Integer.Parse(lDate(1))
                                lDay = Integer.Parse(lDate(2))
                                lTempAdj = Double.Parse(lATempAdjFile.Value(lMonth + 1))
                                lTMaxProj(iRec) = lTMax(iRec) + lTempAdj
                                lTMinProj(iRec) = lTMin(iRec) + lTempAdj
                                If lMonth = 1 AndAlso lDay = 1 Then
                                    pJDay = 1
                                Else
                                    pJDay += 1
                                End If
                                UpdateTempMoveAve(lTMinProj(iRec), lTMaxProj(iRec))
                                lETValsProj(iRec + 1) = ETValueComputedbyHargraeves(pJDay, lStnLat, pRunningTave, pRunningTrng, True, -999)
                                'lSw.WriteLine(lYr & vbTab & lMonth & vbTab & lDay & vbTab & lETVals(iRec))
                            Next
                            Logger.Dbg("SWCHargET:Hargraeves ET ends")
                            ltsHargET.Values = lETValsProj

                            'Dim lSeasonsMonth As New atcSeasonsMonth
                            Dim lProjection As String = FilenameNoExt(FilenameNoPath(lProjectionFile))
                            lMonthlyAnnualStr = lStnID & vbTab
                            'Dim lMonthValues(12) As Double
                            For Each lTimeseriesMonth As atcTimeseries In lSeasonsMonth.Split(ltsHargET, Nothing)
                                Dim lMonthValue As Double = lTimeseriesMonth.Attributes.GetValue("SumAnnual")
                                If pReportNanAsZero AndAlso Double.IsNaN(lMonthValue) Then
                                    lMonthValue = 0.0
                                ElseIf lMonthValue < 0.01 Then
                                    lMonthValue = 0.0
                                End If
                                Dim lSeasonIndex As Integer = lTimeseriesMonth.Attributes.GetValue("SeasonIndex")
                                Select Case lSeasonIndex
                                    Case 1, 3, 5, 7, 8, 10, 12
                                        lMonthValues(lSeasonIndex) = lMonthValue / 31.0
                                    Case 2
                                        lMonthValues(lSeasonIndex) = lMonthValue / 28.25
                                    Case Else
                                        lMonthValues(lSeasonIndex) = lMonthValue / 30.0
                                End Select

                            Next

                            For I As Integer = 1 To 12
                                lMonthlyAnnualStr &= DoubleToString(lMonthValues(I), , pFormat) & vbTab
                            Next
                            lMonthlyAnnualStr &= DoubleToString(Double.Parse(ltsHargET.Attributes.GetValue("SumAnnual")),, pFormat)
                            lMonthlyAnnualStr.TrimEnd(vbTab)
                            Select Case lProjection
                                Case "temp2035central" : lSW2035Central.WriteLine(lMonthlyAnnualStr)
                                Case "temp2035hotdry" : lSW2035HotDry.WriteLine(lMonthlyAnnualStr)
                                Case "temp2035wetwarm" : lSW2035WetWarm.WriteLine(lMonthlyAnnualStr)
                                Case "temp2060central" : lSW2060Central.WriteLine(lMonthlyAnnualStr)
                                Case "temp2060hotdry" : lSW2060HotDry.WriteLine(lMonthlyAnnualStr)
                                Case "temp2060wetwarm" : lSW2060WetWarm.WriteLine(lMonthlyAnnualStr)
                            End Select

                        End If
                    Next
                Else
                    Logger.Dbg("Latitude (" & lStnLat & ") above reasonable limit for ET estimation for Station " & lStnID)
                End If
            Else
                Logger.Dbg("SWCHargET: Data file not found in Station " & lStnID)
            End If
EndCleanUp:
        Next 'Station IDs
        lSw.Close()
        lSW2035Central.Close()
        lSW2035HotDry.Close()
        lSW2035WetWarm.Close()
        lSW2060Central.Close()
        lSW2060HotDry.Close()
        lSW2060WetWarm.Close()
        lStationFile.Clear()
        lStationFile = Nothing

    End Sub

    ''' <summary>updates moving averages Of average daily temperature and daily temperature range stored in structure Tma.</summary>
    ''' <param name="tmin">minimum daily temperature (deg F)</param>
    ''' <param name="tmax">maximum daily temperature (deg F)</param>
    ''' <remarks></remarks>
    Private Sub UpdateTempMoveAve(ByVal tmin As Double, ByVal tmax As Double)

        'find ta And tr from New day's min and max temperature
        Dim ta As Double = (tmin + tmax) / 2.0
        Dim tr As Double = Math.Abs(tmax - tmin)

        If pRunningCount = pMaxRunningCount Then 'array used to store previous days' temperatures is full
            'update the moving averages with the New day's value
            pRunningTave = (pRunningTave * pRunningCount + ta - pRunningTaveVals(pRunningFront)) / pRunningCount
            pRunningTrng = (pRunningTrng * pRunningCount + tr - pRunningTrngVals(pRunningFront)) / pRunningCount

            'replace the values at the front of the moving average window
            pRunningTaveVals(pRunningFront) = ta
            pRunningTrngVals(pRunningFront) = tr

            'move the front one position forward
            pRunningFront += 1
            If pRunningFront = pRunningCount Then pRunningFront = 0

        Else 'array of previous day's values not full (at start of simulation)
            'find New moving averages by adding New values to previous ones
            pRunningTave = (pRunningTave * pRunningCount + ta) / (pRunningCount + 1)
            pRunningTrng = (pRunningTrng * pRunningCount + tr) / (pRunningCount + 1)

            'save New day's values
            pRunningTaveVals(pRunningFront) = ta
            pRunningTrngVals(pRunningFront) = tr

            'increment count And front of moving average window
            pRunningCount += 1
            pRunningFront += 1
            If (pRunningCount = pMaxRunningCount) Then pRunningFront = 0
        End If
    End Sub

End Module

