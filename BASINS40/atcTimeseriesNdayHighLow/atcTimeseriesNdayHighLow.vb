Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Windows.Forms

Public Class atcTimeseriesNdayHighLow
    Inherits atcData.atcTimeseriesSource
    Private pAvailableOperations As atcDataAttributes
    Private Const pName As String = "Timeseries::n-day high/low"

    Private Shared KENTAUattribute As atcAttributeDefinition
    Private Shared KENPLVattribute As atcAttributeDefinition
    Private Shared KENSLPLattribute As atcAttributeDefinition

    Public Overrides ReadOnly Property Name() As String
        Get
            Return pName
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "Statistics"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "n-day high/low"
        End Get
    End Property

    'Opening creates new computed data rather than opening a file
    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    'Definitions of statistics supported by ComputeStatistics
    Public Overrides ReadOnly Property AvailableOperations() As atcDataAttributes
        Get
            If pAvailableOperations Is Nothing Then
                pAvailableOperations = New atcDataAttributes

                Dim defNdayTS As New atcAttributeDefinition
                With defNdayTS
                    .Calculator = Me
                    .Category = Me.Category
                    .CopiesInherit = False
                    .Description = "Timeseries of N-day values, one per year"
                    .Editable = False
                    .Name = "NDayTimeseries"
                    .TypeString = "atcTimeseries"                    
                End With
                atcDataAttributes.AddDefinition(defNdayTS)

                Dim defDays As New atcAttributeDefinition
                With defDays
                    .Name = "NDay"
                    .Description = "Number of days"
                    .DefaultValue = New Double() {1, 2, 3, 7, 10, 30, 60, 90, 183, 365}
                    .Editable = True
                    .TypeString = "Double"
                End With

                Dim defReturnPeriod As New atcAttributeDefinition
                With defReturnPeriod
                    .Name = "Return Period"
                    .Description = "Number of years"
                    .DefaultValue = New Double() {1 / 0.9999, 1 / 0.9995, 1 / 0.999, 1 / 0.998, 1 / 0.995, _
                                                  1 / 0.99, 1 / 0.98, 1 / 0.975, 1 / 0.96, 1 / 0.95, 1 / 0.9, _
                                                  1.25, 1.5, 2, 3, 3.333, 5, 10, 20, 25, 40, 50, 100, _
                                                  200, 500, 1000, 2000, 10000}
                    .Editable = True
                    .TypeString = "Double"
                End With

                Dim defTimeSeriesOne As New atcAttributeDefinition
                With defTimeSeriesOne
                    .Name = "Timeseries"
                    .Description = "One time series"
                    .Editable = True
                    .TypeString = "atcTimeseries"
                End With

                Dim defHigh As New atcAttributeDefinition
                With defHigh
                    .Name = "HighFlag"
                    .Description = "High Flag"
                    .Editable = True
                    .TypeString = "Boolean"
                End With

                AddOperation("7Q10", "Seven day low flow 10-year return period", _
                             "Double", defTimeSeriesOne)
                AddOperation("1High100", "One day high 100-year return period", _
                             "Double", defTimeSeriesOne)

                AddOperation("n-day low timeseries", "n-day low value annual timeseries", _
                             "atcTimeseries", defTimeSeriesOne, defDays, defReturnPeriod)
                AddOperation("n-day high timeseries", "n-day high value annual timeseries", _
                             "atcTimeseries", defTimeSeriesOne, defDays, defReturnPeriod)

                AddOperation("n-day low value", "n-day low value for a return period", _
                             "Double", defTimeSeriesOne, defDays, defReturnPeriod)
                AddOperation("n-day high value", "n-day high value for a return period", _
                             "Double", defTimeSeriesOne, defDays, defReturnPeriod)

                AddOperation("Kendall Tau", "Kendall Tau Statistics", _
                             "Double", defTimeSeriesOne, defDays, defHigh)
            End If
            Return pAvailableOperations
        End Get
    End Property

    Private Sub AddOperation(ByVal aName As String, _
                                  ByVal aDescription As String, _
                                  ByVal aTypeString As String, _
                                  ByVal ParamArray aArgs() As atcAttributeDefinition)
        Dim lResult As New atcAttributeDefinition
        With lResult
            .Name = aName
            .Description = aDescription
            .DefaultValue = Nothing
            .Editable = False
            .TypeString = aTypeString
            .Calculator = Me
            .Category = "N-day and Frequency"
        End With
        Dim lArguments As atcDataAttributes = New atcDataAttributes
        For Each lArg As atcAttributeDefinition In aArgs
            lArguments.SetValue(lArg, Nothing)
        Next
        pAvailableOperations.SetValue(lResult, Nothing, lArguments)

    End Sub

    Private Function HighOrLowValue(ByVal aTS As atcTimeseries, _
                     ByVal aNDay As Integer, _
                     ByVal aHigh As Boolean, _
                     ByRef aDate As Double) As Double

        If aNDay <= 0 Then
            Return Nothing
        ElseIf aTS.numValues < aNDay Then 'Cannot compute a value because fewer than aNDay values are present 
            'Throw New ApplicationException("Only " & aTS.numValues & " values available, needed " & aNDay)
            Logger.Dbg("Only " & aTS.numValues & " values available, needed " & aNDay)
            Return GetNaN()
        Else
            Dim lBestSoFar As Double
            Dim lTimeIndex As Integer = 1
            Dim lNumSummed As Integer = 1
            Dim lRunningSum As Double = 0
            Dim lCurrentValue As Double

            If aHigh Then
                lBestSoFar = GetMinValue()
            Else
                lBestSoFar = GetMaxValue()
            End If

            While lTimeIndex <= aTS.numValues
                lCurrentValue = aTS.Value(lTimeIndex)

                'Can't calculate high or low value if any values in the period are missing
                If Double.IsNaN(lCurrentValue) Then
                    If aTS.ValueAttributesGetValue(lTimeIndex, "Inserted", False) Then
                        lNumSummed = 1
                        lRunningSum = 0
                    Else
                        Throw New ApplicationException("Missing Value at " & DumpDate(aTS.Dates.Value(lTimeIndex)))
                    End If
                Else
                    lRunningSum += lCurrentValue
                    If lNumSummed < aNDay Then
                        lNumSummed += 1
                    Else
                        If (aHigh AndAlso lRunningSum > lBestSoFar) OrElse _
                          ((Not aHigh) AndAlso lRunningSum < lBestSoFar) Then
                            lBestSoFar = lRunningSum
                            aDate = aTS.Dates.Value(lTimeIndex)
                        End If
                        lRunningSum -= aTS.Value(lTimeIndex - aNDay + 1)
                    End If
                End If
                lTimeIndex += 1
            End While
            Return (lBestSoFar / aNDay)
        End If
    End Function

    Private Function HighOrLowTimeseries(ByVal aTimeseries As atcTimeseries, _
                                         ByVal aNDay As Object, _
                                         ByVal aHigh As Boolean, _
                                         ByVal aAttributesStorage As atcDataAttributes, _
                                Optional ByVal aEndMonth As Integer = 0, _
                                Optional ByVal aEndDay As Integer = 0) As atcTimeseriesGroup 'atcTimeseries
        Dim lNDay() As Double = Obj2Array(aNDay)
        Dim newTsGroup As New atcTimeseriesGroup
        Try
            aTimeseries.EnsureValuesRead()
            Dim sjday As Double = aTimeseries.Dates.Value(0)
            Dim ejday As Double = aTimeseries.Dates.Value(aTimeseries.Dates.numValues)
            'Dim indexOld As Integer = 1
            Dim indexNew As Integer = 1
            Dim lTimeCode As Integer = atcTimeUnit.TUYear
            Dim nYears As Integer = timdifJ(sjday, ejday, lTimeCode, 1)
            Dim lHaveEndMonthDay As Boolean = (aEndMonth > 0 AndAlso aEndDay > 0)
            Dim lCurrentYear As atcTimeseries
            Dim lSJday As Double = sjday
            Dim lNextSJday As Double
            Dim lEndJday As Double
            Dim lNaN As Double = GetNaN()

            If TimAddJ(sjday, lTimeCode, 1, nYears) < ejday Then
                nYears += 1
            End If

            For Each lNDayNow As Double In lNDay
                If lNDayNow > 0 Then
                    If aTimeseries.Attributes.GetValue("Tu", atcTimeUnit.TUDay) = atcTimeUnit.TUYear Then
                        'See if existing annual timeseries is the one we want
                        Dim lIsHigh As Boolean
                        If aTimeseries.Attributes.ContainsAttribute("HighFlag") Then
                            lIsHigh = aTimeseries.Attributes.GetValue("HighFlag")
                        ElseIf aTimeseries.Attributes.ContainsAttribute("TSTYPE") Then
                            lIsHigh = aTimeseries.Attributes.GetValue("TSTYPE").ToString.ToUpper.StartsWith("H")
                            aTimeseries.Attributes.SetValue("HighFlag", lIsHigh)
                        ElseIf aTimeseries.Attributes.ContainsAttribute("Constituent") Then
                            lIsHigh = aTimeseries.Attributes.GetValue("Constituent").ToString.ToUpper.StartsWith("H")
                            aTimeseries.Attributes.SetValue("HighFlag", lIsHigh)
                        Else
                            lIsHigh = False
                            aTimeseries.Attributes.SetValue("HighFlag", lIsHigh)
                        End If

                        Dim lExistingNday As Double
                        If aTimeseries.Attributes.ContainsAttribute("NDay") AndAlso aTimeseries.Attributes.GetValue("NDay").GetType.Name = "Double" Then
                            lExistingNday = aTimeseries.Attributes.GetValue("NDay")
                        ElseIf aTimeseries.Attributes.ContainsAttribute("TSTYPE") Then
                            If Double.TryParse(aTimeseries.Attributes.GetValue("TSTYPE").ToString.Substring(1), lExistingNday) Then
                                aTimeseries.Attributes.SetValue("NDay", lExistingNday)
                            End If
                        ElseIf aTimeseries.Attributes.ContainsAttribute("Constituent") Then
                            If Double.TryParse(aTimeseries.Attributes.GetValue("Constituent").ToString.Substring(1), lExistingNday) Then
                                aTimeseries.Attributes.SetValue("NDay", lExistingNday)
                            End If
                        Else
                            lExistingNday = 0
                            aTimeseries.Attributes.SetValue("NDay", lExistingNday)
                        End If

                        If aHigh = lIsHigh AndAlso Math.Abs(lNDayNow - lExistingNday) < 1.0E-29 Then
                            'already have the right annual timeseries
                            newTsGroup.Add(aTimeseries)
                        Else
                            Logger.Dbg("Cannot compute " & lNDayNow & "-day " & " High=" & aHigh & " from annual " & lExistingNday & "-day " & " High=" & lIsHigh)
                        End If

                    Else 'calculate the n-day annual timeseries
                        Dim newTS As New atcTimeseries(Me)
                        newTS.Dates = New atcTimeseries(Me)
                        newTS.numValues = nYears

                        newTS.Dates.Value(0) = sjday

                        lSJday = sjday
                        For indexNew = 1 To nYears
                            lNextSJday = TimAddJ(lSJday, lTimeCode, 1, 1)
                            If lHaveEndMonthDay Then
                                Dim lEndSeason(5) As Integer
                                J2Date(lNextSJday, lEndSeason)
                                If aEndMonth > lEndSeason(1) OrElse (aEndMonth = lEndSeason(1) AndAlso aEndDay > lEndSeason(2)) Then
                                    'Season ends in previous calendar year
                                    lEndSeason(0) -= 1
                                End If
                                lEndSeason(1) = aEndMonth
                                lEndSeason(2) = aEndDay
                                lEndSeason(3) = 24 'end of day
                                lEndJday = Date2J(lEndSeason)
                            Else
                                lEndJday = lNextSJday
                            End If
                            lCurrentYear = SubsetByDate(aTimeseries, lSJday, lEndJday, Me)
                            newTS.Dates.Value(indexNew) = lEndJday
                            Dim lDate As Double = GetNaN()
                            Try
                                newTS.Value(indexNew) = HighOrLowValue(lCurrentYear, CInt(lNDayNow), aHigh, lDate)
                                newTS.ValueAttributes(indexNew).Add("Date", lDate)
                            Catch e As Exception
                                newTS.Value(indexNew) = lNaN
                                newTS.ValueAttributes(indexNew) = New atcDataAttributes()
                                newTS.ValueAttributes(indexNew).SetValue("Explanation", e.Message)
                            End Try
                            lSJday = lNextSJday
                        Next

                        Dim lDescription As String = lNDayNow & " day annual "  'TODO: fill in day and annual
                        If aHigh Then
                            lDescription &= "high values "
                        Else
                            lDescription &= "low values "
                        End If

                        Dim lDateNow As Date = Now
                        CopyBaseAttributes(aTimeseries, newTS)

                        With newTS.Attributes
                            .SetValue("Date Created", lDateNow)
                            .SetValue("Date Modified", lDateNow)
                            .SetValue("Parent Timeseries", aTimeseries)
                            .SetValue("Tu", atcTimeUnit.TUYear)
                            .SetValue("Ts", 1)
                            .SetValue("HighFlag", aHigh)
                            .SetValue("NDay", lNDayNow)

                            Dim ltstype As String
                            If aHigh Then ltstype = "H" Else ltstype = "L"
                            ltstype &= Format(lNDayNow, "000")
                            .SetValue("tstype", ltstype)
                            .SetValue("constituent", ltstype)
                            .SetValue("Description", lDescription & aTimeseries.Attributes.GetValue("Description"))
                            .AddHistory(lDescription)
                            .SetValue("LDIST", "LP")
                        End With

                        Dim lKenTauAttributes As New atcDataAttributes
                        ComputeTau(newTS, lNDayNow, aHigh, lKenTauAttributes)
                        For Each lAttribute As atcDefinedValue In lKenTauAttributes
                            aAttributesStorage.SetValue(lAttribute.Definition, lAttribute.Value, lAttribute.Arguments)
                            newTS.Attributes.SetValue(lAttribute.Definition, lAttribute.Value, lAttribute.Arguments)
                        Next
                        newTsGroup.Add(newTS)
                    End If
                Else
                    Logger.Dbg("Skip:NDay=" & lNDayNow)
                End If
            Next
        Catch ex As Exception
            Logger.Dbg(ex.ToString)
        End Try
        Return newTsGroup
    End Function

    Public Sub ComputeTau(ByRef aTimeseries As atcTimeseries, _
                           ByVal aNDay As Double, _
                           ByVal aHigh As Boolean, _
                           ByVal aAttributesStorage As atcDataAttributes, _
                  Optional ByVal aEndMonth As Integer = 0, _
                  Optional ByVal aEndDay As Integer = 0)
        Dim lNdayTsGroup As atcTimeseriesGroup = HighOrLowTimeseries(aTimeseries, aNDay, aHigh, aAttributesStorage, aEndMonth, aEndDay)

        If Not lNdayTsGroup Is Nothing Then
            For Each lNdayTs As atcTimeseries In lNdayTsGroup
                If Not lNdayTs Is Nothing Then
                    Dim lNday As Integer = lNdayTs.Attributes.GetValue("NDay")
                    Dim lTau As Double
                    Dim lLevel As Double
                    Dim lSlope As Double

                    Try
                        KendallTau(lNdayTs, lTau, lLevel, lSlope)
                    Catch ex As Exception
                        Logger.Dbg("ComputeFreq:Exception:" & ex.ToString & ":" & lNdayTs.ToString)
                        lTau = GetNaN()
                        lLevel = GetNaN()
                        lSlope = GetNaN()
                    End Try

                    Dim lArguments As New atcDataAttributes
                    lArguments.SetValue("Nday", lNday)
                    lArguments.SetValue("HighFlag", aHigh)

                    SetKenTauAttr(aAttributesStorage, aNDay, aHigh, "Value", "Value", lTau, lArguments)
                    SetKenTauAttr(aAttributesStorage, aNDay, aHigh, "ProbLevel", "Probability Level", lLevel, lArguments)
                    SetKenTauAttr(aAttributesStorage, aNDay, aHigh, "Slope", "Slope", lSlope, lArguments)

                    'Store these attributes in WDM-friendly locations in the annual timeseries
                    If KENTAUattribute Is Nothing Then
                        KENTAUattribute = New atcAttributeDefinition
                        KENTAUattribute.Name = "KENTAU"
                        KENTAUattribute.TypeString = "Double"
                        KENTAUattribute.ID = 283
                        KENTAUattribute.CopiesInherit = False
                    End If
                    If KENPLVattribute Is Nothing Then
                        KENPLVattribute = New atcAttributeDefinition
                        KENPLVattribute.Name = "KENPLV"
                        KENPLVattribute.TypeString = "Double"
                        KENPLVattribute.ID = 284
                        KENPLVattribute.CopiesInherit = False
                    End If
                    If KENSLPLattribute Is Nothing Then
                        KENSLPLattribute = New atcAttributeDefinition
                        KENSLPLattribute.Name = "KENSLPL"
                        KENSLPLattribute.TypeString = "Double"
                        KENSLPLattribute.ID = 285
                        KENSLPLattribute.CopiesInherit = False
                    End If
                    lNdayTs.Attributes.SetValue(KENTAUattribute, lTau, lArguments)
                    lNdayTs.Attributes.SetValue(KENPLVattribute, lLevel, lArguments)
                    lNdayTs.Attributes.SetValue(KENSLPLattribute, lSlope, lArguments)
                End If
            Next
        End If
    End Sub

    Private Sub SetKenTauAttr(ByVal aAttributesStorage As atcDataAttributes, _
                              ByVal aNDay As Double, _
                              ByVal aHigh As Boolean, _
                              ByVal aSuffixShort As String, _
                              ByVal aSuffixLong As String, _
                              ByVal aValue As Double, _
                              ByVal aArguments As atcDataAttributes)
        Dim lS As String = aNDay & "-Day"
        If aHigh Then
            lS &= "High"
        Else
            lS &= "Low"
        End If
        Dim lName As String = "KenTau" & lS & aSuffixShort
        Dim lAttrDef As atcAttributeDefinition
        Dim lIndex As Integer = atcDataAttributes.AllDefinitions.Keys.IndexOf(lName)
        If lIndex >= 0 Then
            lAttrDef = atcDataAttributes.AllDefinitions.ItemByIndex(lIndex)
        Else
            lAttrDef = New atcAttributeDefinition
            With lAttrDef
                .Name = lName
                .Description = "Kendall Tau " & lS & " " & aSuffixLong
                .DefaultValue = ""
                .Editable = False
                .TypeString = "Double"
                .Calculator = Me
                .Category = "N-Day and Frequency"
                .CopiesInherit = False
            End With
        End If
        aAttributesStorage.SetValue(lAttrDef, aValue, aArguments)
    End Sub

    Public Sub ComputeFreq(ByRef aTimeseries As atcTimeseries, _
                           ByVal aNDay As Object, _
                           ByVal aHigh As Boolean, _
                           ByVal aRecurOrProb As Object, _
                           ByVal aLogFg As Boolean, _
                           ByVal aAttributesStorage As atcDataAttributes, _
                  Optional ByRef aNdayTsGroup As atcTimeseriesGroup = Nothing, _
                  Optional ByVal aEndMonth As Integer = 0, _
                  Optional ByVal aEndDay As Integer = 0)

        Dim lTsMath As atcTimeseriesSource = Nothing
        Dim lQ As Double
        Dim lMsg As String = ""
        Dim lNaN As Double = GetNaN()

        Dim lRecurOrProbs() As Double = Obj2Array(aRecurOrProb)

        If aNdayTsGroup Is Nothing Then
            'calculate the n day annual timeseries
            aNdayTsGroup = HighOrLowTimeseries(aTimeseries, aNDay, aHigh, aAttributesStorage, aEndMonth, aEndDay)
        End If

        For Each lNdayTs As atcTimeseries In aNdayTsGroup
            If Not lNdayTs Is Nothing Then
                Dim lNumZero As Integer = 0
                Dim lNonLogTs As atcTimeseries = lNdayTs
                For lIndex As Integer = 1 To lNonLogTs.numValues
                    If Math.Abs(lNonLogTs.Value(lIndex)) < 1.0E-30 Then ' See if an NDay annual value is zero
                        lNumZero += 1
                        lNonLogTs.Value(lIndex) = lNaN
                        lNonLogTs.ValueAttributes(lIndex).SetValue("Was Zero", True)
                    End If
                Next
                lNonLogTs.Attributes.DiscardCalculated()

                If aLogFg Then 'calc log10 of n day annual series
                    Dim lArgsMath As New atcDataAttributes
                    lTsMath = New atcTimeseriesMath.atcTimeseriesMath
                    lArgsMath.SetValue("timeseries", New atcTimeseriesGroup(lNonLogTs))
                    lTsMath.Open("log 10", lArgsMath)
                    'Save non-log timeseries
                    lTsMath.DataSets(0).Attributes.SetValue("NDayTimeseries", lNonLogTs)
                    aAttributesStorage.SetValue("NonLogNDayTimeseries", lNonLogTs)
                    lNdayTs = lTsMath.DataSets(0)
                    'Set log-specific attributes
                    aAttributesStorage.SetValue("MEANDD", lNdayTs.Attributes.GetValue("Mean"))
                    aAttributesStorage.SetValue("SDND", lNdayTs.Attributes.GetValue("Standard Deviation"))
                    aAttributesStorage.SetValue("SKWND", lNdayTs.Attributes.GetValue("Skew"))
                    aAttributesStorage.SetValue("LDIST", "LP3")
                Else
                    aAttributesStorage.SetValue("LDIST", "LP")
                End If

                Try
                    PearsonType3(lNdayTs, lRecurOrProbs, aHigh, aLogFg, Me, aAttributesStorage, lNumZero)
                Catch ex As Exception
                    lMsg = "ComputeFreq:Exception:" & ex.ToString & ":"
                    lQ = lNaN
                End Try

                lTsMath = Nothing

                If lNumZero > 0 Then
                    For lIndex As Integer = 1 To lNonLogTs.numValues
                        If lNonLogTs.ValueAttributes(lIndex).GetValue("Was Zero", False) Then
                            lNonLogTs.Value(lIndex) = 0
                            lNonLogTs.ValueAttributes(lIndex).Clear()
                        End If
                    Next
                End If

                If Not ReferenceEquals(aTimeseries, lNdayTs) Then
                    'get rid of intermediate timeseries
                    lNdayTs = Nothing
                    Me.DataSets.Clear()
                End If
            End If
        Next
    End Sub

    Private Function Obj2Array(ByVal aObj As Object) As Array
        Dim lArray() As Double
        If IsNumeric(aObj) Then
            ReDim lArray(0)
            lArray(0) = aObj
        Else 'TODO: check to be sure array
            lArray = aObj
        End If
        Return lArray
    End Function

    'first element of aArgs is atcData object whose attribute(s) will be set to the result(s) of calculation(s)
    'remaining aArgs are expected to follow the args required for the specified operation
    Public Overrides Function Open(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
        Dim ltsGroup As atcTimeseriesGroup = Nothing
        Dim lTs As atcTimeseries
        Dim lTsB As atcTimeseries
        Dim lLogFlg As Boolean = True
        Dim lOperationName As String = aOperationName.ToLower
        Dim lNDay As Object = 1
        Dim lReturn As Object = 100
        Dim lHigh As Boolean = True
        Dim lBoundaryMonth As Integer = 10
        Dim lBoundaryDay As Integer = 1
        Dim lEndMonth As Integer = 0
        Dim lEndDay As Integer = 0
        Dim lFirstYear As Integer = 0
        Dim lLastYear As Integer = 0

        Select Case lOperationName
            Case "7q10"
                lNDay = 7
                lReturn = 10
                lOperationName = "n-day low value"
                'lLogFlg = False
            Case "1hi100", "1high100"
                lNDay = 1
                lReturn = 100
                lOperationName = "n-day high value"
            Case Else
                lNDay = StrFirstInt(lOperationName)
                Dim lPos As Integer = lOperationName.Length
                While IsNumeric(Mid(lOperationName, lPos, 1))
                    lPos -= 1
                End While
                If lPos < lOperationName.Length Then
                    lReturn = lOperationName.Substring(lPos)
                End If
                If lNDay > 0 AndAlso lReturn > 0 Then
                    If lOperationName.Contains("low") Then
                        lOperationName = "n-day low value"
                    Else
                        lOperationName = "n-day high value"
                    End If
                End If
        End Select

        'Change default from True to False if operation name contains "low"
        If lOperationName.Contains("low") Then lHigh = False

        If aArgs Is Nothing Then
            ltsGroup = atcDataManager.UserSelectData("Select data to compute statistics for")
            Dim lForm As New frmSpecifyYearsSeasons
            If lHigh Then
                lBoundaryMonth = 10
                If Not lForm.AskUser("High", ltsGroup, lBoundaryMonth, lBoundaryDay, lEndMonth, lEndDay, lFirstYear, lLastYear, lNDay) Then
                    Return False
                End If
            Else
                lBoundaryMonth = 4
                If Not lForm.AskUser("Low", ltsGroup, lBoundaryMonth, lBoundaryDay, lEndMonth, lEndDay, lFirstYear, lLastYear, lNDay) Then
                    Return False
                End If
            End If
        Else
            ltsGroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries"))
            lLogFlg = aArgs.GetValue("LogFlg", lLogFlg)
            lNDay = aArgs.GetValue("NDay", lNDay)
            lReturn = aArgs.GetValue("Return Period", lReturn)
            lHigh = aArgs.GetValue("HighFlag", lHigh)
            If aArgs.ContainsAttribute("BoundaryMonth") Then
                lBoundaryMonth = aArgs.GetValue("BoundaryMonth")
            ElseIf lHigh Then
                lBoundaryMonth = 10
            Else
                lBoundaryMonth = 4
            End If
            lBoundaryDay = aArgs.GetValue("BoundaryDay", lBoundaryDay)

            If aArgs.ContainsAttribute("EndMonth") Then lEndMonth = aArgs.GetValue("EndMonth")
            If aArgs.ContainsAttribute("EndDay") Then lEndDay = aArgs.GetValue("EndDay")
            If aArgs.ContainsAttribute("FirstYear") Then lFirstYear = aArgs.GetValue("FirstYear")
            If aArgs.ContainsAttribute("LastYear") Then lLastYear = aArgs.GetValue("LastYear")
        End If

        If ltsGroup Is Nothing Then
            ltsGroup = atcDataManager.UserSelectData("Select data to compute statistics for")
        End If

        For Each lTs In ltsGroup
            Dim lNDayTsGroup As atcTimeseriesGroup = Nothing 'atcTimeseries
            If lTs.Attributes.GetValue("Time Unit") = atcTimeUnit.TUYear Then
                lTsB = lTs
            Else
                lTsB = SubsetByDateBoundary(lTs, lBoundaryMonth, lBoundaryDay, Nothing, lFirstYear, lLastYear, lEndMonth, lEndDay)
            End If
            Select Case lOperationName
                Case "n-day low value", "n-day high value"
                    ComputeFreq(lTsB, lNDay, lHigh, lReturn, lLogFlg, lTs.Attributes, lNDayTsGroup, lEndMonth, lEndDay)
                    Me.DataSets.AddRange(lNDayTsGroup)
                Case "n-day low timeseries", "n-day high timeseries"
                    lNDayTsGroup = HighOrLowTimeseries(lTsB, lNDay, lHigh, lTs.Attributes, lEndMonth, lEndDay)
                    Me.DataSets.AddRange(lNDayTsGroup)
                Case "kendall tau"
                    ComputeTau(lTsB, lNDay, lHigh, lTs.Attributes, lEndMonth, lEndDay)
            End Select
        Next

        If Me.DataSets.Count > 0 Then
            Return True 'todo: error checks
        Else
            Return False 'no datasets added, not a data source
        End If
    End Function

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
        For Each lOperation As atcDefinedValue In AvailableOperations
            atcDataAttributes.AddDefinition(lOperation.Definition)
        Next
    End Sub

End Class
