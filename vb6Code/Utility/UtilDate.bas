Attribute VB_Name = "UtilDate"
Option Explicit
'##MODULE_REMARKS Copyright 2001-3 AQUA TERRA Consultants - Royalty-free use permitted under open source license

'##MODULE_DESCRIPTION General date utility subroutines and functions shared by many projects (don't _
 change)

'##GLOBAL JulianHour - one hour as fraction of a day
Public Const JulianHour As Double = 1 / 24
'##GLOBAL JulianMinute - one minute as fraction of a day
Public Const JulianMinute As Double = 1 / 1440
'##GLOBAL JulianSecond - one second as fraction of a day
Public Const JulianSecond As Double = 1 / 86400
'When doing math on months and years, it is more accurate to use timdif and timadd in UtilDateExt
'##GLOBAL JulianMonth - estimate of month as number of days
Public Const JulianMonth As Double = 30.44
'##GLOBAL JulianYear - estimate of year as number of days
Public Const JulianYear As Double = 365.25

Public Function VBdate2MJD(d As Date) As Double
'##SUMMARY VBdate2MJD - convert a VB date to a modfied Julian date(MJD), _
 VB date 0 is 30Dec1899, MJD date 0 is 17Nov1858
'##PARAM d - VBdate to convert
  VBdate2MJD = d + 15018
End Function

Public Function MJD2VBdate(j As Double) As Date
'##SUMMARY MJD2VBdate - convert a modified Julian date(MJD) to a VB date
'##PARM j - MJD to convert
  MJD2VBdate = j - 15018
End Function

'Decimal-aligns numbers by padding before and/or after number with spaces
'Corrects VB's Format bug by always padding return value to length of formatString
'If val is numeric and formatString looks like a date format,
' val is assumed to be a modified julian date and is converted to a VB date then formatted
'If the first character of formatString is L, the result is left aligned unless there is a decimal to align
'Such an initial L is not counted toward the length of the format string
Public Function ATCformat(ByVal val As String, ByVal formatString As String) As String
  Dim retval As String
  Dim LeftAlign As Boolean
  Dim LenFormat As Long
  Dim NumericVal As Boolean
  Dim DecimalFormatPos As Long
  Dim DecimalRetvalPos As Long
  
  If UCase(Left(formatString, 1)) = "L" Then
    LeftAlign = True
    formatString = Mid(formatString, 2)
  End If
  LenFormat = Len(formatString)
  NumericVal = IsNumeric(val)
  If NumericVal Then
    retval = LCase(Trim(formatString))
    If Len(retval) > 0 And FirstCharPos(1, formatString, "ymdhs") <= LenFormat Then
      val = MJD2VBdate(CDbl(val))
    End If
  End If
  If Len(Trim(formatString)) = 0 Then
    retval = val
  Else
    retval = Format(val, Trim(formatString))
  End If
  If Len(retval) < LenFormat Then
    If NumericVal Then
      DecimalFormatPos = InStr(formatString, ".")
    End If
    If DecimalFormatPos > 0 Then
      DecimalRetvalPos = InStr(retval, ".")
      If DecimalRetvalPos > 0 And DecimalRetvalPos < DecimalFormatPos Then
        retval = Space(DecimalFormatPos - DecimalRetvalPos) & retval
      End If
      If LenFormat > Len(retval) Then
        retval = retval & Space(LenFormat - Len(retval))
      End If
    Else
      If LenFormat > Len(retval) Then
        If LeftAlign Then
          retval = retval & Space(LenFormat - Len(retval))
        Else
          retval = Space(LenFormat - Len(retval)) & retval
        End If
      End If
    End If
  End If
  ATCformat = retval
End Function

Function MonthName3(MonthNum As Long)
'##SUMMARY MonthName3 - convert a month number to a 3 character string
'##PARM MonthNum - month number to convert
  Select Case MonthNum
    Case 1: MonthName3 = "Jan"
    Case 2: MonthName3 = "Feb"
    Case 3: MonthName3 = "Mar"
    Case 4: MonthName3 = "Apr"
    Case 5: MonthName3 = "May"
    Case 6: MonthName3 = "Jun"
    Case 7: MonthName3 = "Jul"
    Case 8: MonthName3 = "Aug"
    Case 9: MonthName3 = "Sep"
    Case 10: MonthName3 = "Oct"
    Case 11: MonthName3 = "Nov"
    Case 12: MonthName3 = "Dec"
    Case Else: MonthName3 = "<?>"
  End Select
End Function

Function MonthName(MonthNum As Long)
'##SUMMARY MonthName - convert a month number to a character string
'##PARM MonthNum - month number to convert
  Select Case MonthNum
    Case 1: MonthName = "January"
    Case 2: MonthName = "February"
    Case 3: MonthName = "March"
    Case 4: MonthName = "April"
    Case 5: MonthName = "May"
    Case 6: MonthName = "June"
    Case 7: MonthName = "July"
    Case 8: MonthName = "August"
    Case 9: MonthName = "September"
    Case 10: MonthName = "October"
    Case 11: MonthName = "November"
    Case 12: MonthName = "December"
    Case Else: MonthName = "<unknown>"
  End Select
