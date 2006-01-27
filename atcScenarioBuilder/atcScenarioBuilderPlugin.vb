Imports atcData

Public Class atcScenarioBuilderPlugin
  Inherits atcData.atcDataDisplay

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Tools::ClimateChangeScenario"
    End Get
  End Property

  Public Overrides Function Show(ByVal aManager As atcDataManager, _
                   Optional ByVal aGroup As atcDataGroup = Nothing) As Object
    Dim lForm As New frmMultipleResults 'atcScenarioBuilderForm
    lForm.Initialize(aManager, aGroup)
    Return lForm
  End Function

  Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                  ByVal aParentHandle As Integer)
    g_MapWin = aMapWin
    g_MapWin.Plugins.BroadcastMessage("loading atcScenarioBuilderPlugin")
  End Sub

  Public Overrides Sub Terminate()
    g_MapWin.Plugins.BroadcastMessage("unloading atcScenarioBuilderPlugin")
  End Sub
End Class
