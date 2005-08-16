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
          .Name = "Days"
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

        AddOperation("7Q10", "Seven day low flow 10-year return period", defTimeSeriesOne)

        'AddOperation("n-day low value", "n-day low value for a return period", _
        '             defCategory, defTimeSeriesOne, defDays, defReturnPeriod)

        'AddOperation("n-day high value", "n-day high value for a return period", _
        '             defCategory, defTimeSeriesOne, defDays, defReturnPeriod)

      End If
      Return pAvailableOperations
    End Get
  End Property

  Private Function AddOperation(ByVal aName As String, _
                                ByVal aDescription As String, _
                                ByVal ParamArray aArgs() As atcAttributeDefinition)
    Dim lResult As New atcAttributeDefinition
    With lResult
      .Name = aName
      .Description = aDescription
      .DefaultValue = ""
      .Editable = False
      .TypeString = "Double"
      .Calculator = Me
      .Category = Category
    End With
    Dim lArguments As atcDataAttributes = New atcDataAttributes
    For Each lArg As atcAttributeDefinition In aArgs
      lArguments.SetValue(lArg, Nothing)
    Next
    pAvailableOperations.SetValue(lResult, Nothing, lArguments)

  End Function

  'Take care of missing values before calling this function
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

      Return newTS

    Catch ex As Exception
      Stop
    End Try
  End Function

  Private Sub Compute7q10(ByVal aTimeseries As atcTimeseries)
    Dim lLowTS As atcTimeseries = HighOrLowTimeseries(aTimeseries, 7, False)
    Me.AddDataSet(lLowTS)
    If Not DataManager.DataSources.Contains(Me) Then DataManager.DataSources.Add(Me)
    'TODO: Compute 
  End Sub

  'first element of aArgs is atcData object whose attribute(s) will be set to the result(s) of calculation(s)
  'remaining aArgs are expected to follow the args required for the specified operation
  Public Overrides Function Open(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
    Dim ltsGroup As atcDataGroup
    If aArgs Is Nothing Then
      ltsGroup = DataManager.UserSelectData("Select data to compute statistics for")
      If aOperationName.ToLower = "7q10" Then
        For Each lts As atcTimeseries In ltsGroup
          Compute7q10(lts)
        Next
      End If
    Else
      If aOperationName.ToLower = "7q10" Then
        Compute7q10(aArgs.GetValue("Timeseries"))
      End If
    End If
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    For Each lOperation As atcDefinedValue In AvailableOperations
      atcDataAttributes.AddDefinition(lOperation.Definition)
    Next
  End Sub

End Class