End Function

Function Date2J(d() As Long) As Double
'##SUMMARY Date2J - convert a date arry to a modfied Julian date (MJD)
'##PARM d - date array to convert
  Dim jd As Long, jhms As Double
  '##LOCAL jd - date (year, month, day) portion of MJD
  '##LOCAL hhms - time (hour, minute, second) portion of MJD

  jd = MJD(d(0), d(1), d(2))
  jhms = HMS2J(d(3), d(4), d(5))
  Date2J = jd + jhms
End Function

Function HMS2J(h As Long, m As Long, s As Long) As Double
'##SUMMARY HMS2J - convert an hour, minute, and second to a modifed Julian date (MJD)
'##PARM h - hour to convert
'##PARM m - minute to convert
'##PARM s - second to convert
  HMS2J = CDbl(h / 24) + CDbl(m / 1440) + CDbl(s / 86400)
End Function

Sub J2Date(j As Double, d() As Long)
'##SUMMARY J2Date - convert a modified Julian date (MJD) to a long array
'##PARM j - modfied Julian date to convert
'##PARM d - array containing output year , month, day, hour, minute, second
  Dim jd As Long, jhms As Double, f As Double
  '##LOCAL jd - date portion of date to convert
  '##LOCAL jhms - time portion of date to convert
  '##LOCAL f - fraction of a second resulting from conversion
    jd = Fix(j)
    Call INVMJD(jd, d(0), d(1), d(2))
    jhms = j - jd
    Call J2HMS(jhms, d(3), d(4), d(5), f)
End Sub

Sub J2HMS(j As Double, h As Long, m As Long, s As Long, f As Double)
'##SUMMARY J2HMS - convert a time portion of a modfied Julian date to its component parts
'##Parm j - MJD to convert
'##PARM h - hour portion of MJD
'##PARM m - minute portion of MJD
'##PARM s - second portion of MJD
  Dim t As Double
  '##LOCAL t - intermediate result, units change from hours to seconds
   t = 0.0000004 + (j * 24)
   h = Fix(t)
   t = (t - h) * 60
   m = Fix(t)
   t = (t - m) * 60
   s = Fix(t)
   f = t - s
End Sub

Sub INVMJD(MJD As Long, yr As Long, mn As Long, dy As Long)
'##SUMMARY INVMJD - invert modified Julian date as computed by function MJD (from DelbertDFranz) _
  Developed from information given in: "Astronomical Formulae _
  for Calculators', Jean Meeus, published by Willmann-Bell.
'##PARM MJD - value of modified julian date date number to invert
'##PARM yr - calendar year
'##PARM mn - number of month(1-12)
'##PARM dy - day in the month
  Dim a As Long, ALPHA As Long, b As Long, c As Long, d As Long, e As Long, Z As Long
  '##LOCAL a - intermediate result
  '##LOCAL ALPHA - intermediate result
  '##LOCAL b - intermediate result
  '##LOCAL c - intermediate result
  '##LOCAL d - intermediate result
  '##LOCAL e - intermediate result
  '##LOCAL Z - intermediate result
 
  'convert to Julian time plus the .5 day correction. yields an integer
   Z = MJD + 679006 + 1720994 + 1
 
   If (Z < 2299161) Then
     a = Z
   Else
     ALPHA = CLng(Fix((CDbl(Z) - CDbl(1867216.24)) / CDbl(36524.25)))
     a = Z + 1 + ALPHA - Int(ALPHA / 4)
   End If
 
   b = a + 1524
   c = CLng(Fix((CDbl(b) - 122.1) / 365.25))
   d = CLng(Fix(CDbl(365.25) * CDbl(c)))
   e = CLng(Fix(CDbl(b - d) / 30.6001))
 
   dy = b - d - Fix(30.6001 * CDbl(e))
   If (e <= 13) Then
     mn = e - 1
   Else
     mn = e - 13
   End If
   If (mn >= 3) Then
     yr = c - 4716
   Else
     yr = c - 4715
   End If
End Sub

Function MJD(yr As Long, mn As Long, dy As Long) As Long
'##SUMMARY MJD - 'Compute modified julian date for any date _
 with a year greater than 1582 (from DelbertDFranz) _
 We take the resulting date to represent the elapsed time from _
 some point in the past to the first instant of the given day. _
 The date must be later than Nov. 17, 1858 for MJD to be _
 a positive number.  Thus for use in FEQ the year must be 1859 _
 or greater. _
 This routine and INVMJD have been checked by DDF for every day _
 from 1860 through the year 25000. _
 Developed from information given in: "Astronomical Formulae _
 for Calculators', Jean Meeus, published by Willmann-Bell.
