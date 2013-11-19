Imports atcData

Public Class atcDataTreePlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Data Tree"
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcDataGroup) _
                     As Object 'System.Windows.Forms.Form
        Dim lIcon As System.Drawing.Icon = Nothing
        Return Show(aTimeseriesGroup, lIcon)
    End Function

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcDataGroup, ByVal aIcon As System.Drawing.Icon) As Object
        Dim lTimeseriesGroup As atcTimeseriesGroup = aTimeseriesGroup

        'creating an instance of the form asks user to specify some Data if none has been passed in
        Dim lDataTreeForm As New atcDataTreeForm()
        lDataTreeForm.Initialize(lTimeseriesGroup, aIcon)

        If Not (lTimeseriesGroup Is Nothing) AndAlso lTimeseriesGroup.Count > 0 Then
            lDataTreeForm.Show()
            Return lDataTreeForm
        Else 'No data to display, don't show or return the form
            lDataTreeForm.Dispose()
            Return Nothing
        End If
    End Function

    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)
        Dim lDataTreeForm As New atcDataTreeForm()
        With lDataTreeForm
            .Initialize(aTimeseriesGroup)
            .TreeAction(aOption)
            .Save(aFileName)
        End With
    End Sub
End Class
