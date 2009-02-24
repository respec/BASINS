Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcTimeseriesSWAT
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2008 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private Shared pFilter As String = "SWAT Output Files (output.rch, .sub, .hru)|output.rch;output.sub;output.hru;output.hrux;"
    Private Shared pNaN As Double = GetNaN()
    Private pTableDelimited As Boolean = False
    Private pBaseDataField As Integer
    Private pSubIdField As Integer
    Private pMONcontains As Integer = 0 'IPRINT from file.cio, 0=Monthly, 1=Daily, 2=Yearly
    Private pTimeUnit As atcTimeUnit = atcTimeUnit.TUMonth
    Private pYearBase As Integer = 1900
    Private pNumValues As Integer = 0
    Private pSaveSubwatershedId As Boolean = False
    Private pHeaderSize As Integer = 0
    Private pRecordLength As Integer = 0
    Private pDates As atcTimeseries = Nothing 'Can share dates since they will be the same for all ts in a file

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "SWAT Output Text"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::" & Description
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True 'yes, this class can open files
        End Get
    End Property

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return False 'no saving yet, but could implement if needed 
        End Get
    End Property

    Private Function OpenTable() As atcTable
        Dim lTable As atcTable
        Dim lTableStreaming As atcTableFixedStreaming = Nothing
        If IO.Path.GetFileNameWithoutExtension(Specification) = "tab" Then
            lTable = New atcTableDelimited
            pTableDelimited = True
        Else
            lTableStreaming = New atcTableFixedStreaming
            lTable = lTableStreaming
            pTableDelimited = False
        End If
        With lTable
            If pTableDelimited Then
                .NumHeaderRows = 0
                CType(lTable, atcTableDelimited).Delimiter = vbTab
                pBaseDataField = 6
                pSubIdField = 3
            Else
                .NumHeaderRows = 9
                pBaseDataField = 4
                pSubIdField = 2
            End If
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
                        pSaveSubwatershedId = True
                        If Not pTableDelimited Then
                            'First look at first record as all one field to find some field widths
                            .NumFields = 1
                            If lTableStreaming IsNot Nothing Then lTableStreaming.FieldStart(1) = lFieldStart
                            .FieldLength(1) = 1000
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
                    pRecordLength = lTableStreaming.FieldStart(.NumFields) + .FieldLength(.NumFields) + 2 'CR/LF = 2 chars                    
                End If
                Return lTable
            Else
                lTable = Nothing
                Return Nothing
            End If
        End With
    End Function

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aFileName, aAttributes) Then
            Dim lScenarioName As String
            Dim lDelim As String = ";" 'Used only inside this routine to delimit fields to process
            Dim lFieldsToProcess As String = ""
            Dim lLocationsToProcess As String = ""
            If aAttributes IsNot Nothing Then
                If aAttributes.ContainsAttribute("FieldName") Then lFieldsToProcess = lDelim & aAttributes.GetValue("FieldName", "") & lDelim
                If aAttributes.ContainsAttribute("LocationName") Then lLocationsToProcess = lDelim & aAttributes.GetValue("LocationName", "") & lDelim
            End If
            Dim lKnowInterval As Boolean = False
            Dim lKnowYearBase As Boolean = False

            Dim lLocation As String
            Dim lFieldIndex As Integer
            Dim lFieldName As String

            Dim lLocations As New List(Of String)
            Dim lFieldNums As New List(Of Integer)
            Dim lFieldNames As New List(Of String)