'##PARM yr - calendar year
'##PARM mn - number of month(1-12)
'##PARM dy - day in the month
  Dim a As Long, b As Long, m As Long, y As Long
  '##LOCAL a - intermediate result
  '##LOCAL b - intermediate result
  '##LOCAL m - intermediate result
  '##LOCAL y - intermediate result
 
   If (mn > 2) Then
     y = yr
     m = mn
   Else
     y = yr - 1
     m = mn + 12
   End If
 
   a = Int(y / 100)
   b = 2 - a + Int(a / 4)
 
   MJD = Int((36525 * y) / 100) + Int(30.6001 * (m + 1)) + dy + b - 679006
End Function

Function Jday(yr As Long, mo As Long, dy As Long, hr As Long, mn As Long, sc As Long) As Double
'SUMMARY jday - convert portions of date to a modfied Julian date (MJD)
'##PARM yr - calendar year
'##PARM mn - number of month(1-12)
'##PARM dy - day in the month
'##PARM hr - hour of day
'##PARM mn - minute of day
'##PARM sc - second of day
  Dim d(5) As Long
  'LOCAL d - date array
  d(0) = yr
  d(1) = mo
  d(2) = dy
  d(3) = hr
  d(4) = mn
  d(5) = sc
  Jday = Date2J(d)
End Function

Function JDateIntrvl(j As Double) As Long
'##SUMMARY JDateIntrvl - determines the date interval (6-second thru 1-year) of _
 a modfied Julian date
'##PARM j - MJD to determine interval of
  Dim d(5) As Long
  'LOCAL d - date array
  Call J2Date(j, d)
  JDateIntrvl = DateIntrvl(d)
End Function

Function DateIntrvl(d() As Long) As Long
'##SUMMARY DateIntrvl - determines the date interval (6-second thru 1-year) of a date array
'##PARM d - date array to determine interval of
  Dim i As Long
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
        ElseIf d(2) = 0 Then 'more: x x 0 0 0 0
          i = 2 'at least a month boundary
          If d(1) = 0 Then 'more: x 0 0 0 0 0
            i = 1 'at least a year boundary
          End If
        End If
      ElseIf d(3) = 24 Then 'more: x x x 24 0 0
        i = 3
        If d(2) = daymon(d(0), d(1)) Then 'more: x x 31,30,29,28 24 0 0
          i = 2 'month boundary
          If d(1) = 12 Then 'more: x 12 31 24 0 0
            i = 1 'year boundary
          End If
        End If
      End If
    ElseIf d(4) = 60 Then 'more: x x x x 60 0
      i = 4 'hour boundary
    End If
  ElseIf d(5) = 60 Then 'more: x x x x x 60
    i = 5  'minute boundary
  End If
  
  DateIntrvl = i
End Function

Function daymon(yr As Long, mo As Long) As Long
'##SUMMARY daymon - return the number of days in the given month for the given _
 year, with leap year taken into account.  For an invalid month, -1 is returned. _
 For an invalid year and a valid month, the correct number of days is returned, _
 with February = 28.
'##PARM yr - year, valid range is 1 - 2080
'##PARM mo - month, valid range is 1 - 12
  Static ndaymon As Variant
  'LOCAL ndaymon - number of days in each month
  If Not (IsArray(ndaymon)) Then 'initialize the array
    ndaymon = Array(0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31)
  End If
  
  If mo = 2 Then 'check for leap year
    If yr <= 0 Or yr > 2080 Then 'invalid year
      daymon = 28
    ElseIf yr Mod 100 = 0 Then
      'check whether this is a leap year on a century boundary
      If yr Mod 400 = 0 Then 'on a 400 year boundary
        daymon = 29
      Else 'century boundary case
        daymon = 28
      End If
    ElseIf yr Mod 4 = 0 Then 'leap year
      daymon = 29
    Else 'non leap year
      daymon = 28
    End If
  ElseIf mo >= 1 And mo <= 12 Then 'valid month, not february
    daymon = ndaymon(mo)
  Else 'invalid month
    daymon = -1
  End If
End Function

Public Function addUniqueDate(j As Double, ja() As Double, ji() As Long) As Boolean
'##SUMMARY addUniqueDate - adds a unique MJD date to an array of dates if it is not already there _
 returns true if date added, false if date was already in array
'##PARM j - MJD date to try to add
'##PARM ja - array of current dates
'##PARM ji - array of date intervals corresponding to dates in ja
  Dim i As Long, fnd As Boolean
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

Public Function DumpDate(j As Double, Optional s = "JDate") As String
'##SUMMARY DumpDate - convert a modfied Julian date to a string
'##PARM j - date to convert
'##PARM s - optional prefix for output string
  Dim d(5) As Long
  'LOCAL d - date array set to values from j
  J2Date j, d
  DumpDate = s & ":" & j & ":" & d(0) & "/" & d(1) & "/" & d(2) & " " & d(3) & ":" & d(4) & ":" & d(5)
End Function
