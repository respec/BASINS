Imports atcUSGSRecess
Imports atcStatistics
Imports atcUtility

Public Class clsWTFLinear
    Inherits clsWTF

    Public LinearSlope As Double
    Public LinearIntercept As Double

    Public ParametersSet As Boolean = False

    Public Function AntecedentGWL(ByVal aTimePeak As Double, _
                                  ByVal aTime0 As Double, _
                                  ByVal aGWLH0 As Double) As Double
        If ParametersSet Then
            Dim lDeclineRate As Double = LinearSlope * aGWLH0 + LinearIntercept
            Return aGWLH0 - lDeclineRate * (aTimePeak - aTime0)
        Else
            Return -99
        End If
    End Function

    Public Overrides Sub EstimateParameters(ByVal aFallObj As clsFall)

        If aFallObj Is Nothing OrElse aFallObj.listOfSegments Is Nothing OrElse aFallObj.listOfSegments.Count = 0 Then
            ParametersSet = False
            Exit Sub
        End If

        Dim lCalculator As New atcStatistics.atcStatistics()

        Dim lCoef1Sum As Double
        Dim lCoef2Sum As Double

        Dim ldZwtPerDay As Double
        For Each lSeg As clsRecessionSegment In aFallObj.listOfSegments
            'Dim lRatesInFallingLimbs As New atcCollection()
            Dim lRates(lSeg.Flow.Length - 1) As Double
            'Dim lFlow(lSeg.Flow.Length - 1) As Double
            'For I As Integer = 1 To lSeg.Flow.Length - 1
            '    lFlow(I) = lSeg.Flow(I)
            'Next
            For I As Integer = 2 To lSeg.Flow.Length - 1
                ldZwtPerDay = lSeg.Flow(I) - lSeg.Flow(I - 1)
                lRates(I - 1) = ldZwtPerDay 'Should we ignore zero rates???
                'If Not lRatesInFallingLimbs.Keys.Contains(lSeg.Flow(I - 1)) Then
                '    lRatesInFallingLimbs.Add(lSeg.Flow(I - 1), ldZwtPerDay) 'eg 10.15 ft, -0.05 ft/day
                'End If
            Next

            Dim lCoef1 As Double = 0
            Dim lCoef2 As Double = 0

            lCalculator.DoRegression(lSeg.Flow, lRates, lSeg.Flow.Length - 1, lCoef1, lCoef2)

            lCoef1Sum += lCoef1
            lCoef2Sum += lCoef2
        Next

        LinearSlope = lCoef1Sum / aFallObj.listOfSegments.Count
        LinearIntercept = lCoef2Sum / aFallObj.listOfSegments.Count
        ParametersSet = True
    End Sub

    Public Overrides Function Recharge(ByVal aRiseObj As atcUSGSRecess.clsFall) As Boolean
        'MyBase.Recharge(aRiseObj)
        If Not ParametersSet Then Return False

        For Each lSeg As clsRecessionSegment In aRiseObj.listOfSegments
            With lSeg
                If .Flow Is Nothing Then .ReadData()
                Dim lAntGWL As Double = AntecedentGWL(.Flow.Length - 1, 1, .HzeroDayValue)
                If .AntecedentGWLs.Keys.Contains(AntecedentGWLMethod.Linear) Then
                    .AntecedentGWLs.ItemByKey(AntecedentGWLMethod.Linear) = lAntGWL
                Else
                    .AntecedentGWLs.Add(AntecedentGWLMethod.Linear, lAntGWL)
                End If

                Dim lDeltaH As Double = .Flow(.Flow.Length - 1) - lAntGWL
                Dim lRecharge As Double = SpecificYield * lDeltaH / .Flow.Length - 1
                If .Recharges.Keys.Contains(AntecedentGWLMethod.Linear) Then
                    .Recharges.ItemByKey(AntecedentGWLMethod.Linear) = lRecharge
                Else
                    .Recharges.Add(AntecedentGWLMethod.Linear, lRecharge)
                End If
            End With
        Next
        Return True
    End Function

    Public Overrides Sub Clear()
        MyBase.Clear()
        LinearSlope = 0
        LinearIntercept = 0
        ParametersSet = False
    End Sub
End Class
