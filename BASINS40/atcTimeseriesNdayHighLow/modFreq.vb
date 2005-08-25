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
  Sub PearsonType3(ByVal lN As Integer, ByVal lNzi As Integer, _
                   ByVal lMean As Double, ByVal lStd As Double, ByVal lSkew As Double, _
                   ByVal lRecurOrProb As Double, ByRef lQOut As Double)

    Dim lNumons As Integer = 1
    Dim lLogarh As Integer = 2 'log trans flag 1-yes,2-no
    Dim lIlh As Integer = 2 'stats option 1-hi, 2-low,3-month
    Dim lNqs As Integer = 12 'number of statistics
    Dim lSe() As Single = {0.01, 0.02, 0.05, 0.1, 0.2, 0.3333, 0.5, 0.8, 0.9, 0.96, 0.98, 0.99}
    Dim lI = lNqs - 1
    Dim lC(lI) As Single
    Dim lCcpa(lI) As Single
    Dim lP(lI) As Single
    Dim lQ(lI) As Single
    Dim lAdp(lI) As Single
    Dim lQnew(lI) As Single
    Dim lRi(lI) As Single
    Dim lRsout(2 * lI) As Single
    Dim lRetcod As Integer

    LGPSTX(lN, lNzi, lNumons, lNqs, lMean, lStd, lSkew, lLogarh, True, lIlh, lSe(0), _
           lC(0), lCcpa(0), lP(0), lQ(0), lAdp(0), lQnew(0), lRi(0), lRsout(0), lRetcod)

    If lRecurOrProb > 1 Then lRecurOrProb = 1.0 / lRecurOrProb

    For i As Integer = 0 To lI
      If Math.Abs(lSe(i) - lRecurOrProb) < 0.00001 Then
        lQOut = lQ(i)
        Exit For
      End If
    Next
  End Sub
End Module
