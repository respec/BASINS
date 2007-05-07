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
        Dim lEndDate As Date
        Dim lStr As String
        Dim lRept As New System.Text.StringBuilder
        For Each lTimeseries As atcTimeseries In pDataGroup
            Dim lAttributes As atcDataAttributes = lTimeseries.Attributes            
            lStartDate = Date.FromOADate(lTimeseries.Dates.Value(0))
            lEndDate = Date.FromOADate(lTimeseries.Dates.Value(lTimeseries.numValues))
            For Each lRecurrenceKey As String In pRecurrence.Keys
                Dim lRecurrence As String = pRecurrence.Item(lRecurrenceKey)
                For Each lNdaysKey As String In pNdays.Keys
                    Dim lNdays As String = pNdays.Item(lNdaysKey)
                    Dim lAttrName As String = lNdays
                    If pHigh Then lAttrName &= "High" Else lAttrName &= "Low"
                    lAttrName &= lRecurrence
                    Dim lNdayAttribute As atcDefinedValue = lAttributes.GetDefinedValue(lAttrName)
                    Dim lNdayTs As atcTimeseries = lNdayAttribute.Arguments.GetValue("NDayTimeseries")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("                  Pearson Type III Statistics")
                    lRept.AppendLine()
                    lRept.AppendLine("                  (based on USGS Program A193)")
                    lRept.AppendLine()
                    lRept.AppendLine("  Notice -- Use of Pearson Type III distribution is for")
                    lRept.AppendLine("            preliminary computations.  User is responsible")
                    lRept.AppendLine("            for assessment and interpretation.")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("               " & lAttributes.GetValue("Location") & " " & lAttributes.GetValue("Description"))
                    lRept.AppendLine(lStartDate.ToString("MMMM d").PadLeft(27) & " - start of season")
                    lRept.AppendLine(lEndDate.ToString("MMMM d").PadLeft(27) & " - end of season")
                    lRept.AppendLine("                " & lStartDate.Year & " - " & lEndDate.Year & " - time period")
                    lStr = lNdays & "-day "
                    If pHigh Then lStr &= "high" Else lStr &= "low"
                    lRept.AppendLine(lStr.PadLeft(27) & " - parameter")
                    lRept.AppendLine("                         ** - non-zero values")
                    lRept.AppendLine("                          * - zero values")
                    lRept.AppendLine("                          * - negative values (ignored)")
                    lRept.AppendLine()
                    lRept.AppendLine("            6.025       9.875      11.750      36.500      11.125")
                    lRept.AppendLine("           33.250       9.350      15.500       6.775      21.250")
                    lRept.AppendLine("           13.250      23.000      17.000      10.550      42.500")
                    lRept.AppendLine("           38.000      12.250       7.175      29.000      11.300")
                    lRept.AppendLine("           12.500       3.400      32.750")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("  The following 7 statistics are based on non-zero values:")
                    lRept.AppendLine()
                    lRept.AppendLine("  Mean                                     18.003")
                    lRept.AppendLine("  Variance                                134.982")
                    lRept.AppendLine("  Standard Deviation                       11.618")
                    lRept.AppendLine("  Skewness                                  0.851")
                    lRept.AppendLine("  Standard Error of Skewness                0.481")
                    lRept.AppendLine("  Serial Correlation Coefficient           -0.138")
                    lRept.AppendLine("  Coefficient of Variation                  0.645")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("       Non-exceedance     Recurrence        Parameter")
                    lRept.AppendLine("        Probability        Interval           Value  ")
                    lRept.AppendLine("        -----------       ----------        ---------")
                    lRept.AppendLine("             0.0100           100.00            0.000")
                    lRept.AppendLine("             0.0200            50.00            0.000")
                    lRept.AppendLine("             0.0500            20.00            2.080")
                    lRept.AppendLine("             0.1000            10.00            4.569")
                    lRept.AppendLine("             0.2000             5.00            8.068")
                    lRept.AppendLine("             0.3333             3.00           11.840")
                    lRept.AppendLine("             0.5000             2.00           16.375")
                    lRept.AppendLine("             0.8000             1.25           27.000")
                    lRept.AppendLine("             0.9000             1.11           33.544")
                    lRept.AppendLine("             0.9600             1.04           41.309")
                    lRept.AppendLine("             0.9800             1.02           46.768")
                    lRept.AppendLine("             0.9900             1.01           51.982")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("    7 statistics were added as attributes to data set   163:")
                    lRept.AppendLine()
                    lRept.AppendLine("          MEANVL STDDEV SKEWCF NUMZRO NONZRO LDIST ")
                    lRept.AppendLine("          L04003")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("                  Pearson Type III Statistics")
                    lRept.AppendLine("                           SWSTAT 4.1 ")
                    lRept.AppendLine("                  (based on USGS Program A193)")
                    lRept.AppendLine()
                    lRept.AppendLine("  Notice -- Use of Pearson Type III distribution is for")
                    lRept.AppendLine("            preliminary computations.  User is responsible")
                    lRept.AppendLine("            for assessment and interpretation.")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("               11516530 Klamath River below Iron Gate Dam, CA.                 ")
                    lRept.AppendLine("                    June 15 - start of season")
                    lRept.AppendLine("               September 15 - end of season")
                    lRept.AppendLine("                1960 - 1982 - time period")
                    lRept.AppendLine("                  4-day low - parameter")
                    lRept.AppendLine("                         22 - non-zero values")
                    lRept.AppendLine("                          0 - zero values")
                    lRept.AppendLine("                          1 - negative values (ignored)")
                    lRept.AppendLine()
                    lRept.AppendLine("          718.250     669.750     714.000     775.000     708.000")
                    lRept.AppendLine("          662.000     695.750     695.750     690.500     699.000")
                    lRept.AppendLine("          712.750     693.000     666.250     725.000     708.750")
                    lRept.AppendLine("          711.500     707.250     727.000     697.000     730.750")
                    lRept.AppendLine("          726.000     709.250")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("  The following 7 statistics are based on non-zero values:")
                    lRept.AppendLine()
                    lRept.AppendLine("  Mean                                    706.477")
                    lRept.AppendLine("  Variance                                593.505")
                    lRept.AppendLine("  Standard Deviation                       24.362")
                    lRept.AppendLine("  Skewness                                  0.536")
                    lRept.AppendLine("  Standard Error of Skewness                0.491")
                    lRept.AppendLine("  Serial Correlation Coefficient            0.046")
                    lRept.AppendLine("  Coefficient of Variation                  0.034")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("       Non-exceedance     Recurrence        Parameter")
                    lRept.AppendLine("        Probability        Interval           Value  ")
                    lRept.AppendLine("        -----------       ----------        ---------")
                    lRept.AppendLine("             0.0100           100.00          659.516")
                    lRept.AppendLine("             0.0200            50.00          663.686")
                    lRept.AppendLine("             0.0500            20.00          670.449")
                    lRept.AppendLine("             0.1000            10.00          676.990")
                    lRept.AppendLine("             0.2000             5.00          685.605")
                    lRept.AppendLine("             0.3333             3.00          694.322")
                    lRept.AppendLine("             0.5000             2.00          704.309")
                    lRept.AppendLine("             0.8000             1.25          726.091")
                    lRept.AppendLine("             0.9000             1.11          738.758")
                    lRept.AppendLine("             0.9600             1.04          753.269")
                    lRept.AppendLine("             0.9800             1.02          763.203")
                    lRept.AppendLine("             0.9900             1.01          772.522")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("    7 statistics were added as attributes to data set   168:")
                    lRept.AppendLine()
                    lRept.AppendLine("          MEANVL STDDEV SKEWCF NUMZRO NONZRO LDIST ")
                    lRept.AppendLine("          L04003")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("                  Pearson Type III Statistics")
                    lRept.AppendLine("                           SWSTAT 4.1 ")
                    lRept.AppendLine("                  (based on USGS Program A193)")
                    lRept.AppendLine()
                    lRept.AppendLine("  Notice -- Use of Pearson Type III distribution is for")
                    lRept.AppendLine("            preliminary computations.  User is responsible")
                    lRept.AppendLine("            for assessment and interpretation.")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("               11520500 Klamath River near Seiad Valley, CA.                   ")
                    lRept.AppendLine("                    June 15 - start of season")
                    lRept.AppendLine("               September 15 - end of season")
                    lRept.AppendLine("                1960 - 1982 - time period")
                    lRept.AppendLine("                  4-day low - parameter")
                    lRept.AppendLine("                         23 - non-zero values")
                    lRept.AppendLine("                          0 - zero values")
                    lRept.AppendLine("                          0 - negative values (ignored)")
                    lRept.AppendLine()
                    lRept.AppendLine("         1060.000    1026.250    1032.750     994.500    1032.500")
                    lRept.AppendLine("         1030.000     951.750    1137.500     910.500    1105.000")
                    lRept.AppendLine("          970.000    1335.000    1027.500     810.750    1322.500")
                    lRept.AppendLine("         1310.000     916.250     792.500    1077.500     933.500")
                    lRept.AppendLine("          993.250     810.250    1142.500")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("  The following 7 statistics are based on non-zero values:")
                    lRept.AppendLine()
                    lRept.AppendLine("  Mean                                   1031.402")
                    lRept.AppendLine("  Variance                              22345.283")
                    lRept.AppendLine("  Standard Deviation                      149.483")
                    lRept.AppendLine("  Skewness                                  0.553")
                    lRept.AppendLine("  Standard Error of Skewness                0.481")
                    lRept.AppendLine("  Serial Correlation Coefficient           -0.144")
                    lRept.AppendLine("  Coefficient of Variation                  0.145")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("       Non-exceedance     Recurrence        Parameter")
                    lRept.AppendLine("        Probability        Interval           Value  ")
                    lRept.AppendLine("        -----------       ----------        ---------")
                    lRept.AppendLine("             0.0100           100.00          745.078")
                    lRept.AppendLine("             0.0200            50.00          770.231")
                    lRept.AppendLine("             0.0500            20.00          811.156")
                    lRept.AppendLine("             0.1000            10.00          850.858")
                    lRept.AppendLine("             0.2000             5.00          903.314")
                    lRept.AppendLine("             0.3333             3.00          956.556")
                    lRept.AppendLine("             0.5000             2.00         1017.696")
                    lRept.AppendLine("             0.8000             1.25         1151.534")
                    lRept.AppendLine("             0.9000             1.11         1229.609")
                    lRept.AppendLine("             0.9600             1.04         1319.216")
                    lRept.AppendLine("             0.9800             1.02         1380.659")
                    lRept.AppendLine("             0.9900             1.01         1438.351")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("    7 statistics were added as attributes to data set   173:")
                    lRept.AppendLine()
                    lRept.AppendLine("          MEANVL STDDEV SKEWCF NUMZRO NONZRO LDIST ")
                    lRept.AppendLine("          L04003")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("                Log-Pearson Type III Statistics")
                    lRept.AppendLine("                           SWSTAT 4.1 ")
                    lRept.AppendLine("                  (based on USGS Program A193)")
                    lRept.AppendLine()
                    lRept.AppendLine("  Notice -- Use of Log-Pearson Type III or Pearson-Type III")
                    lRept.AppendLine("            distributions are for preliminary computations.")
                    lRept.AppendLine("            User is responsible for assessment and ")
                    lRept.AppendLine("            interpretation.")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("               11517500 Shasta River near Yreka, CA.                           ")
                    lRept.AppendLine("                    June 15 - start of season")
                    lRept.AppendLine("               September 15 - end of season")
                    lRept.AppendLine("                1960 - 1982 - time period")
                    lRept.AppendLine("                  4-day low - parameter")
                    lRept.AppendLine("                         23 - non-zero values")
                    lRept.AppendLine("                          0 - zero values")
                    lRept.AppendLine("                          0 - negative values (ignored)")
                    lRept.AppendLine()
                    lRept.AppendLine("            6.025       9.875      11.750      36.500      11.125")
                    lRept.AppendLine("           33.250       9.350      15.500       6.775      21.250")
                    lRept.AppendLine("           13.250      23.000      17.000      10.550      42.500")
                    lRept.AppendLine("           38.000      12.250       7.175      29.000      11.300")
                    lRept.AppendLine("           12.500       3.400      32.750")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("  The following 7 statistics are based on non-zero values:")
                    lRept.AppendLine()
                    lRept.AppendLine("  Mean (logs)                                  1.167")
                    lRept.AppendLine("  Variance (logs)                              0.084")
                    lRept.AppendLine("  Standard Deviation (logs)                    0.290")
                    lRept.AppendLine("  Skewness (logs)                             -0.101")
                    lRept.AppendLine("  Standard Error of Skewness (logs)            0.481")
                    lRept.AppendLine("  Serial Correlation Coefficient (logs)       -0.215")
                    lRept.AppendLine("  Coefficient of Variation (logs)              0.248")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("       Non-exceedance     Recurrence        Parameter")
                    lRept.AppendLine("        Probability        Interval           Value  ")
                    lRept.AppendLine("        -----------       ----------        ---------")
                    lRept.AppendLine("             0.0100           100.00            2.965")
                    lRept.AppendLine("             0.0200            50.00            3.605")
                    lRept.AppendLine("             0.0500            20.00            4.816")
                    lRept.AppendLine("             0.1000            10.00            6.209")
                    lRept.AppendLine("             0.2000             5.00            8.413")
                    lRept.AppendLine("             0.3333             3.00           11.100")
                    lRept.AppendLine("             0.5000             2.00           14.858")
                    lRept.AppendLine("             0.8000             1.25           25.829")
                    lRept.AppendLine("             0.9000             1.11           34.269")
                    lRept.AppendLine("             0.9600             1.04           46.112")
                    lRept.AppendLine("             0.9800             1.02           55.716")
                    lRept.AppendLine("             0.9900             1.01           65.938")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("    7 statistics were added as attributes to data set   163:")
                    lRept.AppendLine()
                    lRept.AppendLine("          MEANND SDND   SKWND  NUMZRO NONZRO LDIST ")
                    lRept.AppendLine("          L04003")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("                Log-Pearson Type III Statistics")
                    lRept.AppendLine("                           SWSTAT 4.1 ")
                    lRept.AppendLine("                  (based on USGS Program A193)")
                    lRept.AppendLine()
                    lRept.AppendLine("  Notice -- Use of Log-Pearson Type III or Pearson-Type III")
                    lRept.AppendLine("            distributions are for preliminary computations.")
                    lRept.AppendLine("            User is responsible for assessment and ")
                    lRept.AppendLine("            interpretation.")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("               11516530 Klamath River below Iron Gate Dam, CA.                 ")
                    lRept.AppendLine("                    June 15 - start of season")
                    lRept.AppendLine("               September 15 - end of season")
                    lRept.AppendLine("                1960 - 1982 - time period")
                    lRept.AppendLine("                  4-day low - parameter")
                    lRept.AppendLine("                         22 - non-zero values")
                    lRept.AppendLine("                          0 - zero values")
                    lRept.AppendLine("                          1 - negative values (ignored)")
                    lRept.AppendLine()
                    lRept.AppendLine("          718.250     669.750     714.000     775.000     708.000")
                    lRept.AppendLine("          662.000     695.750     695.750     690.500     699.000")
                    lRept.AppendLine("          712.750     693.000     666.250     725.000     708.750")
                    lRept.AppendLine("          711.500     707.250     727.000     697.000     730.750")
                    lRept.AppendLine("          726.000     709.250")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("  The following 7 statistics are based on non-zero values:")
                    lRept.AppendLine()
                    lRept.AppendLine("  Mean (logs)                                  2.849")
                    lRept.AppendLine("  Variance (logs)                              0.000")
                    lRept.AppendLine("  Standard Deviation (logs)                    0.015")
                    lRept.AppendLine("  Skewness (logs)                              0.368")
                    lRept.AppendLine("  Standard Error of Skewness (logs)            0.491")
                    lRept.AppendLine("  Serial Correlation Coefficient (logs)        0.045")
                    lRept.AppendLine("  Coefficient of Variation (logs)              0.005")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("       Non-exceedance     Recurrence        Parameter")
                    lRept.AppendLine("        Probability        Interval           Value  ")
                    lRept.AppendLine("        -----------       ----------        ---------")
                    lRept.AppendLine("             0.0100           100.00          658.118")
                    lRept.AppendLine("             0.0200            50.00          662.680")
                    lRept.AppendLine("             0.0500            20.00          669.934")
                    lRept.AppendLine("             0.1000            10.00          676.811")
                    lRept.AppendLine("             0.2000             5.00          685.709")
                    lRept.AppendLine("             0.3333             3.00          694.562")
                    lRept.AppendLine("             0.5000             2.00          704.600")
                    lRept.AppendLine("             0.8000             1.25          726.168")
                    lRept.AppendLine("             0.9000             1.11          738.606")
                    lRept.AppendLine("             0.9600             1.04          752.818")
                    lRept.AppendLine("             0.9800             1.02          762.546")
                    lRept.AppendLine("             0.9900             1.01          771.680")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("    7 statistics were added as attributes to data set   168:")
                    lRept.AppendLine()
                    lRept.AppendLine("          MEANND SDND   SKWND  NUMZRO NONZRO LDIST ")
                    lRept.AppendLine("          L04003")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("                Log-Pearson Type III Statistics")
                    lRept.AppendLine("                           SWSTAT 4.1 ")
                    lRept.AppendLine("                  (based on USGS Program A193)")
                    lRept.AppendLine()
                    lRept.AppendLine("  Notice -- Use of Log-Pearson Type III or Pearson-Type III")
                    lRept.AppendLine("            distributions are for preliminary computations.")
                    lRept.AppendLine("            User is responsible for assessment and ")
                    lRept.AppendLine("            interpretation.")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("               11520500 Klamath River near Seiad Valley, CA.                   ")
                    lRept.AppendLine("                    June 15 - start of season")
                    lRept.AppendLine("               September 15 - end of season")
                    lRept.AppendLine("                1960 - 1982 - time period")
                    lRept.AppendLine("                  4-day low - parameter")
                    lRept.AppendLine("                         23 - non-zero values")
                    lRept.AppendLine("                          0 - zero values")
                    lRept.AppendLine("                          0 - negative values (ignored)")
                    lRept.AppendLine()
                    lRept.AppendLine("         1060.000    1026.250    1032.750     994.500    1032.500")
                    lRept.AppendLine("         1030.000     951.750    1137.500     910.500    1105.000")
                    lRept.AppendLine("          970.000    1335.000    1027.500     810.750    1322.500")
                    lRept.AppendLine("         1310.000     916.250     792.500    1077.500     933.500")
                    lRept.AppendLine("          993.250     810.250    1142.500")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("  The following 7 statistics are based on non-zero values:")
                    lRept.AppendLine()
                    lRept.AppendLine("  Mean (logs)                                  3.009")
                    lRept.AppendLine("  Variance (logs)                              0.004")
                    lRept.AppendLine("  Standard Deviation (logs)                    0.062")
                    lRept.AppendLine("  Skewness (logs)                              0.174")
                    lRept.AppendLine("  Standard Error of Skewness (logs)            0.481")
                    lRept.AppendLine("  Serial Correlation Coefficient (logs)       -0.176")
                    lRept.AppendLine("  Coefficient of Variation (logs)              0.021")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("       Non-exceedance     Recurrence        Parameter")
                    lRept.AppendLine("        Probability        Interval           Value  ")
                    lRept.AppendLine("        -----------       ----------        ---------")
                    lRept.AppendLine("             0.0100           100.00          746.119")
                    lRept.AppendLine("             0.0200            50.00          771.978")
                    lRept.AppendLine("             0.0500            20.00          813.348")
                    lRept.AppendLine("             0.1000            10.00          852.910")
                    lRept.AppendLine("             0.2000             5.00          904.684")
                    lRept.AppendLine("             0.3333             3.00          956.840")
                    lRept.AppendLine("             0.5000             2.00         1017.109")
                    lRept.AppendLine("             0.8000             1.25         1150.205")
                    lRept.AppendLine("             0.9000             1.11         1229.485")
                    lRept.AppendLine("             0.9600             1.04         1322.456")
                    lRept.AppendLine("             0.9800             1.02         1387.614")
                    lRept.AppendLine("             0.9900             1.01         1449.943")
                    lRept.AppendLine()
                    lRept.AppendLine()
                    lRept.AppendLine("    7 statistics were added as attributes to data set   173:")
                    lRept.AppendLine()
                    lRept.AppendLine("          MEANND SDND   SKWND  NUMZRO NONZRO LDIST ")
                    lRept.AppendLine("          L04003")
                    lRept.AppendLine()
                    lRept.AppendLine(vbFormFeed)
                Next
            Next
        Next
        Return lRept.ToString
    End Function

End Class
