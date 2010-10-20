Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Collections
Imports System.IO

''' <summary>
''' Reads USGS rdb files containing daily values
''' </summary>
''' <remarks>
''' Would need to change pJulianInterval, ts and tu for non-daily values
''' Does not read provisional values into timeseries
''' </remarks>

Public Class atcTimeseriesRDB
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "USGS RDB Files (*.rdb, *.txt)|*.rdb;*.txt|All Files (*.*)|(*.*)"
    Private pErrorDescription As String
    Private pJulianInterval As Double = 1 'Add one day for daily values to record date at end of interval

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "USGS RDB File"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::USGS RDB"
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, _
                          Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , Filter, True, , 1)
        End If

        If Not IO.File.Exists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
        Else
            Specification = aFileName
            Try
                Dim lTimeStartOpen As Date = Now
                Logger.Dbg("OpenStartFor " & aFileName)

                Dim lInputStream As New FileStream(Specification, FileMode.Open, FileAccess.Read)
                Dim lInputBuffer As New BufferedStream(lInputStream)
                Dim lInputReader As New BinaryReader(lInputBuffer)

                Dim lWQData As Boolean = False
                Dim lMeasurementsData As Boolean = False
                Dim lIdaData As Boolean = False

                Dim lAttributes As New atcDataAttributes
                Dim lCurLine As String = NextLine(lInputReader).Substring(2)
                If lCurLine.IndexOf("qwdata") > -1 Then 'TODO: need better way - only true with BASINS download
                    lWQData = True
                    lAttributes.SetValue("URL", lCurLine)
                ElseIf lCurLine.IndexOf("measurements") > -1 Then
                    lMeasurementsData = True
                    lAttributes.SetValue("URL", lCurLine)
                ElseIf (IO.Path.GetExtension(Specification).ToLower = ".txt") AndAlso lCurLine.StartsWith("retrieved") Then
                    lIdaData = True
                Else
                    lAttributes.SetValue("URL", lCurLine)
                End If

                Dim lAttrName As String
                Dim lAttrValue As String
                While lInputReader.PeekChar = Asc("#")
                    lCurLine = NextLine(lInputReader)
                    If lCurLine.Length = 1 Then Exit While
                    If lCurLine.Length > 50 Then
                        lAttrName = lCurLine.Substring(2, 48).Trim
                        lAttrValue = lCurLine.Substring(50).Trim
                        If lAttrName.Length > 0 AndAlso lAttrName.Length < 30 Then
                            Select Case lAttrName 'translate NWIS attributes to WDM/BASINS names
                                Case "agency_cd" : lAttributes.SetValue("AGENCY", lAttrValue)
                                Case "station_nm" : lAttributes.SetValue("StaNam", lAttrValue)
                                Case "state_cd" : lAttributes.SetValue("STFIPS", lAttrValue)
                                Case "county_cd" : lAttributes.SetValue("CNTYFIPS", lAttrValue)
                                Case "huc_cd" : lAttributes.SetValue("HUCODE", lAttrValue)
                                Case "dec_lat_va" : lAttributes.SetValue("LATDEG", CDbl(lAttrValue))
                                Case "dec_long_va" : lAttributes.SetValue("LNGDEG", -Math.Abs(CDbl(lAttrValue)))
                                Case "alt_va" : lAttributes.SetValue("ELEV", lAttrValue)
                                Case "drain_area_va" : lAttributes.SetValue("DAREA", lAttrValue)
                                Case Else
                                    If lAttrName.Length > 0 AndAlso lAttrValue.Length > 0 Then
                                        lAttributes.SetValue(lAttrName, lAttrValue)
                                        'Logger.Dbg("Set " & lAttrName & " = " & lAttrValue)
                                    End If
                            End Select
                        End If
                    End If
                End While

                lAttributes.AddHistory("Read from " & Specification)

                If lWQData Then
                    ProcessWaterQualityValues(lInputReader, lAttributes)
                ElseIf lMeasurementsData Then
                    ProcessMeasurements(lInputReader, lAttributes)
                ElseIf lIdaData Then
                    Logger.Dbg("AboutToProcessIdaValues;Elapsed " & (Now - lTimeStartOpen).TotalSeconds)
                    ProcessIdaValues(lInputReader, lAttributes)
                Else
                    ProcessDailyValues(lInputReader, lAttributes)
                End If

                Return True
            Catch lException As Exception
                Logger.Dbg("Exception reading '" & aFileName & "': " & lException.Message & vbCrLf & lException.StackTrace)
                Return False
            End Try
        End If
    End Function

    Sub ProcessMeasurements(ByVal aInputReader As BinaryReader, ByVal aAttributes As atcDataAttributes)
        Dim lStationsRDB As New atcTableRDB
        With lStationsRDB
            If .OpenStream(aInputReader.BaseStream) Then
                Dim lDateField As Integer = .FieldNumber("measurement_dt")
                If lDateField = 0 Then Throw New Exception("Required field missing: measurement_dt")

                Dim lValueFieldNames() As String = {"channel_width_va", "xsec_area_va", "velocity_va", "discharge_1", "discharge_va", "gage_height_va", "shift_applied_va"}
                Dim lConstituentNames() As String = {"WIDTH", "XSECT", "VELOCITY", "DISCHARGE1", "DISCHARGE", "GAGE_HEIGHT", "SHIFT_APPLIED"}
                Dim lUnits() As String = {"ft", "square feet", "ft/sec", "cfs", "cfs", "ft", "ft"}
                Dim lLastValueField As Integer = lValueFieldNames.GetUpperBound(0)
                Dim lValueFieldNumber(lLastValueField) As Integer
                Dim lBuilders(lLastValueField) As atcTimeseriesBuilder
                Dim lValueFieldIndex As Integer

                For lValueFieldIndex = 0 To lLastValueField
                    lValueFieldNumber(lValueFieldIndex) = .FieldNumber(lValueFieldNames(lValueFieldIndex))
                    If lValueFieldNumber(lValueFieldIndex) = 0 Then
                        Logger.Dbg("Missing measurement field: " & lValueFieldNames(lValueFieldIndex))
                    Else
                        lBuilders(lValueFieldIndex) = New atcTimeseriesBuilder(Me)
                        With lBuilders(lValueFieldIndex).Attributes
                            .ChangeTo(aAttributes)
                            .SetValue("Constituent", lConstituentNames(lValueFieldIndex))
                            .SetValue("Units", lUnits(lValueFieldIndex))
                            .SetValue("Point", True)
                            .SetValue("Scenario", "OBSERVED")
                            .SetValue("Location", .GetValue("site_no"))
                            .SetValue("Description", "Measurements at " & .GetValue("station_nm"))
                            .SetValue("ID", lValueFieldIndex + 1)
                        End With
                    End If
                Next

                Dim lDateString As String
                Dim lDate As Date
                Dim lValueString As String
                Dim lValue As Double
                For lRecord As Integer = 1 To .NumRecords
                    .CurrentRecord = lRecord
                    lDateString = .Value(lDateField)
                    lDate = Date.Parse(lDateString)

                    For lValueFieldIndex = 0 To lLastValueField
                        If lValueFieldNumber(lValueFieldIndex) > 0 Then
                            lValueString = .Value(lValueFieldNumber(lValueFieldIndex))
                            If IsNumeric(lValueString) Then
                                lValue = Double.Parse(lValueString)
                                'ElseIf lxsec_area_Field >= 0 AndAlso lvelocity_Field >= 0 AndAlso IsNumeric(.Value(lxsec_area_Field)) AndAlso IsNumeric(.Value(lvelocity_Field)) Then
                                '    lValue = .Value(lxsec_area_Field) * .Value(lvelocity_Field)
                                '    Logger.Dbg("Computed flow for " & lDateString & " from xsec_area_va * velocity_va = " & DoubleToString(lValue))
                            Else
                                lValue = GetNaN()
                            End If
                            lBuilders(lValueFieldIndex).AddValue(lDate, lValue)
                            For lField As Integer = 1 To .NumFields
                                If Array.IndexOf(lValueFieldNumber, lField) < 0 Then 'Not a value field, add it as value attribute
                                    Select Case .FieldName(lField)
                                        Case "agency_cd", "site_no", "measurement_dt" 'don't need these as value attributes
                                        Case Else
                                            lBuilders(lValueFieldIndex).AddValueAttribute(.FieldName(lField), .Value(lField))
                                    End Select
                                End If
                            Next
                        End If
                    Next
                Next
                Dim lTs As atcTimeseries
                For lValueFieldIndex = 0 To lLastValueField
                    If lBuilders(lValueFieldIndex) IsNot Nothing AndAlso lBuilders(lValueFieldIndex).NumValues > 0 Then
                        lTs = lBuilders(lValueFieldIndex).CreateTimeseries
                        If lTs.Attributes.GetValue("Count", 0) > 0 Then DataSets.Add(lTs)
                    End If
                Next
            Else
                Throw New Exception("Unable to open")
            End If
        End With

    End Sub

    Sub ProcessWaterQualityValues(ByVal aInputReader As BinaryReader, ByVal aAttributes As atcDataAttributes)
        Dim lCurLine As String
        Dim lConstituentDescriptions As New atcCollection

        While aInputReader.PeekChar = Asc("#")
            lCurLine = NextLine(aInputReader)
            If lCurLine.IndexOf(" The following parameters are included:") > -1 Then
                Do
                    lCurLine = NextLine(aInputReader)
                    If lCurLine.Length <= 2 Then Exit Do
                    lConstituentDescriptions.Add(lCurLine.Substring(3, 5), lCurLine.Substring(12))
                Loop
                Logger.Dbg("ConstituentCount:" & lConstituentDescriptions.Count)
            End If
            'TODO: process more header stuff
        End While

        Dim lRawDataSets As New atcCollection
        Dim lData As atcTimeseriesBuilder = Nothing

        Dim lTable As New atcTableDelimited
        With lTable
            Dim lDateJ As Double
            Dim lDateField As Integer = -1
            Dim lTimeField As Integer = -1
            Dim lLocation As String
            Dim lConstituentField As Integer = -1
            Dim lConstituentDescription As String
            Dim lConstituentString As String
            Dim lLocationField As Integer = -1
            Dim lValueField As Integer = -1
            Dim lValueString As String
            .Delimiter = vbTab
            .OpenStream(aInputReader.BaseStream)

            For lField As Integer = 1 To .NumFields
                Select Case .FieldName(lField)
                    Case "agency_cd"
                    Case "site_no" : lLocationField = lField
                    Case "sample_dt" : lDateField = lField
                    Case "sample_tm" : lTimeField = lField
                    Case "result_va" : lValueField = lField
                    Case "parm_cd" : lConstituentField = lField
                End Select
            Next

            'TODO: are all required fields defined

            While .CurrentRecord < .NumRecords
                lTable.MoveNext()
                lValueString = .Value(lValueField)
                Dim lDateString As String = .Value(lDateField)
                If lValueString.Length = 0 Then
                    'Skip blank values
                ElseIf lDateString.Length = 0 Then
                    Logger.Dbg("ValueSkipped:NoDate:" & lValueString) 'TODO:add more detail
                Else
                    Dim lTimeString As String = .Value(lTimeField)
                    If lTimeString.Length = 0 Then
                        Logger.Dbg("MissingTimeSetTo: 0:00")
                        lTimeString = "0:00"
                    End If
                    Dim lTime As DateTime = "#" & lTimeString & "#"
                    Dim lDate As DateTime = "#" & lDateString & "#"
                    lDateJ = lTime.ToOADate + lDate.ToOADate
                    If lDateJ <> 0 Then
                        lLocation = .Value(lLocationField)
                        lConstituentString = .Value(lConstituentField)
                        Dim lDataKey As String = lLocation & ":" & lConstituentString
                        If Not lData Is Nothing AndAlso lData.Attributes.GetValue("DataKey") = lDataKey Then
                            'Already have correct dataset to append to
                        ElseIf lRawDataSets.Keys.Contains(lDataKey) Then
                            lData = lRawDataSets.ItemByKey(lDataKey)
                        Else
                            lData = New atcTimeseriesBuilder(Me)
                            lData.Attributes.ChangeTo(aAttributes)
                            lConstituentDescription = lConstituentDescriptions.ItemByKey(lConstituentString)
                            lData.Attributes.SetValue("ConstituentDescription", lConstituentDescription)
                            lData.Attributes.SetValue("ID", lRawDataSets.Count + 1)
                            Dim lParsed() As String = lConstituentDescription.Split(",")
                            Dim lConstituentName As String = ""
                            For Each lParse As String In lParsed
                                lConstituentName &= ", " & lParse
                                If Not IsNumeric(lParse) Then Exit For
                            Next
                            lConstituentName = lConstituentName.Substring(2)
                            lData.Attributes.SetValue("Constituent", lConstituentName)
                            lData.Attributes.SetValue("Units", lParsed(lParsed.GetUpperBound(0)))
                            lData.Attributes.SetValue("ParmCode", lConstituentString)
                            lData.Attributes.SetValue("Point", True)
                            lData.Attributes.SetValue("Count", 0)
                            lData.Attributes.SetValue("Scenario", "OBSERVED")
                            lData.Attributes.SetValue("Location", lLocation)
                            lData.Attributes.SetValue("DataKey", lDataKey)
                            lRawDataSets.Add(lDataKey, lData)
                        End If
                        lData.AddValue(lDateJ, lValueString)
                    End If
                End If
            End While
        End With

        For Each lData In lRawDataSets
            lData.Attributes.RemoveByKey("DataKey")
            DataSets.Add(lData.CreateTimeseries)
        Next
        lRawDataSets.Clear()

    End Sub

    Sub ProcessDailyValues(ByVal aInputReader As BinaryReader, ByVal aAttributes As atcDataAttributes)
        Dim lCurLine As String
        Dim lParmCode As String
        Dim lStatisticCode As String
        Dim lConstituentDescriptions As New atcCollection

        While aInputReader.PeekChar = Asc("#")
            lCurLine = NextLine(aInputReader)
            If lCurLine.Length > 50 Then
                'Remember extended column labels
                lParmCode = lCurLine.Substring(10, 5)
                lStatisticCode = lCurLine.Substring(20, 5)
                If IsNumeric(lParmCode) AndAlso IsNumeric(lStatisticCode) Then
                    lConstituentDescriptions.Add(lCurLine.Substring(5, 2) & "_" & lParmCode & "_" & lStatisticCode, lCurLine.Substring(30).Trim)
                End If
            End If
        End While
        Dim lRawDataSets As New atcTimeseriesGroup
        Dim lTSIndex As Integer = 0
        Dim lNCons As Integer = 0
        Dim lData As atcTimeseries = Nothing
        Dim lDateArr(6) As Integer
        lDateArr(3) = 24 'No hours in this file format, put measurement at end of day
        lDateArr(4) = 0 'No minutes in this file format
        lDateArr(5) = 0 'No seconds in this file format

        Dim lTable As New atcTableDelimited
        With lTable
            Dim lDate As Double
            Dim lLocation As String
            Dim lField As Integer
            Dim lConstituentDescription As String
            Dim lDateField As Integer = -1
            Dim lLocationField As Integer = -1
            Dim lValueFields As New ArrayList
            Dim lValueConstituentDescriptions As New atcCollection
            Dim lCurValue As String
            .Delimiter = vbTab
            .OpenStream(aInputReader.BaseStream)

            For lField = 1 To .NumFields
                Select Case .FieldName(lField)
                    Case "agency_cd"
                    Case "site_no" : lLocationField = lField
                    Case "datetime" : lDateField = lField
                    Case Else
                        If .FieldName(lField).EndsWith("_cd") Then 'code field
                            ' TODO: add codes as ValueAttributes and decide how to treat Provisional values
                            'Currently dropping Provisional values below by "peeking" at column next to value
                        Else
                            Dim lConstituentIndex As Integer = _
                                lConstituentDescriptions.IndexFromKey(.FieldName(lField))
                            If lConstituentIndex >= 0 Then
                                lValueFields.Add(lField)
                                lValueConstituentDescriptions.Add(lField, lConstituentDescriptions.ItemByIndex(lConstituentIndex))
                            Else
                                Logger.Dbg("Found value column in RDB not contained in header: " & .FieldName(lField) & " (#" & lField & ")")
                            End If
                        End If
                End Select
            Next

            While lTable.CurrentRecord < lTable.NumRecords
                lTable.MoveNext()
                lDate = Date.Parse(.Value(lDateField)).ToOADate() + pJulianInterval 'add one interval to put date at end of interval

                If lDate <> 0 Then
                    lLocation = .Value(lLocationField)
                    For Each lField In lValueFields
                        lCurValue = .Value(lField).Trim
                        If lCurValue.Length = 0 Then
                            'Skip blank values
                            'If next field is code for this field, then make sure its code starts with "A" for Approved
                        ElseIf .FieldName(lField + 1) <> .FieldName(lField) & "_cd" OrElse _
                               .Value(lField + 1).StartsWith("A") Then
                            lConstituentDescription = lValueConstituentDescriptions.ItemByKey(lField)

                            Dim lDataKey As String = lLocation & ":" & lConstituentDescription
                            If Not lData Is Nothing AndAlso lData.Attributes.GetValue("DataKey") = lDataKey Then
                                'Already have correct dataset to append to
                            ElseIf lRawDataSets.Keys.Contains(lDataKey) Then
                                lData = lRawDataSets.ItemByKey(lDataKey)
                            Else
                                lData = New atcTimeseries(Me)
                                lData.Dates = New atcTimeseries(Me)
                                lData.Attributes.ChangeTo(aAttributes)
                                lData.Attributes.SetValue("ID", lRawDataSets.Count + 1)
                                lData.numValues = lTable.NumRecords - 1

                                Select Case .FieldName(lField).Substring(3, 5)
                                    Case "00060" : lData.Attributes.SetValue("Constituent", "FLOW")
                                    Case Else : lData.Attributes.SetValue("Constituent", .FieldName(lField).Substring(3, 5))
                                End Select

                                Select Case .FieldName(lField).Substring(9, 5)
                                    Case "00001" : lData.Attributes.SetValue("TSFORM", "5") 'Maximum
                                    Case "00002" : lData.Attributes.SetValue("TSFORM", "4") 'Minimum
                                    Case "00003" : lData.Attributes.SetValue("TSFORM", "1") 'Mean
                                End Select
                                lData.Attributes.SetValue("Count", 0)
                                lData.Attributes.SetValue("Scenario", "OBSERVED")
                                lData.Attributes.SetValue("Location", lLocation)
                                lData.Attributes.SetValue("ConstituentDescription", lConstituentDescription)
                                lData.Attributes.SetValue("DataKey", lDataKey)

                                lRawDataSets.Add(lDataKey, lData)
                                lData.Dates.Value(0) = lDate - pJulianInterval
                                lData.Value(0) = GetNaN()
                            End If
                            lTSIndex = lData.Attributes.GetValue("Count") + 1
                            lData.Value(lTSIndex) = lCurValue
                            lData.Dates.Value(lTSIndex) = lDate
                            If .FieldName(lField + 1) = .FieldName(lField) & "_cd" AndAlso _
                               .Value(lField + 1).Contains("e") Then
                                lData.ValueAttributes(lTSIndex).Add("Estimated", True)
                            End If
                            lData.Attributes.SetValue("Count", lTSIndex)
                        End If
                    Next
                End If
            End While
        End With
        '
        Dim lMissingVal As Double = -999
        For Each lData In lRawDataSets
            lTSIndex = lData.Attributes.GetValue("Count")
            If lData.numValues <> lTSIndex Then
                lData.numValues = lTSIndex
            End If
            lData.Attributes.RemoveByKey("DataKey")
            DataSets.Add(FillValues(lData, atcTimeUnit.TUDay, 1, atcUtility.GetNaN, lMissingVal, , Me))
        Next
        lRawDataSets.Clear()
    End Sub

    ''' <summary>
    ''' build a timeseries from IDA flow values in an USGS RDB file
    ''' </summary>
    ''' <param name="aInputReader">Binary reader</param>
    ''' <param name="aAttributes">Generic attributes</param>
    ''' <remarks>flow timeseries added to flow data</remarks>
    Sub ProcessIdaValues(ByVal aInputReader As BinaryReader, ByVal aAttributes As atcDataAttributes)
        Dim lTimeStart As Date = Now
        Logger.Dbg("StartProcessIdaValues")
        Dim lTimeseries As New atcTimeseries(Me)
        lTimeseries.Dates = New atcTimeseries(Me)
        With lTimeseries
            .Attributes.ChangeTo(aAttributes)
            .Attributes.Add("Constituent", "Flow")
            .Attributes.Add("Scenario", "Observed")
            .Attributes.Add("Point", True)

            Dim lCurLine As String
            While aInputReader.PeekChar = Asc("#")
                lCurLine = NextLine(aInputReader)
                If lCurLine.StartsWith("#  USGS") Then
                    .Attributes.Add("Agency", "USGS")
                    .Attributes.Add("Location", lCurLine.Substring(8, 8))
                    .Attributes.Add("StaNam", lCurLine.Substring(17, lCurLine.Length - 17))
                End If
            End While
        End With

        Dim lTable As New atcTableDelimited
        With lTable
            .Delimiter = vbTab
            .OpenStream(aInputReader.BaseStream)
            Dim lTimeStreamOpened As Date = Now
            Logger.Dbg("StreamOpened;Elapsed " & (lTimeStreamOpened - lTimeStart).TotalSeconds & " " & MemUsage())

            Dim lDateField As Integer = -1
            Dim lLocationField As Integer = -1
            Dim lValueField As Integer = -1
            Dim lTimeZoneField As Integer = -1
            Dim lAccuracyCodeField As Integer = -1
            For lField As Integer = 1 To .NumFields
                Select Case .FieldName(lField)
                    Case "site_no" : lLocationField = lField
                    Case "date_time" : lDateField = lField
                    Case "value" : lValueField = lField
                    Case "tz_cd" : lTimeZoneField = lField
                    Case "accuracy_cd" : lAccuracyCodeField = lField
                    Case Else
                        If .FieldName(lField).EndsWith("_cd") Then 'code field
                            ' TODO: add codes as ValueAttributes and decide how to treat Provisional values
                            'Currently dropping Provisional values below by "peeking" at column next to value
                        End If
                End Select
            Next
            lTimeseries.numValues = .NumRecords - 1

            Dim lAccuracyCodeCounter As New atcCollection
            Dim lValue As String = ""
            Dim lDateJ As Double = Double.NaN
            Dim lDatePrevJ As Double = Double.NaN
            Dim lTimeZone As String = ""
            Dim lTimeZonePrev As String
            Dim lTU_TS_Needed As Boolean = True
            Dim lIndex As Integer = 0
            Dim lNotFirst As Boolean = False

            For lRecordIndex As Integer = 2 To .NumRecords
                .CurrentRecord = lRecordIndex
                lDatePrevJ = lDateJ
                Dim lDateString As String = .Value(lDateField)
                Dim lDate As New Date(lDateString.Substring(0, 4), lDateString.Substring(4, 2), lDateString.Substring(6, 2), _
                                      lDateString.Substring(8, 2), lDateString.Substring(10, 2), lDateString.Substring(12, 2))
                lDateJ = lDate.ToOADate

                lTimeZonePrev = lTimeZone
                lTimeZone = .Value(lTimeZoneField)
                If lNotFirst AndAlso lTimeZone <> lTimeZonePrev Then
                    Logger.Dbg("ChangeTimeZoneAt " & lDateString)
                Else
                    Dim lTimeZoneNumeric As Integer
                    Select Case lTimeZone
                        Case "AST", "EDT" : lTimeZoneNumeric = -4
                        Case "EST", "CDT" : lTimeZoneNumeric = -5
                        Case "CST", "MDT" : lTimeZoneNumeric = -6
                        Case "MST", "PDT" : lTimeZoneNumeric = -7
                        Case "PST" : lTimeZoneNumeric = -8
                        Case "GMT", "UTC" : lTimeZoneNumeric = 0
                        Case Else
                            lTimeZoneNumeric = -999
                            Logger.Dbg("UnknownTimeZone " & lTimeZone)
                    End Select
                    If lTimeZoneNumeric >= -12 Then lTimeseries.Attributes.Add("TMZONE", lTimeZoneNumeric)
                    lNotFirst = True
                End If
                If lTU_TS_Needed AndAlso Not Double.IsNaN(lDatePrevJ) Then
                    Dim lTu As atcTimeUnit
                    Dim lTs As Integer
                    CalcTimeUnitStep(lDatePrevJ, lDateJ, lTu, lTs)
                    Dim lTimeDif As Double = lDateJ - lDatePrevJ
                    lTimeseries.Attributes.Add("TU", lTu)
                    lTimeseries.Attributes.Add("TS", lTs)
                    lTU_TS_Needed = False
                    lTimeseries.Dates.Value(0) = lTimeseries.Dates.Value(1) - lTimeDif
                End If

                If lDateJ = 0 Then
                    Logger.Dbg("BadDate " & .CurrentRecord.ToString)
                Else
                    lIndex += 1
                    lTimeseries.Value(lIndex) = .Value(lValueField)
                    lTimeseries.Dates.Value(lIndex) = lDateJ
                    Dim lAccuracyCode As String = .Value(lAccuracyCodeField)
                    lAccuracyCodeCounter.Increment(lAccuracyCode)
                    'NOTE: the next statement adds a requirement for about 3.5 times more memory!! 
                    'lTimeseries.ValueAttributes(lIndex).Add("AccuracyCode", lAccuracyCode)
                End If
                If .CurrentRecord Mod 1000 = 0 Then
                    Logger.Progress("Reading IDA Values", .CurrentRecord, .NumRecords)
                End If
            Next
            Logger.Progress("", 0, 0)
            Logger.Dbg("IdaValuesTimeseriesCreated;Elapsed " & (Now - lTimeStreamOpened).TotalSeconds & " " & MemUsage())
            Logger.Dbg("AccuracyCodeCounts")
            For lIndex = 0 To lAccuracyCodeCounter.Count - 1
                Logger.Dbg(lAccuracyCodeCounter.Keys(lIndex) & ":" & lAccuracyCodeCounter.Item(lIndex))
            Next
        End With
        Dim lTimeCreated As Date = Now
        lTimeseries.Attributes.CalculateAll()
        Logger.Dbg("IdaValuesAttributesCalculated;Elapsed " & (Now - lTimeCreated).TotalSeconds & " " & MemUsage())
        Me.AddDataSet(lTimeseries)
    End Sub

    'Private Function GetData(ByVal aSites As ArrayList, _
    '                Optional ByVal cache_dir As String = "", _
    '                Optional ByVal base_url As String = "http://waterdata.usgs.gov/nwis/dv?cb_00060=on", _
    '                Optional ByVal begin_date As String = "1800-01-01", _
    '                Optional ByVal end_date As String = "2100-01-01", _
    '                Optional ByVal suffix As String = "_dv.txt") As Boolean

    '    Dim pLabel As String = "USGS Daily Streamflow"
    '    Dim save_filename As String
    '    Dim myDownloadFiles As New ArrayList 'of file names
    '    Dim url As String
    '    Dim iSite As Integer
    '    Dim FirstFile As Boolean
    '    Dim FilesNotCreated As String = ""
    '    Dim nFilesNotCreated As Integer
    '    Dim FileNumber As Integer

    '    Dim findPos As Integer
    '    Dim msg As String

    '    Try

    '        Logger.Dbg("  clsUsgsDaily GetData entry")

    '        'http://waterdata.usgs.gov/nwis/dv?cb_00060=on&format=rdb&begin_date=1800-01-01&end_date=2100-01-01&site_no=01591000&referred_module=sw
    '        'cache_dir = pManager.CurrentStatusGetString("cache_dir") & pClassName & g_PathChar
    '        'project_dir = pManager.CurrentStatusGetString("project_dir")
    '        '  SHPfilename = project_dir & pManager.CurrentStatusGetString("USGSdailySHPfile", "gage.shp")
    '        'suffix = pManager.CurrentStatusGetString("USGSdailySaveSuffix", "_dv.txt")


    '        GetData = True

    '        'If Len(WDMfilename) > 0 Then
    '        '    myDownloadFiles = New Collection
    '        'Else 'Save downloaded RDB files in folder inside project_dir if we are not adding to WDM
    '        '    project_dir = project_dir & "USGSflow" & g_PathChar
    '        '    Logger.Dbg("Saving RDB files in " & project_dir)
    '        'End If
    '        MkDirPath(cache_dir)

    '        FirstFile = True
    '        iSite = 0
    '        For Each lSite As String In aSites 'For iSite = 1 To nSites
    '            iSite = iSite + 1
    '            url = base_url & "&format=rdb" & _
    '                        "&begin_date=" & begin_date & _
    '                        "&end_date=" & end_date & _
    '                        "&site_no=" & lSite

    '            'siteAttributes = "# " & url & vbCrLf
    '            'For iAttr = 0 To lSite.NumAttributes - 1
    '            '    siteAttributes = siteAttributes & "# " & lSite.GetAttributeName(iAttr) _
    '            '                            & Space(48 - Len(lSite.GetAttributeName(iAttr))) _
    '            '                                           & lSite.GetAttributeValue(iAttr) & vbCrLf
    '            'Next
    '            save_filename = cache_dir & lSite & suffix

    '            'If Not pManager.Download(url, save_filename, FirstFile, "Downloading " & pLabel & " (" & iSite & " of " & lstSites.Count & ")", siteAttributes) Then
    '            '    nodStatus.AddAttribute("message", "User Cancelled")

    '            '    Exit Function '!!!!!!!!!!!!!!!!!!!

    '            'End If
    '            msg = WholeFileString(save_filename)

    '            findPos = InStr(msg, "<html")
    '            If findPos > 0 Then 'Got an error message or web page, not the data we expected
    '                'msg = Mid(pManager.ResultString, findPos)
    '                Kill(save_filename)
    '                nFilesNotCreated = nFilesNotCreated + 1
    '                FilesNotCreated &= "   " & FilenameNoPath(save_filename)
    '                If InStr(msg, "No data") > 0 Then
    '                    FilesNotCreated &= " (no data)"
    '                ElseIf InStr(msg, "No site") > 0 Then
    '                    FilesNotCreated &= " (no site)"
    '                Else
    '                    FilesNotCreated &= " (error)"
    '                End If
    '                FilesNotCreated = FilesNotCreated & vbCrLf
    '            Else
    '                'Replace LF with CR/LF
    '                msg = ReplaceString(msg, vbLf, vbCrLf)
    '                'Above replacement may have added some unwanted CR
    '                msg = ReplaceString(msg, vbCr & vbCr, vbCr)
    '                SaveFileString(msg, save_filename)
    '                'If Len(WDMfilename) > 0 Then
    '                myDownloadFiles.Add(save_filename)
    '                'Else
    '                'Logger.Dbg("Copying downloaded file to " & project_dir & FilenameNoPath(save_filename))
    '                'FileCopy(save_filename, project_dir & FilenameNoPath(save_filename))
    '                'End If
    '            End If
    '            FirstFile = False
    '        Next
    '        'If Len(WDMfilename) > 0 Then
    '        '    Logger.Dbg("Saving downloaded data to " & WDMfilename)
    '        Try
    '            '    ConvertUsgsDv2Wdm(WDMfilename, myDownloadFiles)
    '        Catch
    '            Logger.Msg("Error writing WDM file" & vbCr & Err.Description & vbCr & "Libraries may need to be installed for saving WDM files", _
    '                                   pLabel & " GetData")
    '        End Try
    '        'End If
    '        'If nFilesNotCreated > 0 Then
    '        save_filename = cache_dir & "USGSflowNoData.txt"
    '        While Len(Dir(save_filename)) > 0
    '            FileNumber = FileNumber + 1
    '            save_filename = cache_dir & "USGSflowNoData(" & FileNumber & ").txt"
    '        End While
    '        If nFilesNotCreated > 10 Then
    '            findPos = 1
    '            For FileNumber = 1 To 10
    '                findPos = FilesNotCreated.IndexOf(CStr(vbCr), findPos + 1)
    '            Next
    '            msg = Left(FilesNotCreated, findPos) & " (and " & (nFilesNotCreated - 10) & " more)"
    '        Else
    '            msg = FilesNotCreated
    '        End If

    '        If Logger.Msg("Did not find data for " & nFilesNotCreated & " of " & aSites.Count & " stations: " & vbCr & vbCr _
    '                 & msg & vbCr _
    '                 & "Save this list to " & save_filename & "?", _
    '                 pLabel & " - Some data not found", "+&Yes", "-&No") = 1 Then
    '            SaveFileString(save_filename, FilesNotCreated)
    '        End If

    '        Logger.Dbg("  clsUsgsDaily GetData exit")
    '        Return True

    '    Catch ex As Exception
    '        Logger.Msg("Error '" & ex.Message & "'", pLabel & " GetData")
    '    End Try
    'End Function

    Public Sub New()
        Filter = pFilter
    End Sub
End Class

