Public Class atcSeasonsAMorPM
    Inherits atcSeasonBase

    Private Shared pAllSeasons As Integer() = {0, 1}

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsAMorPM
        lNewSeason.SeasonsSelected = SeasonsSelected.Clone
        Return lNewSeason
    End Function

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        If Date.FromOADate(aDate).Hour < 12 Then
            Return 0
        Else
            Return 1
        End If
    End Function

    Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
        Select Case aIndex
            Case 0 : Return "AM"
            Case 1 : Return "PM"
            Case Else : Return Nothing
        End Select
    End Function

    Public Overrides Function AllSeasons() As Integer()
        Return pAllSeasons
    End Function
End Class