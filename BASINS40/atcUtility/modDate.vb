Option Strict Off
Option Explicit On

Imports System.DateTime

''' <summary>General date utility subroutines and functions</summary>
''' <remarks>Copyright 2001-6 AQUA TERRA Consultants - Royalty-free use permitted under open source license </remarks>
Public Module modDate
    ''' <summary>Standard timeseries time units</summary>
    Public Enum atcTimeUnit
        TUSecond = 1
        TUMinute = 2
        TUHour = 3
        TUDay = 4
        TUMonth = 5
        TUYear = 6
        TUCentury = 7
    End Enum

    ''' <summary>Standard timeseries transformations</summary>
    Public Enum atcTran
        TranAverSame = 0
        TranSumDiv = 1
        TranMax = 2
        TranMin = 3
        TranNative = 4
    End Enum

    ''' <summary>one hour as fraction of a day</summary>
    Public Const JulianHour As Double = 1 / 24

    ''' <summary>one minute as fraction of a day</summary>
    Public Const JulianMinute As Double = 1 / 1440

    ''' <summary>one second as fraction of a day</summary>
    Public Const JulianSecond As Double = 1 / 86400

    ''' <summary>cound of seconds in a day</summary>
    Public Const SecondsPerDay As Integer = 86400

    ''' <summary>one millisecond as fraction of a day</summary>
    Public Const JulianMillisecond As Double = 1 / 86400000

    ''' <summary>estimate of month as number of days</summary>
    ''' <remarks>When doing math on months and years, it is more accurate to use subroutines Timdif and Timadd</remarks>
    Public Const JulianMonth As Double = 30.44

    ''' <summary>estimate of year as number of days</summary>
    ''' <remarks>When doing math on months and years, it is more accurate to use subroutines Timdif and Timadd</remarks>
    Public Const JulianYear As Double = 365.25

    '''' <summary>Julian days from actual year zero to modified Julian day we use as zero</summary>
    'Private Const JulianModification1858 As Integer = 679006 '17 Nov 1858
    Private Const JulianModification1899 As Integer = 694024 '30 Dec 1899
    'This is the offset we actually use
    Public Const JulianModification As Integer = JulianModification1899

    ''' <summary>Three character month names</summary>
    ''' <remarks>TODO: make this international</remarks>
    Public ReadOnly MonthName3 As String() = {"", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}

    '''' <summary>convert a VB date to a modfied Julian date(MJD)</summary>
    '''' <param name="aDate"> VBdate to convert</param>
    '''' <returns>modified Julian date</returns>
    '''' <remarks>VB date 0 is 30Dec1899, MJD date 0 is 17Nov1858</remarks>
    'Public Function VBdate2MJD(ByVal aDate As Date) As Double
    '    Return aDate.ToOADate + JulianModification1899 - JulianModification
    'End Function

    '''' <summary>convert a modified Julian date(MJD) to a VB date</summary>
    '''' <param name="aJDate"> MJD to convert</param>
    '''' <returns>VB date</returns>
    '''' <remarks>VB date 0 is 30Dec1899, MJD date 0 is 17Nov1858</remarks>
    'Public Function MJD2VBdate(ByVal aJDate As Double) As Date
    '    Return FromOADate(aJDate + JulianModification - JulianModification1899)
    'End Function

    ''' <summary>convert a date array to a modfied Julian date (MJD)</summary>
    ''' <param name="aDate">date array to convert</param>
    ''' <returns>modified Julian date</returns>
    ''' <remarks></remarks>
    Public Function Date2J(ByVal aDate() As Integer) As Double
        Dim lJd As Integer
        Dim lHms As Double
        '##LOCAL lJd - date (year, month, day) portion of MJD
        '##LOCAL lHms - time (hour, minute, second) portion of MJD

        lJd = MJD(aDate(0), aDate(1), aDate(2))
        lHms = HMS2J(aDate(3), aDate(4), aDate(5))
        Return lJd + lHms
    End Function

    ''' <summary>convert an hour, minute, and second to a modifed Julian date (MJD)</summary>
    ''' <param name="aHr">hour to convert</param>
    ''' <param name="aMi">minute to convert</param>
    ''' <param name="aSc">second to convert</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function HMS2J(ByVal aHr As Integer, ByVal aMi As Integer, ByVal aSc As Integer) As Double
        Return (CDbl(aHr) * JulianHour) + (CDbl(aMi) * JulianMinute) + (CDbl(aSc) * JulianSecond)
    End Function

    ''' <summary>
    ''' Round up dates to given time unit
    ''' </summary>
    ''' <param name="aJDate"></param>
    ''' <param name="aTU"></param>
    ''' <param name="aDate"></param>
    ''' <remarks></remarks>
    Sub J2DateRoundup(ByVal aJDate As Double, ByVal aTU As Integer, ByVal aDate() As Integer)
        'round up dates for wdm datasets
        'TODO: needs unit tests!
        J2Date(aJDate, aDate)
        Dim lInd As Integer = 7 - aTU
        If aDate(lInd) > 1 Then
            If aTU <= atcTimeUnit.TUDay Then
                aDate(lInd) = 0
            Else
                aDate(lInd) = 1
            End If
            TIMADD(aDate, aTU, 1, 1, aDate)
        End If
    End Sub

    ''' <summary>
    ''' Convert a modified Julian date (MJD) As Double to an array of integers
    ''' representing year, month, day, hour, minute, second
    ''' </summary>
    ''' <param name="aJd">modfied Julian date to convert</param>
    ''' <param name="aDate">output array containing year, month, day, hour, minute, second</param>
    ''' <remarks></remarks>
    Sub J2Date(ByVal aJd As Double, ByRef aDate() As Integer)
        Dim lJd As Integer 'date portion of date to convert
        Dim lJhms As Double 'time portion of date to convert
        Dim lFrac As Double 'fraction of a second resulting from conversion
        If Double.IsNaN(aJd) Then
            lJd = 0
        Else
            lJd = Fix(aJd)
        End If

        Call INVMJD(lJd, aDate(0), aDate(1), aDate(2))
        lJhms = aJd - lJd
        Call J2HMS(lJhms, aDate(3), aDate(4), aDate(5), lFrac)
    End Sub

    Sub J2HMS(ByVal aJd As Double, ByRef aHr As Integer, ByRef aMi As Integer, ByRef aSc As Integer, ByRef aFrac As Double)
        '##SUMMARY J2HMS - convert a time portion of a modfied Julian date to its component parts
        '##Parm aJd - MJD to convert
        '##PARM aHr - hour portion of MJD
        '##PARM aMi - minute portion of MJD
        '##PARM aSc - second portion of MJD
        '##PARM aFrac - fraction of a second
        Dim lRem As Double
        '##LOCAL lRem - intermediate result, units change from hours to seconds
        If Double.IsNaN(aJd) Then
            lRem = 0
        Else
            lRem = 0.0000004 + ((aJd Mod 1) * 24)
        End If

        aHr = Fix(lRem)
        lRem = (lRem - aHr) * 60
        aMi = Fix(lRem)
        lRem = (lRem - aMi) * 60
        aSc = Fix(lRem)
        aFrac = lRem - aSc
    End Sub

    Sub INVMJD(ByVal aMJD As Integer, ByRef aYr As Integer, ByRef aMn As Integer, ByRef aDy As Integer)
        '##SUMMARY INVMJD - invert modified Julian date as computed by function MJD (from DelbertDFranz) _
        'Developed from information given in: "Astronomical Formulae _
        'for Calculators', Jean Meeus, published by Willmann-Bell.
        '##PARM aMJD - value of modified julian date date number to invert
        '##PARM aYr - calendar year
        '##PARM aMn - number of month(1-12)
        '##PARM aDy - day in the month

        Dim e, c, ALPHA, a, b, d, Z As Integer
        '##LOCAL a - intermediate result
        '##LOCAL ALPHA - intermediate result
        '##LOCAL b - intermediate result
        '##LOCAL c - intermediate result
        '##LOCAL d - intermediate result
        '##LOCAL e - intermediate result
        '##LOCAL Z - intermediate result

        'convert to Julian time plus the .5 day correction. yields an integer
        Z = aMJD + JulianModification + 1720994 + 1

        If (Z < 2299161) Then
            a = Z
        Else
            ALPHA = CInt(Fix((CDbl(Z) - CDbl(1867216.24)) / CDbl(36524.25)))
            a = Z + 1 + ALPHA - Int(ALPHA / 4)
        End If

        b = a + 1524
        c = CInt(Fix((CDbl(b) - 122.1) / 365.25))
        d = CInt(Fix(CDbl(365.25) * CDbl(c)))
        e = CInt(Fix(CDbl(b - d) / 30.6001))

        aDy = b - d - Fix(30.6001 * CDbl(e))
        If (e <= 13) Then
            aMn = e - 1
        Else
            aMn = e - 13
        End If
        If (aMn >= 3) Then
            aYr = c - 4716
        Else
            aYr = c - 4715
        End If
    End Sub

    Function MJD(ByVal aYr As Integer, ByVal aMn As Integer, ByVal aDy As Integer) As Integer
        '##SUMMARY MJD - 'Compute modified julian date for any date _
        'with a year greater than 1582 (from DelbertDFranz) _
        'We take the resulting date to represent the elapsed time from _
        'some point in the past to the first instant of the given day. _
        'The date must be later than Nov. 17, 1858 for MJD to be _
        'a positive number.  Thus for use in FEQ the year must be 1859 _
        'or greater. _
        'This routine and INVMJD have been checked by DDF for every day _
        'from 1860 through the year 25000. _
        'Developed from information given in: "Astronomical Formulae _
        'for Calculators', Jean Meeus, published by Willmann-Bell.
        '##PARM aYr - calendar year
        '##PARM aMn - number of month(1-12)
        '##PARM aDy - day in the month

        Dim m, a, b, y As Integer
        '##LOCAL a - intermediate result
        '##LOCAL b - intermediate result
        '##LOCAL m - intermediate result
        '##LOCAL y - intermediate result

        If (aMn > 2) Then
            y = aYr
            m = aMn
        Else
            y = aYr - 1
            m = aMn + 12
        End If

        a = Int(y / 100)
        b = 2 - a + Int(a / 4)

        MJD = Int((36525 * y) / 100) + _
              Int(30.6001 * (m + 1)) + _
              aDy + b - JulianModification
    End Function

    Function Jday(ByVal aYr As Integer, ByVal aMn As Integer, ByVal aDy As Integer, _
                  ByVal aHr As Integer, ByVal aMi As Integer, ByVal aSc As Integer) As Double
        'SUMMARY jday - convert portions of date to a modfied Julian date (MJD)
        '##PARM aYr - calendar year
        '##PARM aMn - number of month(1-12)
        '##PARM aDy - day in the month
        '##PARM aHr - hour of day
        '##PARM aMi - minute of day
        '##PARM aSc - second of day
        Dim d(5) As Integer
        'LOCAL d - date array
        d(0) = aYr
        d(1) = aMn
        d(2) = aDy
        d(3) = aHr
        d(4) = aMi
        d(5) = aSc
        Jday = Date2J(d)
    End Function

    Function JDateIntrvl(ByVal aJd As Double) As Integer
        '##SUMMARY JDateIntrvl - determines the date interval (6-second thru 1-year) of _
        'a modfied Julian date
        '##PARM aJd - MJD to determine interval of
        Dim lDate(5) As Integer
        'LOCAL d - date array
        Call J2Date(aJd, lDate)
        JDateIntrvl = DateIntrvl(lDate)
    End Function

    Function DateIntrvl(ByVal d() As Integer) As Integer
        '##SUMMARY DateIntrvl - determines the date interval (6-second thru 1-year) of a date array
        '##PARM d - date array to determine interval of
        Dim i As Integer
        'LOCAL i - pending date interval
        i = 6 'at least a second boundary
        If d(5) = 0 Then 'more: x x x x x 0
            i = 5 'at least a minute boundary
            If d(4) = 0 Then 'more: x x x x 0 0
                i = 4 'at least an hour boundary
                If d(3) = 0 Then 'more: x x x 0 0 0
                    i = 3 'at least a day boundary
                    If d(2) = 1 Then 'more: x x 1 0 0 0
                        i = 2 'at least a month boundary
                        If d(1) = 1 Then 'more: x 1 1 0 0 0
                            i = 1 'a year boundary
                        End If
                    ElseIf d(2) = 0 Then  'more: x x 0 0 0 0
                        i = 2 'at least a month boundary
                        If d(1) = 0 Then 'more: x 0 0 0 0 0
                            i = 1 'at least a year boundary
                        End If
                    End If
                ElseIf d(3) = 24 Then  'more: x x x 24 0 0
                    i = 3
                    If d(2) = daymon(d(0), d(1)) Then 'more: x x 31,30,29,28 24 0 0
                        i = 2 'month boundary
                        If d(1) = 12 Then 'more: x 12 31 24 0 0
                            i = 1 'year boundary
                        End If
                    End If
                End If
            ElseIf d(4) = 60 Then  'more: x x x x 60 0
                i = 4 'hour boundary
            End If
        ElseIf d(5) = 60 Then  'more: x x x x x 60
            i = 5 'minute boundary
        End If

        DateIntrvl = i
    End Function

    Function daymon(ByVal yr As Integer, ByVal mo As Integer) As Integer
        '##SUMMARY daymon - return the number of days in the given month for the given _
        'year, with leap year taken into account.  For an invalid month, -1 is returned. _
        'For an invalid year and a valid month, the correct number of days is returned, _
        'with February = 28.
        '##PARM yr - year, valid range is 1 - 2080
        '##PARM mo - month, valid range is 1 - 12
        Static ndaymon() As Integer = {0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
        'LOCAL ndaymon - number of days in each month

        If mo < 1 OrElse mo > 12 Then
            daymon = -1 'invalid month

        ElseIf mo = 2 Then  'check for leap year
            If yr <= 0 Or yr > 2080 Then 'invalid year
                daymon = 28
            ElseIf yr Mod 100 = 0 Then
                'check whether this is a leap year on a century boundary
                If yr Mod 400 = 0 Then 'on a 400 year boundary
                    daymon = 29
                Else 'century boundary case
                    daymon = 28
                End If
            ElseIf yr Mod 4 = 0 Then  'leap year
                daymon = 29
            Else 'non leap year
                daymon = 28
            End If

        Else 'valid month, not February
            daymon = ndaymon(mo)
        End If
    End Function

    Public Function addUniqueDate(ByVal j As Double, ByRef ja() As Double, ByRef ji() As Integer) As Boolean
        '##SUMMARY addUniqueDate - adds a unique MJD date to an array of dates if it is not already there _
        'returns true if date added, false if date was already in array
        '##PARM j - MJD date to try to add
        '##PARM ja - array of current dates
        '##PARM ji - array of date intervals corresponding to dates in ja
        Dim i As Integer
        Dim fnd As Boolean
        'LOCAL i - index of current date array
        'LOCAL fnd - date found in current date array flag
        fnd = False 'assmume not found
        For i = 0 To UBound(ja) 'loop thru existing dates
            If j = ja(i) Then 'found the new date
                fnd = True
                Exit For
            End If
        Next i
        If Not fnd Then 'add the date to the array
            i = UBound(ja)
            ReDim Preserve ja(i + 1)
            ReDim Preserve ji(i + 1)
            ja(i + 1) = j
            ji(i + 1) = JDateIntrvl(j)
        End If

        addUniqueDate = Not fnd 'true if date added
    End Function

    Public Function DumpDate(ByVal j As Double) As String ', Optional ByVal s As String = "JDate:"
        '##SUMMARY DumpDate - convert a modfied Julian date to a string
        '##PARM j - date to convert
        '##PARM s - optional prefix for output string
        Dim d(5) As Integer
        'LOCAL d - date array set to values from j
        J2Date(j, d)
        DumpDate = Format(j, "00000.00000") & " : " & _
                   Format(d(0), "0000") & "/" & _
                   Format(d(1), "00") & "/" & _
                   Format(d(2), "00") & " " & _
                   Format(d(3), "00") & ":" & _
                   Format(d(4), "00") & ":" & _
                   Format(d(5), "00")
    End Function

    'Public Sub DTMCMN(ByVal sdates() As Integer, ByVal edates() As Integer, _
    '                  ByVal TSTEP() As Integer, ByVal TCODE() As Integer, _
    '                  ByRef sdat() As Integer, ByRef edat() As Integer, _
    '                  ByRef ts As Integer, ByRef tc As Integer, _
    '                  ByRef retcod As Integer) 'tc As ATCTimeUnit
    '    '##SUMMARY DTMCMN - determine the time period common to a number of pairs of dates. _
    '    'also determines the smallest common time step and unit.
    '    '##PARM sdates - input array of beginning dates
    '    '##PARM edates - input array of ending dates
    '    '##PARM tstep  - input array of time steps
    '    '##PARM tcode  - input array of time units codes _
    '    '1 - second        4 - day _
    '    '2 - minute        5 - month _
    '    '3 - hour          6 - year
    '    '##PARM stat   - output common starting date
    '    '##PARM edat   - output common ending date
    '    '##PARM ts     - output smallest common time step
    '    '##PARM tc     - output smallest common time units code
    '    '##PARM retcod - output return code _
    '    '0 - there is a common time period and time step and units _
    '    '-1 - there is no common time period _
    '    '-2 - there is a common time period, but the time step and _
    '    'time units are not compatible
    '    Dim tstepf, ndat, n, tcdcmp As Integer
    '    '##LOCAL ndat   - number of dates to compare
    '    '##LOCAL n      - index of date being compared
    '    '##LOCAL tstepf - time step compatibility flag _
    '    '0 - compatible time steps _
    '    '1 - incompatible time steps
    '    '##LOCAL tcdcmp - flag indicating order of time steps _
    '    '0 - time steps are the same _
    '    '1 - first time step is smaller _
    '    '2 - second time step is smaller _
    '    '-1 - time units span day-month boundry

    '    DatCmn(sdates, edates, sdat, edat, retcod) 'get common time period

    '    If (retcod = 0) Then 'get common time step and units
    '        ts = TSTEP(1)
    '        tc = TCODE(1)
    '        'check others
    '        ndat = (UBound(sdates) / 6) '- 1 jlk 10/14/99
    '        n = 1
    '        Do While n < ndat And retcod = 0 'look for smallest common time step and unit
    '            cmptim(TCODE(n), TSTEP(n), tc, ts, tstepf, tcdcmp)
    '            If (tstepf = 0 And tcdcmp <> -1) Then 'compatible time steps, do not span day-month boundry
    '                If (tcdcmp = 2) Then 'new larger time step
    '                    ts = TSTEP(n)
    '                    tc = TCODE(n)
    '                End If
    '            Else 'incompatible time steps or time units span day-month boundry
    '                retcod = -2
    '            End If
    '            n = n + 1
    '        Loop
    '        If (retcod = -2) Then 'time step and time units are not all compatible
    '            ts = 0
    '            tc = 0
    '        End If
    '    Else 'no common time period
    '        retcod = -1
    '        ts = 0
    '        tc = 0
    '    End If
    'End Sub

    'Public Sub DatCmn(ByVal sd() As Integer, ByVal ed() As Integer, _
    '                  ByRef SDate() As Integer, ByRef EDate() As Integer, ByRef retcod As Integer)
    '    '##SUMMARY DatCmn - determine the time period common to a number of sets of dates.
    '    '##PARM sd     - input array of beginning dates
    '    '##PARM ed     - input array of ending dates
    '    '##PARM sdate  - output common starting data
    '    '##PARM edate  - output common ending date
    '    '##PARM retcod - output return code _
    '    '0 - there is a common time period _
    '    '-1 - there is no common time period
    '    Dim ljdate As Double
    '    Dim ndat As Integer
    '    Dim sjdate, ejdate As Double
    '    Dim j, i As Integer
    '    Dim d(5) As Integer
    '    '##LOCAL ljdate - temp modified julian date
    '    '##LOCAL ndat   - number of dates to check
    '    '##LOCAL sjdate - earliest modfied julian date
    '    '##LOCAL ejdate - latest modified julian date
    '    '##LOCAL j      - input array pointer
    '    '##LOCAL i      - loop counter thru dates arrays
    '    '##LOCAL d      - temp date array
    '    ndat = (UBound(sd) / 6) ' - 1 jlk 10/14/99
    '    sjdate = -1.0E+30 'way in past
    '    For i = 0 To ndat - 1
    '        j = i * 6
    '        d(0) = sd(j) : d(1) = sd(j + 1) : d(2) = sd(j + 2)
    '        d(3) = sd(j + 3) : d(4) = sd(j + 4) : d(5) = sd(j + 5)
    '        ljdate = Date2J(d)
    '        If ljdate > sjdate Then 'new latest start
    '            sjdate = ljdate
    '        End If
    '    Next i

    '    ejdate = 1.0E+30 'way in future
    '    For i = 0 To ndat - 1
    '        j = i * 6
    '        d(0) = ed(j) : d(1) = ed(j + 1) : d(2) = ed(j + 2)
    '        d(3) = ed(j + 3) : d(4) = ed(j + 4) : d(5) = ed(j + 5)
    '        ljdate = Date2J(d)
    '        If ljdate < ejdate Then 'new earliest end
    '            ejdate = ljdate
    '        End If
    '    Next i

    '    If ejdate > sjdate Then 'common start date before common end date, as hoped for
    '        J2Date(sjdate, SDate)
    '        J2Date(ejdate, EDate)
    '        retcod = 0
    '    Else
    '        For i = 0 To 5
    '            SDate(i) = 0
    '            EDate(i) = 0
    '        Next i
    '        retcod = -1
    '    End If
    'End Sub

    Private Sub cmptim(ByVal tcode1 As Integer, ByVal tstep1 As Integer, _
                       ByVal tcode2 As Integer, ByVal tstep2 As Integer, _
                       ByRef tstepf As Integer, ByRef tcdcmp As Integer) 'tcode1 As ATCTimeUnit, tcode2 As ATCTimeUnit

        '     Compare one time unit and step to a second time unit and
        '     step.  Two flags are returned.  The first flag indicates
        '     compatible/incompatible time steps.  The second flag
        '     indicates which time step is smaller.  Time steps are
        '     considered compatible if one is an even multiple of the
        '     other.  One hour and 30 minutes are compatible; one hour
        '     and 90 minutes are incompatible.  Comparison of time units
        '     and time steps which cross the day-month boundry are handled
        '     a little different.  If the smaller time step is a day or
        '     less and is compatible with 1 day and the larger time step
        '     is compatible with one month, than the smaller and the
        '     larger time steps are considered to be compatible.  The time
        '     step of a day or less will be considered to be the smaller
        '     time step.
        '     EXAMPLES:  TCODE1 TSTEP1 TCODE2 TSTEP2 TSTEPF TCDCMP
        '                  3      1      2      60     0      0
        '                  3      1      2      90     1      1
        '                  3      1      2      30     0      2

        '     TCODE1 - time units code
        '              1 - second    4 - day
        '              2 - minute    5 - month
        '              3 - hour      6 - year
        '     TSTEP1 - time step, in TCODE1 units
        '     TCODE2 - time units code
        '              1 - second    4 - day
        '              2 - minute    5 - month
        '              3 - hour      6 - year
        '     TSTEP2 - time step in TCODE2 units
        '     TSTEPF - time step compatability flag
        '              0 - compatible time steps
        '              1 - incompatible time steps
        '     TCDCMP - flag indicating order of time steps
        '               0 - time steps are the same
        '               1 - first time step is smaller
        '               2 - second time step is smaller
        '              -1 - time units span day-month boundry

        Dim tc(2) As Integer
        Dim ts(2) As Integer
        Dim tsx As Integer 'tc(2) As ATCTimeUnit
        Dim tcx As Integer
        Dim tsfx(2) As Integer
        Dim tcdx(2) As Integer 'tcx As ATCTimeUnit, tcdx(2) As ATCTimeUnit

        tc(1) = tcode1
        tc(2) = tcode2
        ts(1) = tstep1
        ts(2) = tstep2

        'If (tc(1) < TUSecond Or tc(1) > TUYear Or

        If (tc(1) < 1 Or tc(1) > 6 Or tc(2) < 1 Or tc(2) > 6 Or ts(1) < 1 Or ts(1) > 1440 Or ts(2) < 1 Or ts(2) > 1440) Then
            'an invalid time units code or time step
            tstepf = 1
            tcdcmp = -1
        ElseIf ((tc(1) <= 4 And tc(2) >= 5) Or (tc(2) <= 4 And tc(1) >= 5)) Then
            'special case for time units that cross day-month boundry
            tstepf = 1
            tcdcmp = -1
            If (tc(1) <= 4) Then
                'first time unit is day or smaller, second is month or larger
                tsx = 1
                tcx = 4
                Call cmptm2(tc(1), ts(1), tcx, tsx, tsfx(1), tcdx(1))
                tsx = 1
                tcx = 5
                Call cmptm2(tc(2), ts(2), tcx, tsx, tsfx(2), tcdx(2))
                If (tsfx(1) = 0 And tsfx(2) = 0) Then
                    'times compatible with boundaries
                    If ((tcdx(1) = 0 Or tcdx(1) = 1) And (tcdx(2) = 0 Or tcdx(2) = 2)) Then
                        'smaller time a day or less, larger time a month or more
                        tstepf = 0
                        tcdcmp = 1
                    End If
                End If
            Else
                'second time unit is day or smaller, first is month or larger
                tsx = 1
                tcx = 5
                Call cmptm2(tc(1), ts(1), tcx, tsx, tsfx(1), tcdx(1))
                tsx = 1
                tcx = 4
                Call cmptm2(tc(2), ts(2), tcx, tsx, tsfx(2), tcdx(2))
                If (tsfx(1) = 0 And tsfx(2) = 0) Then
                    'times compatible with boundaries
                    If ((tcdx(1) = 0 Or tcdx(1) = 2) And (tcdx(2) = 0 Or tcdx(2) = 1)) Then
                        'larger time a month or more, smaller time a day or less
                        tstepf = 0
                        tcdcmp = 2
                    End If
                End If
            End If
        Else
            'valid time steps and units do not cross day-month boundry
            Call cmptm2(tc(1), ts(1), tc(2), ts(2), tstepf, tcdcmp)
        End If
    End Sub

    Private Sub cmptm2(ByVal tc1 As Integer, ByVal ts1 As Integer, _
                       ByVal tc2 As Integer, ByVal ts2 As Integer, _
                       ByRef tstepf As Integer, ByRef tcdcmp As Integer)

        '     This routine compares one time unit and step to a second time
        '     unit and step.  Two flags are returned.  The first flag
        '     indicates compatible/incompatible time steps.  The second flag
        '     indicates which timestep is smaller.

        '     TC1    - time units code
        '              1 - second    4 - day
        '              2 - minute    5 - month
        '              3 - hour      6 - year
        '     TS1    - time step, in TC1 units
        '     TC2    - time units code, see TC1
        '     TS2    - time step, in TC2 units
        '     TSTEPF - time step compatability flag
        '              0 - compatible time series
        '              1 - incompatible time steps
        '     TCDCMP - flag indicating order of time steps
        '               0 - time steps are the same
        '               1 - first time step is smaller
        '               2 - second time step is smaller
        '              -1 - time units span day-month boundry

        Dim convdn() As Integer = {-1, 0, 60, 60, 24, 0, 12, 100}

        If ((tc1 <= 4 And tc2 > 4) Or (tc1 > 4 And tc2 <= 4)) Then
            'time units span day-month boundry
            tstepf = 1
            tcdcmp = -1
        Else
            'acceptable time units
            If (tc1 <> tc2) Then
                'time units not same, adjust larger to agree with smaller
                If (tc1 < tc2) Then
                    'adjust second time units to agree with first
                    Do While tc1 < tc2
                        ts2 *= convdn(tc2)
                        tc2 -= 1
                    Loop
                Else
                    'adjust first time units to agree with second
                    Do While tc2 < tc1
                        ts1 *= convdn(tc1)
                        tc1 -= 1
                    Loop
                End If
            End If

            'Time units converted, check time step
            tstepf = 0
            If (ts1 = ts2) Then
                'Same time step
                tcdcmp = 0
            ElseIf (ts1 < ts2) Then
                'First time step smaller
                tcdcmp = 1
                If (ts2 Mod ts1) <> 0 Then tstepf = 1
            Else
                'Second time step smaller
                tcdcmp = 2
                If (ts1 Mod ts2) <> 0 Then tstepf = 1
            End If
        End If
    End Sub

    Public Sub timcnv(ByRef d() As Integer)
        '     Convert a date that uses the midnight convention of 00:00
        '     to the convention 24:00.  For example, 1982/10/01 00:00:00
        '     would be converted to the date 1982/09/30 24:00:00.

        If (d(3) = 0) Then
            If (d(4) = 0 And d(5) = 0) Then
                '     date using new day boundry convention, convert to old
                d(3) = 24
                d(2) = d(2) - 1
                If (d(2) = 0) Then
                    d(1) = d(1) - 1
                    If (d(1) = 0) Then
                        d(0) = d(0) - 1
                        d(1) = 12
                    End If
                    d(2) = daymon(d(0), d(1))
                End If
            End If
        End If
    End Sub

    'Public Function RoundToSecond(ByVal aDate As Double) As Double
    '    Dim lNumDays As Double = Math.Truncate(aDate)
    '    Dim lNumSeconds As Double = Math.Round((aDate - lNumDays) * SecondsPerDay)
    '    Return lNumDays + lNumSeconds / SecondsPerDay
    'End Function

    Public Function TimAddJ(ByVal jStartDate As Double, _
                            ByVal TCODE As Integer, _
                            ByVal TSTEP As Integer, _
                            ByVal NVALS As Integer) As Double
        Dim DATE1(6) As Integer
        Dim DATE2(6) As Integer
        Dim lDate As DateTime, lDateNew As DateTime

        If NVALS >= 0 Then 'add
            Select Case TCODE
                Case 1 : TimAddJ = jStartDate + TSTEP * NVALS * JulianSecond
                Case 2 : TimAddJ = jStartDate + TSTEP * NVALS * JulianMinute
                Case 3 : TimAddJ = jStartDate + TSTEP * NVALS * JulianHour
                Case 4 : TimAddJ = jStartDate + TSTEP * NVALS ' JulianDay = 1
                Case 5, 6, 7 'month, year, century
                    J2Date(jStartDate, DATE1)
                    TIMADD(DATE1, TCODE, TSTEP, NVALS, DATE2)
                    TimAddJ = Date2J(DATE2)
            End Select
        Else 'subtract
            lDate = FromOADate(jStartDate)
            Select Case TCODE
                Case 1 : lDateNew = lDate.AddSeconds(TSTEP * NVALS)
                Case 2 : lDateNew = lDate.AddMinutes(TSTEP * NVALS)
                Case 3 : lDateNew = lDate.AddHours(TSTEP * NVALS)
                Case 4 : lDateNew = lDate.AddDays(TSTEP * NVALS)
                Case 5 : lDateNew = lDate.AddMonths(TSTEP * NVALS)
                Case 6 : lDateNew = lDate.AddYears(TSTEP * NVALS)
                Case 7 : lDateNew = lDate.AddYears(TSTEP * NVALS * 100)
            End Select
            TimAddJ = lDateNew.ToOADate
        End If
    End Function

    Public Sub TIMADD(ByVal DATE1() As Integer, _
                      ByVal TCODE As Integer, ByVal TSTEP As Integer, _
                      ByVal NVALS As Integer, ByRef DATE2() As Integer)

        ' Add NVALS time steps to first date to compute second date.
        ' The first date is assumed to be valid.
        '     DATE1  - starting date
        '     TCODE  - time units
        '              1 - second          5 - month
        '              2 - minute          6 - year
        '              3 - hour            7 - century
        '              4 - Day
        '     TSTEP  - time step in TCODE units
        '     NVALS  - number of time steps to be added
        '     DATE2  - new date
        Dim STPOS, TMN, TDY, TYR, CARRY, DPM, TMO, THR, TSC, DONFG As Integer

        TYR = DATE1(0)
        TMO = DATE1(1)
        TDY = DATE1(2)
        THR = DATE1(3)
        TMN = DATE1(4)
        TSC = DATE1(5)

        'figure out how much time to add and where to start
        CARRY = NVALS * TSTEP
        STPOS = TCODE
        If (STPOS = 7) Then 'time units are centuries, convert to years
            STPOS = 6
            CARRY = CARRY * 100
        End If

        'add the time, not changing insig. parts
        If (STPOS = 1) Then 'seconds
            TSC = TSC + CARRY
            CARRY = Fix(TSC / 60)
            TSC = TSC - (CARRY * 60)
        End If
        If (STPOS <= 2 And CARRY > 0) Then ' minutes
            TMN = TMN + CARRY
            CARRY = Fix(TMN / 60)
            TMN = TMN - (CARRY * 60)
        End If
        If (STPOS <= 3 And CARRY > 0) Then ' hours
            THR = THR + CARRY
            CARRY = Fix(THR / 24)
            THR = THR - (CARRY * 24)
            If (THR = 0 And TMN = 0 And TSC = 0) Then 'this is the day boundry problem
                THR = 24
                CARRY = CARRY - 1
            End If
        End If
        If (STPOS <= 4 And CARRY > 0) Then ' days
            TDY = TDY + CARRY
            If (TDY > 28) Then 'may need month/year adjustment
                DONFG = 0
                While DONFG = 0
                    DPM = daymon(TYR, TMO)
                    If (TDY > DPM) Then 'add another month
                        TDY = TDY - DPM
                        TMO = TMO + 1
                        If (TMO > 12) Then 'add year
                            TMO = 1
                            TYR = TYR + 1
                        End If
                    ElseIf (TDY <= 0) Then  'subtract another month
                        TMO = TMO - 1
                        If (TMO = 0) Then
                            TYR = TYR - 1
                            TMO = 12
                        End If
                        TDY = TDY - daymon(TYR, TMO)
                    Else
                        DONFG = 1
                    End If
                End While
            End If
            'month and year updated here all done
        End If

        If (STPOS >= 5) Then
            If (STPOS = 5) Then 'months
                TMO = TMO + CARRY
                CARRY = Fix((TMO - 1) / 12)
                TMO = TMO - (CARRY * 12)
            End If
            If (STPOS <= 6 And CARRY > 0) Then ' years
                TYR = TYR + CARRY
            End If

            'check days/month
            DPM = daymon(TYR, TMO)
            If (DPM < TDY) Then
                TDY = DPM
            End If
            If (daymon(DATE1(0), DATE1(1)) = DATE1(2)) Then
                TDY = DPM
            End If
        End If

        DATE2(0) = TYR
        DATE2(1) = TMO
        DATE2(2) = TDY
        DATE2(3) = THR
        DATE2(4) = TMN
        DATE2(5) = TSC
    End Sub

    Public Sub TimDif(ByVal DATE1() As Integer, ByVal DATE2() As Integer, _
                      ByVal TCODE As Integer, ByVal TSTEP As Integer, _
                      ByRef NVALS As Integer)

        '     Calculate the number of time steps between two dates.  Part
        '     intervals at a time step less than TCODE and TSSTEP are not
        '     included.  If the second date is before the first date, or the
        '     second date is the same as the first date, the number of time
        '     steps will be returned as 0.  Dates are assumed to be valid.
        '       DATE1  - first (starting) date
        '       DATE2  - second (ending) date
        '       TCODE  - time units code
        '                1 - seconds     5 - months
        '                2 - minutes     6 - years
        '                3 - hours       7 - centuries
        '                4 - days
        '       TSTEP  - time step in TCODE units
        ' Output:
        '       NVALS  - number of time steps between DATE1 and DATE2

        NVALS = pTimDif(Date2J(DATE1), Date2J(DATE2), DATE1, DATE2, TCODE, TSTEP)
    End Sub

    Public Function timdifJ(ByVal StartJDate As Double, ByVal EndJDate As Double, _
                            ByVal TCODE As Integer, ByVal TSTEP As Integer) As Integer
        '     Calculate the number of time steps between two dates.  Part
        '     intervals at a time step less than TCODE and TSSTEP are not
        '     included.  If the second date is before the first date, or the
        '     second date is the same as the first date, the number of time
        '     steps will be returned as 0.  Dates are assumed to be valid.
        '       StartJDate - first (starting) Julian date
        '       EndJDate   - second (ending) Julian date
        '       TCODE      - time units code
        '                1 - seconds     5 - months
        '                2 - minutes     6 - years
        '                3 - hours       7 - centuries
        '                4 - days
        '       TSTEP  - time step in TCODE units
        'Returns NVALS - number of time steps between DATE1 and DATE2

        Dim tmpstr(6) As Integer
        Dim tmpend(6) As Integer
        If (EndJDate <= StartJDate) Then ' end date is the same as or before start date
            timdifJ = 0
        Else 'end date follows start date, make temp copies of dates
            J2Date(StartJDate, tmpstr)
            J2Date(EndJDate, tmpend)
            timdifJ = pTimDif(StartJDate, EndJDate, tmpstr, tmpend, TCODE, TSTEP)
        End If
    End Function

    Private Function pTimDif(ByVal StartJDate As Double, ByVal EndJDate As Double, _
                             ByVal DATE1() As Integer, ByVal DATE2() As Integer, _
                             ByVal TCODE As Integer, ByVal TSTEP As Integer) As Integer
        Dim NVALS As Integer

        If TCODE = 0 Or TSTEP = 0 Then
            NVALS = 0
        Else
            'convert dates to old format
            Call timcnv(DATE1)
            Call timcnv(DATE2)

            Select Case TCODE
                Case 1 'seconds
                    NVALS = ((EndJDate - StartJDate) * 86400.0#) / TSTEP
                Case 2 'minutes
                    NVALS = ((EndJDate - StartJDate) * 1440.0#) / TSTEP
                Case 3 'hours
                    NVALS = ((EndJDate - StartJDate) * 24) / TSTEP
                Case 4 'days
                    NVALS = (EndJDate - StartJDate) / TSTEP
                Case 5 'months
                    NVALS = ((DATE2(0) - DATE1(0)) * 12 + DATE2(1) - DATE1(1)) / TSTEP
                Case 6 'years
                    NVALS = (DATE2(0) - DATE1(0)) / TSTEP
                Case 7 'centuries
                    NVALS = (DATE2(0) - DATE1(0)) / (TSTEP * 100)
                Case Else
                    NVALS = 0
            End Select

            Dim tmpDATE(5) As Integer
            tmpDATE(0) = DATE2(0)
            tmpDATE(1) = DATE2(1)
            tmpDATE(2) = DATE2(2)
            tmpDATE(3) = DATE2(3)
            tmpDATE(4) = DATE2(4)
            tmpDATE(5) = DATE2(5)
            Do
                Call TIMADD(DATE1, TCODE, TSTEP, NVALS, tmpDATE)
                If EndJDate < Date2J(tmpDATE) Then 'estimate too high
                    NVALS = NVALS - 1
                Else 'estimate ok
                    Exit Do
                End If
            Loop
        End If
        Return NVALS
    End Function

    Public Sub CalcTimeUnitStep(ByVal aSJDate As Double, ByVal aEJDate As Double, ByRef aTu As Integer, ByRef aTs As Integer)
        aTu = 6
        aTs = 0
        While aTs < 1
            aTs = timdifJ(aSJDate, aEJDate, aTu, 1)
            If aTs = 0 Then aTu = aTu - 1
        End While
    End Sub

    Public Function StringToJdate(ByVal aText As String, ByVal aIntervalStart As Boolean) As Double
        Try
            Dim lDateArray(6) As Integer
            If aText.Length > 3 Then
                lDateArray(0) = StrFirstInt(aText)
                If aIntervalStart Then
                    lDateArray(1) = 1
                    lDateArray(2) = 1
                    lDateArray(3) = 0
                Else
                    lDateArray(1) = 12
                    lDateArray(2) = 31
                    lDateArray(3) = 24
                End If
                lDateArray(4) = 0
                lDateArray(5) = 0

                If aText.Length > 1 Then
                    aText = aText.Substring(1)
                    lDateArray(1) = StrFirstInt(aText)
                End If

                If aText.Length > 1 Then
                    aText = aText.Substring(1)
                    lDateArray(2) = StrFirstInt(aText)
                End If

                Return Date2J(lDateArray)
            Else
                Return 0
            End If
        Catch e As Exception
            Return 0
        End Try
    End Function
End Module
