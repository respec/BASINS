Public Class atcSeasonsCalendarYear
    Inherits atcSeasonBase

    Private pSeasonsSelected As New ArrayList

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsCalendarYear
        lNewSeason.pSeasonsSelected = pSeasonsSelected.Clone
        Return lNewSeason
    End Function

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Return Date.FromOADate(aDate).Year
    End Function

    Public Overrides Property SeasonSelected(ByVal aSeasonIndex As Integer) As Boolean
        Get
            Return pSeasonsSelected.Contains(aSeasonIndex)
        End Get
        Set(ByVal newValue As Boolean)
            If newValue Then
                If Not pSeasonsSelected.Contains(aSeasonIndex) Then
                    pSeasonsSelected.Add(newValue)
                End If
            Else
                While pSeasonsSelected.Contains(aSeasonIndex)
                    pSeasonsSelected.Remove(aSeasonIndex)
                End While
            End If
        End Set
    End Property

End Class