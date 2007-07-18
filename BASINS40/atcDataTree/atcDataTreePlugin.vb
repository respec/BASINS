Imports atcData

Public Class atcDataTreePlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Data Tree"
        End Get
    End Property

    Public Overrides Function Show(Optional ByVal aDataGroup As atcDataGroup = Nothing) _
                     As Object 'System.Windows.Forms.Form
        Dim lDataGroup As atcDataGroup = aDataGroup

        If lDataGroup Is Nothing Then
            lDataGroup = New atcDataGroup
        End If

        Dim lForm As New atcDataTreeForm(lDataGroup)
        If Not (lDataGroup Is Nothing) AndAlso lDataGroup.Count > 0 Then
            lForm.Show()
            Return lForm
        Else 'No data to display, don't show or return the form
            lForm.Dispose()
            Return Nothing
        End If
    End Function

    Public Overrides Sub Save(ByVal aDataGroup As atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)
        Dim lForm As New atcDataTreeForm(aDataGroup)
        With lForm
            .TreeAction(aOption)
            .Save(aFileName)
        End With
    End Sub
End Class
