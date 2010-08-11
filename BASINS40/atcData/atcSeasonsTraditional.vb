Public Class atcSeasonsTraditional
    Inherits atcSeasonBase

    Private Shared pAllSeasons As Integer() = {0, 1, 2, 3}
    Private Shared pSeasonNames() As String = {"Winter", "Spring", "Summer", "Autumn"}

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsTraditional
        lNewSeason.SeasonsSelected = SeasonsSelected
        Return lNewSeason
    End Function

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Dim lDate As Date = Date.FromOADate(aDate)
        'NOTE: The exact starting day of seasons is not always agreed on
        'and can change from year to year. 
        'Here we assume March 20, June 21, September 22 and December 21 
        'are always the first days of the seasons
        Select Case lDate.Month
            Case 1, 2 : Return 0
            Case 3 : If lDate.Day < 20 Then Return 0 Else Return 1
            Case 4, 5 : Return 1
            Case 6 : If lDate.Day < 21 Then Return 1 Else Return 2
            Case 7, 8 : Return 2
            Case 9 : If lDate.Day < 22 Then Return 2 Else Return 3
            Case 10, 11 : Return 3
            Case 12 : If lDate.Day < 21 Then Return 3 Else Return 0
            Case Else : Return -1
        End Select
    End Function

    Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
        Select Case aIndex
            Case Is < 0 : Return Nothing
            Case Is < 4 : Return pSeasonNames(aIndex)
            Case Else : Return Nothing
        End Select
    End Function

    Public Overrides Function AllSeasons() As Integer()
        Return pAllSeasons
    End Function

    Public Overrides Function AllSeasonNames() As String()
        Return pSeasonNames
    End Function
End Class
