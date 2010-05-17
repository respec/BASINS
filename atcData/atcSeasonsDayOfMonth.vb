Public Class atcSeasonsDayOfMonth
    Inherits atcSeasonBase

    Private Shared pAllSeasons As Integer() = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31}

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsDayOfMonth
        lNewSeason.SeasonsSelected = SeasonsSelected
        Return lNewSeason
    End Function

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Return Date.FromOADate(aDate).Day
    End Function

    Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
        Select Case aIndex
            Case Is < 1 : Return Nothing
            Case Is < 32 : Return CStr(aIndex)
            Case Else : Return Nothing
        End Select
    End Function

    Public Overrides Function AllSeasons() As Integer()
        Return pAllSeasons
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
