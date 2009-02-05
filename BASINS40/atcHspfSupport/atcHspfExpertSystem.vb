Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcExpertSystem
    Friend pErrorCriteria As New hexErrorCriteria
    Public Storms As New Generic.List(Of hexStorm)
    Public Sites As New Generic.List(Of hexSite)
    Private pStatistics As New hexStatistics
    Private pDatasetTypes As New hexDatasetTypes

    Private pUci As atcUCI.HspfUci
    Private pDataSource As atcTimeseriesSource

    Private pName As String
    Private pSubjectiveData(25) As Integer
    Private pLatMin As Double, pLatMax As Double
    Private pLngMin As Double, pLngMax As Double
    Private pSDateJ As Double, pEDateJ As Double
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

    Public Sub New(ByVal aUci As atcUCI.HspfUci, ByVal aDataSource As atcTimeseriesSource)
        pUci = aUci
        pDataSource = aDataSource
        ReadEXSFile(IO.Path.GetFileNameWithoutExtension(aUci.Name) & ".exs")
        'pErrorCriteria.Edit()
    End Sub

    Public ReadOnly Property SDateJ() As Double
        Get
            Return pSDateJ
        End Get
    End Property
    Public ReadOnly Property EDateJ() As Double
        Get
            Return pEDateJ
        End Get
    End Property

    Public Function Report() As String
        CalcStats(pDataSource)
        Dim lStr As String = CalcErrorTerms(pUci)
        Return lStr
    End Function

    Public Function AsString() As String
        Dim lText As New Text.StringBuilder
        Dim lStr As String = pName.PadRight(8) & _
                             Sites.Count.ToString.PadLeft(5) & _
                             "1".PadLeft(5) & _
                             pLatMin.ToString.PadLeft(8) & _
                             pLatMax.ToString.PadLeft(8) & _
                             pLngMin.ToString.PadLeft(8) & _
                             pLngMax.ToString.PadLeft(8)
        If pSDateJ > 0 Then
            Dim lDate(5) As Integer
            J2Date(pSDateJ, lDate)
            lStr &= lDate(0).ToString.PadLeft(5) & lDate(1).ToString.PadLeft(2) & lDate(2).ToString.PadLeft(2)
            J2Date(pEDateJ, lDate)
            lStr &= lDate(0).ToString.PadLeft(6) & lDate(1).ToString.PadLeft(2) & lDate(2).ToString.PadLeft(2)
        End If
        lText.AppendLine(lStr)
        For Each lSite As hexSite In Sites
            lStr = ""
            With lSite
                For lDsnIndex As Integer = 0 To 9
                    lStr &= .Dsn(lDsnIndex).ToString.PadLeft(4)
                Next lDsnIndex
                lStr &= .StatDN.PadLeft(3) & "  " & .Name
            End With
            lText.AppendLine(lStr)
        Next
        lText.AppendLine(Storms.Count.ToString.PadLeft(4))
        For Each lStorm As hexStorm In Storms
            lStr = ""
            With lStorm
                Dim lDate(5) As Integer
                J2Date(.SDateJ, lDate)
                lStr &= lDate(0).ToString.PadLeft(5)
                For lDateIndex As Integer = 1 To 5
                    lStr &= lDate(lDateIndex).ToString.PadLeft(3)
                Next
                J2Date(.EDateJ, lDate)
                lStr &= lDate(0).ToString.PadLeft(5)
                For lDateIndex As Integer = 1 To 5
                    lStr &= lDate(lDateIndex).ToString.PadLeft(3)
                Next
            End With
            lText.AppendLine(lStr)
        Next

        lStr = ""
        For Each lSite As hexSite In Sites
            lStr &= lSite.Area.ToString.PadLeft(8)
        Next
        lText.AppendLine(lStr)

        lStr = ""
        For Each lError As hexErrorCriterion In pErrorCriteria
            lStr &= Format(lError.Value, "#####.00").PadLeft(8)
        Next
        lText.AppendLine(lStr)

        For lSiteIndex As Integer = 1 To Sites.Count
            lStr = ""
            For lIndex As Integer = 0 To 7
                lStr &= DecimalAlign(pHSPFOutput1(lIndex + 1, lSiteIndex - 1), 8)
            Next lIndex
            lText.AppendLine(lStr)
            lStr = ""
            For lIndex As Integer = 0 To 7
                lStr &= DecimalAlign(pHSPFOutput2(lIndex + 1, lSiteIndex - 1), 8)
            Next lIndex
            lText.AppendLine(lStr)
            lStr = ""
            For lIndex As Integer = 0 To 5
                lStr &= DecimalAlign(pHSPFOutput3(lIndex + 1, lSiteIndex - 1), 8)
            Next lIndex
            lText.AppendLine(lStr)
        Next lSiteIndex

        lStr = ""
        For lIndex As Integer = 0 To 19
            lStr &= pSubjectiveData(lIndex + 1).ToString.PadLeft(4)
        Next lIndex
        lText.AppendLine(lStr)

        lStr = ""
        For lIndex As Integer = 20 To 22
            lStr &= pSubjectiveData(lIndex + 1).ToString.PadLeft(4)
        Next lIndex
        lText.AppendLine(lStr)
        Return lText.ToString
    End Function

    Private Sub ReadEXSFile(ByVal aFilename As String)
        If Not FileExists(aFilename) Then
            Throw New ApplicationException("ExpertSystemFile " & aFilename & " not found")
        Else
            Dim lExsFileString As String = WholeFileString(aFilename)
            Dim lExsRecords() As String = lExsFileString.Split(vbLf)

            'Read first line of file
            Dim lExsRecord As String = lExsRecords(0).PadRight(51)
            pName = lExsRecord.Substring(0, 8)
            Dim lNSites As Integer = lExsRecord.Substring(8, 5)
            Dim lCurSite As Integer = lExsRecord.Substring(14, 5)
            pLatMin = lExsRecord.Substring(19, 8)
            pLatMax = lExsRecord.Substring(27, 8)
            pLngMin = lExsRecord.Substring(35, 8)
            pLngMax = lExsRecord.Substring(43, 8)
            If lExsRecord.Length = 51 Then
                pSDateJ = pUci.GlobalBlock.SDateJ
                pEDateJ = pUci.GlobalBlock.EdateJ
            Else
                lExsRecord.PadRight(70)
                Dim lDate(5) As Integer
                lDate(0) = lExsRecord.Substring(52, 4)
                lDate(1) = lExsRecord.Substring(56, 2)
                lDate(2) = lExsRecord.Substring(58, 2)
                pSDateJ = Date2J(lDate)
                lDate(0) = lExsRecord.Substring(62, 4)
                lDate(1) = lExsRecord.Substring(66, 2)
                lDate(2) = lExsRecord.Substring(68, 2)
                pEDateJ = Date2J(lDate)
            End If

            'Default unspecified lat/integer min/max values to contiguous 48 states
            If ((pLatMin < 0.01) And (pLatMin > -0.01)) Then
                pLatMin = 24
            End If
            If ((pLatMax < 0.01) And (pLatMax > -0.01)) Then
                pLatMax = 50
            End If
            If ((pLngMin < 0.01) And (pLngMin > -0.01)) Then
                pLngMin = 66
            End If
            If ((pLngMax < 0.01) And (pLngMax > -0.01)) Then
                pLngMax = 125
            End If

            'Read Site block
            For lSiteIndex As Integer = 1 To lNSites
                lExsRecord = lExsRecords(lSiteIndex)
                Dim lDsn(9) As Integer
                For lConsIndex As Integer = 0 To 9
                    lDsn(lConsIndex) = lExsRecord.Substring(lConsIndex * 4, 4)
                Next lConsIndex
                Dim lStatDN As Integer = lExsRecord.Substring(42, 2)  '0 or 1
                Dim lName As String = lExsRecord.Substring(45).Replace(vbCr, "").Trim
                Dim lSite As New hexSite(Me, lName, lStatDN, lDsn)
                Sites.Add(lSite)
            Next lSiteIndex

            Dim lRecordIndex As Integer = lNSites + 1
            'Read number of storms
            Dim lNStorms As Integer = lExsRecords(lRecordIndex).Substring(0, 4)

            'Read storm end/start dates
            Dim lStormSDate(5) As Integer
            Dim lStormEDate(5) As Integer
            For lStormIndex As Integer = 1 To lNStorms
                lExsRecord = lExsRecords(lRecordIndex + lStormIndex)
                lStormSDate(0) = lExsRecord.Substring(0, 5) 'Left(textLine, 5)
                lStormEDate(0) = lExsRecord.Substring(21, 5) 'Mid(textLine, 21, 5)
                For lTimeIndex As Integer = 0 To 4
                    lStormSDate(lTimeIndex + 1) = lExsRecord.Substring(6 + 3 * lTimeIndex, 3)
                    lStormEDate(lTimeIndex + 1) = lExsRecord.Substring(26 + 3 * lTimeIndex, 3)
                Next lTimeIndex
                'Get the starting and ending storm dates in a 1-D Julian array
                Storms.Add(New hexStorm(lStormSDate, lStormEDate))
            Next lStormIndex

            'Read basin area (acres)
            lRecordIndex += lNStorms + 1
            lExsRecord = lExsRecords(lRecordIndex)
            For lSiteIndex As Integer = 0 To lNSites - 1
                Sites(lSiteIndex).Area = lExsRecord.Substring((lSiteIndex * 8), 8)
            Next lSiteIndex

            'Read error terms
            lRecordIndex += 1 'lNSites
            lExsRecord = lExsRecords(lRecordIndex)
            lRecordIndex += 1
            For lErrorIndex As Integer = 1 To 10
                pErrorCriteria(lErrorIndex).Value = lExsRecord.Substring((lErrorIndex - 1) * 8, 8)
            Next lErrorIndex
            pErrorCriteria(11).Value = 15  'storm peak criteria not kept in EXS file
            If (pErrorCriteria(10).Value < 0.000001 And pErrorCriteria(10).Value > -0.000001) Then
                'percent of time in baseflow read in as zero, change to 30
                pErrorCriteria(10).Value = 30.0#
            End If

            'Read latest hspf output
            ReDim pHSPFOutput1(8, Sites.Count)
            ReDim pHSPFOutput2(8, Sites.Count)
            ReDim pHSPFOutput3(6, Sites.Count)
            For lSiteIndex As Integer = 0 To Sites.Count - 1
                lExsRecord = lExsRecords(lRecordIndex).PadRight(80)
                lRecordIndex += 1
                For lIndex As Integer = 0 To 7
                    pHSPFOutput1(lIndex + 1, lSiteIndex) = lExsRecord.Substring(8 * lIndex, 8)
                Next lIndex
                lExsRecord = lExsRecords(lRecordIndex).PadRight(80)
                lRecordIndex += 1
                For lIndex As Integer = 0 To 7
                    pHSPFOutput2(lIndex + 1, lSiteIndex) = lExsRecord.Substring(8 * lIndex, 8)
                Next lIndex
                lExsRecord = lExsRecords(lRecordIndex).PadRight(80)
                lRecordIndex += 1
                For lIndex As Integer = 0 To 5
                    pHSPFOutput3(lIndex + 1, lSiteIndex) = lExsRecord.Substring(8 * lIndex, 8)
                Next lIndex
            Next lSiteIndex

            'Flags for ancillary data (1=yes, 0=no, -1=unknown, -2=undefined)
            lExsRecord = lExsRecords(lRecordIndex).PadRight(80)
            For lIndex As Integer = 0 To 19
                pSubjectiveData(lIndex + 1) = lExsRecord.Substring(lIndex * 4, 4)
            Next lIndex
            lRecordIndex += 1
            lExsRecord = lExsRecords(lRecordIndex).PadRight(80)
            For lIndex As Integer = 20 To 22
                pSubjectiveData(lIndex + 1) = lExsRecord.Substring((lIndex - 20) * 4, 4)
            Next lIndex
            '  'Change subjective data based on other data
            '  If (SISTVO(CURSIT) > OBSTVO(CURSIT)) Then
            '    'Simulated storm runoff volumes higher than obs
            '    SISROV = 1
            '  ElseIf (SISTVO(CURSIT) < OBSTVO(CURSIT)) Then
            '    'Simulated storm runoff volumes lower than obs
            '    SISROV = 0
            '  End If
        End If
    End Sub

    Private Sub CalcStats(ByVal aDataSource As atcTimeseriesSource)
        Dim lDataSetTypes() As String = {"SimTotRunoff", "ObsStreamflow", _
                                         "SimInterflow", "SimBaseflow", _
                                         "ObsPotentialET", "SimActualET"}
        ReDim pStats(pStatistics.Count, lDataSetTypes.GetUpperBound(0) + 1, Sites.Count)

        'get number of values
        Dim lTimeStep As Integer, lTimeUnit As Integer, lNVals As Integer
        lTimeStep = 1
        lTimeUnit = 4 'day
        lNVals = timdifJ(pSDateJ, pEDateJ, lTimeUnit, lTimeStep)

        For lSiteIndex As Integer = 1 To Sites.Count
            Dim lSite As hexSite = Sites(lSiteIndex - 1)
            For Each lDatasetType As String In lDataSetTypes ' As Integer = 1 To pDatasetTypes.Count
                Dim lStatGroup As Integer = pDatasetTypes.IndexFromKey(lDatasetType)
                'set Stats to undefined for this group
                ZipR(pStatistics.Count, GetNaN, pStats, lStatGroup, lSiteIndex)

                Dim lDSN As Integer
                With lSite
                    Select Case lStatGroup 'get the correct dsn
                        Case 1 : lDSN = .Dsn(0)
                        Case 2 : lDSN = .Dsn(1)
                        Case 3 : lDSN = .Dsn(2)
                        Case 4 : lDSN = .Dsn(3)
                        Case 5 : lDSN = .Dsn(6)
                        Case 6 : lDSN = .Dsn(7)
                    End Select
                End With
                'Get data - daily values and max values as necessary
                Dim lTSer As atcTimeseries = aDataSource.DataSets(aDataSource.DataSets.IndexFromKey(lDSN))
                'subset by date to simulation period
                Dim lNewTSer As atcTimeseries = SubsetByDate(lTSer, pSDateJ, pEDateJ, Nothing)
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

                If lDataProblem Then  'if we weren't able to retrieve the data set
                    'set Stats to undefined
                    ZipR(pStatistics.Count, GetNaN, pStats, lStatGroup, lSiteIndex)
                    Logger.Msg("Unable to retrieve DSN " & lDSN & vbCrLf & _
                               "from the file " & aDataSource.Name, "Bad Data Set")
                Else  'generate statistics
                    Dim lValues() As Double = lDailyTSer.Values
                    'total volume always needed 
                    pStats(1, lStatGroup, lSiteIndex) = lDailyTSer.Attributes.GetDefinedValue("Sum").Value
                    'others?
                    If (lStatGroup = 1 Or lStatGroup = 2) Then  'full range of pStats desired
                        pStats(2, lStatGroup, lSiteIndex) = lDailyTSer.Attributes.GetValue("%Sum50") '50% low
                        pStats(3, lStatGroup, lSiteIndex) = lDailyTSer.Attributes.GetValue("Sum") - lDailyTSer.Attributes.GetValue("%Sum90") '10% high
                        pStats(11, lStatGroup, lSiteIndex) = lDailyTSer.Attributes.GetValue("%Sum10") '10% low
                        pStats(12, lStatGroup, lSiteIndex) = lDailyTSer.Attributes.GetValue("%Sum25") '25% low
                        pStats(13, lStatGroup, lSiteIndex) = lDailyTSer.Attributes.GetValue("Sum") - lDailyTSer.Attributes.GetValue("%Sum75") '25% high
                        pStats(14, lStatGroup, lSiteIndex) = lDailyTSer.Attributes.GetValue("Sum") - lDailyTSer.Attributes.GetValue("%Sum50") '50% high

                        Dim lTmpDate(5) As Integer
                        J2Date(pSDateJ, lTmpDate)

                        pStats(7, lStatGroup, lSiteIndex) = 0.0# 'summer volume
                        pStats(8, lStatGroup, lSiteIndex) = 0.0# 'winter volume
                        For i As Integer = 1 To lDailyTSer.numValues
                            If (lTmpDate(1) = 12 Or lTmpDate(1) = 1 Or lTmpDate(1) = 2) Then
                                'in the winter
                                pStats(8, lStatGroup, lSiteIndex) += lValues(i)
                            ElseIf (lTmpDate(1) = 6 Or lTmpDate(1) = 7 Or lTmpDate(1) = 8) Then
                                'in the summer
                                pStats(7, lStatGroup, lSiteIndex) += lValues(i)
                            End If
                            TIMADD(lTmpDate, lTimeUnit, lTimeStep, lTimeStep, lTmpDate)
                        Next i
                    End If

                    If (lStatGroup >= 1 And lStatGroup <= 4) Then  'calc storm info
                        pStats(4, lStatGroup, lSiteIndex) = 0.0# 'initialize storm volume
                        pStats(5, lStatGroup, lSiteIndex) = 0.0# 'storm peaks
                        pStats(9, lStatGroup, lSiteIndex) = 0.0# 'summer storms
                        pStats(10, lStatGroup, lSiteIndex) = 0.0# 'winter storms
                        For Each lStorm As hexStorm In Storms
                            If lStorm.SDateJ >= pSDateJ And _
                               lStorm.EDateJ <= pEDateJ Then 'storm within run span
                                'TODO: this matches VB6Script results, needs to have indexes checked!
                                Dim lN1 As Integer, lN2 As Integer
                                lN1 = timdifJ(pSDateJ, lStorm.SDateJ, lTimeUnit, lTimeStep) + 1
                                lN2 = timdifJ(pSDateJ, lStorm.EDateJ, lTimeUnit, lTimeStep)
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
                                        If (lTmpDate(1) = 12 Or lTmpDate(1) = 1 Or lTmpDate(1) = 2) Then 'in the winter
                                            pStats(10, lStatGroup, lSiteIndex) += lValues(i)
                                        ElseIf (lTmpDate(1) = 6 Or lTmpDate(1) = 7 Or lTmpDate(1) = 8) Then 'in the summer
                                            pStats(9, lStatGroup, lSiteIndex) += lValues(i)
                                        End If
                                        TIMADD(lTmpDate, lTimeUnit, lTimeStep, lTimeStep, lTmpDate)
                                    Next i
                                    pStats(5, lStatGroup, lSiteIndex) += lRtmp
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
                            If (lSavDat > 0.0000000001) Then 'have some flow
                                lRecession = lRecessionTimser.Values(lIndex) / lSavDat
                            Else 'no flow
                                lRecession = GetNaN()
                            End If
                            lSavDat = lRecessionTimser.Values(lIndex)
                            lRecessionTimser.Values(lIndex - 1) = lRecession
                        Next lIndex
                        lRecessionTimser.Attributes.DiscardCalculated()
                        lRecessionTimser.Attributes.CalculateAll()

                        'new percent of time in base flow term
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
            Dim lSite As hexSite = Sites(lSiteIndex - 1)
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

            'volume error in highest 25% flows
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
            If Double.IsNaN(pStats(6, 1, lSiteIndex)) Or _
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
            If (Double.IsNaN(lSummerError) Or _
                   Double.IsNaN(lWinterError)) Then 'one term or the other has not been obtained
                lSite.ErrorTerm(7) = GetNaN()
            Else 'okay to calculate this term
                lSite.ErrorTerm(7) = Math.Abs(lSummerError - lWinterError)
            End If

            lSite.ErrorTerm(17) = lSummerError
            lSite.ErrorTerm(18) = lWinterError
            lSite.ErrorTerm(8) = lSummerStormVolumeError
            lSite.ErrorTerm(19) = lWinterStormVolumeError
            lSite.ErrorTerm(16) = lAverageStormPeakError
        Next lSiteIndex

        Dim lStr As String = StatReportAsString(auci)
        Return lStr
    End Function

    Private Function StatReportAsString(ByVal aUci As atcUCI.HspfUci) As String
        Dim lStr As String
        lStr = aUci.GlobalBlock.RunInf.Value & vbCrLf
        lStr &= "Expert System Statistics for " & aUci.Name & vbCrLf
        lStr &= "UCI Edited: ".PadLeft(15) & FileDateTime(aUci.Name) & vbCrLf
        lStr &= TimeSpanAsString(pSDateJ, pEDateJ)

        For lSiteIndex As Integer = 1 To Sites.Count
            Dim lSite As hexSite = Sites(lSiteIndex - 1)
            lStr &= "Site: ".PadLeft(15) & lSite.Name & vbCrLf & vbCrLf

            'statistics summary
            lStr &= StatDetails("Total (" & aUci.GlobalBlock.YearCount & " year run)", lSiteIndex, 1)
            lStr &= StatDetails("Annual Average", lSiteIndex, aUci.GlobalBlock.YearCount)

            'Write the error terms
            lStr &= Space(35) & "Error Terms" & vbCrLf & vbCrLf
            lStr &= Space(35) & "Current".PadLeft(12) & "Criteria".PadLeft(12) & vbCrLf
            For lErrorPrintIndex As Integer = 1 To pErrorCriteria.Count
                For lErrorTerm As Integer = 1 To pErrorCriteria.Count
                    Dim lErrorCriterion As hexErrorCriterion = pErrorCriteria.Criterion(lErrorTerm)
                    If lErrorCriterion.PrintPosition = lErrorPrintIndex Then
                        If lSite.ErrorTerm(lErrorTerm) <> 0.0# Then
                            lStr &= (pErrorCriteria(lErrorTerm).Name & " =").PadLeft(35) & _
                                    DecimalAlign(lSite.ErrorTerm(lErrorTerm))
                            If pErrorCriteria(lErrorTerm).Value > 0 Then
                                lStr &= DecimalAlign(pErrorCriteria(lErrorTerm).Value)
                                If Math.Abs(lSite.ErrorTerm(lErrorTerm)) < pErrorCriteria(lErrorTerm).Value Then
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
        Dim lConv As Double
        Dim lStr As String

        lStr = Space(30) & aTitle & vbCrLf & vbCrLf
        lStr &= Space(30) & _
              "Observed".PadLeft(15) & _
              "Simulated".PadLeft(15) & _
              "Simulated".PadLeft(15) & _
              "Simulated".PadLeft(15) & vbCrLf
        lStr &= Space(30) & _
              "Total Runoff".PadLeft(15) & _
              "Total Runoff".PadLeft(15) & _
              "Surface Runoff".PadLeft(15) & _
              "Interflow".PadLeft(15) & vbCrLf
        'Write runoff block
        For lStatPrintIndex As Integer = 1 To pStatistics.Count 'loop for each statistic to print
            For lStatIndex As Integer = 1 To pStatistics.Count
                Dim lStatistic As hexStatistic = pStatistics(lStatIndex - 1)
                If lStatistic.PrintPosition = lStatPrintIndex Then
                    lStr &= (lStatistic.Name & " =").PadLeft(30)
                    Dim l() As Integer = {0, 2, 1, 3, 4} 'gets print order correct within statistic
                    For k As Integer = 1 To 4
                        If Not Double.IsNaN(pStats(lStatIndex, l(k), aSite)) Then
                            If lStatIndex = 5 Or lStatIndex = 6 Then 'dont need adjustment for storm peaks or recession rate
                                lConv = 1
                            Else
                                lConv = aConv
                            End If
                            lStr &= DecimalAlign(pStats(lStatIndex, l(k), aSite) / lConv, 15)
                        Else
                            lStr &= Space(15)
                        End If
                    Next k
                    lStr = lStr.TrimEnd & vbCrLf
                    Exit For
                End If
            Next lStatIndex
        Next lStatPrintIndex
        lStr &= vbCrLf
        'Write EvapoTranspiration block
        lStr &= Space(30) & "          EvapoTranspiration" & vbCrLf
        lStr &= Space(30) & "Potential".PadLeft(15) & "Actual".PadLeft(15) & vbCrLf
        lStr &= ("total (inches) = ").PadLeft(30)
        lStr &= DecimalAlign(pStats(1, 5, aSite) / aConv, 15)
        lStr &= DecimalAlign(pStats(1, 6, aSite) / aConv, 15)
        lStr &= vbCrLf & vbCrLf
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

