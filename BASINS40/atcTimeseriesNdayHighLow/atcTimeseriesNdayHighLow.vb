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
          .DefaultValue = ""
          .Editable = True
          .TypeString = "Double"
        End With

        Dim defReturnPeriod As New atcAttributeDefinition
        With defReturnPeriod
          .Name = "Return Period"
          .Description = "Number of years"
          .DefaultValue = ""
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

  'Take care of missing values by setting to NaN before calling this function
  Private Function HighOrLowValue(ByVal aTS As atcTimeseries, ByVal aNumValues As Integer, ByVal aHigh As Boolean) As Double
    If aNumValues > 0 And aTS.numValues >= aNumValues Then
      Dim lBestSoFar As Double
      Dim lTimeIndex As Integer
      Dim lRunningSum As Double = 0
      Dim lCurrentValue As Double

      'Add up the first N values without checking for a limit
      For lTimeIndex = 1 To aNumValues - 1
        lCurrentValue = aTS.Value(lTimeIndex)

        'Can't calculate high or low value if any values in the period are missing
        If Double.IsNaN(lCurrentValue) Then
          Return Double.NaN
        End If
        lRunningSum += lCurrentValue
      Next

      If aHigh Then
        lBestSoFar = Double.MinValue
      Else
        lBestSoFar = Double.MaxValue
      End If

      While lTimeIndex <= aTS.numValues
        lCurrentValue = aTS.Value(lTimeIndex)

        'Can't calculate high or low value if any values in the period are missing
        If Double.IsNaN(lCurrentValue) Then
          Return Double.NaN
        End If

        lRunningSum += lCurrentValue

        If aHigh Then
          If lRunningSum > lBestSoFar Then lBestSoFar = lRunningSum
        Else
          If lRunningSum < lBestSoFar Then lBestSoFar = lRunningSum
        End If

        lTimeIndex += 1
        lRunningSum -= aTS.Value(lTimeIndex - aNumValues)
      End While
      Return (lBestSoFar / aNumValues)
    Else 'Cannot compute a value because fewer than aNumValues values are present 
      Return Double.NaN
    End If
  End Function

  Private Function HighOrLowTimeseries(ByVal aTS As atcTimeseries, ByVal aNumValues As Integer, ByVal aHigh As Boolean) As atcTimeseries
    Try
      Dim lTimeCode As Integer = 6 '4=day, 5=month, 6=year
      aTS.EnsureValuesRead()
      Dim sjday As Double = aTS.Dates.Value(0)
      Dim ejday As Double = aTS.Dates.Value(aTS.Dates.numValues)
      'Dim indexOld As Integer = 1
      Dim indexNew As Integer = 1
      Dim nYears As Integer = timdifJ(sjday, ejday, lTimeCode, 1)
      Dim newValues(nYears) As Double
      Dim newDates(nYears) As Double
      newDates(0) = sjday

      For indexNew = 1 To nYears
        Dim nextSJday As Double = TimAddJ(sjday, lTimeCode, 1, 1)
        Dim oneYear As atcTimeseries = SubsetByDate(aTS, sjday, nextSJday, Me)
        newDates(indexNew) = nextSJday
        newValues(indexNew) = HighOrLowValue(oneYear, aNumValues, aHigh)
        sjday = nextSJday
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
      newTS.Attributes.SetValue("NDay", aNumValues)

      Dim lDescription As String = aNumValues & " day annual "  'TODO: fill in day and annual
      If aHigh Then
        lDescription &= "high values "
      Else
        lDescription &= "low values "
      End If
      newTS.Attributes.SetValue("Description", lDescription & aTS.Attributes.GetValue("Description"))
      newTS.Attributes.AddHistory(lDescription)

      Return newTS

    Catch ex As Exception
      Stop
    End Try
  End Function

  Private Sub ComputeFreq(ByRef aTimeseries As atcTimeseries, _
                          ByVal aNDay As Integer, _
                          ByVal aHigh As Boolean, _
                          ByVal aRecurOrProb As Double, _
                          ByVal aLogFg As Boolean)

    Dim lNdayTs As atcTimeseries
    Dim lTsMath As atcDataSource

    If aTimeseries.Attributes.GetValue("Tu", 1) <> 6 Then
      'calculate the n day annual timeseries
      lNdayTs = HighOrLowTimeseries(aTimeseries, aNDay, aHigh)
    Else 'already an annual timeseries
      If (aHigh = aTimeseries.Attributes.GetValue("HighFlag") And _
         aNDay = aTimeseries.Attributes.GetValue("NDay")) Then
        lNdayTs = aTimeseries
      End If
    End If

    If Not lNdayTs Is Nothing Then
      If aLogFg Then 'calc log10 of n day annual series
        Dim lArgsMath As New atcDataAttributes
        lTsMath = New atcTimeseriesMath.atcTimeseriesMath
        lArgsMath.SetValue("timeseries", New atcDataGroup(lNdayTs))
        'DataManager.OpenDataSource(lTsMath, "log 10", lArgsMath)
        lTsMath.Open("log 10", lArgsMath)
        lNdayTs = lTsMath.DataSets(0)
      End If

      Dim lQ As Double = PearsonType3(lNdayTs, aRecurOrProb, aHigh)

      If aLogFg Then 'remove log10 transform 
        lQ = 10 ^ lQ
        DataManager.DataSources.Remove(lTsMath)
        lTsMath = Nothing
      End If

      Dim lS As String
      If aNDay = 7 And aRecurOrProb = 10 And Not aHigh Then
        lS = aNDay & "Q" & aRecurOrProb
      ElseIf aHigh Then
        lS = aNDay & "Hi" & aRecurOrProb
      Else
        lS = aNDay & "Low" & aRecurOrProb
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
      aTimeseries.Attributes.SetValue(lNewAttribute, lQ)

      If Not (aTimeseries Is lNdayTs) Then 'get rid of intermediate timeseries
        lNdayTs = Nothing
        Me.DataSets.Clear()
      End If
    End If

  End Sub

  'first element of aArgs is atcData object whose attribute(s) will be set to the result(s) of calculation(s)
  'remaining aArgs are expected to follow the args required for the specified operation
  Public Overrides Function Open(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
    Dim ltsGroup As atcDataGroup
    Dim lTs As atcTimeseries
    Dim lNDayTs As atcTimeseries
    Dim lNDay As Integer
    Dim lReturn As Double

    If aArgs Is Nothing Then
      'TODO: need something like specify computation (atcTimseriesMath) here
      ltsGroup = DataManager.UserSelectData("Select data to compute statistics for")
      lNDay = 1
      lReturn = 100
    Else
      ltsGroup = aArgs.GetValue("Timeseries")

      Dim lobjNDay As Object = aArgs.GetValue("NDay")
      If IsNumeric(lobjNDay) Then
        lNDay = lobjNDay
      End If

      Dim lobjReturn As Object = aArgs.GetValue("Return Period")
      If IsNumeric(lobjReturn) Then
        lReturn = lobjReturn
      End If
    End If

    If ltsGroup Is Nothing Then
      ltsGroup = DataManager.UserSelectData("Select data to compute statistics for")
    End If

    For Each lTs In ltsGroup
      Select Case aOperationName.ToLower
        Case "7q10" : ComputeFreq(lTs, 7, False, 10.0, True)
        Case "1hi100" : ComputeFreq(lTs, 1, True, 100.0, True)
        Case "n-day low value" : ComputeFreq(lTs, lNDay, False, lReturn, True)
        Case "n-day high value" : ComputeFreq(lTs, lNDay, True, lReturn, True)
        Case "n-day low timeseries"
          lNDayTs = HighOrLowTimeseries(lTs, lNDay, False)
          Me.DataSets.Add(lNDayTs)
        Case "n-day high timeseries"
          lNDayTs = HighOrLowTimeseries(lTs, lNDay, True)
          Me.DataSets.Add(lNDayTs)
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

  Public Overrides Function NewOne() As atcDataPlugin
    Return New atcTimeseriesNdayHighLow
  End Function

End Class
