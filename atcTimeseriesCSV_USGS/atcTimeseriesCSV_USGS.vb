Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.IO


''' <summary>
''' Reads USGS CSV files containing daily values
''' </summary>

Public Class atcTimeseriesCSV_USGS
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "USGS CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
    Private pJulianInterval As Double = 1 'Add one day for daily values to record date at end of interval

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "USGS CSV File"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::USGS CSV"
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

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String,
                          Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean

        If MyBase.Open(aFileName, aAttributes) Then
            If Not IO.File.Exists(Specification) Then
                Logger.Dbg("Opening new file " & Specification)
                Return True
            ElseIf Specification.Contains(",") Then
                Logger.Dbg("Multiple files selected " & Specification)
                Return True
            ElseIf IO.Path.GetFileName(Specification).ToLower.StartsWith("nwis_stations") Then
                Throw New ApplicationException("Station file does not contain timeseries data: " & IO.Path.GetFileName(Specification))
            Else
                Try
                    Dim lTimeStartOpen As Date = Now
                    Logger.Dbg("OpenStartFor " & Specification)

                    Dim lInputStream As New FileStream(Specification, FileMode.Open, FileAccess.Read)
                    Dim lInputBuffer As New BufferedStream(lInputStream)
                    Dim lInputReader As New BinaryReader(lInputBuffer)

                    Dim lSite As Boolean = False
                    Dim lWQData As Boolean = False
                    Dim lMeasurementsData As Boolean = False
                    Dim lPeriodicGroundwaterData As Boolean = False
                    Dim lIdaData As Boolean = False
                    'Dim lDailyDischargeData As Boolean = False

                    Dim lAttributes As New atcDataAttributes
                    Dim lDefFlagFormat As String = "IsNewParamFormat"
                    Dim lDefStartDefinition As String = "StartDefinition"
                    lAttributes.SetValue(lDefFlagFormat, True)
                    lAttributes.SetValue(lDefStartDefinition, 30)
                    Dim lHeaderLine As String

                    Dim lFirstLine As Boolean = True
                    Dim lAtts As New atcCollection
                    Dim lHeader As String
                    Dim lValue As String
                    Dim lConstituent As String = ""
                    Dim lLocation As String = ""

                    'get whatever attributes we can from header
                    While lInputReader.PeekChar = 35 ' Asc("#")
                        lHeaderLine = NextLine(lInputReader)
                        'change double quotes to single
                        lHeaderLine = Replace(lHeaderLine, Chr(34), "'")
                        lHeaderLine = Replace(lHeaderLine, " ", "_")
                        lHeaderLine = "# " & Mid(lHeaderLine, 3)
                        Dim lAttVals As String = NextLine(lInputReader)
                        lAttVals = Replace(lAttVals, Chr(34), "'")
                        If lHeaderLine.Length > 0 Then
                            While lHeaderLine.Length > 0
                                lHeader = StrRetRem(lHeaderLine)
                                lValue = StrRetRem(lAttVals)
                                If lHeader = "site_no" Or lHeader = "monitoring_location_id" Then
                                    lAttributes.SetValue("AGENCY", MapWinUtility.Strings.StrSplit(lValue, "-", ""))
                                    lLocation = MapWinUtility.Strings.StrSplit(lValue, "-", "")
                                    lAttributes.SetValue("Location", lLocation)
                                ElseIf lHeader = "Description" Then
                                    lAttributes.SetValue("Description", lValue)
                                ElseIf lHeader = "Parameter" Then
                                    Dim lUnits As String = Nothing
                                    Select Case lValue
                                        Case "00045" : lConstituent = "Precipitation" : lUnits = "inches"
                                        Case "00060" : lConstituent = "Streamflow" : lUnits = "cubic feet per second"
                                        Case "discharge" : lConstituent = "Streamflow" : lUnits = "cubic feet per second"
                                        Case "61055" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Water level, depth below measuring point, feet 
                                        Case "62611" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Groundwater level above NAVD 1988, feet 
                                        Case "72019" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Depth to water level, feet below land surface 
                                        Case "72020" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Elevation above NGVD 1929, feet 
                                        Case "72150" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Groundwater level relative to Mean Sea Level (MSL), feet.
                                    End Select
                                    lAttributes.SetValue("Parameter", lValue)
                                    lAttributes.SetValue("Constituent", lConstituent)
                                    lAttributes.SetValue("Units", lUnits)
                                ElseIf lHeader = "Statistic" Then
                                    If IsNumeric(lValue) Then
                                        lAttributes.SetValue("statistic", lValue)
                                        Select Case lValue
                                            Case "00001" : lAttributes.SetValue("TSFORM", "5") 'Maximum
                                            Case "00002" : lAttributes.SetValue("TSFORM", "4") 'Minimum
                                            Case "00003" : lAttributes.SetValue("TSFORM", "1") 'Mean
                                            Case "00006" : lAttributes.SetValue("TSFORM", "2") 'Sum
                                        End Select
                                    End If
                                ElseIf lHeader = "Site_Number" Or lHeader = "monitoring_location_number" Then
                                    lAttributes.SetValue("STAID", lValue)
                                    lLocation = lValue
                                ElseIf lHeader = "Station_name" Or lheader = "monitoring_location_name" Then
                                    lAttributes.SetValue("StaNam", lValue)
                                ElseIf lHeader = "Latitude" Then
                                    lAttributes.SetValue("Latitude", CDbl(lValue))
                                ElseIf lHeader = "Longitude" Then
                                    lAttributes.SetValue("Longitude", -Math.Abs(CDbl(lValue)))
                                ElseIf lHeader = "HUC_code" Then
                                ElseIf lHeader = "Drainage_area" Then
                                    If lValue.Length > 0 Then
                                        lAttributes.SetValue("drainage area", CDbl(lValue))
                                    End If
                                End If
                            End While
                        End If
                        Exit While
                    End While

                    lAttributes.SetValue("Count", 0)
                    lAttributes.SetValue("Scenario", "OBSERVED")
                    Dim lDataKey As String = lLocation & ":" & lConstituent
                    lAttributes.SetValue("DataKey", lDataKey)
                    lAttributes.AddHistory("Read from " & Specification)
                    If lAttributes.GetValue("Location") Is Nothing Then
                        lAttributes.SetValue("Location", FilenameNoExt(FilenameNoPath(Specification)))
                    End If

                    If FilenameNoPath(Specification.Contains("periodic")) Then
                        lPeriodicGroundwaterData = True
                    End If

                    If lSite Then
                        Throw New ApplicationException("Station list does Not contain timeseries data:   " & IO.Path.GetFileName(Specification))
                    ElseIf lWQData Then
                        'ProcessWaterQualityValues(Specification, lAttributes)
                    ElseIf lMeasurementsData Then
                        'ProcessMeasurements(Specification, lAttributes)
                    ElseIf lIdaData Then
                        'ProcessIdaValues(Specification, lAttributes)
                    ElseIf lPeriodicGroundwaterData Then
                        ProcessPeriodicGroundwater(lInputReader, lAttributes)
                    Else 'If lDailyDischargeData Then
                        ProcessDailyValues(lInputReader, lAttributes)
                    End If

                    Return True
                Catch lException As Exception
                    Throw New ApplicationException("Exception reading '" & Specification & "': " & lException.Message, lException)
                End Try
            End If
        End If
    End Function

    Sub ProcessDailyValues(ByVal aInputReader As BinaryReader, ByVal aAttributes As atcDataAttributes)
        Dim lCurLine As String
        Dim lDDTS As String
        Dim lParmCode As String
        Dim lStatisticCode As String
        Dim lLocation As String
        Dim lConstituent As String
        Dim lQualificationCode As String
        Dim lQualificationCodes As New atcCollection
        Dim lConstituentDescriptions As New atcCollection
        Dim lArr() As String

        lQualificationCodes.Add("P", "Provisional")
        lQualificationCodes.Add("A", "Approved")

        lParmCode = aAttributes.GetValue("Parameter")
        lStatisticCode = aAttributes.GetValue("statistic")
        lLocation = aAttributes.GetValue("Location")
        lConstituent = aAttributes.GetValue("Constituent")

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
            'Dim lLocation As String
            Dim lField As Integer
            'Dim lConstituentDescription As String
            Dim lDateField As Integer = -1
            Dim lLocationField As Integer = -1
            Dim lValueFields As New ArrayList
            'Dim lValueConstituentDescriptions As New atcCollection
            Dim lCurValue As Double = 0
            .Delimiter = ","
            .OpenStream(aInputReader.BaseStream)

            For lField = 1 To .NumFields
                Select Case .FieldName(lField)
                    Case "agency_cd"
                    Case "site_no" : lLocationField = lField
                    Case "Time" : lDateField = lField
                    Case "Daily values" : lValueFields.Add(lField)
                        'Case Else
                        '    If .FieldName(lField).EndsWith("_cd") Then 'code field
                        '    Else
                        '        Dim lConstituentIndex As Integer =
                        '            lConstituentDescriptions.IndexFromKey(.FieldName(lField))
                        '        If lConstituentIndex >= 0 Then
                        '            lValueFields.Add(lField)
                        '            lValueConstituentDescriptions.Add(lField, lConstituentDescriptions.ItemByIndex(lConstituentIndex))
                        '        Else
                        '            Logger.Dbg("Found value column in RDB not contained in header: " & .FieldName(lField) & " (#" & lField & ")")
                        '        End If
                        '    End If
                End Select
            Next

            Dim lUSGSToolboxProgram As Boolean = False
            Try
                Dim lProgramName As String = System.Reflection.Assembly.GetEntryAssembly.Location
                If lProgramName.Contains("USGSToolbox") OrElse lProgramName.Contains("USGSHydroToolbox") Then
                    lUSGSToolboxProgram = True
                End If
            Catch ex As Exception
                lUSGSToolboxProgram = False
            End Try
            Dim lParsedDate As Date
            Dim lNumRecs As Integer = lTable.NumRecords
            While lTable.CurrentRecord <= lNumRecs
                If Date.TryParse(.Value(lDateField), lParsedDate) Then
                    lDate = lParsedDate.ToOADate() + pJulianInterval 'add one interval to put date at end of interval
                    'lLocation = .Value(lLocationField)
                    For Each lField In lValueFields
                        If Double.TryParse(.Value(lField).Trim, lCurValue) Then
                            'If next field is code for this field, then make sure its code is in the allowed codes, QualificationCodes
                            'This test was for skipping provisional values: 'If .FieldName(lField + 1) <> .FieldName(lField) & "_cd" OrElse QualificationCodes.Contains(.Value(lField + 1).Trim().Substring(0, 1)) Then

                            lQualificationCode = ""
                            If .FieldName(lField + 1) = "Approval status" Then
                                lQualificationCode = .Value(lField + 1).Trim.Replace(":", "")
                            End If

                            'lConstituentDescription = lValueConstituentDescriptions.ItemByKey(lField)

                            Dim lDataKey As String = lLocation & ":" & lConstituent
                            'If lCode.StartsWith("P") Then lDataKey &= ":Provisional" 'Make provisional data a separate timeseries
                            If lData IsNot Nothing AndAlso lData.Attributes.GetValue("DataKey") = lDataKey Then
                                'Already have correct dataset to append to
                            ElseIf lRawDataSets.Keys.Contains(lDataKey) Then
                                lData = lRawDataSets.ItemByKey(lDataKey)
                            Else
                                lData = New atcTimeseries(Me)
                                lData.Dates = New atcTimeseries(Me)
                                lData.Attributes.ChangeTo(aAttributes)
                                lData.Attributes.SetValue("ID", lRawDataSets.Count + 1)
                                lData.numValues = lTable.NumRecords ' - 1  why minus 1???
                                'lArr = .FieldName(lField).Split("_")
                                'Dim lParmCd As String = lArr(1) '.FieldName(lField).Substring(3, 5)
                                Dim lUnits As String = Nothing
                                Select Case lParmCode
                                    Case "00045"
                                        If lUSGSToolboxProgram Then
                                            lConstituent = "Precipitation" : lUnits = "inches"
                                        Else
                                            lConstituent = "PREC" : lUnits = "in"
                                        End If
                                    Case "00060"
                                        If lUSGSToolboxProgram Then
                                            If Len(lConstituent) = 0 Then
                                                lConstituent = "Streamflow"
                                            End If
                                            lUnits = "cubic feet per second"
                                        Else
                                            If Len(lConstituent) = 0 Then
                                                lConstituent = "FLOW"
                                            End If
                                            lUnits = "cfs"
                                        End If
                                    Case "61055" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Water level, depth below measuring point, feet 
                                    Case "62611" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Groundwater level above NAVD 1988, feet 
                                    Case "72019" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Depth to water level, feet below land surface 
                                    Case "72020" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Elevation above NGVD 1929, feet 
                                    Case "72150" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Groundwater level relative to Mean Sea Level (MSL), feet.
                                End Select
                                lData.Attributes.SetValue("parm_cd", lParmCode)
                                lData.Attributes.SetValue("Constituent", lConstituent)
                                'lData.Attributes.SetValue("Description", lConstituentDescription)

                                'If lUnits Is Nothing Then
                                '    If lConstituentDescription.ToLower.Contains("cubic feet per second") Then
                                '        lUnits = "cubic feet per second"
                                '    ElseIf lConstituentDescription.Contains("feet") Then
                                '        lUnits = "feet"
                                '    End If
                                'End If

                                If lUnits IsNot Nothing Then
                                    If lUSGSToolboxProgram Then
                                        Select Case lUnits
                                            Case "ft" : lUnits = "feet"
                                            Case "cfs" : lUnits = "cubic feet per second"
                                        End Select
                                    Else
                                        Select Case lUnits
                                            Case "feet" : lUnits = "ft"
                                            Case "cubic feet per second" : lUnits = "cfs"
                                        End Select
                                    End If
                                    lData.Attributes.SetValue("Units", lUnits)
                                End If

                                'lStatisticCode = lArr(2) 'SafeSubstring(.FieldName(lField), 9, 5)
                                If IsNumeric(lStatisticCode) Then
                                    lData.Attributes.SetValue("statistic", lStatisticCode)
                                    Select Case lStatisticCode
                                        Case "00001" : lData.Attributes.SetValue("TSFORM", "5") 'Maximum
                                        Case "00002" : lData.Attributes.SetValue("TSFORM", "4") 'Minimum
                                        Case "00003" : lData.Attributes.SetValue("TSFORM", "1") 'Mean
                                        Case "00006" : lData.Attributes.SetValue("TSFORM", "2") 'Sum
                                    End Select
                                End If
                                lData.Attributes.SetValue("Count", 0)
                                lData.Attributes.SetValue("Scenario", "OBSERVED")
                                lData.Attributes.SetValue("Location", lLocation)
                                lData.Attributes.SetValue("DataKey", lDataKey)

                                lRawDataSets.Add(lDataKey, lData)
                                lData.Dates.Value(0) = lDate - pJulianInterval
                                lData.Value(0) = GetNaN()
                            End If
                            lTSIndex = lData.Attributes.GetValue("Count") + 1
                            If lTSIndex > lData.numValues Then
                            Else
                                lData.Value(lTSIndex) = lCurValue
                                lData.Dates.Value(lTSIndex) = lDate
                                Dim lCodeChar As String = lQualificationCode(0)
                                lData.ValueAttributes(lTSIndex).Add(lCodeChar, True)
                                Dim lAttributeName As String = "ValueAttributeDescription_" & lCodeChar
                                If Not lData.Attributes.ContainsAttribute(lAttributeName) AndAlso lQualificationCodes.Keys.Contains(lCodeChar) Then
                                    lData.Attributes.SetValue(lAttributeName, lQualificationCodes.ItemByKey(lCodeChar))
                                End If
                                lData.Attributes.SetValue("Count", lTSIndex)
                            End If
                        End If
                    Next
                End If
                lTable.MoveNext()
            End While
        End With

        Dim lMissingVal As Double = -999
        For Each lData In lRawDataSets
            lTSIndex = lData.Attributes.GetValue("Count")
            If lData.numValues <> lTSIndex Then
                lData.numValues = lTSIndex
            End If
            lData.Attributes.RemoveByKey("DataKey")
            DataSets.Add(FillValues(lData, atcTimeUnit.TUDay, 1, GetNaN, lMissingVal, , Me))
        Next
        lRawDataSets.Clear()

        For A As Integer = 0 To lData.Attributes.Count - 1
            With lData.Attributes(A)
                If .Arguments IsNot Nothing Then .Arguments.Clear()
                If .Value.GetType().Name = "atcTimeseries" Then
                    .Value.Clear()
                End If
            End With
        Next
        lData.Clear()
        lData = Nothing
        lTable.Clear()
        lTable = Nothing
    End Sub

    Sub ProcessPeriodicGroundwater(ByVal aInputReader As BinaryReader, ByVal aAttributes As atcDataAttributes)
        Dim lTable As New atcTableDelimited
        With lTable
            Dim lDate As Double
            'Dim lLocation As String
            Dim lField As Integer
            'Dim lConstituentDescription As String
            Dim lDateField As Integer = -1
            Dim lLocationField As Integer = -1
            Dim lParmField As Integer = -1
            Dim lParmCode As String = ""
            Dim lUnitField As Integer = -1
            Dim lValueFields As New ArrayList
            'Dim lValueConstituentDescriptions As New atcCollection
            Dim lCurValue As Double = 0
            .Delimiter = ","
            .OpenStream(aInputReader.BaseStream)

            'header for each data value:
            'time,value,vertical_datum,approval_status,qualifier,measuring_agency,parameter_code,field_measurements_series_id,field_visit_id,observing_procedure_code,observing_procedure,unit_of_measure,last_modified,control_condition,measurement_rated,field_measurement_id
            For lField = 1 To .NumFields
                Select Case .FieldName(lField)
                    Case "time" : lDateField = lField
                    Case "value" : lValueFields.Add(lField)
                    Case "parameter_code" : lParmField = lField
                    Case "unit_of_measure" : lUnitField = lField
                End Select
            Next

            Dim lLastValueField As Integer = lValueFields.Count
            'Dim lValueFieldNumber(lLastValueField) As Integer
            Dim lBuilders(lLastValueField) As atcTimeseriesBuilder
            Dim lValueFieldIndex As Integer

            For lValueFieldIndex = 0 To lLastValueField - 1
                lBuilders(lValueFieldIndex) = New atcTimeseriesBuilder(Me)
                With lBuilders(lValueFieldIndex).Attributes
                    .ChangeTo(aAttributes)
                    .SetValue("Point", True)
                    .SetValue("Scenario", "OBSERVED")
                    .SetValue("ID", lValueFieldIndex + 1)
                End With
            Next

            Dim lDateString As String
            'Dim lTimeString As String
            Dim lParsedDate As Date
            Dim lValueString As String
            Dim lValue As Double
            For lRecord As Integer = 1 To .NumRecords
                .CurrentRecord = lRecord
                lDateString = .Value(lDateField)
                'If lTimeField > 0 Then
                '    lTimeString = .Value(lTimeField).Trim
                '    If lTimeString.Contains(":") AndAlso lTimeString.Length = 5 Then
                '        lDateString &= " " & lTimeString
                '    ElseIf lTimeString.Length = 4 Then
                '        lDateString &= " " & lTimeString.Substring(0, 2) & ":" & lTimeString.Substring(2, 2)
                '    End If
                'End If

                Dim lUnits As String = Nothing
                Dim lConstituent As String = ""
                lParmCode = .Value(lParmField)
                Select Case lParmCode
                    Case "61055" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Water level, depth below measuring point, feet 
                    Case "62611" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Groundwater level above NAVD 1988, feet 
                    Case "72019" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Depth to water level, feet below land surface 
                    Case "72020" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Elevation above NGVD 1929, feet 
                    Case "72150" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Groundwater level relative to Mean Sea Level (MSL), feet.
                End Select

                If Date.TryParse(lDateString, lParsedDate) Then
                    lDate = lParsedDate.ToOADate() '+ pJulianInterval 'add one interval to put date at end of interval
                    For lValueFieldIndex = 0 To lLastValueField - 1
                        If lValueFields(lValueFieldIndex) > 0 Then
                            lValueString = .Value(lValueFields(lValueFieldIndex))
                            If Double.TryParse(lValueString, lValue) Then
                                lBuilders(lValueFieldIndex).AddValue(lDate, lValue)
                                'add other attributes here 
                                With lBuilders(lValueFieldIndex).Attributes
                                    .SetValue("Constituent", lConstituent)
                                    .SetValue("Units", lUnits)
                                    .SetValue("Location", .GetValue("site_no"))
                                    .SetValue("parm_cd", lParmCode)
                                End With
                            End If
                        End If
                    Next
                End If
            Next
            Dim lTs As atcTimeseries
            For lValueFieldIndex = 0 To lLastValueField - 1
                If lBuilders(lValueFieldIndex) IsNot Nothing AndAlso lBuilders(lValueFieldIndex).NumValues > 0 Then
                    lTs = lBuilders(lValueFieldIndex).CreateTimeseries
                    If lTs.Attributes.GetValue("Count", 0) > 0 Then DataSets.Add(lTs)
                End If
            Next
        End With

    End Sub

    Public Sub New()
        Filter = pFilter
    End Sub
End Class