Public Class hexSite
    Private pName As String
    Private pArea As Double
    Private pStatDN As Integer
    Private pDSN(9) As Integer '2-D. 1st dim = stat# (see below), and 2nd = site#
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
    Private pErrorTerm() As Double

    Public Sub New(ByVal aExpertSystem As atcExpertSystem, ByVal aName As String, ByVal aStatDN As Integer, ByVal aDsn() As Integer)
        pName = aName
        pStatDN = aStatDN
        pDSN = aDsn
        ReDim pErrorTerm(aExpertSystem.pErrorCriteria.Count)
    End Sub
    Public ReadOnly Property Name() As String
        Get
            Return pName
        End Get
    End Property
    Public Property Area() As String
        Set(ByVal aArea As String)
            pArea = aArea
        End Set
        Get
            Return pArea
        End Get
    End Property
    Public ReadOnly Property StatDN() As String
        Get
            Return pStatDN
        End Get
    End Property
    Public ReadOnly Property Dsn(ByVal aConstituentIndex As Integer) As Integer
        Get
            Return pDSN(aConstituentIndex)
        End Get
    End Property
    Public Property ErrorTerm(ByVal aIndex As Integer) As Double
        Get
            Return pErrorTerm(aIndex)
        End Get
        Set(ByVal aValue As Double)
            pErrorTerm(aIndex) = aValue
        End Set
    End Property
