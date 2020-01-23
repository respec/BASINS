Imports atcUtility
Imports atcData

Public Class atcSeasonalAttributesPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
#If Toolbox = "Hydro" Then
            Return "Analysis::Attributes"
#Else
            Return "Analysis::Seasonal Attributes"
#End If
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
        Dim lIcon As System.Drawing.Icon = Nothing
        Return Show(aTimeseriesGroup, lIcon)
    End Function

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcDataGroup, ByVal aIcon As System.Drawing.Icon) As Object
        Dim lTimeseriesGroup As atcTimeseriesGroup = aTimeseriesGroup
        If lTimeseriesGroup Is Nothing Then
            lTimeseriesGroup = New atcTimeseriesGroup
        End If

        Dim lForm As New frmDisplaySeasonalAttributes()
        lForm.Initialize(aTimeseriesGroup, aIcon)

        If Not (lTimeseriesGroup Is Nothing) AndAlso lTimeseriesGroup.Count > 0 Then
            lForm.Show()
            Return lForm
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOptions() As String)

        Dim lForm As New frmDisplaySeasonalAttributes()
        lForm.Initialize(aTimeseriesGroup, Nothing)

        For Each lOption As String In aOptions
            Select Case lOption
                Case "SwapRowsColumns"
                    lForm.SwapRowsColumns = True
            End Select
        Next

        SaveFileString(aFileName, lForm.ToString)
    End Sub

End Class
