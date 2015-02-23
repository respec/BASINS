Imports atcUSGSRecess

Public Class clsWTFFall
    Inherits clsWTF

    'Fall Method Parms
    Private pGWLAsymptote As Double 'd, water level/pour point elevation where groundwater stop falling
    Public Property GWLAsymptote() As Double
        Get
            Return pGWLAsymptote
        End Get
        Set(ByVal value As Double)
            pGWLAsymptote = value
        End Set
    End Property

    'Kgw is the groundwater-level recession constant, which is negative; in units of (ln(length)/time)
    Private pKgwl As Double
    Public Property KGWL() As Double
        Get
            Return pKgwl
        End Get
        Set(ByVal value As Double)
            pKgwl = value
        End Set
    End Property

    Public ParametersSet As Boolean = False

    Public Function AntecedentGWL(ByVal aTimePeak As Double, _
                                  ByVal aTime0 As Double, _
                                  ByVal aGWLH0 As Double) As Double

        If ParametersSet Then
            Dim lGWLStartAboveD As Double = aGWLH0 - GWLAsymptote
            Dim lPower As Double = KGWL * (aTimePeak - aTime0)
            Dim lGWLAntecedent As Double = lGWLStartAboveD * Math.Pow(Math.E, lPower) + GWLAsymptote
            Return lGWLAntecedent
        Else
            Return -99
        End If
    End Function

    Public Overrides Function Recharge(ByVal aRiseObj As atcUSGSRecess.clsFall) As Boolean
        'MyBase.Recharge(aRiseObj)
        If Not ParametersSet Then Return False

        For Each lSeg As clsRecessionSegment In aRiseObj.listOfSegments
            With lSeg
                If .Flow Is Nothing Then
                    If Double.IsNaN(GWLAsymptote) Then
                        .GetData(True, GWLAsymptote)
                    End If
                End If
                Dim lAntGWL As Double = AntecedentGWL(.Flow.Length - 1, 1, .HzeroDayValue)
                If .AntecedentGWLMethods.Keys.Contains(AntecedentGWLMethod.FALL) Then
                    .AntecedentGWLMethods.ItemByKey(AntecedentGWLMethod.FALL) = lAntGWL
                Else
                    .AntecedentGWLMethods.Add(AntecedentGWLMethod.FALL, lAntGWL)
                End If

                Dim lDeltaH As Double = .Flow(.Flow.Length - 1) - lAntGWL 'feet
                If lDeltaH < 0 Then lDeltaH = 0
                Dim lRecharge As Double = SpecificYield * lDeltaH * 12 ' inch
                If .Recharges.Keys.Contains(AntecedentGWLMethod.FALL) Then
                    .Recharges.ItemByKey(AntecedentGWLMethod.FALL) = lRecharge
                Else
                    .Recharges.Add(AntecedentGWLMethod.FALL, lRecharge)
                End If
            End With
        Next
        Return True
    End Function

    Public Overrides Sub Clear()
        MyBase.Clear()
        GWLAsymptote = 0
        KGWL = 0
        ParametersSet = False
    End Sub
End Class
