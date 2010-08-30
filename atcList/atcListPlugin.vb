Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class atcListPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::List"
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
        Dim lForm As New atcListForm
        lForm.Initialize(aTimeseriesGroup)
        Return lForm
    End Function

    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOptions() As String)

        If Not aTimeseriesGroup Is Nothing AndAlso aTimeseriesGroup.Count > 0 Then
            Dim lForm As New atcListForm
            Dim lFilterNoData As Boolean = False
            Dim lViewValues As Boolean = True

            Dim lSpecifiedFormat As Boolean = False

            Dim lMaxWidth As Integer = 10
            Dim lFormat As String = "#,##0.########"
            Dim lExpFormat As String = "#.#e#"
            Dim lCantFit As String = "#"
            Dim lSignificantDigits As Integer = 5

            Dim lDisplayAttributes As ArrayList = Nothing

            For Each lOption As String In aOptions
                Select Case lOption.ToLower
                    Case "filternodata" : lFilterNoData = True
                    Case "viewnovalues" : lViewValues = False
                    Case "attributerows" : lForm.mnuAttributeRows.Checked = True
                    Case "attributecolumns" : lForm.mnuAttributeColumns.Checked = True
                    Case Else
                        If lOption.ToLower.StartsWith("dateformat") Then
                            With lForm.DateFormat
                                Dim lDateValue As String = lOption.Substring(10)
                                Dim lDateBoolean As Boolean
                                If Not Boolean.TryParse(lDateValue, lDateBoolean) Then
                                    lDateBoolean = True
                                End If
                                Dim lDateSection As String = StrSplit(lDateValue, "=", """")
                                Select Case lDateSection.ToLower
                                    Case "orderdmy" : .DateOrder = atcDateFormat.DateOrderEnum.DayMonthYear
                                    Case "orderjulian" : .DateOrder = atcDateFormat.DateOrderEnum.JulianDate
                                    Case "ordermdy" : .DateOrder = atcDateFormat.DateOrderEnum.MonthDayYear
                                    Case "orderymd" : .DateOrder = atcDateFormat.DateOrderEnum.YearMonthDay

                                    Case "includeyears" : .IncludeYears = lDateBoolean
                                        .IncludeMonths = False : .IncludeDays = False : .IncludeHours = False : .IncludeMinutes = False : .IncludeSeconds = False
                                    Case "includemonths" : .IncludeMonths = lDateBoolean
                                        .IncludeDays = False : .IncludeHours = False : .IncludeMinutes = False : .IncludeSeconds = False
                                    Case "includedays" : .IncludeDays = lDateBoolean
                                        .IncludeHours = False : .IncludeMinutes = False : .IncludeSeconds = False
                                    Case "includehours" : .IncludeHours = lDateBoolean
                                        .IncludeMinutes = False : .IncludeSeconds = False
                                    Case "includeminutes" : .IncludeMinutes = lDateBoolean
                                        .IncludeSeconds = False
                                    Case "includeseconds" : .IncludeSeconds = lDateBoolean

                                    Case "twodigityears" : .TwoDigitYears = lDateBoolean
                                    Case "midnight24" : .Midnight24 = lDateBoolean
                                    Case "monthnames" : .MonthNames = lDateBoolean

                                    Case "dateseparator" : .DateSeparator = lDateValue
                                    Case "timeseparator" : .TimeSeparator = lDateValue
                                End Select
                            End With
                            If lOption.StartsWith("valueformat") Then
                                lSpecifiedFormat = True
                                Dim lValue As String = lOption.Substring(11)
                                Dim lSection As String = StrSplit(lValue, "=", """")
                                Select Case lSection.ToLower
                                    Case "maxwidth" : Integer.TryParse(lValue, lMaxWidth)
                                    Case "format" : lFormat = lValue
                                    Case "expformat" : lExpFormat = lValue
                                    Case "cantfit" : lCantFit = lValue
                                    Case "significantdigits" : Integer.TryParse(lValue, lSignificantDigits)
                                End Select
                            End If
                        ElseIf lOption.ToLower.StartsWith("displayattributes:") Then
                            lDisplayAttributes = New ArrayList
                            lDisplayAttributes.AddRange(lOption.Substring(18).Split(","))
                        End If
                        Logger.Dbg("UnknownParameter:" & lOption)
                End Select
            Next

            If lSpecifiedFormat Then lForm.ValueFormat(lMaxWidth, lFormat, lExpFormat, lCantFit, lSignificantDigits)

            lForm.Initialize(aTimeseriesGroup, lDisplayAttributes, lViewValues, lFilterNoData, False)
            atcUtility.SaveFileString(aFileName, lForm.ToString)
            lForm.Dispose()
        End If
    End Sub

End Class
