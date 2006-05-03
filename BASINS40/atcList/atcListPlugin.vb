Imports atcData

Public Class atcListPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::List"
        End Get
    End Property

    Public Overrides Function Show(ByVal aManager As atcData.atcDataManager, _
                     Optional ByVal aGroup As atcData.atcDataGroup = Nothing) As Object
        Dim lForm As New atcListForm
        lForm.Initialize(aManager, aGroup)
        Return lForm
    End Function

End Class
