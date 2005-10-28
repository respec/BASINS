Public Class atcDateFormat

  Public Enum DateOrderEnum
    YearMonthDay
    MonthDayYear
    DayMonthYear
    JulianDate
  End Enum

  Private pDateOrder As DateOrderEnum = DateOrderEnum.YearMonthDay
  Private pDateSeparator As String = "/"
  Private pTimeSeparator As String = ":"
  Private pIncludeYears As Boolean = True
  Private pIncludeMonths As Boolean = True
  Private pIncludeDays As Boolean = True
  Private pIncludeHours As Boolean = True
  Private pIncludeMinutes As Boolean = True
  Private pIncludeSeconds As Boolean = False
  Private pTwoDigitYears As Boolean = False
  Private pDateMonthNames As Boolean = False
  'Private pDateEndInterval As Boolean = True
  Private pMidnight24 As Boolean = True

  Public Property DateSeparator() As String
    Get
      Return pDateSeparator
    End Get
    Set(ByVal newValue As String)
      pDateSeparator = newValue
    End Set
  End Property

  Public Property TimeSeparator() As String
    Get
      Return pTimeSeparator
    End Get
    Set(ByVal newValue As String)
      pTimeSeparator = newValue
    End Set
  End Property

  Public Property IncludeYears() As Boolean
    Get
      Return pIncludeYears
    End Get
    Set(ByVal newValue As Boolean)
      pIncludeYears = newValue
    End Set
  End Property

  Public Property IncludeMonths() As Boolean
    Get
      Return pIncludeMonths
    End Get
    Set(ByVal newValue As Boolean)
      pIncludeMonths = newValue
    End Set
  End Property

  Public Property IncludeDays() As Boolean
    Get
      Return pIncludeDays
    End Get
    Set(ByVal newValue As Boolean)
      pIncludeDays = newValue
    End Set
  End Property

  Public Property IncludeHours() As Boolean
    Get
      Return pIncludeHours
    End Get
    Set(ByVal newValue As Boolean)
      pIncludeHours = newValue
    End Set
  End Property

  Public Property IncludeMinutes() As Boolean
    Get
      Return pIncludeMinutes
    End Get
    Set(ByVal newValue As Boolean)
      pIncludeMinutes = newValue
    End Set
  End Property

  Public Property IncludeSeconds() As Boolean
    Get
      Return pIncludeSeconds
    End Get
    Set(ByVal newValue As Boolean)
      pIncludeSeconds = newValue
    End Set
  End Property

  Public Property TwoDigitYears() As Boolean
    Get
      Return pTwoDigitYears
    End Get
    Set(ByVal newValue As Boolean)
      pTwoDigitYears = newValue
    End Set
  End Property

  Public Function JDateToString(ByVal aJulianDate As Double) As String
    Dim retval As String
    Dim curDate(5) As Integer

    J2Date(aJulianDate, curDate)
    If curDate(3) = 24 Then
      J2Date(aJulianDate + JulianHour, curDate)
      curDate(3) = 0
    End If

    If pMidnight24 Then timcnv(curDate) 'convert to 24th hour previous day

    Select Case pDateOrder
      Case DateOrderEnum.YearMonthDay
        If pIncludeYears Then
          retval = YearString(curDate)
        End If
        If pIncludeMonths Then
          If pIncludeYears Then retval &= pDateSeparator
          retval &= MonthString(curDate)
        End If
        If pIncludeDays Then
          If pIncludeYears OrElse pIncludeMonths Then retval &= pDateSeparator
          retval &= Format(curDate(2), "00")
        End If
      Case DateOrderEnum.MonthDayYear
        If pIncludeMonths Then
          retval &= MonthString(curDate)
        End If
        If pIncludeDays Then
          If pIncludeMonths Then retval &= pDateSeparator
          retval &= Format(curDate(2), "00")
        End If
        If pIncludeYears Then
          If pIncludeDays OrElse pIncludeMonths Then retval &= pDateSeparator
          retval &= YearString(curDate)
        End If
      Case DateOrderEnum.DayMonthYear
        If pIncludeDays Then
          retval &= Format(curDate(2), "00")
        End If
        If pIncludeMonths Then
          If pIncludeDays Then retval &= pDateSeparator
          retval &= MonthString(curDate)
        End If
        If pIncludeYears Then
          If pIncludeDays OrElse pIncludeMonths Then retval &= pDateSeparator
          retval &= YearString(curDate)
        End If
      Case DateOrderEnum.JulianDate
        retval = ATCformat(aJulianDate, "00000.000")
    End Select
    If pIncludeHours OrElse pIncludeMinutes OrElse pIncludeSeconds Then retval &= " "
    If pIncludeHours Then retval &= Format(curDate(3), "00")
    If pIncludeMinutes Then retval &= pTimeSeparator & Format(curDate(4), "00")
    If pIncludeSeconds Then retval &= pTimeSeparator & Format(curDate(5), "00")
    Return retval
  End Function

  Private Function YearString(ByVal aDate() As Integer) As String
    If pTwoDigitYears Then
      Return Right(CStr(aDate(0)), 2)
    Else
      Return Format(aDate(0), "0000")
    End If
  End Function

  Private Function MonthString(ByVal aDate() As Integer) As String
    If pDateMonthNames Then
      Return MonthName3(aDate(1))
    Else
      Return Format(aDate(1), "00")
    End If
  End Function

End Class
