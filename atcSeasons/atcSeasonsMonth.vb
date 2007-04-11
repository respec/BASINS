Public Class atcSeasonsMonth
    Inherits atcSeasonBase

    Private Shared pAllSeasons As Integer() = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}
    Private Shared pMonthNames() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsMonth
        lNewSeason.SeasonsSelected = SeasonsSelected
        Return lNewSeason
    End Function

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Return Date.FromOADate(aDate).Month
    End Function

    Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
        Select Case aIndex
            Case Is < 1 : Return Nothing
            Case Is < 13 : Return pMonthNames(aIndex - 1)
            Case Else : Return Nothing
        End Select
    End Function

    Public Overrides Function AllSeasons() As Integer()
        Return pAllSeasons
    End Function

    Public Overrides Function AllSeasonNames() As String()
        Return pMonthNames
    End Function
End Class