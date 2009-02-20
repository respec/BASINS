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
                            CType(lTable, atcTableFixedStreaming).FieldStart(lField) = lFieldStart
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
                            CType(lTable, atcTableFixedStreaming).FieldStart(lField) = lFieldStart
                            lFieldStart += .FieldLength(lField)
                        Next
                    Case ".hru", ".hrux"
                        pSaveSubwatershedId = True
                        If Not pTableDelimited Then
                            'First look at first record as all one field to find some field widths
                            .NumFields = 1
                            lTableStreaming.FieldStart(1) = lFieldStart
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
                                lTableStreaming.FieldStart(lField) = lFieldStart
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
            Dim lDelim As String = ";" 'Used only inside this routine to delimit fields to process
            Dim lFieldsToProcess As String = ""
            If aAttributes IsNot Nothing Then
                lFieldsToProcess = lDelim & aAttributes.GetValue("FieldName", "") & lDelim
            End If
            Dim lTable As atcTable = OpenTable()
            If lTable Is Nothing Then
                Logger.Dbg("Unable to open " & Specification)
                Return False
            Else
                With lTable
                    'Dim lTSBuilders As New atcData.atcTimeseriesGroupBuilder(Me)
                    'Dim lTSBuilder As atcData.atcTimeseriesBuilder
                    Dim lTS As atcData.atcTimeseries
                    Dim lFirstLocation As String = ""

                    Dim lField As Integer
                    Dim lLocation As String
                    Dim lKey As String
                    Dim lDate As Double
                    Dim lYear As Integer

                    Logger.Status("Reading records for " & Format((.NumFields - pBaseDataField + 1), "#,###") & " constituents from " & Specification, True)
                    .CurrentRecord = 1
                    Dim lYearReading As Integer = 0
                    Dim lMonReading As Integer = 0
                    Dim lDayReading As Integer = 0
                    Dim lMONvalue As Integer
                    Dim lCIOfilename As String = IO.Path.Combine(IO.Path.GetDirectoryName(Specification), "file.cio")
                    If IO.File.Exists(lCIOfilename) Then
                        Dim lCIO() As String
                        lCIO = IO.File.ReadAllLines(lCIOfilename)
                        Integer.TryParse(lCIO(8).Substring(12, 4), pYearBase)
                        Integer.TryParse(lCIO(58).Substring(15, 1), pMONcontains)
                        Dim lSkipYears As Integer = 0
                        Integer.TryParse(lCIO(59).Substring(0, 20), lSkipYears)
                        pYearBase += lSkipYears
                    End If
                    lYear = pYearBase
                    pDates = New atcTimeseries(Nothing)
                    Do
                        If pTableDelimited Then
                            lLocation = .Value(1).ToString.Replace("""", "").PadLeft(4) & .Value(2).ToString.PadLeft(5)
                        Else
                            lLocation = .Value(1)
                        End If

                        If lFirstLocation.Length = 0 Then
                            lFirstLocation = lLocation
                        ElseIf lLocation = lFirstLocation Then 'Found first location again, so we have seen everything once
                            Exit Do
                        End If

                        Try
                            If Integer.TryParse(.Value(pBaseDataField - 1).Trim, lMONvalue) Then
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
                                        Else
                                            GoTo NextRecord
                                        End If
                                    Case 1 'Daily
                                        If lDate = 0 Then
                                            lDate = atcUtility.Jday(pYearBase, 1, 1, 24, 0, 0)
                                            lDayReading = lMONvalue
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
                                End Select
                                If lYear <> lYearReading Then
                                    'Logger.Status("Reading year " & lYear, True)
                                    'Logger.Flush()
                                    lYearReading = lYear
                                    If pYearBase = 0 Then pYearBase = lYear
                                End If
                                'Debug.Print(lYear & " " & DumpDate(lDate) & " at " & lLocation)
                                For lField = pBaseDataField To .NumFields
                                    Dim lFieldName As String = .FieldName(lField).ToString.Replace("""", "")
                                    If lFieldsToProcess.Length = 0 OrElse lFieldsToProcess.Contains(lDelim & lFieldName.Replace("_", "/") & lDelim) Then
                                        lTS = New atcTimeseries(Me)
                                        With lTS.Attributes
                                            Select Case pMONcontains
                                                Case 0 : .SetValue("tu", atcTimeUnit.TUMonth)
                                                Case 1 : .SetValue("tu", atcTimeUnit.TUDay)
                                                Case 2 : .SetValue("tu", atcTimeUnit.TUYear)
                                                Case Else
                                                    Logger.Msg("Unknown time units in SWAT Outupt " & Specification)
                                            End Select

                                            .Add("FieldIndex", lField)
                                            If pSaveSubwatershedId Then
                                                .Add("SubId", lTable.Value(pSubIdField).Trim)
                                                .Add("CropId", lLocation.Substring(0, 4).Trim)
                                                .Add("HruId", lLocation.Substring(4).Trim)
                                            End If

                                            .SetValue("Scenario", "Simulate") 'TODO: get a name for the scenario
                                            .SetValue("Units", SplitUnits(lFieldName).Trim)
                                            .SetValue("Constituent", lFieldName.Trim)
                                            .SetValue("Location", lLocation.Trim)
                                            .SetValue("ID", Me.DataSets.Count + 1)
                                            .AddHistory("Read from " & Specification)

                                            .SetValue("point", False)
                                            .SetValue("ts", 1)
                                            .SetValue("TSFILL", pNaN)
                                            .SetValue("MVal", pNaN)
                                            .SetValue("MAcc", pNaN)

                                        End With
                                        lTS.ValuesNeedToBeRead = True
                                        lTS.Dates = pDates
                                        DataSets.Add(lTS)
                                    End If
                                Next
                            Else 'got to end of run summary, value is number of years as a decimal or we have reached blank line after end
                                Exit Do
                            End If
                        Catch ex As FormatException
                            Logger.Dbg("FormatException " & .CurrentRecord & ":" & lField & ":" & .Value(lField))
                        Catch ex As Exception
                            Logger.Dbg("Stopping reading SWAT output: " & ex.Message)
                            Exit Do
                        End Try
NextRecord:
                        .CurrentRecord += 1
                    Loop
                    Try
                        pNumValues = (FileLen(Specification) - .Header.Length) / (pRecordLength * (.CurrentRecord - 1)) - 1
                    Catch ex As Exception
                        Logger.Dbg("Unable to determine number of values in " & Specification & vbCrLf & " FileLen=" & FileLen(Specification) & ", .HeaderLength=" & .Header.Length & ", RecordLength=" & pRecordLength & ", CurrentRecord=" & .CurrentRecord)
                    End Try
                    Logger.Dbg("Created " & Format(DataSets.Count, "#,##0") & " timeseries from first " & Format(.CurrentRecord, "#,##0") & " records")
                    Logger.Progress("", 0, 0)
                    Return True
                End With
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
                'Do we need to do FillValues? It seems like we already always have them filled, it is faster to skip, and lets us share pDates
                'With lReadTS.Attributes
                'Dim lFilledTS As atcTimeseries = FillValues(lReadTS, .GetValue("tu"), .GetValue("ts"), .GetValue("TSFILL"), .GetValue("MVal"), .GetValue("MAcc"), Me)
                'lReadTS.Dates.Values = lFilledTS.Dates.Values
                'lReadTS.Values = lFilledTS.Values
                'End With
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
        SplitUnits = aConstituent.Substring(lUnitsStart)
        aConstituent = aConstituent.Substring(0, lUnitsStart)
    End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class