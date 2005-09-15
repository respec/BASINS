Imports atcData

Public Class atcDebugTimserPlugin
  Inherits atcDataDisplay

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Tools::DebugTimser"
    End Get
  End Property

  Public Overrides Function Show(ByVal aDataManager As atcDataManager, _
                   Optional ByVal aDataGroup As atcDataGroup = Nothing) As Object
    Dim lDataGroup As atcDataGroup = aDataGroup
    If lDataGroup Is Nothing Then
      lDataGroup = New atcDataGroup
    End If

    Dim lDebugTimserForm As New atcDebugTimserForm(aDataManager, lDataGroup)
    If Not (lDataGroup Is Nothing) AndAlso lDataGroup.Count > 0 Then
      lDebugTimserForm.Show()
    End If

    Return lDebugTimserForm
  End Function

  Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                  ByVal aParentHandle As Integer)
    aMapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcDebugTimserPlugin")
  End Sub

  Public Overrides Sub Save(ByVal aDataManager As atcDataManager, _
                  ByVal aDataGroup As atcDataGroup, _
                  ByVal aFileName As String, _
                  ByVal ParamArray aOption() As String)
    Dim lDebugTimserForm As New atcDebugTimserForm(aDataManager, aDataGroup)
    With lDebugTimserForm
      .TreeAction(aOption)
      .Save(aFileName)
    End With
  End Sub

End Class
