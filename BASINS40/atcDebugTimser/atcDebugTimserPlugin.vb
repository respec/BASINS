Imports atcData

Public Class atcDebugTimserPlugin
  Inherits atcDataDisplay
  Public g_MapWin As MapWindow.Interfaces.IMapWin

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Tools::DebugTimser"
    End Get
  End Property

  Public Overrides Function Show(ByVal aDataManager As atcData.atcDataManager, _
                   Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing) As Object
    Dim lDataGroup As atcDataGroup = aDataGroup
    If lDataGroup Is Nothing Then lDataGroup = New atcDataGroup
    Dim lDebugTimserForm As New atcDebugTimserForm(aDataManager, lDataGroup)

    If Not (lDataGroup Is Nothing) AndAlso lDataGroup.Count > 0 Then lDebugTimserForm.Show()
    Return lDebugTimserForm
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
    g_MapWin = MapWin
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcDebugTimserPlugin")
  End Sub

  Public Overrides Sub Terminate()
    g_MapWin.Plugins.BroadcastMessage("atcDataPlugin unloading atcDebugTimserPlugin")
  End Sub

  Public Sub Save(ByVal aDataManager As atcData.atcDataManager, _
                  ByVal aDataGroup As atcData.atcDataGroup, _
                  ByVal aFileName As String, _
                  ByVal ParamArray aOption() As String)
    Dim lDebugTimserForm As New atcDebugTimserForm(aDataManager, aDataGroup)
    With lDebugTimserForm
      .TreeAction(aOption)
      .Save(aFileName)
    End With
  End Sub
End Class
