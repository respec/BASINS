Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Partial Public Class atcTimeseriesSWAT
    Private Function OpenOutput(Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        Dim lScenarioName As String
        Dim lDelim As String = ";" 'Used only inside this routine to delimit fields to process
        Dim lFieldsToProcess As String = ""
        Dim lLocationsToProcess As String = ""
        Dim lSUBsToProcess As String = ""
        If aAttributes IsNot Nothing Then
            If aAttributes.ContainsAttribute("FieldName") Then lFieldsToProcess = lDelim & aAttributes.GetValue("FieldName", "") & lDelim
            If aAttributes.ContainsAttribute("LocationName") Then lLocationsToProcess = lDelim & aAttributes.GetValue("LocationName", "") & lDelim
            If aAttributes.ContainsAttribute("SUB") Then lSUBsToProcess = lDelim & aAttributes.GetValue("SUB", "") & lDelim
        End If
        Dim lKnowInterval As Boolean = False
        Dim lKnowYearBase As Boolean = False

        Dim lLocation As String
        Dim lFieldIndex As Integer
        Dim lFieldName As String

        Dim lLocations As New List(Of String)
        Dim lSubs As New List(Of String)
        Dim lFieldNums As New List(Of Integer)
        Dim lFieldUnits As New List(Of String)
        Dim lFieldConstituents As New List(Of String)

ReOpenTable:
        Using lTable As atcTable = OpenTableOutput()
            If lTable Is Nothing Then
                Logger.Dbg("Unable to open " & Specification)
                Return False
            End If
            Try
                lScenarioName = IO.Path.GetFileName(IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(Specification)))
            Catch
                lScenarioName = "Simulated"
            End Try

            With lTable
                'Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Me)
                'Dim lTSBuilder As atcData.atcTimeseriesBuilder
                'Dim lFirstLocation As String = ""

                Logger.Status("Reading " & Format((.NumFields - pBaseDataField + 1), "#,###") & " constituents from " & Specification, True)
                .CurrentRecord = 1
                Dim lMONvalue As Integer
                Dim lMONvaluePrev As Integer = -1
                If Not lKnowInterval AndAlso Not lKnowYearBase Then
                    Try
                        Dim lCIOfilename As String = IO.Path.Combine(IO.Path.GetDirectoryName(Specification), "file.cio")
                        If IO.File.Exists(lCIOfilename) Then
                            Dim lCIO() As String
                            lCIO = IO.File.ReadAllLines(lCIOfilename)
                            If Not lKnowYearBase Then
                                lKnowYearBase = Integer.TryParse(lCIO(8).Substring(12, 4), pYearBase)
                                Dim lSkipYears As Integer = 0
                                Integer.TryParse(lCIO(59).Substring(0, 20), lSkipYears)
                                pYearBase += lSkipYears
                            End If
                            If Not lKnowInterval Then
                                lKnowInterval = Integer.TryParse(lCIO(58).Substring(15, 1), pMONcontains)
                            End If
                        End If
                    Catch e As Exception
                        Logger.Dbg("Exception reading file.cio: " & e.Message)
                    End Try

                    If Not lKnowInterval Then
                        If Integer.TryParse(.Value(pBaseDataField - 1).Trim, lMONvalue) Then
                            If lMONvalue > 366 Then 'must be yearly
                                lKnowInterval = True
                                pMONcontains = 2
                                lKnowYearBase = True
                                pYearBase = lMONvalue
                            Else 'Could be daily or monthly, have to scan MON column to be sure

                            End If
                        End If
                    End If
                End If

                ' Store index and name of each field we will process in lFieldNums and lFieldNames
                For lFieldIndex = 0 To .NumFields
                    If lFieldIndex >= pBaseDataField Then
                        lFieldName = .FieldName(lFieldIndex).ToString.Replace("""", "").Trim
                        If lFieldsToProcess.Length = 0 OrElse lFieldsToProcess.Contains(lDelim & lFieldName.Replace("_", "/") & lDelim) Then
                            lFieldNums.Add(lFieldIndex)
                        End If
                    Else
                        lFieldName = lFieldIndex
                    End If
                    lFieldUnits.Add(SplitUnits(lFieldName))
                    lFieldConstituents.Add(lFieldName)
                Next

                'Find name of each location in first field of table, count total number of values in file, and figure out interval if we don't have it yet
                pNumValues = 0
                Do
                    If Not Integer.TryParse(.Value(pBaseDataField - 1).Trim, lMONvalue) Then
                        Exit Do
                    Else
                        If lMONvaluePrev <> lMONvalue Then pNumValues += 1
                        If pTableDelimited Then
                            lLocation = (.Value(1).ToString.Replace("""", "").PadLeft(4) & .Value(2).ToString.PadLeft(5)).Trim
                        Else
                            lLocation = .Value(1).Trim
                        End If

                        If lLocationsToProcess.Length = 0 OrElse lLocationsToProcess.Contains(lDelim & lLocation & lDelim) Then
                            If lSUBsToProcess.Length = 0 OrElse lSUBsToProcess.Contains(lDelim & .Value(pSubIdField).Trim & lDelim) Then
                                If Not lLocations.Contains(lLocation) Then
                                    lLocations.Add(lLocation)
                                    If pSaveSubwatershedId Then lSubs.Add(.Value(pSubIdField).Trim)
                                End If
                            End If
                        End If

                        If Not lKnowInterval Then 'Still need to figure out if it is daily or monthly
                            If lMONvalue > 366 Then 'must be monthly since we reached a year before going past 12 (detected annual earlier)
                                lKnowInterval = True
                                pMONcontains = 0
                                lKnowYearBase = True
                                pYearBase = lMONvalue
                            ElseIf lMONvalue > 12 Then 'must be daily
                                lKnowInterval = True
                                pMONcontains = 1
                            End If
                        End If
                        lMONvaluePrev = lMONvalue
                        .CurrentRecord += 1
                        If .EOF Then
                            Exit Do
                        End If
                    End If
                Loop

                Select Case pMONcontains
                    Case 0 : pTimeUnit = atcTimeUnit.TUMonth
                    Case 1 : pTimeUnit = atcTimeUnit.TUDay
                    Case 2 : pTimeUnit = atcTimeUnit.TUYear
                    Case Else
                        Logger.Msg("Unknown time units in SWAT Outupt " & Specification)
                End Select

                Logger.Dbg("Found " & Format(lFieldNums.Count, "#,###") & " constituents and " & Format(lLocations.Count, "#,###") & " locations," & vbCrLf _
                         & "total of " & Format(lFieldNums.Count * lLocations.Count, "#,###") & " data sets.")
                'If Logger.Msg("Found " & Format(lFieldNums.Count, "#,###") & " constituents and " & Format(lLocations.Count, "#,###") & " locations," & vbCrLf _
                '         & "total of " & Format(lFieldNums.Count * lLocations.Count, "#,###") & " data sets. Continue?", vbYesNo, Specification) = MsgBoxResult.Yes Then
                '    'GoTo ReOpenTable 're-open the table to start over, can't set .CurrentRecord back to one with streaming table
                'Else
                '    Logger.Progress("", 0, 0)
                '    Return False
                'End If
                .Clear()
            End With
        End Using

        pDates = New atcTimeseries(Nothing)

        Dim lDatasetsSoFar As Integer = 0
        Dim lTotalDatasets As Integer = lFieldNums.Count * lLocations.Count
        Dim lTS As atcData.atcTimeseries
        Dim lDataKeys As New ArrayList(lTotalDatasets - 1)
        Dim lDataSets(lTotalDatasets - 1) As atcDataSet

        Logger.Status("Reading " & Format(lTotalDatasets, "#,###") & " datasets from " & Specification, True)

        Me.Attributes.AddHistory("Read from " & Specification)
        Dim lAttHistory1 As atcDefinedValue = Me.Attributes.GetDefinedValue("History 1")

        Me.Attributes.SetValue("Scenario", lScenarioName)
        Dim lAttScenario As atcDefinedValue = Me.Attributes.GetDefinedValue("Scenario")

        Dim lDefLocation As atcAttributeDefinition = atcDataAttributes.GetDefinition("Location", True)
        Dim lDefSubId As atcAttributeDefinition = atcDataAttributes.GetDefinition("SubId", True)
        Dim lDefCropId As atcAttributeDefinition = atcDataAttributes.GetDefinition("CropId", True)
        Dim lDefHruId As atcAttributeDefinition = atcDataAttributes.GetDefinition("HruId", True)

        Dim lAttTimeStep As atcDefinedValue = Nothing
        Dim lAttTimeUnit As atcDefinedValue = Nothing
        Dim lAttInterval As atcDefinedValue = Nothing
        Dim lAttPoint As atcDefinedValue = Nothing

        Dim lAttSubId As atcDefinedValue = Nothing
        Dim lAttCropId As atcDefinedValue = Nothing
        Dim lAttHruId As atcDefinedValue = Nothing

        Dim lAttsUnits As New atcCollection
        Dim lAttsConstituent As New atcCollection
        Dim lAttsFieldIndex As New atcCollection

        Dim lDefUnits As atcAttributeDefinition = atcDataAttributes.GetDefinition("Units", True)
        Dim lDefConstituent As atcAttributeDefinition = atcDataAttributes.GetDefinition("Constituent", True)
        Dim lDefFieldIndex As atcAttributeDefinition = atcDataAttributes.GetDefinition("FieldIndex", True)

        For lLocationIndex As Integer = 0 To lLocations.Count - 1
            lLocation = lLocations(lLocationIndex)
            Dim lAttLocation As New atcDefinedValue(lDefLocation, lLocation)
            If pSaveSubwatershedId Then
                lAttSubId = New atcDefinedValue(lDefSubId, lSubs(lLocationIndex))
                lAttCropId = New atcDefinedValue(lDefLocation, lLocation.Substring(0, 4).Trim)
                lAttHruId = New atcDefinedValue(lDefLocation, lLocation.Substring(4).Trim)
            End If

            Try
                For Each lFieldIndex In lFieldNums
                    lTS = New atcTimeseries(Me)
                    lDataSets(lDatasetsSoFar) = lTS
                    lDatasetsSoFar += 1
                    lDataKeys.Add(lDatasetsSoFar)
                    With lTS.Attributes

                        .Add(lAttHistory1)
                        .Add(lAttScenario)
                        .Add(lAttLocation)

                        If lAttTimeStep Is Nothing Then
                            lTS.SetInterval(pTimeUnit, 1)
                            lAttTimeStep = .GetDefinedValue("Time Step")
                            lAttTimeUnit = .GetDefinedValue("Time Unit")
                            lAttInterval = .GetDefinedValue("Interval")
                            .SetValue("point", False)
                            lAttPoint = .GetDefinedValue("Point")
                        Else
                            .Add(lAttTimeStep)
                            .Add(lAttTimeUnit)
                            .Add(lAttInterval)
                            .Add(lAttPoint)
                        End If

                        If pSaveSubwatershedId Then
                            .Add(lAttSubId)
                            .Add(lAttCropId)
                            .Add(lAttHruId)
                        End If

                        Dim lAttFieldIndex As atcDefinedValue = lAttsFieldIndex.ItemByKey(lFieldIndex)
                        If lAttFieldIndex Is Nothing Then
                            lAttFieldIndex = New atcDefinedValue(lDefFieldIndex, lFieldIndex)
                            lAttsFieldIndex.Add(lFieldIndex, lAttFieldIndex)
                        End If
                        .Add(lAttFieldIndex)

                        Dim lAttUnits As atcDefinedValue = lAttsUnits.ItemByKey(lFieldUnits(lFieldIndex))
                        If lAttUnits Is Nothing Then
                            lAttUnits = New atcDefinedValue(lDefUnits, lFieldUnits(lFieldIndex))
                            lAttsUnits.Add(lFieldUnits(lFieldIndex), lAttUnits)
                        End If
                        .Add(lAttUnits)

                        Dim lAttConstituent As atcDefinedValue = lAttsConstituent.ItemByKey(lFieldConstituents(lFieldIndex))
                        If lAttConstituent Is Nothing Then
                            lAttConstituent = New atcDefinedValue(lDefConstituent, lFieldConstituents(lFieldIndex))
                            lAttsConstituent.Add(lFieldConstituents(lFieldIndex), lAttConstituent)
                        End If
                        .Add(lAttConstituent)

                        .SetValue("ID", lDatasetsSoFar)
                    End With
                    lTS.ValuesNeedToBeRead = True
                    lTS.Dates = pDates
                Next
            Catch ex As Exception
                Logger.Dbg("Stopping opening SWAT output: " & ex.Message)
                Exit For
            End Try
NextRecord:
            Logger.Progress(lDatasetsSoFar, lTotalDatasets)
        Next
        GC.Collect()
        GC.WaitForPendingFinalizers()
        DataSets.AddRange(lDataKeys, lDataSets)
        Logger.Dbg("Created " & Format(DataSets.Count, "#,##0") & " timeseries") ' from first " & Format(.CurrentRecord, "#,##0") & " records")
        Logger.Progress("", 0, 0)
        Return True
    End Function

    Private Function OpenTableOutput() As atcTable
        Dim lTable As atcTable
        Dim lTableStreaming As atcTableFixedStreaming = Nothing
        If IO.Path.GetFileNameWithoutExtension(Specification) = "tab" Then
            lTable = New atcTableDelimited
            pTableDelimited = True
            lTable.NumHeaderRows = 0
            CType(lTable, atcTableDelimited).Delimiter = vbTab
            pBaseDataField = 6
            pSubIdField = 3
        Else
            lTableStreaming = New atcTableFixedStreaming
            lTable = lTableStreaming
            pTableDelimited = False
            lTable.NumHeaderRows = 9
            pBaseDataField = 4
            pSubIdField = 2
        End If
        With lTable
            If .OpenFile(Specification) Then
                Dim lConstituentHeader As String = ""
                If Not pTableDelimited Then
                    lConstituentHeader = CType(lTable, atcTableFixedStreaming).Header(9)
                End If
                Dim lField As Integer
                Dim lLastField As Integer
                Dim lFieldStart As Integer = 1
                Select Case IO.Path.GetExtension(Specification).ToLower
                    Case ".rch"
                        lLastField = 3 + (lConstituentHeader.Length - 25) / 12
                        .NumFields = lLastField
                        For lField = 1 To lLastField
                            Select Case lField
                                Case 1 : .FieldLength(lField) = 10
                                Case 2 : .FieldLength(lField) = 9
                                Case 3 : .FieldLength(lField) = 6
                                Case Else : .FieldLength(lField) = 12
                            End Select
                            .FieldName(lField) = Mid(lConstituentHeader, lFieldStart, .FieldLength(lField)).Trim
                            If lTableStreaming IsNot Nothing Then lTableStreaming.FieldStart(lField) = lFieldStart
                            lFieldStart += .FieldLength(lField)
                        Next
                    Case ".sub"
                        lLastField = 3 + (lConstituentHeader.Length - 26) / 10
                        .NumFields = lLastField
                        For lField = 1 To lLastField
                            Select Case lField
                                Case 1 : .FieldLength(lField) = 10
                                Case 2 : .FieldLength(lField) = 9
                                Case 3 : .FieldLength(lField) = 5
                                Case 23 : .FieldLength(lField) = 12
                                Case Else : .FieldLength(lField) = 10
                            End Select
                            .FieldName(lField) = Mid(lConstituentHeader, lFieldStart, .FieldLength(lField)).Trim
                            If lTableStreaming IsNot Nothing Then lTableStreaming.FieldStart(lField) = lFieldStart
                            lFieldStart += .FieldLength(lField)
                        Next
                    Case ".hru", ".hrux"
                        'Default SWAT header is off by one character, we insert a space after first SURQ
                        lConstituentHeader = "LULC HRU      GIS  SUB  MGT  MON   AREAkm2  PRECIPmm SNOFALLmm SNOMELTmm     IRRmm     PETmm      ETmm SW_INITmm  SW_ENDmm    PERCmm GW_RCHGmm DA_RCHGmm   REVAPmm  SA_IRRmm  DA_IRRmm   SA_STmm   DA_STmm SURQ_GENmmSURQ_CNTmm   TLOSSmm    LATQmm    GW_Qmm    WYLDmm   DAILYCN TMP_AVdgC TMP_MXdgC TMP_MNdgCSOL_TMPdgCSOLARMJ/m2  SYLDt/ha  USLEt/haN_APPkg/haP_APPkg/haNAUTOkg/haPAUTOkg/ha NGRZkg/ha PGRZkg/haNCFRTkg/haPCFRTkg/haNRAINkg/ha NFIXkg/ha F-MNkg/ha A-MNkg/ha A-SNkg/ha F-MPkg/haAO-LPkg/ha L-APkg/ha A-SPkg/ha DNITkg/ha  NUPkg/ha  PUPkg/ha ORGNkg/ha ORGPkg/ha SEDPkg/haNSURQkg/haNLATQkg/ha NO3Lkg/haNO3GWkg/ha SOLPkg/ha P_GWkg/ha    W_STRS  TMP_STRS    N_STRS    P_STRS  BIOMt/ha       LAI   YLDt/ha   BACTPct  BACTLPct"
                        pSaveSubwatershedId = True
                        If Not pTableDelimited Then
                            'First look at first record as all one field to find some field widths
                            .NumFields = 1
                            If lTableStreaming IsNot Nothing Then lTableStreaming.FieldStart(1) = lFieldStart
                            .FieldLength(1) = 1000 ' This is the safety net to get all of the one line in
                            .CurrentRecord = 1
                            Dim lValue As String = .Value(1)
                            Dim lFirstFieldWidth As Integer = lValue.Substring(0, 10).TrimEnd.Length

                            lLastField = 3 + (lValue.Length - lFirstFieldWidth - 24) / 10
                            .NumFields = lLastField
                            For lField = 1 To lLastField
                                Select Case lField
                                    Case 1 : .FieldLength(lField) = lFirstFieldWidth 'LULC+HRU
                                    Case 2 : .FieldLength(lField) = 5 'SUB
                                    Case 3 : .FieldLength(lField) = 5 'MON (contains day, month, year, and/or total years)
                                    Case 71, 72 'Sometimes the last two fields are in scientific notation and are 11 not 10 wide
                                        'TODO: is this test sufficient? can other fields ever be 11 wide?
                                        If SafeSubstring(lValue, lFieldStart, 10).Contains("E") Then
                                            .FieldLength(lField) = 11
                                        Else
                                            .FieldLength(lField) = 10
                                        End If
                                    Case Else : .FieldLength(lField) = 10
                                End Select
                                .FieldName(lField) = SafeSubstring(lConstituentHeader, lFieldStart - 1, .FieldLength(lField)).Trim
                                If lTableStreaming IsNot Nothing Then lTableStreaming.FieldStart(lField) = lFieldStart
                                Select Case lField
                                    Case 1 : lFieldStart += 9 + lFirstFieldWidth 'skip GIS to SUB
                                    Case 2 : lFieldStart += 10                   'skip MGT to MON
                                    Case Else : lFieldStart += .FieldLength(lField)
                                End Select
                            Next
                        End If
                    Case Else
                        Throw New ApplicationException("Unknown file extension for " & Specification)
                End Select
                If pTableDelimited Then
                    pRecordLength = .CurrentRecordAsDelimitedString().Length
                Else
                    pRecordLength = lTableStreaming.FieldStart(.NumFields) + .FieldLength(.NumFields) + 1
                End If
                Return lTable
            Else
                lTable = Nothing
                Return Nothing
            End If
        End With
    End Function

    Private Sub ReadDataOutput(ByVal aReadMe As atcDataSet)
        Dim lReadTS As atcTimeseries
        Dim lReadThese As New Generic.List(Of atcTimeseries)
        Dim lUniqueLocations As New Generic.List(Of String)
        Dim lReadLocation As New Generic.List(Of Integer)
        Dim lReadField As New Generic.List(Of Integer)
        Dim lReadValues As New Generic.List(Of Double()) 'array of double data values for each timeseries

        'Reading all datasets at once is much faster than one at a time, but all might be too large
        If Me.DataSets.Count * pNumValues < 100000000 Then 'Read them all if they will fit in ~1G
            For Each lReadTS In Me.DataSets
                If lReadTS.Serial = aReadMe.Serial OrElse lReadTS.ValuesNeedToBeRead Then
                    AddTsToList(lReadTS, lReadThese, lReadLocation, lUniqueLocations, lReadField, lReadValues)
                End If
            Next
        Else
            AddTsToList(aReadMe, lReadThese, lReadLocation, lUniqueLocations, lReadField, lReadValues)
        End If
        Dim lFinalReadingIndex As Integer = lReadThese.Count - 1

        If lFinalReadingIndex < 0 Then
            Logger.Dbg("No datasets to read")
        Else
            Dim lField As Integer = lReadField(0)
            Dim lTable As atcTable = OpenTableOutput()
            If lTable Is Nothing Then
                Logger.Dbg("Unable to open " & Specification)
            Else
                Dim lLocation As String
                Dim lUniqueLocationIndex As Integer
                Dim lLocationIndex As Integer
                Dim lDate As Double
                Dim lPrevDate As Double = 0
                Dim lYear As Integer = pYearBase
                Dim lYearReading As Integer = 0
                Dim lMonReading As Integer = 0
                Dim lDayReading As Integer = 0
                Dim lMONvalue As Integer

                Dim lVd() As Double = lReadValues(0)
                Dim lJd(-1) As Double 'array of julian dates
                Dim lValueIndex As Integer = 0

                Dim lNeedDates As Boolean
                If pDates.numValues = 0 Then
                    lNeedDates = True
                    ReDim lJd(pNumValues)
                    lJd(0) = pNaN
                Else
                    lNeedDates = False
                End If

                Debug.Print("Start ReadData Now @: " & Date.Now())
                With lTable
                    Do
                        If Not Integer.TryParse(.Value(pBaseDataField - 1).Trim, lMONvalue) Then
                            Exit Do 'got to end of run summary, value is number of years as a decimal or we have reached blank line after end
                        Else
                            Try
                                Select Case pMONcontains
                                    Case 0 'Monthly
                                        If lMONvalue < 13 Then
                                            If lMONvalue <> lMonReading Then
                                                lMonReading = lMONvalue
                                                If lMonReading = 1 AndAlso lDate > 0 Then
                                                    lYear += 1
                                                End If
                                            End If
                                            lDate = atcUtility.Jday(lYear, lMONvalue, daymon(lYear, lMONvalue), 24, 0, 0)
                                            If lNeedDates AndAlso lValueIndex = 0 Then lJd(0) = atcUtility.Jday(lYear, lMONvalue, 1, 0, 0, 0)
                                        Else 'Skip yearly lines in monthly output
                                            GoTo NextRecord
                                        End If
                                    Case 1 'Daily
                                        If lDate = 0 Then
                                            lDayReading = lMONvalue
                                            lDate = atcUtility.Jday(pYearBase, 1, lMONvalue, 24, 0, 0)
                                            If lNeedDates Then lJd(0) = atcUtility.Jday(pYearBase, 1, 1, 0, 0, 0)
                                        ElseIf lMONvalue <> lDayReading Then
                                            lDate += 1
                                            lDayReading = lMONvalue
                                            If lDayReading = 1 Then
                                                lYear += 1
                                            End If
                                        End If
                                    Case 2 'Yearly
                                        lYear = lMONvalue
                                        lDate = atcUtility.Jday(lYear, 12, 31, 24, 0, 0)
                                        If lNeedDates AndAlso lValueIndex = 0 Then lJd(0) = atcUtility.Jday(lYear - 1, 12, 31, 24, 0, 0)
                                End Select

                                If lDate > lPrevDate Then
                                    lValueIndex += 1
                                    If lValueIndex > pNumValues Then
                                        Logger.Dbg("Increasing size of value array to " & lValueIndex)
                                        If lNeedDates Then ReDim Preserve lJd(lValueIndex + 1)
                                        For lIndex As Integer = 0 To lFinalReadingIndex
                                            lVd = lReadValues(lIndex)
                                            ReDim Preserve lVd(lValueIndex)
                                            lVd(lValueIndex) = pNaN
                                        Next
                                    End If
                                    If lYear <> lYearReading Then
                                        If pMONcontains = 1 Then Logger.Status("Reading year " & lYear, True)
                                        Logger.Flush()
                                        lYearReading = lYear
                                        If pYearBase = 0 Then pYearBase = lYear
                                    End If

                                    lPrevDate = lDate
                                    If lNeedDates Then lJd(lValueIndex) = lDate
                                End If

                                If pTableDelimited Then
                                    lLocation = (.Value(1).ToString.Replace("""", "").PadLeft(4) & .Value(2).ToString.PadLeft(5)).Trim
                                Else
                                    lLocation = .Value(1).Trim
                                End If
                                lUniqueLocationIndex = lUniqueLocations.IndexOf(lLocation)

                                'The original loop for reading value
                                For lLocationIndex = 0 To lFinalReadingIndex
                                    If lReadLocation(lLocationIndex) = lUniqueLocationIndex Then
                                        If lFinalReadingIndex > 0 Then
                                            lField = lReadField(lLocationIndex)
                                            lVd = lReadValues(lLocationIndex)
                                        End If
                                        If Not Double.TryParse(.Value(lField).Trim, lVd(lValueIndex)) Then
                                            lVd(lValueIndex) = pNaN
                                        End If
                                        If lFinalReadingIndex > 0 Then lReadValues(lLocationIndex) = lVd
                                    End If
                                Next

                                'Dim lfirstInd As Integer = lReadLocation.IndexOf(lLocation)
                                ''Dim llastInd As Integer = lReadLocation.LastIndexOf(lLocation)
                                ''For lLocationIndex = lfirstInd To llastInd
                                'While lfirstInd >= 0
                                '    If lFinalReadingIndex > 0 Then
                                '        If Not Double.TryParse(.Value(lReadField(lLocationIndex)).Trim, lReadValues(lLocationIndex)(lValueIndex)) Then
                                '            lReadValues(lLocationIndex)(lValueIndex) = pNaN
                                '        End If
                                '    Else
                                '        If Not Double.TryParse(.Value(lField).Trim, lVd(lValueIndex)) Then
                                '            lVd(lValueIndex) = pNaN
                                '        End If
                                '    End If
                                '    lfirstInd = lReadLocation.IndexOf(lLocation, lfirstInd + 1)
                                'End While

                                'Working version III
                                'Dim lfirstInd As Integer = lReadLocation.IndexOf(lLocation)
                                'Dim llastInd As Integer = lReadLocation.LastIndexOf(lLocation)
                                'For lLocationIndex = lfirstInd To llastInd
                                '    If lReadLocation(lLocationIndex) = lLocation Then
                                '        If lFinalReadingIndex > 0 Then
                                '            If Not Double.TryParse(.Value(lReadField(lLocationIndex)).Trim, lReadValues(lLocationIndex)(lValueIndex)) Then
                                '                lReadValues(lLocationIndex)(lValueIndex) = pNaN
                                '            End If
                                '        Else
                                '            If Not Double.TryParse(.Value(lField).Trim, lVd(lValueIndex)) Then
                                '                lVd(lValueIndex) = pNaN
                                '            End If
                                '        End If
                                '    End If
                                'Next

                                ''Working version I
                                'Dim lfirstInd As Integer = lReadLocation.IndexOf(lLocation)
                                'For lLocationIndex = lfirstInd To lFinalReadingIndex
                                '    If lReadLocation(lLocationIndex) = lLocation Then
                                '        If lFinalReadingIndex > 0 Then
                                '            If Not Double.TryParse(.Value(lReadField(lLocationIndex)).Trim, lReadValues(lLocationIndex)(lValueIndex)) Then
                                '                lReadValues(lLocationIndex)(lValueIndex) = pNaN
                                '            End If
                                '        Else
                                '            If Not Double.TryParse(.Value(lField).Trim, lVd(lValueIndex)) Then
                                '                lVd(lValueIndex) = pNaN
                                '            End If
                                '        End If
                                '    Else
                                '        'Exit For
                                '    End If
                                'Next

                                'Working verion II
                                'Dim lfirstInd As Integer = lReadLocation.IndexOf(lLocation)
                                'Dim llastInd As Integer = lReadLocation.LastIndexOf(lLocation)
                                'For lLocationIndex = lfirstInd To llastInd
                                '    If lReadLocation(lLocationIndex) = lLocation Then
                                '        If lFinalReadingIndex > 0 Then
                                '            lField = lReadField(lLocationIndex)
                                '            lVd = lReadValues(lLocationIndex)
                                '        End If
                                '        If Not Double.TryParse(.Value(lField).Trim, lVd(lValueIndex)) Then
                                '            lVd(lValueIndex) = pNaN
                                '        End If
                                '        If lFinalReadingIndex > 0 Then lReadValues(lLocationIndex) = lVd
                                '    End If
                                'Next
NextRecord:
                                .CurrentRecord += 1
                                If .EOF Then
                                    Logger.Dbg("Reached EOF at record " & .CurrentRecord)
                                    Exit Do
                                End If
                            Catch ex As FormatException
                                Logger.Dbg("FormatException " & .CurrentRecord & ":" & lField & ":" & .Value(lField))
                            Catch ex As Exception
                                Logger.Dbg("Stopping reading SWAT output at record " & .CurrentRecord & ": " & ex.Message)
                                Exit Do
                            End Try
                        End If
                    Loop
                    'Logger.Dbg("Read " & Format(lNumTS, "#,##0") & " timeseries From " & Format(.CurrentRecord, "#,##0") & " Records")
                End With
                Debug.Print("End ReadData middle Now @: " & Date.Now())

                If lNeedDates Then
                    If lValueIndex < pNumValues Then
                        Logger.Dbg("Decreasing size of date array to " & lValueIndex)
                        ReDim Preserve lJd(lValueIndex)
                    End If
                    pDates.Values = lJd
                End If

                For lIndex As Integer = 0 To lFinalReadingIndex
                    lReadTS = lReadThese(lIndex)
                    lVd = lReadValues(lIndex)
                    'For debugging only
                    'Debug.WriteLine(lIndex & " " & lReadTS.Attributes.GetValue("Constituent", "unk") & " " & lVd(1) & " " & lVd(2))
                    'If Double.IsNaN(lVd(1)) Then
                    '    Debug.WriteLine("")
                    'End If

                    If lValueIndex < pNumValues Then
                        ReDim Preserve lVd(lValueIndex)
                    End If

                    lReadTS.ValuesNeedToBeRead = False

                    lReadTS.Values = lVd
                    With lReadTS.Attributes
                        .SetValue("point", False)
                        '.SetValue("TSFILL", pNaN)
                        '.SetValue("MVal", pNaN)
                        '.SetValue("MAcc", pNaN)
                        'Do we need to do FillValues? It seems like we already always have them filled, it is faster to skip, and lets us share pDates
                        'Dim lFilledTS As atcTimeseries = FillValues(lReadTS, .GetValue("tu"), .GetValue("ts"), .GetValue("TSFILL"), .GetValue("MVal"), .GetValue("MAcc"), Me)
                        'lReadTS.Dates.Values = lFilledTS.Dates.Values
                        'lReadTS.Values = lFilledTS.Values
                    End With
                    Logger.Progress(lIndex, lFinalReadingIndex)
                Next
                pNumValues = lValueIndex
                Logger.Progress("", 0, 0)
            End If
        End If
        lReadThese.Clear()
        lReadLocation.Clear()
        lReadField.Clear()
        lReadValues.Clear()
        Debug.Print("ReadDataOutput Complete @" & Date.Now())
    End Sub

    Private Function SplitUnits(ByRef aConstituent As String) As String
        Dim lUnitsStart As Integer = 0
        For Each lChar As Char In aConstituent.ToCharArray
            If Char.IsLower(lChar) Then Exit For
            lUnitsStart += 1
        Next
        SplitUnits = aConstituent.Substring(lUnitsStart).Trim
        aConstituent = aConstituent.Substring(0, lUnitsStart).Trim
    End Function
End Class
