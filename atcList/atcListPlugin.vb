Imports atcData

Public Class atcListPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::List"
        End Get
    End Property

    Public Overrides Function Show(ByVal aDataManager As atcData.atcDataManager, _
                     Optional ByVal aDataGroup As atcData.atcDataGroup = Nothing) As Object
        Dim lForm As New atcListForm
        lForm.Initialize(aDataManager, aDataGroup)
        Return lForm
    End Function

    Public Overrides Sub Save(ByVal aDataManager As atcData.atcDataManager, _
                          ByVal aDataGroup As atcData.atcDataGroup, _
                          ByVal aFileName As String, _
                          ByVal ParamArray aOption() As String)

        If Not aDataGroup Is Nothing AndAlso aDataGroup.Count > 0 Then
            Dim lForm As New atcListForm
            lForm.Initialize(aDataManager, aDataGroup)
            atcUtility.SaveFileString(aFileName, lForm.ToString)
            lForm.Dispose()
        End If
    End Sub

End Class
