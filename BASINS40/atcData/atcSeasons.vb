Imports atcUtility

Public Class atcSeasons

  'Divide the data in aTS into a group of TS, one per season
  Public Overridable Function Split(ByVal aTS As atcTimeseries) As atcDataGroup
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

  Public Overridable Function SeasonIndex(ByVal aDate As Double) As Integer

  End Function

  Public Overridable Function SeasonName(ByVal aDate As Double) As String
    Return SeasonName(SeasonIndex(aDate))
  End Function

  Public Overridable Function SeasonName(ByVal aIndex As Integer) As String
    Return CStr(aIndex)
  End Function

  Public Overrides Function ToString() As String
    Return "<none>"
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
    Return "AM or PM"
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
    Return "Day"
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
    Return "Day of Week"
  End Function
End Class

Public Class atcSeasonsDayOfYear
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).DayOfYear
  End Function

  Public Overrides Function ToString() As String
    Return "Day of Year"
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
    Return "Year"
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
End Class

Public Class atcSeasonsYearSubset
  Inherits atcSeasons

  Private pTS As atcTimeseries
  Private pStartDate As Double
  Private pEndDate As Double

  'Season 0 will be when values are out of range  
  'Season 1 will be when values are in range  
  Public Sub New(ByVal aStartDate As Double, ByVal aEndDate As Double)
    pStartDate = aStartDate
    pEndDate = aEndDate
  End Sub

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Dim lYears As Integer = timdifJ(pStartDate, aDate, 6, 1)
    Dim lStartdate As Double = TimAddJ(pStartDate, 6, 1, lYears)
    lYears = timdifJ(pEndDate, aDate, 6, 1)
    Dim lEnddate As Double = TimAddJ(pEndDate, 6, 1, lYears)

    If aDate > lStartdate AndAlso aDate < lEnddate Then
      Return 1
    Else
      Return 0
    End If

  End Function

  Public Overrides Function ToString() As String
    Return "YearSubset"
  End Function

End Class