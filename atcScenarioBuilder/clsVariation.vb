Imports atcData

Public Class Variation

  Public Name As String = "<untitled>"
  Public DataSets As atcDataGroup
  Public ComputationSource As atcDataSource
  Public Operation As String = ""
  Public Min As Double = Double.NaN
  Public Max As Double = Double.NaN
  Public Increment As Double = Double.NaN

  Public ColorBelowMin As System.Drawing.Color = System.Drawing.Color.DeepSkyBlue
  Public ColorInRange As System.Drawing.Color = System.Drawing.Color.White
  Public ColorAboveMax As System.Drawing.Color = System.Drawing.Color.OrangeRed

  Public Overrides Function ToString() As String
    Dim retStr As String = Name & " " & Operation
    If Not Double.IsNaN(Min) Then retStr &= " from " & Format(Min, "0.0")
    If Not Double.IsNaN(Max) Then retStr &= " to " & Format(Max, "0.0")
    If Not Double.IsNaN(Increment) Then retStr &= " step " & Format(Increment, "0.0")
    Return retStr
  End Function
End Class
