Public Class atcSeasonsDayOfWeek
  Inherits atcSeasonBase

  Private pDayName() As String = {"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"}

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).DayOfWeek
  End Function

  Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
    Select Case aIndex
      Case Is < 0 : Return Nothing
      Case Is < 7 : Return pDayName(aIndex)
      Case Else : Return Nothing
    End Select
  End Function
End Class
