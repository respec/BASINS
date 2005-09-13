Public Class atcSeasonsDayOfMonth
  Inherits atcSeasonBase

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Day
  End Function
End Class
