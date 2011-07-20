Public Class atcSeasonsDayOfWeek
    Inherits atcSeasonBase

    Private Shared pAllSeasons As Integer() = {1, 2, 3, 4, 5, 6, 7}
    Private Shared pDayNames() As String = {"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"}

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsDayOfWeek
        lNewSeason.SeasonsSelected = SeasonsSelected
        Return lNewSeason
    End Function

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Return Date.FromOADate(aDate).DayOfWeek + 1
    End Function

    Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
        Select Case aIndex
            Case Is < 1 : Return Nothing
            Case Is < 8 : Return pDayNames(aIndex - 1)
            Case Else : Return Nothing
        End Select
    End Function

    Public Overrides Function AllSeasons() As Integer()
        Return pAllSeasons
    End Function

    Public Overrides Function AllSeasonNames() As String()
        Return pDayNames
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
