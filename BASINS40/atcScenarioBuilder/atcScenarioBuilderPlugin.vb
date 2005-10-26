Imports atcData

Public Class atcScenarioBuilderPlugin
  Inherits atcDataDisplay
  Private pMapWin As MapWindow.Interfaces.IMapWin

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Tools::ClimateChangeScenario"
    End Get
  End Property

  Public Overrides Function Show(ByVal aManager As atcDataManager, _
                   Optional ByVal aGroup As atcDataGroup = Nothing)
    Dim lForm As New atcScenarioBuilderForm
    lForm.Initialize(aManager, pMapWin, aGroup)
    Return lForm
  End Function

  Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                  ByVal aParentHandle As Integer)
    pMapWin = aMapWin
    pMapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcScenarioBuilderPlugin")
  End Sub

  Public Overrides Sub Terminate()
    pMapWin.Plugins.BroadcastMessage("atcDataPlugin unloading atcScenarioBuilderPlugin")
  End Sub
End Class
