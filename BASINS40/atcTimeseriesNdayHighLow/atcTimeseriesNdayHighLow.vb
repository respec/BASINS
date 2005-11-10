Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Public Class atcTimeseriesNdayHighLow
  Inherits atcDataSource
  Private pAvailableOperations As atcDataAttributes
  Private Const pName As String = "Timeseries::n-day high/low"

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
        AddOperation("1Hi100", "One day high 100-year return period", _
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

  Private Function AddOperation(ByVal aName As String, _
                                ByVal aDescription As String, _
                                ByVal aTypeString As String, _
                                ByVal ParamArray aArgs() As atcAttributeDefinition)
    Dim lResult As New atcAttributeDefinition
    With lResult
      .Name = aName
      .Description = aDescription
      .DefaultValue = ""
      .Editable = False
      .TypeString = aTypeString
      .Calculator = Me
      .Category = "nDay & Frequency"
    End With
    Dim lArguments As atcDataAttributes = New atcDataAttributes
    For Each lArg As atcAttributeDefinition In aArgs
      lArguments.SetValue(lArg, Nothing)
    Next
    pAvailableOperations.SetValue(lResult, Nothing, lArguments)

  End Function

  Private Function HighOrLowValue(ByVal aTS As atcTimeseries, _
                   ByVal aNDay As Integer, _
                   ByVal aHigh As Boolean) As Double

    If aNDay > 0 And aTS.numValues >= aNDay Then
      Dim lBestSoFar As Double
      Dim lTimeIndex As Integer = 1
      Dim lNumSummed As Integer = 1
      Dim lRunningSum As Double = 0
      Dim lCurrentValue As Double

      If aHigh Then
        lBestSoFar = Double.MinValue
      Else
        lBestSoFar = Double.MaxValue
      End If

      While lTimeIndex <= aTS.numValues
        lCurrentValue = aTS.Value(lTimeIndex)

        'Can't calculate high or low value if any values in the period are missing
        If Double.IsNaN(lCurrentValue) Then
          If aTS.ValueAttributesGetValue(lTimeIndex, "Inserted", False) Then
            lNumSummed = 1
            lRunningSum = 0
          Else
            Return Double.NaN
          End If
        Else
          lRunningSum += lCurrentValue
          If lNumSummed < aNDay Then
            lNumSummed += 1
          Else
            If aHigh Then
              If lRunningSum > lBestSoFar Then lBestSoFar = lRunningSum
            Else
              If lRunningSum < lBestSoFar Then lBestSoFar = lRunningSum
            End If
            lRunningSum -= aTS.Value(lTimeIndex - aNDay + 1)
          End If
        End If
        lTimeIndex += 1
      End While

      Return (lBestSoFar / aNDay)

    Else 'Cannot compute a value because fewer than aNDay values are present 
      Return Double.NaN
    End If
  End Function

  Private Function HighOrLowTimeseries(ByVal aTS As atcTimeseries, ByVal aNDay As Object, ByVal aHigh As Boolean) As atcDataGroup 'atcTimeseries
    Try
      Dim lNDay() As Double = Obj2Array(aNDay)

      Dim lTimeCode As Integer = 6 '4=day, 5=month, 6=year

      aTS.EnsureValuesRead()
      Dim sjday As Double = aTS.Dates.Value(0)
      Dim ejday As Double = aTS.Dates.Value(aTS.Dates.numValues)
      'Dim indexOld As Integer = 1
      Dim indexNew As Integer = 1
      Dim nYears As Integer = timdifJ(sjday, ejday, lTimeCode, 1)

      If TimAddJ(sjday, lTimeCode, 1, nYears) < ejday Then
        nYears += 1
      End If

      Dim newTsGroup As New atcDataGroup

      For Each lNDayNow As Double In lNDay
        Dim newValues(nYears) As Double
        Dim newDates(nYears) As Double
        newDates(0) = sjday

        Dim lsjday = sjday
        For indexNew = 1 To nYears
          Dim nextSJday As Double = TimAddJ(lsjday, lTimeCode, 1, 1)
          Dim oneYear As atcTimeseries = SubsetByDate(aTS, lsjday, nextSJday, Me)
          newDates(indexNew) = nextSJday
          newValues(indexNew) = HighOrLowValue(oneYear, CInt(lNDayNow), aHigh)
          lsjday = nextSJday
        Next

        Dim newTS As New atcTimeseries(Me)
        newTS.Values = newValues
        newTS.Dates = New atcTimeseries(Me)
        newTS.Dates.Values = newDates
        newTS.Attributes.SetValue("Parent Timeseries", aTS)
        Dim lDateNow As Date = Now
        newTS.Attributes.SetValue("Date Created", lDateNow)
        newTS.Attributes.SetValue("Date Modified", lDateNow)

        CopyBaseAttributes(aTS, newTS)

        newTS.Attributes.SetValue("Tu", 6)
        newTS.Attributes.SetValue("Ts", 1)
        newTS.Attributes.SetValue("HighFlag", aHigh)
        newTS.Attributes.SetValue("NDay", lNDayNow)

        Dim lDescription As String = lNDayNow & " day annual "  'TODO: fill in day and annual
        If aHigh Then
          lDescription &= "high values "
        Else
          lDescription &= "low values "
        End If
        newTS.Attributes.SetValue("Description", lDescription & aTS.Attributes.GetValue("Description"))
        newTS.Attributes.AddHistory(lDescription)
        newTsGroup.Add(newTS)
      Next

      Return newTsGroup

    Catch ex As Exception
      LogDbg(ex.ToString)
    End Try
  End Function

  Private Sub ComputeTau(ByRef aTimeseries As atcTimeseries, _
                         ByVal aNDay As Object, _
                         ByVal aHigh As Boolean, _
                         ByVal aAttributesStorage As atcDataAttributes)
    Dim lNdayTsGroup As atcDataGroup

    If aTimeseries.Attributes.GetValue("Tu", 1) <> 6 Then
      'calculate the n day annual timeseries
      lNdayTsGroup = HighOrLowTimeseries(aTimeseries, aNDay, aHigh)
    Else 'already an annual timeseries
      If (aHigh = aTimeseries.Attributes.GetValue("HighFlag") And _
         aNDay = aTimeseries.Attributes.GetValue("NDay")) Then
        lNdayTsGroup = New atcDataGroup(aTimeseries)
      End If
    End If

    For Each lNdayTs As atcTimeseries In lNdayTsGroup
      If Not lNdayTs Is Nothing Then
        Dim lNday As Integer = lNdayTs.Attributes.GetValue("NDay")
        Dim lTau As Double
        Dim lLevel As Double
        Dim lSlope As Double
        Dim lMsg As String = ""

        Try
          KendallTau(lNdayTs, lTau, lLevel, lSlope)
        Catch ex As Exception
          lMsg = "ComputeFreq:Exception:" & ex.ToString & ":" & lNdayTs.ToString
          LogDbg(lMsg)
          lTau = Double.NaN
          lLevel = Double.NaN
          lSlope = Double.NaN
        End Try

        Dim lS As String
        If aHigh Then
          lS = lNday & "Hi"
        Else
          lS = lNday & "Low"
        End If
        Dim lNewAttribute As New atcAttributeDefinition
        With lNewAttribute
          .Name = lS & "KT"
          .Description = lS & " Kendall Tau "
          .DefaultValue = ""
          .Editable = False
          .TypeString = "Double"
          .Calculator = Me
          .Category = "nDay & Frequency"
        End With
        Dim lKenTauValue As atcAttributeDefinition = lNewAttribute.Clone
        lKenTauValue.Description = lKenTauValue.Description & "Value"
        lKenTauValue.Name = lKenTauValue.Name & "Value"
        Dim lKenTauProbLevel As atcAttributeDefinition = lNewAttribute.Clone
        lKenTauProbLevel.Name = lKenTauProbLevel.Name & "ProbLevel"
        lKenTauProbLevel.Description = lKenTauProbLevel.Description & "Probability Level"
        Dim lKenTauSlope As atcAttributeDefinition = lNewAttribute.Clone
        lKenTauSlope.Name = lKenTauSlope.Name & "Slope"
        lKenTauSlope.Description = lKenTauSlope.Description & "Slope"

        Dim lArguments As New atcDataAttributes
        lArguments.SetValue("Nday", lNday)
        lArguments.SetValue("HighFlag", aHigh)

        aAttributesStorage.SetValue(lKenTauValue, lTau, lArguments)
        aAttributesStorage.SetValue(lKenTauProbLevel, lLevel, lArguments)
        aAttributesStorage.SetValue(lKenTauSlope, lSlope, lArguments)
      End If
    Next

  End Sub

  Private Sub ComputeFreq(ByRef aTimeseries As atcTimeseries, _
                          ByVal aNDay As Object, _
                          ByVal aHigh As Boolean, _
                          ByVal aRecurOrProb As Object, _
                          ByVal aLogFg As Boolean, _
                          ByVal aAttributesStorage As atcDataAttributes)

    Dim lNdayTsGroup As atcDataGroup
    Dim lTsMath As atcDataSource
    Dim lQ As Double
    Dim lMsg As String = ""

    Dim lRecurOrProb() As Double = Obj2Array(aRecurOrProb)

    If aTimeseries.Attributes.GetValue("Tu", 1) <> 6 Then
      'calculate the n day annual timeseries
      lNdayTsGroup = HighOrLowTimeseries(aTimeseries, aNDay, aHigh)
    Else 'already an annual timeseries
      If (aHigh = aTimeseries.Attributes.GetValue("HighFlag", True) And _
         aNDay = aTimeseries.Attributes.GetValue("NDay", 1)) Then
        lNdayTsGroup = New atcDataGroup(aTimeseries)
      End If
    End If

    For Each lNdayTs As atcTimeseries In lNdayTsGroup
      If Not lNdayTs Is Nothing Then
        If aLogFg Then 'calc log10 of n day annual series
          Dim lArgsMath As New atcDataAttributes
          lTsMath = New atcTimeseriesMath.atcTimeseriesMath
          lArgsMath.SetValue("timeseries", New atcDataGroup(lNdayTs))
          lTsMath.Open("log 10", lArgsMath)
          lNdayTs = lTsMath.DataSets(0)
        End If
        Dim lNday As Integer = lNdayTs.Attributes.GetValue("NDay")

        For Each lRecurOrProbNow As Double In lRecurOrProb
          Try
            lQ = PearsonType3(lNdayTs, lRecurOrProbNow, aHigh)
          Catch ex As Exception
            lMsg = "ComputeFreq:Exception:" & ex.ToString & ":"
            lQ = Double.NaN
          End Try

          If lQ = 0 Or Double.IsNaN(lQ) Then
            If lMsg.Length = 0 Then
              lMsg = "ComputeFreq:ZeroOrNan:" & lQ & ":"
              lQ = Double.NaN
            End If

            lMsg &= lNday & ":" & lRecurOrProbNow & ":" & aHigh & ":" & lNdayTs.Attributes.GetValue("Count")
            LogDbg(lMsg)
            lMsg = ""
          End If

          If aLogFg Then 'remove log10 transform 
            lQ = 10 ^ lQ
          End If

          Dim lS As String
          If lNday = 7 And lRecurOrProbNow = 10 And Not aHigh Then
            lS = lNday & "Q" & lRecurOrProbNow
          ElseIf aHigh Then
            lS = lNday & "Hi" & DoubleToString(lRecurOrProbNow, , "#0.####")
          Else
            lS = lNday & "Low" & DoubleToString(lRecurOrProbNow, , "#0.####")
          End If

          Dim lNewAttribute As New atcAttributeDefinition
          With lNewAttribute
            .Name = lS
            .Description = lS
            .DefaultValue = ""
            .Editable = False
            .TypeString = "Double"
            .Calculator = Me
            .Category = "nDay & Frequency"
          End With

          Dim lArguments As New atcDataAttributes
          lArguments.SetValue("Nday", lNday)
          lArguments.SetValue("Return Period", lRecurOrProbNow)

          aAttributesStorage.SetValue(lNewAttribute, lQ, lArguments)
        Next

        If aLogFg Then 'remove log10 transform timser
          DataManager.DataSources.Remove(lTsMath)
          lTsMath = Nothing
        End If

        If Not (aTimeseries Is lNdayTs) Then 'get rid of intermediate timeseries
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
    Dim ltsGroup As atcDataGroup
    Dim lTs As atcTimeseries
    Dim lTsB As atcTimeseries
    Dim lNDayTsGroup As atcDataGroup 'atcTimeseries
    Dim lLogFlg As Boolean
    Dim lOperationName As String = aOperationName.ToLower
    Dim lNDay As Object
    Dim lReturn As Object
    Dim lHigh As Boolean
    Dim lBoundaryMonth As Integer
    Dim lBoundaryDay As Integer

    If aArgs Is Nothing Then
      'TODO: need something like specify computation (atcTimseriesMath) here, defaults for now
      'ltsGroup = DataManager.UserSelectData("Select data to compute statistics for")
      lNDay = 1
      lReturn = 100
      lLogFlg = True
      lHigh = True
      lBoundaryMonth = 10
      lBoundaryDay = 1
    Else
      ltsGroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries"))
      lLogFlg = aArgs.GetValue("LogFlg", True)
      lNDay = aArgs.GetValue("NDay")
      lReturn = aArgs.GetValue("Return Period")
      lHigh = aArgs.GetValue("HighFlag", True)
    End If

    Select Case lOperationName
      Case "7q10"
        lNDay = 7
        lReturn = 10
        lOperationName = "n-day low value"
      Case "1hi100"
        lNDay = 1
        lReturn = 100
        lOperationName = "n-day high value"
    End Select

    Select Case lOperationName
      Case "n-day low value", "n-day low timeseries"
        lHigh = False
      Case "n-day high value", "n-day high timeseries"
        lHigh = True
    End Select

    If lHigh Then
      lBoundaryMonth = 10
    Else
      lBoundaryMonth = 4
    End If
    If Not aArgs Is Nothing Then
      'allow override of default boundary month/day
      lBoundaryMonth = aArgs.GetValue("Boundary Month", lBoundaryMonth)
      lBoundaryDay = aArgs.GetValue("Boundary Day", 1)
    End If

    If ltsGroup Is Nothing Then
      ltsGroup = DataManager.UserSelectData("Select data to compute statistics for")
    End If

    For Each lTs In ltsGroup
      lTsB = SubsetByDateBoundary(lTs, lBoundaryMonth, lBoundaryDay, Nothing)
      Select Case lOperationName
        Case "n-day low value", "n-day high value"
          ComputeFreq(lTsB, lNDay, lHigh, lReturn, lLogFlg, lTs.Attributes)
        Case "n-day low timeseries", "n-day high timeseries"
          lNDayTsGroup = HighOrLowTimeseries(lTsB, lNDay, lHigh)
          Me.DataSets.AddRange(lNDayTsGroup)
        Case "kendall tau"
          ComputeTau(lTsB, lNDay, lHigh, lTs.Attributes)
      End Select
    Next

    If Me.DataSets.Count > 0 Then
      Return True 'todo: error checks
    Else
      Return False 'no datasets added, not a data source
    End If
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    For Each lOperation As atcDefinedValue In AvailableOperations
      atcDataAttributes.AddDefinition(lOperation.Definition)
    Next
  End Sub

End Class
