Imports atcData

Public Class atcListPlugin
  Inherits atcDataDisplay
  Public g_MapWin As MapWindow.Interfaces.IMapWin

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Tools::List"
    End Get
  End Property

  Public Overrides Function Show(ByVal aManager As atcData.atcDataManager, _
                   Optional ByVal aGroup As atcData.atcDataGroup = Nothing)
    Dim lForm As New atcListForm
    lForm.Initialize(aManager, aGroup)
    Return lForm
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    g_MapWin = MapWin
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcListPlugin")
  End Sub

  Public Overrides Sub Terminate()
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin unloading atcListPlugin")
  End Sub
End Class
