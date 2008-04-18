Imports atcUtility
Imports atcData

Public Class atcSeasonalAttributesPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Seasonal Attributes"
        End Get
    End Property

    Public Overrides Function Show(ByVal aDataGroup As atcData.atcDataGroup) As Object
        Dim lDataGroup As atcDataGroup = aDataGroup
        If lDataGroup Is Nothing Then
            lDataGroup = New atcDataGroup
        End If

        Dim lForm As New frmDisplaySeasonalAttributes(lDataGroup)
        If Not (lDataGroup Is Nothing) AndAlso lDataGroup.Count > 0 Then
            lForm.Show()
            Return lForm
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Sub Save(ByVal aDataGroup As atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOptions() As String)

        Dim lForm As New frmDisplaySeasonalAttributes(aDataGroup)

        For Each lOption As String In aOptions
            Select Case lOption
                Case "SwapRowsColumns"
                    lForm.SwapRowsColumns = True
            End Select
        Next

        SaveFileString(aFileName, lForm.ToString)
    End Sub

End Class
