Public Class atcDateFormat

    Public Enum DateOrderEnum
        YearMonthDay
        MonthDayYear
        DayMonthYear
        JulianDate
    End Enum

    Public DateOrder As DateOrderEnum = DateOrderEnum.YearMonthDay
    Public DateSeparator As String = "/"
    Public DateTimeSeparator As String = " "
    Public TimeSeparator As String = ":"
    Public IncludeYears As Boolean = True
    Public IncludeMonths As Boolean = True
    Public IncludeDays As Boolean = True
    Public IncludeHours As Boolean = True
    Public IncludeMinutes As Boolean = True
    Public IncludeSeconds As Boolean = False
    Public TwoDigitYears As Boolean = False
    Public MonthNames As Boolean = False
    'Private pDateEndInterval As Boolean = True

    ''' <summary>
    ''' Midnight will be formatted as 24:00 on the previous day if true, will be formatted as 00:00 on the next day if false
    ''' </summary>
    ''' <remarks>default = True</remarks>
    Public Midnight24 As Boolean = True

    Public Sub New()
    End Sub

    Public Sub New(ByVal aFormat As String)
        Me.FromString(aFormat)
    End Sub

    Public Overrides Function ToString() As String
        Dim lStr As New System.Text.StringBuilder
        lStr.Append("DateOrder=" & DateOrder.ToString & vbLf)
        lStr.Append("DateSeparator=" & DateSeparator & vbLf)
        lStr.Append("DateTimeSeparator=" & DateTimeSeparator & vbLf)
        lStr.Append("TimeSeparator=" & TimeSeparator & vbLf)
        lStr.Append("IncludeYears=" & IncludeYears & vbLf)
        lStr.Append("IncludeMonths=" & IncludeMonths & vbLf)
        lStr.Append("IncludeDays=" & IncludeDays & vbLf)
        lStr.Append("IncludeHours=" & IncludeHours & vbLf)
        lStr.Append("IncludeMinutes=" & IncludeMinutes & vbLf)
        lStr.Append("IncludeSeconds=" & IncludeSeconds & vbLf)
        lStr.Append("TwoDigitYears=" & TwoDigitYears & vbLf)
        lStr.Append("MonthNames=" & MonthNames & vbLf)
        lStr.Append("Midnight24=" & Midnight24 & vbLf)
        Return lStr.ToString
    End Function

    Public Sub FromString(ByVal aDateFormat As String)
        If Not String.IsNullOrEmpty(aDateFormat) Then
            Dim lParts() As String = aDateFormat.Split(vbLf)
            For Each lPart As String In lParts
                Try
                    Dim lNameValue() As String = lPart.Split("=")
                    If lNameValue.Length = 2 Then
                        Select Case lNameValue(0)
                            Case "DateOrder" : DateOrder = [Enum].Parse(DateOrder.GetType, lNameValue(1))
                            Case "DateSeparator" : DateSeparator = lNameValue(1)
                            Case "DateTimeSeparator" : DateTimeSeparator = lNameValue(1)
                            Case "TimeSeparator" : TimeSeparator = lNameValue(1)
                            Case "IncludeYears" : Boolean.TryParse(lNameValue(1), IncludeYears)
                            Case "IncludeMonths" : Boolean.TryParse(lNameValue(1), IncludeMonths)
                            Case "IncludeDays" : Boolean.TryParse(lNameValue(1), IncludeDays)
                            Case "IncludeHours" : Boolean.TryParse(lNameValue(1), IncludeHours)
                            Case "IncludeMinutes" : Boolean.TryParse(lNameValue(1), IncludeMinutes)
                            Case "IncludeSeconds" : Boolean.TryParse(lNameValue(1), IncludeSeconds)
                            Case "TwoDigitYears" : Boolean.TryParse(lNameValue(1), TwoDigitYears)
                            Case "MonthNames" : Boolean.TryParse(lNameValue(1), MonthNames)
                            Case "Midnight24" : Boolean.TryParse(lNameValue(1), Midnight24)
                            Case Else
                                MapWinUtility.Logger.Dbg("Date format part not known '" & lPart & "'")
                        End Select
                    End If
                Catch
                    MapWinUtility.Logger.Dbg("Could not parse date format part '" & lPart & "'")
                End Try
            Next
        End If
    End Sub

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

            If Midnight24 Then 'convert to 24th hour previous day
                timcnv(lCurDate)
            End If

            Select Case DateOrder
                Case DateOrderEnum.YearMonthDay
                    If IncludeYears Then
                        lRetval = YearString(lCurDate)
                    End If
                    If IncludeMonths Then
                        If IncludeYears Then
                            lRetval &= DateSeparator
                        End If
                        lRetval &= MonthString(lCurDate)
                    End If
                    If IncludeDays Then
                        If IncludeYears OrElse IncludeMonths Then
                            lRetval &= DateSeparator
                        End If
                        lRetval &= Format(lCurDate(2), "00")
                    End If
                Case DateOrderEnum.MonthDayYear
                    If IncludeMonths Then
                        lRetval &= MonthString(lCurDate)
                    End If
                    If IncludeDays Then
                        If IncludeMonths Then
                            lRetval &= DateSeparator
                        End If
                        lRetval &= Format(lCurDate(2), "00")
                    End If
                    If IncludeYears Then
                        If IncludeDays OrElse IncludeMonths Then
                            lRetval &= DateSeparator
                        End If
                        lRetval &= YearString(lCurDate)
                    End If
                Case DateOrderEnum.DayMonthYear
                    If IncludeDays Then
                        lRetval &= Format(lCurDate(2), "00")
                    End If
                    If IncludeMonths Then
                        If IncludeDays Then
                            lRetval &= DateSeparator
                        End If
                        lRetval &= MonthString(lCurDate)
                    End If
                    If IncludeYears Then
                        If IncludeDays OrElse IncludeMonths Then
                            lRetval &= DateSeparator
                        End If
                        lRetval &= YearString(lCurDate)
                    End If
                Case DateOrderEnum.JulianDate
                    lRetval = StrPad(DoubleToString(aJulianDate, 9, "00,000.000", , , 8), 10)
            End Select
            If (IncludeHours OrElse IncludeMinutes OrElse IncludeSeconds) AndAlso lRetval.Length > 0 Then
                lRetval &= DateTimeSeparator
            End If
            If IncludeHours Then
                lRetval &= Format(lCurDate(3), "00")
            End If
            If IncludeMinutes Then
                lRetval &= TimeSeparator & Format(lCurDate(4), "00")
            End If
            If IncludeSeconds Then
                lRetval &= TimeSeparator & Format(lCurDate(5), "00")
            End If
        End If

        Return lRetval
    End Function

    Private Function YearString(ByVal aDate() As Integer) As String
        If TwoDigitYears Then
            Return Right(CStr(aDate(0)), 2)
        Else
            Return Format(aDate(0), "0000")
        End If
    End Function

    Private Function MonthString(ByVal aDate() As Integer) As String
        If MonthNames Then
            Return MonthName3(aDate(1))
        Else
            Return Format(aDate(1), "00")
        End If
    End Function

End Class
