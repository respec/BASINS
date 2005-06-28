Imports atcData

Public Class atcDebugTimserPlugin
  Inherits atcTimeseriesDisplay
  Public g_MapWin As MapWindow.Interfaces.IMapWin

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "DebugTimser"
    End Get
  End Property

  Public Overrides Sub Show(ByVal aTimeseriesManager As atcData.atcDataManager, _
                   Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing)
    Dim lForm As New atcDebugTimserForm(aTimeseriesManager, aTimeseriesGroup)
  End Sub

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    g_MapWin = MapWin
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcDebugTimserPlugin")
  End Sub

  Public Overrides Sub Terminate()
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin unloading atcDebugTimserPlugin")
  End Sub
End Class
