Public Class atcSeasonsDayOfYear
  Inherits atcSeasonBase

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).DayOfYear
  End Function
End Class
