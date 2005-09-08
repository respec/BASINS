Imports atcUtility

Public Class atcSeasons

  'Divide the data in aTS into a group of TS, one per season
  Public Function Split(ByVal aTS As atcTimeseries) As atcDataGroup
    Dim lNewGroup As New atcDataGroup
    Dim lSeasonIndex As Integer = -1
    Dim lPrevSeasonIndex As Integer
    Dim lNewTS As atcTimeseries
    Dim lNewTSvalueIndex As Integer
    Dim lPoint As Boolean = aTS.Attributes.GetValue("point", False)

    For iValue As Integer = 1 To aTS.numValues
      lPrevSeasonIndex = lSeasonIndex
      If lPoint Then
        lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue))
      Else '
        lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue - 1))
      End If
      lNewTS = lNewGroup.ItemByKey(lSeasonIndex)
      If lNewTS Is Nothing Then
        lNewTS = New atcTimeseries(Nothing)
        CopyBaseAttributes(aTS, lNewTS)
        lNewTS.Dates = New atcTimeseries(Nothing)
        lNewTS.numValues = aTS.numValues
        lNewTS.Dates.numValues = aTS.numValues
        lNewTS.Attributes.AddHistory("Split by " & ToString() & " " & SeasonName(lSeasonIndex))
        lNewTS.Attributes.Add("SeasonDefinition", Me)
        lNewTS.Attributes.Add("SeasonIndex", lSeasonIndex)
        lNewGroup.Add(lSeasonIndex, lNewTS)
      End If

      If lPoint Then
        lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 1)
      Else
        lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 0)
        If lPrevSeasonIndex <> lSeasonIndex Then
          lNewTS.Values(lNewTSvalueIndex) = Double.NaN
          lNewTS.Dates.Value(lNewTSvalueIndex) = aTS.Dates.Value(iValue - 1)
          lNewTSvalueIndex += 1
        End If
      End If
      lNewTS.Value(lNewTSvalueIndex) = aTS.Value(iValue)
      lNewTS.Dates.Value(lNewTSvalueIndex) = aTS.Dates.Value(iValue)
      lNewTS.Attributes.SetValue("NextIndex", lNewTSvalueIndex + 1)
    Next

    For Each lNewTS In lNewGroup
      lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 1) - 1
      lNewTS.numValues = lNewTSvalueIndex
      lNewTS.Dates.numValues = lNewTSvalueIndex
      lNewTS.Attributes.RemoveByKey("nextindex")
    Next
    Return lNewGroup
  End Function

  Public Sub SetSeasonalAttributes(ByVal aTS As atcTimeseries, _
                                   ByVal aAttributes As atcDataAttributes, _
                          Optional ByVal aCalculatedAttributes As atcDataAttributes = Nothing)

    Dim lSplit As atcDataGroup = Me.Split(aTS)

    If aCalculatedAttributes Is Nothing Then
      aCalculatedAttributes = aTS.Attributes
    End If

    Dim lFormat As String = ""
    lFormat = lFormat.PadRight(Int(Log10(lSplit.Count)) + 1, "0")

    For Each lSeasonalTS As atcTimeseries In lSplit
      Dim lSeasonIndex As Integer = lSeasonalTS.Attributes.GetValue("SeasonIndex", 0)
      Dim lSeasonName As String = SeasonName(lSeasonIndex)
      For Each lAttribute As atcDefinedValue In aAttributes
        Dim lNewAttrDefinition As atcAttributeDefinition = lAttribute.Definition.Clone _
           (lAttribute.Definition.Name & " " & Me.ToString & " " & Format(lSeasonIndex, lFormat) & " " & lSeasonName, _
            Me.ToString & " " & lAttribute.Definition.Description)
        Dim lNewArguments As New atcDataAttributes
        lNewArguments.SetValue("SeasonDefinition", Me)
        lNewArguments.SetValue("SeasonIndex", lSeasonIndex)
        lNewArguments.SetValue("lSeasonName", lSeasonName)
        lNewAttrDefinition.Calculator = lAttribute.Definition.Calculator
        aCalculatedAttributes.SetValue(lNewAttrDefinition, lSeasonalTS.Attributes.GetValue(lAttribute.Definition.Name), lNewArguments)
      Next
    Next
  End Sub

  Public Overridable Function SeasonIndex(ByVal aDate As Double) As Integer
    Return -1
  End Function

  Public Overridable Function SeasonName(ByVal aDate As Double) As String
    Return SeasonName(SeasonIndex(aDate))
  End Function

  Public Overridable Function SeasonName(ByVal aIndex As Integer) As String
    Return CStr(aIndex)
  End Function

  Public Overrides Function ToString() As String
    Return Me.GetType.Name
  End Function
