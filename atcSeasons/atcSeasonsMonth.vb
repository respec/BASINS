Public Class atcSeasonsMonth
  Inherits atcSeasonBase

  Private pMonthName() As String = {"", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Return Date.FromOADate(aDate).Month
  End Function

  Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
    Select Case aIndex
      Case Is < 1 : Return Nothing
      Case Is < 13 : Return pMonthName(aIndex)
      Case Else : Return Nothing
    End Select
  End Function

End Class