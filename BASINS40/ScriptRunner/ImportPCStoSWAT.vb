Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcSeasons
Imports atcMwGisUtility

Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports System.Collections.Specialized

Module ScriptImportPCStoSWAT
    Enum PCSCOL
        LOCATION = 1
        DMRDATE = 10
        PARAM = 12
        PIPE = 13
        CAVG = 3
        CMAX = 4
        CMIN = 5
        QAVG = 18
        QMAX = 19
        QMIN = 20
    End Enum

    Enum SwatPCSCOL
        p0Month = 0
        p1YEAR = 1
        p2FLOMON = 2
        p3SEDMON = 3
        p4ORGNMON = 4
        p5ORGPMON = 5
        p6NO3MON = 6
        p7NH3MON = 7
        p8NO2MON = 8
        p9MINPMON = 9
        p10CBODMON = 10
        p11DISOXYMON = 11
        p12CHLAMON = 12
        p13SOLPSTMON = 13
        p14SRBPSTMON = 14
        p15BACTPMON = 15
        p16BACTLPMON = 16
        p17CMTL1MON = 17
        p18CMTL2MON = 18
        p19CMTL3MON = 19
    End Enum

    Private pDebug As Boolean = False

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir("C:\test")
        Logger.StartToFile("ScriptImportPCStoSWAT.log", , , True)

        Dim lPath As String = "C:\UMRB\Data\PCSSorted"
        Dim lLocationColumn As Integer = PCSCOL.LOCATION
        Dim lScenario As String = "Observed"
        Dim lDateColumn As Integer = PCSCOL.DMRDATE
        Dim lConstituentColumn As Integer = PCSCOL.PARAM
        Dim lPipeColumn As Integer = PCSCOL.PIPE
        Dim lCAVGColumn As Integer = PCSCOL.CAVG

        Dim logFile As String = IO.Path.Combine(lPath, "zConversionLog.txt")
        Dim lswLog As New IO.StreamWriter(logFile, False)

        Dim lPARAM As New Generic.Dictionary(Of String, String)
        lPARAM.Add("dolbd", "00300")
        lPARAM.Add("sslbd", "00530")
        lPARAM.Add("flowmgd", "50050")
        lPARAM.Add("cbodlbd", "80082")
        lPARAM.Add("bodlbd", "00310")

        Dim lFill As Boolean = False

        Dim lUserParms As New atcCollection
        With lUserParms
            .Add("CSV File pattern", lPath)
            .Add("Fill Monthly", lFill)
        End With
        Dim lAsk As New frmArgs
        If lAsk.AskUser("Specify full path of text file to import and WDM file to write into", lUserParms) Then
            With lUserParms
                lPath = .ItemByKey("CSV File pattern")
                lFill = .ItemByKey("Fill Monthly")
            End With

            'process the data into timeseries
            Dim lTSBuilders As atcData.atcTimeseriesGroupBuilder
            Dim lTSBuilder As atcData.atcTimeseriesBuilder
            Dim lDataSource As New atcTimeseriesSource
            Dim lValueStr As String = String.Empty
            Dim location As String = String.Empty
            Dim lValue As Double
            Dim lDate As Double
            Dim lConstituent As String = String.Empty
            Dim lDescription As String = String.Empty

            Dim lKey As String = String.Empty

            Dim lswatTemplate As String = IO.Path.Combine(lPath, "swat_template.csv")

            Dim lswatPCSInputFile As String = String.Empty
            Dim lDatePartition(5) As Integer
            Dim lfoundRecord As Boolean = False

            Dim lprevConstituentID As String = String.Empty
            Dim lthisConstituentID As String = String.Empty
            Dim lprevConstituentDate As String = String.Empty
            Dim lthisConstituentDate As String = String.Empty

            Dim lproblem As Boolean = False
            Dim lPARAMCount As New Generic.Dictionary(Of String, Integer)


            For Each lFilename As String In IO.Directory.GetFiles(lPath, "*.CSV")
                lproblem = False
                'For each point source file, create a new TimeseriesGroupBuilder
                lTSBuilders = New atcData.atcTimeseriesGroupBuilder(Nothing)
                location = IO.Path.GetFileName(lFilename).Substring(0, 9) ' e.g. IA0037687 etc
                lswatPCSInputFile = IO.Path.GetFileNameWithoutExtension(lFilename) & "_swat.csv"

                Dim lCSV As New atcTableDelimited
                With lCSV
                    .Delimiter = ","c
                    Logger.Dbg("Opening " & lFilename)
                    If .OpenFile(lFilename) Then
                        Logger.Dbg("Reading " & lFilename & " with " & .NumRecords & " records")
                        For lRecord As Integer = 1 To .NumRecords ' bypass the first title line internally
                            lfoundRecord = False
                            .CurrentRecord = lRecord
                            Try
                                lthisConstituentID = .Value(PCSCOL.PARAM).Trim()

                                'lDate = Jday(.Value(lYearColumn), .Value(lMonthColumn), .Value(lDayColumn), 24, 0, 0)
                                lDate = Date.Parse(.Value(PCSCOL.DMRDATE)).ToOADate
                                lthisConstituentDate = lDate
                                If Not lthisConstituentID = lprevConstituentID Then
                                    lprevConstituentID = lthisConstituentID
                                    lprevConstituentDate = lthisConstituentDate
                                Else ' the same constituent
                                    If lthisConstituentDate = lprevConstituentDate Then ' same date, then problem here
                                        Logger.Dbg("There are duplicate dates in PCS for : " & lthisConstituentID & vbCrLf & lFilename)
                                        lCSV.Clear() 'release memory
                                        lswLog.WriteLine("There are duplicate dates in PCS date: " & lthisConstituentID & lFilename)
                                        lswLog.Flush()
                                        lproblem = True
                                        Exit For
                                    Else
                                        lprevConstituentDate = lthisConstituentDate
                                    End If
                                End If

                                Select Case lthisConstituentID
                                    Case lPARAM("dolbd") ' 00300
                                        lKey = "dolbd"
                                        lConstituent = lPARAM("dolbd")
                                        lDescription = location & "_" & lKey
                                        lfoundRecord = True
                                        If Not lPARAMCount.ContainsKey(lKey) Then
                                            lPARAMCount.Add(lKey, 0)
                                        Else
                                            lPARAMCount.Item(lKey) += 1
                                        End If

                                    Case lPARAM("bodlbd") ' 00310
                                        lKey = "bodlbd"
                                        lConstituent = lPARAM("bodlbd")
                                        lDescription = location & "_" & lKey
                                        lfoundRecord = True
                                        If Not lPARAMCount.ContainsKey(lKey) Then
                                            lPARAMCount.Add(lKey, 0)
                                        Else
                                            lPARAMCount.Item(lKey) += 1
                                        End If
                                    Case lPARAM("sslbd") ' 00530
                                        lKey = "sslbd"
                                        lConstituent = lPARAM("sslbd")
                                        lDescription = location & "_" & lKey
                                        lfoundRecord = True
                                        If Not lPARAMCount.ContainsKey(lKey) Then
                                            lPARAMCount.Add(lKey, 0)
                                        Else
                                            lPARAMCount.Item(lKey) += 1
                                        End If
                                    Case lPARAM("flowmgd") ' 50050
                                        lKey = "flowmgd"
                                        lConstituent = lPARAM("flowmgd")
                                        lDescription = location & "_" & lKey
                                        lfoundRecord = True
                                        If Not lPARAMCount.ContainsKey(lKey) Then
                                            lPARAMCount.Add(lKey, 0)
                                        Else
                                            lPARAMCount.Item(lKey) += 1
                                        End If
                                    Case lPARAM("cbodlbd") ' 80082
                                        lKey = "cbodlbd"
                                        lConstituent = lPARAM("cbodlbd")
                                        lDescription = location & "_" & lKey
                                        lfoundRecord = True
                                        If Not lPARAMCount.ContainsKey(lKey) Then
                                            lPARAMCount.Add(lKey, 0)
                                        Else
                                            lPARAMCount.Item(lKey) += 1
                                        End If
                                End Select

                                If Not lfoundRecord Then
                                    Continue For
                                End If

                                'The keys are lower case as later search routines automatically convert them to lowercase
                                lTSBuilder = lTSBuilders.Builder(lKey) ' test for duplicate internally
                                If lTSBuilder.NumValues = 0 Then 'Set attributes of new builder
                                    lTSBuilder.Attributes.SetValue("Scenario", "PCS")
                                    lTSBuilder.Attributes.SetValue("Constituent", lConstituent)
                                    lTSBuilder.Attributes.SetValue("Location", location)
                                    lTSBuilder.Attributes.SetValue("Description", lDescription)
                                    If lFill Then
                                        lTSBuilder.Attributes.SetValue("tu", atcTimeUnit.TUMonth)
                                    Else
                                        lTSBuilder.Attributes.SetValue("tu", atcTimeUnit.TUUnknown)
                                    End If
                                    lTSBuilder.Attributes.SetValue("ts", 1)
                                End If

                                Try

                                    'Set value and unit
                                    lValueStr = .Value(PCSCOL.QAVG)

                                    If lTSBuilder.NumValues = 0 Then
                                        lTSBuilder.Attributes.SetValue("Unit", "lbd")
                                    End If

                                    If lValueStr = "" Then
                                        lValueStr = .Value(PCSCOL.CAVG)

                                        If lTSBuilder.NumValues = 0 Then
                                            If lKey.StartsWith("flow") Then
                                                lTSBuilder.Attributes.SetValue("Unit", "mgd")
                                            Else
                                                lTSBuilder.Attributes.SetValue("Unit", "mgl")
                                            End If
                                        End If
                                    End If

                                    lValue = GetNaN()
                                    If Not Double.TryParse(lValueStr, lValue) Then
                                        If lValueStr.Length = 0 Then
                                            Logger.Dbg("No value found at date " & DumpDate(lDate) & " for " & lKey & " in " & lFilename)
                                        Else
                                            Logger.Dbg("Could not parse value '" & lValueStr & "' at date " & DumpDate(lDate) & " for " & lKey & " in " & lFilename)
                                        End If
                                    End If
                                Catch
                                    Logger.Dbg("Error adding value at date " & DumpDate(lDate) & " for " & lKey & " in " & lFilename)
                                End Try
                                'DO is concentration, convert unit later
                                lTSBuilder.AddValue(lDate, lValue)
                            Catch
                                Logger.Dbg("Error adding value at date " & DumpDate(lDate) & " in " & lFilename)
                            End Try

                        Next lRecord
                    Else
                        Logger.Msg("Unable to open text file: '" & lFilename & "'", "PCS to SWAT")
                    End If
                End With

                If lproblem Then ' move onto the next PCS file
                    Continue For
                End If
                'Dim lUpdateReportSB As New Text.StringBuilder
                'lUpdateReportSB.AppendLine("Description" & vbTab & "Constituent" & vbTab & _
                '                           "Index" & vbTab & "Month" & vbTab & "Prev" & vbTab & _
                '                           "Intrp" & vbTab & "Next" & vbTab & "Mean" & vbTab & "MissCnt")
                'Write out to SWAT point source file
                writeSWATPointSource(lTSBuilders, lswatTemplate, lswatPCSInputFile, lPARAM)
                lPARAMCount.Clear()
            Next
        End If
        lswLog.Flush()
        lswLog.Close()
    End Sub

    Public Sub writeSWATPointSource(ByRef aTSBuilders As atcData.atcTimeseriesGroupBuilder, ByVal aswatTemplate As String, ByVal aswatPCSInputFile As String, ByVal aPARAM As Dictionary(Of String, String))
        Dim lsr As New IO.StreamReader(aswatTemplate)
        Dim lsw As IO.StreamWriter
        lsw = New IO.StreamWriter(aswatPCSInputFile, False)

        Dim lStartDates As New Generic.Dictionary(Of String, Double)
        Dim lTSBuilder As atcData.atcTimeseriesBuilder = Nothing

        'Find the earliest time for each time series
        Dim learliest As Double = Double.MaxValue
        Dim ldate As Double = 0.0
        Dim lTS As atcTimeseries
        For Each lkey As String In aPARAM.Keys
            lTSBuilder = aTSBuilders.Builder(lkey)
            'If lTSBuilder.Attributes.ContainsAttribute(lkey) Then
            'If lTSBuilder.Attributes.GetFormattedValue("key") = lkey Then
            If lTSBuilder.NumValues > 0 Then
                lTS = lTSBuilder.CreateTimeseries()
                ldate = lTS.Dates.Value(1)
                If ldate < learliest Then
                    learliest = ldate
                End If
                lStartDates.Add(lkey, ldate)
            End If
        Next

        ' If no pcs data available, the simply copy the template file
        ' to be the new swat input file
        If learliest = Double.MaxValue Then
            lsr.Close()
            lsw.Close()
            'copy
            'LaunchProgram("copy.exe /Y", My.Computer.FileSystem.CurrentDirectory, " " & aswatTemplate & " " & aswatPCSInputFile)
            IO.File.Copy(aswatTemplate, aswatPCSInputFile, True)
            Exit Sub
        End If


        Dim lSimStartYear As Integer = 1960
        Dim lSimStartMon As Integer = 1
        '1960-01-31: 21946

        ldate = Date.Parse("1960-01-01").ToOADate
        Dim lSimStartDates(5) As Integer
        J2Date(ldate, lSimStartDates)
        Dim ldate2StartDates(5) As Integer
        J2Date(learliest, ldate2StartDates)
        Dim lnumLines2Skip As Integer = lines2skip(lSimStartDates, ldate2StartDates)

        lnumLines2Skip += 1 ' plus the title line
        Dim line As String = String.Empty
        For i As Integer = 1 To lnumLines2Skip
            line = lsr.ReadLine()
            lsw.WriteLine(line)
        Next
        lsw.Flush()

        Dim lpcsCols(19) As String
        Dim lflow As Double
        Dim lsed As Double
        Dim lcbod As Double
        Dim lbod As Double
        Dim ldo As Double

        'Some how need a better search routine without having to create a new structure while searching!
        'lTSFlow.Attributes.GetFormattedValue("unit") --> "mgd"

        Dim lTSFlow As atcTimeseries = aTSBuilders.Builder("flowmgd").CreateTimeseries()
        Dim lTSSed As atcTimeseries = aTSBuilders.Builder("sslbd").CreateTimeseries()
        Dim lTSCBod As atcTimeseries = aTSBuilders.Builder("cbodlbd").CreateTimeseries()
        Dim lTSDO As atcTimeseries = aTSBuilders.Builder("dolbd").CreateTimeseries()
        Dim lTSBod As atcTimeseries = aTSBuilders.Builder("bodlbd").CreateTimeseries()

        Dim lhasFlow As Boolean = False
        Dim lhasSed As Boolean = False
        Dim lhasBod As Boolean = False
        Dim lhasCBod As Boolean = False
        Dim lhasDO As Boolean = False

        If lTSFlow.numValues > 0 Then lhasFlow = True
        If lTSSed.numValues > 0 Then lhasSed = True
        If lTSCBod.numValues > 0 Then lhasCBod = True
        If lTSBod.numValues > 0 Then lhasBod = True
        If lTSDO.numValues > 0 Then lhasDO = True

        Dim lDatePartition(5) As Integer
        Dim lYear As Integer
        Dim lMonth As Integer
        Dim lnewLine As String = String.Empty
        While Not lsr.EndOfStream
            line = lsr.ReadLine()
            lpcsCols = line.Split(",")
            lYear = Integer.Parse(lpcsCols(SwatPCSCOL.p1YEAR))
            lMonth = Integer.Parse(lpcsCols(SwatPCSCOL.p0Month))
            ldate = Date.Parse(lYear & "-" & lMonth & "-" & Str(daymon(lYear, lMonth))).ToOADate

            lflow = Double.NaN
            lsed = Double.NaN
            lcbod = Double.NaN
            lbod = Double.NaN
            ldo = Double.NaN


            'TODO: use the information of lTSFlow etc's Unit attribute to do the unit conversion for SWAT input
            Try
                'Need to do the unit conversion and proper formatting
                If lhasFlow Then
                    lflow = findMatchingPCSData(lTSFlow, ldate)
                End If
                If lflow.ToString = Double.NaN.ToString OrElse lflow < 0 Then
                    lflow = 0.0
                Else
                    'lflow = CType((lflow + 0.0005) * 1000, Integer) / 1000.0
                End If

                If lhasSed Then
                    lsed = findMatchingPCSData(lTSSed, ldate)
                End If
                If lsed.ToString = Double.NaN.ToString OrElse lsed < 0 Then
                    lsed = 0.0
                Else
                    'lsed = CType((lsed + 0.05) * 10, Integer) / 10.0
                End If

                If lhasBod Then
                    lbod = findMatchingPCSData(lTSBod, ldate)
                End If
                If lbod.ToString = Double.NaN.ToString OrElse lbod < 0 Then
                    lbod = 0.0
                Else
                    'lbod = CType((lbod + 0.05) * 10, Integer) / 10.0
                End If

                If lhasCBod Then
                    lcbod = findMatchingPCSData(lTSCBod, ldate)
                End If
                If lcbod.ToString = Double.NaN.ToString OrElse lcbod < 0 Then
                    lcbod = 0.0
                Else
                    'lcbod = CType((lcbod + 0.05) * 10, Integer) / 10.0
                End If

                If lhasDO Then
                    ldo = findMatchingPCSData(lTSDO, ldate)
                End If
                If ldo.ToString = Double.NaN.ToString OrElse ldo < 0 Then
                    ldo = 0.0
                Else
                    'ldo = CType((ldo + 0.05) * 10, Integer) / 10.0
                End If

            Catch ex As Exception
                Logger.Msg("Problem doing: " & vbCrLf & line, "Problem converting values")
            End Try

            lpcsCols(SwatPCSCOL.p2FLOMON) = DoubleToString(lflow)
            lpcsCols(SwatPCSCOL.p3SEDMON) = DoubleToString(lsed)
            lpcsCols(SwatPCSCOL.p10CBODMON) = DoubleToString(lcbod)
            lpcsCols(SwatPCSCOL.p11DISOXYMON) = DoubleToString(ldo)

            lnewLine = String.Join(",", lpcsCols)
            lsw.WriteLine(lnewLine)
            'line = String.Empty
        End While

        lsr.Close()
        lsw.Flush()
        lsw.Close()
    End Sub

    Public Function lines2skip(ByVal aSimStart() As Integer, ByVal adate2Start() As Integer) As Integer
        Dim lSimStartYear As Integer = aSimStart(0)
        Dim ldate2StartYear As Integer = adate2Start(0)
        Dim lSimStartMon As Integer = aSimStart(1) 'SWAT point source inputs always starts from a January
        Dim ldate2StartMon As Integer = adate2Start(1)

        Dim llines2skip As Integer = 0
        llines2skip = (ldate2StartYear - lSimStartYear) * 12 + ldate2StartMon - 1
        Return llines2skip
    End Function

    Public Function findMatchingPCSData(ByVal aTS As atcTimeseries, ByVal aDate As Double) As Double
        Dim lVal As Double = Double.NaN
        ' The following search would have to be faster if a long timeseries
        For i As Integer = 0 To aTS.numValues
            If aTS.Dates.Value(i) = aDate Then
                lVal = aTS.Value(i)
                Exit For
            End If
        Next
        Return lVal
    End Function
End Module
