Imports atcData
Imports atcUtility

Public Class atcSeasonsMonthDay
    Inherits atcSeasonBase

    Shared ndaymon() As Integer = {0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsDayOfYear
        lNewSeason.SeasonsSelected = SeasonsSelected
        Return lNewSeason
    End Function

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Dim lDates(5) As Integer
        J2Date(aDate, lDates)
        'timcnv(lDates) 'has to get rid of this step to make the initial date to be the beginning of day instead of end of previous day
        Dim lIndex As Integer = lDates(2)
        For lMon As Integer = 1 To lDates(1) - 1
            lIndex += ndaymon(lMon)
        Next
        Return lIndex
    End Function

    Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
        Dim lMon As Integer = 0
        While aIndex > ndaymon(lMon)
            aIndex -= ndaymon(lMon)
            lMon += 1
        End While

        Return lMon & "/" & aIndex
    End Function

    Public Overrides Function AllSeasons() As Integer()
        Dim lAllSeasons As Integer()
        ReDim lAllSeasons(366)
        For lSeasonIndex As Integer = 0 To 366
            lAllSeasons(lSeasonIndex) = lSeasonIndex - 1
        Next
        Return lAllSeasons
    End Function

    Public Overrides Property SeasonSelected(ByVal aSeasonIndex As Integer) As Boolean
        Get
            Return MyBase.SeasonSelected(aSeasonIndex - 1)
        End Get
        Set(ByVal newValue As Boolean)
            MyBase.SeasonSelected(aSeasonIndex - 1) = newValue
        End Set
    End Property

End Class