End Class

Public Class hexStorm
    Private pSDateJ As Double
    Private pEDateJ As Double

    Public Sub New(ByVal aStormSDate() As Integer, ByVal aStormEDate() As Integer)
        pSDateJ = Date2J(aStormSDate)
        pEDateJ = Date2J(aStormEDate)
    End Sub
    Public ReadOnly Property SDateJ() As Double
        Get
            Return pSDateJ
        End Get
    End Property
    Public ReadOnly Property EDateJ() As Double
        Get
            Return pEDateJ
        End Get
    End Property
End Class

Friend Class hexErrorCriteria
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
        pErrorCriteria.Add("E1", New hexErrorCriterion("Error in total volume (%)", 1))
        pErrorCriteria.Add("E2", New hexErrorCriterion("Error in low-flow recession", 8))
        pErrorCriteria.Add("E3", New hexErrorCriterion("Error in 50% lowest flows (%)", 5))
        pErrorCriteria.Add("E4", New hexErrorCriterion("Error in 10% highest flows (%)", 2))
        pErrorCriteria.Add("E5", New hexErrorCriterion("Error in storm volumes (%)", 9))
        pErrorCriteria.Add("E6", New hexErrorCriterion("Ratio of interflow to surface runoff (in/in)", 10))
        pErrorCriteria.Add("E7", New hexErrorCriterion("Seasonal volume error (%)", 11))
        pErrorCriteria.Add("E8", New hexErrorCriterion("Summer storm volume error (%)", 18))
        pErrorCriteria.Add("E9", New hexErrorCriterion("Multiplier on third and fourth error terms", 14))
        pErrorCriteria.Add("E10", New hexErrorCriterion("Percent of flows to use in low-flow recession error", 15))
        pErrorCriteria.Add("E11", New hexErrorCriterion("Average storm peak flow error (%)", 12))
        pErrorCriteria.Add("E12", New hexErrorCriterion("Error in 10% lowest flows (%)", 7))
        pErrorCriteria.Add("E13", New hexErrorCriterion("Error in 25% lowest flows (%)", 6))
        pErrorCriteria.Add("E14", New hexErrorCriterion("Error in 25% highest flows (%)", 3))
        pErrorCriteria.Add("E15", New hexErrorCriterion("Error in 50% highest flows (%)", 4))
        pErrorCriteria.Add("E16", New hexErrorCriterion("Error in average storm peak (%)", 13))
        pErrorCriteria.Add("E17", New hexErrorCriterion("Summer volume error (%)", 16))
        pErrorCriteria.Add("E18", New hexErrorCriterion("Winter volume error (%)", 17))
        pErrorCriteria.Add("E19", New hexErrorCriterion("Winter storm volume error (%)", 19))
    End Sub
    Public ReadOnly Property Count() As Integer
        Get
            Return pErrorCriteria.Count
        End Get
    End Property
    Default Public ReadOnly Property Criterion(ByVal aIndex As Integer) As hexErrorCriterion
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

