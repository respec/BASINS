Imports atcData

Public Class atcSeasonsYearSubset
  Inherits atcSeasonBase

  Private pTS As atcTimeseries
  Private pStartDate As Date
  Private pEndDate As Date
  Private pEndDateNextYear As Boolean = False

  'Season 0 = values are outside range  
  'Season 1 = values are in range  
  Public Sub New(ByVal aStartDate As Date, ByVal aEndDate As Date)
    pStartDate = aStartDate
    pEndDate = aEndDate
    If pEndDate.Year > pStartDate.Year Then
      pEndDateNextYear = True
    End If
  End Sub

  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Dim lDate As Date = Date.FromOADate(aDate)
    Dim lDay As Integer
    If lDate.Month = 2 And lDate.Day = 29 Then
      lDay = 28
    Else
      lDay = lDate.Day
    End If
    Dim lDateNoYear As Date = New Date(pStartDate.Year, lDate.Month, lDay, _
                                       lDate.Hour, lDate.Minute, lDate.Second, _
                                       lDate.Millisecond)
    If pEndDateNextYear Then
      If lDateNoYear >= pStartDate Then
        Return 1
      Else
        lDateNoYear = New Date(pEndDate.Year, lDate.Month, lDate.Day, _
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
End Class