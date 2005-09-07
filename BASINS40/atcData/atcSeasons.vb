Public Class atcSeasons

  'Divide the data in aTS into a group of TS, one per season
  Public Overridable Function Divide(ByVal aTS As atcTimeseries) As atcDataGroup
    Dim lNewGroup As New atcDataGroup
    Dim lSeasonIndex As Integer
    Dim lNewTS As atcTimeseries
    Dim lNewTSvalueIndex As Integer
    For iValue As Integer = 1 To aTS.numValues
      lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue))
      lNewTS = lNewGroup.ItemByKey(lSeasonIndex)
      If lNewTS Is Nothing Then
        lNewTS = New atcTimeseries(Nothing)
        lNewTS.Dates = New atcTimeseries(Nothing)
        lNewTS.numValues = aTS.numValues
        lNewTS.Dates.numValues = aTS.numValues
        lNewTS.Attributes.AddHistory("Divide by date " & lSeasonIndex)
        lNewGroup.Add(lSeasonIndex, lNewTS)
      End If
      lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 1)
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
End Class

Public Class atcSeasonsHour
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Hour
  End Function
End Class

Public Class atcSeasonsDayOfMonth
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Day
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
End Class

Public Class atcSeasonsDayOfYear
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).DayOfYear
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
End Class

Public Class atcSeasonsYear
  Inherits atcSeasons

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Year
  End Function
End Class