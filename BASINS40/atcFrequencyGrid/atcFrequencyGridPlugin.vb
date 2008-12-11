Imports atcUtility
Imports atcData

Public Class atcFrequencyGridPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Frequency Grid"
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
        Dim lTimeseriesGroup As atcTimeseriesGroup = aTimeseriesGroup
        If lTimeseriesGroup Is Nothing Then
            lTimeseriesGroup = New atcTimeseriesGroup
        End If

        Dim lForm As New frmDisplayFrequencyGrid(lTimeseriesGroup)
        If Not (lTimeseriesGroup Is Nothing) AndAlso lTimeseriesGroup.Count > 0 AndAlso Not lForm.IsDisposed Then
            Return lForm
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOptions() As String)

        Dim lForm As New frmDisplayFrequencyGrid(aTimeseriesGroup)

        For Each lOption As String In aOptions
            Select Case lOption
                Case "SwapRowsColumns"
                    lForm.SwapRowsColumns = True
                Case "Low"
                    lForm.HighDisplay = False
                Case "High"
                    lForm.HighDisplay = True
            End Select
        Next

        SaveFileString(aFileName, lForm.ToString)
    End Sub

End Class
