Imports atcData

Public Class atcGraphPlugin
  Inherits atcTimeseriesDisplay

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Graph"
    End Get
  End Property

  Public Overrides Sub Show(ByVal aTimeseriesManager As atcData.atcTimeseriesManager, _
                   Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing)
    Dim gForm As New atcGraphForm(aTimeseriesManager, aTimeseriesGroup)
    gForm.Show()
  End Sub

End Class
