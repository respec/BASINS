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
            '.Add("History 1")

            .Add("Data Source")
            .Add("STAID")
            .Add("STANAM")
            .Add("Constituent")
            .Add("Id")
            .Add("Time step")
            .Add("Time unit")
            .Add("Min")
            .Add("Max")
            .Add("Mean")
            .Add("Standard Deviation")
            .Add("Count")
            .Add("CountMissing")
        End With

        Dim pNDayAttributes As New ArrayList
        With pNDayAttributes
            '.Add("History 1")
            .Add("Data Source")
            .Add("STAID")
            .Add("STANAM")
            .Add("Constituent")
            .Add("Id")
            .Add("Time steps")
            .Add("Time units")
            .Add("Count")
            .Add("CountMissing")
        End With

        Dim pTrendAttributes As New ArrayList
        With pTrendAttributes
            '.Add("History 1")
            .Add("Data Source")
            .Add("STAID")
            .Add("STANAM")
            .Add("Constituent")
            .Add("Id")
            .Add("Time step")
            .Add("Time unit")
            .Add("Count")
            .Add("CountMissing")
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