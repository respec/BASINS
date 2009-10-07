Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class clsSWSTATPlugin
    Inherits atcData.atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::SWSTAT"
        End Get
    End Property

    Public Overrides ReadOnly Property Icon() As System.Drawing.Icon
        Get
            Dim lResources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSWSTAT))
            Return CType(lResources.GetObject("$this.Icon"), System.Drawing.Icon)
        End Get
    End Property

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
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

        lForm.Initialize(aTimeseriesGroup, pBasicAttributes, pNDayAttributes, pTrendAttributes)
        Return lForm
    End Function

    Public Overrides Sub Save(ByVal aTimeseriesGroup As atcData.atcDataGroup, _
                              ByVal aFileName As String, _
                              ByVal ParamArray aOption() As String)

        If Not aTimeseriesGroup Is Nothing AndAlso aTimeseriesGroup.Count > 0 Then
            Dim lForm As New frmSWSTAT

            lForm.Initialize(aTimeseriesGroup)
            atcUtility.SaveFileString(aFileName, lForm.ToString)
            lForm.Dispose()
        End If
    End Sub

    Public Shared Function ComputeRankedAnnualTimeseries(ByVal aTimeseriesGroup As atcTimeseriesGroup, _
                                                         ByVal aNDay() As Double, _
                                                         ByVal aHighFlag As Boolean, _
                                                         ByVal aFirstYear As Integer, _
                                                         ByVal aLastYear As Integer, _
                                                         ByVal aBoundaryMonth As Integer, _
                                                         ByVal aBoundaryDay As Integer, _
                                                         ByVal aEndMonth As Integer, _
                                                         ByVal aEndDay As Integer) As atcTimeseriesGroup
        Dim lArgs As New atcDataAttributes
        lArgs.SetValue("Timeseries", aTimeseriesGroup)

        lArgs.SetValue("NDay", aNDay)
        lArgs.SetValue("HighFlag", aHighFlag)

        lArgs.SetValue("FirstYear", aFirstYear)
        lArgs.SetValue("LastYear", aLastYear)

        lArgs.SetValue("BoundaryMonth", aBoundaryMonth)
        lArgs.SetValue("BoundaryDay", aBoundaryDay)

        lArgs.SetValue("EndMonth", aEndMonth)
        lArgs.SetValue("EndDay", aEndDay)

        Dim lHighLow As String = "low"
        If aHighFlag Then
            lHighLow = "high"
        End If

        Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
        If lCalculator.Open("n-day " & lHighLow & " timeseries", lArgs) Then
            For Each lDataset As atcTimeseries In lCalculator.DataSets
                ComputeRanks(lDataset, Not aHighFlag, False)
            Next
        End If
        Return lCalculator.DataSets
    End Function
End Class