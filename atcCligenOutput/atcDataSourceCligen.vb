Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports atcMetCmp

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.IO

Public Class atcDataSourceCligen
    Inherits atcTimeseriesSource
    '##MODULE_REMARKS Copyright 2005 AQUA TERRA Consultants - Royalty-free use permitted under open source license

    Private pAvailableOperations As atcDataAttributes ' atcDataGroup
    Private Shared pFileFilter As String = "Cligen Output Files (*.dat)|*.dat"
    Private pErrorDescription As String
    Private pColDefs As Hashtable
    'Private pReadAll As Boolean = False

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Cligen Output"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::Cligen Output"
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

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aArgs As atcData.atcDataAttributes = Nothing) As Boolean
        Dim lData As atcTimeseries

        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not FileExists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , pFileFilter, True, , 1)
        End If

        If Not FileExists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
        Else
            Me.Specification = aFileName

            Dim lIncludeDaily As Boolean = True
            Dim lIncludeHourly As Boolean = False
            If Not aArgs Is Nothing Then
                lIncludeDaily = aArgs.GetValue("Include Daily")
                lIncludeHourly = aArgs.GetValue("Include Hourly")
            End If

            Try
                Dim lDailyTimeseriesGroup As New atcTimeseriesGroup
                Dim lTable As New atcTableFixed
                Dim lSCol() As Integer = {0, 2, 5, 8, 12, 18, 24, 29, 36, 42, 48, 53, 58, 64}
                Dim lFLen() As Integer = {0, 2, 2, 4, 6, 6, 5, 7, 6, 6, 5, 5, 6, 6}
                Dim lFldNames() As String = {"", "da", "mo", "year", "prcp", "dur", "tp", "ip", _
                                             "tmax", "tmin", "rad", "w-vl", "w-dir", "tdew"}
                Dim lTSKey As String
                Dim lTSIndex As Integer
                Dim lLocation As String = IO.Path.GetFileNameWithoutExtension(aFileName)
                Dim lConstituentCode As Integer = -1
                Dim lJDate As Double
                Dim lDate() As Integer = {0, 0, 0, 24, 0, 0}
                Dim i As Integer
                With lTable
                    .NumFields = 13
                    .NumHeaderRows = 15
                    For i = 1 To .NumFields
                        .FieldName(i) = lFldNames(i)
                        .FieldLength(i) = lFLen(i)
                        .FieldStart(i) = lSCol(i)
                    Next
                    If lTable.OpenFile(aFileName) Then
                        For lRecordNumber As Integer = 1 To lTable.NumRecords
                            lTable.CurrentRecord = lRecordNumber
                            For i = 4 To .NumFields
                                lTSKey = .FieldName(i)
                                lData = lDailyTimeseriesGroup.ItemByKey(lTSKey)
                                If lData Is Nothing Then
                                    lData = New atcTimeseries(Me)
                                    lData.Dates = New atcTimeseries(Me)
                                    lData.numValues = .NumRecords
                                    lData.Value(0) = GetNaN()
                                    lData.Dates.Value(0) = GetNaN()
                                    lData.Attributes.SetValue("Count", 0)
                                    lData.Attributes.SetValue("Scenario", "CLIGEN")
                                    lData.Attributes.SetValue("Location", lLocation)
                                    lData.Attributes.SetValue("Constituent", .FieldName(i))
                                    lData.Attributes.SetValue("point", False)
                                    lData.Attributes.SetValue("tu", atcTimeUnit.TUDay)
                                    lData.Attributes.SetValue("ts", 1)
                                    lDailyTimeseriesGroup.Add(lTSKey, lData)
                                End If
                                lDate(0) = .Value(3)
                                lDate(1) = .Value(2)
                                lDate(2) = .Value(1)
                                lJDate = Date2J(lDate)
                                If lJDate <> 0 Then
                                    lTSIndex = lData.Attributes.GetValue("Count") + 1
                                    lData.Value(lTSIndex) = .Value(i)
                                    lData.Dates.Value(lTSIndex) = lJDate
                                    lData.Attributes.SetValue("Count", lTSIndex)
                                    If lTSIndex = 1 Then 'put start date in 0th position of date array
                                        lData.Dates.Value(0) = lJDate - 1
                                    End If
                                End If
                            Next i
                        Next lRecordNumber
                        For Each lData In lDailyTimeseriesGroup
                            lData.numValues = lData.Attributes.GetValue("Count")
                        Next
                        If lIncludeDaily Then
                            DataSets.AddRange(lDailyTimeseriesGroup)
                        End If
                        If lIncludeHourly Then
                            Dim lHourlyData As New atcTimeseriesGroup
                            lHourlyData = DisaggCliGen(lDailyTimeseriesGroup)
                            DataSets.AddRange(lHourlyData)
                        End If
                        Open = True
                    Else
                        Open = False
                        Logger.Msg("Unable to process Cligen file " & aFileName, "Cligen Open")
                    End If
                End With
            Catch endEx As EndOfStreamException
                Open = False
            End Try
        End If
    End Function

    Public Overrides ReadOnly Property AvailableOperations() As atcData.atcDataAttributes
        Get
            Dim lOperations As atcDataAttributes
            If Not pAvailableOperations Is Nothing Then
                lOperations = pAvailableOperations
            Else
                lOperations = New atcDataAttributes
                Dim lArguments As atcDataAttributes

                Dim defIncludeDaily As New atcAttributeDefinition
                With defIncludeDaily
                    .Name = "Include Daily"
                    .Description = "Include Daily data as output timeseries"
                    .DefaultValue = False
                    .Editable = True
                    .TypeString = "Boolean"
                End With

                Dim defIncludeHourly As New atcAttributeDefinition
                With defIncludeHourly
                    .Name = "Include Hourly"
                    .Description = "Include Hourly data as output timeseries"
                    .DefaultValue = False
                    .Editable = True
                    .TypeString = "Boolean"
                End With

                Dim lCliGenOutput As New atcAttributeDefinition
                With lCliGenOutput
                    .Name = "CliGen Output"
                    .Description = "Read CliGen output file"
                    .Editable = False
                    .TypeString = "atcTimeseries"
                    .Calculator = Me
                End With

                lArguments = New atcDataAttributes
                lArguments.SetValue(defIncludeDaily, Nothing)
                lArguments.SetValue(defIncludeHourly, Nothing)

                lOperations.SetValue(lCliGenOutput, Nothing, lArguments)
            End If

            Return lOperations
        End Get
    End Property

    Private Function parseWQObsDate(ByVal aDate As String, ByVal aTime As String) As Double
        'assume point values at specified time
        Dim d(5) As Integer 'date array
        Dim l As Integer 'Length of year (2 or 4 digit year)
        Dim i As Integer 'Year offset (1900 for 2-digit year)

        If Not IsNumeric(aTime) Then aTime = "1200" 'assume noon for missing obstime
        If IsNumeric(aDate) Then
            If Len(aDate) = 8 Then ' 4 dig yr
                l = 4
                i = 0
            Else
                l = 2
                i = 1900
            End If
            d(0) = Left(aDate, l) + i
            d(1) = Mid(aDate, l + 1, 2)
            d(2) = Right(aDate, 2)
            If IsNumeric(aTime) Then
                d(3) = Left(aTime, 2)
                d(4) = Right(aTime, 2)
            End If
            Return Date2J(d)
        Else
            Return 0
        End If
    End Function

    Private Function DisaggCliGen(ByVal aDailyData As atcTimeseriesGroup) As atcTimeseriesGroup

        Dim lDisTS As New atcMetCmp.atcMetCmpPlugin
        Dim lts As atcTimeseries

        Dim lArgsMet As New atcDataAttributes
        Dim lMatch As New atcTimeseriesGroup
        Dim lErr As String = ""
        Dim lStr As String = ""

        Logger.Dbg("CliGenUpdate:FindDataManager")
        Dim lTsMath As atcTimeseriesSource = New atcTimeseriesMath.atcTimeseriesMath
        Logger.Dbg("CliGenUpdate:TsMath: " & lTsMath.ToString)

        Dim lDataGroup As New atcTimeseriesGroup

        Dim lTMax As atcTimeseries = Nothing
        Dim lTMin As atcTimeseries = Nothing
        Dim lATmp As atcTimeseries = Nothing
        Dim lDewPt As atcTimeseries = Nothing
        Dim lPrecip As atcTimeseries = Nothing
        Dim lPrecDur As atcTimeseries = Nothing
        Dim lPrecPkTime As atcTimeseries = Nothing
        Dim lPrecPkInten As atcTimeseries = Nothing
        For Each lDS As atcTimeseries In aDailyData
            lStr = UCase(lDS.Attributes.GetValue("Constituent"))
            Select Case lStr
                Case "TMAX"
                    Logger.Dbg("Found CliGen TMax data")
                    lArgsMet.Clear()
                    lArgsMet.SetValue("Timeseries", lDS)
                    lTsMath.Open("Celsius to F", lArgsMet)
                    lTMax = lTsMath.DataSets(0)
                    Logger.Dbg("Converted CliGen TMax data to Deg F")
                Case "TMIN"
                    Logger.Dbg("Found CliGen TMin data")
                    lArgsMet.Clear()
                    lArgsMet.SetValue("Timeseries", lDS)
                    lTsMath.Open("Celsius to F", lArgsMet)
                    lTMin = lTsMath.DataSets(0)
                    Logger.Dbg("Converted CliGen TMax data to Deg F")
                Case "RAD"
                    Logger.Dbg("Using Daily Solar Radiation to calculate Daily Cloud Cover")
                    lArgsMet.Clear()
                    lArgsMet.SetValue("SRAD", lDS)
                    lArgsMet.SetValue("Latitude", "38.85") 'latitude of Washington National
                    lDisTS.Open("Cloud Cover from Solar", lArgsMet)
                    Logger.Dbg("Cloud Cover from Solar Radiation complete, now disaggregate it to Hourly")
                    lts = Aggregate(lDisTS.DataSets(0), atcTimeUnit.TUHour, 1, atcTran.TranAverSame)
                    Logger.Dbg("Disaggregated Daily Cloud Cover to Hourly")
                    lDataGroup.Add(lts)
                    Logger.Dbg("Disaggregating Daily Solar Radiation to Hourly")
                    lArgsMet.Clear()
                    lArgsMet.SetValue("SRAD", lDS)
                    lArgsMet.SetValue("Latitude", "38.85") 'latitude of Washington National
                    lDisTS.Open("Solar Radiation (Disaggregate)", lArgsMet)
                    Logger.Dbg("Solar Radiation Disaggregation complete!")
                    lDataGroup.Add(lDisTS.DataSets(0))
                Case "W-VL"
                    Logger.Dbg("Found Wlind Velocity (m/s) - convert to mi/day")
                    lArgsMet.Clear()
                    lArgsMet.SetValue("Timeseries", lDS)
                    lArgsMet.SetValue("Number", 53.7) 'multiplier converts (m/s) to (mi/day)
                    lTsMath.Open("Multiply", lArgsMet)
                    Dim lWind As atcTimeseries = lTsMath.DataSets(0)
                    Logger.Dbg("Disaggregating Daily Wind to Hourly")
                    Dim lHrDist() As Double = {0, 0.034, 0.034, 0.034, 0.034, 0.034, 0.034, 0.034, 0.035, 0.037, 0.041, 0.046, 0.05, 0.053, 0.054, 0.058, 0.057, 0.056, 0.05, 0.043, 0.04, 0.038, 0.035, 0.035, 0.034}
                    lArgsMet.Clear()
                    lArgsMet.SetValue("TWND", lWind)
                    lArgsMet.SetValue("Hourly Distribution", lHrDist)
                    lDisTS.Open("Wind (Disaggregate)", lArgsMet)
                    Logger.Dbg("Wind Disaggregation complete!")
                    lDataGroup.Add(lDisTS.DataSets(0))
                Case "TDEW"
                    Logger.Dbg("Found Dewpoint Temp - Convert to Deg F")
                    lArgsMet.Clear()
                    lArgsMet.SetValue("Timeseries", lDS)
                    lTsMath.Open("Celsius to F", lArgsMet)
                    lDewPt = lTsMath.DataSets(0)
                    Logger.Dbg("Converted Dewpoint Temp to Deg F")
                Case "PRCP"
                    Logger.Dbg("Found Precip")
                    lPrecip = lDS
                    Logger.Dbg("Precip contains " & lPrecip.numValues & " values")
                Case "DUR"
                    Logger.Dbg("Found Precip Duration")
                    lPrecDur = lDS
                    Logger.Dbg("Precip Duration contains " & lPrecDur.numValues & " values")
                Case "TP"
                    Logger.Dbg("Found Precip Time to Peak")
                    lPrecPkTime = lDS
                    Logger.Dbg("Precip Time to Peak contains " & lPrecPkTime.numValues & " values")
                Case "IP"
                    Logger.Dbg("Found Precip Peak Intensity")
                    lPrecPkInten = lDS
                    Logger.Dbg("Precip Peak Intensity contains " & lPrecPkInten.numValues & " values")
            End Select
        Next

        If Not lTMin Is Nothing AndAlso Not lTMax Is Nothing Then 'disaggregate tmin/tmax to hrly temp
            Logger.Dbg("Disaggregating TMin/TMax to Hourly Temp")
            lArgsMet.Clear()
            lArgsMet.SetValue("TMIN", lTMin)
            lArgsMet.SetValue("TMAX", lTMax)
            lArgsMet.SetValue("Observation Time", "24")
            lDisTS.Open("Temperature", lArgsMet)
            lATmp = lDisTS.DataSets(0)
            Logger.Dbg("Temperature Disaggregation complete!")
            lDataGroup.Add(lATmp)
            'now disaggregate Dewpoint using hourly temp
            lArgsMet.Clear()
            lArgsMet.SetValue("Dewpoint", lDewPt)
            lArgsMet.SetValue("ATMP", lATmp)
            lDisTS.Open("Dewpoint", lArgsMet)
            Logger.Dbg("Dewpoint Temp Disaggregation complete!")
            lDataGroup.Add(lDisTS.DataSets(0))
            'now generate Hamon PET
            Dim lCTS() As Double = {0, 0.0045, 0.01, 0.01, 0.01, 0.0085, 0.0085, 0.0085, 0.0085, 0.0085, 0.0095, 0.0095, 0.0095}
            Logger.Dbg("Computing Hamon PET from Hourly temperature")
            lts = atcMetCmp.PanEvaporationTimeseriesComputedByHamonX(lATmp, Nothing, True, 39, lCTS)
            Logger.Dbg("Hamon PET generation complete!")
            lts.Attributes.SetValue("Location", lTMin.Attributes.GetValue("Location"))
            lDataGroup.Add(lts)
        End If
        If Not lPrecip Is Nothing AndAlso Not lPrecDur Is Nothing AndAlso _
           Not lPrecPkTime Is Nothing AndAlso Not lPrecPkInten Is Nothing Then 'disagg daily precip
            Logger.Dbg("Disaggregating Precip")
            lts = atcMetCmp.DisCliGenPrecip(lPrecip, lPrecDur, lPrecPkTime, lPrecPkInten)
            Logger.Dbg("Precip Disaggregation complete!")
            lArgsMet.Clear()
            lArgsMet.SetValue("Timeseries", lts)
            lArgsMet.SetValue("Number", 25.4) 'multiplier converts mm to inches
            lTsMath.Open("Divide", lArgsMet)
            lDataGroup.Add(lTsMath.DataSets(0))
        End If
        Return lDataGroup
    End Function

End Class