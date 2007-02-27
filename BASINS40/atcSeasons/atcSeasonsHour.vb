Public Class atcSeasonsHour
    Inherits atcSeasonBase

    Private Shared pAllSeasons As Integer() = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23}

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsHour
        lNewSeason.SeasonsSelected = SeasonsSelected.Clone
        Return lNewSeason
    End Function

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Return Date.FromOADate(aDate).Hour
    End Function

    Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
        Select Case aIndex
            Case Is < 0 : Return Nothing
            Case Is < 24 : Return CStr(aIndex)
            Case Else : Return Nothing
        End Select
    End Function

    Public Overrides Function AllSeasons() As Integer()
        Return pAllSeasons
    End Function

End Class
