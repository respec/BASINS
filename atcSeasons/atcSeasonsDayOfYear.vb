Public Class atcSeasonsDayOfYear
    Inherits atcSeasonBase

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsDayOfYear
        lNewSeason.SeasonsSelected = SeasonsSelected
        Return lNewSeason
    End Function

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Return Date.FromOADate(aDate).DayOfYear
    End Function

    Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
        Select Case aIndex
            Case Is < 1 : Return Nothing
            Case Is < 367 : Return CStr(aIndex)
            Case Else : Return Nothing
        End Select
    End Function

    Public Overrides Function AllSeasons() As Integer()
        Dim lAllSeasons As Integer()
        ReDim lAllSeasons(365)
        For lSeasonIndex As Integer = 0 To 365
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
