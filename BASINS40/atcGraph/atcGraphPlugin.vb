Imports atcData

Public Class atcGraphPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Graph"
        End Get
    End Property

    Public Overrides Sub Save(ByVal aDataGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)

        If Not aDataGroup Is Nothing AndAlso aDataGroup.Count > 0 Then
            Dim lForm As New atcGraphForm(aDataGroup)
            lForm.SaveBitmapToFile(aFileName)
            lForm.Dispose()
        End If
    End Sub

    Public Overrides Function Show(Optional ByVal aDataGroup As atcDataGroup = Nothing) As Object
        Dim lDataGroup As atcDataGroup = aDataGroup
        If lDataGroup Is Nothing Then lDataGroup = New atcDataGroup

        Dim lForm As New atcGraphForm(lDataGroup)
        If Not (lDataGroup Is Nothing) AndAlso lDataGroup.Count > 0 Then
            lForm.Show()
            Return lForm
        Else 'No data to display, don't show or return the form
            lForm.Dispose()
            Return Nothing
        End If
    End Function

End Class
