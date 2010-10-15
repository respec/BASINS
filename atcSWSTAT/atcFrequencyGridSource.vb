Imports atcData
Imports atcUtility

Public Class atcFrequencyGridSource
    Inherits atcControls.atcGridSource

    Private pOnlyNdays() As Double
    Private pOnlyReturns() As Double

    Private pDataGroup As atcTimeseriesGroup
    Private pNdays As SortedList
    Private pRecurrence As SortedList
    'Private pAdj As SortedList
    'Private pAdjProb As SortedList
    Private pHigh As Boolean

    Private pCalculatedNdays As New ArrayList
    Private pCalculatedRecurrence As New ArrayList

    ''' <summary>
    ''' Create grid source for displaying N-Day/Return Interval values
    ''' </summary>
    ''' <param name="aDataGroup">Timeseries data whose attributes to display</param>
    ''' <param name="aNdays">Which numbers of days are displayed (Nothing = display all present in attributes)</param>
    ''' <param name="aNyears">Which return periods are displayed (Nothing = display all present in attributes)</param>
    ''' <remarks></remarks>
    Sub New(ByVal aDataGroup As atcData.atcTimeseriesGroup, ByVal aNdays() As Double, ByVal aNyears() As Double)
        Dim lAdjStr As String
        Dim lAdjProbStr As String
        Dim lHighLow As String
        Dim lKey As String
        MyBase.ColorCells = True
        pDataGroup = aDataGroup
        pRecurrence = New SortedList
        pNdays = New SortedList
        'pAdj = New SortedList
        'pAdjProb = New SortedList

        For Each lData As atcDataSet In pDataGroup
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing Then
                    lAdjStr = ""
                    lAdjProbStr = ""
                    If pHigh Then lHighLow = "High" Else lHighLow = "Low"

                    If lAttribute.Arguments.ContainsAttribute("Nday") Then
                        Dim lNdays As String = lAttribute.Arguments.GetFormattedValue("Nday")
                        lKey = Format(lAttribute.Arguments.GetValue("Nday"), "00000.0000")
                        If Not pNdays.ContainsKey(lKey) AndAlso IsIn(lNdays, aNdays) Then
                            pNdays.Add(lKey, lNdays)
                        End If
                        lAdjStr &= lNdays & lHighLow
                    End If
                    If lAttribute.Arguments.ContainsAttribute("Return Period") Then
                        Dim lNyears As String = lAttribute.Arguments.GetFormattedValue("Return Period")
                        lKey = Format(lAttribute.Arguments.GetValue("Return Period"), "00000.0000")
                        If Not pRecurrence.ContainsKey(lKey) AndAlso IsIn(lNyears, aNyears) Then
                            pRecurrence.Add(lKey, lNyears)
                        End If ' Not pRecurrence.ContainsKey(lKey)
                        'lAdjStr &= lNyears
                        'lKey = lData.Serial & ":" & lKey
                        ''Add the adjusted probability and adjusted parameter values
                        'lAdjStr &= "Adj"
                        'lAdjProbStr = lAdjStr & "Prob"
                        'If lData.Attributes.ContainsAttribute(lAdjStr) AndAlso Not pAdj.ContainsKey(lKey) Then
                        '    pAdj.Add(lKey, lData.Attributes.GetValue(lAdjStr).ToString)
                        'End If
                        'If lData.Attributes.ContainsAttribute(lAdjProbStr) AndAlso Not pAdjProb.ContainsKey(lKey) Then
                        '    pAdjProb.Add(lKey, lData.Attributes.GetValue(lAdjProbStr).ToString)
                        'End If
                    End If ' lAttribute.Arguments.ContainsAttribute("Return Period)
                End If 'Not lAttribute.Arguments Is Nothing
            Next ' lAttribute
        Next ' lData

        If aNdays IsNot Nothing Then
            For Each lNdays As Double In aNdays
                lKey = Format(lNdays, "00000.0000")
                If Not pNdays.ContainsKey(lKey) Then
                    pNdays.Add(lKey, DoubleToString(lNdays))
                End If
            Next
        End If

        If aNyears IsNot Nothing Then
            For Each lNYears As Double In aNyears
                lKey = Format(lNYears, "00000.0000")
                If Not pRecurrence.ContainsKey(lKey) Then
                    pRecurrence.Add(lKey, Format(lNYears, "0"))
                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' Return True if aArray is Nothing or else it contains a number within 1e-30 of aNumber
    ''' </summary>
    Private Function IsIn(ByVal aNumber As Double, ByVal aArray() As Double) As Boolean
        If aArray Is Nothing Then
            Return True
        Else
            For Each lCheck As Double In aArray
                If Math.Abs(lCheck - aNumber) < 1.0E+30 Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

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
                Return 4
            Else
                Return pNdays.Count + FixedColumns
            End If
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property Rows() As Integer
        Get
            Try
                Return pDataGroup.Count * pRecurrence.Count + FixedRows
            Catch
                Return 1
            End Try
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property FixedRows() As Integer
        Get
            Return 1
        End Get
        Set(ByVal value As Integer)
            'Ignore attemts to change this property
        End Set
    End Property

    Overrides Property FixedColumns() As Integer
        Get
            Return 3
        End Get
        Set(ByVal value As Integer)
            'Ignore attemts to change this property
        End Set
    End Property

    Public Function DataSetAt(ByVal aRow As Integer) As atcDataSet
        Return pDataGroup((aRow - FixedRows) \ pRecurrence.Count)
    End Function

    Public Function NdaysAt(ByVal aColumn As Integer) As String
        Return pNdays.GetByIndex(aColumn - FixedColumns)
    End Function

    Public Function RecurrenceAt(ByVal aRow As Integer) As String
        'remove any thousands commas in return period
        Return ReplaceString(pRecurrence.GetByIndex((aRow - Me.FixedRows) Mod pRecurrence.Count), ",", "")
    End Function

    Public Shared Function DataSetLabel(ByVal aDataSet As atcDataSet) As String
        Dim lLabel As String = ""
        Dim lScenario As String = aDataSet.Attributes.GetValue("Scenario", "")
        If lScenario.Length > 0 AndAlso lScenario <> "<unk>" Then
            lLabel = lScenario
        End If
        Dim lLocation As String = aDataSet.Attributes.GetValue("STAID", "")
        If lLocation.Length = 0 Then 'use Location attribute for start of location header
            lLocation = aDataSet.Attributes.GetValue("Location", "")
        End If
        If lLocation.Length > 0 AndAlso lLocation <> "<unk>" Then
            If lLabel.Length > 0 Then lLabel &= " "
            lLabel &= lLocation
        End If
        Dim lConstituent As String = aDataSet.Attributes.GetValue("Constituent", "")
        If lConstituent.Length > 0 AndAlso lConstituent <> "<unk>" Then
            If lLabel.Length > 0 Then lLabel &= " "
            lLabel &= lConstituent
        End If
        Return lLabel
    End Function

    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
        Get
            If aRow = 0 Then
                Select Case aColumn
                    Case 0 : Return "Data Set"
                    Case 1 : Return "Probability"
                    Case 2 : Return "Return Period"
                    Case Else : Return NdaysAt(aColumn)
                End Select
            Else
                Select Case aColumn
                    Case 0 : Return DataSetLabel(DataSetAt(aRow))
                    Case 1 : Return DoubleToString(1 / RecurrenceAt(aRow), , "0.0000")
                    Case 2 : Return RecurrenceAt(aRow)
                    Case Else
                        Dim lDataSet As atcDataSet = DataSetAt(aRow)
                        Dim lAttrName As String = NdaysAt(aColumn)
                        If pHigh Then lAttrName &= "High" Else lAttrName &= "Low"
                        lAttrName &= RecurrenceAt(aRow)
                        'If pAdj.Count > 0 Then
                        '    RecurrenceAt(aRow)
                        '    Dim lKey As String = lDataSet.Serial & ":" & Format(CDbl(RecurrenceAt(aRow)), "00000.0000")
                        '    If pAdj.Item(lKey) Is Nothing Then
                        '        CellValue = ""
                        '    Else
                        '        CellValue = DoubleToString(pAdj.Item(lKey), , "0.000")
                        '    End If
                        'Else
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
                        If lDataSet.Attributes.ContainsAttribute(lAttrName & "Adj") Then
                            CellValue = lDataSet.Attributes.GetFormattedValue(lAttrName & "Adj")
                        ElseIf lDataSet.Attributes.ContainsAttribute(lAttrName) Then
                            CellValue = lDataSet.Attributes.GetFormattedValue(lAttrName)
                        Else
                            CellValue = ""
                        End If
                        'End If
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
            If aColumn < FixedColumns OrElse aRow < FixedRows Then
                Return System.Drawing.SystemColors.Control
            Else
                Return System.Drawing.SystemColors.Window
            End If
        End Get
        Set(ByVal Value As System.Drawing.Color)
        End Set
    End Property

    Public Function AllNday() As atcTimeseriesGroup
        Dim lAllNday As New atcTimeseriesGroup
        Dim lCopyAttrs() As String = {"MEANDD", "SDND", "SKWND", "LDIST"}

        For Each lTimeseries As atcTimeseries In pDataGroup
            Dim lAttributes As atcDataAttributes = lTimeseries.Attributes
            For Each lNdaysKey As String In pNdays.Keys
                Dim lNdays As String = pNdays.Item(lNdaysKey)
                Dim lAttrName As String = lNdays
                If pHigh Then lAttrName &= "High" Else lAttrName &= "Low"
                Dim lNdayAttribute As atcDefinedValue = lAttributes.GetDefinedValue(lAttrName & pRecurrence.GetByIndex(0))
                Dim lNdayTs As atcTimeseries = lNdayAttribute.Arguments.GetValue("NDayTimeseries")
                If lNdayTs.Attributes.ContainsAttribute("NDayTimeseries") Then 'find non-log version
                    lNdayTs = lNdayTs.Attributes.GetValue("NDayTimeseries")
                End If

                lNdayTs.Attributes.GetValue("Skew") ' Trigger calculation of all basic attributes

                For lRow As Integer = Me.FixedRows To Me.FixedRows + pRecurrence.Count - 1
                    Dim lCompleteAttName As String = lAttrName & RecurrenceAt(lRow)
                    If lAttributes.ContainsAttribute(lCompleteAttName) Then
                        Try
                            lNdayAttribute = lAttributes.GetDefinedValue(lCompleteAttName)
                            Dim lValue As Double = lNdayAttribute.Value
                            If lAttributes.ContainsAttribute(lCompleteAttName & "Adj") Then
                                lValue = lAttributes.GetValue(lCompleteAttName & "Adj")
                            End If
                            lNdayTs.Attributes.SetValue(lNdayAttribute.Definition, lNdayAttribute.Value, lNdayAttribute.Arguments)
                        Catch
                        End Try
                    End If
                Next

                For Each lAttrName In lCopyAttrs
                    If lAttributes.ContainsAttribute(lAttrName) Then
                        lNdayAttribute = lAttributes.GetDefinedValue(lAttrName)
                        lNdayTs.Attributes.SetValue(lNdayAttribute.Definition, lNdayAttribute.Value, lNdayAttribute.Arguments)
                    End If
                Next

                lAllNday.Add(lNdayTs)
            Next
        Next
        Return lAllNday
    End Function

    Public Function CreateReport(Optional ByVal aExpFmt As Boolean = False) As String
        Dim lStartDate As Date
        Dim lStartDateAnnual As Date
        Dim lEndDate As Date
        Dim lStr As String
        Dim lIndex As Integer
        Dim lColumn As Integer
        Dim lRept As New System.Text.StringBuilder
        Try
            For Each lTimeseries As atcTimeseries In pDataGroup
                Dim lAttributes As atcDataAttributes = lTimeseries.Attributes
                For Each lNdaysKey As String In pNdays.Keys
                    Dim lNdays As String = pNdays.Item(lNdaysKey)
                    Dim lAttrName As String = lNdays
                    If pHigh Then lAttrName &= "High" Else lAttrName &= "Low"
                    Dim lNdayAttribute As atcDefinedValue = lAttributes.GetDefinedValue(lAttrName & pRecurrence.GetByIndex(0))
                    If lNdayAttribute Is Nothing Then
                        Debug.Print("why!")
                    End If
                    Dim lNdayTs As atcTimeseries = lNdayAttribute.Arguments.GetValue("NDayTimeseries")
                    Dim lNdayTsNonLog As atcTimeseries
                    Dim lIsLog As Boolean = False
                    Dim lLogString As String = "   "

                    If lAttributes.GetValue("LDIST", "") = "LP3" Then
                        'Get original version of NDayTimeseries (not log version)
                        lIsLog = True
                        lLogString = "(logs)"
                        lNdayTsNonLog = lAttributes.GetValue("NonLogNDayTimeseries")
                    Else
                        lNdayTsNonLog = lNdayTs
                    End If
                    Dim lLocation As String = lAttributes.GetValue("STAID", "")
                    If lLocation.Length = 0 Then 'use Location attribute for start of location header
                        lLocation = lAttributes.GetValue("Location", "")
                    End If
                    lLocation &= " " & lAttributes.GetValue("STANAM", "")
                    lStartDate = Date.FromOADate(lNdayTs.Dates.Value(0))
                    lStartDateAnnual = Date.FromOADate(lNdayTs.Dates.Value(1))
                    lEndDate = Date.FromOADate(lNdayTs.Dates.Value(lNdayTs.numValues))

                    Dim lPositiveNdayTs As atcTimeseries
                    Dim lEpsilon As Double = (lNdayTs.Attributes.GetValue("Max") - lNdayTs.Attributes.GetValue("Min")) / Math.Pow(10, 9)

                    Dim lNumZero As Integer = lNdayTsNonLog.Attributes.GetValue("Count Zero", -1)
                    Dim lNumMissing As Integer = lNdayTsNonLog.Attributes.GetValue("Count Missing", 0)
                    Dim lNumPositive As Integer = lNdayTsNonLog.numValues - lNumZero - lNumMissing

                    If lNumMissing = 0 AndAlso lNumZero = 0 Then
                        lPositiveNdayTs = lNdayTsNonLog
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

                    Dim lLogNdayTs As atcTimeseries 'set to log version if using logs, otherwise contains non-log values
                    If lIsLog Then 'log version of n-day timeseries for stats
                        lLogNdayTs = New atcTimeseries(Nothing)
                        lLogNdayTs.Dates = New atcTimeseries(Nothing)
                        Dim lCurNewValueIndex As Integer = 1
                        Dim lNumNewValues As Integer = lNumPositive
                        lLogNdayTs.numValues = lNumPositive
                        For lIndex = 1 To lNdayTs.numValues
                            If lNdayTsNonLog.Value(lIndex) - lEpsilon > 0 Then
                                lLogNdayTs.Value(lCurNewValueIndex) = Math.Log10(lNdayTs.Value(lIndex))
                                lLogNdayTs.Dates.Value(lCurNewValueIndex) = lNdayTs.Dates.Value(lIndex)
                                lCurNewValueIndex += 1
                            End If
                        Next
                    Else
                        lLogNdayTs = lPositiveNdayTs
                    End If

                    If aExpFmt Then 'build tabbed export file
                        lRept.AppendLine("Identifier" & vbTab & lAttributes.GetValue("STAID", ""))
                        If pHigh Then
                            lRept.AppendLine("    Parameter" & vbTab & "H" & StrPad(CStr(lNdays), 3, "0"))
                        Else
                            lRept.AppendLine("    Parameter" & vbTab & "L" & StrPad(CStr(lNdays), 3, "0"))
                        End If
                        lRept.AppendLine("    SeasBg" & vbTab & lStartDate.Month)
                        lRept.AppendLine("    SeaDBg" & vbTab & lStartDate.Day)
                        lRept.AppendLine("    SeasNd" & vbTab & lEndDate.Month)
                        lRept.AppendLine("    SeaDNd" & vbTab & lEndDate.Day)
                        lRept.AppendLine("    BegYear" & vbTab & lStartDateAnnual.Year)
                        lRept.AppendLine("    EndYear" & vbTab & lEndDate.Year)
                        lRept.AppendLine("    NumZro" & vbTab & lNumZero)
                        lRept.AppendLine("    NonZro" & vbTab & lNumPositive)
                        lRept.AppendLine("    NumNeg" & vbTab & lNumMissing)
                        If lIsLog Then
                            lRept.AppendLine("    Ldist" & vbTab & "LP3")
                            lRept.AppendLine("    MeanND" & vbTab & DoubleToString(lLogNdayTs.Attributes.GetValue("Mean", 0), , "0.000"))
                            lRept.AppendLine("    SdNd" & vbTab & DoubleToString(lLogNdayTs.Attributes.GetValue("Standard Deviation", 0), , "0.000"))
                            lRept.AppendLine("    SkwNd" & vbTab & DoubleToString(lLogNdayTs.Attributes.GetValue("Skew", 0), , "0.000"))
                        Else
                            lRept.AppendLine("    Ldist" & vbTab & "LP")
                            lRept.AppendLine("    Meanvl" & vbTab & DoubleToString(lLogNdayTs.Attributes.GetValue("Mean", 0), , "0.000"))
                            lRept.AppendLine("    StdDev" & vbTab & DoubleToString(lLogNdayTs.Attributes.GetValue("Standard Deviation", 0), , "0.000"))
                            lRept.AppendLine("    Skewcf" & vbTab & DoubleToString(lLogNdayTs.Attributes.GetValue("Skew", 0), , "0.000"))
                        End If
                    Else
                        lRept.AppendLine()
                        lRept.AppendLine()
                        lRept.AppendLine("Program SWStat             U.S. GEOLOGICAL SURVEY             Seq.000.000")
                        lRept.AppendLine("Ver. 5.0 Beta 1   Log-Pearson & Pearson Type III Statistics   Run Date / Time")
                        lRept.AppendLine("10/1/2009                based on USGS Program A193           " & System.DateTime.Now.ToString)
                        lRept.AppendLine()
                        lRept.AppendLine(" Notice -- Log-Pearson Type III or Pearson Type III distributions are for")
                        lRept.AppendLine("           preliminary computations. Users are responsible for assessment")
                        lRept.AppendLine("           and interpretation.")


                        lRept.AppendLine()
                        lRept.AppendLine()
                        lRept.AppendLine("       Description:  " & lLocation)

                        'Dates by USGS SWSTAT convention as follows
                        ' for seasons - if season ends on month boundary - label with last day of previous month - 4/1 -> March 31
                        '               otherwise - label with day specified - 9/15 -> September 15
                        ' for years - if 
                        Dim lEndDateForPrint As Date = lEndDate
                        Dim lEndYear As Integer = lEndDate.Year
                        Dim lEndMon As Integer = lEndDate.Month
                        Dim lEndDay As Integer = lEndDate.Day
                        If lEndDay = 1 Then
                            lEndMon = lEndMon - 1
                            If lEndMon = 0 Then
                                lEndMon = 12
                            End If
                            lEndDay = Date.DaysInMonth(lEndYear, lEndMon)
                            lEndDateForPrint = New Date(lEndYear, lEndMon, lEndDay)
                        End If
                        lRept.AppendLine("            Season:  " & lStartDate.ToString("MMMM") & lStartDate.Day.ToString.PadLeft(3) & " - " & _
                                                                   lEndDateForPrint.ToString("MMMM") & lEndDateForPrint.Day.ToString.PadLeft(3))

                        'TODO: verify how the USGS convention for this works
                        Dim lStartYear As Integer = lStartDateAnnual.Year
                        If Not pHigh AndAlso lEndMon <= lStartDate.Month Then
                            lStartYear -= 1
                        End If
                        lRept.AppendLine("  Period of Record:  " & _
                                         lStartDate.ToString("MMMM") & lStartDate.Day.ToString.PadLeft(3) & ", " & lStartYear & " - " & _
                                         lEndDateForPrint.ToString("MMMM") & lEndDateForPrint.Day.ToString.PadLeft(3) & ", " & lEndYear)

                        lStr = lNdays & "-day "
                        If pHigh Then lStr &= "high" Else lStr &= "low"
                        lRept.AppendLine("         Parameter:  " & lStr)

                        lStr = lNumPositive
                        lRept.AppendLine("   non-zero values:  " & lStr.PadLeft(4))
                        lStr = lNumZero
                        lRept.AppendLine("       zero values:  " & lStr.PadLeft(4))
                        lStr = lNumMissing
                        lRept.AppendLine("   negative values:  " & lStr.PadLeft(4) & "  (ignored)")
                        ''''

                        lRept.AppendLine()
                        lRept.AppendLine("Input time series (zero and negative values not included in listing.)")
                        lColumn = 9
                        For lIndex = 1 To lPositiveNdayTs.numValues
                            If lColumn > 8 Then
                                lRept.AppendLine()
                                'lRept.Append("     ")
                                lColumn = 1
                            End If
                            lStr = DoubleToString(lPositiveNdayTs.Value(lIndex), , "0.000")
                            lRept.Append(lStr.PadLeft(10))
                            lColumn += 1
                        Next

                        lRept.AppendLine()
                        lRept.AppendLine()
                        lRept.AppendLine()
                        If lIsLog Then
                            lRept.AppendLine("  LOG PEARSON TYPE III Frequency Curve Parameters")
                            lRept.AppendLine("  (based on logs of the non-zero values)")
                        Else
                            lRept.AppendLine("  PEARSON TYPE III Frequency Curve Parameters")
                            lRept.AppendLine("  (based on non-zero values)")
                        End If
                        lRept.AppendLine()
                        lRept.AppendLine(FormatStat(lLogNdayTs, "Mean", lLogString))
                        lRept.AppendLine(FormatStat(lLogNdayTs, "Variance", lLogString))
                        lRept.AppendLine(FormatStat(lLogNdayTs, "Standard Deviation", lLogString))
                        lRept.AppendLine(FormatStat(lLogNdayTs, "Skew", lLogString, "Skewness"))
                        lRept.AppendLine(FormatStat(lLogNdayTs, "Standard Error of Skew", lLogString, "Standard Error of Skewness"))
                        lRept.AppendLine(FormatStat(lLogNdayTs, "Serial Correlation Coefficient", lLogString))
                        lRept.AppendLine(FormatStat(lLogNdayTs, "Coefficient of Variation", lLogString))
                        lRept.AppendLine()
                        lRept.AppendLine()
                        lRept.AppendLine("Frequency Curve - Parameter values at selected probabilities")
                        lRept.AppendLine()

                        If pHigh Then
                            If lNumZero > 0 Then
                                lRept.AppendLine("                            Adjusted    Variance    95-Pct Confidence")
                            Else
                                lRept.AppendLine("                                        Variance    95-Pct Confidence")
                            End If
                            lRept.AppendLine(" Exceedence     Recurrence  Parameter      of          Intervals")
                        Else
                            If lNumZero > 0 Then
                                lRept.AppendLine("   Non-                     Adjusted    Variance    95-Pct Confidence")
                            Else
                                lRept.AppendLine("   Non-                                 Variance    95-Pct Confidence")
                            End If
                            lRept.AppendLine(" exceedance     Recurrence  Parameter      of          Intervals")
                        End If
                        'If just aligns code
                        If True Then
                            lRept.AppendLine(" Probability     Interval     Value     Estimate    Lower      Upper")
                            lRept.AppendLine(" -----------    ----------  ---------   --------  ---------  ---------")
                        End If
                    End If

                    Dim lExpRows(6) As String
                    If aExpFmt Then
                        lExpRows(0) = "    Recur"
                        If pHigh Then
                            lExpRows(1) = "    Exceed"
                        Else
                            lExpRows(1) = "    NonExc"
                        End If
                        If pHigh Then
                            lExpRows(2) = "    " & lNdays & "-Day High"
                        Else
                            lExpRows(2) = "    " & lNdays & "-Day Low"
                        End If
                        lExpRows(3) = "    K-Value"
                        lExpRows(4) = "    Variance"
                        lExpRows(5) = "    CL_Low"
                        lExpRows(6) = "    CL_Up"
                    End If

                    Dim lReverseString As String = ""
                    Dim lThisRow As String = ""
                    For Each lRecurrenceKey As String In pRecurrence.Keys
                        Dim lRecurrence As String = pRecurrence.Item(lRecurrenceKey).ToString.Replace(",", "")
                        Dim lNyears As Double = CDbl(lRecurrence)

                        lStr = DoubleToString(1 / lNyears, , "0.0000", "0.000")
                        If aExpFmt Then
                            lExpRows(0) &= vbTab & DoubleToString(lNyears, , "0.00")
                            lExpRows(1) &= vbTab & lStr
                        Else
                            lThisRow = ("  " & lStr.PadLeft(10))

                            If lNyears < 1.05 Then
                                lThisRow &= DoubleToString(lNyears, , "0.000", "0.0000").PadLeft(14)
                            Else
                                lThisRow &= DoubleToString(lNyears, , "0.00").PadLeft(13) & " "
                            End If
                        End If

                        If lNumZero > 0 Then 'If there is/are a zero annual event, then display adjusted values 
                            'but, don't display adjusted probs
                            'lThisRow &= pAdjProb.Item(lRecurrenceKey).PadLeft(15)
                            Dim lAdjVal As String = ""
                            If lAttributes.ContainsAttribute(lAttrName & lRecurrence & "Adj") Then
                                lAdjVal = DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence & "Adj"), , "0.000", "0.000")
                            End If
                            If aExpFmt Then
                                lExpRows(2) &= vbTab & lAdjVal
                            Else
                                lThisRow &= lAdjVal.PadLeft(11)
                            End If
                        ElseIf aExpFmt Then
                            lExpRows(2) &= vbTab & DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence, 0), , "0.000", "0.000")
                        Else
                            lThisRow &= DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence, 0), , "0.000", "0.000").PadLeft(11)
                        End If

                        'K Value (export only), variance of estimate and confidence intervals
                        If aExpFmt Then
                            lExpRows(3) &= vbTab & DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence & " K Value", 0), , "0.000", "0.000")
                            lExpRows(4) &= vbTab & DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence & " Variance of Estimate", 0), , "0.000", "0.000")
                            lExpRows(5) &= vbTab & DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence & " CI Lower", 0), , "0.000", "0.000")
                            lExpRows(6) &= vbTab & DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence & " CI Upper", 0), , "0.000", "0.000")
                        Else
                            lThisRow &= DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence & " Variance of Estimate", 0), , "0.000", "0.000").PadLeft(11)
                            lThisRow &= DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence & " CI Lower", 0), , "0.000", "0.000").PadLeft(11)
                            lThisRow &= DoubleToString(lAttributes.GetValue(lAttrName & lRecurrence & " CI Upper", 0), , "0.000", "0.000").PadLeft(11)
                        End If

                        If pHigh Then
                            lReverseString &= lThisRow & vbCrLf
                        Else
                            lReverseString = lThisRow & vbCrLf & lReverseString
                        End If
                    Next ' for each lRecurrenceKey As String In pRecurrence.Keys

                    If aExpFmt Then
                        For i As Integer = 0 To UBound(lExpRows)
                            lRept.AppendLine(lExpRows(i))
                        Next
                    Else
                        lRept.Append(lReverseString)

                        lRept.AppendLine()
                        If lNumZero > 0 Then
                            lRept.AppendLine(" Note -- Conditional Probability Adjustment applied because of zero flow(s),")
                            lRept.AppendLine("         Adjusted parameter values (column 3) correspond with non-exceedence")
                            lRept.AppendLine("         probabilities (column 1) and recurrence intervals (column 2).")
                        End If
                        lRept.AppendLine()
                        'lRept.AppendLine("    7 statistics were added as attributes to data set   163:")
                        'lRept.AppendLine()
                        'lRept.AppendLine("          MEANVL STDDEV SKEWCF NUMZRO NONZRO LDIST ")
                        'lRept.AppendLine("          L04003")

                        lRept.AppendLine(vbFormFeed)
                    End If
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
