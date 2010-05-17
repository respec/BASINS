Public Class atcSeasonsCalendarYear
    Inherits atcSeasonBase

    Private Shared pAllSeasons As Integer() = {}

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsCalendarYear
        lNewSeason.SetAllSeasons(pAllSeasons)
        lNewSeason.SeasonsSelected = SeasonsSelected
        Return lNewSeason
    End Function

    Public Overrides Function AllSeasons() As Integer()
        Return pAllSeasons
    End Function

    Public Sub SetAllSeasons(ByVal aAllSeasons As Integer())
        pAllSeasons = aAllSeasons.Clone
    End Sub

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Return Date.FromOADate(aDate).Year
    End Function
End Class