Public Class atcSeasonsCalendarYear
  Inherits atcSeasonBase

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Year
  End Function
End Class