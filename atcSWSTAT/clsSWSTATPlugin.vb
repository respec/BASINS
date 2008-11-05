Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcFrequencyGridPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::SWSTAT"
        End Get
    End Property

    Public Overrides Function Show(ByVal aDataGroup As atcData.atcDataGroup) As Object
        Dim lForm As New frmSWSTAT

        Dim pBasicAttributes As New ArrayList
        With pBasicAttributes
            .Add("ID")
            .Add("Min")
            .Add("Max")
            .Add("Mean")
            .Add("Standard Deviation")
            .Add("Count")
            .Add("Count Missing")
        End With

        Dim pNDayAttributes As New ArrayList
        With pNDayAttributes
            .Add("STAID")
            .Add("STANAM")
            .Add("Constituent")
        End With

        Dim pTrendAttributes As New ArrayList
        With pTrendAttributes
            .Add("Original ID")
            .Add("KENTAU")
            .Add("KENPLV")
            .Add("KENSLPL")
            .Add("Count")
            .Add("CountMissing")
            .Add("Min")
            .Add("Max")
            .Add("Constituent")
            .Add("STAID")
        End With

        lForm.Initialize(aDataGroup, pBasicAttributes, pNDayAttributes, pTrendAttributes)
        Return lForm
    End Function

    Public Overrides Sub Save(ByVal aDataGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)

        If Not aDataGroup Is Nothing AndAlso aDataGroup.Count > 0 Then
            Dim lForm As New frmSWSTAT

            lForm.Initialize(aDataGroup)
            atcUtility.SaveFileString(aFileName, lForm.ToString)
            lForm.Dispose()
        End If
    End Sub
End Class