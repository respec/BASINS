Public Class atcSeasonsMonth
  Inherits atcSeasonBase

  Private pMonthName() As String = {"", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Month
  End Function

  Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
    Return pMonthName(aIndex)
  End Function
End Class