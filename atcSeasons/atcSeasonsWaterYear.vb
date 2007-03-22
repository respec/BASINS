Imports MapWinUtility

Public Class atcSeasonsWaterYear
    Inherits atcSeasonBase

    Private Shared pAllSeasons As Integer() = {}

    Public Overrides Function AllSeasons() As Integer()
        Return pAllSeasons
    End Function

    Public Sub SetAllSeasons(ByVal aAllSeasons As Integer())
        pAllSeasons = aAllSeasons.Clone
    End Sub

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Dim lDate As Date = Date.FromOADate(aDate)
        If lDate.Month < 10 Then
            Return lDate.Year
        Else
            Return lDate.Year + 1
        End If
    End Function

End Class