Imports atcData

Public Class atcScenarioBuilderPlugin
  Inherits atcDataDisplay
  Public g_MapWin As MapWindow.Interfaces.IMapWin

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Tools::Scenario"
    End Get
  End Property

  Public Overrides Function Show(ByVal aManager As atcData.atcDataManager, _
                   Optional ByVal aGroup As atcData.atcDataGroup = Nothing)
    Dim lForm As New atcScenarioBuilderForm
    lForm.Initialize(aManager, aGroup)
    Return lForm
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    g_MapWin = MapWin
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcScenarioBuilderPlugin")
  End Sub

  Public Overrides Sub Terminate()
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin unloading atcScenarioBuilderPlugin")
  End Sub
End Class
