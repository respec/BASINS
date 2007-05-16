Imports atcData
Imports atcUtility

Friend Class atcFrequencyGridSource
    Inherits atcControls.atcGridSource

    Private pDataGroup As atcDataGroup
    Private pNdays As SortedList
    Private pRecurrence As SortedList
    Private pHigh As Boolean

    Private pCalculatedNdays As New ArrayList
    Private pCalculatedRecurrence As New ArrayList

    Sub New(ByVal aDataGroup As atcData.atcDataGroup)
        pDataGroup = aDataGroup
        pRecurrence = New SortedList
        pNdays = New SortedList
        Dim lKey As String
        MyBase.ColorCells = True
        For Each lData As atcDataSet In pDataGroup
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing Then
                    If lAttribute.Arguments.ContainsAttribute("Nday") Then
                        Dim lNdays As String = lAttribute.Arguments.GetFormattedValue("Nday")
                        lKey = Format(lAttribute.Arguments.GetValue("Nday"), "00000.0000")
                        If Not pNdays.ContainsKey(lKey) Then
                            pNdays.Add(lKey, lNdays)
                        End If
                    End If
                    If lAttribute.Arguments.ContainsAttribute("Return Period") Then
                        Dim lNyears As String = lAttribute.Arguments.GetFormattedValue("Return Period")
                        lKey = Format(lAttribute.Arguments.GetValue("Return Period"), "00000.0000")
                        If Not pRecurrence.ContainsKey(lKey) Then
                            pRecurrence.Add(lKey, lNyears)
                        End If
                    End If
                End If
            Next
        Next
    End Sub

    Public Property High() As Boolean
        Get
            Return pHigh
        End Get
        Set(ByVal newValue As Boolean)
            pHigh = newValue
        End Set
    End Property

    Overrides Property Columns() As Integer
        Get
            If pNdays Is Nothing Then
                Return 3
            Else
                Return pNdays.Count + 2
            End If
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property Rows() As Integer
        Get
            Try
                Return pDataGroup.Count * pRecurrence.Count + 1
            Catch
                Return 1
            End Try
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Public Function DataSetAt(ByVal aRow As Integer) As atcDataSet
        Return pDataGroup((aRow - 1) \ pRecurrence.Count)
    End Function

    Public Function NdaysAt(ByVal aColumn As Integer) As String
        Return pNdays.GetByIndex(aColumn - 2)
    End Function

    Public Function RecurrenceAt(ByVal aRow As Integer) As String
        'remove any thousands commas in return period
        Return ReplaceString(pRecurrence.GetByIndex((aRow - 1) Mod pRecurrence.Count), ",", "")
    End Function

    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
        Get
            If aRow = 0 Then
                Select Case aColumn
                    Case 0 : Return "Data Set"
                    Case 1 : Return "Return Period"
                    Case Else : Return NdaysAt(aColumn)
                End Select
            Else
                Select Case aColumn
                    Case 0 : Return DataSetAt(aRow).ToString
                    Case 1 : Return RecurrenceAt(aRow)
                    Case Else
                        Dim lDataSet As atcDataSet = DataSetAt(aRow)
                        Dim lAttrName As String = NdaysAt(aColumn)
                        If pHigh Then lAttrName &= "High" Else lAttrName &= "Low"
                        lAttrName &= RecurrenceAt(aRow)

                        If Not lDataSet.Attributes.ContainsAttribute(lAttrName) Then
                            Try
                                Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
                                Dim lArgs As New atcDataAttributes
                                Dim lOperationName As String
                                Dim lNdays(pNdays.Count) As Double
                                Dim lNextNdays As Integer = 1
                                Dim lReturns(pRecurrence.Count) As Double
                                Dim lNextReturns As Double = 1
                                Dim lValue As Double

                                If pHigh Then
                                    lOperationName = "n-day high value"
                                Else
                                    lOperationName = "n-day low value"
                                End If

                                lArgs.SetValue("Timeseries", lDataSet)

                                lNdays(0) = NdaysAt(aColumn)
                                For Each lNday As DictionaryEntry In pNdays
                                    lValue = CDbl(lNday.Value)
                                    If lValue <> lNdays(0) AndAlso Not pCalculatedNdays.Contains(lValue) Then
                                        pCalculatedNdays.Add(lValue)
                                        lNdays(lNextNdays) = lValue
                                        lNextNdays += 1
                                    End If
                                Next

                                lReturns(0) = RecurrenceAt(aColumn)
                                For Each lReturn As DictionaryEntry In pRecurrence
                                    lValue = CDbl(ReplaceString(lReturn.Value, ",", ""))
                                    If lValue <> lReturns(0) AndAlso Not pCalculatedRecurrence.Contains(lValue) Then
                                        pCalculatedRecurrence.Add(lValue)
                                        lReturns(lNextReturns) = lValue
                                        lNextReturns += 1
                                    End If
                                Next

                                ReDim Preserve lNdays(lNextNdays - 1)
                                ReDim Preserve lReturns(lNextReturns - 1)

                                lArgs.SetValue("NDay", lNdays)
                                lArgs.SetValue("Return Period", lReturns)

                                lCalculator.Open(lOperationName, lArgs)
                            Catch e As Exception
                                'LogDbg(Me.Name & " Could not calculate value at row " & aRow & ", col " & aColumn & ". " & e.ToString)
                            End Try
                        End If

                        CellValue = lDataSet.Attributes.GetFormattedValue(lAttrName)
                        If CellValue = "NaN" Then CellValue = ""

                End Select
            End If
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
        Get
            If aColumn > 0 Then
                Return atcControls.atcAlignment.HAlignDecimal
            Else
                Return atcControls.atcAlignment.HAlignLeft
            End If
        End Get
        Set(ByVal Value As atcControls.atcAlignment)
        End Set
    End Property

    Overrides Property CellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As System.Drawing.Color
        Get
            If aColumn > 1 AndAlso aRow > 0 Then
                Return System.Drawing.SystemColors.Window
            Else
                Return System.Drawing.SystemColors.Control
            End If
        End Get
        Set(ByVal Value As System.Drawing.Color)
        End Set
    End Property

    Public Function CreateReport() As String
        Dim lStartDate As Date
        Dim lStartDateAnnual As Date
        Dim lEndDate As Date
        Dim lStr As String
        Dim lIndex As Integer
        Dim lColumn As Integer
        Dim lRept As New System.Text.StringBuilder
        Dim lCurrentValue As Double
        Try
            For Each lTimeseries As atcTimeseries In pDataGroup
                Dim lAttributes As atcDataAttributes = lTimeseries.Attributes
                For Each lNdaysKey As String In pNdays.Keys
                    Dim lNdays As String = pNdays.Item(lNdaysKey)
                    Dim lAttrName As String = lNdays
                    If pHigh Then lAttrName &= "High" Else lAttrName &= "Low"
                    Dim lNdayAttribute As atcDefinedValue = lAttributes.GetDefinedValue(lAttrName & pRecurrence.GetByIndex(0))
                    Dim lNdayTs As atcTimeseries = lNdayAttribute.Arguments.GetValue("NDayTimeseries")
                    Dim lNdayTsNonLog As atcTimeseries
                    Dim lIsLog As Boolean = False
                    Dim lLogString As String = "   "

                    If lNdayTs.Attributes.ContainsAttribute("NDayTimeseries") Then
                        'Get original version of NDayTimeseries (not log version)
                        lIsLog = True
                        lLogString = "(logs)"
                        lNdayTsNonLog = lNdayTs.Attributes.GetValue("NDayTimeseries")
                    Else
                        lNdayTsNonLog = lNdayTs
                    End If
                    Dim lLocation As String = lAttributes.GetValue("STAID", "") & " " & lAttributes.GetValue("STANAM", "")
                    lStartDate = Date.FromOADate(lNdayTs.Dates.Value(0))
                    lStartDateAnnual = Date.FromOADate(lNdayTs.Dates.Value(1))
                    lEndDate = Date.FromOADate(lNdayTs.Dates.Value(lNdayTs.numValues))

                    Dim lPositiveNdayTs As atcTimeseries
                    Dim lNumZero As Integer = 0
                    Dim lNumPositive As Integer = 0
                    Dim lNumNegative As Integer = 0
                    Dim lEpsilon As Double = (lNdayTs.Attributes.GetValue("Max") - lNdayTs.Attributes.GetValue("Min")) / Math.Pow(10, 9)

                    lRept.AppendLine()
                    lRept.AppendLine()
                    If lIsLog Then
                        lRept.AppendLine("                Log-Pearson Type III Statistics")
                        lRept.AppendLine()
                        lRept.AppendLine("                  (based on USGS Program A193)")
                        lRept.AppendLine()
                        lRept.AppendLine("  Notice -- Use of Log-Pearson Type III or Pearson-Type III")
                        lRept.AppendLine("            distributions are for preliminary computations.")
                        lRept.AppendLine("            User is responsible for assessment and ")
                        lRept.AppendLine("            interpretation.")
                    Else
                        lRept.AppendLine("                  Pearson Type III Statistics")
                        lRept.AppendLine()
                        lRept.AppendLine("                  (based on USGS Program A193)")
                        lRept.AppendLine()
                        lRept.AppendLine("  Notice -- Use of Pearson Type III distribution is for")
                        lRept.AppendLine("            preliminary computations.  User is responsible")
                        lRept.AppendLine("            for assessment and interpretation.")
                    End If


                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("               " & lLocation.PadRight(64))
                    lRept.AppendLine(lStartDate.ToString("MMMM").PadLeft(24) & lStartDate.Day.ToString.PadLeft(3) & " - start of season")
                    lRept.AppendLine(lEndDate.ToString("MMMM").PadLeft(24) & lEndDate.Day.ToString.PadLeft(3) & " - end of season")
                    lRept.AppendLine("                " & lStartDateAnnual.Year & " - " & lEndDate.Year & " - time period")
                    lStr = lNdays & "-day "
                    If pHigh Then lStr &= "high" Else lStr &= "low"
                    lRept.AppendLine(lStr.PadLeft(27) & " - parameter")

                    For lIndex = 1 To lNdayTsNonLog.numValues
                        lCurrentValue = lNdayTsNonLog.Value(lIndex)
                        If Double.IsNaN(lCurrentValue) Then
                            lNumNegative += 1
                        ElseIf Math.Abs(lCurrentValue) < lEpsilon Then
                            lNumZero += 1
                        ElseIf lNdayTs.Value(lIndex) < 0 Then
                            lNumNegative += 1
                        Else
                            lNumPositive += 1
                        End If
                    Next

                    lStr = lNumPositive
                    lRept.AppendLine(lStr.PadLeft(27) & " - non-zero values")
                    lStr = lNumZero
                    lRept.AppendLine(lStr.PadLeft(27) & " - zero values")
                    lStr = lNumNegative
                    lRept.AppendLine(lStr.PadLeft(27) & " - negative values (ignored)")

                    If lNumNegative = 0 AndAlso lNumZero = 0 Then
                        lPositiveNdayTs = lNdayTs
                    Else
                        Dim lCurNewValueIndex As Integer = 1
                        Dim lNumNewValues As Integer = lNumPositive
                        lPositiveNdayTs = New atcTimeseries(Nothing)
                        lPositiveNdayTs.Dates = New atcTimeseries(Nothing)
                        lPositiveNdayTs.numValues = lNumPositive
                        For lIndex = 1 To lNdayTsNonLog.numValues
                            If lNdayTsNonLog.Value(lIndex) - lEpsilon > 0 Then
                                lPositiveNdayTs.Value(lCurNewValueIndex) = lNdayTsNonLog.Value(lIndex)
                                lPositiveNdayTs.Dates.Value(lCurNewValueIndex) = lNdayTsNonLog.Dates.Value(lIndex)
                                lCurNewValueIndex += 1
                            End If
                        Next
                    End If

                    lColumn = 6
                    For lIndex = 1 To lPositiveNdayTs.numValues
                        If lColumn > 5 Then
                            lRept.AppendLine()
                            lRept.Append("     ")
                            lColumn = 1
                        End If
                        lStr = DoubleToString(lPositiveNdayTs.Value(lIndex), , "0.000")
                        lRept.Append(lStr.PadLeft(12))
                        lColumn += 1
                    Next

                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("  The following 7 statistics are based on non-zero values:")
                    lRept.AppendLine()

                    If lIsLog Then 'switch back to log version of n-day timeseries for stats
                        Dim lCurNewValueIndex As Integer = 1
                        Dim lNumNewValues As Integer = lNumPositive
                        lPositiveNdayTs = New atcTimeseries(Nothing)
                        lPositiveNdayTs.Dates = New atcTimeseries(Nothing)
                        lPositiveNdayTs.numValues = lNumPositive
                        For lIndex = 1 To lNdayTs.numValues
                            If lNdayTsNonLog.Value(lIndex) - lEpsilon > 0 Then
                                lPositiveNdayTs.Value(lCurNewValueIndex) = lNdayTs.Value(lIndex)
                                lPositiveNdayTs.Dates.Value(lCurNewValueIndex) = lNdayTs.Dates.Value(lIndex)
                                lCurNewValueIndex += 1
                            End If
                        Next
                    End If

                    lRept.AppendLine(FormatStat(lPositiveNdayTs, "Mean", lLogString))
                    lRept.AppendLine(FormatStat(lPositiveNdayTs, "Variance", lLogString))
                    lRept.AppendLine(FormatStat(lPositiveNdayTs, "Standard Deviation", lLogString))
                    lRept.AppendLine(FormatStat(lPositiveNdayTs, "Skew", lLogString, "Skewness"))
                    lRept.AppendLine(FormatStat(lPositiveNdayTs, "Standard Error of Skew", lLogString, "Standard Error of Skewness"))
                    lRept.AppendLine(FormatStat(lPositiveNdayTs, "Serial Correlation Coefficient", lLogString))
                    lRept.AppendLine(FormatStat(lPositiveNdayTs, "Coefficient of Variation", lLogString))

                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("       Non-exceedance     Recurrence        Parameter")
                    lRept.AppendLine("        Probability        Interval           Value  ")
                    lRept.AppendLine("        -----------       ----------        ---------")

                    'For Each lRecurrenceKey As String In pRecurrence.Keys
                    '    Dim lRecurrence As String = pRecurrence.Item(lRecurrenceKey)
                    '    Dim lNyears As Double = CDbl(lRecurrence)
                    '    lStr = DoubleToString(1 / lNyears, , "0.0000")
                    '    lRept.Append("  " & lStr.PadLeft(17))
                    '    lStr = DoubleToString(lNyears, , "0.00")
                    '    lRept.Append(lStr.PadLeft(17))
                    '    lStr = DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence, 0), , "0.000")
                    '    lRept.AppendLine(lStr.PadLeft(17))                        
                    'Next

                    Dim lReverseString As String = ""
                    Dim lThisRow As String
                    For Each lRecurrenceKey As String In pRecurrence.Keys
                        Dim lRecurrence As String = pRecurrence.Item(lRecurrenceKey)
                        Dim lNyears As Double = CDbl(lRecurrence)
                        lStr = DoubleToString(1 / lNyears, , "0.0000")
                        lThisRow = ("  " & lStr.PadLeft(17))
                        lStr = DoubleToString(lNyears, , "0.00")
                        lThisRow &= lStr.PadLeft(17)
                        lStr = DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence, 0), , "0.000")
                        lThisRow &= lStr.PadLeft(17)
                        lReverseString = lThisRow & vbCrLf & lReverseString
                    Next
                    lRept.Append(lReverseString)

                    lRept.AppendLine()
                    lRept.AppendLine()
                    'lRept.AppendLine("    7 statistics were added as attributes to data set   163:")
                    'lRept.AppendLine()
                    'lRept.AppendLine("          MEANVL STDDEV SKEWCF NUMZRO NONZRO LDIST ")
                    'lRept.AppendLine("          L04003")

                    lRept.AppendLine(vbFormFeed)
                Next
            Next
        Catch e As Exception
            lRept.AppendLine()
            lRept.AppendLine("Exception while creating report: " & e.Message)
        End Try
        Return lRept.ToString
    End Function

    Private Function FormatStat(ByVal aTS As atcTimeseries, ByVal aAttName As String, ByVal aLogString As String, Optional ByVal aAttLabel As String = "") As String
        If aAttLabel.Length = 0 Then aAttLabel = aAttName
        Dim lValue As String = DoubleToString(aTS.Attributes.GetValue(aAttName, 0), , "0.000")
        Dim lName As String = aAttLabel & " " & aLogString
        Return "  " & lName & Space(34 - aAttLabel.Length) & lValue.PadLeft(9)
    End Function
End Class
