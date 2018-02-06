Imports atcUtility
Imports atcData
Imports MapWinUtility


Public Class atcExpertSystem
    'Friend ErrorCriteria As HexErrorCriteria
    Public Storms As New Generic.List(Of HexStorm)
    Public StormsSorted As New Generic.List(Of HexStorm)
    Public Sites As New Generic.List(Of HexSite)
    Public SDateJ As Double, EDateJ As Double
    Public Name As String
    Public ObservedStorms As New atcCollection
    Public SimulatedStorms As New atcCollection

    Private pFlowOnly As Boolean
    Private pStatistics As New HexStatistics
    Private pDatasetTypes As New HexDatasetTypes
    Private pSummerMonths() As Integer = {6, 7, 8}
    Private pWinterMonths() As Integer = {12, 1, 2}
    Private pUci As atcUCI.HspfUci
    Private pDataSource As atcDataSource


    Private pSubjectiveData(25) As Integer
    Private pLatMin As Double, pLatMax As Double
    Private pLngMin As Double, pLngMax As Double
    'TODO: get rid of next two global arrays, store in site class
    Private pStats(,,) As Double
    Private pHSPFOutput1(,) As Double
    '1 = SIMTRO - simulated total runoff
    '2 = OBSTRO - observed total runoff
    '3 = S010FD - simulated total of highest 10 percent daily mean of flows (in)
    '4 = O010FD - observed total of highest 10 percent daily mean of flows (in)
    '5 = S50100 - simulated total of lowest 50 percent daily mean of flows (in)
    '6 = O50100 - observed total of lowest 50 percent daily mean of flows (in)
    '7 = TACTET - simulated total actual evapotranspiration (in)
    '8 = TPOTET - total potential evapotranspiration (in)
    Private pHSPFOutput2(,) As Double
    '1 = SISTVO - simulated total storm volume (in)
    '2 = OBSTVO - observed total storm volume (in)
    '3 = SPKSTR - simulated storm peaks volume (in)
    '4 = OPKSTR - observed storm peaks volume (in)
    '5 = QTRSIM - mean simulated low-flow recession (dimensionless)
    '6 = QTRMEA - mean observed low-flow recession (dimensionless)
    '7 = INFSUM - total simulated storm interflow (in)
    '8 = SROSUM - total simulated storm surface runoff (in)
    Private pHSPFOutput3(,) As Double
    '1 = SMRSIM - simulated summer flow volume (in)
    '2 = SMROBS - observed summer flow volume (in)
    '3 = WNRSIM - simulated winter flow volume (in)
    '4 = WNROBS - observed winter flow volume (in)
    '5 = SRHSIM - simulated summer flow volume (in)
    '6 = SRHOBS - observed summer flow volume (in)
    Private Const pConvert As Double = 24.0# * 3600.0# * 12.0# / 43560.0#
    Private Const pNSteps As Integer = 500
    Private pCountOfMissingData As Integer = 0
    Private pPercentOfMissingData As Double = 0.0

    Public Sub New(ByVal aUci As atcUCI.HspfUci,
                   ByVal lExpertSystemFileName As String,
                   ByVal aSDateJ As Double,
                   ByVal aEDateJ As Double,
                   Optional ByVal aExpertFlowOnly As Boolean = False)
        pFlowOnly = aExpertFlowOnly
        pUci = aUci

        Dim lFileName As String = lExpertSystemFileName
        'IO.Path.GetFileNameWithoutExtension(aUci.Name) & ".exs"

        If Len(lFileName) = 0 Then
            'create a blank one
            SDateJ = aSDateJ
            EDateJ = aEDateJ
            ReDim pHSPFOutput1(8, 1)
            ReDim pHSPFOutput2(8, 1)
            ReDim pHSPFOutput3(6, 1)
            Exit Sub
        End If

        If Not FileExists(lFileName) Then
            Throw New ApplicationException("ExpertSystemFile " & lFileName & " not found")
        Else
            Dim lines() As String = IO.File.ReadAllLines(lFileName)
            Dim lExsRecords As New ArrayList()
            Dim regWhiteSpace As New System.Text.RegularExpressions.Regex("\s")
            For Each line As String In lines
                If Not line.Contains("***") Then
                    lExsRecords.Add(RTrim(line))
                End If
            Next

            Dim lExsRecord As String = lExsRecords(0).PadRight(51)
            Name = Trim(lExsRecord.Substring(0, 8))
            Dim lWdmFilename As String = CurDir() & "\" & Name & ".wdm"
            'Not sure if CurDir is the best way
            ' Dim pDatasource As New atcDataSource
            pDataSource = atcDataManager.DataSourceBySpecification(lWdmFilename)
            If pDataSource Is Nothing Then
                atcDataManager.OpenDataSource(lWdmFilename)
                pDataSource = atcDataManager.DataSourceBySpecification(lWdmFilename)
            End If

            Dim lNSites As Integer
            If Not Integer.TryParse(lExsRecord.Substring(8, 5), lNSites) Then
                Throw New ApplicationException("Number of Sites are not in correct format. Program will quit!")
            End If
            If lNSites = 0 Then
                Throw New ApplicationException("The number of sites is 0. Program will quit!")
            End If
            Dim lCurSite As Integer = lExsRecord.Substring(13, 5)
            If Not (Double.TryParse(lExsRecord.Substring(19, 8), pLatMin) AndAlso Double.TryParse(lExsRecord.Substring(26, 8), pLatMax) _
            AndAlso Double.TryParse(lExsRecord.Substring(34, 8), pLngMin) AndAlso Double.TryParse(lExsRecord.Substring(42, 8), pLngMax)) Then
                Throw New ApplicationException("Latitude and Longitude of Watershed are not in correct format. Program will quit!")
            End If

            If lExsRecord.Length = 51 Then
                SDateJ = aSDateJ
                EDateJ = aEDateJ
                Logger.Dbg("The simulation time period from the uci file is used for the calibration!")
            Else 'the user could be entering dates on this line if calib period diff from sim period
                lExsRecord.PadRight(70)
                Dim lDate(5) As Integer
                If Not (Integer.TryParse(lExsRecord.Substring(52, 4), lDate(0)) _
                    AndAlso Integer.TryParse(lExsRecord.Substring(56, 2), lDate(1)) _
                    AndAlso Integer.TryParse(lExsRecord.Substring(58, 2), lDate(2))) Then
                    Throw New ApplicationException("The analysis start date in EXS file is not in correct format. Program will quit!")
                End If
                lDate(3) = 0
                lDate(4) = 0
                SDateJ = Date2J(lDate)

                If Not (Integer.TryParse(lExsRecord.Substring(62, 4), lDate(0)) _
                  AndAlso Integer.TryParse(lExsRecord.Substring(66, 2), lDate(1)) _
                  AndAlso Integer.TryParse(lExsRecord.Substring(68, 2), lDate(2))) Then
                    Throw New ApplicationException("The analysis end date in EXS file is not in correct format. Program will quit!")
                End If
                lDate(3) = 24
                lDate(4) = 0
                EDateJ = Date2J(lDate)
                Logger.Status("Analysis time period in the EXS file " & lFileName & " is used for the hydrologic calibration")
                Logger.Dbg("Analysis time period in the EXS file " & lFileName & " is used for the hydrologic calibration")

            End If

            'Default unspecified lat/integer min/max values to contiguous 48 states
            If ((pLatMin < 0.01) And (pLatMin > -0.01)) Then
                pLatMin = 24
                Logger.Dbg("Minimum Latitude was not provided, a value of 24 is being assumed")
            End If
            If ((pLatMax < 0.01) And (pLatMax > -0.01)) Then
                pLatMax = 50
                Logger.Dbg("Maximum Latitude was not provided, a value of 50 is being assumed")
            End If
            If ((pLngMin < 0.01) And (pLngMin > -0.01)) Then
                pLngMin = 66
                Logger.Dbg("Minimum Longitude was not provided, a value of 66 is being assumed")
            End If
            If ((pLngMax < 0.01) And (pLngMax > -0.01)) Then
                pLngMax = 125
                Logger.Dbg("Maximum Longiude was not provided, a value of 125 is being assumed")
            End If
            'Read Site block
            For lSiteIndex As Integer = 1 To lNSites
                lExsRecord = lExsRecords(lSiteIndex)
                Dim lDsn(9) As Integer
                For lConsIndex As Integer = 0 To 9
                    lDsn(lConsIndex) = lExsRecord.Substring(lConsIndex * 4, 4)
                    If lDsn(lConsIndex) = 0 Then Throw New ApplicationException(lConsIndex & "DSN is missing for site" & lSiteIndex & ". Program will quit!")
                Next lConsIndex
                Dim lStatDN As Integer = lExsRecord.Substring(42, 2)  '0 or 1
                Dim lName As String = lExsRecord.Substring(45).Replace(vbCr, "").Trim
                'Dim lErrorTerms( As Double
                Dim lSite As New HexSite(Me, lName, lStatDN, lDsn) ', lErrorTerms)
                Sites.Add(lSite)
            Next (lSiteIndex)

            Dim lRecordIndex As Integer = lNSites + 1
            'Read number of storms
            Dim lNStorms As Integer
            If Not Integer.TryParse(lExsRecords(lRecordIndex), lNStorms) Then
                Throw New ApplicationException("The number of storms are not in correct format. Program will quit!")
            End If

            'Read storm end/start dates
            Dim lStormSDate(5) As Integer
            Dim lStormEDate(5) As Integer
            For lStormIndex As Integer = 1 To lNStorms
                lExsRecord = lExsRecords(lRecordIndex + lStormIndex)
                If Not Integer.TryParse(lExsRecord.Substring(0, 5), lStormSDate(0)) _
                        And Integer.TryParse(lExsRecord.Substring(21, 5), lStormEDate(0)) Then 'Checking if storm dates are
                    'are in correct format
                    Throw New ApplicationException("The dates for storm number " & lStormIndex & " are not in correct format. Program will quit!")
                End If

                For lTimeIndex As Integer = 0 To 4
                    If Not Integer.TryParse(lExsRecord.Substring(6 + 3 * lTimeIndex, 3), lStormSDate(lTimeIndex + 1)) _
                    And Integer.TryParse(lExsRecord.Substring(25 + 3 * lTimeIndex, 3), lStormEDate(lTimeIndex + 1)) Then
                        Throw New ApplicationException("The dates for storm number " & lStormIndex & " are not in correct format. Program will quit!")
                    End If
                Next lTimeIndex

                'Get the starting and ending storm dates in a 1-D Julian array
                Storms.Add(New HexStorm(lStormIndex, lStormSDate, lStormEDate))

            Next lStormIndex

            'Read basin area (acres)
            lRecordIndex += lNStorms + 1
            lExsRecord = lExsRecords(lRecordIndex)
            For lSiteIndex As Integer = 0 To lNSites - 1
                If Not Double.TryParse(SafeSubstring(lExsRecord, (lSiteIndex * 8), 8), Sites(lSiteIndex).Area) Then
                    Throw New ApplicationException("The area for Site " & lSiteIndex & " is not in correct format. Program will quit!")
                End If
            Next lSiteIndex

            'Read error terms
            lRecordIndex += 1 'lNSites


            For lSiteIndex As Integer = 0 To Sites.Count - 1
                lExsRecord = lExsRecords(lRecordIndex)
                For lErrorIndex As Integer = 1 To Sites(lSiteIndex).ErrorCriteria.Count
                    If lExsRecord.Length >= lErrorIndex * 8 Then 'Becky's note: this makes sure that the error 
                        'criterion is actually on the line by only reading it if the total line length exceeds 
                        'that needed for this error to have been included
                        'Becky's additional note: when ErrorCriteria was defined, the 'New' method automatically
                        'filled it with the default values - the code below reads the user-defined values if there
                        'are any.  If the user doesn't supply any and the if-then never trips, then no harm, the
                        'program just uses the defaults.

                        Dim lErrorCriteriumValue As String = lExsRecord.Substring((lErrorIndex - 1) * 8, 8)
                        If lErrorCriteriumValue.Length > 0 Then
                            Sites(lSiteIndex).ErrorCriteria(lErrorIndex).Value = lErrorCriteriumValue
                        End If
                    End If
                Next lErrorIndex
                If (Sites(lSiteIndex).ErrorCriteria(10).Value < 0.000001 And Sites(lSiteIndex).ErrorCriteria(10).Value > -0.000001) Then
                    'percent of time in baseflow read in as zero, change to 30
                    Sites(lSiteIndex).ErrorCriteria(10).Value = 30.0#
                End If
                lRecordIndex += 1
            Next lSiteIndex
            'Read latest hspf output
            ReDim pHSPFOutput1(8, Sites.Count)
            ReDim pHSPFOutput2(8, Sites.Count)
            ReDim pHSPFOutput3(6, Sites.Count)

            If Not lRecordIndex >= lExsRecords.Count Then

                If Not (lRecordIndex >= lExsRecords.Count Or lExsRecords(lRecordIndex).Trim = "" Or
                        lExsRecords(lRecordIndex).Tolower.contains("seasons")) Then

                    'If no text is found in lines after the error criteria, HSPEXP can still work.
                    For lSiteIndex As Integer = 0 To Sites.Count - 1
                        lExsRecord = lExsRecords(lRecordIndex).PadRight(80)
                        For lIndex As Integer = 0 To 7
                            pHSPFOutput1(lIndex + 1, lSiteIndex) = lExsRecord.Substring(8 * lIndex, 8)
                        Next lIndex
                        lRecordIndex += 1
                        lExsRecord = lExsRecords(lRecordIndex).PadRight(80)

                        For lIndex As Integer = 0 To 7
                            pHSPFOutput2(lIndex + 1, lSiteIndex) = lExsRecord.Substring(8 * lIndex, 8)
                        Next lIndex
                        lRecordIndex += 1
                        lExsRecord = lExsRecords(lRecordIndex).PadRight(80)
                        For lIndex As Integer = 0 To 5
                            pHSPFOutput3(lIndex + 1, lSiteIndex) = lExsRecord.Substring(8 * lIndex, 8)
                        Next lIndex
                        lRecordIndex += 1
                    Next lSiteIndex
                End If
                'Flags for ancillary data (1=yes, 0=no, -1=unknown, -2=undefined)
                lExsRecord = lExsRecords(lRecordIndex).PadRight(80)
                If Not (lExsRecords(lRecordIndex).Trim = "" Or
                        lExsRecords(lRecordIndex).Tolower.contains("seasons")) Then

                    lExsRecord = Trim(lExsRecord.Replace(vbCrLf, "")) & "  " & Trim(lExsRecords(lRecordIndex + 1))
                    lExsRecord = lExsRecord.Replace("  ", ",")
                    Dim pSubjectiveDataStr() As String = lExsRecord.Split(",")
                    pSubjectiveData = Array.ConvertAll(pSubjectiveDataStr, Function(str) Int32.Parse(str))
                    lRecordIndex += 2
                    'For lIndex As Integer = 0 To 19
                    '    pSubjectiveData(lIndex + 1) = lExsRecord.Substring(lIndex * 4, 4)
                    'Next lIndex
                    'lRecordIndex += 1
                    'lExsRecord = lExsRecords(lRecordIndex).PadRight(80)
                    'For lIndex As Integer = 20 To 22
                    '    pSubjectiveData(lIndex + 1) = lExsRecord.Substring((lIndex - 20) * 4, 4)
                    'Next lIndex
                End If

                '  'Change subjective data based on other data
                '  If (SISTVO(CURSIT) > OBSTVO(CURSIT)) Then
                '    'Simulated storm runoff volumes higher than obs
                '    SISROV = 1
                '  ElseIf (SISTVO(CURSIT) < OBSTVO(CURSIT)) Then
                '    'Simulated storm runoff volumes lower than obs
                '    SISROV = 0
                '  End If

                If lExsRecords.Count > lRecordIndex AndAlso lExsRecords(lRecordIndex).tolower.contains("seasons") Then
                    lRecordIndex += 1
                    Dim Months() As String = lExsRecords(lRecordIndex).split(",")
                    pSummerMonths = Array.ConvertAll(Months, Function(str) Int32.Parse(str))
                    lRecordIndex += 1
                    Months = lExsRecords(lRecordIndex).split(",")
                    pWinterMonths = Array.ConvertAll(Months, Function(str) Int32.Parse(str))

                End If
                lRecordIndex += 1

            End If
        End If
        'pErrorCriteria.Edit()
    End Sub


    Public Function ExpertWDMFileName() As String
        Return Name
    End Function
    Public Function ExpertWDMDataSource() As atcDataSource
        Return pDataSource
    End Function

    Public Function Report() As String
        CalcStats(pDataSource)
        Return CalcErrorTerms(pUci)
    End Function

    Public Sub CalcAdvice(ByRef aString As String, ByVal aSiteIndex As Integer) 'Becky added this method to compute & export advice
        Try
            'For lSiteIndex As Integer = 1 To Sites.Count
            'aString &= vbCrLf & vbCrLf & "Site " & lSiteIndex.ToString & ":" & vbCrLf & vbCrLf
            Dim lSite As HexSite = Sites(aSiteIndex - 1)
            Dim lErr As Integer = 1
            Dim lCriteriaList As New List(Of Double) 'I created these lists so I could pass them other places where the Hex things aren't defined
            Dim lErrorTermList As New List(Of Double)
            lCriteriaList.Add(0) 'add blank value for 0th place
            lErrorTermList.Add(0) 'add blank value for 0th place
            For i As Integer = 1 To 20
                If i = 8 Then 'fix the summer storm volume error to be the seasonal storm error 
                    lCriteriaList.Add(lSite.ErrorCriteria.Criterion(i).Value)
                    lErrorTermList.Add(lSite.ErrorTerm(i) - lSite.ErrorTerm(5)) 'summer storm minus total storm
                ElseIf i = 20 Then
                    'put the pure summer storm error in slot 20
                    lCriteriaList.Add(lSite.ErrorCriteria.Criterion(8).Value)
                    lErrorTermList.Add(lSite.ErrorTerm(8))
                Else
                    lCriteriaList.Add(lSite.ErrorCriteria.Criterion(i).Value)
                    lErrorTermList.Add(lSite.ErrorTerm(i))
                End If
            Next i
            Do Until lCriteriaList(lErr) < Math.Abs(lErrorTermList(lErr))
                'do this loop until we find an error outside of the range of the criterion
                Select Case lErr
                    'the actual error terms are held in 1, 2, 3, 4, 5, 7, 8, 12, 13, 14, 15, 16, 17, 18, 19, 20
                    'this select case steps over the blank terms
                    Case 20
                        'if we've gotten this far and no errors, then all criteria are okay
                        lErr = -1
                        Exit Do
                    Case Is < 5
                        lErr += 1
                    Case 5
                        lErr = 7
                    Case 7
                        lErr = 8
                    Case 8
                        lErr = 12
                    Case Is > 11
                        lErr += 1
                End Select
            Loop
            If lErr > 0 Then 'if -1, have gone through and met all criteria - use method in RWZUtilities to get the appropriate advice
                aString &= ErrMessage(lCriteriaList, lErrorTermList, lErr)
            Else
                aString &= "The calibration for site " & lSite.Name & " is successful based on current criteria.  If you wish to refine " & _
                    "the calibration further, adjust the error criteria in your .exs file."
            End If
            'Next lSiteIndex
        Catch ex As Exception
            aString &= "An error was encountered while attempting to produce advice.  Please consult the statistics file to make an " & _
                "educated decision about your next step.  Error message: " & ex.Message
        End Try
    End Sub

    Private Function ErrMessage(ByVal aCriteria As List(Of Double), ByVal aError As List(Of Double), ByVal aItem As Integer) As String
        ErrMessage = "Above all else, make sure that you keep track of the changes you make and the effects those changes have." & vbCrLf & _
            "This advice may take you in loops or get stuck on one point, but if you have a record of educated changes, you can" & vbCrLf & _
            "make your own informed decisions.  Additionally, you can change the criteria in the .exs file if you would like the" & vbCrLf & _
            "program to skip over a particular statistic.  The statistics are evaluated in this order: total volume, baseflow" & vbCrLf & _
            "recession, high and low flows, storm peaks and volumes, and finally seasonal errors.  You cannot move past an item on" & vbCrLf & _
            "this list and look at the next statistic until you have forced the errors for that item to fall within the relevant criteria." & vbCrLf & vbCrLf & vbCrLf & vbCrLf
        Select Case aItem
            Case 1 'total volume
                ErrMessage &= ExpertAdvice.WaterBalance(aCriteria, aError)
            Case 2 'baseflow recession
                ErrMessage &= ExpertAdvice.LowFlowRecession(aCriteria(aItem), aError(aItem))
            Case 3, 4 'high or low flows
                ErrMessage &= ExpertAdvice.HighLowFlows(aCriteria, aError, aItem)
            Case 5, 16 'storm flows
                ErrMessage &= ExpertAdvice.Stormflows(aCriteria, aError, aItem)
            Case 7, 8 'seasonal
                ErrMessage &= ExpertAdvice.SeasonalAdvice(aCriteria, aError, aItem)
            Case Else 'new errors
                ErrMessage = "You have met all the traditional error criteria.  If you choose to proceed, the next " & _
                    "item to consider is the error for " & ErrName(aItem) & ", which has an error of " & aError(aItem) & "%"
        End Select
    End Function

    Private Function ErrName(ByVal aItem As Integer) As String
        Select Case aItem
            Case 12
                ErrName = "lowest 10% of flows"
            Case 13
                ErrName = "lowest 25% of flows"
            Case 14
                ErrName = "highest 25% of flows"
            Case 15
                ErrName = "highest 50% of flows"
            Case 16
                ErrName = "average storm peaks"
            Case 17
                ErrName = "summer volume"
            Case 18
                ErrName = "winter volume"
            Case 19
                ErrName = "winter storm volume"
            Case 20
                ErrName = "summer storm volume"
            Case Else
                ErrName = ""
        End Select
    End Function

    Private Sub CalcStats(ByVal aDataSource As atcDataSource)
        Dim lDataSetTypes() As String = {"SimTotRunoff", "ObsStreamflow"}

        ReDim pStats(pStatistics.Count, lDataSetTypes.GetUpperBound(0) + 1, Sites.Count)

        'get number of values
        Dim lTimeStep As Integer, lTimeUnit As Integer, lNVals As Integer
        lTimeStep = 1
        lTimeUnit = 4 'day
        lNVals = timdifJ(SDateJ, EDateJ, lTimeUnit, lTimeStep)

        For lSiteIndex As Integer = 1 To Sites.Count
            Dim lSite As HexSite = Sites(lSiteIndex - 1)
            'Looked at observed data before hand to check if there are missing values. If yes, then it is Flow only calculation
            Dim obslTSer As atcTimeseries = aDataSource.DataSets(aDataSource.DataSets.IndexFromKey(lSite.DSN(1))) 'Getting the observed data
            obslTSer = SubsetByDate(obslTSer, SDateJ, EDateJ, Nothing)
            obslTSer = Aggregate(obslTSer, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)
            If obslTSer.Attributes.GetDefinedValue("Count Missing").Value > 0 Then
                pFlowOnly = True
            End If

            If Not pFlowOnly Then
                ReDim Preserve lDataSetTypes(5)
                lDataSetTypes(2) = "SimInterflow"
                lDataSetTypes(3) = "SimBaseflow"
                lDataSetTypes(4) = "ObsPotentialET"
                lDataSetTypes(5) = "SimActualET"
            End If
            ReDim pStats(pStatistics.Count, lDataSetTypes.GetUpperBound(0) + 1, Sites.Count)
            For Each lDatasetType As String In lDataSetTypes
                Dim lStatGroup As Integer = pDatasetTypes.IndexFromKey(lDatasetType)
                'set Stats to undefined for this group
                ZipR(pStatistics.Count, GetNaN, pStats, lStatGroup, lSiteIndex)
                Dim lDSN As Integer
                With lSite
                    Select Case lStatGroup 'get the correct dsn
                        Case 1 : lDSN = .DSN(0)
                        Case 2 : lDSN = .DSN(1)
                        Case 3 : lDSN = .DSN(2)
                        Case 4 : lDSN = .DSN(3)
                        Case 5 : lDSN = .DSN(6)
                        Case 6 : lDSN = .DSN(7)
                    End Select
                End With
                'Get data - daily values and max values as necessary
                Dim lTSer As atcTimeseries = aDataSource.DataSets(aDataSource.DataSets.IndexFromKey(lDSN))
                If lTSer Is Nothing Then
                    Throw New ApplicationException("Data set Number " & lDSN & " was not found. Program will quit.")
                    Stop
                End If
                'subset by date to simulation period
                Dim lNewTSer As atcTimeseries = SubsetByDate(lTSer, SDateJ, EDateJ, Nothing)
                If lNewTSer.numValues < 1 Then
                    Throw New ApplicationException("The data set Number " & lDSN & " has no data in the analysis period. Program will quit.")
                    Stop
                End If

                'don't Clear lTSer as that will clear the original, precluding its future use
                lTSer = Nothing
                Dim lDailyTSer As atcTimeseries
                If lStatGroup = 2 Then 'observed flow in cfs, want average
                    lDailyTSer = Aggregate(lNewTSer, atcTimeUnit.TUDay, 1, atcTran.TranAverSame)

                Else 'want total values
                    lDailyTSer = Aggregate(lNewTSer, atcTimeUnit.TUDay, 1, atcTran.TranSumDiv)
                End If
                lNewTSer.Clear()
                lNewTSer.Dates.Clear()
                lNewTSer = Nothing

                'check to make sure we got values
                Dim lDataProblem As Boolean = False
                If lDailyTSer.Values.Length = 0 Then
                    lDataProblem = True
                End If
                Dim lValueCollection As New atcCollection
                Dim lKeyForValueCollection As Integer = 0
                If lStatGroup = 1 Or lStatGroup = 2 Then

                    For i As Integer = 1 To obslTSer.numValues
                        pCountOfMissingData = obslTSer.Attributes.GetDefinedValue("Count Missing").Value
                        pPercentOfMissingData = pCountOfMissingData * 100 / obslTSer.Attributes.GetDefinedValue("Count").Value
                        If Double.IsNaN(obslTSer.Values(i)) Then
                            pFlowOnly = True

                            If lStatGroup = 1 Then
                                lDailyTSer.Value(i) = Double.NaN
                            End If
                        Else

                            lValueCollection.Increment(lKeyForValueCollection, lDailyTSer.Values(i))

                            lKeyForValueCollection += 1
                        End If

                    Next


                End If

                lValueCollection.SortByValue()
                For i As Integer = 0 To lValueCollection.Count - 1
                    lValueCollection.Increment("Sum", lValueCollection(i))

                    If i < lValueCollection.Count * 0.9 Then lValueCollection.Increment("%Sum90", lValueCollection(i))

                    If i < lValueCollection.Count * 0.75 Then lValueCollection.Increment("%Sum75", lValueCollection(i))

                    If i < lValueCollection.Count * 0.5 Then lValueCollection.Increment("%Sum50", lValueCollection(i))

                    If i < lValueCollection.Count * 0.25 Then lValueCollection.Increment("%Sum25", lValueCollection(i))

                    If i < lValueCollection.Count * 0.1 Then lValueCollection.Increment("%Sum10", lValueCollection(i))

                Next


                If lDataProblem Then  'if we weren't able to retrieve the data set
                    'set Stats to undefined

                    ZipR(pStatistics.Count, GetNaN, pStats, lStatGroup, lSiteIndex)
                    Logger.Msg("Unable to retrieve DSN " & lDSN & vbCrLf &
                               "from the file " & aDataSource.Name, "Bad Data Set")
                Else  'generate statistics
                    Dim lValues() As Double = lDailyTSer.Values
                    'total volume always needed 
                    'RWZSetArgs(lDailyTSer) 'Becky's addition Dec 9: this uses atcTimeseriesStatistics to calculate all the sums, bins needed below
                    pStats(1, lStatGroup, lSiteIndex) = lDailyTSer.Attributes.GetDefinedValue("Sum").Value 'Becky commented this out and used .GetValue instead
                    'others?
                    If (lStatGroup = 1 Or lStatGroup = 2) Then  'full range of pStats desired
                        pStats(2, lStatGroup, lSiteIndex) = lValueCollection.ItemByKey("%Sum50") 'lDailyTSer.Attributes.GetValue("%Sum50") '50% low
                        pStats(3, lStatGroup, lSiteIndex) = lValueCollection.ItemByKey("Sum") - lValueCollection.ItemByKey("%Sum90") '10% high
                        pStats(11, lStatGroup, lSiteIndex) = lValueCollection.ItemByKey("%Sum10") '10% low
                        pStats(12, lStatGroup, lSiteIndex) = lValueCollection.ItemByKey("%Sum25") '25% low
                        pStats(13, lStatGroup, lSiteIndex) = lValueCollection.ItemByKey("Sum") - lValueCollection.ItemByKey("%Sum75") '25% high
                        pStats(14, lStatGroup, lSiteIndex) = lValueCollection.ItemByKey("Sum") - lValueCollection.ItemByKey("%Sum50") '50% high

                        Dim lTmpDate(5) As Integer
                        J2Date(SDateJ, lTmpDate)

                        pStats(7, lStatGroup, lSiteIndex) = 0.0# 'summer volume
                        pStats(8, lStatGroup, lSiteIndex) = 0.0# 'winter volume
                        For i As Integer = 1 To lDailyTSer.numValues
                            If Double.IsNaN(lValues(i)) Then
                                TIMADD(lTmpDate, lTimeUnit, lTimeStep, lTimeStep, lTmpDate)
                                Continue For
                            End If
                            If pSummerMonths.Length > 0 Then
                                For Each SeasonMonth As Integer In pSummerMonths
                                    If lTmpDate(1) = SeasonMonth Then
                                        pStats(7, lStatGroup, lSiteIndex) += lValues(i)
                                    End If
                                Next
                                For Each SeasonMonth As Integer In pWinterMonths
                                    If lTmpDate(1) = SeasonMonth Then

                                        pStats(8, lStatGroup, lSiteIndex) += lValues(i)
                                    End If
                                Next

                            Else
                                If (lTmpDate(1) = 12 Or lTmpDate(1) = 1 Or lTmpDate(1) = 2) Then
                                    'in the winter
                                    pStats(8, lStatGroup, lSiteIndex) += lValues(i)
                                ElseIf (lTmpDate(1) = 6 Or lTmpDate(1) = 7 Or lTmpDate(1) = 8) Then
                                    'in the summer
                                    pStats(7, lStatGroup, lSiteIndex) += lValues(i)
                                End If
                            End If
                            TIMADD(lTmpDate, lTimeUnit, lTimeStep, lTimeStep, lTmpDate)
                        Next i
                    End If


                    If (lStatGroup >= 1 And lStatGroup <= 4) Then  'calc storm info
                        pStats(4, lStatGroup, lSiteIndex) = 0.0# 'initialize storm volume
                        pStats(5, lStatGroup, lSiteIndex) = 0.0# 'storm peaks
                        pStats(9, lStatGroup, lSiteIndex) = 0.0# 'summer storms
                        pStats(10, lStatGroup, lSiteIndex) = 0.0# 'winter storms
                        For Each lStorm As HexStorm In Storms

                            If lStorm.SDateJ >= SDateJ And
                               lStorm.EDateJ <= EDateJ Then 'storm within run span
                                'TODO: this matches VB6Script results, needs to have indexes checked!
                                Dim lN1 As Integer, lN2 As Integer
                                lN1 = timdifJ(SDateJ, lStorm.SDateJ, lTimeUnit, lTimeStep) + 1
                                lN2 = timdifJ(SDateJ, lStorm.EDateJ, lTimeUnit, lTimeStep)
                                Dim SkipStorm As Boolean = False
                                For i As Integer = lN1 To lN2
                                    If Double.IsNaN(lValues(i)) Then 'Skip the storm calculation if any of the value in the storm period i Nan
                                        SkipStorm = True
                                    End If
                                Next
                                If SkipStorm Then Continue For
                                Dim lNLimit As Integer = lDailyTSer.Values.GetUpperBound(0)
                                If lN2 <= lNLimit Then
                                    Dim lTmpDate(5) As Integer
                                    J2Date(lStorm.SDateJ - 1, lTmpDate)
                                    Dim lRtmp As Double = lDailyTSer.Values(lN1)
                                    For i As Integer = lN1 To lN2

                                        pStats(4, lStatGroup, lSiteIndex) += lValues(i)

                                        If (lDailyTSer.Values(i) > lRtmp) Then 'a new peak
                                            lRtmp = lDailyTSer.Values(i)
                                        End If

                                        If pSummerMonths.Length > 0 AndAlso pWinterMonths.Length > 0 Then
                                            For Each SeasonMonth As Integer In pSummerMonths
                                                If lTmpDate(1) = SeasonMonth Then
                                                    pStats(9, lStatGroup, lSiteIndex) += lValues(i)
                                                End If
                                            Next
                                            For Each SeasonMonth As Integer In pWinterMonths
                                                If lTmpDate(1) = SeasonMonth Then
                                                    pStats(10, lStatGroup, lSiteIndex) += lValues(i)
                                                End If
                                            Next

                                        Else
                                            If (lTmpDate(1) = 12 Or lTmpDate(1) = 1 Or lTmpDate(1) = 2) Then 'in the winter
                                                pStats(10, lStatGroup, lSiteIndex) += lValues(i)
                                            ElseIf (lTmpDate(1) = 6 Or lTmpDate(1) = 7 Or lTmpDate(1) = 8) Then 'in the summer
                                                pStats(9, lStatGroup, lSiteIndex) += lValues(i)
                                            End If
                                            TIMADD(lTmpDate, lTimeUnit, lTimeStep, lTimeStep, lTmpDate)
                                        End If
                                    Next i
                                    pStats(5, lStatGroup, lSiteIndex) += lRtmp
                                    J2Date(lStorm.SDateJ, lTmpDate)
                                    Dim StormStartDate As String = lTmpDate(0) & "," & lTmpDate(1) & "," & lTmpDate(2)
                                    If lStatGroup = 1 Then
                                        SimulatedStorms.Increment("Vol_" & StormStartDate, pStats(4, lStatGroup, lSiteIndex))


                                        SimulatedStorms.Increment("Peak_" & StormStartDate, pStats(5, lStatGroup, lSiteIndex) * lSite.Area * 43560.0# / (12.0# * 24.0# * 3600.0#))
                                    ElseIf lStatGroup = 2 Then
                                        ObservedStorms.Increment("Vol_" & StormStartDate, pStats(4, lStatGroup, lSiteIndex))
                                        ObservedStorms.Increment("Peak_" & StormStartDate, pStats(5, lStatGroup, lSiteIndex))
                                    End If


                                End If
                            End If
                        Next
                    End If

                    If (lStatGroup = 1 Or lStatGroup = 2) Then 'Change flows to recessions
                        Dim lRecessionTimser As atcTimeseries = lDailyTSer.Clone
                        'save first data value
                        Dim lSavDat As Double = lRecessionTimser.Values(1)
                        For lIndex As Integer = 2 To lRecessionTimser.Values.GetUpperBound(0)
                            Dim lRecession As Double
                            If lSavDat > 0.0000000001 Then 'have some flow
                                lRecession = lRecessionTimser.Values(lIndex) / lSavDat
                            Else 'no flow
                                lRecession = GetNaN()
                            End If
                            lSavDat = lRecessionTimser.Values(lIndex)
                            lRecessionTimser.Values(lIndex - 1) = lRecession
                        Next lIndex
                        lRecessionTimser.Attributes.DiscardCalculated() 'Becky commented this out, concerned 'calculated' isnt stored properly
                        'lRecessionTimser.Attributes.CalculateAll()

                        'new percent of time in base flow term
                        'RWZRecessionPrep(lRecessionTimser) 'added by Becky to clear out attributes since DiscardCalculated doesn't work
                        'from lDailyTSer and replace with values appropriate to the new recession timeseries
                        Dim lStr As String = lRecessionTimser.Attributes.GetFormattedValue("%50")
                        If IsNumeric(lStr) Then
                            pStats(6, lStatGroup, lSiteIndex) = lStr
                        Else
                            pStats(6, lStatGroup, lSiteIndex) = GetNaN()
                        End If
                        lRecessionTimser.Clear()
                        lRecessionTimser.Dates.Clear()
                        lRecessionTimser = Nothing
                    End If
                End If

                If lStatGroup = 1 Or lStatGroup = 3 Or lStatGroup = 4 Then 'take average over NStorms
                    If Storms.Count > 0 Then pStats(5, lStatGroup, lSiteIndex) /= Storms.Count
                    'convert storm peak stat from acre-inch/day to cfs
                    pStats(5, lStatGroup, lSiteIndex) *= lSite.Area * 43560.0# / (12.0# * 24.0# * 3600.0#)
                ElseIf lStatGroup = 2 Then
                    For i As Integer = 1 To pStatistics.Count
                        If i < 5 Or i > 6 Then 'convert observed runoff values
                            pStats(i, lStatGroup, lSiteIndex) *= pConvert / lSite.Area
                        ElseIf i = 5 AndAlso Storms.Count > 0 Then 'take average over NStorms
                            pStats(i, lStatGroup, lSiteIndex) /= Storms.Count
                        End If
                    Next i
                End If
                lDailyTSer.Clear()
                lDailyTSer.Dates.Clear()
                lDailyTSer = Nothing
            Next lDatasetType
        Next lSiteIndex
    End Sub

    Private Function CalcErrorTerms(ByVal aUci As atcUCI.HspfUci) As String
        For lSiteIndex As Integer = 1 To Sites.Count
            Dim lSite As HexSite = Sites(lSiteIndex - 1)
            'total volume error
            If (pStats(1, 2, lSiteIndex) > 0.0#) Then
                lSite.ErrorTerm(1) = 100.0# * ((pStats(1, 1, lSiteIndex) - pStats(1, 2, lSiteIndex)) _
                                                  / pStats(1, 2, lSiteIndex))
            Else
                lSite.ErrorTerm(1) = GetNaN()
            End If

            '     'total volume difference
            '      VOLDIF(lSiteIndex) = pStats(1, 1, lSiteIndex) - pStats(1, 2, lSiteIndex)
            '
            '     'unrealized potential evapotranspiration
            '      ETDIF(lSiteIndex) = pStats(1, 5, lSiteIndex) - pStats(1, 6, lSiteIndex)

            'volume error in lowest 50% flows
            If (pStats(2, 2, lSiteIndex) > 0.0#) Then
                lSite.ErrorTerm(3) = 100.0# * ((pStats(2, 1, lSiteIndex) - pStats(2, 2, lSiteIndex)) _
                                                  / pStats(2, 2, lSiteIndex))
            Else
                lSite.ErrorTerm(3) = GetNaN()
            End If

            'volume error in highest 10% flows
            If (pStats(3, 2, lSiteIndex) > 0.0#) Then
                lSite.ErrorTerm(4) = 100.0# * ((pStats(3, 1, lSiteIndex) - pStats(3, 2, lSiteIndex)) _
                                           / pStats(3, 2, lSiteIndex))
            Else
                lSite.ErrorTerm(4) = GetNaN()
            End If

            'volume error in lowest 10% flows
            If (pStats(11, 2, lSiteIndex) > 0.0#) Then
                lSite.ErrorTerm(12) = 100.0# * ((pStats(11, 1, lSiteIndex) - pStats(11, 2, lSiteIndex)) _
                                           / pStats(11, 2, lSiteIndex))
            Else
                lSite.ErrorTerm(12) = GetNaN()
            End If

            'volume error in lowest 25% flows
            If (pStats(12, 2, lSiteIndex) > 0.0#) Then
                lSite.ErrorTerm(13) = 100.0# * ((pStats(12, 1, lSiteIndex) - pStats(12, 2, lSiteIndex)) _
                                           / pStats(12, 2, lSiteIndex))
            Else
                lSite.ErrorTerm(13) = GetNaN()
            End If

            'volume error in highest 25% flows
            If (pStats(13, 2, lSiteIndex) > 0.0#) Then
                lSite.ErrorTerm(14) = 100.0# * ((pStats(13, 1, lSiteIndex) - pStats(13, 2, lSiteIndex)) _
                                           / pStats(13, 2, lSiteIndex))
            Else
                lSite.ErrorTerm(14) = GetNaN()
            End If

            'volume error in highest 50% flows - Becky corrected this comment, which used to say high 25%
            If (pStats(14, 2, lSiteIndex) > 0.0#) Then
                lSite.ErrorTerm(15) = 100.0# * ((pStats(14, 1, lSiteIndex) - pStats(14, 2, lSiteIndex)) _
                                           / pStats(14, 2, lSiteIndex))
            Else
                lSite.ErrorTerm(15) = GetNaN()
            End If

            'total storm volume
            If (pStats(4, 2, lSiteIndex) > 0.0#) Then
                lSite.ErrorTerm(5) = 100.0# * ((pStats(4, 1, lSiteIndex) - pStats(4, 2, lSiteIndex)) _
                                           / pStats(4, 2, lSiteIndex))
            Else
                lSite.ErrorTerm(5) = GetNaN()
            End If

            'summer storm volume
            Dim lSummerStormVolumeError As Double = GetNaN()
            If (pStats(9, 2, lSiteIndex) > 0.0# And pStats(4, 2, lSiteIndex) > 0.0#) Then
                lSummerStormVolumeError = (100.0# * ((pStats(9, 1, lSiteIndex) - pStats(9, 2, lSiteIndex)) _
                                            / pStats(9, 2, lSiteIndex))) '- lSite.ErrorTerm(5)
            End If

            'winter storm volume
            Dim lWinterStormVolumeError As Double = GetNaN()
            If (pStats(10, 2, lSiteIndex) > 0.0# And pStats(4, 2, lSiteIndex) > 0.0#) Then
                lWinterStormVolumeError = (100.0# * ((pStats(10, 1, lSiteIndex) - pStats(10, 2, lSiteIndex)) _
                                            / pStats(10, 2, lSiteIndex))) '- lSite.ErrorTerm(5)
            End If

            'average peak
            Dim lAverageStormPeakError As Double = GetNaN()
            If (pStats(5, 2, lSiteIndex) > 0.0#) Then
                lAverageStormPeakError = (100.0# * ((pStats(5, 1, lSiteIndex) - pStats(5, 2, lSiteIndex)) _
                                            / pStats(5, 2, lSiteIndex)))
            End If

            'error in low flow recession
            If Double.IsNaN(pStats(6, 1, lSiteIndex)) Or
               Double.IsNaN(pStats(6, 2, lSiteIndex)) Then
                lSite.ErrorTerm(2) = GetNaN()
            Else 'okay to calculate this term
                lSite.ErrorTerm(2) = (1.0# - pStats(6, 1, lSiteIndex)) - (1.0# - pStats(6, 2, lSiteIndex))
            End If

            'summer flow volume
            Dim lSummerError As Double = GetNaN()
            If (pStats(7, 2, lSiteIndex) > 0.0#) Then
                lSummerError = 100.0# * ((pStats(7, 1, lSiteIndex) - pStats(7, 2, lSiteIndex)) _
                                           / pStats(7, 2, lSiteIndex))
            End If

            'winter flow volume
            Dim lWinterError As Double = GetNaN()
            If (pStats(8, 2, lSiteIndex) > 0.0#) Then
                lWinterError = 100.0# * ((pStats(8, 1, lSiteIndex) - pStats(8, 2, lSiteIndex)) _
                                           / pStats(8, 2, lSiteIndex))
            End If

            'error in seasonal volume
            If (Double.IsNaN(lSummerError) Or
                   Double.IsNaN(lWinterError)) Then 'one term or the other has not been obtained
                lSite.ErrorTerm(7) = GetNaN()
            Else 'okay to calculate this term
                lSite.ErrorTerm(7) = (lSummerError - lWinterError) 'Becky changed this so that it was no longer absolute value
                'so that we can tell which is the greater error (if negative, winter bigger error than summer and vice-versa)
                'lSite.ErrorTerm(7) = Math.Abs(lSummerError - lWinterError)
            End If

            lSite.ErrorTerm(17) = lSummerError
            lSite.ErrorTerm(18) = lWinterError
            lSite.ErrorTerm(8) = lSummerStormVolumeError
            lSite.ErrorTerm(19) = lWinterStormVolumeError
            lSite.ErrorTerm(16) = lAverageStormPeakError

            lSite.ErrorTerm(20) = pCountOfMissingData


        Next lSiteIndex

        Dim lStr As String = StatReportAsString(aUci)
        Return lStr
    End Function

    Private Function StatReportAsString(ByVal aUci As atcUCI.HspfUci) As String
        Dim lStr As String
        lStr = aUci.GlobalBlock.RunInf.Value & vbCrLf
        lStr &= "Expert System Statistics for " & aUci.Name & vbCrLf
        lStr &= "UCI Edited: ".PadLeft(15) & FileDateTime(aUci.Name) & vbCrLf
        lStr &= TimeSpanAsString(SDateJ, EDateJ, "Analysis Period: ")

        For lSiteIndex As Integer = 1 To Sites.Count
            Dim lSite As HexSite = Sites(lSiteIndex - 1)
            lStr &= "Site: ".PadLeft(15) & lSite.Name & vbCrLf
            If pPercentOfMissingData > 0 Then
                lStr &= "The observed data is not continuous in this analysis period. The analysis utilizes " & vbCrLf &
                  "simulated and observed data only on the days (time periods) when observed data are " & vbCrLf &
                 "available. Use the results with caution." & vbCrLf
                lStr &= FormatNumber(pPercentOfMissingData, 1) & "% of observed data is missing." & vbCrLf & vbCrLf
            End If
            'statistics summary
            lStr &= StatDetails("Total (" & YearCount(SDateJ, EDateJ) & " year run)", lSiteIndex, 1)
            lStr &= StatDetails("Annual Average", lSiteIndex, YearCount(SDateJ, EDateJ))

            'Write the error terms
            lStr &= Space(35) & "Error Terms" & vbCrLf & vbCrLf
            lStr &= Space(35) & "Current".PadLeft(12) & "Criteria".PadLeft(12) & vbCrLf
            For lErrorPrintIndex As Integer = 1 To lSite.ErrorCriteria.Count
                For lErrorTerm As Integer = 1 To lSite.ErrorCriteria.Count
                    Dim lErrorCriterion As HexErrorCriterion = lSite.ErrorCriteria.Criterion(lErrorTerm)
                    If lErrorCriterion.PrintPosition = lErrorPrintIndex Then
                        If lSite.ErrorTerm(lErrorTerm) <> 0.0# Then
                            lStr &= (lSite.ErrorCriteria(lErrorTerm).Name & " =").PadLeft(35) &
                                    DecimalAlign(lSite.ErrorTerm(lErrorTerm))
                            If lSite.ErrorCriteria(lErrorTerm).Value > 0 Then
                                lStr &= DecimalAlign(lSite.ErrorCriteria(lErrorTerm).Value)
                                If Math.Abs(lSite.ErrorTerm(lErrorTerm)) < lSite.ErrorCriteria(lErrorTerm).Value Then
                                    lStr &= " OK"
                                Else
                                    lStr &= "    Needs Work"
                                End If
                            End If
                            lStr &= vbCrLf
                        End If
                        Exit For
                    End If
                Next lErrorTerm
            Next lErrorPrintIndex
            lStr &= vbCrLf & vbCrLf
        Next lSiteIndex

        Return lStr
    End Function

    Private Function StatDetails(ByVal aTitle As String, ByVal aSite As Integer, ByVal aConv As Double) As String
        Dim lStr As String = Space(30) & aTitle & vbCrLf & vbCrLf
        lStr &= Space(30) & _
              "Observed".PadLeft(15) & _
              "Simulated".PadLeft(15)
        If Not pFlowOnly Then
            lStr &= "Simulated".PadLeft(15) & _
                    "Simulated".PadLeft(15)
        End If
        lStr &= vbCrLf
        lStr &= Space(30) & _
              "Total Runoff".PadLeft(15) & _
              "Total Runoff".PadLeft(15)
        If Not pFlowOnly Then
            lStr &= "Surface Runoff".PadLeft(15) & _
                    "Interflow".PadLeft(15)
        End If
        lStr &= vbCrLf
        'Write runoff block
        For lStatPrintIndex As Integer = 1 To pStatistics.Count 'loop for each statistic to print
            For lStatIndex As Integer = 1 To pStatistics.Count
                Dim lStatistic As HexStatistic = pStatistics(lStatIndex - 1)
                If lStatistic.PrintPosition = lStatPrintIndex Then
                    lStr &= (lStatistic.Name & " =").PadLeft(30)
                    Dim lColumnPointer() As Integer = {0, 2, 1, 3, 4} 'gets print order correct within statistic
                    Dim lColumnMax As Integer = 4
                    If pFlowOnly Then lColumnMax = 2
                    For lColumn As Integer = 1 To lColumnMax
                        If Not Double.IsNaN(pStats(lStatIndex, lColumnPointer(lColumn), aSite)) Then
                            Dim lConv As Double = aConv
                            If lStatIndex = 5 Or lStatIndex = 6 Then 'dont need adjustment for storm peaks or recession rate
                                lConv = 1
                            End If
                            lStr &= DecimalAlign(pStats(lStatIndex, lColumnPointer(lColumn), aSite) / lConv, 15)
                        Else
                            lStr &= Space(15)
                        End If
                    Next lColumn
                    lStr = lStr.TrimEnd & vbCrLf
                    Exit For
                End If
            Next lStatIndex
        Next lStatPrintIndex
        lStr &= vbCrLf

        If Not pFlowOnly Then 'Write EvapoTranspiration block
            lStr &= Space(30) & "EvapoTranspiration".PadRight(28) & vbCrLf
            lStr &= Space(30) & "Potential".PadLeft(15) & "Actual".PadLeft(15) & vbCrLf
            lStr &= ("total (inches) = ").PadLeft(30)
            lStr &= DecimalAlign(pStats(1, 5, aSite) / aConv, 15)
            lStr &= DecimalAlign(pStats(1, 6, aSite) / aConv, 15)
            lStr &= vbCrLf
        End If

        lStr &= vbCrLf
        Return lStr
    End Function

    Private Sub ZipR(ByVal aLength As Long, ByVal aZip As Double, ByVal lArray(,,) As Double, _
                     ByVal aSecondDim As Long, ByVal aThirdDim As Long)
        'Fill the array X of size Length with the given value aZIP.
        For lIndex As Integer = 1 To aLength
            lArray(lIndex, aSecondDim, aThirdDim) = aZip
        Next lIndex
    End Sub
End Class

Public Class HexSite
    Public Area As Double
    Public ReadOnly Name As String
    Public ReadOnly StatDN As Integer
    Public ReadOnly DSN(9) As Integer '2-D. 1st dim = stat# (see below), and 2nd = site#
    '0 = simulated total runoff (in)
    '1 = observed streamflow (cfs)
    '2 = simulated surface runoff (in)
    '3 = simulated interflow (in)
    '4 = simulated base flow (in)
    '5 = precipitation (in)
    '6 = potential evapotranspiration (in)
    '7 = actual evapotranspiration (in)
    '8 = upper zone storage (in)
    '9 = lower zone storage (in)
    Public ErrorTerm(20) As Double
    Friend ErrorCriteria As HexErrorCriteria

    Public Sub New(ByVal aExpertSystem As atcExpertSystem, ByVal aName As String, ByVal aStatDN As Integer, _
                   ByVal aDsn() As Integer) ', ByVal aErrorTerm As Double)
        Name = aName
        StatDN = aStatDN
        DSN = aDsn
        'ReDim ErrorTerm(aExpertSystem.Sites(0).ErrorCriteria.Count)
        ErrorCriteria = New HexErrorCriteria
    End Sub
End Class

Public Class HexStorm
    Public ReadOnly SDateJ As Double
    Public ReadOnly EDateJ As Double

    Public Sub New(ByVal stormindex As Integer, ByVal aStormSDate() As Integer, ByVal aStormEDate() As Integer)
        SDateJ = Date2J(aStormSDate)
        EDateJ = Date2J(aStormEDate)
        If SDateJ > EDateJ Then
            Throw New ApplicationException("The storm number " & stormindex & " ends before it starts. Please fix the dates. Program will quit!")
        End If
    End Sub
End Class

Friend Class HexErrorCriteria
    Implements IEnumerable

    Private pErrorCriteria As New atcCollection

    Public Sub New()
        '1 = acceptable error in total volume
        '2 = acceptable error in low-flow recession (ratio q(t-1)/q(t))
        '3 = acceptable error in 50 percent lowest flows (%)
        '4 = acceptable error in 10 percent highest flows (%)
        '5 = acceptable error in storm values (%)
        '6 = ration of interflow to surface runoff (in/in)
        '7 = acceptable error in seasonal volume (%)
        '8 = acceptable error in summer storm volumes (%)
        '9 = multiplier on third and fourth error terms
        '10 = percent of flows to use in low-flow recession error
        pErrorCriteria.Add("E1", New HexErrorCriterion("Error in total volume (%)", 1, 10))
        pErrorCriteria.Add("E2", New HexErrorCriterion("Error in low-flow recession", 8, 0.03))
        pErrorCriteria.Add("E3", New HexErrorCriterion("Error in 50% lowest flows (%)", 5, 10))
        pErrorCriteria.Add("E4", New HexErrorCriterion("Error in 10% highest flows (%)", 2, 15))
        pErrorCriteria.Add("E5", New HexErrorCriterion("Error in storm volumes (%)", 9, 15))
        pErrorCriteria.Add("E6", New HexErrorCriterion("Ratio of interflow to surface runoff (in/in)", 10, 2.5))
        pErrorCriteria.Add("E7", New HexErrorCriterion("Seasonal volume error (%)", 11, 20))
        pErrorCriteria.Add("E8", New HexErrorCriterion("Summer storm volume error (%)", 18, 15))
        pErrorCriteria.Add("E9", New HexErrorCriterion("Multiplier on third and fourth error terms", 14, 1.5))
        pErrorCriteria.Add("E10", New HexErrorCriterion("Percent of flows to use in low-flow recession error", 15, 30))
        pErrorCriteria.Add("E11", New HexErrorCriterion("Average storm peak flow error (%)", 12, 15))
        pErrorCriteria.Add("E12", New HexErrorCriterion("Error in 10% lowest flows (%)", 7, 20))
        pErrorCriteria.Add("E13", New HexErrorCriterion("Error in 25% lowest flows (%)", 6, 15))
        pErrorCriteria.Add("E14", New HexErrorCriterion("Error in 25% highest flows (%)", 3, 10))
        pErrorCriteria.Add("E15", New HexErrorCriterion("Error in 50% highest flows (%)", 4, 10))
        pErrorCriteria.Add("E16", New HexErrorCriterion("Error in average storm peak (%)", 13, 15))
        pErrorCriteria.Add("E17", New HexErrorCriterion("Summer volume error (%)", 16, 20))
        pErrorCriteria.Add("E18", New HexErrorCriterion("Winter volume error (%)", 17, 15))
        pErrorCriteria.Add("E19", New HexErrorCriterion("Winter storm volume error (%)", 19, 15))
    End Sub
    Public ReadOnly Property Count() As Integer
        Get
            Return pErrorCriteria.Count
        End Get
    End Property
    Default Public ReadOnly Property Criterion(ByVal aIndex As Integer) As HexErrorCriterion
        Get
            Return pErrorCriteria(aIndex - 1)
        End Get
    End Property
    Public Sub Edit()
        Dim lForm As New frmErrorCriteria
        lForm.Edit(Me)
        lForm.ShowDialog()
    End Sub

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return pErrorCriteria.GetEnumerator
    End Function
End Class

Friend Class HexErrorCriterion
    Friend ReadOnly Name As String
    Friend Value As Double
    Friend ReadOnly PrintPosition As Integer

    Public Sub New(ByVal aName As String, ByVal aPrintPosition As String, ByVal aDefault As Double)
        Name = aName
        PrintPosition = aPrintPosition
        Value = aDefault
    End Sub
End Class

Friend Class HexStatistics
    Inherits Generic.List(Of HexStatistic)

    Public Sub New()
        Add(New HexStatistic("total (inches)", 1)) '1
        Add(New HexStatistic("50% low (inches)", 5)) '2
        Add(New HexStatistic("10% high (inches)", 2)) '3
        Add(New HexStatistic("storm volume (inches)", 8)) '4
        Add(New HexStatistic("average storm peak (cfs)", 9)) '5
        Add(New HexStatistic("baseflow recession rate", 10)) '6
        Add(New HexStatistic("summer volume (inches)", 11)) '7
        Add(New HexStatistic("winter volume (inches)", 12)) '8
        Add(New HexStatistic("summer storms (inches)", 13)) '9
        Add(New HexStatistic("winter storms (inches)", 14)) '10
        Add(New HexStatistic("10% low (inches)", 7)) '11
        Add(New HexStatistic("25% low (inches)", 6)) '12
        Add(New HexStatistic("25% high (inches)", 3)) '13
        Add(New HexStatistic("50% high (inches)", 4)) '14
    End Sub
End Class

Friend Class HexStatistic
    Public ReadOnly Name As String
    Public ReadOnly PrintPosition As Integer

    Public Sub New(ByVal aName As String, ByVal aPrintPosition As Integer)
        Name = aName
        PrintPosition = aPrintPosition
    End Sub
End Class

Friend Class HexDatasetTypes
    Private pDatasetTypes As New atcCollection
    '1 = simulated total runoff (in)
    '2 = observed streamflow (cfs)
    '3 = simulated surface runoff (in)
    '4 = simulated interflow (in)
    '5 = simulated base flow (in)
    '6 = precipitation (in)
    '7 = potential evapotranspiration (in)
    '8 = actual evapotranspiration (in)
    '9 = upper zone storage (in)
    '10 = lower zone storage (in)

    Public Sub New()
        'start of stats stored in pStats
        pDatasetTypes.Add("SimTotRunoff", New HexDatasetType("Total Runoff", "Simulated", "in"))
        pDatasetTypes.Add("ObsStreamflow", New HexDatasetType("Streamflow", "Observed", "cfs"))
        pDatasetTypes.Add("SimInterflow", New HexDatasetType("Interflow", "Simulated", "in"))
        pDatasetTypes.Add("SimBaseflow", New HexDatasetType("Baseflow", "Simulated", "in"))
        pDatasetTypes.Add("ObsPotentialET", New HexDatasetType("Potential Evapotranspriation", "Observed", "in"))
        pDatasetTypes.Add("SimActualET", New HexDatasetType("Actual Evapotranspriation", "Simulated", "in"))
        'end of stats stored in pStats
        pDatasetTypes.Add("SimSurfaceRunoff", New HexDatasetType("Surface Runoff", "Simulated", "in"))
        pDatasetTypes.Add("ObsPrecipitation", New HexDatasetType("Precipitation", "Observed", "in"))
        pDatasetTypes.Add("SimUpperZoneStorage", New HexDatasetType("Upper Zone Storage", "Simulated", "in"))
        pDatasetTypes.Add("SimLowerZoneStorage", New HexDatasetType("Lower Zone Storage", "Simulated", "in"))
    End Sub

    Default Public ReadOnly Property DatasetType(ByVal aIndex As Integer) As HexDatasetType
        Get
            Return pDatasetTypes(aIndex - 1)
        End Get
    End Property
    Public Function IndexFromKey(ByVal aKey As String) As Integer
        Try
            Return pDatasetTypes.Keys.IndexOf(aKey) + 1
        Catch lEx As Exception
            Return -1
        End Try
    End Function
End Class

Friend Class HexDatasetType
    Public ReadOnly Name As String
    Public ReadOnly Units As String
    Public ReadOnly Type As String

    Public Sub New(ByVal aName As String, ByVal aType As String, ByVal aUnits As String)
        Name = aName
        Type = aType
        Units = aUnits
    End Sub
End Class
