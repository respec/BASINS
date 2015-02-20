Imports atcUSGSRecess

Public Class clsWTFPower
    Inherits clsWTF

    'The user chooses values of hte coefficients to have best fit to extracted data
    'Decline rate = dZwt/dt = -(c + d(Zwt - e)^f)
    Public ParamCIntercept As Double 'intercept on the decline rate axis [L/T]
    Public ParamDMultiplier As Double 'a multiplier of elevation [-]
    Public ParamEDatum As Double 'an elevation modifier (e.g. a datum) with units of elevation [L]
    Public ParamFExp As Double 'the exponent

    Public ParametersSet As Boolean = False

    Public Function AntecedentGWL(ByVal aTimePeak As Double, _
                                  ByVal aTime0 As Double, _
                                  ByVal aGWLH0 As Double) As Double
        If ParametersSet Then
            Dim lDeclineRate As Double = ParamCIntercept + ParamDMultiplier * Math.Pow((aGWLH0 - ParamEDatum), ParamFExp)
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
        If ParamCIntercept = 0 And ParamDMultiplier = 0 And ParamEDatum = 0 And ParamFExp = 0 Then
            ParametersSet = False
        End If
    End Sub

    Public Overrides Function Recharge(ByVal aRiseObj As atcUSGSRecess.clsFall) As Boolean
        'MyBase.Recharge(aRiseObj)
        If Not ParametersSet Then Return False

        For Each lSeg As clsRecessionSegment In aRiseObj.listOfSegments
            With lSeg
                If .Flow Is Nothing Then .ReadData()
                Dim lAntGWL As Double = AntecedentGWL(.Flow.Length - 1, 1, .HzeroDayValue)
                If .AntecedentGWLMethods.Keys.Contains(AntecedentGWLMethod.Power) Then
                    .AntecedentGWLMethods.ItemByKey(AntecedentGWLMethod.Power) = lAntGWL
                Else
                    .AntecedentGWLMethods.Add(AntecedentGWLMethod.Power, lAntGWL)
                End If

                Dim lDeltaH As Double = .Flow(.Flow.Length - 1) - lAntGWL
                Dim lRecharge As Double = SpecificYield * lDeltaH / .Flow.Length - 1
                If .Recharges.Keys.Contains(AntecedentGWLMethod.Power) Then
                    .Recharges.ItemByKey(AntecedentGWLMethod.Power) = lRecharge
                Else
                    .Recharges.Add(AntecedentGWLMethod.Power, lRecharge)
                End If
            End With
        Next
        Return True
    End Function

    Public Overrides Sub Clear()
        MyBase.Clear()
        ParamCIntercept = 0
        ParamDMultiplier = 0
        ParamEDatum = 0
        ParamFExp = 0
        ParametersSet = False
    End Sub
End Class
