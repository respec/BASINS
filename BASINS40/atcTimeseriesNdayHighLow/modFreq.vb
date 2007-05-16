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
    Declare Sub KENT Lib "usgs_swstats.dll" (ByRef X As Single, _
                                              ByRef N As Integer, _
                                              ByRef TAU As Single, _
                                              ByRef PLEVEL As Single, _
                                              ByRef SLOPE As Single)

    'Kendall Tau Calculation
    Sub KendallTau(ByVal aTs As atcTimeseries, ByRef aTau As Double, ByRef aLevel As Double, ByRef aSlope As Double)
        Dim lQ() As Single
        Dim lN As Integer = aTs.numValues
        Dim lTau As Single
        Dim lLevel As Single
        Dim lSlope As Single
        ReDim lQ(lN - 1)
        For i As Integer = 1 To aTs.numValues
            If Double.IsNaN(aTs.Values(i)) Then
                lQ(i - 1) = 0
            Else
                lQ(i - 1) = aTs.Values(i)
            End If
        Next
        KENT(lQ(0), lN, lTau, lLevel, lSlope)
        aTau = lTau
        aLevel = lLevel
        aSlope = lSlope
    End Sub

    'frequency analysis for specified recurrence interval or probability
    Function PearsonType3(ByVal aTs As atcTimeseries, ByVal aRecurOrProb As Double, ByVal aHigh As Boolean) As Double

        Dim lN As Integer = aTs.Attributes.GetValue("Count")
        Dim lMean As Double = aTs.Attributes.GetValue("Mean")
        Dim lStd As Double = aTs.Attributes.GetValue("Standard Deviation")
        Dim lSkew As Double = aTs.Attributes.GetValue("Skew")

        If lN = 0 OrElse lMean <= 0 Then 'no data or problem data
            Return Double.NaN
        Else
            'Turn recurrence into probability
            If aRecurOrProb > 1 Then aRecurOrProb = 1.0 / aRecurOrProb

            Dim lNzi As Integer = aTs.numValues - lN
            Dim lNumons As Integer = 1
            Dim lLogarh As Integer = 2 'log trans flag 1-yes,2-no (handle earlier)

            Dim lSe(0) As Single
            If aHigh Then
                lSe(0) = aRecurOrProb
            Else
                lSe(0) = 1 - aRecurOrProb
            End If
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

            Dim lIlh As Integer  'stats option 1-hi, 2-low,3-month
            If aHigh Then lIlh = 1 Else lIlh = 2

            LGPSTX(lN, lNzi, lNumons, (lI + 1), lMean, lStd, lSkew, lLogarh, lIlh, False, lSe(0), _
                   lC(0), lCcpa(0), lP(0), lQ(0), lAdp(0), lQnew(0), lRi(0), lRsout(0), lRetcod)

            Return lQ(0)
        End If
    End Function
End Module
