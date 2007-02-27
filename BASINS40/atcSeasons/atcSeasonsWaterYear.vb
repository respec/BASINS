Public Class atcSeasonsWaterYear
    Inherits atcSeasonBase

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsWaterYear
        lNewSeason.SeasonsSelected = SeasonsSelected.Clone
        Return lNewSeason
    End Function

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Dim lDate As Date = Date.FromOADate(aDate)
        If lDate.Month < 10 Then
            Return lDate.Year
        Else
            Return lDate.Year + 1
        End If
    End Function
End Class