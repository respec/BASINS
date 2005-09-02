Imports atcData

Public Class atcGraphPlugin
  Inherits atcDataDisplay
  Private pMapWin As MapWindow.Interfaces.IMapWin

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Tools::Graph"
    End Get
  End Property

  Public Sub Save(ByVal aDataManager As atcData.atcDataManager, _
                  ByVal aDataGroup As atcData.atcDataGroup, _
                  ByVal aFileName As String, _
                  ByVal ParamArray aOption() As String)

    If Not aDataGroup Is Nothing AndAlso aDataGroup.Count > 0 Then
      Dim lForm As New atcGraphForm(aDataManager, aDataGroup)
      lForm.SaveBitmapToFile(aFileName)
      lForm.Dispose()
    End If
  End Sub

  Public Overrides Function Show(ByVal aDataManager As atcDataManager, _
                        Optional ByVal aDataGroup As atcDataGroup = Nothing) As Object 'System.Windows.Forms.Form
    Dim lDataGroup As atcDataGroup = aDataGroup
    If lDataGroup Is Nothing Then lDataGroup = New atcDataGroup

    Dim lForm As New atcGraphForm(aDataManager, lDataGroup)
    If lDataGroup.Count > 0 Then
      lForm.Show()
      Return lForm
    Else 'No data to display, don't show or return the form
      lForm.Dispose()
      Return Nothing
    End If
  End Function

  Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                  ByVal aParentHandle As Integer)
    pMapWin = aMapWin
    pMapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcGraphPlugin")
  End Sub

  Public Overrides Sub Terminate()
    pMapWin.Plugins.BroadcastMessage("atcDataPlugin unloading atcGraphPlugin")
  End Sub
End Class
