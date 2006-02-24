Public Class atcSeasonsDayOfYear
  Inherits atcSeasonBase

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).DayOfYear
  End Function

  Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
    Select Case aIndex
      Case Is < 1 : Return Nothing
      Case Is < 367 : Return CStr(aIndex)
      Case Else : Return Nothing
    End Select
  End Function

End Class
