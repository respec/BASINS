Public Class atcSeasonsAMorPM
  Inherits atcSeasonBase

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