Friend Class hexErrorCriterion
    Private pName As String
    Friend Value As Double
    Private pPrintPosition As Integer

    Public Sub New(ByVal aName As String, ByVal aPrintPosition As String)
        pName = aName
        pPrintPosition = aPrintPosition
    End Sub

    Friend ReadOnly Property Name() As String
        Get
            Return pName
        End Get
    End Property
    Friend ReadOnly Property PrintPosition() As Integer
        Get
            Return pPrintPosition
        End Get
    End Property
End Class

Friend Class hexStatistics
    Inherits Generic.List(Of hexStatistic)

    Public Sub New()
        Add(New hexStatistic("total (inches)", 1)) '1
        Add(New hexStatistic("50% low (inches)", 5)) '2
        Add(New hexStatistic("10% high (inches)", 2)) '3
        Add(New hexStatistic("storm volume (inches)", 8)) '4
        Add(New hexStatistic("average storm peak (cfs)", 9)) '5
        Add(New hexStatistic("baseflow recession rate", 10)) '6
        Add(New hexStatistic("summer volume (inches)", 11)) '7
        Add(New hexStatistic("winter volume (inches)", 12)) '8
        Add(New hexStatistic("summer storms (inches)", 13)) '9
        Add(New hexStatistic("winter storms (inches)", 14)) '10
        Add(New hexStatistic("10% low (inches)", 7)) '11
        Add(New hexStatistic("25% low (inches)", 6)) '12
        Add(New hexStatistic("25% high (inches)", 3)) '13
        Add(New hexStatistic("50% high (inches)", 4)) '14
    End Sub

