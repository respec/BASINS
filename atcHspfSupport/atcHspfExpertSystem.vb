Imports atcUtility
Imports MapWinUtility

Public Class ExpertSystem
    Private pName As String
    Private pNStorms As Integer

    Private pSites As atcCollection 'of site

    Private pStormSDates(,) As Integer
    Private pStormEDates(,) As Integer
    Private pSubjectiveData(20) As Integer
    Private pLatMin As Double, pLatMax As Double
    Private pLngMin As Double, pLngMax As Double
    Private pStormSJDates() As Double, pStormEJDates() As Double
    Private pErrorCriteria(11) As Double
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

    Public Sub New(ByVal aName As String)
        pName = aName
        pSites = New atcCollection
        ReadEXSFile(aName & ".exs")
    End Sub

    Public ReadOnly Property ErrorCriteria(ByVal aErrorCriteriaIndex As Integer) As Double
        Get
            Return pErrorCriteria(aErrorCriteriaIndex)
        End Get
    End Property
    Public ReadOnly Property Name() As String
        Get
            Return pName
        End Get
    End Property
    Public ReadOnly Property NSites() As Integer
        Get
            Return pSites.Count
        End Get
    End Property
    Public ReadOnly Property NStorms() As Integer
        Get
            Return pNStorms
        End Get
    End Property
    Public ReadOnly Property StormSJDate(ByVal aStormIndex As Integer) As Double
        Get
            Return pStormSJDates(aStormIndex)
        End Get
    End Property
    Public ReadOnly Property StormEJDate(ByVal aStormIndex As Integer) As Double
        Get
            Return pStormEJDates(aStormIndex)
        End Get
    End Property
    Public ReadOnly Property SiteArea(ByVal aSiteIndex As Integer) As Double
        Get
            Return pSites(aSiteIndex - 1).Area
        End Get
    End Property
    Public ReadOnly Property SiteName(ByVal aSiteIndex As Integer) As String
        Get
            Return pSites(aSiteIndex - 1).Name
        End Get
    End Property
    Public ReadOnly Property SiteDsn(ByVal aConstituentIndex As Integer, _
                                     ByVal aSiteIndex As Integer) As Integer
        Get
            Return pSites(aSiteIndex - 1).Dsn(aConstituentIndex)
        End Get
    End Property

    Private Sub ReadEXSFile(ByVal aFilename As String)
        Dim lExsFileString As String = WholeFileString(aFilename)
        Dim lExsRecords() As String = lExsFileString.Split(vbLf)

        'Read first line of file
        Dim lExsRecord As String = lExsRecords(0)
        Dim lNSites As Integer = lExsRecord.Substring(8, 5) 'Mid(textLine, 9, 5)
        Dim lCurSite As Integer = lExsRecord.Substring(14, 5) 'Mid(textLine, 14, 5)
        pLatMin = lExsRecord.Substring(19, 5) 'Mid(textLine, 19, 8)
        pLatMax = lExsRecord.Substring(27, 5) 'Mid(textLine, 27, 8)
        pLngMin = lExsRecord.Substring(35, 5) ' Mid(textLine, 35, 8)
        pLngMax = lExsRecord.Substring(43, 5) 'Mid(textLine, 43, 8)

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
            Dim lSite As New Site(lName, lStatDN, lDsn)
            pSites.Add(lSite)
        Next lSiteIndex

        Dim lRecordIndex As Integer = lNSites + 1
        'Read number of storms
        pNStorms = lExsRecords(lRecordIndex).Substring(0, 4)

        'Read storm end/start dates
        ReDim pStormSDates(5, pNStorms)
        ReDim pStormEDates(5, pNStorms)
        ReDim pStormSJDates(pNStorms)
        ReDim pStormEJDates(pNStorms)
        For lStormIndex As Integer = 1 To pNStorms
            lExsRecord = lExsRecords(lRecordIndex + lStormIndex)
            pStormSDates(0, lStormIndex) = lExsRecord.Substring(0, 5) 'Left(textLine, 5)
            pStormEDates(0, lStormIndex) = lExsRecord.Substring(21, 5) 'Mid(textLine, 21, 5)
            For lTimeIndex As Integer = 0 To 4
                pStormSDates(lTimeIndex + 1, lStormIndex) = lExsRecord.Substring(6 + 3 * lTimeIndex, 3)
                pStormEDates(lTimeIndex + 1, lStormIndex) = lExsRecord.Substring(26 + 3 * lTimeIndex, 3)
            Next lTimeIndex
            'Get the starting and ending storm dates in a 1-D Julian array
            Dim thisStormSDate(5) As Integer, thisStormEDate(5) As Integer
            For lTimeIndex As Integer = 0 To 5
                thisStormSDate(lTimeIndex) = pStormSDates(lTimeIndex, lStormIndex)
                thisStormEDate(lTimeIndex) = pStormEDates(lTimeIndex, lStormIndex)
            Next lTimeIndex
            pStormSJDates(lStormIndex) = Date2J(thisStormSDate)
            pStormEJDates(lStormIndex) = Date2J(thisStormEDate)
        Next lStormIndex

        'Read basin area (acres)
        lRecordIndex += pNStorms + 1
        lExsRecord = lExsRecords(lRecordIndex)
        For lSiteIndex As Integer = 1 To lNSites
            pSites(lSiteIndex - 1).area = lExsRecord.Substring(((lSiteIndex - 1) * 8), 8)
        Next lSiteIndex

        'Read error terms
        lRecordIndex += 1 'lNSites
        lExsRecord = lExsRecords(lRecordIndex)
        lRecordIndex += 1
        For lErrorIndex As Integer = 0 To 9
            pErrorCriteria(lErrorIndex + 1) = lExsRecord.Substring(lErrorIndex * 8, 8)
        Next lErrorIndex
        pErrorCriteria(11) = 15  'storm peak criteria not kept in EXS file
        If (pErrorCriteria(10) < 0.000001 And pErrorCriteria(10) > -0.000001) Then
            'percent of time in baseflow read in as zero, change to 30
            pErrorCriteria(10) = 30.0#
        End If

        'Read latest hspf output
        ReDim pHSPFOutput1(8, pSites.Count)
        ReDim pHSPFOutput2(8, pSites.Count)
        ReDim pHSPFOutput3(6, pSites.Count)
        For lSiteIndex As Integer = 1 To pSites.Count
            lExsRecord = lExsRecords(lRecordIndex)
            lRecordIndex += 1
            For lIndex As Integer = 0 To 7
                pHSPFOutput1(lIndex + 1, lSiteIndex) = lExsRecord.Substring(8 * lIndex, 8)
            Next lIndex
            lExsRecord = lExsRecords(lRecordIndex)
            lRecordIndex += 1
            For lIndex As Integer = 0 To 7
                pHSPFOutput2(lIndex + 1, lSiteIndex) = lExsRecord.Substring(8 * lIndex, 8)
            Next lIndex
            lExsRecord = lExsRecords(lRecordIndex)
            lRecordIndex += 1
            For lIndex As Integer = 0 To 5
                pHSPFOutput3(lIndex + 1, lSiteIndex) = lExsRecord.Substring(8 * lIndex, 8)
            Next lIndex
        Next lSiteIndex

        'Flags for ancillary data (1=yes, 0=no, -1=unknown, -2=undefined)
        lExsRecord = lExsRecords(lRecordIndex)
        For lIndex As Integer = 0 To 19
            pSubjectiveData(lIndex + 1) = lExsRecord.Substring(lIndex * 4, 4)
        Next lIndex
        '  'Change subjective data based on other data
        '  If (SISTVO(CURSIT) > OBSTVO(CURSIT)) Then
        '    'Simulated storm runoff volumes higher than obs
        '    SISROV = 1
        '  ElseIf (SISTVO(CURSIT) < OBSTVO(CURSIT)) Then
        '    'Simulated storm runoff volumes lower than obs
        '    SISROV = 0
        '  End If
    End Sub
End Class

Friend Class Site
    Private pName As String
    Private pArea As Double
    Private pStatDN As Integer
    Private pDSN(9) As Integer '2-D. 1st dim = stat# (see below), and 2nd = site#
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
    Public Sub New(ByVal aName As String, ByVal aStatDN As Integer, ByVal aDsn() As Integer)
        pName = aName
        pStatDN = aStatDN
        pDSN = aDsn
    End Sub
    Public ReadOnly Property Name() As String
        Get
            Return pName
        End Get
    End Property
    Public Property Area() As String
        Set(ByVal value As String)
            pArea = value
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
End Class
