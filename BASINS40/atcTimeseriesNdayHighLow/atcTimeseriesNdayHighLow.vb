Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Public Class atcTimeseriesNdayHighLow
  Inherits atcDataSource
  Private pAvailableOperations As atcDataGroup
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
  Public Overrides ReadOnly Property AvailableOperations() As atcDataGroup
    Get
      If pAvailableOperations Is Nothing Then
        pAvailableOperations = New atcDataGroup

        Dim defCategory As New atcAttributeDefinition
        With defCategory
          .Name = "Category"
          .Description = ""
          .Editable = False
          .TypeString = "String"
        End With

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

        AddOperation("7Q10", "Seven day low flow 10-year return period", _
                     defCategory, defTimeSeriesOne)

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
                                ByVal aCategory As atcAttributeDefinition, _
                                ByVal ParamArray aArgs() As atcAttributeDefinition)
    Dim lResult As New atcAttributeDefinition
    With lResult
      .Name = aName
      .Description = aDescription
      .DefaultValue = ""
      .Editable = False
      .TypeString = "Double"
      .Calculator = Me
    End With
    Dim lArguments As atcDataAttributes = New atcDataAttributes
    For Each lArg As atcAttributeDefinition In aArgs
      lArguments.SetValue(lArg, Nothing)
    Next
    Dim lData As New atcDataSet
    lData.Attributes.SetValue(lResult, Nothing, lArguments)
    'lData.Attributes.SetValue(aCategory, Category)
    pAvailableOperations.Add(lResult.Name.ToLower, lData)

  End Function

  'Take care of missing values before calling this function
  Private Function HighOrLowValue(ByVal aTS As atcTimeseries, ByVal aNumValues As Integer, ByVal aHigh As Boolean) As Double
    If aTS.numValues >= aNumValues Then
      Dim lBestSoFar As Double
      Dim lTimeIndex As Integer
      Dim lRunningSum As Double = 0

      'Add up the first N values without checking for a limit
      For lTimeIndex = 1 To aNumValues - 1
        lRunningSum += aTS.Value(lTimeIndex)
      Next

      If aHigh Then
        lBestSoFar = Double.MinValue
      Else
        lBestSoFar = Double.MaxValue
      End If

      For lTimeIndex = aNumValues To aTS.numValues
        lRunningSum += aTS.Value(lTimeIndex)

        If aHigh Then
          If lRunningSum > lBestSoFar Then lBestSoFar = lRunningSum
        Else
          If lRunningSum < lBestSoFar Then lBestSoFar = lRunningSum
        End If

        lRunningSum -= aTS.Value(lTimeIndex - aNumValues)
        lTimeIndex += 1
      Next
      Return lBestSoFar
    Else 'Cannot compute a value because fewer than aNumValues values are present 
      Return Double.NaN
    End If
  End Function

  Private Function HighOrLowTimeseries(ByVal aTS As atcTimeseries, ByVal aNumValues As Integer, ByVal aHigh As Boolean) As atcTimeseries
    Try
      aTS.EnsureValuesRead()
      Dim sjday As Double = aTS.Dates.Value(0)
      Dim ejday As Double = aTS.Dates.Value(aTS.Dates.numValues)
      'Dim indexOld As Integer = 1
      Dim indexNew As Integer = 1
      Dim nYears As Integer = timdifJ(sjday, ejday, 6, 1)
      Dim newValues(nYears) As Double
      Dim newDates(nYears) As Double
      newDates(0) = sjday

      For indexNew = 1 To nYears
        Dim nextSJday As Double = TimAddJ(sjday, 6, 1, 1)
        Dim oneYear As atcTimeseries = SubsetByDate(aTS, sjday, nextSJday)
        newDates(indexNew) = nextSJday
        newValues(indexNew) = HighOrLowValue(oneYear, aNumValues, aHigh)
        sjday = nextSJday
      Next

      Dim newTS As New atcTimeseries(Me)
      newTS.Values = newValues
      newTS.Dates = New atcTimeseries(Me)
      newTS.Dates.Values = newDates

      Return newTS
    Catch ex As Exception
      Stop
    End Try
  End Function

  'Set the named attribute in aTimeseries using the definition from pAvailableStatistics
  Private Sub SetAttribute(ByVal aTimeseries As atcTimeseries, ByVal aName As String, ByVal aValue As Double)
    Dim ds As atcDataSet = pAvailableOperations.ItemByKey(aName)
    If Not ds Is Nothing Then
      Dim def As atcAttributeDefinition = ds.Attributes.GetDefinition(aName)
      If Not def Is Nothing Then
        aTimeseries.Attributes.SetValue(def, aValue)
      End If
    End If
  End Sub

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
    MapWin.Plugins.BroadcastMessage("Loading atcDataSource atcTimeseriesNdayHighLow")
    For Each operation As atcDataSet In AvailableOperations
      atcDataAttributes.AddDefinition(operation.Attributes(0).Definition)
    Next
  End Sub

  'WARNING: cousin copy in atcTimeseriesMath
  Private Function SubsetByDate(ByVal aTimeseries As atcTimeseries, _
                              ByVal aStartDate As Double, _
                              ByVal aEndDate As Double) As atcTimeseries
    'TODO: boundary conditions...
    Dim iStart As Integer = 0
    Dim iEnd As Integer = aTimeseries.numValues
    Dim numNewValues As Integer = iEnd + 1

    'TODO: binary search for iStart and iEnd could be faster
    While iStart < iEnd AndAlso aTimeseries.Dates.Value(iStart) < aStartDate
      iStart += 1
    End While

    While iEnd > iStart AndAlso aTimeseries.Dates.Value(iEnd) > aEndDate
      iEnd -= 1
    End While

    numNewValues = iEnd - iStart + 1
    Dim newValues(numNewValues) As Double
    Dim newDates(numNewValues) As Double

    If iStart > 0 Then
      System.Array.Copy(aTimeseries.Values, iStart - 1, newValues, 0, numNewValues + 1)
      System.Array.Copy(aTimeseries.Dates.Values, iStart - 1, newDates, 0, numNewValues + 1)
    Else
      System.Array.Copy(aTimeseries.Values, iStart, newValues, 0, numNewValues)
      System.Array.Copy(aTimeseries.Dates.Values, iStart, newDates, 0, numNewValues)
    End If
    Dim lnewTS As New atcTimeseries(Me)
    lnewTS.Dates = New atcTimeseries(Me)
    lnewTS.Values = newValues
    lnewTS.Dates.Values = newDates

    lnewTS.Attributes.ChangeTo(aTimeseries.Attributes)

    For Each lAttribute As atcDefinedValue In lnewTS.Attributes
      If lAttribute.Definition.Calculated Then
        lnewTS.Attributes.Remove(lAttribute)
      End If
    Next

    lnewTS.Attributes.SetValue("Parent Timeseries", aTimeseries)
    Return lnewTS
  End Function

End Class