End Class

Public Class atcSeasonsAMPM
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    If Date.FromOADate(aDate).Hour < 12 Then
      Return 0
    Else
      Return 1
    End If
  End Function

  Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
    If aIndex = 0 Then Return "AM" Else Return "PM"
  End Function

  Public Overrides Function ToString() As String
    Return "AM/PM"
  End Function
End Class

Public Class atcSeasonsHour
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Hour
  End Function

  Public Overrides Function ToString() As String
    Return "Hour"
  End Function
End Class

Public Class atcSeasonsDayOfMonth
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Day
  End Function

  Public Overrides Function ToString() As String
    Return "DayOfMonth"
  End Function
End Class

Public Class atcSeasonsDayOfWeek
  Inherits atcSeasons

  Private pDayName() As String = {"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"}

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).DayOfWeek
  End Function

  Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
    Return pDayName(aIndex)
  End Function

  Public Overrides Function ToString() As String
    Return "Weekday"
  End Function
End Class

Public Class atcSeasonsDayOfYear
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).DayOfYear
  End Function

  Public Overrides Function ToString() As String
    Return "DayOfYear"
  End Function

End Class

Public Class atcSeasonsMonth
  Inherits atcSeasons

  Private pMonthName() As String = {"", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Month
  End Function

  Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
    Return pMonthName(aIndex)
  End Function

  Public Overrides Function ToString() As String
    Return "Month"
  End Function

End Class

Public Class atcSeasonsYear
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Year
  End Function

  Public Overrides Function ToString() As String
    Return "CalendarYear"
  End Function

End Class

Public Class atcSeasonsWaterYear
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Dim lDate As Date = Date.FromOADate(aDate)
    If lDate.Month < 10 Then
      Return lDate.Year
    Else
      Return lDate.Year + 1
    End If
  End Function

  Public Overrides Function ToString() As String
    Return "WaterYear"
  End Function

End Class

Public Class atcSeasonsThresholdTS
  Inherits atcSeasons

  Private pTS As atcTimeseries
  Private pThreshold As Double

  'Season 0 will be when values are < aThreshold or are out of range of aTS.Dates
  'Season 1 will be when values are >= aThreshold
  Public Sub New(ByVal aTS As atcTimeseries, ByVal aThreshold As Double)
    pTS = aTS
    aThreshold = aThreshold
  End Sub

  'If aDate is between two dates in aTS.Dates, the value at the later date is used
  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Dim iValue As Integer
    Dim lNumValues As Integer = pTS.Dates.numValues
    If lNumValues < 1 Then Return 0
    If aDate < pTS.Dates.Value(1) Then Return 0
    If aDate > pTS.Dates.Value(lNumValues) Then Return 0
    For iValue = 2 To lNumValues
      If aDate >= pTS.Dates.Value(iValue) Then
        If pTS.Value(iValue) >= pThreshold Then Return 1 Else Return 0
      End If
    Next
  End Function

  Public Overrides Function ToString() As String
    Return "Threshold"
  End Function
End Class

Public Class atcSeasonsYearSubset
  Inherits atcSeasons

  Private pTS As atcTimeseries
  Private pStartDate As Date
  Private pEndDate As Date
  Private pEndDateNextYear As Boolean = False

  'Season 0 will be when values are out of range  
  'Season 1 will be when values are in range  
  Public Sub New(ByVal aStartDate As Date, ByVal aEndDate As Date)
    pStartDate = aStartDate
    pEndDate = aEndDate
    If pEndDate.Year > pStartDate.Year Then
      pEndDateNextYear = True
    End If
  End Sub

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Dim lDate As Date = Date.FromOADate(aDate)
    Dim lDay As Integer
    If lDate.Month = 2 And lDate.Day = 29 Then
      lDay = 28
    Else
      lday = lDate.Day
    End If
    Dim lDateNoYear As Date = New Date(pStartDate.Year, lDate.Month, lDay, _
                                       lDate.Hour, lDate.Minute, lDate.Second, _
                                       lDate.Millisecond)
    If pEndDateNextYear Then
      If lDateNoYear >= pStartDate Then
        Return 1
      Else
        lDateNoYear = New Date(pEndDate.Year, lDate.Month, lDate.Day, _
                               lDate.Hour, lDate.Minute, lDate.Second, _
                               lDate.Millisecond)
        If lDateNoYear < pEndDate Then
          Return 1
        Else
          Return 0
        End If
      End If
    Else
      If lDateNoYear >= pStartDate AndAlso lDateNoYear < pEndDate Then
        Return 1
      Else
        Return 0
      End If
    End If

  End Function

  Public Overrides Function ToString() As String
    Return "YearSubset"
  End Function

End Class