ReOpenTable:
            Dim lTable As atcTable = OpenTable()
            If lTable Is Nothing Then
                Logger.Dbg("Unable to open " & Specification)
                Return False
            Else
                Try
                    lScenarioName = IO.Path.GetFileName(IO.Path.GetDirectoryName(IO.Path.GetDirectoryName(Specification)))
                Catch
                    lScenarioName = "Simulated"
                End Try

                With lTable
                    'Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Me)
                    'Dim lTSBuilder As atcData.atcTimeseriesBuilder
                    Dim lFirstLocation As String = ""

                    Logger.Status("Reading " & Format((.NumFields - pBaseDataField + 1), "#,###") & " constituents from " & Specification, True)
                    .CurrentRecord = 1
                    Dim lYearReading As Integer = 0
                    Dim lMonReading As Integer = 0
                    Dim lDayReading As Integer = 0
                    Dim lMONvalue As Integer
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
                        lFieldNames.Add(lFieldName)
                    Next

                    'Find name of each location in first field of table
                    Do
                        If pTableDelimited Then
                            lLocation = .Value(1).ToString.Replace("""", "").PadLeft(4) & .Value(2).ToString.PadLeft(5)
                        Else
                            lLocation = .Value(1)
                        End If

                        lLocation = lLocation.Trim

                        If lFirstLocation.Length = 0 Then
                            lFirstLocation = lLocation
                        ElseIf lLocation = lFirstLocation Then 'Found first location again, so we have seen everything once
                            Exit Do
                        End If

                        If lLocationsToProcess.Length = 0 OrElse lLocationsToProcess.Contains(lDelim & lLocation & lDelim) Then
                            lLocations.Add(lLocation)
                        End If
                        .CurrentRecord += 1
                    Loop

                    Try
                        pNumValues = (FileLen(Specification) - .Header.Length) / (pRecordLength * (.CurrentRecord - 1)) - 1
                    Catch ex As Exception
                        Logger.Dbg("Unable to determine number of values in " & Specification & vbCrLf & " FileLen=" & FileLen(Specification) & ", .HeaderLength=" & .Header.Length & ", RecordLength=" & pRecordLength & ", CurrentRecord=" & .CurrentRecord)
                    End Try

                    If Not lKnowInterval Then 'Still need to figure out if it is daily or monthly
                        Integer.TryParse(.Value(pBaseDataField - 1).Trim, lMONvalue)
                        While lMONvalue <= 12
                            .CurrentRecord += lLocations.Count
                            If Integer.TryParse(.Value(pBaseDataField - 1).Trim, lMONvalue) Then
                            Else
                                Logger.Dbg("Could not parse integer in field " & (pBaseDataField - 1) & " = " & .Value(pBaseDataField - 1))
                                Exit While
                            End If
                        End While
                        If lMONvalue > 1000 Then 'must be monthly since we reached a year before going past 12
                            lKnowInterval = True
                            pMONcontains = 0
                            lKnowYearBase = True
                            pYearBase = lMONvalue
                        Else 'must be daily
                            lKnowInterval = True
                            pMONcontains = 1
                        End If
                    End If

                    Select Case pMONcontains
                        Case 0 : pTimeUnit = atcTimeUnit.TUMonth
                        Case 1 : pTimeUnit = atcTimeUnit.TUDay
                        Case 2 : pTimeUnit = atcTimeUnit.TUYear
                        Case Else
                            Logger.Msg("Unknown time units in SWAT Outupt " & Specification)
                    End Select

                    If Logger.Msg("Found " & Format(lFieldNums.Count, "#,###") & " constituents and " & Format(lLocations.Count, "#,###") & " locations," & vbCrLf _
                             & "total of " & Format(lFieldNums.Count * lLocations.Count, "#,###") & " data sets. Continue?", vbYesNo, Specification) = MsgBoxResult.Yes Then
                        'GoTo ReOpenTable 're-open the table to start over, can't set .CurrentRecord back to one with streaming table
                    Else
                        Logger.Progress("", 0, 0)
                        Return False
                    End If
                End With

                pDates = New atcTimeseries(Nothing)

                Dim lDatasetsSoFar As Integer = 0
                Dim lTotalDatasets As Integer = lFieldNums.Count * lLocations.Count
                Dim lTS As atcData.atcTimeseries
                Dim lDataKeys(lTotalDatasets - 1) As Integer
                Dim lDataSets(lTotalDatasets - 1) As atcDataSet

                Logger.Status("Reading " & Format(lTotalDatasets, "#,###") & " datasets from " & Specification, True)

                For Each lLocation In lLocations
                    Try
                        For Each lFieldIndex In lFieldNums
                            lFieldName = lFieldNames.Item(lFieldIndex).Clone
                            lTS = New atcTimeseries(Me)
                            lDataSets(lDatasetsSoFar) = lTS
                            lDataKeys(lDatasetsSoFar) = lDatasetsSoFar + 1
                            lDatasetsSoFar += 1
                            With lTS.Attributes
                                .SetValue("FieldIndex", lFieldIndex)
                                If pSaveSubwatershedId Then
                                    'TODO: we are not going through table now but need to get SubId: .SetValue("SubId", lTable.Value(pSubIdField).Trim)
                                    .SetValue("CropId", lLocation.Substring(0, 4).Trim)
                                    .SetValue("HruId", lLocation.Substring(4).Trim)
                                End If

                                .SetValue("Scenario", lScenarioName)
                                .SetValue("Units", SplitUnits(lFieldName))
                                .SetValue("Constituent", lFieldName)
                                .SetValue("Location", lLocation)
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
                DataSets.AddRange(lDataKeys, lDataSets)
                Logger.Dbg("Created " & Format(DataSets.Count, "#,##0") & " timeseries") ' from first " & Format(.CurrentRecord, "#,##0") & " records")
                Logger.Progress("", 0, 0)
                Return True
            End If
        End If
    End Function

    Public Overrides Sub ReadData(ByVal aReadMe As atcDataSet)
        Dim lField As Integer = aReadMe.Attributes.GetValue("FieldIndex", 0)
        If Not DataSets.Contains(aReadMe) Then
            Logger.Dbg("Dataset not from this source:" & aReadMe.ToString & vbCrLf & _
                       "Source:'" & Specification & "'")
        ElseIf lField < 1 Then
            Logger.Dbg("Dataset does not have a field index:" & aReadMe.ToString & vbCrLf & _
                       "Source:'" & Specification & "'")
        Else

            Dim lTable As atcTable = OpenTable()
            If lTable Is Nothing Then
                Logger.Dbg("Unable to open " & Specification)
            Else
                Dim lReadTS As atcTimeseries = aReadMe 'atcTimeseries type so we can access Values and Dates
                Logger.Status("Reading values for " & lReadTS.ToString)
                Dim lDataLocation As String = lReadTS.Attributes.GetValue("Location")
                Dim lLocation As String
                Dim lDate As Double
                Dim lYear As Integer = pYearBase
                Dim lYearReading As Integer = 0
                Dim lMonReading As Integer = 0
                Dim lDayReading As Integer = 0
                Dim lMONvalue As Integer

                Dim lValueIndex As Integer = 1

                Dim lVd(pNumValues) As Double 'array of double data values
                Dim lJd(-1) As Double 'array of julian dates

                Dim lNeedDates As Boolean
                If pDates.numValues = 0 Then
                    lNeedDates = True
                    ReDim lJd(pNumValues)
                    lJd(0) = pNaN
                Else
                    lNeedDates = False
                End If

                lVd(0) = pNaN

                With lTable
                    Do
                        If pTableDelimited Then
                            lLocation = .Value(1).ToString.Replace("""", "").PadLeft(4) & .Value(2).ToString.PadLeft(5)
                        Else
                            lLocation = .Value(1)
                        End If

                        Try
                            If lLocation = lDataLocation Then
                                If Integer.TryParse(.Value(pBaseDataField - 1).Trim, lMONvalue) Then
                                    If lValueIndex > pNumValues Then
                                        If lNeedDates Then ReDim Preserve lJd(lValueIndex)
                                        ReDim Preserve lVd(lValueIndex)
                                    End If
                                    Select Case pMONcontains
                                        Case 0 'Monthly
                                            If lMONvalue < 13 Then
                                                If lMONvalue <> lMonReading Then
                                                    lMonReading = lMONvalue
                                                    If lMonReading = 1 Then
                                                        lYear += 1
                                                    End If
                                                End If
                                                lDate = atcUtility.Jday(lYear, lMONvalue, daymon(lYear, lMONvalue), 24, 0, 0)
                                                If lNeedDates AndAlso lValueIndex = 1 Then lJd(0) = atcUtility.Jday(lYear, lMONvalue, 1, 0, 0, 0)
                                            Else 'Skip yearly lines in monthly output
                                                GoTo NextRecord
                                            End If
                                        Case 1 'Daily
                                            If lDate = 0 Then
                                                lDate = atcUtility.Jday(pYearBase, 1, 1, 24, 0, 0)
                                                lDayReading = lMONvalue
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
                                            If lNeedDates AndAlso lValueIndex = 1 Then lJd(0) = atcUtility.Jday(lYear - 1, 12, 31, 24, 0, 0)
                                    End Select
                                    If lYear <> lYearReading Then
                                        Logger.Status("Reading year " & lYear, True)
                                        Logger.Flush()
                                        lYearReading = lYear
                                        If pYearBase = 0 Then pYearBase = lYear
                                    End If
                                    'Debug.Print(lYear & " " & DumpDate(lDate) & " at " & lLocation)
                                    Dim lFieldName As String = .FieldName(lField).ToString.Replace("""", "")
                                    If lNeedDates Then lJd(lValueIndex) = lDate
                                    If Not Double.TryParse(.Value(lField).Trim, lVd(lValueIndex)) Then
                                        lVd(lValueIndex) = pNaN
                                    End If
                                    lValueIndex += 1
                                Else 'got to end of run summary, value is number of years as a decimal or we have reached blank line after end
                                    Exit Do
                                End If
                            End If
NextRecord:
                            .CurrentRecord += 1
                        Catch ex As FormatException
                            Logger.Dbg("FormatException " & .CurrentRecord & ":" & lField & ":" & .Value(lField))
                        Catch ex As Exception
                            Logger.Dbg("Stopping reading SWAT output at record " & .CurrentRecord & ": " & ex.Message)
                            Exit Do
                        End Try
                    Loop
                    'Logger.Dbg("Read " & Format(lNumTS, "#,##0") & " timeseries From " & Format(.CurrentRecord, "#,##0") & " Records")
                End With

                If lValueIndex <= pNumValues Then
                    If lNeedDates Then ReDim Preserve lJd(lValueIndex - 1)
                    ReDim Preserve lVd(lValueIndex - 1)
                End If

                lReadTS.ValuesNeedToBeRead = False
                If lNeedDates Then
                    pDates.Values = lJd
                End If
                lReadTS.Values = lVd
                With lReadTS.Attributes
                    .SetValue("point", False)
                    .SetValue("ts", 1)
                    .SetValue("tu", pTimeUnit)
                    .SetValue("TSFILL", pNaN)
                    .SetValue("MVal", pNaN)
                    .SetValue("MAcc", pNaN)
                    .AddHistory("Read from " & Specification)
                    'Do we need to do FillValues? It seems like we already always have them filled, it is faster to skip, and lets us share pDates
                    'Dim lFilledTS As atcTimeseries = FillValues(lReadTS, .GetValue("tu"), .GetValue("ts"), .GetValue("TSFILL"), .GetValue("MVal"), .GetValue("MAcc"), Me)
                    'lReadTS.Dates.Values = lFilledTS.Dates.Values
                    'lReadTS.Values = lFilledTS.Values
                End With
                Logger.Progress("", 0, 0)
            End If
        End If
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

    Public Sub New()
        Filter = pFilter
    End Sub
End Class