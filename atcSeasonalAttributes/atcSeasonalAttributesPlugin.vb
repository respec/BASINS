Imports atcData

Public Class atcSeasonalAttributesPlugin
  Inherits atcDataDisplay
  Private pMapWin As MapWindow.Interfaces.IMapWin

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Tools::SeasonalAttributes"
    End Get
  End Property

  Public Overrides Function Show(ByVal aManager As atcData.atcDataManager, _
                   Optional ByVal aGroup As atcData.atcDataGroup = Nothing)
    Dim lForm As New frmDisplaySeasonalAttributes
    lForm.Initialize(aManager, aGroup)
    Return lForm
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    pMapWin = MapWin
    pMapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcSeasonalAttributesPlugin")
  End Sub

  Public Overrides Sub Terminate()
    pMapWin.Plugins.BroadcastMessage("atcDataPlugin unloading atcSeasonalAttributesPlugin")
  End Sub
End Class
