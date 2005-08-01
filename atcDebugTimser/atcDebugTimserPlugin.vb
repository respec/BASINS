Imports atcData

Public Class atcDebugTimserPlugin
  Inherits atcDataDisplay
  Public g_MapWin As MapWindow.Interfaces.IMapWin

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "DebugTimser"
    End Get
  End Property

  Public Overrides Function Show(ByVal aDataManager As atcData.atcDataManager, _
                   Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing) As Object
    Return New atcDebugTimserForm(aDataManager, aDataGroup)
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    g_MapWin = MapWin
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcDebugTimserPlugin")
  End Sub

  Public Overrides Sub Terminate()
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin unloading atcDebugTimserPlugin")
  End Sub
End Class
