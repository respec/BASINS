Imports atcData

Public Class atcDataTreePlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Data Tree"
        End Get
    End Property

    Public Overrides Function Show(ByVal aDataGroup As atcDataGroup) _
                     As Object 'System.Windows.Forms.Form
        Dim lDataGroup As atcDataGroup = aDataGroup

        'creating an instance of the form asks user to specify some Data if none has been passed in
        Dim lDataTreeForm As New atcDataTreeForm(lDataGroup)

        If Not (lDataGroup Is Nothing) AndAlso lDataGroup.Count > 0 Then
            lDataTreeForm.Show()
            Return lDataTreeForm
        Else 'No data to display, don't show or return the form
            lDataTreeForm.Dispose()
            Return Nothing
        End If
    End Function

    Public Overrides Sub Save(ByVal aDataGroup As atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)
        Dim lDataTreeForm As New atcDataTreeForm(aDataGroup)
        With lDataTreeForm
            .TreeAction(aOption)
            .Save(aFileName)
        End With
    End Sub
End Class
