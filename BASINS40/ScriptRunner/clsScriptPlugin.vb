Public Class ScriptPlugin
    Inherits atcData.atcDataPlugin

    Private ToolButtonName As String = "Test"

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "ScriptPlugin"
        End Get
    End Property

    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
        Dim lScriptNames As Generic.List(Of String) = ScriptNames()
        If lScriptNames.Count = 1 Then
            ToolButtonName = lScriptNames(0)
        End If
        pMapWin.Toolbar.AddButton(ToolButtonName)
    End Sub

    Public Overrides Sub Terminate()
        MyBase.Terminate()
        pMapWin.Toolbar.RemoveButton(ToolButtonName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        MyBase.ItemClicked(aItemName, aHandled)
        If aItemName = ToolButtonName Then
            ScriptDriver.Main(pMapWin)
        End If
    End Sub

End Class
