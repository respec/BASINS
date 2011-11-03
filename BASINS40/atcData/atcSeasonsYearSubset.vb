Imports atcData

Public Class atcSeasonsYearSubset
    Inherits atcSeasonBase

    Private Shared pAllSeasons As Integer() = {0, 1}
    Private pStartDate As Date
    Private pEndDate As Date
    Private pEndDateNextYear As Boolean = False

    Public Overrides Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonsYearSubset(pStartDate, pEndDate)
        lNewSeason.SeasonsSelected = SeasonsSelected
        Return lNewSeason
    End Function

    'Season 0 = values are outside range  
    'Season 1 = values are in range  
    Public Sub New(ByVal aStartDate As Date, ByVal aEndDate As Date)
        pStartDate = aStartDate
        pEndDate = aEndDate
        pEndDateNextYear = (pEndDate.Year > pStartDate.Year)
    End Sub

    Public Sub New(ByVal aStartMonth As Integer, ByVal aStartDay As Integer, ByVal aEndMonth As Integer, ByVal aEndDay As Integer)
        aStartDay = Math.Min(aStartDay, atcUtility.daymon(1901, aStartMonth))
        If aStartDay < 1 Then aStartDay = 1
        aEndDay = Math.Min(aEndDay, atcUtility.daymon(1901, aEndMonth))
        If aEndDay < 1 Then aEndDay = 1

        pStartDate = New Date(1900, aStartMonth, aStartDay, 0, 0, 0, 0)
        If aEndMonth < aStartMonth OrElse aEndMonth = aStartMonth AndAlso aEndDay < aStartDay Then
            pEndDateNextYear = True
            pEndDate = New Date(1901, aEndMonth, aEndDay, 0, 0, 0, 0)
        Else
            pEndDate = New Date(1900, aEndMonth, aEndDay, 0, 0, 0, 0)
        End If
        pEndDate = pEndDate.AddDays(1) 'Beginning of next day = end of desired day
    End Sub

#If BatchMode Then
#Else
    Public Sub New()
        Dim lStartMonth As Integer = CInt(GetSetting("BASINS4", "Seasons", "YearSubsetStartMonth", "1"))
        Dim lStartDay As Integer = CInt(GetSetting("BASINS4", "Seasons", "YearSubsetStartDay", "1"))
        Dim lEndMonth As Integer = CInt(GetSetting("BASINS4", "Seasons", "YearSubsetEndMonth", "12"))
        Dim lEndDay As Integer = CInt(GetSetting("BASINS4", "Seasons", "YearSubsetEndDay", "31"))

        Dim lForm As New frmSpecifyYearSubset
        If lForm.AskUser(lStartMonth, lStartDay, lEndMonth, lEndDay) Then
            pStartDate = New Date(1900, lStartMonth, lStartDay, 0, 0, 0, 0)
            pEndDate = New Date(1900, lEndMonth, lEndDay, 0, 0, 0, 0)

            SaveSetting("BASINS4", "Seasons", "YearSubsetStartMonth", lStartMonth)
            SaveSetting("BASINS4", "Seasons", "YearSubsetStartDay", lStartDay)
            SaveSetting("BASINS4", "Seasons", "YearSubsetEndMonth", lEndMonth)
            SaveSetting("BASINS4", "Seasons", "YearSubsetEndDay", lEndDay)
        Else
            Throw New Exception("User cancelled")
        End If
    End Sub
#End If

    Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
        Dim lDate As Date = Date.FromOADate(aDate)
        Dim lDay As Integer = lDate.Day
        If lDay = 29 AndAlso lDate.Month = 2 Then
            lDay = 28  'place leap day in same season as the 28th
        End If
        Dim lDateNoYear As Date = New Date(pStartDate.Year, lDate.Month, lDay, _
                                           lDate.Hour, lDate.Minute, lDate.Second, _
                                           lDate.Millisecond)
        If pEndDateNextYear Then
            If lDateNoYear >= pStartDate Then
                Return 1
            Else
                lDateNoYear = New Date(pEndDate.Year, lDate.Month, lDay, _
                                       lDate.Hour, lDate.Minute, lDate.Second, _
                                       lDate.Millisecond)
                If lDateNoYear < pEndDate Then
                    Return 1
                Else
                    Return 0
                End If
            End If
        Else
            If lDateNoYear >= pStartDate AndAlso lDateNoYear < pEndDate Then
                Return 1
            Else
                Return 0
            End If
        End If
    End Function

    Public Overrides Function Split(ByVal aTS As atcData.atcTimeseries, ByVal aSource As atcData.atcTimeseriesSource) As atcData.atcTimeseriesGroup
        'Do the split
        Dim lSplit As atcTimeseriesGroup = MyBase.Split(aTS, aSource)

        'Set attributes specifying when season begins and ends
        For Each lDataSet As atcDataSet In lSplit
            With lDataSet.Attributes
                If .GetValue("SeasonIndex", 0) = 1 Then 'In specified season
                    .SetValue("seasbg", pStartDate.Month)
                    .SetValue("seadbg", pStartDate.Day)
                    .SetValue("seasnd", pEndDate.Month)
                    .SetValue("seadnd", pEndDate.Day)
                Else 'Out-of-season has opposite start/end
                    .SetValue("seasbg", pEndDate.Month)
                    .SetValue("seadbg", pEndDate.Day)
                    .SetValue("seasnd", pStartDate.Month)
                    .SetValue("seadnd", pStartDate.Day)
                End If
            End With
        Next
        Return lSplit
    End Function

    Public Overloads Overrides Function SeasonName(ByVal aIndex As Integer) As String
        Select Case aIndex
            Case Is < 0 : Return Nothing
            Case 0 : Return "Outside"
            Case 1 : Return "Inside"
            Case Else : Return Nothing
        End Select
    End Function

    Public Overrides Function AllSeasons() As Integer()
        Return pAllSeasons
    End Function

End Class