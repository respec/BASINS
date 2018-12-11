Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.Collections
Imports System.IO
Imports System.Text.RegularExpressions

''' <summary>
''' Reads USGS rdb files containing daily values
''' </summary>
''' <remarks>
''' Would need to change pJulianInterval, ts and tu for non-daily values
''' Includes provisional values in timeseries and marks them with a value attribute
''' </remarks>

Public Class atcTimeseriesRDB
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "USGS RDB Files (*.rdb, *.txt)|*.rdb;*.txt|All Files (*.*)|*.*"
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

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function Save(ByVal aSaveFileName As String, Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean
        Dim lSaveFileName As String = ""
        Try
            Dim lFilenameRoot As String = FilenameNoExt(aSaveFileName)
            Dim lsaveRDBTables As atcCollection = AsTableRDB()
            For Each lKeyLoc As String In lsaveRDBTables.Keys
                Dim lsaveRDBTable As atcTableRDB = lsaveRDBTables.ItemByKey(lKeyLoc)
                lSaveFileName = GetNewFileName(lFilenameRoot & "_" & SafeFilename(lKeyLoc, ""), ".rdb")
                'lSaveFileName = lFilenameRoot & "_" & SafeFilename(lKeyLoc, "") & ".rdb"
                'Logger.Dbg("Save " & Me.Name & " in " & lSaveFileName)
                'If IO.File.Exists(lSaveFileName) Then
                '    Dim lExtension As String = IO.Path.GetExtension(lSaveFileName)
                '    Dim lRenamedFilename As String = GetNewFileName(lSaveFileName.Substring(0, lSaveFileName.Length - lExtension.Length), lExtension)
                '    Select Case aExistAction
                '        Case EnumExistAction.ExistAppend
                '            Logger.Dbg("Save: File already exists and aExistAction = ExistAppend, not implemented.")
                '            Throw New ApplicationException("Append not implemented for " & Me.Name)
                '        Case EnumExistAction.ExistAskUser
                '            Select Case Logger.MsgCustom("Attempting to save but file already exists: " & vbCrLf & lSaveFileName, "File already exists",
                '                                         "Overwrite", "Do not write", "Save as " & IO.Path.GetFileName(lRenamedFilename))
                '                Case "Overwrite"
                '                    IO.File.Delete(lSaveFileName)
                '                Case "Do not write"
                '                    Return False
                '                Case Else
                '                    lSaveFileName = lRenamedFilename
                '            End Select
                '        Case EnumExistAction.ExistNoAction
                '            Logger.Dbg("Save: File already exists and aExistAction = ExistNoAction, not saving " & lSaveFileName)
                '        Case EnumExistAction.ExistReplace
                '            Logger.Dbg("Save: File already exists, deleting old " & lSaveFileName)
                '            IO.File.Delete(lSaveFileName)
                '        Case EnumExistAction.ExistRenumber
                '            Logger.Dbg("Save: File already exists, saving as " & lRenamedFilename)
                '            lSaveFileName = lRenamedFilename
                '    End Select
                'End If
                lsaveRDBTable.WriteFile(lSaveFileName)
            Next
            Return True
        Catch e As Exception
            Logger.Msg("Error writing '" & lSaveFileName & "': " & e.ToString, MsgBoxStyle.OkOnly, "Did not write file")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Break up Datasets into groups of RDB tables, one per location
    ''' each location table would contain all constituents for that location
    ''' </summary>
    ''' <returns>a collection of atcTableRDBs, one per location</returns>
    ''' <remarks></remarks>
    Private Function AsTableRDB() As atcCollection 'of atcTableRDB(s)
        'build headers
        'set column numbers, column width, column type, and column names

        'Step 1. Break into location groups
        Dim lStationGroups As New atcCollection()
        Dim lLocation As String = ""
        Dim lCons As String = ""
        Dim lParmCd As String = ""
        Dim lStatCd As Integer = 0
        Dim lStatCdStr As String = ""
        For Each lDS As atcTimeseries In DataSets
            Dim lStatCdNeedtoIncrement As Boolean = False
            With lDS.Attributes
                lLocation = .GetValue("Location", "")

                Dim lSeason As String = SafeFilename(.GetValue("SeasonName", ""), "")
                If lSeason.Length > 0 Then
                    If lLocation.Length > 0 Then lLocation &= "_"
                    lLocation &= lSeason
                End If

                lCons = .GetValue("Constituent", "")
                If lCons.StartsWith("BF_") Then
                    'For baseflow timeseries, construct fictious parm_cd and stat_cd
                    lParmCd = "11111"
                    lStatCdStr = lStatCd.ToString.PadLeft(5, "0")
                    lStatCdNeedtoIncrement = True
                    .SetValue("parm_cd", lParmCd)
                    .SetValue("statistic", lStatCdStr)
                Else
                    lParmCd = .GetValue("parm_cd", "")
                    lStatCdStr = .GetValue("statistic", "")
                End If
            End With
            If Not String.IsNullOrEmpty(lLocation) Then
                If lStationGroups.Keys.Contains(lLocation) Then
                    If Not CType(lStationGroups.ItemByKey(lLocation), atcTimeseriesGroup).Keys.Contains(lCons) Then
                        CType(lStationGroups.ItemByKey(lLocation), atcTimeseriesGroup).Add(lCons, lDS)
                    Else
                        'only holds unique constituents for each location
                        'hence, do nothing here for duplicate constituents
                    End If
                Else
                    Dim lNewSetCons As New atcTimeseriesGroup()
                    lNewSetCons.Add(lCons, lDS)
                    lStationGroups.Add(lLocation, lNewSetCons)
                End If
            End If
            If lStatCdNeedtoIncrement Then lStatCd += 1
        Next

        Dim lUniqueGroups As New atcCollection
        'Break up any groups that do not have the same dates
        For Each lKeyLoc As String In lStationGroups.Keys
            Dim lStationTSGroup As atcTimeseriesGroup = lStationGroups.ItemByKey(lKeyLoc)
            Dim lFirstStart, lLastEnd, lCommonStart, lCommonEnd As Double
            If Not CommonDates(lStationTSGroup, lFirstStart, lLastEnd, lCommonStart, lCommonEnd) _
                OrElse lFirstStart <> lCommonStart OrElse lLastEnd <> lCommonEnd Then
                Dim lIndex As Integer = 1
                For Each lTs As atcTimeseries In lStationTSGroup
                    lUniqueGroups.Add(lKeyLoc & "_" & lIndex, New atcTimeseriesGroup(lTs))
                    lIndex += 1
                Next
            Else
                lUniqueGroups.Add(lKeyLoc, lStationTSGroup)
            End If
        Next

        'Step 2. Build one atcTableRDB per location
        Dim lRDBTables As New atcCollection()
        For Each lKeyLoc As String In lUniqueGroups.Keys
            Dim lStationTSGroup As atcTimeseriesGroup = lUniqueGroups.ItemByKey(lKeyLoc)
            Dim lRDBTable As New atcTableRDB()
            Dim lDD_cd As String = "02"
            With lRDBTable
                .NumFields = 3 + lstationTSGroup.Count 'USGS Location DateDaily(assume daily for now)
                .FieldName(1) = "agency_cd" : .FieldType(1) = "S" : .FieldLength(1) = 5
                .FieldName(2) = "site_no" : .FieldType(2) = "S" : .FieldLength(2) = 15
                .FieldName(3) = "datetime" : .FieldType(3) = "D" : .FieldLength(3) = 20
                Dim lFieldLengthValue As Integer = 14
                Dim lFieldInd As Integer = 4
                For Each lTS As atcTimeseries In lstationTSGroup
                    lParmCd = lTS.Attributes.GetValue("parm_cd")
                    lStatCdStr = lTS.Attributes.GetValue("statistic")
                    .FieldName(lFieldInd) = lDD_cd & "_" & lParmCd & "_" & lStatCdStr
                    .FieldType(lFieldInd) = "N" : .FieldLength(lFieldInd) = lFieldLengthValue
                    lFieldInd += 1
                Next
                .NumRecords = lstationTSGroup(0).Dates.numValues '(lLastEnd - lFirstStart) / (JulianHour * 24) + 5 'assume daily here
                .CurrentRecord = 1
                Dim lagency As String = lstationTSGroup(0).Attributes.GetValue("agency", "")
                Dim location As String = lstationTSGroup(0).Attributes.GetValue("Location", "00000000")
                Dim lDates(5) As Integer
                For I As Integer = 0 To .NumRecords - 1
                    .Value(1) = lagency
                    .Value(2) = location
                    J2Date(lstationTSGroup(0).Dates.Value(I), lDates)
                    Dim ldatetime As String = lDates(0) & "-" & lDates(1).ToString.PadLeft(2, "0") & "-" & lDates(2).ToString.PadLeft(2, "0")
                    .Value(3) = ldatetime
                    lFieldInd = 4
                    Dim lFoundValueThisRecord As Boolean = False
                    For Each lTS As atcTimeseries In lstationTSGroup
                        .Value(lFieldInd) = DoubleToString(lTS.Value(I + 1), lFieldLengthValue, "0.00")
                        If Not lFoundValueThisRecord AndAlso .Value(lFieldInd) <> "NaN" Then lFoundValueThisRecord = True
                        lFieldInd += 1
                    Next
                    If lFoundValueThisRecord Then
                        .CurrentRecord += 1
                    End If
                Next
                .NumRecords = .CurrentRecord - 1

                Dim lHeaderAtts As String = AsRDBHeader(lstationTSGroup)
                Dim lOneLineWarning As String = ""
                lOneLineWarning = "# File-format description:  http://waterdata.usgs.gov/nwis/?tab_delimited_format_info" & Environment.NewLine
                lOneLineWarning &= "# Automated-retrieval info: http://help.waterdata.usgs.gov/faq/automated-retrievals" & Environment.NewLine
                lOneLineWarning &= "# " & Environment.NewLine
                lOneLineWarning &= "# retrieved: " & lstationTSGroup(0).Attributes.GetValue("retrieved", "") & Environment.NewLine
                Dim lOneLineExtra As String
                lOneLineExtra = "# Data provided for site " & location & Environment.NewLine
                lOneLineExtra &= "#    DD parameter statistic   Description" & Environment.NewLine
                'lOneLineExtra &= "#    02   00000     00000     As described by the column header below" & Environment.NewLine
                For Each lTS As atcTimeseries In lstationTSGroup
                    Dim lOneLineParmStr As String = ""
                    With lTS.Attributes
                        lParmCd = .GetValue("parm_cd")
                        lStatCdStr = .GetValue("statistic")
                        lCons = .GetValue("Constituent")
                    End With
                    lOneLineParmStr = "#" & lDD_cd.PadLeft(6, " ") & lParmCd.PadLeft(8, " ") & lStatCdStr.PadLeft(10, " ") & "     " & lCons
                    lOneLineParmStr = lOneLineParmStr.PadRight(55, " ") 'has to be more than 50 letters long for reading back
                    lOneLineExtra &= lOneLineParmStr & Environment.NewLine
                Next
                lOneLineExtra &= "#" & Environment.NewLine
                lOneLineExtra &= "# Data-value qualification codes included in this output: " & Environment.NewLine
                Dim lValueAttNamePrefix As String = "ValueAttributeDescription_"
                For Each lAtt As atcDefinedValue In lstationTSGroup(0).Attributes
                    Dim lName As String = lAtt.Definition.Name
                    If lName.ToLower().StartsWith(lValueAttNamePrefix.ToLower()) Then
                        lOneLineExtra &= "#     " & lName.ToString.Substring(lValueAttNamePrefix.Length) & "  " & lAtt.Value & Environment.NewLine
                    End If
                Next
                lOneLineExtra &= "#" ' & Environment.NewLine

                'Dim lOneLineColumnHeader As String = ""
                'For I As Integer = 1 To .NumFields
                '    lOneLineColumnHeader &= .FieldName(I) & vbTab
                'Next
                'lOneLineColumnHeader = lOneLineColumnHeader.TrimEnd(vbTab) & Environment.NewLine
                'For I As Integer = 1 To .NumFields
                '    lOneLineColumnHeader &= .FieldLength(I) & .FieldType(I) & vbTab
                'Next

                lHeaderAtts &= lOneLineWarning
                lHeaderAtts &= lOneLineExtra
                .Header = lHeaderAtts
            End With
            lRDBTables.Add(lKeyLoc, lRDBTable)
        Next
        Return lRDBTables
    End Function

    Private Function AsRDBHeader(ByVal aTSGroup As atcTimeseriesGroup) As String
        Dim lAttributes As New atcCollection()
        Dim lName As String
        Dim lValue As String
        For Each lAtt As atcDefinedValue In aTSGroup(0).Attributes
            If lAtt.Definition.Calculated Then Continue For
            lName = lAtt.Definition.Name
            lName = LookupParmName(lName)
            If lName.ToLower().Contains("timeseries") Then Continue For
            If Not lAttributes.Keys.Contains(lName) Then
                lAttributes.Add(lName, lAtt)
            End If
        Next

        For Each lTS As atcTimeseries In aTSGroup
        Next

        Dim lHeaderString As New Text.StringBuilder()

        Dim lOneLineInHeader As String
        For Each lAttName As String In lAttributes.Keys
            Dim lAtt As atcDefinedValue = lAttributes.ItemByKey(lAttName)
            lValue = lAtt.Value.ToString()
            If lAttName.ToLower.StartsWith("http") OrElse _
               lAttName.ToLower.StartsWith("url") Then
                lOneLineInHeader = "# " & lValue
            ElseIf lAttName.ToLower.StartsWith("download") Then
                lOneLineInHeader = "# " & lAttName & " " & lValue.Trim()
            ElseIf lAttName.ToLower.StartsWith("valueattributedescription") Then
                Continue For
            Else
                lOneLineInHeader = "# " & lAttName
                lOneLineInHeader = lOneLineInHeader.PadRight(50, " ")
                lOneLineInHeader &= lValue.Trim()
            End If
            lHeaderString.AppendLine(lOneLineInHeader)
        Next

        Return lHeaderString.ToString()
    End Function

    Private Function LookupParmName(ByVal aName As String) As String
        Dim lName As String = aName.Trim().ToLower()
        Dim lLookupTable As New atcCollection()
        With lLookupTable
            .Add("agency_cd", "agency")
            .Add("download_date", "retrieved")
            .Add("site_no", "location")
            .Add("station_nm", "stanam")
            .Add("dec_lat_va", "latitude")
            .Add("dec_long_va", "longitude")
            .Add("state_cd", "stfips")
            .Add("county_cd", "cntyfips")
            .Add("huc_cd", "hucode")
            .Add("drain_area_va", "drainage area")
        End With

        If lLookupTable.Keys.Contains(lName) Then
            If Not lName = "site_no" Then Return lLookupTable.ItemByKey(lName)
        Else
            For Each lKey As String In lLookupTable.Keys
                If lLookupTable.ItemByKey(lKey) = lName Then
                    Return lKey
                End If
            Next
        End If

        Return lName
    End Function

    Public Sub SaveAsTimeseries(ByVal aSaveFileName As String)
        'Dim lTimeseries As atcTimeseries = Me.DataSets(0)
        'Dim lWriter As New System.IO.StreamWriter(aSaveFileName)

        Dim lInterval As Double = 0
        'Dim lInterval As Double = lTimeseries.Attributes.GetValue("Interval", JulianHour)
        Dim lDatasetsToWrite As atcTimeseriesGroup = Me.DataSets ' New atcTimeseriesGroup(lTimeseries)
        'For Each lTimeseries In Me.DataSets
        '    If lTimeseries.Attributes.GetValue("Interval", JulianHour) <> lInterval Then
        '        Logger.Msg("Different interval data cannot be written to same file, skipping " & lTimeseries.ToString & " - " & DoubleToString(lTimeseries.Attributes.GetValue("Interval", JulianHour) * 24) & " hours <> " & DoubleToString(lInterval * 24))
        '    ElseIf lTimeseries.Dates.numValues < lLastTimeStep Then
        '        Logger.Msg("Different number of values cannot be written to same file, skipping " & lTimeseries.ToString & " which contains " & lTimeseries.Dates.numValues & " values instead of " & lLastTimeStep)
        '    Else
        '        lDatasetsToWrite.Add(lTimeseries)
        '    End If
        'Next

        'Dim lDelimiter As String = " "
        'Dim lIndex As Integer = 0
        'For Each lTimeseries As atcTimeseries In lDatasetsToWrite
        '    lIndex += 1
        '    Dim lLastTimeStep As Integer = lTimeseries.Dates.numValues
        '    lWriter.WriteLine(TimeseriesStart)
        '    lWriter.WriteLine(lTimeseries.Attributes.GetValue("Description", "Timeseries " & lIndex))
        '    If lTimeseries.Dates.Value(1) > JulianYear Then
        '        lWriter.WriteLine(TimeseriesAbsolute)
        '    Else
        '        lWriter.WriteLine(TimeseriesRelative)
        '    End If


        '    For lTimeStep As Integer = 1 To lLastTimeStep
        '        Dim lDateArray(5) As Integer
        '        modDate.J2Date(lTimeseries.Dates.Value(lTimeStep) - lInterval, lDateArray)


        '        lWriter.Write(Format(lDateArray(0), "0000") & _
        '                      lDelimiter & lDateArray(1) & _
        '                      lDelimiter & lDateArray(2) & _
        '                      lDelimiter & lDateArray(3) & _
        '                      lDelimiter & lDateArray(4))

        '        lWriter.Write(lDelimiter & DoubleToString(lTimeseries.Value(lTimeStep)))
        '        lWriter.WriteLine()
        '    Next
        '    lWriter.WriteLine(TimeseriesEnd)
        'Next
        'lWriter.Close()
    End Sub

    Public Overrides Function Open(ByVal aFileName As String, _
                          Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        'If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
        '    aFileName = FindFile("Select " & Name & " file to open", , , Filter, True, , 1)
        'End If
        'If Not IO.File.Exists(aFileName) Then
        '    Throw New ApplicationException("File '" & aFileName & "' not found")

        If MyBase.Open(aFileName, aAttributes) Then
            If Not IO.File.Exists(Specification) Then
                Logger.Dbg("Opening new file " & Specification)
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
                    Dim lDailyDischargeData As Boolean = False

                    Dim lAttributes As New atcDataAttributes
                    Dim lDefFlagFormat As String = "IsNewParamFormat"
                    Dim lDefStartDefinition As String = "StartDefinition"
                    lAttributes.SetValue(lDefFlagFormat, True)
                    lAttributes.SetValue(lDefStartDefinition, 30)
                    Dim lCurLine As String

                    Dim lAttrName As String
                    Dim lAttrValue As String
                    Dim lFirstLine As Boolean = True

                    While lInputReader.PeekChar = 35 ' Asc("#")
                        lCurLine = NextLine(lInputReader)

                        If lCurLine.Contains("Data for the following station is contained in this file") Then
                            With lAttributes
                                If Not .ContainsAttribute("AGENCY") Then 'Only parse this line if attributes not already set
                                    lCurLine = NextLine(lInputReader)
                                    If lCurLine.Contains("----------") Then
                                        lCurLine = NextLine(lInputReader).Substring(1).Trim
                                        .SetValue("AGENCY", MapWinUtility.Strings.StrSplit(lCurLine, " ", ""))
                                        .SetValue("Location", MapWinUtility.Strings.StrSplit(lCurLine, " ", ""))
                                        .SetValue("StaNam", lCurLine.Trim)
                                    End If
                                End If
                            End With
                        End If

                        If lFirstLine AndAlso lCurLine.Contains("http://") Then 'This looks like a file we downloaded and prepended the original URL
                            If lCurLine.Contains("qwdata") Then
                                lWQData = True
                            ElseIf lCurLine.Contains("measurement") Then
                                lMeasurementsData = True
                            ElseIf lCurLine.Contains("gwlevels") Then
                                lPeriodicGroundwaterData = True
                            ElseIf lCurLine.Contains("dv?") OrElse lCurLine.Contains("provisional") Then
                                lDailyDischargeData = True
                            ElseIf lCurLine.Contains("ida.water.usgs.gov") Then
                                lIdaData = True
                            End If
                            If Not lAttributes.ContainsAttribute("URL") Then
                                lAttributes.SetValueIfMissing("URL", lCurLine.Substring(lCurLine.IndexOf("http://")).Trim)
                            End If
                        ElseIf lCurLine.Contains("Site File") Then
                            lSite = True
                            Exit While
                        ElseIf lCurLine.Contains("File created on") Then
                            lAttributes.SetValue("retrieved", lCurLine.Substring(lCurLine.IndexOf("File created on") + 15).Trim)
                        ElseIf lCurLine.ToLower.Contains("retrieved:") Then
                            lAttributes.SetValue("retrieved", lCurLine.Substring(lCurLine.ToLower.IndexOf("retrieved:") + 10).Trim)
                        ElseIf lCurLine.ToLower.Contains("download_date") Then
                            lAttributes.SetValue("retrieved", lCurLine.Substring(lCurLine.ToLower.IndexOf("download_date") + 13).Trim)
                        ElseIf lCurLine.Contains("contains selected water-quality data") Then
                            lWQData = True
                            Exit While
                        ElseIf lCurLine.Contains("instantaneous data archive") Then
                            lIdaData = True
                            Exit While
                        ElseIf lCurLine.Contains("DD parameter statistic") Then
                            lDailyDischargeData = True
                            lAttributes.SetValue(lDefFlagFormat, False)
                            Exit While
                        ElseIf lCurLine.Replace(" ", "").ToLower().Contains("tsparameterstatistic") Then
                            lDailyDischargeData = True
                            lAttributes.SetValue(lDefStartDefinition, lCurLine.IndexOf("Description", StringComparison.OrdinalIgnoreCase))
                            Exit While
                        ElseIf lCurLine.Replace(" ", "").ToLower().Contains("ts_idparameterstatistic") Then
                            lDailyDischargeData = True
                            lAttributes.SetValue(lDefStartDefinition, lCurLine.IndexOf("Description", StringComparison.OrdinalIgnoreCase))
                            Exit While
                        ElseIf lCurLine.Contains("Surface water measurements") Then
                            lMeasurementsData = True
                            Exit While
                        ElseIf lMeasurementsData AndAlso lCurLine.Contains("Stations in this file include") Then
                            Exit While
                        ElseIf lCurLine.Contains("groundwater levels") Then
                            lPeriodicGroundwaterData = True
                            Exit While
                        End If

                        If lCurLine.Length > 50 Then
                            lAttrName = lCurLine.Substring(2, 48).Trim
                            lAttrValue = lCurLine.Substring(50).Trim
                            If lAttrName.Length > 0 AndAlso lAttrName.Length < 30 Then
                                Select Case lAttrName 'translate NWIS attributes to WDM/BASINS names
                                    Case "site_no"
                                        lAttributes.SetValue("STAID", lAttrValue)
                                        lAttributes.SetValue("Location", lAttrValue)
                                    Case "dec_lat_va"
                                        Try
                                            lAttributes.SetValue("Latitude", CDbl(lAttrValue))
                                        Catch
                                            Logger.Dbg("Could not parse Latitude " & lAttrValue)
                                        End Try
                                    Case "dec_long_va"
                                        Try
                                            lAttributes.SetValue("Longitude", -Math.Abs(CDbl(lAttrValue)))
                                        Catch
                                            Logger.Dbg("Could not parse Longitude " & lAttrValue)
                                        End Try
                                    Case Else
                                        If lAttrName.Length > 0 Then
                                            Select Case lAttrValue
                                                Case "", "-", "--" 'Skip setting non-values
                                                Case Else
                                                    lAttributes.SetValue(lAttrName, lAttrValue)
                                            End Select
                                        End If
                                End Select
                            End If
                        End If
                        lFirstLine = False
                    End While

                    lAttributes.AddHistory("Read from " & Specification)

                    If lSite Then
                        Throw New ApplicationException("Station list does Not contain timeseries data: " & IO.Path.GetFileName(Specification))
                    ElseIf lWQData Then
                        ProcessWaterQualityValues(lInputReader, lAttributes)
                    ElseIf lMeasurementsData Then
                        ProcessMeasurements(lInputReader, lAttributes)
                    ElseIf lIdaData Then
                        ProcessIdaValues(lInputReader, lAttributes)
                    ElseIf lPeriodicGroundwaterData Then
                        ProcessPeriodicGroundwater(lInputReader, lAttributes)
                    Else 'If lDailyDischargeData Then
                        ProcessDailyValues(lInputReader, lAttributes)
                    End If

                    Try
                        If lInputReader IsNot Nothing Then lInputReader.Close()
                        If lInputBuffer IsNot Nothing Then lInputBuffer.Close()
                        If lInputStream IsNot Nothing Then lInputStream.Close()
                    Catch
                    End Try

                    Return True
                Catch lException As Exception
                    Throw New ApplicationException("Exception reading '" & Specification & "': " & lException.Message, lException)
                End Try
            End If
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
                                            Dim lAttributeValue As String = .Value(lField).Trim
                                            If lAttributeValue.Length > 0 Then
                                                lBuilders(lValueFieldIndex).AddValueAttribute(.FieldName(lField), lAttributeValue)
                                            End If
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

    Sub ProcessPeriodicGroundwater(ByVal aInputReader As BinaryReader, ByVal aAttributes As atcDataAttributes)
        Dim lStationsRDB As New atcTableRDB
        With lStationsRDB
            If .OpenStream(aInputReader.BaseStream) Then
                Dim lDateField As Integer = .FieldNumber("lev_dt")
                If lDateField = 0 Then Throw New Exception("Required field missing: lev_dt")
                Dim lTimeField As Integer = .FieldNumber("lev_tm")

                Dim lValueFieldNames() As String = {"lev_va", "sl_lev_va"}
                Dim lConstituentNames() As String = {"GW LEVEL", "GW LEVEL"}
                Dim lDescriptions() As String = {"Water level value in feet below land surface", "Water level value in feet above specific vertical datum"}
                Dim lUnits() As String = {"ft", "ft"}
                Dim lLastValueField As Integer = lValueFieldNames.GetUpperBound(0)
                Dim lValueFieldNumber(lLastValueField) As Integer
                Dim lBuilders(lLastValueField) As atcTimeseriesBuilder
                Dim lValueFieldIndex As Integer

                For lValueFieldIndex = 0 To lLastValueField
                    lValueFieldNumber(lValueFieldIndex) = .FieldNumber(lValueFieldNames(lValueFieldIndex))
                    If lValueFieldNumber(lValueFieldIndex) = 0 Then
                        Logger.Dbg("Missing field: " & lValueFieldNames(lValueFieldIndex))
                    Else
                        lBuilders(lValueFieldIndex) = New atcTimeseriesBuilder(Me)
                        With lBuilders(lValueFieldIndex).Attributes
                            .ChangeTo(aAttributes)
                            .SetValue("Constituent", lConstituentNames(lValueFieldIndex))
                            .SetValue("Units", lUnits(lValueFieldIndex))
                            .SetValue("Point", True)
                            .SetValue("Scenario", "OBSERVED")
                            .SetValue("Location", .GetValue("site_no"))
                            .SetValue("Description", lDescriptions(lValueFieldIndex))
                            .SetValue("parm_cd", lValueFieldNames(lValueFieldIndex))
                            .SetValue("ID", lValueFieldIndex + 1)
                        End With
                    End If
                Next

                Dim lDateString As String
                Dim lTimeString As String
                Dim lDate As Date
                Dim lValueString As String
                Dim lValue As Double
                For lRecord As Integer = 1 To .NumRecords
                    .CurrentRecord = lRecord
                    lDateString = .Value(lDateField)
                    If lTimeField > 0 Then
                        lTimeString = .Value(lTimeField).Trim
                        If lTimeString.Contains(":") AndAlso lTimeString.Length = 5 Then
                            lDateString &= " " & lTimeString
                        ElseIf lTimeString.Length = 4 Then
                            lDateString &= " " & lTimeString.Substring(0, 2) & ":" & lTimeString.Substring(2, 2)
                        End If
                    End If
                    If Date.TryParse(lDateString, lDate) Then
                        For lValueFieldIndex = 0 To lLastValueField
                            If lValueFieldNumber(lValueFieldIndex) > 0 Then
                                lValueString = .Value(lValueFieldNumber(lValueFieldIndex))
                                If Double.TryParse(lValueString, lValue) Then
                                    lBuilders(lValueFieldIndex).AddValue(lDate, lValue)
                                    For lField As Integer = 1 To .NumFields
                                        If Array.IndexOf(lValueFieldNumber, lField) < 0 Then 'Not a value field, add it as value attribute
                                            Select Case .FieldName(lField)
                                                Case "agency_cd", "site_no", "lev_dt", "lev_tm", "lev_agency_cd" 'don't need these as value attributes
                                                Case Else
                                                    Dim lAttributeValue As String = .Value(lField).Trim
                                                    If lAttributeValue.Length > 0 Then
                                                        lBuilders(lValueFieldIndex).AddValueAttribute(.FieldName(lField), lAttributeValue)
                                                    End If
                                            End Select
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
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
                            lData.Attributes.SetValue("Description", lConstituentDescription)
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
        Dim lDDTS As String
        Dim lParmCode As String
        Dim lStatisticCode As String
        Dim lQualificationCode As String
        Dim lQualificationCodes As New atcCollection
        Dim lConstituentDescriptions As New atcCollection
        Dim lArr() As String

        While aInputReader.PeekChar = Asc("#")
            lCurLine = NextLine(aInputReader)
            If lCurLine.Length > 50 Then
                'Remember extended column labels
                lArr = Regex.Split(lCurLine, "\s+")
                lDDTS = lArr(1)
                lParmCode = lArr(2)
                lStatisticCode = lArr(3)
                'If aAttributes.GetValue("IsNewParamFormat", False) Then
                '    lDDTS = lCurLine.Substring(9, 6)
                '    lParmCode = lCurLine.Substring(22, 5)
                '    lStatisticCode = lCurLine.Substring(32, 5)
                'Else
                '    lDDTS = lCurLine.Substring(5, 2)
                '    lParmCode = lCurLine.Substring(10, 5)
                '    lStatisticCode = lCurLine.Substring(20, 5)
                'End If
                If IsNumeric(lParmCode) AndAlso IsNumeric(lStatisticCode) Then
                    'lConstituentDescriptions.Add(lCurLine.Substring(5, 2) & "_" & lParmCode & "_" & lStatisticCode, lCurLine.Substring(30).Trim)
                    If aAttributes IsNot Nothing Then
                        lConstituentDescriptions.Add(lDDTS & "_" & lParmCode & "_" & lStatisticCode, lCurLine.Substring(aAttributes.GetValue("StartDefinition", 30)).Trim)
                    Else
                        lConstituentDescriptions.Add(lDDTS & "_" & lParmCode & "_" & lStatisticCode, lCurLine.Substring(30).Trim)
                    End If
                End If
            End If
            If lCurLine.Length > 10 Then
                lQualificationCode = lCurLine.Substring(1, 7).Trim
                If lQualificationCode.Length = 1 Then
                    lQualificationCodes.Add(lQualificationCode, lCurLine.Substring(9).Trim)
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
            Dim lCurValue As Double = 0
            .Delimiter = vbTab
            .OpenStream(aInputReader.BaseStream)

            For lField = 1 To .NumFields
                Select Case .FieldName(lField)
                    Case "agency_cd"
                    Case "site_no" : lLocationField = lField
                    Case "datetime" : lDateField = lField
                    Case Else
                        If .FieldName(lField).EndsWith("_cd") Then 'code field
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

            Dim lParsedDate As Date
            While lTable.CurrentRecord < lTable.NumRecords
                lTable.MoveNext()
                If Date.TryParse(.Value(lDateField), lParsedDate) Then
                    lDate = lParsedDate.ToOADate() + pJulianInterval 'add one interval to put date at end of interval
                    lLocation = .Value(lLocationField)
                    For Each lField In lValueFields
                        If Double.TryParse(.Value(lField).Trim, lCurValue) Then
                            'If next field is code for this field, then make sure its code is in the allowed codes, QualificationCodes
                            'This test was for skipping provisional values: 'If .FieldName(lField + 1) <> .FieldName(lField) & "_cd" OrElse QualificationCodes.Contains(.Value(lField + 1).Trim().Substring(0, 1)) Then

                            lQualificationCode = ""
                            If .FieldName(lField + 1) = .FieldName(lField) & "_cd" Then
                                lQualificationCode = .Value(lField + 1).Trim.Replace(":", "")
                            End If

                            lConstituentDescription = lValueConstituentDescriptions.ItemByKey(lField)

                            Dim lDataKey As String = lLocation & ":" & lConstituentDescription
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
                                lData.numValues = lTable.NumRecords - 1
                                lArr = .FieldName(lField).Split("_")
                                Dim lParmCd As String = lArr(1) '.FieldName(lField).Substring(3, 5)
                                Dim lConstituent As String = lParmCd
                                Dim lUnits As String = Nothing
                                Select Case lConstituent
                                    Case "00045"
                                        Try
                                            If System.Reflection.Assembly.GetEntryAssembly.Location.Contains("USGSToolbox") Then
                                                lConstituent = "Precipitation" : lUnits = "inches"
                                            Else
                                                lConstituent = "PREC" : lUnits = "in"
                                            End If
                                        Catch
                                            lConstituent = "PREC" : lUnits = "in"
                                        End Try
                                    Case "00060"
                                        Try
                                            If System.Reflection.Assembly.GetEntryAssembly.Location.Contains("USGSToolbox") Then
                                                lConstituent = "Streamflow" : lUnits = "cubic feet per second"
                                            Else
                                                lConstituent = "FLOW" : lUnits = "cfs"
                                            End If
                                        Catch
                                            lConstituent = "FLOW" : lUnits = "cfs"
                                        End Try

                                    Case "61055" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Water level, depth below measuring point, feet 
                                    Case "62611" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Groundwater level above NAVD 1988, feet 
                                    Case "72019" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Depth to water level, feet below land surface 
                                    Case "72020" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Elevation above NGVD 1929, feet 
                                    Case "72150" : lConstituent = "GW LEVEL" : lUnits = "feet" 'Groundwater level relative to Mean Sea Level (MSL), feet.
                                    Case Else
                                        lConstituent = lParmCd
                                End Select
                                lData.Attributes.SetValue("parm_cd", lParmCd)
                                lData.Attributes.SetValue("Constituent", lConstituent)
                                lData.Attributes.SetValue("Description", lConstituentDescription)

                                If lUnits Is Nothing Then
                                    If lConstituentDescription.ToLower.Contains("cubic feet per second") Then
                                        lUnits = "cubic feet per second"
                                    ElseIf lConstituentDescription.Contains("feet") Then
                                        lUnits = "feet"
                                    End If
                                End If

                                If lUnits IsNot Nothing Then
                                    Try
                                        If System.Reflection.Assembly.GetEntryAssembly.Location.Contains("USGSToolbox") Then
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
                                    Catch ex As Exception
                                        Select Case lUnits
                                            Case "feet" : lUnits = "ft"
                                            Case "cubic feet per second" : lUnits = "cfs"
                                        End Select
                                    End Try
                                    lData.Attributes.SetValue("Units", lUnits)
                                End If

                                lStatisticCode = lArr(2) 'SafeSubstring(.FieldName(lField), 9, 5)
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
                                lData.Value(lTSIndex) = lCurValue
                                lData.Dates.Value(lTSIndex) = lDate
                                For Each lCodeChar As String In lQualificationCode
                                    lData.ValueAttributes(lTSIndex).Add(lCodeChar, True)
                                    Dim lAttributeName As String = "ValueAttributeDescription_" & lCodeChar
                                    If Not lData.Attributes.ContainsAttribute(lAttributeName) AndAlso lQualificationCodes.Keys.Contains(lCodeChar) Then
                                        lData.Attributes.SetValue(lAttributeName, lQualificationCodes.ItemByKey(lCodeChar))
                                    End If
                                Next
                                lData.Attributes.SetValue("Count", lTSIndex)
                        End If
                    Next
                End If
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
    End Sub

    ''' <summary>
    ''' build a timeseries from IDA flow values in an USGS RDB file
    ''' </summary>
    ''' <param name="aInputReader">Binary reader</param>
    ''' <param name="aAttributes">Generic attributes</param>
    ''' <remarks>flow timeseries added to flow data</remarks>
    Sub ProcessIdaValues(ByVal aInputReader As BinaryReader, ByVal aAttributes As atcDataAttributes)
        Dim lTimeStart As Date = Now
        Dim lNaN As Double = GetNaN()
        Logger.Status("Opening IDA Data")
        Dim lTimeseries As New atcTimeseries(Me)
        lTimeseries.Dates = New atcTimeseries(Me)

        Dim lCurLine As String
        While aInputReader.PeekChar = Asc("#")
            lCurLine = NextLine(aInputReader)
        End While

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
                        'If .FieldName(lField).EndsWith("_cd") Then 'code field
                            ' TODO: add codes as ValueAttributes
                        'End If
                End Select
            Next
            lTimeseries.numValues = .NumRecords - 1
            lTimeseries.Dates.Value(0) = lNaN

            Dim lAccuracyCodeCounter As New atcCollection
            Dim lValue As String = ""
            Dim lDateJ As Double = lNaN
            Dim lDatePrevJ As Double = lDateJ
            Dim lTimeZone As String = ""
            Dim lTimeZonePrev As String
            Dim lIndex As Integer = 0
            Dim lTimeZoneNumeric As Double = lNaN 'Hours offset from UTC
            Dim lTimeZoneOfFirstValue As Double = lNaN
            Dim lTimeDifTotals As New atcCollection

            Logger.Status("Reading IDA Values")

            For lRecordIndex As Integer = 2 To .NumRecords
                .CurrentRecord = lRecordIndex
                lDatePrevJ = lDateJ
                Try
                    Dim lDateString As String = .Value(lDateField)
                    lTimeZonePrev = lTimeZone
                    lTimeZone = .Value(lTimeZoneField)

                    If lTimeZone <> lTimeZonePrev Then
                        Select Case lTimeZone
                            Case "AST", "EDT" : lTimeZoneNumeric = -4
                            Case "EST", "CDT" : lTimeZoneNumeric = -5
                            Case "CST", "MDT" : lTimeZoneNumeric = -6
                            Case "MST", "PDT" : lTimeZoneNumeric = -7
                            Case "PST" : lTimeZoneNumeric = -8
                            Case "GMT", "UTC" : lTimeZoneNumeric = 0
                            Case Else
                                Logger.Dbg("UnknownTimeZone " & lTimeZone)
                        End Select
                        If lRecordIndex = 2 Then
                            lTimeZoneOfFirstValue = lTimeZoneNumeric
                        Else
                            Logger.Dbg("ChangeTimeZoneAt " & lDateString & " from " & lTimeZonePrev & " to " & lTimeZone)
                        End If
                    End If

                    Dim lDate As New Date(lDateString.Substring(0, 4), lDateString.Substring(4, 2), lDateString.Substring(6, 2), _
                                          lDateString.Substring(8, 2), lDateString.Substring(10, 2), lDateString.Substring(12, 2))
                    lDateJ = lDate.ToOADate

                    If lDateJ = 0 Then
                        Logger.Dbg("Bad Date " & .CurrentRecord.ToString)
                    Else
                        If lTimeZoneNumeric <> lTimeZoneOfFirstValue Then
                            lDateJ += (lTimeZoneOfFirstValue - lTimeZoneNumeric) / 24
                        End If

                        lIndex += 1
                        lTimeseries.Value(lIndex) = .Value(lValueField)
                        lTimeseries.Dates.Value(lIndex) = lDateJ

                        If lRecordIndex > 3 Then
                            lTimeDifTotals.Increment(lDateJ - lDatePrevJ)
                        End If
                        'Dim lAccuracyCode As String = .Value(lAccuracyCodeField)
                        'lAccuracyCodeCounter.Increment(lAccuracyCode)
                        'NOTE: the next statement adds a requirement for about 3.5 times more memory
                        'lTimeseries.ValueAttributes(lIndex).Add("AccuracyCode", lAccuracyCode)
                    End If
                    If .CurrentRecord Mod 1000 = 0 Then
                        Logger.Progress(.CurrentRecord, .NumRecords)
                    End If
                Catch e As Exception
                    Logger.Dbg("Exception processing IDA file " & Specification & " record " & .CurrentRecord & " = " & .CurrentRecordAsDelimitedString & vbCrLf & e.ToString)
                End Try
            Next
            'Logger.Dbg("AccuracyCodeCounts")
            'For lIndex = 0 To lAccuracyCodeCounter.Count - 1
            '    Logger.Dbg(lAccuracyCodeCounter.Keys(lIndex) & ":" & lAccuracyCodeCounter.Item(lIndex))
            'Next

            'Dim lMostCommonTimeDif As Double = 15 * JulianMinute
            'Dim lMostCommonTimeDifCount As Integer = 0

            'For lTimeDifIndex As Integer = 0 To lTimeDifTotals.Count - 1
            '    If lTimeDifTotals(lTimeDifIndex) > lMostCommonTimeDifCount Then
            '        lMostCommonTimeDifCount = lTimeDifTotals(lTimeDifIndex)
            '        lMostCommonTimeDif = lTimeDifTotals.Keys(lTimeDifIndex)
            '    End If
            'Next

            'Dim lTu As atcTimeUnit = atcTimeUnit.TUUnknown
            'Dim lTs As Integer = 1

            'If lMostCommonTimeDifCount > 0 Then
            '    CalcTimeUnitStep(0, lMostCommonTimeDif, lTu, lTs)
            'Else
            '    lTu = atcTimeUnit.TUMinute
            '    lTs = 15
            'End If

            Logger.Dbg("IdaValuesTimeseriesCreated;Elapsed seconds " & (Now - lTimeStreamOpened).TotalSeconds & " " & MemUsage())
            Logger.Progress("", 0, 0)

            'lTimeseries = FillValues(lTimeseries, lTu, lTs, lNaN, lNaN, lNaN, Me)
            With lTimeseries.Attributes
                .ChangeTo(aAttributes)
                .SetValue("Constituent", "Flow")
                .SetValue("Scenario", "Observed")
                '.SetValue("tu", lTu)
                '.SetValue("ts", lTs)
                .SetValue("Point", True) 'FillValues sets this to False, but really this is instantaneous/point data
                If Not Double.IsNaN(lTimeZoneOfFirstValue) Then
                    .SetValue("TMZONE", lTimeZoneOfFirstValue)
                End If
            End With

            Me.AddDataSet(lTimeseries)
        End With
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

