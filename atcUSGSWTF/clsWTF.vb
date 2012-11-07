Imports atcUSGSRecess

Public Class clsWTF

    Private pSpecificYield As Double
    Public Property SpecificYield() As Double
        Get
            Return pSpecificYield
        End Get
        Set(ByVal value As Double)
            pSpecificYield = value
        End Set
    End Property

    Public Overridable Sub EstimateParameters(ByVal aFallObj As clsFall)
    End Sub

    Public Overridable Function Recharge(ByVal aRiseObj As clsFall) As Boolean
    End Function

End Class

Public Enum AntecedentGWLMethod
    FALL = 1
    Linear = 2
    Power = 3
End Enum