Imports atcData

Public Class atcGraphPlugin
  Inherits atcDataDisplay
  Public g_MapWin As MapWindow.Interfaces.IMapWin

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Graph"
    End Get
  End Property

  Public Overrides function Show(ByVal aTimeseriesManager As atcData.atcDataManager, _
                   Optional ByVal aTimeseriesGroup As atcData.atcDataGroup = Nothing)
    Return New atcGraphForm(aTimeseriesManager, aTimeseriesGroup)
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    g_MapWin = MapWin
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcGraphPlugin")
  End Sub

  Public Overrides Sub Terminate()
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin unloading atcGraphPlugin")
  End Sub
End Class
