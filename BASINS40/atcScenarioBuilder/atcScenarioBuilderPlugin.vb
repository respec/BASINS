Imports atcData

Public Class atcScenarioBuilderPlugin
  Inherits atcData.atcDataDisplay

  Public Overrides ReadOnly Property Name() As String
    Get
            Return "Analysis::Climate Assessment Tool"
    End Get
  End Property

  Public Overrides Function Show(ByVal aManager As atcDataManager, _
                   Optional ByVal aGroup As atcDataGroup = Nothing) As Object
    g_DataManager = aManager
    Dim lForm As New frmCAT
    lForm.Initialize(aGroup)
    Return lForm
  End Function

  Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                  ByVal aParentHandle As Integer)
    MyBase.Initialize(aMapWin, aParentHandle)
    g_MapWin = aMapWin
  End Sub

End Class
