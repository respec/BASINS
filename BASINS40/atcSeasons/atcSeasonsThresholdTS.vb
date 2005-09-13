Public Class atcSeasonsThresholdTS
  Inherits atcSeasons

  Private pTS As atcTimeseries
  Private pThreshold As Double

  'Season 0 will be when values are < aThreshold or are out of range of aTS.Dates
  'Season 1 will be when values are >= aThreshold
  Public Sub New(ByVal aTS As atcTimeseries, ByVal aThreshold As Double)
    pTS = aTS
    aThreshold = aThreshold
  End Sub

  'If aDate is between two dates in aTS.Dates, the value at the later date is used
  Public Overrides Function SeasonIndex(ByVal aDate As Double) As Integer
    Dim iValue As Integer
    Dim lNumValues As Integer = pTS.Dates.numValues
    If lNumValues < 1 Then Return 0
    If aDate < pTS.Dates.Value(1) Then Return 0
    If aDate > pTS.Dates.Value(lNumValues) Then Return 0
    For iValue = 2 To lNumValues
      If aDate >= pTS.Dates.Value(iValue) Then
        If pTS.Value(iValue) >= pThreshold Then Return 1 Else Return 0
      End If
    Next
  End Function

  Public Overrides Function ToString() As String
    Return "Threshold"
  End Function
End Class