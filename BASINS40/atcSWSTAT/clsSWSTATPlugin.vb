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
            .Add("CountMissing")
        End With

        Dim pNDayAttributes As New ArrayList
        With pNDayAttributes
            '.Add("Data Source")
            .Add("STAID")
            .Add("STANAM")
            .Add("Constituent")
            '.Add("ID")
            '.Add("Time steps")
            '.Add("Time units")
            '.Add("Count")
            '.Add("CountMissing")
        End With

        Dim pTrendAttributes As New ArrayList
        With pTrendAttributes
            .Add("Original ID")
            .Add("KENTAU")
            .Add("KENPLV")
            .Add("KENSLPL")
            '.Add("YearFrom")
            '.Add("YearTo")
            .Add("Count")
            .Add("CountMissing")
            '.Add("Non-zeroReturnsCode")
            '.Add("Non-zeroReturnsNO.")
            .Add("Min")
            .Add("Max")
            '.Add("Qualifiers")
            .Add("Constituent")
            .Add("STAID")
            '.Add("STANAM")
            '.Add("Time step")
            '.Add("Time unit")
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