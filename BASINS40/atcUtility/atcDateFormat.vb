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

    Public Property DateOrder() As DateOrderEnum
        Get
            Return pDateOrder
        End Get
        Set(ByVal newValue As DateOrderEnum)
            pDateOrder = newValue
        End Set
    End Property

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

    ''' <summary>
    ''' Midnight will be formatted as 24:00 on the previous day if true, will be formatted as 00:00 on the next day if false
    ''' </summary>
    ''' <value>True to format midnight as 24:00, false to format midnight as 00:00</value>
    ''' <returns>current setting</returns>
    ''' <remarks>default = True</remarks>
    Public Property Midnight24() As Boolean
        Get
            Return pMidnight24
        End Get
        Set(ByVal newValue As Boolean)
            pMidnight24 = newValue
        End Set
    End Property

    Public Function JDateToString(ByVal aJulianDate As Double) As String
        Dim lRetval As String = ""
        Dim lCurDate(5) As Integer

        If Double.IsNaN(aJulianDate) Then
            lRetval = "?"
        Else
            J2Date(aJulianDate, lCurDate)
            If lCurDate(3) = 24 Then
                J2Date(aJulianDate + JulianHour, lCurDate)
                lCurDate(3) = 0
            End If

            If pMidnight24 Then 'convert to 24th hour previous day
                timcnv(lCurDate)
            End If

            Select Case pDateOrder
                Case DateOrderEnum.YearMonthDay
                    If pIncludeYears Then
                        lRetval = YearString(lCurDate)
                    End If
                    If pIncludeMonths Then
                        If pIncludeYears Then
                            lRetval &= pDateSeparator
                        End If
                        lRetval &= MonthString(lCurDate)
                    End If
                    If pIncludeDays Then
                        If pIncludeYears OrElse pIncludeMonths Then
                            lRetval &= pDateSeparator
                        End If
                        lRetval &= Format(lCurDate(2), "00")
                    End If
                Case DateOrderEnum.MonthDayYear
                    If pIncludeMonths Then
                        lRetval &= MonthString(lCurDate)
                    End If
                    If pIncludeDays Then
                        If pIncludeMonths Then
                            lRetval &= pDateSeparator
                        End If
                        lRetval &= Format(lCurDate(2), "00")
                    End If
                    If pIncludeYears Then
                        If pIncludeDays OrElse pIncludeMonths Then
                            lRetval &= pDateSeparator
                        End If
                        lRetval &= YearString(lCurDate)
                    End If
                Case DateOrderEnum.DayMonthYear
                    If pIncludeDays Then
                        lRetval &= Format(lCurDate(2), "00")
                    End If
                    If pIncludeMonths Then
                        If pIncludeDays Then
                            lRetval &= pDateSeparator
                        End If
                        lRetval &= MonthString(lCurDate)
                    End If
                    If pIncludeYears Then
                        If pIncludeDays OrElse pIncludeMonths Then
                            lRetval &= pDateSeparator
                        End If
                        lRetval &= YearString(lCurDate)
                    End If
                Case DateOrderEnum.JulianDate
                    lRetval = StrPad(DoubleToString(aJulianDate, 9, "00,000.000", , , 8), 10)
            End Select
            If pIncludeHours OrElse pIncludeMinutes OrElse pIncludeSeconds Then
                lRetval &= " "
            End If
            If pIncludeHours Then
                lRetval &= Format(lCurDate(3), "00")
            End If
            If pIncludeMinutes Then
                lRetval &= pTimeSeparator & Format(lCurDate(4), "00")
            End If
            If pIncludeSeconds Then
                lRetval &= pTimeSeparator & Format(lCurDate(5), "00")
            End If
        End If

        Return lRetval
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
