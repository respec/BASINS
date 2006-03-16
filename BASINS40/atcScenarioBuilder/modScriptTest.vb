Imports atcData

Module modScriptTest

  Public Function BuiltInVariationScript(ByVal aVariationTemplate As Variation) As Variation
    Dim lNewVariation As New LogVariation
    lNewVariation.XML = aVariationTemplate.XML
    Return lNewVariation
  End Function

  Private Class LogVariation
    Inherits Variation

    Private pIncrementsSinceStart As Integer = 0

    Public Overrides ReadOnly Property Iterations() As Integer
      Get
        Try
          Return (Max - Min) / Increment + 1
        Catch ex As Exception
          Return 1
        End Try
      End Get
    End Property

    Public Overrides Function StartIteration() As atcDataGroup
      Me.CurrentValue = Me.Min
      pIncrementsSinceStart = 0
      Return VaryData()
    End Function

    Public Overrides Function NextIteration() As atcDataGroup
      pIncrementsSinceStart += 1
      If pIncrementsSinceStart < Iterations Then
        Me.CurrentValue = Me.Min + Me.Increment * pIncrementsSinceStart
        Return VaryData()
      Else
        Return Nothing
      End If
    End Function

  End Class
End Module
