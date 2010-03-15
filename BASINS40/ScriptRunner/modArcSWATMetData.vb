Imports atcData
Imports atcUtility
Imports atcWDM
Imports MapWinUtility

Module modArcSWATMetData

    Public SwatMetDataAlwaysEnsureComplete As Boolean = True
    Private pModifiedData As atcTimeseriesGroup

    Public Sub WriteSwatMetInput(ByVal aWDMMetFile As atcWDM.atcDataSourceWDM, _
                                 ByVal aStationLkUpTable As atcTableDBF, _
                                 ByVal aProjectFolder As String, _
                                 ByVal aSaveInFolder As String, _
                                 ByVal aDateStart As Double, _
                                 ByVal aDateEnd As Double, _
                                 Optional ByVal aStationList As String = "")

        'The aDateStart and aDateEnd are not used as the full range of dates are written out
        'Later version could enforce user-specified start and ending date
        'Logger.Dbg(MemUsage(), "Before Writing PCP")
        WritePCP(aWDMMetFile, aStationLkUpTable, aSaveInFolder, aDateStart, aDateEnd, True, aStationList)
        'Logger.Dbg(MemUsage(), "After Writing PCP")
        GC.Collect()
        System.Threading.Thread.Sleep(30)

        'Logger.Dbg(MemUsage(), "Before Writing TMP")
        WriteTMP(aWDMMetFile, aStationLkUpTable, aSaveInFolder, aDateStart, aDateEnd, True, aStationList)
        'Logger.Dbg(MemUsage(), "Before Writing TMP")
        GC.Collect()
        System.Threading.Thread.Sleep(30)

        'Logger.Dbg(MemUsage(), "Before Writing SLR")
        'WriteSLR(aWDMMetFile, aStationLkUpTable, aSaveInFolder, aDateStart, aDateEnd, True)
        'Logger.Dbg(MemUsage(), "Before Writing SLR")
        'GC.Collect()
        'GC.WaitForPendingFinalizers()

        'Logger.Dbg(MemUsage(), "Before Writing WND")
        'WriteWND(aWDMMetFile, aStationLkUpTable, aSaveInFolder, aDateStart, aDateEnd, True)
        'Logger.Dbg(MemUsage(), "Before Writing WND")
        'GC.Collect()
        'GC.WaitForPendingFinalizers()

        'Logger.Dbg(MemUsage(), "Before Writing PCP")
        'WritePET(aWDMMetFile, aStationLkUpTable, aSaveInFolder, aDateStart, aDateEnd, True)
        'Logger.Dbg(MemUsage(), "Before Writing PCP")
        'GC.Collect()
        'GC.WaitForPendingFinalizers()

        ''WriteDEWGroup(aWDMMetFile, aStationLkupTable, aSaveInFolder, aDateStart, aDateEnd)
        ''WriteCLOGroup(aWDMMetFile, aStationLkupTable, aSaveInFolder, aDateStart, aDateEnd)
    End Sub

    'Writes the SWAT precipitation file and returns the timeseries used to write the file
    Public Function WritePCP(ByVal aWDM As atcDataSource, _
                              ByVal aStationLkUpTable As atcTableDBF, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double, _
                              ByVal aWriteAll As Boolean, _
                              Optional ByVal aStationList As String = "") As atcTimeseries
        Dim lWDMconstituent As String = "PREC"
        If aWriteAll Then
            Dim lTSGroup As atcDataGroup = Nothing
            If aStationList <> "" Then 'screen stations to rid of duplicates or unnecessary ones
                lTSGroup = New atcDataGroup
                Dim lreader As System.IO.StreamReader = New IO.StreamReader(aStationList)
                lreader.ReadLine() ' rid of title line
                While Not lreader.EndOfStream
                    lTSGroup.Add(aWDM.DataSets.ItemByKey(Integer.Parse(lreader.ReadLine().Split(",")(2))))
                End While
                lreader.Close()
                lreader = Nothing
            Else
                lTSGroup = aWDM.DataSets.FindData("Constituent", lWDMconstituent)
            End If

            WritePCP(lTSGroup, aStationLkUpTable, aSaveInFolder, aDateStart, aDateEnd)
            lTSGroup.Clear()
            lTSGroup = Nothing
            Return Nothing
        Else
            Dim lTS As atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
            Return WritePCP(New atcDataGroup(lTS), aStationLkUpTable, aSaveInFolder, aDateStart, aDateEnd)
        End If
    End Function

    Private Function WritePCP(ByVal aTsGroup As atcDataGroup, _
                              ByVal aStationLkUpTable As atcTableDBF, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double) As atcTimeseries

        'Assuming the aTsGroup already contains the correct set of PREC TSs
        'Hence, no duplicates exist
        Dim lWriteDaily As Boolean = True
        SwatMetDataAlwaysEnsureComplete = False

        Dim lTS As atcTimeseries = Nothing
        Dim lpathPrec As String = IO.Path.Combine(aSaveInFolder, "Precipitation")

        'Make directories:
        If Not IO.Directory.Exists(lpathPrec) Then
            MkDir(lpathPrec)
        End If

        If aTsGroup IsNot Nothing Then

            Dim lStnList As New Dictionary(Of String, String)
            Dim lgageID As Integer = 1
            For Each lOrigTs As atcTimeseries In aTsGroup
                'lOrigTs.Attributes.SetValueIfMissing("Units", "in")
                'If lOrigTs.Attributes.GetFormattedValue("Location") = "MN210852" Then
                '    Logger.Msg("Step throught this one")
                'End If

                Dim loc As String = lOrigTs.Attributes.GetFormattedValue("Location").Substring(0, 8)
                If Not Integer.TryParse(loc.Substring(2), lgageID) Then
                    Logger.Msg("Extracting Station ID failed for station: " & loc)
                    Continue For
                End If

                ''Trim extra values not starting at the beginning of a day
                'Dim lDateStart(5) As Integer
                'J2Date(lOrigTs.Dates.Value(0), lDateStart)
                'If lDateStart(3) > 0 Then
                '    Logger.Dbg("Write ArcSWAT PCP input: found fractional day at beginning: " & lOrigTs.Attributes.GetFormattedValue("Location"))
                '    lDateStart(3) = 0
                '    Dim lDateNewStart(5) As Integer
                '    TIMADD(lDateStart, atcTimeUnit.TUDay, 1, 1, lDateNewStart)
                '    lTS = SubsetByDate(lOrigTs, Date2J(lDateNewStart), lOrigTs.Dates.Value(lOrigTs.numValues), Nothing)
                'Else
                '    lTS = lOrigTs
                'End If

                ''Trim extra values not ending at the end of a day
                'Dim lDateEnd(5) As Integer
                'J2Date(lTS.Dates.Value(lTS.numValues), lDateEnd)
                'If lDateEnd(3) > 0 Then
                '    Logger.Dbg("Write ArcSWAT PCP input: found fractional day at the end: " & lOrigTs.Attributes.GetFormattedValue("Location"))
                '    lDateEnd(3) = 0
                '    lTS = SubsetByDate(lTS, lTS.Dates.Value(0), Date2J(lDateEnd), Nothing)
                'End If

                'lTS = SubsetByDateBoundary(lOrigTs, lDate.Month, lDate.Day, Nothing)

                lTS = TrimTimeseries(lOrigTs)

                aStationLkUpTable.FindFirst(1, loc.Substring(2))
                Dim lLat As Double = aStationLkUpTable.Value(4) 'in decimal degrees
                Dim longitude As Double = aStationLkUpTable.Value(5) ' in decimal degrees
                Dim lElev As Double = aStationLkUpTable.Value(6) * 0.3048 'feet -> m

                lStnList.Add(loc, lgageID.ToString & "," & loc & "," & lLat.ToString & "," & longitude.ToString & "," & lElev.ToString)


                'lTS = SubsetByDate(lOrigTs, aDateStart, aDateEnd, Nothing)
                lTS = atcData.modTimeseriesMath.Aggregate(lTS, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                lTS = atcTimeseriesMath.atcTimeseriesMath.Compute("Multiply", lTS, 25.4) 'in to mm
                lTS.Attributes.SetValue("Units", "mm/day")
                If SwatMetDataAlwaysEnsureComplete Then lTS = EnsureComplete(lTS, aDateStart, aDateEnd, Nothing)

                Dim lTextFilename As String = IO.Path.Combine(lpathPrec, loc & ".txt")
                Dim lWriter As New IO.StreamWriter(lTextFilename, False)

                Dim lDate As Date
                For i As Integer = 1 To lTS.numValues
                    If i = 1 Then
                        lDate = Date.FromOADate(lTS.Dates.Value(i - 1))
                        lWriter.WriteLine(YYYYmmdd(lDate) & vbCrLf & DoubleToString(lTS.Value(i), 5, "##0.0", Nothing, "#", 5))
                    Else
                        lWriter.WriteLine(DoubleToString(lTS.Value(i), 5, "##0.0", Nothing, "#", 5))
                    End If
                Next
                lWriter.Flush()
                lWriter.Close()
                lWriter = Nothing

                lgageID = lgageID + 1
                lTS.Clear()
                lTS = Nothing
            Next

            Dim lPGAGELOC As New atcTableDBF
            lPGAGELOC.NumRecords = lStnList.Count
            lPGAGELOC.NumFields = 5
            lPGAGELOC.NumHeaderRows = 1

            lPGAGELOC.FieldName(1) = "ID"
            lPGAGELOC.FieldType(1) = "N"
            lPGAGELOC.FieldLength(1) = 6
            lPGAGELOC.FieldDecimalCount(1) = 0

            lPGAGELOC.FieldName(2) = "NAME"
            lPGAGELOC.FieldType(2) = "C"
            lPGAGELOC.FieldLength(2) = 8

            lPGAGELOC.FieldName(3) = "LAT"
            lPGAGELOC.FieldType(3) = "N"
            lPGAGELOC.FieldLength(3) = 8
            lPGAGELOC.FieldDecimalCount(3) = 4

            lPGAGELOC.FieldName(4) = "LONG"
            lPGAGELOC.FieldType(4) = "N"
            lPGAGELOC.FieldLength(4) = 8
            lPGAGELOC.FieldDecimalCount(4) = 4

            lPGAGELOC.FieldName(5) = "ELEVATION"
            lPGAGELOC.FieldType(5) = "N"
            lPGAGELOC.FieldLength(5) = 6
            lPGAGELOC.FieldDecimalCount(5) = 0

            lPGAGELOC.InitData()

            Dim lMetMetaDataLog As String = IO.Path.Combine(lpathPrec, "pGageLoc.dbf")
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lMetMetaDataLog))

            'Dim lWriterLog As New IO.StreamWriter(lMetMetaDataLog, False)
            With lPGAGELOC
                Dim lrecIndex As Integer = 1
                For Each lgage As String In lStnList.Keys
                    .CurrentRecord = lrecIndex
                    Dim arr() As String = lStnList.Item(lgage).Split(",")
                    .Value(1) = CInt(arr(0))
                    .Value(2) = arr(1)
                    .Value(3) = CType(Double.Parse(arr(2)) * 10000 + 0.5, Integer) / 10000
                    .Value(4) = CType(Double.Parse(arr(3)) * 10000 + 0.5, Integer) / 10000
                    .Value(5) = CType(Double.Parse(arr(4)), Integer)
                    lrecIndex += 1
                Next
            End With
            lPGAGELOC.WriteFile(lMetMetaDataLog)
            lPGAGELOC.Clear()
            lPGAGELOC = Nothing
            'lWriterLog.Flush()
            'lWriterLog.Close()

            lStnList.Clear()
            lStnList = Nothing
        Else
            Return Nothing
        End If

        Return lTS 'Only returns last dataset written, only useful when writing data from one station
    End Function

    Public Function WriteTMP(ByVal aWDM As atcDataSource, _
                             ByVal aStationLkUpTable As atcTableDBF, _
                             ByVal aSaveInFolder As String, _
                             ByVal aDateStart As Double, _
                             ByVal aDateEnd As Double, _
                             ByVal aWriteAll As Boolean, _
                             Optional ByVal aStationList As String = "") As atcTimeseries

        Dim lWDMconstituent As String = "ATEM"
        If aWriteAll Then

            Dim lTSGroup As atcDataGroup = Nothing
            If aStationList <> "" Then 'screen stations to rid of duplicates or unnecessary ones
                lTSGroup = New atcDataGroup
                Dim lreader As System.IO.StreamReader = New IO.StreamReader(aStationList)
                lreader.ReadLine() ' rid of title line
                Dim lDSN As Integer = 0
                While Not lreader.EndOfStream
                    lDSN = Integer.Parse(lreader.ReadLine().Split(",")(3))
                    If lDSN < 0 Then Continue While
                    lTSGroup.Add(aWDM.DataSets.ItemByKey(lDSN))
                End While
                lreader.Close()
                lreader = Nothing
            Else
                lTSGroup = aWDM.DataSets.FindData("Constituent", lWDMconstituent)
            End If

            WriteTMP(lTSGroup, aStationLkUpTable, aSaveInFolder, aDateStart, aDateEnd)
            If lTSGroup IsNot Nothing Then
                lTSGroup.Clear()
                lTSGroup = Nothing
            End If
            Return Nothing
        Else
            Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
            Return WriteTMP(New atcDataGroup(lTS), aStationLkUpTable, aSaveInFolder, aDateStart, aDateEnd)
        End If
    End Function

    Private Function WriteTMP(ByVal aTSGroup As atcDataGroup, _
                              ByVal aStationLkUpTable As atcTableDBF, _
                              ByVal aSaveInFolder As String, _
                              ByVal aDateStart As Double, _
                              ByVal aDateEnd As Double, _
                              Optional ByVal aStationList As String = "") As atcTimeseries

        'Assuming the aTsGroup already contains the correct set of PREC TSs
        'Hence, no duplicates exist
        Dim lWriteDaily As Boolean = True
        SwatMetDataAlwaysEnsureComplete = False

        Dim lTS As atcTimeseries = Nothing
        Dim lpathAtem As String = IO.Path.Combine(aSaveInFolder, "Temperature")

        'Make directories:
        If Not IO.Directory.Exists(lpathAtem) Then
            MkDir(lpathAtem)
        End If

        If aTSGroup IsNot Nothing Then

            Dim lStnList As New Dictionary(Of String, String)
            Dim lgageID As Integer = 1

            For Each lOrigTs As atcTimeseries In aTSGroup
                Debug.WriteLine(lOrigTs.Serial)
                'lOrigTs.EnsureValuesRead()
                'lOrigTs.Attributes.SetValueIfMissing("Units", "F")

                Dim loc As String = lOrigTs.Attributes.GetFormattedValue("Location").Substring(0, 8)
                If Not Integer.TryParse(loc.Substring(2), lgageID) Then
                    Logger.Msg("Extracting Station ID failed for station: " & loc)
                    Continue For
                End If

                ''Trim extra values not starting at the beginning of a day
                'Dim lDateStart(5) As Integer
                'J2Date(lOrigTs.Dates.Value(0), lDateStart)
                'If lDateStart(3) > 0 Then
                '    Logger.Dbg("Write ArcSWAT Tmp input: found fractional day at beginning: " & lOrigTs.Attributes.GetFormattedValue("Location"))
                '    lDateStart(3) = 0
                '    Dim lDateNewStart(5) As Integer
                '    TIMADD(lDateStart, atcTimeUnit.TUDay, 1, 1, lDateNewStart)
                '    lTS = SubsetByDate(lOrigTs, Date2J(lDateNewStart), lOrigTs.Dates.Value(lOrigTs.numValues), Nothing)
                'Else
                '    lTS = lOrigTs
                'End If

                ''Trim extra values not ending at the end of a day
                'Dim lDateEnd(5) As Integer
                'J2Date(lTS.Dates.Value(lTS.numValues), lDateEnd)
                'If lDateEnd(3) > 0 Then
                '    Logger.Dbg("Write ArcSWAT Tmp input: found fractional day at ending: " & lOrigTs.Attributes.GetFormattedValue("Location"))
                '    lDateEnd(3) = 0
                '    lTS = SubsetByDate(lTS, lTS.Dates.Value(0), Date2J(lDateEnd), Nothing)
                'End If

                lTS = TrimTimeseries(lOrigTs)

                aStationLkUpTable.FindFirst(1, loc.Substring(2))
                Dim lLat As Double = aStationLkUpTable.Value(4) 'in decimal degrees
                Dim longitude As Double = aStationLkUpTable.Value(5) ' in decimal degrees
                Dim lElev As Double = aStationLkUpTable.Value(6) * 0.3048 'feet -> m

                lStnList.Add(loc, lgageID.ToString & "," & loc & "," & lLat.ToString & "," & longitude.ToString & "," & lElev.ToString)

                'lTS = SubsetByDate(lOrigTs, aDateStart, aDateEnd, Nothing)

                Dim lTsMax As atcTimeseries = atcData.modTimeseriesMath.Aggregate(lTS, atcTimeUnit.TUDay, 1, atcTran.TranMax)
                lTsMax = atcTimeseriesMath.atcTimeseriesMath.Compute("F to Celsius", lTsMax)
                lTsMax.Attributes.SetValue("Units", "C")
                If SwatMetDataAlwaysEnsureComplete Then lTsMax = EnsureComplete(lTsMax, aDateStart, aDateEnd, Nothing)

                Dim lTsMin As atcTimeseries = atcData.modTimeseriesMath.Aggregate(lTS, atcTimeUnit.TUDay, 1, atcTran.TranMin)
                lTsMin = atcTimeseriesMath.atcTimeseriesMath.Compute("F to Celsius", lTsMin)
                lTsMin.Attributes.SetValue("Units", "C")
                If SwatMetDataAlwaysEnsureComplete Then lTsMin = EnsureComplete(lTsMin, aDateStart, aDateEnd, Nothing)


                Dim lTextFilename As String = IO.Path.Combine(lpathAtem, loc & ".txt")
                Dim lWriter As New IO.StreamWriter(lTextFilename, False)

                Dim lDate As Date
                For i As Integer = 1 To lTsMax.numValues
                    If i = 1 Then
                        lDate = Date.FromOADate(lTsMax.Dates.Value(i - 1))
                        lWriter.Write(YYYYmmdd(lDate) & vbCrLf)
                        lWriter.WriteLine(DoubleToString(lTsMax.Value(i), 5, "##0.0", Nothing, "#", 5) & "," & DoubleToString(lTsMin.Value(i), 5, "##0.0", Nothing, "#", 5))
                    Else
                        lWriter.WriteLine(DoubleToString(lTsMax.Value(i), 5, "##0.0", Nothing, "#", 5) & "," & DoubleToString(lTsMin.Value(i), 5, "##0.0", Nothing, "#", 5))
                    End If
                Next
                lWriter.Flush()
                lWriter.Close()
                lWriter = Nothing

                lgageID = lgageID + 1
                lTS.Clear()
                lTsMax.Clear()
                lTsMin.Clear()
                lTS = Nothing
                lTsMax = Nothing
                lTsMin = Nothing
            Next

            Dim lTGageLoc As New atcTableDBF
            lTGageLoc.NumRecords = lStnList.Count
            lTGageLoc.NumFields = 5
            lTGageLoc.NumHeaderRows = 1

            lTGageLoc.FieldName(1) = "ID"
            lTGageLoc.FieldType(1) = "N"
            lTGageLoc.FieldLength(1) = 6
            lTGageLoc.FieldDecimalCount(1) = 0

            lTGageLoc.FieldName(2) = "NAME"
            lTGageLoc.FieldType(2) = "C"
            lTGageLoc.FieldLength(2) = 8

            lTGageLoc.FieldName(3) = "LAT"
            lTGageLoc.FieldType(3) = "N"
            lTGageLoc.FieldLength(3) = 8
            lTGageLoc.FieldDecimalCount(3) = 4

            lTGageLoc.FieldName(4) = "LONG"
            lTGageLoc.FieldType(4) = "N"
            lTGageLoc.FieldLength(4) = 8
            lTGageLoc.FieldDecimalCount(4) = 4

            lTGageLoc.FieldName(5) = "ELEVATION"
            lTGageLoc.FieldType(5) = "N"
            lTGageLoc.FieldLength(5) = 6
            lTGageLoc.FieldDecimalCount(5) = 0

            lTGageLoc.InitData()

            Dim lMetMetaDataLog As String = IO.Path.Combine(lpathAtem, "tGageLoc.dbf")
            IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lMetMetaDataLog))

            'Dim lWriterLog As New IO.StreamWriter(lMetMetaDataLog, False)
            With lTGageLoc
                Dim lrecIndex As Integer = 1
                For Each lgage As String In lStnList.Keys
                    .CurrentRecord = lrecIndex
                    Dim arr() As String = lStnList.Item(lgage).Split(",")
                    .Value(1) = CInt(arr(0))
                    .Value(2) = arr(1)
                    .Value(3) = CType(Double.Parse(arr(2)) * 10000 + 0.5, Integer) / 10000
                    .Value(4) = CType(Double.Parse(arr(3)) * 10000 + 0.5, Integer) / 10000
                    .Value(5) = CType(Double.Parse(arr(4)), Integer)
                    lrecIndex += 1
                Next
            End With
            lTGageLoc.WriteFile(lMetMetaDataLog)
            lTGageLoc.Clear()
            lTGageLoc = Nothing

            lStnList.Clear()
            lStnList = Nothing
        Else
            Return Nothing
        End If

        Return lTS
    End Function

    'Private Sub WriteTMPline(ByVal aWriter As IO.StreamWriter, ByVal aDate As Date, ByVal aMax As Double, ByVal aMin As Double)
    '    If aMax < aMin Then
    '        aMax = -99
    '        aMin = -99
    '    Else
    '        If Double.IsNaN(aMax) Then aMax = -99
    '        If Double.IsNaN(aMin) Then aMin = -99
    '    End If
    '    aWriter.WriteLine(YYYYddd(aDate) & f51(aMax) & f51(aMin))
    'End Sub

    'Public Function WriteSLR(ByVal aWDM As atcDataSource, _
    '                          ByVal aSaveInFolder As String, _
    '                          ByVal aDateStart As Double, _
    '                          ByVal aDateEnd As Double, _
    '                          ByVal aWriteAll As Boolean) As atcTimeseries
    '    Dim lWDMconstituent As String = "SOLR" '"DSOL"
    '    If aWriteAll Then
    '        Dim lTSGroup As atcDataGroup = aWDM.DataSets.FindData("Constituent", lWDMconstituent)
    '        WriteSLR(lTSGroup, aSaveInFolder, aDateStart, aDateEnd)
    '        If lTSGroup IsNot Nothing Then
    '            lTSGroup.Clear()
    '            lTSGroup = Nothing
    '        End If
    '        Return Nothing
    '    Else
    '        Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
    '        Return WriteSLR(New atcDataGroup(lTS), aSaveInFolder, aDateStart, aDateEnd)
    '    End If
    'End Function

    'Private Function WriteSLR(ByVal aTSGroup As atcDataGroup, _
    '                          ByVal aSaveInFolder As String, _
    '                          ByVal aDateStart As Double, _
    '                          ByVal aDateEnd As Double) As atcTimeseries

    '    Dim lWriteDaily As Boolean = True
    '    SwatMetDataAlwaysEnsureComplete = False
    '    Dim lStnList As New ArrayList
    '    Dim lTS As atcTimeseries = Nothing

    '    If aTSGroup IsNot Nothing Then
    '        Dim lTsGroup As New atcTimeseriesGroup
    '        For Each lOrigTs As atcTimeseries In aTSGroup
    '            'lOrigTs.Attributes.SetValueIfMissing("Units", "Langley")
    '            'lOrigTs.EnsureValuesRead()
    '            lTS = atcData.modTimeseriesMath.Aggregate(lOrigTs, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
    '            lTS = atcTimeseriesMath.atcTimeseriesMath.Compute("Multiply", lTS, 0.484 * 0.0864) 'convert Langley to MJ/m2
    '            lTS.Attributes.SetValue("Units", "MJ/m2/day")
    '            If SwatMetDataAlwaysEnsureComplete Then lTS = EnsureComplete(lTS, aDateStart, aDateEnd, Nothing)
    '            lTsGroup.Add(lTS)
    '        Next

    '        Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, "slr.slr")
    '        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lTextFilename))
    '        Dim lWriter As New IO.StreamWriter(lTextFilename)

    '        lWriter.Write(MetGroupHeader(aTSGroup))
    '        'lWriter.WriteLine(MetHeader(aTS)) 'Not processed by model

    '        Dim lDate As Date
    '        Dim lValue As Double

    '        For Each lTS In lTsGroup
    '            lStnList.Add(lTS.Attributes.GetFormattedValue("Location"))
    '        Next

    '        Dim lGridSource As New atcData.atcTimeseriesGridSource(lTsGroup, New ArrayList, True)
    '        lGridSource.DateFormat.IncludeDays = True
    '        lGridSource.DateFormat.IncludeHours = False
    '        lGridSource.DateFormat.IncludeMinutes = False

    '        For lRow As Integer = 1 To lGridSource.Rows - 1
    '            lDate = Date.Parse(lGridSource.CellValue(lRow, 0))
    '            If lWriteDaily Then
    '                lWriter.Write(YYYYddd(lDate))
    '            Else
    '                lWriter.Write(YYYYddd(lDate) & Format(lDate, "HH:mm"))
    '            End If
    '            For lCol As Integer = 1 To lGridSource.Columns - 1
    '                If Double.TryParse(lGridSource.CellValue(lRow, lCol), lValue) Then
    '                    If Double.IsNaN(lValue) Then lValue = -99.9
    '                    lWriter.Write(f51(lValue))
    '                Else
    '                    lWriter.Write(f51(-99.9))
    '                End If
    '            Next
    '            lWriter.Write(vbCrLf)
    '        Next
    '        lWriter.Close()
    '        lTsGroup.Clear()
    '        lTsGroup = Nothing
    '    Else
    '        Return Nothing
    '    End If

    '    Dim lMetMetaDataLog As String = IO.Path.Combine(aSaveInFolder, "slr1_log.txt")
    '    IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lMetMetaDataLog))
    '    Dim lWriterLog As New IO.StreamWriter(lMetMetaDataLog, False)

    '    'SLR file holds upto 300 stns
    '    For i As Integer = 0 To lStnList.Count - 1
    '        lWriterLog.WriteLine("Column " & String.Format("{0:###}", i + 1).PadLeft(3) & "->" & lStnList(i).ToString)
    '    Next
    '    lWriterLog.Flush()
    '    lWriterLog.Close()

    '    lStnList.Clear()
    '    lStnList = Nothing

    '    Return lTS
    'End Function

    'Public Function WriteWND(ByVal aWDM As atcDataSource, _
    '                          ByVal aSaveInFolder As String, _
    '                          ByVal aDateStart As Double, _
    '                          ByVal aDateEnd As Double, _
    '                          ByVal aWriteAll As Boolean) As atcTimeseries
    '    Dim lWDMconstituent As String = "WIND"
    '    If aWriteAll Then
    '        Dim lTSGroup As atcDataGroup = aWDM.DataSets.FindData("Constituent", lWDMconstituent)
    '        WriteWND(lTSGroup, aSaveInFolder, aDateStart, aDateEnd)
    '        If lTSGroup IsNot Nothing Then
    '            lTSGroup.Clear()
    '            lTSGroup = Nothing
    '        End If
    '        Return Nothing
    '    Else

    '        Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
    '        Return WriteWND(New atcDataGroup(lTS), aSaveInFolder, aDateStart, aDateEnd)
    '    End If
    'End Function

    'Private Function WriteWND(ByVal aTSGroup As atcDataGroup, _
    '                          ByVal aSaveInFolder As String, _
    '                          ByVal aDateStart As Double, _
    '                          ByVal aDateEnd As Double) As atcTimeseries
    '    Dim lWriteDaily As Boolean = True
    '    SwatMetDataAlwaysEnsureComplete = False
    '    Dim lStnList As New ArrayList
    '    Dim lTS As atcTimeseries = Nothing

    '    If aTSGroup IsNot Nothing Then
    '        Dim lTsGroup As New atcTimeseriesGroup
    '        For Each lOrigTs As atcTimeseries In aTSGroup
    '            'lOrigTs.Attributes.SetValueIfMissing("Units", "mph")
    '            'lOrigTs.EnsureValuesRead()
    '            lTS = atcData.modTimeseriesMath.Aggregate(lOrigTs, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
    '            lTS = atcTimeseriesMath.atcTimeseriesMath.Compute("Multiply", lTS, 0.44704) 'convert wind speed miles per hour to meters per second
    '            lTS.Attributes.SetValue("Units", "m/s")
    '            If SwatMetDataAlwaysEnsureComplete Then lTS = EnsureComplete(lTS, aDateStart, aDateEnd, Nothing)
    '            lTsGroup.Add(lTS)
    '        Next

    '        'Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, lTSWIND.Attributes.GetValue("Location", "met") & ".wnd")
    '        Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, "wnd.wnd")
    '        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lTextFilename))
    '        Dim lWriter As New IO.StreamWriter(lTextFilename)

    '        lWriter.Write(MetGroupHeader(aTSGroup))
    '        'lWriter.WriteLine(MetHeader(aTS)) 'Not processed by model

    '        Dim lDate As Date
    '        Dim lValue As Double

    '        For Each lTS In lTsGroup
    '            lStnList.Add(lTS.Attributes.GetFormattedValue("Location"))
    '        Next

    '        Dim lGridSource As New atcData.atcTimeseriesGridSource(lTsGroup, New ArrayList, True)
    '        lGridSource.DateFormat.IncludeDays = True
    '        lGridSource.DateFormat.IncludeHours = False
    '        lGridSource.DateFormat.IncludeMinutes = False

    '        For lRow As Integer = 1 To lGridSource.Rows - 1
    '            lDate = Date.Parse(lGridSource.CellValue(lRow, 0))
    '            If lWriteDaily Then
    '                lWriter.Write(YYYYddd(lDate))
    '            Else
    '                lWriter.Write(YYYYddd(lDate) & Format(lDate, "HH:mm"))
    '            End If
    '            For lCol As Integer = 1 To lGridSource.Columns - 1
    '                If Double.TryParse(lGridSource.CellValue(lRow, lCol), lValue) Then
    '                    If Double.IsNaN(lValue) Then lValue = -99.9
    '                    lWriter.Write(f51(lValue))
    '                Else
    '                    lWriter.Write(f51(-99.9))
    '                End If
    '            Next
    '            lWriter.Write(vbCrLf)
    '        Next

    '        lWriter.Close()
    '        lTsGroup.Clear()
    '        lTsGroup = Nothing
    '    Else
    '        Return Nothing
    '    End If

    '    Dim lMetMetaDataLog As String = IO.Path.Combine(aSaveInFolder, "wnd1_log.txt")
    '    IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lMetMetaDataLog))
    '    Dim lWriterLog As New IO.StreamWriter(lMetMetaDataLog, False)

    '    'WND file holds upto 300 stns
    '    For i As Integer = 0 To lStnList.Count - 1
    '        lWriterLog.WriteLine("Column " & String.Format("{0:###}", i + 1).PadLeft(3) & "->" & lStnList(i).ToString)
    '    Next
    '    lWriterLog.Flush()
    '    lWriterLog.Close()

    '    lStnList.Clear()
    '    lStnList = Nothing

    '    Return lTS
    'End Function

    ''Write the SWAT Potential EvapoTranspiration file and return the timeseries used to write the file
    'Public Function WritePET(ByVal aWDM As atcDataSource, _
    '                          ByVal aSaveInFolder As String, _
    '                          ByVal aDateStart As Double, _
    '                          ByVal aDateEnd As Double, _
    '                          ByVal aWriteAll As Boolean) As atcTimeseries
    '    Dim lWDMconstituent As String = "PEVT"
    '    If aWriteAll Then
    '        Dim lTSGroup As atcDataGroup = aWDM.DataSets.FindData("Constituent", lWDMconstituent)
    '        WritePET(lTSGroup, aSaveInFolder, aDateStart, aDateEnd)
    '        If lTSGroup IsNot Nothing Then
    '            lTSGroup.Clear()
    '            lTSGroup = Nothing
    '        End If
    '        Return Nothing
    '    Else
    '        Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
    '        Return WritePET(New atcDataGroup(lTS), aSaveInFolder, aDateStart, aDateEnd)
    '    End If
    'End Function

    'Private Function WritePET(ByVal aTSGroup As atcDataGroup, _
    '                          ByVal aSaveInFolder As String, _
    '                          ByVal aDateStart As Double, _
    '                          ByVal aDateEnd As Double) As atcTimeseries

    '    Dim lWriteDaily As Boolean = True
    '    SwatMetDataAlwaysEnsureComplete = False
    '    Dim lStnList As New ArrayList
    '    Dim lTS As atcTimeseries = Nothing

    '    If aTSGroup IsNot Nothing Then
    '        Dim lTsGroup As New atcTimeseriesGroup
    '        For Each lOrigTs As atcTimeseries In aTSGroup
    '            'lOrigTs.Attributes.SetValueIfMissing("Units", "in")
    '            'lOrigTs.EnsureValuesRead()
    '            lTS = atcData.modTimeseriesMath.Aggregate(lOrigTs, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
    '            lTS = atcTimeseriesMath.atcTimeseriesMath.Compute("Multiply", lTS, 25.4)
    '            lTS.Attributes.SetValue("Units", "mm/day")
    '            If SwatMetDataAlwaysEnsureComplete Then lTS = EnsureComplete(lTS, aDateStart, aDateEnd, Nothing)
    '            lTsGroup.Add(lTS)
    '        Next

    '        Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, "pet1.pet")
    '        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lTextFilename))
    '        Dim lWriter As New IO.StreamWriter(lTextFilename)

    '        lWriter.Write(MetGroupHeader(aTSGroup))
    '        'lWriter.WriteLine(MetHeader(aTS)) 'Not processed by model
    '        'lWriter.WriteLine(LatLonElev(aTS))

    '        Dim lDate As Date
    '        Dim lValue As Double

    '        For Each lTS In lTsGroup
    '            lStnList.Add(lTS.Attributes.GetFormattedValue("Location"))
    '        Next

    '        Dim lGridSource As New atcData.atcTimeseriesGridSource(lTsGroup, New ArrayList, True)
    '        lGridSource.DateFormat.IncludeDays = True
    '        lGridSource.DateFormat.IncludeHours = False
    '        lGridSource.DateFormat.IncludeMinutes = False

    '        For lRow As Integer = 1 To lGridSource.Rows - 1
    '            lDate = Date.Parse(lGridSource.CellValue(lRow, 0))
    '            If lWriteDaily Then
    '                lWriter.Write(YYYYddd(lDate))
    '            Else
    '                lWriter.Write(YYYYddd(lDate) & Format(lDate, "HH:mm"))
    '            End If
    '            For lCol As Integer = 1 To lGridSource.Columns - 1
    '                If Double.TryParse(lGridSource.CellValue(lRow, lCol), lValue) Then
    '                    If Double.IsNaN(lValue) Then lValue = -99.9
    '                    lWriter.Write(f51(lValue))
    '                Else
    '                    lWriter.Write(f51(-99.9))
    '                End If
    '            Next
    '            lWriter.Write(vbCrLf)
    '        Next

    '        lWriter.Close()
    '        lTsGroup.Clear()
    '        lTsGroup = Nothing
    '    Else
    '        Return Nothing
    '    End If

    '    Dim lMetMetaDataLog As String = IO.Path.Combine(aSaveInFolder, "pet1_log.txt")
    '    IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lMetMetaDataLog))
    '    Dim lWriterLog As New IO.StreamWriter(lMetMetaDataLog, False)

    '    'PCP file holds upto 300 stns
    '    For i As Integer = 0 To lStnList.Count - 1
    '        lWriterLog.WriteLine("Column " & String.Format("{0:###}", i + 1).PadLeft(3) & "->" & lStnList(i).ToString)
    '    Next
    '    lWriterLog.Flush()
    '    lWriterLog.Close()

    '    lStnList.Clear()
    '    lStnList = Nothing

    '    Return lTS
    'End Function

    ''Write the SWAT relative humidity file and return the timeseries used to write the file
    'Private Function WriteHMD(ByVal aWDM As atcDataSource, _
    '                          ByVal aSaveInFolder As String, _
    '                          ByVal aDateStart As Double, _
    '                          ByVal aDateEnd As Double) As atcTimeseries
    '    Dim lWDMconstituent As String = "DEWP"
    '    Dim lDew As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
    '    Dim lTemp As atcData.atcTimeseries = GetWDMTimeseries(aWDM, "ATEM")

    '    Return WriteHMD(lDew, lTemp, aSaveInFolder, aDateStart, aDateEnd)
    'End Function

    'Private Function WriteHMD(ByVal lDew As atcTimeseries, _
    '                          ByVal lTemp As atcTimeseries, _
    '                          ByVal aSaveInFolder As String, _
    '                          ByVal aDateStart As Double, _
    '                          ByVal aDateEnd As Double) As atcTimeseries
    '    Dim lHMD As atcTimeseries = Nothing
    '    If lDew IsNot Nothing AndAlso lTemp IsNot Nothing Then
    '        lDew.EnsureValuesRead()
    '        lTemp.EnsureValuesRead()
    '        lTemp = atcData.modTimeseriesMath.Aggregate(lTemp, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
    '        If SwatMetDataAlwaysEnsureComplete Then lTemp = EnsureComplete(lTemp, aDateStart, aDateEnd, Nothing)
    '        lTemp = atcTimeseriesMath.atcTimeseriesMath.Compute("F to Celsius", lTemp)

    '        lDew = atcData.modTimeseriesMath.Aggregate(lDew, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
    '        If SwatMetDataAlwaysEnsureComplete Then lDew = EnsureComplete(lDew, aDateStart, aDateEnd, Nothing)
    '        lDew = atcTimeseriesMath.atcTimeseriesMath.Compute("F to Celsius", lDew)

    '        Dim lTextFilename As String = IO.Path.Combine(aSaveInFolder, "hmd1.hmd")
    '        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lTextFilename))
    '        Dim lWriter As New IO.StreamWriter(lTextFilename)
    '        lWriter.WriteLine(MetHeader(lDew)) 'Not processed by model
    '        lWriter.WriteLine(LatLonElev(lDew))

    '        lHMD = lDew.Clone
    '        With lHMD.Attributes
    '            .SetValue("Constituent", "Relative Humidity")
    '            .SetValue("Units", "fraction")
    '        End With

    '        Dim lDate As Date
    '        Dim lDewValue As Double
    '        Dim lTempValue As Double
    '        Dim lValue As Double
    '        For lIndex As Integer = 1 To lDew.numValues
    '            lDate = Date.FromOADate(lDew.Dates.Value(lIndex - 1))

    '            lDewValue = lDew.Value(lIndex)
    '            lTempValue = lTemp.Value(lIndex)

    '            If lDewValue > lTempValue Then
    '                Debug.WriteLine("dew > temp, " & DoubleToString(lDewValue) & " > " & DoubleToString(lTempValue))
    '            End If

    '            lValue = Math.Exp((17.269 * lDewValue) / (273.3 + lDewValue)) _
    '                   / Math.Exp((17.269 * lTempValue) / (273.3 + lTempValue))
    '            lHMD.Value(lIndex) = lValue
    '            If Double.IsNaN(lValue) Then lValue = -99
    '            lWriter.WriteLine(YYYYddd(lDate) & f83(lValue))
    '        Next
    '        lWriter.Close()
    '    End If
    '    Return lHMD
    'End Function

    ''returns the dewpoint timeseries in degrees C, does not write SWAT input
    'Private Function WriteDEW(ByVal aWDM As atcDataSource, _
    '                          ByVal aSaveInFolder As String, _
    '                          ByVal aDateStart As Double, _
    '                          ByVal aDateEnd As Double) As atcTimeseries
    '    Dim lWDMconstituent As String = "DEWP"
    '    Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
    '    If lTS IsNot Nothing Then
    '        lTS.EnsureValuesRead()
    '        lTS = atcData.modTimeseriesMath.Aggregate(lTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)

    '        If SwatMetDataAlwaysEnsureComplete Then lTS = EnsureComplete(lTS, aDateStart, aDateEnd, Nothing)

    '        lTS = atcTimeseriesMath.atcTimeseriesMath.Compute("F to Celsius", lTS)
    '        lTS.Attributes.SetValue("Units", "C")
    '    End If
    '    Return lTS
    'End Function

    ''return the filled cloud cover dataset, does not write SWAT input
    'Private Function WriteCLO(ByVal aWDM As atcDataSource, _
    '                          ByVal aSaveInFolder As String, _
    '                          ByVal aDateStart As Double, _
    '                          ByVal aDateEnd As Double) As atcTimeseries
    '    Dim lWDMconstituent As String = "CLOU"
    '    Dim lTS As atcData.atcTimeseries = GetWDMTimeseries(aWDM, lWDMconstituent)
    '    If lTS IsNot Nothing Then
    '        lTS = atcData.modTimeseriesMath.Aggregate(lTS, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)

    '        If SwatMetDataAlwaysEnsureComplete Then lTS = EnsureComplete(lTS, aDateStart, aDateEnd, Nothing)

    '        'lTS.Attributes.SetValueIfMissing("Units", "tenths")
    '        lTS = atcTimeseriesMath.atcTimeseriesMath.Compute("Multiply", lTS, 0.1)
    '        lTS.Attributes.SetValue("Units", "fraction")
    '    End If
    '    Return lTS
    'End Function

    ''' <summary>
    ''' Return the modified data with this constituent if any, otherwise return the data from the given data source
    ''' </summary>
    ''' <param name="aWDM">Data source containing original data</param>
    ''' <param name="aWDMConstituentName">Name of constituent to search for</param>
    ''' <returns>uses module variable pModifiedData</returns>
    ''' <remarks>Does not yet support multiple met stations in one run</remarks>
    Private Function GetWDMTimeseries(ByVal aWDM As atcDataSource, ByVal aWDMConstituentName As String) As atcTimeseries
        ' Search modified data for given constituent
        If pModifiedData IsNot Nothing Then
            For Each lTSsearch As atcData.atcTimeseries In pModifiedData
                Select Case lTSsearch.Attributes.GetValue("Constituent", "")
                    Case aWDMConstituentName : Return lTSsearch
                End Select
            Next
        End If
        ' Search for this data in original data source
        For Each lTSsearch As atcData.atcTimeseries In aWDM.DataSets
            Select Case lTSsearch.Attributes.GetValue("Constituent", "")
                Case aWDMConstituentName : Return lTSsearch
            End Select
        Next
        Logger.Dbg("No " & aWDMConstituentName & " available in " & aWDM.Specification)
        Return Nothing
    End Function

    'Private Function GetWDMTimeseriesGroup(ByVal aSource As atcDataSource, ByVal aWDMConstituentName As String) As atcDataGroup
    '    'This version is to get the widest duration of the all the TSs
    '    Dim lTSGroup As atcDataGroup = aSource.DataSets.FindData("Constituent", aWDMConstituentName)
    '    Return lTSGroup
    '    ' Search for this data in original data source
    '    Dim lSJD As Double = Double.MaxValue
    '    Dim lEJD As Double = Double.MinValue

    '    If lTSGroup.Count = 0 Then
    '        Logger.Dbg("No " & aWDMConstituentName & " available in " & aSource.Specification)
    '        Return Nothing
    '    End If

    '    For Each lts As atcTimeseries In lTSGroup
    '        Dim lthisSJD As Double = lts.Attributes.GetValue("SJDay", "")
    '        Dim lthisEJD As Double = lts.Attributes.GetValue("EJDay", "")
    '        If lthisSJD < lSJD Then lSJD = lthisSJD
    '        If lthisEJD > lEJD Then lEJD = lthisEJD
    '    Next

    '    Dim lTSGroupCommonDur As New atcTimeseriesGroup
    '    Dim ltsTmp As atcTimeseries = Nothing
    '    For Each lts As atcTimeseries In lTSGroup
    '        ltsTmp = SubsetByDate(lts, lSJD, lEJD, Nothing)
    '        lTSGroupCommonDur.Add(ltsTmp)
    '    Next
    '    lTSGroup.Clear()
    '    lTSGroup = Nothing
    '    Return lTSGroupCommonDur
    'End Function

    Private Function MetHeader(ByVal aTimeseries As atcData.atcTimeseries) As String
        With aTimeseries.Attributes
            Return .GetValue("Constituent", "") & " at " _
                 & .GetValue("Location", "") & ", " _
                 & .GetValue("STANAM", "") _
                 & " data from BASINS via D4EM/EDDT"
        End With
    End Function

    Private Function MetGroupHeader(ByVal aTSGroup As atcDataGroup) As String
        Dim lheader As New System.Text.StringBuilder
        lheader.AppendLine(aTSGroup(0).Attributes.GetFormattedValue("Constituent") & " Input File" & Space(10) & Date.Now & " SWAT2005""")

        Dim lati(aTSGroup.Count - 1) As String
        Dim llong(aTSGroup.Count - 1) As String
        Dim lelev(aTSGroup.Count - 1) As String

        Dim lfield As String = String.Empty
        Dim lval As Double = 0.0
        Dim lflagTemp As String = aTSGroup(0).Attributes.GetFormattedValue("Constituent")
        For i As Integer = 0 To aTSGroup.Count - 1

            If Double.TryParse(aTSGroup(i).Attributes.GetFormattedValue("Latitude"), lval) Then
                lfield = DoubleToString(lval, 5, "#.00", , , 5).ToString.PadLeft(5)
            Else
                lfield = "00.00"
            End If
            If lflagTemp = "ATEM" Then
                lati(i) = lfield & lfield
            Else
                lati(i) = lfield
            End If

            If Double.TryParse(aTSGroup(i).Attributes.GetFormattedValue("Longitude"), lval) Then
                lfield = DoubleToString(lval, 5, "#.0", , , 5).ToString.PadLeft(5)
            Else
                lfield = "00.00"
            End If
            If lflagTemp = "ATEM" Then
                llong(i) = lfield & lfield
            Else
                llong(i) = lfield
            End If

            If lflagTemp = "ATEM" Then
                lelev(i) = "  100  100"
            Else
                lelev(i) = "  100"
            End If
        Next

        lheader.AppendLine("Lati   " & String.Join(Nothing, lati))
        lheader.AppendLine("Long   " & String.Join(Nothing, llong))
        lheader.AppendLine("Elev   " & String.Join(Nothing, lelev))

        Return lheader.ToString
    End Function

    Private Function LatLonElev(ByVal aTimeseries As atcData.atcTimeseries) As String
        With aTimeseries.Attributes
            'Lat and Lon not processed by model
            Return ("LatDeg " & DoubleToString(CDbl(.GetValue("Latitude", "0")), 5) & vbCrLf _
                  & "LonDeg " & DoubleToString(CDbl(.GetValue("Longitude", "0")), 5) & vbCrLf _
                  & "Elev(m)" & Format(CInt(.GetValue("Elevation", "0")), "00000"))
            'TODO: populate station list with elevation, add to WDM attributes, convert to meters
        End With
    End Function

    Private Function f51(ByVal aValue As Double) As String
        If aValue < 0 Then
            Return Format(aValue, "#0.0").PadLeft(5)
        Else
            Return Format(aValue, "##0.0").PadLeft(5)
        End If
    End Function

    Private Function f83(ByVal aValue As Double) As String
        If aValue < 0 Then
            Return Format(aValue, "##0.000").PadLeft(8)
        Else
            Return Format(aValue, "###0.000").PadLeft(8)
        End If
    End Function

    Private Function YYYYddd(ByVal aDate As Date) As String
        Return aDate.Year & Format(aDate.DayOfYear, "000")
    End Function

    Private Function YYYYmmdd(ByVal aDate As Date) As String
        Return aDate.Year & Format(aDate.Month, "00") & Format(aDate.Day, "00")
    End Function

    Private Function EnsureComplete(ByVal aTimeseries As atcTimeseries, _
                                    ByVal aStartDate As Double, _
                                    ByVal aEndDate As Double, _
                                    ByVal aDataSource As atcDataSource) As atcTimeseries

        Dim lNewTimeseries As atcTimeseries
        Dim lReport As New Text.StringBuilder

        lReport.AppendLine("Complete date range from " & Date.FromOADate(aStartDate) & " to " & Date.FromOADate(aEndDate))

        Dim lOverlapTimeseries As atcTimeseries = SubsetByDate(aTimeseries, aStartDate, aEndDate, aDataSource)
        If lOverlapTimeseries.numValues > 0 Then
            If Math.Abs(lOverlapTimeseries.Dates.Value(0) - aStartDate) < JulianSecond AndAlso _
               Math.Abs(lOverlapTimeseries.Dates.Value(lOverlapTimeseries.numValues) - aEndDate) < JulianSecond Then
                'Requested time 
                lNewTimeseries = lOverlapTimeseries
            Else
                lNewTimeseries = NewTimeseries(aStartDate, aEndDate, atcTimeUnit.TUDay, 1, , GetNaN)
                'First copy overlapping values into new timeseries
                Dim lOverlapStart As Integer = FindDateAtOrAfter(lNewTimeseries.Dates.Values, lOverlapTimeseries.Dates.Value(1))
                System.Array.Copy(lOverlapTimeseries.Values, 1, lNewTimeseries.Values, lOverlapStart, lOverlapTimeseries.numValues)
            End If
        Else
            lNewTimeseries = NewTimeseries(aStartDate, aEndDate, atcTimeUnit.TUDay, 1, , GetNaN)
        End If

        'Fill in missing values from same date in another year of input data
        Dim lOldIndex As Integer = 1
        Dim lOldDateArray(5) As Integer
        Dim lNewDateArray(5) As Integer

        For lNewIndex As Integer = 1 To lNewTimeseries.numValues
            If Double.IsNaN(lNewTimeseries.Value(lNewIndex)) Then
                J2Date(lNewTimeseries.Dates.Value(lNewIndex), lNewDateArray)
                Dim lSearchOldIndex As Integer = lOldIndex
                While lSearchOldIndex <= aTimeseries.numValues
                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
                        If lOldDateArray(1) = lNewDateArray(1) AndAlso lOldDateArray(2) = lNewDateArray(2) Then
                            lReport.AppendLine("Missing value for " & lNewDateArray(0) & "/" & lNewDateArray(1) & "/" & lNewDateArray(2) & ", copied from " & lOldDateArray(0) & "/" & lOldDateArray(1) & "/" & lOldDateArray(2) & ", " & aTimeseries.Value(lSearchOldIndex))
                            GoTo FoundOldDate
                        End If
                    End If
                    lSearchOldIndex += 1
                End While
                lSearchOldIndex = 1
                While lSearchOldIndex < lOldIndex
                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
                        If lOldDateArray(1) = lNewDateArray(1) AndAlso lOldDateArray(2) = lNewDateArray(2) Then
                            GoTo FoundOldDate
                        End If
                    End If
                    lSearchOldIndex += 1
                End While

                lSearchOldIndex = lOldIndex
                Dim lNearestIndex As Integer = lOldIndex
                Dim lNearestDays As Integer = 500
                Dim lDays As Integer
                While lSearchOldIndex <= aTimeseries.numValues
                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
                        If lOldDateArray(1) = lNewDateArray(1) Then
                            lDays = Math.Abs(lOldDateArray(2) - lNewDateArray(2))
                            If lDays < lNearestDays Then lNearestIndex = lSearchOldIndex
                        End If
                    End If
                    lSearchOldIndex += 1
                End While
                lSearchOldIndex = 1
                While lSearchOldIndex < lOldIndex
                    If Not Double.IsNaN(aTimeseries.Value(lSearchOldIndex)) Then
                        J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
                        If lOldDateArray(1) = lNewDateArray(1) Then
                            lDays = Math.Abs(lOldDateArray(2) - lNewDateArray(2))
                            If lDays < lNearestDays Then lNearestIndex = lSearchOldIndex
                        End If
                    End If
                    lSearchOldIndex += 1
                End While

                lSearchOldIndex = lNearestIndex
                J2Date(aTimeseries.Dates.Value(lSearchOldIndex), lOldDateArray)
                lReport.AppendLine("Value not found for " & lNewDateArray(0) & "/" & lNewDateArray(1) & "/" & lNewDateArray(2) & " using value from " & lNearestDays & " days away: " & lOldDateArray(0) & "/" & lOldDateArray(1) & "/" & lOldDateArray(2) & ", " & aTimeseries.Value(lSearchOldIndex))
FoundOldDate:
                lNewTimeseries.Value(lNewIndex) = aTimeseries.Value(lSearchOldIndex)
                lNewTimeseries.ValueAttributes(lNewIndex).SetValue("Original Date", aTimeseries.Dates.Value(lSearchOldIndex))
            End If
        Next

        lNewTimeseries.Attributes.SetValue("Description", lReport.ToString)
        Return lNewTimeseries

    End Function

End Module
