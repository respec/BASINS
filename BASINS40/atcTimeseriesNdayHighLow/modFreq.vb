Imports atcData

Module modFreq
  Declare Sub LGPSTX Lib "usgs_swstats.dll" (ByRef N As Integer, _
                                             ByRef NZI As Integer, _
                                             ByRef NUMONS As Integer, _
                                             ByRef NQS As Integer, _
                                             ByRef XBAR As Single, _
                                             ByRef STD As Single, _
                                             ByRef SKEW As Single, _
                                             ByRef LOGARH As Integer, _
                                             ByRef ILH As Integer, _
                                             ByRef DBG As Boolean, _
                                             ByRef SE As Single, _
                                             ByRef C As Single, _
                                             ByRef CCPA As Single, _
                                             ByRef P As Single, _
                                             ByRef Q As Single, _
                                             ByRef ADP As Single, _
                                             ByRef QNEW As Single, _
                                             ByRef RI As Single, _
                                             ByRef RSOUT As Single, _
                                             ByRef RETCOD As Integer)

  'frequency analysis for specified recurrence interval or probability
  Function PearsonType3(ByVal aTs As atcTimeseries, ByVal aRecurOrProb As Double, ByVal aHigh As Boolean) As Double

    Dim lN As Integer = aTs.Attributes.GetValue("Count")
    Dim lMean As Double = aTs.Attributes.GetValue("Mean")
    Dim lStd As Double = aTs.Attributes.GetValue("Standard Deviation")
    Dim lSkew As Double = aTs.Attributes.GetValue("Skew")

    If lN = 0 Or lMean <= 0 Then 'no data or problem data
      Return Double.NaN
    Else
      Dim lNzi As Integer = aTs.numValues - lN

      Dim lNumons As Integer = 1
      Dim lLogarh As Integer = 2 'log trans flag 1-yes,2-no (handle earlier)

      'be sure aRecurOrProb is available in lSe
      Dim lSe() As Single = {0.005, 0.01, 0.02, 0.04, 0.05, 0.1, 0.2, 0.3333, 0.5, _
                             0.6667, 0.8, 0.9, 0.95, 0.96, 0.98, 0.99, 0.995}
      Dim lI As Integer = UBound(lSe)  'number of recurrence intervals to calculate-1

      Dim lC(lI) As Single
      Dim lCcpa(lI) As Single
      Dim lP(lI) As Single
      Dim lQ(lI) As Single
      Dim lAdp(lI) As Single
      Dim lQnew(lI) As Single
      Dim lRi(lI) As Single
      Dim lRsout(1 + (2 * lI)) As Single
      Dim lRetcod As Integer

      Dim lIlh As Integer = 2 'stats option 1-hi, 2-low,3-month
      If aHigh Then lIlh = 1

      LGPSTX(lN, lNzi, lNumons, lI + 1, lMean, lStd, lSkew, lLogarh, lIlh, False, lSe(0), _
             lC(0), lCcpa(0), lP(0), lQ(0), lAdp(0), lQnew(0), lRi(0), lRsout(0), lRetcod)

      If aRecurOrProb > 1 Then aRecurOrProb = 1.0 / aRecurOrProb

      'TODO: what about lQnew?
      For i As Integer = 0 To lI
        If Math.Abs(lSe(i) - aRecurOrProb) < 0.0001 Then
          Return lQ(i)
        End If
      Next
      Return Double.NaN
    End If
  End Function
End Module