End Class

Friend Class hexStatistic
    Dim pName As String
    Dim pPrintPosition As Integer

    Public Sub New(ByVal aName As String, ByVal aPrintPosition As Integer)
        pName = aName
        pPrintPosition = aPrintPosition
    End Sub
    Public ReadOnly Property Name() As String
        Get
            Return pName
        End Get
    End Property
    Public ReadOnly Property PrintPosition() As String
        Get
            Return pPrintPosition
        End Get
    End Property
End Class

Friend Class hexDatasetTypes
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
        pDatasetTypes.Add("SimTotRunoff", New hexDatasetType("Total Runoff", "Simulated", "in"))
        pDatasetTypes.Add("ObsStreamflow", New hexDatasetType("Streamflow", "Observed", "cfs"))
        pDatasetTypes.Add("SimInterflow", New hexDatasetType("Interflow", "Simulated", "in"))
        pDatasetTypes.Add("SimBaseflow", New hexDatasetType("Baseflow", "Simulated", "in"))
        pDatasetTypes.Add("ObsPotentialET", New hexDatasetType("Potential Evapotranspriation", "Observed", "in"))
        pDatasetTypes.Add("SimActualET", New hexDatasetType("Actual Evapotranspriation", "Simulated", "in"))
        'end of stats stored in pStats
        pDatasetTypes.Add("SimSurfaceRunoff", New hexDatasetType("Surface Runoff", "Simulated", "in"))
        pDatasetTypes.Add("ObsPrecipitation", New hexDatasetType("Precipitation", "Observed", "in"))
        pDatasetTypes.Add("SimUpperZoneStorage", New hexDatasetType("Upper Zone Storage", "Simulated", "in"))
        pDatasetTypes.Add("SimLowerZoneStorage", New hexDatasetType("Lower Zone Storage", "Simulated", "in"))
    End Sub

    Default Public ReadOnly Property DatasetType(ByVal aIndex As Integer) As hexDatasetType
        Get
            Return pDatasetTypes(aIndex - 1)
        End Get
    End Property
    Public Function IndexFromKey(ByVal aKey As String) As Integer
        Try
            Return pDatasetTypes.Keys.IndexOf(aKey) + 1
        Catch e As Exception
            Return -1
        End Try
    End Function
End Class

Friend Class hexDatasetType
    Dim pName As String
    Dim pUnits As String
    Dim pType As String

    Public Sub New(ByVal aName As String, ByVal aType As String, ByVal aUnits As String)
        pName = aName
        pType = aType
        pUnits = aUnits
    End Sub
    Public ReadOnly Property Name() As String
        Get
            Return pName
        End Get
    End Property
    Public ReadOnly Property Type() As String
        Get
            Return pType
        End Get
    End Property
    Public ReadOnly Property Units() As String
        Get
            Return pUnits
        End Get
    End Property
End Class
