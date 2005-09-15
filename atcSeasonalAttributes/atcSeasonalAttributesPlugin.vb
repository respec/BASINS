Imports atcUtility
Imports atcData

Public Class atcSeasonalAttributesPlugin
  Inherits atcDataDisplay

  Public Overrides ReadOnly Property Name() As String
    Get
      Return "Tools::SeasonalAttributes"
    End Get
  End Property

  Public Overrides Function Show(ByVal aDataManager As atcData.atcDataManager, _
                   Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing)
    Dim lDataGroup As atcDataGroup = aDataGroup
    If lDataGroup Is Nothing Then
      lDataGroup = New atcDataGroup
    End If

    Dim lForm As New frmDisplaySeasonalAttributes(aDataManager, lDataGroup)
    If Not (lDataGroup Is Nothing) AndAlso lDataGroup.Count > 0 Then
      lForm.Show()
    End If
  End Function

  Public Overrides Sub Save(ByVal aDataManager As atcDataManager, _
                            ByVal aDataGroup As atcDataGroup, _
                            ByVal aFileName As String, _
                            ByVal ParamArray aOption() As String)
    Dim lForm As New frmDisplaySeasonalAttributes(aDataManager, aDataGroup)
    SaveFileString(aFileName, lForm.agdMain.ToString)
  End Sub

  Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                  ByVal aParentHandle As Integer)
    aMapWin.Plugins.BroadcastMessage("atcDataPlugin loading atcSeasonalAttributesPlugin")
  End Sub
End Class
