Imports atcData

Public Class atcListPlugin
  Inherits atcDataDisplay

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

  Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                  ByVal ParentHandle As Integer)
    aMapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcListPlugin")
  End Sub
End Class
